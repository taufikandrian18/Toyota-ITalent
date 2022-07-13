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
    public class TaskUploadFileService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly TaskService TaskMan;
        private readonly ClaimHelper ClaimMan;

        public TaskUploadFileService(TalentContext talentContext, IFileStorageService fileService, TaskService taskService, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this.FileMan = fileService;
            this.TaskMan = taskService;
            this.ClaimMan = claimHelper;
        }

        /// <summary>
        /// Insert Task File Upload
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertTaskFileUpload(TaskFileUploadFormModel model)
        {

            var taskNumber = await this.TaskMan.GetNumber(new GetNumberTaskModel
            {
                CompetencyId = model.CompetencyId,
                EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
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
                EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
                QuestionTypeId = 11,
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

            var insertFileUpload = new TaskFileUploadTypes
            {
                Question = model.Question,
                TaskId = insertTask.TaskId
            };

            this.DB.TaskFileUploadTypes.Add(insertFileUpload);
            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Get Task File Upload Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskFileUploadViewDetail> GetTaskFileUploadDetail(int id)
        {
            var searchTask = await (from t in DB.Tasks

                                    join b in DB.Blobs on t.BlobId equals b.BlobId into btm
                                    from b in btm.DefaultIfEmpty()

                                    join tfut in DB.TaskFileUploadTypes on t.TaskId equals tfut.TaskId
                                    join m in DB.Modules on t.ModuleId equals m.ModuleId
                                    join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                                    join et in DB.EvaluationTypes on t.EvaluationTypeId equals et.EvaluationTypeId
                                    join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                                    where t.TaskId == id && t.QuestionTypeId == 11 && t.IsDeleted == false
                                    select new TaskFileUploadViewDetail
                                    {
                                        LayoutType = t.LayoutType,
                                        BlobId = t.BlobId,
                                        StoryLineDescription = t.StoryLineDescription,
                                        CompetencyTypeName = ct.CompetencyTypeName,
                                        PrefixCode = c.PrefixCode,
                                        EvaluationTypeName = et.EvaluationTypeName,
                                        TaskNumber = t.TaskNumber,
                                        ModuleName = m.ModuleName,
                                        Question = tfut.Question,
                                        Name = b.Name,
                                        Mime = b.Mime,
                                        ModuleId = m.ModuleId,
                                        CompetencyId = c.CompetencyId,
                                        CompetencyTypeId = c.CompetencyTypeId,
                                        EvaluationTypeId = et.EvaluationTypeId
                                    }).AsNoTracking().FirstOrDefaultAsync();

            if (searchTask == null)
            {
                return new TaskFileUploadViewDetail();
            }

            return searchTask;
        }

        /// <summary>
        /// Update File Upload Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTaskFileUpload(TaskFileUploadFormModel model)
        {
            var findTask = await this.DB.Tasks.Where(Q => Q.TaskId == model.TaskId && Q.QuestionTypeId == 11 && Q.IsDeleted == false).FirstOrDefaultAsync();
            var userLogin = this.ClaimMan.GetLoginUserId();
            if (findTask == null)
            {
                return false;
            }

            //Cheking if tasks need to re-insert
            var toBeUpdated = findTask.ModuleId != model.ModuleId || findTask.CompetencyId != model.CompetencyId || findTask.EvaluationTypeId != model.EvaluationTypeId;

            if (toBeUpdated == true)
            {
                findTask.IsDeleted = true;
                findTask.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                findTask.UpdatedBy = userLogin;

                var taskNumber = await this.TaskMan.GetNumber(new GetNumberTaskModel
                {
                    CompetencyId = model.CompetencyId,
                    EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
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

                //Insert new Task
                var insertTask = new Talent.Entities.Entities.Tasks
                {
                    CompetencyId = model.CompetencyId,
                    BlobId = getUploadBlobUpdate,
                    //BlobId = model.BlobId,
                    EvaluationTypeId = model.EvaluationTypeId.GetValueOrDefault(),
                    QuestionTypeId = 11,
                    ModuleId = model.ModuleId,
                    TaskNumber = taskNumber,
                    LayoutType = model.LayoutType.GetValueOrDefault(),
                    StoryLineDescription = model.StoryLineDescription,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = userLogin,
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                };

                this.DB.Tasks.Add(insertTask);

                var insertFileUpload = new TaskFileUploadTypes
                {
                    Question = model.Question,
                    TaskId = insertTask.TaskId
                };

                this.DB.TaskFileUploadTypes.Add(insertFileUpload);
                await this.DB.SaveChangesAsync();
            }
            else
            {
                //if (findTask.BlobId != null && findTask.BlobId != model.BlobId)
                //{
                //    //Remove Blob nya
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
                findTask.UpdatedBy = userLogin;

                var findTaskFileUpload = await this.DB.TaskFileUploadTypes.Where(Q => Q.TaskId == model.TaskId).FirstOrDefaultAsync();
                findTaskFileUpload.Question = model.Question;

                await this.DB.SaveChangesAsync();
            }

            return true;
        }

    }
}
