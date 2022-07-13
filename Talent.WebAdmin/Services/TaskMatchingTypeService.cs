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
    public class TaskMatchingTypeService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly FileService FileServiceMan;

        public TaskMatchingTypeService(TalentContext db, IFileStorageService fileService, ClaimHelper claimHelper, FileService fs)
        {
            this.DB = db;
            this.FileMan = fileService;
            this.HelperMan = claimHelper;
            this.FileServiceMan = fs;
        }

        public async Task<TaskMatchingTypeFormModel> GetTaskMatchingTypeById(int id)
        {
            var task = await this.DB.Tasks.AsNoTracking().Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            var matchingType = await this.DB.TaskMatchingTypes.AsNoTracking().Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            var blob = task.BlobId.HasValue ? await this.DB.Blobs.AsNoTracking().Where(Q => Q.BlobId == task.BlobId).FirstOrDefaultAsync() : null;

            var list = (
                from c in await DB.TaskMatchingChoices.AsNoTracking().Where(Q => Q.TaskId == id).ToListAsync()
                join b in await DB.Blobs.AsNoTracking().ToListAsync() on c.BlobId equals b.BlobId into emptyBlobId
                from eb in emptyBlobId.DefaultIfEmpty()
                select new TaskMatchingChoiceFormModel
                {
                    TaskMatchingCode = c.TaskMatchingChoiceCode,
                    Text = c.Text,
                    BlobId = c.BlobId.HasValue ? c.BlobId : null,
                    FileName = c.BlobId.HasValue ? eb.Name : null,
                    Mime = c.BlobId.HasValue ? eb.Mime : null
                }).OrderBy(Q => Q.TaskMatchingCode).ToList();

            var result = new TaskMatchingTypeFormModel
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
                Question = matchingType.Question,
                Answer = matchingType.Answer,
                Score = matchingType.Score,
                TaskMatchingChoices = list
            };

            if (result == null)
            {
                result = new TaskMatchingTypeFormModel();
            }

            return result;
        }

        public async Task<int> CreateTaskMatchingType(TaskMatchingTypeFormModel model)
        {
            var userId = this.HelperMan.GetLoginUserId();
            var taskNumber = await this.DB.Tasks.Where(Q => Q.CompetencyId == model.CompetencyId && Q.ModuleId == model.ModuleId && Q.EvaluationTypeId == model.EvaluationTypeId).CountAsync() + 1;
            Guid? blobId = model.BlobId.HasValue ? model.BlobId : null;

            var createModel = new Tasks
            {
                CompetencyId = model.CompetencyId,
                QuestionTypeId = model.QuestionTypeId,
                ModuleId = model.ModuleId,
                //BlobId = blobId,
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

            if (model.ImageUpload != null)
            {
                if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                {
                    var theBlobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);
                    createModel.BlobId = theBlobId;
                }
            }

            this.DB.Tasks.Add(createModel);

            var createModel2 = new TaskMatchingTypes
            {
                TaskId = createModel.TaskId,
                Question = model.Question,
                Answer = model.Answer,
                Score = model.Score
            };

            this.DB.TaskMatchingTypes.Add(createModel2);

            foreach (TaskMatchingChoiceFormModel t in model.TaskMatchingChoices)
            {
                //Guid? blobId2 = t.BlobId.HasValue ? t.BlobId : null;
                var createModel3 = new TaskMatchingChoices
                {
                    TaskId = createModel.TaskId,
                    TaskMatchingChoiceCode = t.TaskMatchingCode,
                    Text = t.Text,
                };

                if (t.ImageUpload != null)
                {
                    if (string.IsNullOrEmpty(t.ImageUpload.Base64) == false)
                    {
                        var theBlobId = await FileServiceMan.UploadFileFromBase64(t.ImageUpload);
                        createModel3.BlobId = theBlobId;
                    }
                }

                this.DB.TaskMatchingChoices.Add(createModel3);
            }

            await DB.SaveChangesAsync();
            return createModel.TaskId;
        }

        public async Task<bool> UpdateTaskMatchingType(TaskMatchingTypeFormModel model)
        {
            var userId = this.HelperMan.GetLoginUserId();
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var updateModel = await this.DB.Tasks.FindAsync(model.TaskId);
            Blobs blobs = null;

            if (updateModel == null)
            {
                return false;
            }

            if (updateModel.BlobId != null)
            {
                blobs = await DB.Blobs.Where(Q => Q.BlobId == updateModel.BlobId).FirstOrDefaultAsync();
            }
            // Delete file jika model null tp ada file di DB
            // PLIS LAH INI GK SULIT BUAT DIPIKIRIN dan cuma butuh waktu 2 menit!!!
            /// - Rant oleh Aldrian.
            if (blobs != null && model.BlobId == null)
            {
                await FileMan.DeleteFileAsync(blobs.BlobId, blobs.Mime);
                DB.Blobs.Remove(blobs);
            }
            if (model.TaskNumber == updateModel.TaskNumber)
            {
                updateModel.CompetencyId = model.CompetencyId;
                updateModel.ModuleId = model.ModuleId;
                updateModel.EvaluationTypeId = model.EvaluationTypeId;
                //updateModel.BlobId = model.BlobId.HasValue ? model.BlobId : updateModel.BlobId;
                updateModel.LayoutType = model.LayoutType;
                updateModel.StoryLineDescription = model.StoryLineDescription;
                updateModel.UpdatedAt = thisDate;
                updateModel.UpdatedBy = userId;

                if (model.ImageUpload != null)
                {
                    if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);

                        if (blobs != null)
                        {
                            await FileServiceMan.DeleteFileAsync(blobs.BlobId, blobs.Mime);
                        }

                        updateModel.BlobId = blobId;
                    }
                }

                var updateModel2 = await this.DB.TaskMatchingTypes.FindAsync(model.TaskId);

                updateModel2.Question = model.Question;
                updateModel2.Answer = model.Answer;
                updateModel2.Score = model.Score;

                var dataToDelete = await DB.TaskMatchingChoices.AsNoTracking().Where(Q => Q.TaskId == model.TaskId).Select(Q => Q.TaskMatchingChoiceId).ToListAsync();
                for (int i = 0; i < dataToDelete.Count(); i++)
                {
                    var deleteModel = await this.DB.TaskMatchingChoices.FindAsync(dataToDelete[i]);
                    DB.TaskMatchingChoices.Remove(deleteModel);
                }

                foreach (TaskMatchingChoiceFormModel t in model.TaskMatchingChoices)
                {
                    //Guid? blobId = t.BlobId.HasValue ? t.BlobId : null;

                    var createModel = new TaskMatchingChoices
                    {
                        TaskId = t.TaskId,
                        TaskMatchingChoiceCode = t.TaskMatchingCode,
                        Text = t.Text,
                    };

                    var isExist = t.BlobId.HasValue;

                    if (t.ImageUpload != null)
                    {
                        if (string.IsNullOrEmpty(t.ImageUpload.Base64) == false)
                        {
                            //delete existing file on server
                            if (isExist)
                            {
                                var file = await DB.Blobs.Where(Q => Q.BlobId == t.BlobId).FirstOrDefaultAsync();
                                if (file != null)
                                {
                                    await FileServiceMan.DeleteFileAsync(file.BlobId, file.Mime);
                                }
                            }
                            //upload new file 
                            var theBlobId = await FileServiceMan.UploadFileFromBase64(t.ImageUpload);
                            createModel.BlobId = theBlobId;
                        }
                    }
                    else
                    {
                        if (isExist)
                        {
                            createModel.BlobId = t.BlobId;
                        }
                    }

                    this.DB.TaskMatchingChoices.Add(createModel);
                }
            }
            else
            {
                var taskNumber = await this.DB.Tasks.Where(Q => Q.CompetencyId == model.CompetencyId && Q.ModuleId == model.ModuleId && Q.EvaluationTypeId == model.EvaluationTypeId).CountAsync() + 1;

                updateModel.IsDeleted = true;
                updateModel.UpdatedAt = thisDate;
                updateModel.UpdatedBy = userId;

                var createModel = new Tasks
                {
                    CompetencyId = model.CompetencyId,
                    ModuleId = model.ModuleId,
                    EvaluationTypeId = model.EvaluationTypeId,
                    QuestionTypeId = model.QuestionTypeId,
                    //BlobId = model.BlobId.HasValue ? model.BlobId : updateModel.BlobId,
                    TaskNumber = taskNumber,
                    LayoutType = model.LayoutType,
                    StoryLineDescription = model.StoryLineDescription,
                    CreatedBy = userId,
                    CreatedAt = updateModel.CreatedAt,
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedBy = userId,
                    IsDeleted = false,
                };

                if (model.ImageUpload != null)
                {
                    if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);

                        if (blobs != null)
                        {
                            await FileServiceMan.DeleteFileAsync(blobs.BlobId, blobs.Mime);
                        }

                        createModel.BlobId = blobId;
                    }
                }

                this.DB.Tasks.Add(createModel);

                var createModel2 = new TaskMatchingTypes
                {
                    TaskId = createModel.TaskId,
                    Question = model.Question,
                    Answer = model.Answer,
                    Score = model.Score
                };

                this.DB.TaskMatchingTypes.Add(createModel2);

                foreach (TaskMatchingChoiceFormModel t in model.TaskMatchingChoices)
                {
                    //Guid? blobId = t.BlobId.HasValue ? t.BlobId : null;
                    var createModel3 = new TaskMatchingChoices
                    {
                        TaskId = createModel.TaskId,
                        TaskMatchingChoiceCode = t.TaskMatchingCode,
                        Text = t.Text,
                    };

                    var isExist = t.BlobId.HasValue;

                    if (t.ImageUpload != null)
                    {
                        if (string.IsNullOrEmpty(t.ImageUpload.Base64) == false)
                        {
                            //delete existing file on server
                            if (isExist)
                            {
                                var file = await DB.Blobs.Where(Q => Q.BlobId == t.BlobId).FirstOrDefaultAsync();
                                if (file != null)
                                {
                                    await FileServiceMan.DeleteFileAsync(file.BlobId, file.Mime);
                                }
                            }
                            //upload new file 
                            var theBlobId = await FileServiceMan.UploadFileFromBase64(t.ImageUpload);
                            createModel3.BlobId = theBlobId;
                        }
                    }
                    else
                    {
                        if (isExist)
                        {
                            createModel3.BlobId = t.BlobId;
                        }
                    }

                    this.DB.TaskMatchingChoices.Add(createModel3);
                }
            }

            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            await this.DB.SaveChangesAsync();

            return true;
        }
    }
}
