using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class TaskTrueFalseService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly TaskService TaskMan;
        private readonly ClaimHelper ClaimMan;

        public TaskTrueFalseService(TalentContext talentContext, IFileStorageService fileService, TaskService taskService, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this.FileMan = fileService;
            this.TaskMan = taskService;
            this.ClaimMan = claimHelper;
        }

        /// <summary>
        /// Get True False Task By Id for View Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TrueFalseTypeViewDetails> GetTaskTruFalseTypeById(int id)
        {
            var searchTask = await (from t in DB.Tasks

                                    join b in DB.Blobs on t.BlobId equals b.BlobId into btm
                                    from b in btm.DefaultIfEmpty()

                                    join ttft in DB.TaskTrueFalseTypes on t.TaskId equals ttft.TaskId
                                    join m in DB.Modules on t.ModuleId equals m.ModuleId
                                    join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                                    join et in DB.EvaluationTypes on t.EvaluationTypeId equals et.EvaluationTypeId
                                    join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                                    join tp in DB.TimePoints on ttft.Score equals tp.TimePointId
                                    where tp.PointTypeId == 5 && t.TaskId == id && t.QuestionTypeId == 1 && t.IsDeleted == false
                                    select new TrueFalseTypeViewDetails
                                    {
                                        LayoutType = t.LayoutType,
                                        BlobId = t.BlobId,
                                        StoryLineDescription = t.StoryLineDescription,
                                        CompetencyTypeName = ct.CompetencyTypeName,
                                        PrefixCode = c.PrefixCode,
                                        EvaluationTypeName = et.EvaluationTypeName,
                                        TaskNumber = t.TaskNumber,
                                        ModuleName = m.ModuleName,
                                        Question = ttft.Question,
                                        Answer = ttft.Answer,
                                        Name = b.Name,
                                        Mime = b.Mime,
                                        ModuleId = m.ModuleId,
                                        TimePoint = new TimePointTaskModel
                                        {
                                            Score = tp.Score,
                                            TimePointId = tp.TimePointId,
                                            Points = tp.Points,
                                        },
                                        CompetencyId = c.CompetencyId,
                                        CompetencyTypeId = c.CompetencyTypeId,
                                        EvaluationTypeId = et.EvaluationTypeId
                                    }).AsNoTracking().FirstOrDefaultAsync();

            if (searchTask == null)
            {
                return new TrueFalseTypeViewDetails();
            }

            return searchTask;
        }

        /// <summary>
        /// Insert True False Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertTrueFalse(TaskTrueFalseFormModel model)
        {
            var taskNumber = await this.TaskMan.GetNumber(new GetNumberTaskModel
            {
                CompetencyId = model.CompetencyId,
                EvaluationTypeId = model.EvaluationTypeId,
                ModuleId = model.ModuleId
            });

            var userLogin = this.ClaimMan.GetLoginUserId();

            Guid? getUploadBlob = null;
            if (model.FileContent != null)
            {
                getUploadBlob = await FileMan.UploadFileFromBase64(model.FileContent);
            }

            var insertTask = new Talent.Entities.Entities.Tasks
            {
                CompetencyId = model.CompetencyId,
                BlobId = getUploadBlob,
                //BlobId = model.BlobId,
                EvaluationTypeId = model.EvaluationTypeId,
                QuestionTypeId = 1,
                ModuleId = model.ModuleId,
                TaskNumber = taskNumber,
                LayoutType = model.LayoutType.GetValueOrDefault(),
                StoryLineDescription = model.StoryLineDescription,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedBy = userLogin
            };

            this.DB.Tasks.Add(insertTask);
            this.DB.SaveChanges();

            var insertTrueFalse = new TaskTrueFalseTypes
            {
                TaskId = insertTask.TaskId,
                Question = model.Question,
                Answer = model.Answer,
                Score = model.TimePoint.TimePointId
            };

            this.DB.TaskTrueFalseTypes.Add(insertTrueFalse);
            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Update Task True False
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTaskTrueFalse(TaskTrueFalseFormModel model)
        {
            var findTask = await this.DB.Tasks.Where(Q => Q.TaskId == model.TaskId && Q.QuestionTypeId == 1 && Q.IsDeleted == false).FirstOrDefaultAsync();

            if (findTask == null)
            {
                return false;
            }

            //Checking if Task need to be re-inserted
            var toBeUpdated = findTask.ModuleId != model.ModuleId || findTask.CompetencyId != model.CompetencyId || findTask.EvaluationTypeId != model.EvaluationTypeId;
            var userLogin = this.ClaimMan.GetLoginUserId();

            if (toBeUpdated == true)
            {
                findTask.IsDeleted = true;
                findTask.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                findTask.UpdatedBy = userLogin;

                //Insert new Task

                var taskNumber = await this.TaskMan.GetNumber(new GetNumberTaskModel
                {
                    CompetencyId = model.CompetencyId,
                    EvaluationTypeId = model.EvaluationTypeId,
                    ModuleId = model.ModuleId
                });

                Guid? getUploadBlobUpdate = null;

                if (findTask.BlobId == model.BlobId && findTask.BlobId != null)
                {
                    getUploadBlobUpdate = model.BlobId;
                }

                if (model.FileContent != null)
                {
                    if (model.BlobId != null)
                    {
                        await FileMan.DeleteFileAsync(model.BlobId.GetValueOrDefault(), model.FileContent.ContentType);
                    }
                    getUploadBlobUpdate = await FileMan.UploadFileFromBase64(model.FileContent);
                }

                var insertTask = new Talent.Entities.Entities.Tasks
                {
                    CompetencyId = model.CompetencyId,
                    //BlobId = model.BlobId,
                    BlobId = getUploadBlobUpdate,
                    EvaluationTypeId = model.EvaluationTypeId,
                    QuestionTypeId = 1,
                    ModuleId = model.ModuleId,
                    TaskNumber = taskNumber,
                    LayoutType = model.LayoutType.GetValueOrDefault(),
                    StoryLineDescription = model.StoryLineDescription,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = userLogin,
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedBy = userLogin
                };

                this.DB.Tasks.Add(insertTask);
                this.DB.SaveChanges();

                var insertTrueFalse = new TaskTrueFalseTypes
                {
                    TaskId = insertTask.TaskId,
                    Question = model.Question,
                    Answer = model.Answer,
                    Score = model.TimePoint.TimePointId
                };

                this.DB.TaskTrueFalseTypes.Add(insertTrueFalse);

                await this.DB.SaveChangesAsync();
            }
            else
            {
                //if (findTask.BlobId != null && findTask.BlobId != model.BlobId)
                //{
                //    ////Remove blob nya
                //    //var BlobId = await this.DB.Blobs.Where(Q => Q.BlobId == findTask.BlobId).FirstOrDefaultAsync();

                //    //await this.FileMan.DeleteFileAsync(BlobId.BlobId, BlobId.Mime);

                //    //this.DB.Blobs.Remove(BlobId);

                //}

                Guid? getUploadBlobUpdate = null;

                if (findTask.BlobId == model.BlobId && findTask.BlobId != null)
                {
                    getUploadBlobUpdate = model.BlobId;
                }

                if (model.FileContent != null)
                {
                    if (model.BlobId != null)
                    {
                        await FileMan.DeleteFileAsync(model.BlobId.GetValueOrDefault(), model.FileContent.ContentType);

                        var BlobId = await this.DB.Blobs.Where(Q => Q.BlobId == findTask.BlobId).FirstOrDefaultAsync();
                        this.DB.Blobs.Remove(BlobId);
                    }
                    getUploadBlobUpdate = await FileMan.UploadFileFromBase64(model.FileContent);
                }

                findTask.StoryLineDescription = model.StoryLineDescription;
                findTask.LayoutType = model.LayoutType.GetValueOrDefault();
                //findTask.BlobId = model.BlobId;
                findTask.BlobId = getUploadBlobUpdate;

                findTask.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                findTask.UpdatedBy = userLogin;

                var findTrueFalse = await this.DB.TaskTrueFalseTypes.Where(Q => Q.TaskId == model.TaskId).FirstOrDefaultAsync();
                findTrueFalse.Answer = model.Answer;
                findTrueFalse.Score = model.TimePoint.TimePointId;
                findTrueFalse.Question = model.Question;

                await this.DB.SaveChangesAsync();
            }

            return true;
        }
    }
}
