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
    public class TaskEssayTypeService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;
        private readonly IFileStorageService _FileMan;

        public TaskEssayTypeService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileStorageService)
        {
            this.DB = db;
            this.HelperMan = claimHelper;
            this._FileMan = fileStorageService;
        }

        public async Task<TaskEssayTypeFormModel> GetTaskEssayTypeById(int id)
        {
            var task = await this.DB.Tasks.AsNoTracking().Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            var essayType = await this.DB.TaskEssayTypes.AsNoTracking().Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            var blob = task.BlobId.HasValue ? await this.DB.Blobs.AsNoTracking().Where(Q => Q.BlobId == task.BlobId).FirstOrDefaultAsync() : null;

            var result = new TaskEssayTypeFormModel
            {
                TaskId = id,
                CompetencyId = task.CompetencyId,
                QuestionTypeId = task.QuestionTypeId,
                ModuleId = task.ModuleId,
                BlobId = task.BlobId,
                FileName = blob == null ? null : blob.Name,
                Mime = blob == null ? null : blob.Mime,
                EvaluationTypeId = task.EvaluationTypeId,
                TaskNumber = task.TaskNumber,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                IsDeleted = false,
                Question = essayType.Question
            };

            if (result == null)
            {
                result = new TaskEssayTypeFormModel();
            }

            return result;
        }

        public async Task<int> CreateTaskEssayType(TaskEssayTypeFormModel model)
        {
            var userId = this.HelperMan.GetLoginUserId();
            var taskNumber = await this.DB.Tasks.Where(Q => Q.CompetencyId == model.CompetencyId && Q.ModuleId == model.ModuleId && Q.EvaluationTypeId == model.EvaluationTypeId).CountAsync() + 1;
            //Guid? blobId = model.BlobId.HasValue ? model.BlobId : null;

            Guid? getUploadBlob = null;
            if (!string.IsNullOrEmpty(model.FileContent?.Base64))
            {
                getUploadBlob = await _FileMan.UploadFileFromBase64(model.FileContent);
            }

            var createModel = new Tasks
            {
                CompetencyId = model.CompetencyId,
                QuestionTypeId = model.QuestionTypeId,
                ModuleId = model.ModuleId,
                //BlobId = blobId,
                BlobId = getUploadBlob,
                EvaluationTypeId = model.EvaluationTypeId,
                TaskNumber = taskNumber,
                LayoutType = model.LayoutType,
                StoryLineDescription = model.StoryLineDescription,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedBy = userId,
                IsDeleted = false,
            };

            this.DB.Tasks.Add(createModel);
            //this.DB.SaveChanges();

            var createModel2 = new TaskEssayTypes
            {
                TaskId = createModel.TaskId,
                Question = model.Question
            };

            this.DB.TaskEssayTypes.Add(createModel2);
            await this.DB.SaveChangesAsync();


            return createModel.TaskId;
        }

        public async Task<bool> UpdateTaskEssayType(TaskEssayTypeFormModel model)
        {
            var updateModel = await this.DB.Tasks.FindAsync(model.TaskId);
            var userId = this.HelperMan.GetLoginUserId();

            if (updateModel == null)
            {
                return false;
            }

            if (model.TaskNumber == updateModel.TaskNumber)
            {
                Guid? getUploadBlobUpdate = null;

                if (updateModel.BlobId == model.BlobId && updateModel.BlobId != null)
                {
                    getUploadBlobUpdate = model.BlobId;
                }

                if (model.FileContent != null)
                {
                    if (model.BlobId != null)
                    {
                        await _FileMan.DeleteFileAsync(model.BlobId.GetValueOrDefault(), model.FileContent.ContentType);

                        var BlobId = await this.DB.Blobs.Where(Q => Q.BlobId == updateModel.BlobId).FirstOrDefaultAsync();
                        this.DB.Blobs.Remove(BlobId);
                    }
                    getUploadBlobUpdate = await _FileMan.UploadFileFromBase64(model.FileContent);
                }

                updateModel.CompetencyId = model.CompetencyId;
                updateModel.ModuleId = model.ModuleId;
                updateModel.BlobId = getUploadBlobUpdate;
                updateModel.EvaluationTypeId = model.EvaluationTypeId;
                updateModel.LayoutType = model.LayoutType;
                updateModel.StoryLineDescription = model.StoryLineDescription;
                updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                updateModel.UpdatedBy = userId;

                var updateModel2 = await this.DB.TaskEssayTypes.FindAsync(model.TaskId);

                updateModel2.Question = model.Question;
            }
            else
            {
                var taskNumber = await this.DB.Tasks.Where(Q => Q.CompetencyId == model.CompetencyId && Q.ModuleId == model.ModuleId && Q.EvaluationTypeId == model.EvaluationTypeId).CountAsync() + 1;

                updateModel.IsDeleted = true;
                updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                updateModel.UpdatedBy = userId;

                Guid? getUploadBlobUpdate = null;

                if (updateModel.BlobId == model.BlobId && updateModel.BlobId != null)
                {
                    getUploadBlobUpdate = model.BlobId;
                }

                if (model.FileContent != null)
                {
                    if (model.BlobId != null)
                    {
                        await _FileMan.DeleteFileAsync(model.BlobId.GetValueOrDefault(), model.FileContent.ContentType);
                    }
                    getUploadBlobUpdate = await _FileMan.UploadFileFromBase64(model.FileContent);
                }

                var createModel = new Tasks
                {
                    CompetencyId = model.CompetencyId,
                    ModuleId = model.ModuleId,
                    EvaluationTypeId = model.EvaluationTypeId,
                    TaskNumber = taskNumber,
                    QuestionTypeId = model.QuestionTypeId,
                    //BlobId = model.BlobId.HasValue ? model.BlobId : updateModel.BlobId,
                    BlobId = getUploadBlobUpdate,
                    LayoutType = model.LayoutType,
                    StoryLineDescription = model.StoryLineDescription,
                    CreatedBy = userId,
                    CreatedAt = updateModel.CreatedAt,
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedBy = userId,
                    IsDeleted = false,
                };

                this.DB.Tasks.Add(createModel);
                //this.DB.SaveChanges();

                var createModel2 = new TaskEssayTypes
                {
                    TaskId = createModel.TaskId,
                    Question = model.Question
                };

                this.DB.TaskEssayTypes.Add(createModel2);
                //this.DB.SaveChanges();
            }

            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            await this.DB.SaveChangesAsync();

            return true;
        }

    }
}
