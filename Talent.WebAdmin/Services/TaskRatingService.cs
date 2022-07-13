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
    public class TaskRatingService
    {
        private readonly TalentContext DB;
        private readonly TaskService _TaskMan;
        private readonly BlobService _BlobMan;
        private readonly ClaimHelper _HelperMan;
        private readonly IFileStorageService _FileMan;

        public TaskRatingService(TalentContext talentContext, TaskService taskService, BlobService blobService, ClaimHelper claimHelper, IFileStorageService fileStorageService)
        {
            this.DB = talentContext;
            this._TaskMan = taskService;
            this._BlobMan = blobService;
            this._HelperMan = claimHelper;
            this._FileMan = fileStorageService;
        }

        public async Task<TaskRatingTypeModel> GetTaskRatingTypeById(int id)
        {
            var timePoint = await this.DB.TimePoints.ToListAsync();

            var data = await this.DB.TaskRatingTypes.Where(Q => Q.TaskId == id).Select(Q => new TaskRatingTypeModel
            {
                TaskId = Q.TaskId,
                Question = Q.Question,
                Score0To20 = Q.Score0To20,
                Score21To40 = Q.Score21To40,
                Score41To60 = Q.Score41To60,
                Score61To80 = Q.Score61To80,
                Score81To100 = Q.Score81To100
            }).FirstOrDefaultAsync();

            data.Score0To20 = timePoint.Find(Q => Q.TimePointId == data.Score0To20).Score.GetValueOrDefault();
            data.Score21To40 = timePoint.Find(Q => Q.TimePointId == data.Score21To40).Score.GetValueOrDefault();
            data.Score41To60 = timePoint.Find(Q => Q.TimePointId == data.Score41To60).Score.GetValueOrDefault();
            data.Score61To80 = timePoint.Find(Q => Q.TimePointId == data.Score61To80).Score.GetValueOrDefault();
            data.Score81To100 = timePoint.Find(Q => Q.TimePointId == data.Score81To100).Score.GetValueOrDefault();

            return data;
        }

        public async Task<List<TaskRatingChoicesModel>> GetTaskRatingChoiceById(int id)
        {
            var data = await this.DB.TaskRatingChoices.Where(Q => Q.TaskId == id).Select(Q => new TaskRatingChoicesModel
            {
                TaskId = Q.TaskId,
                Number = Q.Number,
                TaskRatingChoiceId = Q.TaskRatingChoiceId,
                Text = Q.Text
            }).ToListAsync();
            return data;
        }

        public async Task<int> CountTaskRatingId()
        {
            return await this.DB.TaskRatingChoices.CountAsync() + 1;
        }

        public async Task<bool> InsertTaskRatingAsync(TaskRatingModel t)
        {
            var userId = this._HelperMan.GetLoginUserId();
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            var taskNumber = await this._TaskMan.GetNumber(new GetNumberTaskModel
            {
                CompetencyId = t.Task.CompetencyId,
                ModuleId = t.Task.ModuleId,
                EvaluationTypeId = t.Task.EvaluationTypeId
            });
            //TASK

            Guid? getUploadBlob = null;
            if (t.Task.FileContent != null)
            {
                getUploadBlob = await _FileMan.UploadFileFromBase64(t.Task.FileContent);
            }

            var task = new Talent.Entities.Entities.Tasks
            {
                //TaskId = CountTask,
                //BlobId = t.Task.BlobId,
                BlobId = getUploadBlob,
                CompetencyId = t.Task.CompetencyId,
                QuestionTypeId = 9,
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

            var x = task.TaskId;
            //TYPE
            var type = new TaskRatingTypes
            {
                TaskId = x,
                Question = t.Type.Question,
                Score0To20 = t.Type.Score0To20,
                Score21To40 = t.Type.Score21To40,
                Score41To60 = t.Type.Score41To60,
                Score61To80 = t.Type.Score61To80,
                Score81To100 = t.Type.Score81To100,
            };

            //CHOICE
            var choice = new List<TaskRatingChoices>();
            foreach (var c in t.Choice)
            {
                choice.Add(new TaskRatingChoices
                {
                    TaskId = x,
                    Number = c.Number,
                    Text = c.Text,
                });
            }
            await this.DB.AddAsync(type);
            await this.DB.AddRangeAsync(choice);
            this.DB.SaveChanges();

            return true;
        }
        //UPDATE 
        public async Task<bool> UpdateTaskRating(int taskId, TaskRatingModel t)
        {
            //Variable Pembantu
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            var userId = this._HelperMan.GetLoginUserId();

            //Variable yng ingin di update
            var taskUpdate = await this.DB.Tasks.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var typeUpdate = await this.DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var choice = await this.DB.TaskRatingChoices.Where(Q => Q.TaskId == taskId).ToListAsync();
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
                taskUpdate.QuestionTypeId = 9;
                taskUpdate.LayoutType = t.Task.LayoutType;
                taskUpdate.StoryLineDescription = t.Task.StoryLineDescription;
                taskUpdate.EvaluationTypeId = t.Task.EvaluationTypeId;
                taskUpdate.UpdatedBy = userId;
                taskUpdate.UpdatedAt = datenow;

                //Type
                typeUpdate.Question = t.Type.Question;
                typeUpdate.Score0To20 = t.Type.Score0To20;
                typeUpdate.Score21To40 = t.Type.Score21To40;
                typeUpdate.Score41To60 = t.Type.Score41To60;
                typeUpdate.Score61To80 = t.Type.Score61To80;
                typeUpdate.Score81To100 = t.Type.Score81To100;

                //PICTURE
                //kalau yang di update lebih dikit... maka listnya harus di kurangin dulu 

                if (choice.Count > t.Choice.Count)
                {
                    for (int i = choice.Count - 1; i > t.Choice.Count - 1; i--)
                    {
                        var remove = choice[i];
                        this.DB.TaskRatingChoices.Remove(remove);
                    }
                    //Assign Data
                    for (int i = 0; i < t.Choice.Count; i++)
                    {
                        //TASKTEBAKGAMABRID
                        choice[i].Text = t.Choice[i].Text;
                        choice[i].Number = t.Choice[i].Number;
                        choice[i].TaskId = taskUpdate.TaskId;
                    }
                }

                //kalau yang di update lebih banyak... maka listnya harus di tambahin dulu
                if (choice.Count < t.Choice.Count)
                {
                    //edit yang udah ada
                    for (int i = 0; i < choice.Count - 1; i++)
                    {
                        choice[i].Text = t.Choice[i].Text;
                        choice[i].Number = t.Choice[i].Number;
                        choice[i].TaskId = taskUpdate.TaskId;
                    }
                    //tambahin yang kurang
                    for (int i = choice.Count; i < t.Choice.Count; i++)
                    {
                        var add = new TaskRatingChoices
                        {
                            TaskId = taskId,
                            Text = t.Choice[i].Text,
                            Number = t.Choice[i].Number
                        };
                        await this.DB.AddAsync(add);
                    }
                }

                if (choice.Count == t.Choice.Count)
                {
                    for (int i = 0; i < choice.Count; i++)
                    {
                        //cek kalo blob id ada ato engga di db.
                        choice[i].Text = t.Choice[i].Text;
                        choice[i].Number = t.Choice[i].Number;
                        choice[i].TaskId = taskUpdate.TaskId;
                    }
                }
                await this.DB.SaveChangesAsync();

                return true;
            }

            //SOFT DELETE TASK
            taskUpdate.IsDeleted = true;
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
                BlobId = getUploadBlob,
                //BlobId = t.Task.BlobId,
                CompetencyId = t.Task.CompetencyId,
                QuestionTypeId = 9,
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

            await this.DB.AddAsync(task);
            var x = task.TaskId;
            //TYPE
            var type = new TaskRatingTypes
            {
                TaskId = x,
                Question = t.Type.Question,
                Score0To20 = t.Type.Score0To20,
                Score21To40 = t.Type.Score21To40,
                Score41To60 = t.Type.Score41To60,
                Score61To80 = t.Type.Score61To80,
                Score81To100 = t.Type.Score81To100
            };
            //CHOICE
            var choices = new List<TaskRatingChoices>();
            foreach (var p in t.Choice)
            {
                choices.Add(new TaskRatingChoices
                {
                    TaskId = x,
                    Number = p.Number,
                    Text = p.Text
                });
            }
            await this.DB.AddAsync(type);
            await this.DB.AddRangeAsync(choices);
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskRatingAsync(int id)
        {
            var userId = this._HelperMan.GetLoginUserId();
            var data = await this.DB.Tasks.Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            data.IsDeleted = true;
            data.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            data.UpdatedBy = userId;
            await this.DB.SaveChangesAsync();
            return true;
        }

    }
}
