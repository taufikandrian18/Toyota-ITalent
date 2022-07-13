using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class TaskMultipleChoiceService
    {
        private readonly TalentContext DB;
        private readonly TaskService _TaskMan;
        private readonly IFileStorageService _FileMan;
        private readonly BlobService _BlobMan;
        private readonly ClaimHelper _HelperMan;

        public TaskMultipleChoiceService(TalentContext talentContext, TaskService taskService, IFileStorageService fileService, BlobService blobService, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this._TaskMan = taskService;
            this._FileMan = fileService;
            this._BlobMan = blobService;
            this._HelperMan = claimHelper;
        }

        public async Task<int> CountTaskMultipleChoiceId()
        {
            return await this.DB.TaskMultipleChoiceChoices.CountAsync() + 1;
        }

        public async Task<TaskMultipleChoiceTypeModel> GetTaskMultipleChoiceTypeById(int id)
        {
            var timePoint = await this.DB.TimePoints.ToListAsync();
            var data = await this.DB.TaskMultipleChoiceTypes.Where(Q => Q.TaskId == id).Select(Q => new TaskMultipleChoiceTypeModel
            {
                TaskId = Q.TaskId,
                AnswerId = Q.AnswerId,
                Question = Q.Question,
                Score = Q.Score
            }).FirstOrDefaultAsync();

            data.Score = timePoint.Find(Q => Q.TimePointId == data.Score).Score.GetValueOrDefault();

            //var isAnswer = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskMultipleChoiceChoiceId == data.AnswerId).Select(Q => Q.Number).FirstOrDefaultAsync();
            //data.AnswerId = isAnswer;
            return data;
        }

        public async Task<List<TaskMultipleChoiceChoiceModel>> GetTaskMultipleChoiceChoiceById(int id)
        {

            var data = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskId == id).Select(Q => new TaskMultipleChoiceChoiceModel
            {
                TaskId = Q.TaskId,
                Number = Q.Number,
                TaskMultipleChoiceChoiceId = Q.TaskMultipleChoiceChoiceId,
                Text = Q.Text
            }).ToListAsync();
            return data;
        }

        public async Task<bool> InsertTaskMultipleChoice(TaskMultipleChoiceModel t)
        {
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            var userId = this._HelperMan.GetLoginUserId();
            //var TMCID = await this.CountTaskMultipleChoiceId();
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
                QuestionTypeId = 10,
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

            //CHOICE
            var choice = new List<TaskMultipleChoiceChoices>();
            foreach (var c in t.Choice)
            {
                choice.Add(new TaskMultipleChoiceChoices
                {
                    //TaskMultipleChoiceChoiceId = TMCID,
                    TaskId = x,
                    Number = c.Number,
                    Text = c.Text,
                });
            }
            await this.DB.AddRangeAsync(choice);

            //int isAnswer;
            //isAnswer = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskId == x && Q.Number == t.Type.AnswerId).Select(Q => Q.TaskMultipleChoiceChoiceId).FirstOrDefaultAsync();

            //TYPE
            var type = new TaskMultipleChoiceTypes
            {
                TaskId = x,
                Question = t.Type.Question,
                //AnswerId = isAnswer,
                AnswerId = t.Type.AnswerId,
                Score = t.Type.Score,

            };

            this.DB.Add(type);
            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTaskMultipleChoice(int taskId, TaskMultipleChoiceModel t)
        {
            //Variable Pembantu
            var datenow = DateTime.UtcNow.ToIndonesiaTimeZone();
            var userId = this._HelperMan.GetLoginUserId();

            //Variable yng ingin di update
            var taskUpdate = await this.DB.Tasks.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var typeUpdate = await this.DB.TaskMultipleChoiceTypes.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var choice = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskId == taskId).ToListAsync();
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
                taskUpdate.QuestionTypeId = 10;
                taskUpdate.LayoutType = t.Task.LayoutType;
                taskUpdate.StoryLineDescription = t.Task.StoryLineDescription;
                taskUpdate.EvaluationTypeId = t.Task.EvaluationTypeId;
                taskUpdate.UpdatedBy = userId;
                taskUpdate.UpdatedAt = datenow;

                //Choice
                for (int i = 0; i < 5; i++)
                {
                    choice[i].Text = t.Choice[i].Text;
                    choice[i].Number = t.Choice[i].Number;
                    choice[i].TaskId = taskUpdate.TaskId;
                }

                //int isAnswer;
                //isAnswer = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskId == taskUpdate.TaskId && Q.Number == t.Type.AnswerId).Select(Q => Q.TaskMultipleChoiceChoiceId).FirstOrDefaultAsync();


                //Type
                typeUpdate.Question = t.Type.Question;
                typeUpdate.Score = t.Type.Score;
                //typeUpdate.AnswerId = isAnswer; //Answer
                typeUpdate.AnswerId = t.Type.AnswerId; //Answer


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
                BlobId = getUploadBlob,
                CompetencyId = t.Task.CompetencyId,
                QuestionTypeId = 10,
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

            //CHOICE
            var choices = new List<TaskMultipleChoiceChoices>();
            foreach (var c in t.Choice)
            {
                choices.Add(new TaskMultipleChoiceChoices
                {
                    //TaskMultipleChoiceChoiceId = TMCID,
                    TaskId = x,
                    Number = c.Number,
                    Text = c.Text,
                });
            }

            await this.DB.AddRangeAsync(choices);


            //int isAnswer;
            //isAnswer = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskId == x && Q.Number == t.Type.AnswerId).Select(Q => Q.TaskMultipleChoiceChoiceId).FirstOrDefaultAsync();

            //TYPE
            var type = new TaskMultipleChoiceTypes
            {
                TaskId = x,
                Question = t.Type.Question,
                Score = t.Type.Score,
                //AnswerId = isAnswer //Answer
                AnswerId = t.Type.AnswerId //Answer
            };

            this.DB.Add(type);
            await this.DB.SaveChangesAsync();

            return true;
        }
    }
}
