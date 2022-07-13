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
    public class ChecklistService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;
        private readonly IFileStorageService FileMan;

        public ChecklistService(TalentContext talentContext, ClaimHelper claimHelper, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.HelperMan = claimHelper;
            this.FileMan = fileService;
        }

        public async Task InsertChecklistTask(ChecklistCreateModel model)
        {
            var thisDate = DateTime.UtcNow.AddHours(7);
            var userId = this.HelperMan.GetLoginUserId();
            Guid? getUploadBlob = null;

            if (!String.IsNullOrEmpty(model.FileContent.Base64))
            {
                getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);
            }

            var newTask = new Talent.Entities.Entities.Tasks
            {
                TaskId = model.Task.TaskId,
                CompetencyId = model.Task.CompetencyId,
                QuestionTypeId = 8,
                ModuleId = model.Task.ModuleId,
                EvaluationTypeId = model.Task.EvaluationTypeId,
                BlobId = getUploadBlob,
                TaskNumber = model.Task.TaskNumber,
                LayoutType = model.Task.LayoutType,
                StoryLineDescription = model.Task.StoryLineDescription,
                IsDeleted = false,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            this.DB.Tasks.Add(newTask);


            var newQuestion = new TaskChecklistTypes
            {
                Question = model.Question,
                TaskId = newTask.TaskId
            };

            this.DB.TaskChecklistTypes.Add(newQuestion);

            List<TaskChecklistChoices> newChoices = new List<TaskChecklistChoices>();

            foreach (var choice in model.Choices)
            {
                newChoices.Add(new TaskChecklistChoices
                {
                    TaskId = newTask.TaskId,
                    Number = choice.Number,
                    Text = choice.Text,
                    IsAnswer = choice.IsAnswer,
                    Score = choice.Score
                });
            }

            this.DB.TaskChecklistChoices.AddRange(newChoices);
            await this.DB.SaveChangesAsync();
        }

        public async Task<ChecklistViewDetailModel> GetChecklistTaskById(int taskId)
        {
            var data = await (from t in DB.Tasks
                              join b in DB.Blobs on t.BlobId equals b.BlobId into bNull
                              from b1 in bNull.DefaultIfEmpty()
                              join m in DB.Modules on t.ModuleId equals m.ModuleId
                              join tct in DB.TaskChecklistTypes on t.TaskId equals tct.TaskId
                              join eva in DB.EvaluationTypes on t.EvaluationTypeId equals eva.EvaluationTypeId

                              join com in DB.Competencies on t.CompetencyId equals com.CompetencyId
                              join comtype in DB.CompetencyTypes on com.CompetencyTypeId equals comtype.CompetencyTypeId
                              join comevmap in DB.CompetencyEvaluationMappings on com.CompetencyId equals comevmap.CompetencyId
                              where t.TaskId == taskId
                              select new ChecklistViewDetailModel
                              {
                                  Task = new TaskModel
                                  {
                                      TaskId = t.TaskId,
                                      CompetencyId = t.CompetencyId,
                                      QuestionTypeId = t.QuestionTypeId,
                                      ModuleId = t.ModuleId,
                                      EvaluationTypeId = t.EvaluationTypeId,
                                      BlobId = t.BlobId,
                                      TaskNumber = t.TaskNumber,
                                      LayoutType = t.LayoutType,
                                      StoryLineDescription = t.StoryLineDescription,
                                      IsDeleted = t.IsDeleted,
                                      CreatedAt = t.CreatedAt,
                                      CreatedBy = t.CreatedBy,
                                      UpdatedAt = t.UpdatedAt
                                  },
                                  BlobImageName = b1.Name,
                                  BlobImageFileType = b1.Mime,
                                  Question = tct.Question,
                                  ModuleName = m.ModuleName,
                                  PrefixCode = com.PrefixCode,
                                  CompetencyTypeId = com.CompetencyTypeId,
                                  EvaluationTypeId = eva.EvaluationTypeId,
                                  CompetencyTypeName = comtype.CompetencyTypeName,
                                  EvaluationTypeName = eva.EvaluationTypeName
                              }).FirstOrDefaultAsync();

            data.Choices = await (from tcc in DB.TaskChecklistChoices
                                  join t in DB.TimePoints on tcc.Score equals t.TimePointId
                                  where tcc.TaskId == taskId && t.PointTypeId == 5
                                  select new ChecklistChoiceModel
                                  {
                                      Number = tcc.Number,
                                      Text = tcc.Text,
                                      IsAnswer = tcc.IsAnswer,
                                      Score = t.Score.GetValueOrDefault()
                                  }).ToListAsync();

            data.TotalScore = 0;
            data.TotalPoint = 0;

            var dataTimePoint = await this.DB.TimePoints.Where(Q => Q.PointTypeId == 5).ToListAsync();
            int choicePoint = 0;

            foreach (var choice in data.Choices)
            {
                data.TotalScore += choice.Score;
                choicePoint = dataTimePoint
                    .Where(Q => Q.Score == choice.Score)
                    .Select(Q => Q.Points).FirstOrDefault();

                data.TotalPoint += choicePoint;
            }

            return data;
        }

        public async Task UpdateChecklistTask(ChecklistUpdateModel model, int taskId)
        {
            var thisDate = DateTime.UtcNow.AddHours(7);
            var userId = this.HelperMan.GetLoginUserId();
            Guid? getUploadBlob = null;

            var validateTaskModule = await (from t in DB.Tasks
                                            join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                                            where t.TaskId == taskId
                                            select new
                                            {
                                                t.CompetencyId,
                                                c.CompetencyTypeId,
                                                t.ModuleId,
                                                t.EvaluationTypeId
                                            }).FirstOrDefaultAsync();

            bool isChangeTaskCodeAndModule = true;

            if (validateTaskModule.CompetencyId == model.Task.CompetencyId &&
                validateTaskModule.CompetencyTypeId == model.CompetencyTypeId &&
                validateTaskModule.ModuleId == model.Task.ModuleId &&
                validateTaskModule.EvaluationTypeId == model.Task.EvaluationTypeId)
            {
                isChangeTaskCodeAndModule = false;
            }

            var dataTask = await this.DB.Tasks.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();

            if (!String.IsNullOrEmpty(model.FileContent.Base64))
            {
                getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);
            }

            var newTask = new Talent.Entities.Entities.Tasks
            {
                CompetencyId = model.Task.CompetencyId,
                QuestionTypeId = 3,
                ModuleId = model.Task.ModuleId,
                EvaluationTypeId = model.Task.EvaluationTypeId,
                BlobId = getUploadBlob,
                TaskNumber = model.Task.TaskNumber,
                LayoutType = model.Task.LayoutType,
                StoryLineDescription = model.Task.StoryLineDescription,
                IsDeleted = false,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            if (isChangeTaskCodeAndModule == false)
            {
                dataTask.BlobId = getUploadBlob;
                dataTask.LayoutType = model.Task.LayoutType;
                dataTask.StoryLineDescription = model.Task.StoryLineDescription;
                dataTask.UpdatedAt = thisDate;
                dataTask.UpdatedBy = userId;

                this.DB.Tasks.Update(dataTask);
            }

            else
            {
                dataTask.IsDeleted = true;
                dataTask.UpdatedBy = userId;
                dataTask.UpdatedAt = thisDate;

                this.DB.Tasks.Update(dataTask);

                this.DB.Tasks.Add(newTask);
            }

            var dataTaskType = await this.DB.TaskChecklistTypes.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();

            if (isChangeTaskCodeAndModule == false)
            {
                dataTaskType.Question = model.Question;

                this.DB.TaskChecklistTypes.Update(dataTaskType);
            }

            else
            {
                var newTaskType = new TaskChecklistTypes
                {
                    TaskId = newTask.TaskId,
                    Question = model.Question
                };

                this.DB.TaskChecklistTypes.Add(newTaskType);
            }

            var dataTaskChoice = await this.DB.TaskChecklistChoices.Where(Q => Q.TaskId == taskId).ToListAsync();
            this.DB.TaskChecklistChoices.RemoveRange(dataTaskChoice);
            await this.DB.SaveChangesAsync();

            List<TaskChecklistChoices> choices = new List<TaskChecklistChoices>();

            int insertTaskId = 0;

            if (isChangeTaskCodeAndModule == false)
            {
                insertTaskId = taskId;
            }

            else
            {
                insertTaskId = newTask.TaskId;
            }

            foreach (var choice in model.Choices)
            {
                choices.Add(new TaskChecklistChoices
                {
                    TaskId = insertTaskId,
                    Number = choice.Number,
                    Text = choice.Text,
                    IsAnswer = choice.IsAnswer,
                    Score = choice.Score
                });
            }

            this.DB.TaskChecklistChoices.AddRange(choices);
            await this.DB.SaveChangesAsync();
        }

    }
}
