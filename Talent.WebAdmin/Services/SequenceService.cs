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
    public class SequenceService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelpMan;
        private readonly IFileStorageService FileMan;

        public SequenceService(TalentContext talentContext, ClaimHelper claimHelper, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.HelpMan = claimHelper;
            this.FileMan = fileService;
        }

        public async Task<List<CompetencyMappingJoinModel>> GetTaskCode()
        {
            var datas = await (from com in DB.Competencies
                               join comtype in DB.CompetencyTypes on com.CompetencyTypeId equals comtype.CompetencyTypeId
                               join comevmap in DB.CompetencyEvaluationMappings on com.CompetencyId equals comevmap.CompetencyId
                               join eva in DB.EvaluationTypes on comevmap.EvaluationTypeId equals eva.EvaluationTypeId
                               select new CompetencyMappingJoinModel
                               {
                                   CompetencyId = com.CompetencyId,
                                   EvaluationTypeId = eva.EvaluationTypeId,
                                   CompetencyTypeId = com.CompetencyTypeId,
                                   PrefixCode = com.PrefixCode,
                                   CompetencyTypeName = comtype.CompetencyTypeName,
                                   EvaluationTypeName = eva.EvaluationTypeName
                               }).ToListAsync();

            return datas;
        }

        public async Task<List<ModuleForTaskModel>> GetModuleAsync()
        {
            var data = await this.DB.Modules.Where(Q => Q.IsDeleted == false).Select(Q => new ModuleForTaskModel
            {
                ModuleId = Q.ModuleId,
                ModuleName = Q.ModuleName
            }).ToListAsync();

            return data;
        }

        public async Task<int> GetNumber(GetNumberTaskModel model)
        {
            var query = from t in DB.Tasks
                        join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                        where c.CompetencyId == model.CompetencyId && t.EvaluationTypeId == model.EvaluationTypeId && t.ModuleId == model.ModuleId
                        select new
                        {
                            TaskId = t.TaskId,
                        };

            var countData = await query.CountAsync();
            var totalNumber = countData + 1;
            return totalNumber;
        }

        public async Task<List<TimePointTaskModel>> GetAllTaskTimePoints()
        {
            var result = await this
                .DB
                .TimePoints
                .Where(Q => Q.PointTypeId == 5 && Q.IsDeleted == false)
                .Select(Q => new TimePointTaskModel
                {
                    Points = Q.Points,
                    Score = Q.Score,
                    TimePointId = Q.TimePointId
                }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<int> InsertSequenceTask(SequenceCreateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var userId = this.HelpMan.GetLoginUserId();
            Guid? getUploadBlob = null;

            if (!String.IsNullOrEmpty(model.FileContent.Base64))
            {
                getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);
            }

            var newTask = new Talent.Entities.Entities.Tasks
            {
                TaskId = model.Task.TaskId,
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

            this.DB.Tasks.Add(newTask);

            var newQuestion = new TaskSequenceTypes
            {
                TaskId = newTask.TaskId,
                Question = model.Question,
                Answer = model.Answer
            };

            this.DB.TaskSequenceTypes.Add(newQuestion);

            List<TaskSequenceChoices> choices = new List<TaskSequenceChoices>();

            foreach (var choice in model.Choices)
            {
                choices.Add(new TaskSequenceChoices
                {

                    TaskId = newTask.TaskId,
                    Number = choice.ChoiceNumber,
                    Text = choice.ChoiceText,
                    Score = choice.Score
                });
            }

            this.DB.TaskSequenceChoices.AddRange(choices);

            await this.DB.SaveChangesAsync();

            return 1;
        }

        /// <summary>
        /// Ambil task sequence choice dan type dari task ID.
        /// </summary>
        /// <param name="taskId">Parameter task ID yang dikirim.</param>
        /// <returns>Objek dalam format <seealso cref="SequenceViewDetailModel"/></returns>
        public async Task<SequenceViewDetailModel> GetTaskSequenceById(int taskId)
        {
            var data = await (from t in DB.Tasks
                              join b in DB.Blobs on t.BlobId equals b.BlobId into bNull
                              from b1 in bNull.DefaultIfEmpty()
                              join m in DB.Modules on t.ModuleId equals m.ModuleId
                              join tst in DB.TaskSequenceTypes on t.TaskId equals tst.TaskId

                              join eva in DB.EvaluationTypes on t.EvaluationTypeId equals eva.EvaluationTypeId
                              join com in DB.Competencies on t.CompetencyId equals com.CompetencyId

                              join comtype in DB.CompetencyTypes on com.CompetencyTypeId equals comtype.CompetencyTypeId
                              join comevmap in DB.CompetencyEvaluationMappings on com.CompetencyId equals comevmap.CompetencyId
                              where t.TaskId == taskId
                              select new SequenceViewDetailModel
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
                                  Question = tst.Question,
                                  Answer = tst.Answer,
                                  ModuleName = m.ModuleName,
                                  PrefixCode = com.PrefixCode,
                                  CompetencyTypeId = com.CompetencyTypeId,
                                  EvaluationTypeId = eva.EvaluationTypeId,
                                  CompetencyTypeName = comtype.CompetencyTypeName,
                                  EvaluationTypeName = eva.EvaluationTypeName
                              })
                              .FirstOrDefaultAsync();

            data.Choices = await (from tsc in DB.TaskSequenceChoices
                                  join t in DB.TimePoints on tsc.Score equals t.TimePointId
                                  where tsc.TaskId == taskId
                                  orderby tsc.Number
                                  select new SequenceChoiceModel
                                  {
                                      TaskSequenceChoiceId = tsc.TaskSequenceChoiceId,
                                      ChoiceNumber = tsc.Number,
                                      ChoiceText = tsc.Text,
                                      Score = t.Score.GetValueOrDefault()
                                  })
                                  .ToListAsync();

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

        public async Task<int> UpdateTaskSequenceAsync(SequenceUpdateModel model, int taskId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var userId = this.HelpMan.GetLoginUserId();

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

            var getUploadBlob = model.Task.BlobId;

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
                dataTask.UpdatedAt = thisDate;
                dataTask.UpdatedBy = userId;

                this.DB.Tasks.Update(dataTask);

                this.DB.Tasks.Add(newTask);
                // await this.DB.SaveChangesAsync();
            }

            var dataTaskType = await this.DB.TaskSequenceTypes.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();

            if (isChangeTaskCodeAndModule == false)
            {
                dataTaskType.Question = model.Question;
                dataTaskType.Answer = model.Answer;

                this.DB.TaskSequenceTypes.Update(dataTaskType);
            }
            else
            {
                var newTaskType = new TaskSequenceTypes
                {
                    TaskId = newTask.TaskId,
                    Question = model.Question,
                    Answer = model.Answer
                };

                this.DB.TaskSequenceTypes.Add(newTaskType);
            }

            var dataTaskChoice = await this.DB.TaskSequenceChoices.Where(Q => Q.TaskId == taskId).ToListAsync();
            this.DB.TaskSequenceChoices.RemoveRange(dataTaskChoice);

            List<TaskSequenceChoices> choices = new List<TaskSequenceChoices>();

            int insertTaskId = newTask.TaskId;

            if (isChangeTaskCodeAndModule == false)
            {
                insertTaskId = taskId;
            }

            foreach (var choice in model.Choices)
            {
                choices.Add(new TaskSequenceChoices
                {
                    TaskId = insertTaskId,
                    Number = choice.ChoiceNumber,
                    Text = choice.ChoiceText,
                    Score = choice.Score
                });
            }

            this.DB.TaskSequenceChoices.AddRange(choices);
            await this.DB.SaveChangesAsync();

            return 1;
        }

    }
}
