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
    public class TaskHotSpotService
    {
        private readonly TalentContext DB;
        private readonly TaskService _TaskMan;
        private readonly BlobService _BlobMan;
        private readonly ClaimHelper _HelperMan;
        private readonly IFileStorageService _FileMan;

        public TaskHotSpotService(TalentContext talentContext, TaskService taskService, BlobService blobService, ClaimHelper claimHelper, IFileStorageService fileStorageService)
        {
            this.DB = talentContext;
            this._TaskMan = taskService;
            this._BlobMan = blobService;
            this._HelperMan = claimHelper;
            this._FileMan = fileStorageService;
        }

        public async Task<TaskHotSpotTypeModel> GetTaskHotSpotTypeById(int id)
        {
            var data = await this.DB.TaskHotSpotTypes.Where(Q => Q.TaskId == id).Select(Q => new TaskHotSpotTypeModel
            {
                TaskId = Q.TaskId,
                Question = Q.Question,
                BlobId = Q.BlobId
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<TaskHotSpotAnswerModel>> GetTaskHotSpotChoiceById(int id)
        {
            var timePoint = await this.DB.TimePoints.ToListAsync();

            var data = await this.DB.TaskHotSpotAnswers.Where(Q => Q.TaskId == id).Select(Q => new TaskHotSpotAnswerModel
            {
                TaskId = Q.TaskId,
                Number = Q.Number,
                Answer = Q.Answer,
                Score = Q.Score,
                TaskHotSpotAnswerId = Q.TaskHotSpotAnswerId
            }).ToListAsync();

            foreach (var d in data)
            {
                d.Score = timePoint.Find(Q => Q.TimePointId == d.Score).Score.GetValueOrDefault();
            }

            return data;
        }

        public async Task<int> CountTaskHotSpotId()
        {
            return await this.DB.TaskHotSpotAnswers.CountAsync() + 1;
        }

        public async Task<bool> InsertTaskHotspotAsync(TaskHotSpotModel t)
        {
            var userId = this._HelperMan.GetLoginUserId();
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            //var THSID = await this.CountTaskHotSpotId();
            var taskNumber = await this._TaskMan.GetNumber(new GetNumberTaskModel
            {
                CompetencyId = t.Task.CompetencyId,
                ModuleId = t.Task.ModuleId,
                EvaluationTypeId = t.Task.EvaluationTypeId
            });

            Guid? getUploadBlob = null;
            if (t.Task.FileContent != null)
            {
                getUploadBlob = await _FileMan.UploadFileFromBase64(t.Task.FileContent);
            }

            //TASK
            var task = new Talent.Entities.Entities.Tasks
            {
                //TaskId = CountTask,
                BlobId = getUploadBlob,
                //BlobId = t.Task.BlobId,
                CompetencyId = t.Task.CompetencyId,
                QuestionTypeId = 5,
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

            await this.DB.AddAsync(task);

            Guid? getUploadBlobIdType = null;
            if (t.Type.FileContent != null)
            {
                getUploadBlobIdType = await _FileMan.UploadFileFromBase64(t.Type.FileContent);
            }
            var x = task.TaskId;
            //TYPE
            var type = new TaskHotSpotTypes
            {
                TaskId = x,
                Question = t.Type.Question,
                BlobId = getUploadBlobIdType.GetValueOrDefault(),
                //BlobId = t.Type.BlobId,
            };

            //CHOICE
            var choice = new List<TaskHotSpotAnswers>();
            foreach (var c in t.Choice)
            {
                choice.Add(new TaskHotSpotAnswers
                {
                    //TaskHotSpotAnswerId = THSID,
                    TaskId = x,
                    Number = c.Number,
                    Answer = c.Answer,
                    Score = c.Score
                });
                //THSID++;
            }
            await this.DB.AddAsync(type);
            await this.DB.AddRangeAsync(choice);
            this.DB.SaveChanges();

            return true;
        }

        public async Task<bool> UpdateTaskHotspotAsync(int taskId, TaskHotSpotModel t)
        {
            //Variable Pembantu
            var userId = this._HelperMan.GetLoginUserId();
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            //Variable yng ingin di update
            var taskUpdate = await this.DB.Tasks.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var typeUpdate = await this.DB.TaskHotSpotTypes.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var choice = await this.DB.TaskHotSpotAnswers.Where(Q => Q.TaskId == taskId).ToListAsync();

            //Temporary
            var temp = choice;
            var blobTempDelete = new List<BlobModel>();

            //KALAU TASKCODE & NUMBER TIDAK BERUBAH
            if (t.Task.CompetencyId == taskUpdate.CompetencyId && t.Task.EvaluationTypeId == taskUpdate.EvaluationTypeId && t.Task.ModuleId == taskUpdate.ModuleId && t.Task.TaskNumber == taskUpdate.TaskNumber)
            {
                if (taskUpdate.BlobId != t.Task.BlobId && taskUpdate.BlobId != null)
                {
                    var blob = await this._BlobMan.GetBlobById(taskUpdate.BlobId);
                    blobTempDelete.Add(blob);
                }

                Guid? getUploadBlobUpdate = null;

                if (taskUpdate.BlobId == t.Task.BlobId && taskUpdate.BlobId != null)
                {
                    getUploadBlobUpdate = t.Task.BlobId;
                }

                if (t.Task.FileContent != null)
                {
                    if (t.Task.BlobId != null)
                    {
                        await _FileMan.DeleteFileAsync(t.Task.BlobId.GetValueOrDefault(), t.Task.FileContent.ContentType);
                    }
                    getUploadBlobUpdate = await _FileMan.UploadFileFromBase64(t.Task.FileContent);
                }

                //Task
                taskUpdate.BlobId = getUploadBlobUpdate;
                //taskUpdate.BlobId = t.Task.BlobId;
                taskUpdate.QuestionTypeId = 5;
                taskUpdate.LayoutType = t.Task.LayoutType;
                taskUpdate.StoryLineDescription = t.Task.StoryLineDescription;
                taskUpdate.EvaluationTypeId = t.Task.EvaluationTypeId;
                taskUpdate.UpdatedAt = datenow;
                taskUpdate.UpdatedBy = userId;

                //Type
                if (typeUpdate.BlobId != t.Type.BlobId && taskUpdate.BlobId != null)
                {
                    var blob = await this._BlobMan.GetBlobById(taskUpdate.BlobId);
                    blobTempDelete.Add(blob);
                }

                Guid? getUploadBlobUpdateType = null;

                if (typeUpdate.BlobId == t.Type.BlobId && typeUpdate.BlobId != null)
                {
                    getUploadBlobUpdateType = t.Type.BlobId;
                }

                if (t.Type.FileContent != null)
                {
                    if (t.Type.BlobId != null)
                    {
                        await _FileMan.DeleteFileAsync(t.Type.BlobId.GetValueOrDefault(), t.Type.FileContent.ContentType);
                    }
                    getUploadBlobUpdateType = await _FileMan.UploadFileFromBase64(t.Type.FileContent);
                }

                typeUpdate.BlobId = getUploadBlobUpdateType.GetValueOrDefault() ;
                //typeUpdate.BlobId = t.Type.BlobId;
                typeUpdate.Question = t.Type.Question;

                //CHOICE
                //kalau yang di update lebih dikit... maka listnya harus di kurangin dulu 

                if (choice.Count > t.Choice.Count)
                {
                    for (int i = choice.Count - 1; i > t.Choice.Count - 1; i--)
                    {
                        var remove = choice[i];
                        this.DB.TaskHotSpotAnswers.Remove(remove);
                    }
                    //Assign Data
                    for (int i = 0; i < t.Choice.Count; i++)
                    {
                        //TASKTEBAKGAMABRID
                        choice[i].Answer = t.Choice[i].Answer;
                        choice[i].Score = t.Choice[i].Score;
                        choice[i].TaskId = taskUpdate.TaskId;
                        choice[i].Number = t.Choice[i].Number;
                    }
                }

                //kalau yang di update lebih banyak... maka listnya harus di tambahin dulu
                if (choice.Count < t.Choice.Count)
                {
                    //edit yang udah ada
                    for (int i = 0; i < choice.Count; i++)
                    {
                        choice[i].Answer = t.Choice[i].Answer;
                        choice[i].Score = t.Choice[i].Score;
                        choice[i].TaskId = taskUpdate.TaskId;
                        choice[i].Number = t.Choice[i].Number;
                    }
                    //tambahin yang kurang
                    for (int i = choice.Count; i < t.Choice.Count; i++)
                    {
                        var add = new TaskHotSpotAnswers
                        {
                            //TaskHotSpotAnswerId = THSID,
                            TaskId = taskId,
                            Number = t.Choice[i].Number,
                            Answer = t.Choice[i].Answer,
                            Score = t.Choice[i].Score
                        };
                        await this.DB.AddAsync(add);
                        //THSID++;
                    }
                }

                if (choice.Count == t.Choice.Count)
                {
                    for (int i = 0; i < choice.Count; i++)
                    {
                        //cek kalo blob id ada ato engga di db.
                        choice[i].Number = t.Choice[i].Number;
                        choice[i].Answer = t.Choice[i].Answer;
                        choice[i].Score = t.Choice[i].Score;
                        choice[i].TaskId = taskUpdate.TaskId;
                    }
                }
                await this.DB.SaveChangesAsync();
                return true;
            }

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

            Guid? getUploadBlob = null;
            if (taskUpdate.BlobId == t.Task.BlobId && taskUpdate.BlobId != null)
            {
                getUploadBlob = t.Task.BlobId;
            }

            if (t.Task.FileContent != null)
            {
                if (t.Task.BlobId != null)
                {
                    await _FileMan.DeleteFileAsync(t.Task.BlobId.GetValueOrDefault(), t.Task.FileContent.ContentType);
                }
                getUploadBlob = await _FileMan.UploadFileFromBase64(t.Task.FileContent);
            }

            //TASK
            var task = new Talent.Entities.Entities.Tasks
            {
                //BlobId = t.Task.BlobId,
                BlobId = getUploadBlob,
                CompetencyId = t.Task.CompetencyId,
                QuestionTypeId = 5,
                ModuleId = t.Task.ModuleId,
                TaskNumber = taskNumber,
                LayoutType = t.Task.LayoutType,
                StoryLineDescription = t.Task.StoryLineDescription,
                EvaluationTypeId = t.Task.EvaluationTypeId,
                CreatedAt = taskUpdate.CreatedAt,
                CreatedBy = userId,
                UpdatedAt = datenow,
                UpdatedBy = userId
            };

            await this.DB.AddAsync(task);

            Guid? getUploadBlobType = null;

            if (typeUpdate.BlobId == t.Type.BlobId && typeUpdate.BlobId != null)
            {
                getUploadBlobType = t.Type.BlobId;
            }

            if (t.Type.FileContent != null)
            {
                if (t.Type.BlobId != null)
                {
                    await _FileMan.DeleteFileAsync(t.Type.BlobId.GetValueOrDefault(), t.Type.FileContent.ContentType);
                }
                getUploadBlobType = await _FileMan.UploadFileFromBase64(t.Type.FileContent);
            }

            var x = task.TaskId;
            //TYPE
            var type = new TaskHotSpotTypes
            {
                TaskId = x,
                Question = t.Type.Question,
                BlobId = getUploadBlobType.GetValueOrDefault(),
                //BlobId = t.Type.BlobId,
            };
            //CHOICE
            var choices = new List<TaskHotSpotAnswers>();
            foreach (var p in t.Choice)
            {
                choices.Add(new TaskHotSpotAnswers
                {
                    //TaskHotSpotAnswerId = THSID,
                    TaskId = x,
                    Answer = p.Answer,
                    Score = p.Score,
                    Number = p.Number
                });
                //THSID++;
            }
            await this.DB.AddAsync(type);
            await this.DB.AddRangeAsync(choices);
            await this.DB.SaveChangesAsync();
            return true;
        }
    }
}
