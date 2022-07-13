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
    public class TaskShortAnswerService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly TaskService TaskMan;
        private readonly ClaimHelper ClaimMan;

        public TaskShortAnswerService(TalentContext talentContext, IFileStorageService fileService, TaskService taskService, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this.FileMan = fileService;
            this.TaskMan = taskService;
            this.ClaimMan = claimHelper;
        }

        /// <summary>
        /// To Inser Short Answer Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertShortAnswer(TaskShortAnswerForm model)
        {

            var taskNumber = await this.TaskMan.GetNumber(new GetNumberTaskModel
            {
                CompetencyId = model.CompetencyId.GetValueOrDefault(),
                EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
                ModuleId = model.ModuleId.GetValueOrDefault()
            });
            var userLogin = this.ClaimMan.GetLoginUserId();

            Guid? getUploadBlob = null;
            if (model.FileContent != null)
            {
                getUploadBlob = await FileMan.UploadFileFromBase64(model.FileContent);
            }

            var insertTask = new Talent.Entities.Entities.Tasks
            {
                CompetencyId = model.CompetencyId.GetValueOrDefault(),
                //BlobId = model.BlobId,
                BlobId = getUploadBlob,
                EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
                QuestionTypeId = 6,
                ModuleId = model.ModuleId.GetValueOrDefault(),
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

            var insertShortAnswer = new TaskShortAnswerTypes
            {
                Question = model.Question,
                TaskId = insertTask.TaskId
            };

            this.DB.TaskShortAnswerTypes.Add(insertShortAnswer);
            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Get Task Short Answer Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskShortAnswerViewDetail> GetTaskShortAnswerDetail(int id)
        {
            var searchTask = await (from t in DB.Tasks

                                    join b in DB.Blobs on t.BlobId equals b.BlobId into btm
                                    from b in btm.DefaultIfEmpty()

                                    join tsat in DB.TaskShortAnswerTypes on t.TaskId equals tsat.TaskId
                                    join m in DB.Modules on t.ModuleId equals m.ModuleId
                                    join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                                    join et in DB.EvaluationTypes on t.EvaluationTypeId equals et.EvaluationTypeId
                                    join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                                    where t.TaskId == id && t.QuestionTypeId == 6 && t.IsDeleted == false
                                    select new TaskShortAnswerViewDetail
                                    {
                                        LayoutType = t.LayoutType,
                                        BlobId = t.BlobId,
                                        StoryLineDescription = t.StoryLineDescription,
                                        CompetencyTypeName = ct.CompetencyTypeName,
                                        PrefixCode = c.PrefixCode,
                                        EvaluationTypeName = et.EvaluationTypeName,
                                        TaskNumber = t.TaskNumber,
                                        ModuleName = m.ModuleName,
                                        Question = tsat.Question,
                                        Name = b.Name,
                                        Mime = b.Mime,
                                        ModuleId = m.ModuleId,
                                        CompetencyId = c.CompetencyId,
                                        CompetencyTypeId = c.CompetencyTypeId,
                                        EvaluationTypeId = et.EvaluationTypeId
                                    }).AsNoTracking().FirstOrDefaultAsync();

            if (searchTask == null)
            {
                return new TaskShortAnswerViewDetail();
            }

            return searchTask;
        }
        /// <summary>
        /// Update Task Short Answer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTaskShortAnswer(TaskShortAnswerForm model)
        {
            var findTask = await this.DB.Tasks.Where(Q => Q.TaskId == model.TaskId && Q.QuestionTypeId == 6 && Q.IsDeleted == false).FirstOrDefaultAsync();
            var userLogin = this.ClaimMan.GetLoginUserId();
            if (findTask == null)
            {
                return false;
            }
            //Chek if task need to re-inserted
            var toBeUpdated = findTask.ModuleId != model.ModuleId || findTask.CompetencyId != model.CompetencyId || findTask.EvaluationTypeId != model.EvaluationTypeId;

            if (toBeUpdated == true)
            {
                findTask.IsDeleted = true;
                findTask.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                findTask.UpdatedBy = userLogin;

                var taskNumber = await this.TaskMan.GetNumber(new GetNumberTaskModel
                {
                    CompetencyId = model.CompetencyId.GetValueOrDefault(),
                    EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
                    ModuleId = model.ModuleId.GetValueOrDefault()
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

                //Insert new Task
                var insertTask = new Talent.Entities.Entities.Tasks
                {
                    CompetencyId = model.CompetencyId.GetValueOrDefault(),
                    //BlobId = model.BlobId,
                    BlobId = getUploadBlobUpdate,
                    EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
                    QuestionTypeId = 6,
                    ModuleId = model.ModuleId.GetValueOrDefault(),
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

                var insertShortAnswer = new TaskShortAnswerTypes
                {
                    Question = model.Question,
                    TaskId = insertTask.TaskId
                };

                this.DB.TaskShortAnswerTypes.Add(insertShortAnswer);
                await this.DB.SaveChangesAsync();
            }
            else
            {
                //if (findTask.BlobId != null && findTask.BlobId != model.BlobId)
                //{
                //    //Remove blob nya
                //    var BlobId = await this.DB.Blobs.Where(Q => Q.BlobId == findTask.BlobId).FirstOrDefaultAsync();

                //    await this.FileMan.DeleteFileAsync(BlobId.BlobId, BlobId.Mime);

                //    this.DB.Blobs.Remove(BlobId);
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
                findTask.BlobId = getUploadBlobUpdate;

                findTask.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                var findShortAnswer = await this.DB.TaskShortAnswerTypes.Where(Q => Q.TaskId == model.TaskId).FirstOrDefaultAsync();
                findShortAnswer.Question = model.Question;

                await this.DB.SaveChangesAsync();
            }

            return true;
        }

    }
}
