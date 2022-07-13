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
    public class TaskTebakGambarService
    {
        private readonly TalentContext _Db;
        private readonly TaskService _TaskMan;
        private readonly IFileStorageService _FileMan;
        private readonly BlobService _BlobMan;
        private readonly ClaimHelper _HelperMan;
        private readonly FileService FileServiceMan;

        public TaskTebakGambarService(TalentContext talentContext, TaskService taskService, IFileStorageService fileService, BlobService blobService, ClaimHelper claimHelper, FileService fs)
        {
            this._Db = talentContext;
            this._TaskMan = taskService;
            this._FileMan = fileService;
            this._BlobMan = blobService;
            this._HelperMan = claimHelper;
            this.FileServiceMan = fs;
        }

        public async Task<TaskTebakGambarTypesModel> GetTaskTebakGambarTypeById(int id)
        {
            var timePoint = await this._Db.TimePoints.ToListAsync();

            var data = await this._Db.TaskTebakGambarTypes.Where(Q => Q.TaskId == id).Select(Q => new TaskTebakGambarTypesModel
            {
                TaskId = Q.TaskId,
                Answer = Q.Answer,
                Question = Q.Question,
                Score = Q.Score
            }).FirstOrDefaultAsync();

            data.Score = timePoint.Find(Q => Q.TimePointId == data.Score).Score.GetValueOrDefault();

            return data;
        }

        public async Task<List<TaskTebakGambarPicturesModel>> GetTaskTebakGambarPictureById(int id)
        {
            var data = await this._Db.TaskTebakGambarPictures.Where(Q => Q.TaskId == id).Select(Q => new TaskTebakGambarPicturesModel
            {
                TaskId = Q.TaskId,
                BlobId = Q.BlobId,
                Number = Q.Number,
                TaskTebakGambarId = Q.TaskTebakGambarId
            }).OrderBy(Q => Q.Number).ToListAsync();
            return data;
        }

        public async Task<int> Count()
        {
            return await this._Db.Tasks.CountAsync() + 1;
        }

        public async Task<int> CountTaskTebakGambarId()
        {
            return await this._Db.TaskTebakGambarPictures.CountAsync() + 1;
        }

        public async Task<bool> InsertTaskTebakGambar(TaskTebakGambarModel t)
        {
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            var userId = this._HelperMan.GetLoginUserId();
            //var CountTTGID = await this.CountTaskTebakGambarId() + 1;

            var taskNumber = await this._TaskMan.GetNumber(new GetNumberTaskModel
            {
                CompetencyId = t.Task.CompetencyId,
                ModuleId = t.Task.ModuleId,
                EvaluationTypeId = t.Task.EvaluationTypeId
            });

            //TASK
            //pake transaction
            var task = new Talent.Entities.Entities.Tasks
            {
                //TaskId = CountTask,
                //BlobId = t.Task.BlobId,
                CompetencyId = t.Task.CompetencyId,
                QuestionTypeId = 4,
                ModuleId = t.Task.ModuleId,
                TaskNumber = taskNumber,
                LayoutType = t.Task.LayoutType,
                StoryLineDescription = t.Task.StoryLineDescription,
                EvaluationTypeId = t.Task.EvaluationTypeId,
                CreatedAt = datenow,
                CreatedBy = userId,
                UpdatedAt = datenow,
                UpdatedBy = userId,
                IsDeleted = false
            };

            //add blob id 
            if (t.Task.FileContent != null)
            {
                if (string.IsNullOrEmpty(t.Task.FileContent.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(t.Task.FileContent);

                    task.BlobId = blobId;
                }
            }

            await this._Db.AddAsync(task);

            var x = task.TaskId;
            //TYPE
            var type = new TaskTebakGambarTypes
            {
                TaskId = x,
                Answer = t.Type.Answer,
                Question = t.Type.Question,
                Score = t.Type.Score
            };

            //PICTURE
            var picture = new List<TaskTebakGambarPictures>();
            foreach (var p in t.Picture)
            {
                var pic = new TaskTebakGambarPictures
                {
                    //BlobId = p.BlobId,
                    TaskId = x,
                    Number = p.Number,
                    //TaskTebakGambarId = CountTTGID
                };

                if (p.ImageUpload != null)
                {
                    if (string.IsNullOrEmpty(p.ImageUpload.Base64) == false)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(p.ImageUpload);

                        pic.BlobId = blobId;
                    }
                }

                picture.Add(pic);
                //CountTTGID++;
            }
            await this._Db.AddAsync(type);
            await this._Db.AddRangeAsync(picture);
            await this._Db.SaveChangesAsync();
            return true;
        }

        //UPDATE 
        public async Task<bool> UpdateTaskTebakGambar(int taskId, TaskTebakGambarModel t)
        {
            //Variable Pembantu
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            //var CountTask = await this.Count();
            var CountTTGID = await this.CountTaskTebakGambarId() + 1;
            var userId = this._HelperMan.GetLoginUserId();

            //Variable yng ingin di update
            var taskUpdate = await this._Db.Tasks.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var typeUpdate = await this._Db.TaskTebakGambarTypes.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var pictureUpdate = await this._Db.TaskTebakGambarPictures.Where(Q => Q.TaskId == taskId).ToListAsync();

            var toBeUpdated = taskUpdate.ModuleId != t.Task.ModuleId || taskUpdate.CompetencyId != t.Task.CompetencyId || taskUpdate.EvaluationTypeId != t.Task.EvaluationTypeId;

            t.Picture.OrderBy(Q => Q.Number);

            //Update yang lama
            if (toBeUpdated == false)
            {
                this._Db.RemoveRange(pictureUpdate);

                if (t.Task.FileContent != null)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(t.Task.FileContent);
                    taskUpdate.BlobId = blobId;
                }
                else
                {
                    taskUpdate.BlobId = t.Task.BlobId;
                }

                taskUpdate.CompetencyId = t.Task.CompetencyId;
                taskUpdate.EvaluationTypeId = t.Task.EvaluationTypeId;
                taskUpdate.LayoutType = t.Task.LayoutType;
                taskUpdate.ModuleId = t.Task.ModuleId;
                taskUpdate.StoryLineDescription = t.Task.StoryLineDescription;
                taskUpdate.UpdatedAt =datenow;
                taskUpdate.UpdatedBy = userId;

                typeUpdate.Answer = t.Type.Answer;
                typeUpdate.Question = t.Type.Question;
                typeUpdate.Score = t.Type.Score;

                var picture = new List<TaskTebakGambarPictures>();
                foreach (var p in t.Picture)
                {
                    var value = new TaskTebakGambarPictures
                    {
                        TaskId = taskUpdate.TaskId,
                        Number = p.Number,
                    };


                    if (p.ImageUpload != null)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(p.ImageUpload);
                        value.BlobId = blobId;
                    }
                    else
                    {
                        value.BlobId = p.BlobId.Value;
                    }

                    picture.Add(value);
                }

                await this._Db.AddRangeAsync(picture);

            }
            //Buat baru
            else
            {
                //Temporary
                var temp = pictureUpdate;
                var blobTempDelete = new List<BlobModel>();

                //SOFT DELETE TASK
                taskUpdate.IsDeleted = true;
                taskUpdate.UpdatedAt = datenow;
                taskUpdate.UpdatedBy = userId;

                var taskNumber = await this._TaskMan.GetNumber(new GetNumberTaskModel
                {
                    CompetencyId = t.Task.CompetencyId,
                    ModuleId = t.Task.ModuleId,
                    EvaluationTypeId = t.Task.EvaluationTypeId
                });

                //TASK
                var task = new Talent.Entities.Entities.Tasks
                {
                    //BlobId = t.Task.BlobId,
                    CompetencyId = t.Task.CompetencyId,
                    QuestionTypeId = 4,
                    ModuleId = t.Task.ModuleId,
                    TaskNumber = taskNumber,
                    LayoutType = t.Task.LayoutType,
                    StoryLineDescription = t.Task.StoryLineDescription,
                    EvaluationTypeId = t.Task.EvaluationTypeId,
                    CreatedAt = taskUpdate.CreatedAt,
                    CreatedBy = userId,
                    UpdatedAt = datenow,
                    UpdatedBy = userId,
                    IsDeleted = false
                };

                if (t.Task.FileContent != null)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(t.Task.FileContent);
                    task.BlobId = blobId;
                }
                else
                {
                    task.BlobId = t.Task.BlobId;
                }

                await this._Db.AddAsync(task);
                var x = task.TaskId;
                //TYPE
                var type = new TaskTebakGambarTypes
                {
                    TaskId = x,
                    Answer = t.Type.Answer,
                    Question = t.Type.Question,
                    Score = t.Type.Score
                };
                //PICTURE
                var picture = new List<TaskTebakGambarPictures>();
                foreach (var p in t.Picture)
                {
                    var value = new TaskTebakGambarPictures
                    {
                        TaskId = x,
                        Number = p.Number,
                    };


                    if (p.ImageUpload != null)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(p.ImageUpload);
                        value.BlobId = blobId;
                    }
                    else
                    {
                        value.BlobId = p.BlobId.Value;
                    }

                    picture.Add(value);
                }
                await this._Db.AddAsync(type);
                await this._Db.AddRangeAsync(picture);
            }

            await this._Db.SaveChangesAsync();

            return true;
        }
    }
}
