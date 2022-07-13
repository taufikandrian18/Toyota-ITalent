using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class TaskMatrixTypeService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;
        private readonly FileService FileServiceMan;

        public TaskMatrixTypeService(TalentContext db, ClaimHelper claimHelper, FileService fs)
        {
            this.DB = db;
            this.HelperMan = claimHelper;
            this.FileServiceMan = fs;
        }

        public async Task<TaskMatrixTypeFormModel> GetTaskMatrixTypeById(int id)
        {
            var task = await this.DB.Tasks.AsNoTracking().Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            var matrixType = await this.DB.TaskMatrixTypes.AsNoTracking().Where(Q => Q.TaskId == id).FirstOrDefaultAsync();
            var blob = task.BlobId.HasValue ? await this.DB.Blobs.AsNoTracking().Where(Q => Q.BlobId == task.BlobId).FirstOrDefaultAsync() : null;

            var choices = await this.DB.TaskMatrixChoices.Where(Q => Q.TaskId == id).OrderBy(Q => Q.TaskMatrixChoiceId).Select(Q => new TaskMatrixChoiceFormModel
            {
                Text = Q.Text
            }).ToListAsync();

            var questions = await this.DB.TaskMatrixQuestions.Where(Q => Q.TaskId == id).OrderBy(Q => Q.Number).Select(Q => new TaskMatrixQuestionFormModel
            {
                Question = Q.Question
            }).ToListAsync();

            var result = new TaskMatrixTypeFormModel
            {
                TaskId = id,
                CompetencyId = task.CompetencyId,
                QuestionTypeId = task.QuestionTypeId,
                ModuleId = task.ModuleId,
                BlobId = task.BlobId,
                FileName = blob?.Name,
                Mime = blob?.Mime,
                EvaluationTypeId = task.EvaluationTypeId,
                TaskNumber = task.TaskNumber,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                IsDeleted = false,
                Question = matrixType.Question,
                ScoreColumn1 = matrixType.ScoreColumn1,
                ScoreColumn2 = matrixType.ScoreColumn2,
                ScoreColumn3 = matrixType.ScoreColumn3,
                ScoreColumn4 = matrixType.ScoreColumn4,
                ScoreColumn5 = matrixType.ScoreColumn5,
                TaskMatrixChoices = choices,
                TaskMatrixQuestions = questions
            };

            if (result == null)
            {
                result = new TaskMatrixTypeFormModel();
            }

            return result;
        }

        public async Task<int> CreateTaskMatrixType(TaskMatrixTypeFormModel model)
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

            var createModel2 = new TaskMatrixTypes
            {
                TaskId = createModel.TaskId,
                Question = model.Question,
                ScoreColumn1 = model.ScoreColumn1,
                ScoreColumn2 = model.ScoreColumn2,
                ScoreColumn3 = model.ScoreColumn3,
                ScoreColumn4 = model.ScoreColumn4,
                ScoreColumn5 = model.ScoreColumn5
            };

            this.DB.TaskMatrixTypes.Add(createModel2);

            List<TaskMatrixChoices> choices = new List<TaskMatrixChoices>();
            foreach (TaskMatrixChoiceFormModel t in model.TaskMatrixChoices.OrderBy(Q => Q.Number).ToList())
            {
                choices.Add(new TaskMatrixChoices
                {
                    TaskId = createModel.TaskId,
                    Text = t.Text
                });
            }
            this.DB.TaskMatrixChoices.AddRange(choices);

            List<TaskMatrixQuestions> questions = new List<TaskMatrixQuestions>();
            var i = 0;
            foreach (TaskMatrixQuestionFormModel t in model.TaskMatrixQuestions.OrderBy(Q => Q.Number).ToList())
            {
                i++;
                questions.Add(new TaskMatrixQuestions
                {
                    TaskId = createModel.TaskId,
                    Question = t.Question,
                    Number = i
                });
            }
            this.DB.TaskMatrixQuestions.AddRange(questions);
            await this.DB.SaveChangesAsync();

            return createModel.TaskId;
        }

        public async Task<bool> UpdateTaskMatrixType(TaskMatrixTypeFormModel model)
        {
            var userId = this.HelperMan.GetLoginUserId();
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

            if (model.TaskNumber == updateModel.TaskNumber)
            {
                updateModel.CompetencyId = model.CompetencyId;
                updateModel.ModuleId = model.ModuleId;
                updateModel.EvaluationTypeId = model.EvaluationTypeId;
                updateModel.BlobId = model.BlobId;
                updateModel.LayoutType = model.LayoutType;
                updateModel.StoryLineDescription = model.StoryLineDescription;
                updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
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

                var updateModel2 = await this.DB.TaskMatrixTypes.FindAsync(model.TaskId);

                updateModel2.Question = model.Question;
                updateModel2.ScoreColumn1 = model.ScoreColumn1;
                updateModel2.ScoreColumn2 = model.ScoreColumn2;
                updateModel2.ScoreColumn3 = model.ScoreColumn3;
                updateModel2.ScoreColumn4 = model.ScoreColumn4;
                updateModel2.ScoreColumn5 = model.ScoreColumn5;

                // Choices
                var choicesToDelete = await DB.TaskMatrixChoices.AsNoTracking().Where(Q => Q.TaskId == model.TaskId).ToListAsync();
                if (choicesToDelete != null)
                {
                    DB.TaskMatrixChoices.RemoveRange(choicesToDelete);
                }

                List<TaskMatrixChoices> choices = new List<TaskMatrixChoices>();
                foreach (TaskMatrixChoiceFormModel t in model.TaskMatrixChoices.OrderBy(Q => Q.Number).ToList())
                {
                    choices.Add(new TaskMatrixChoices
                    {
                        TaskId = updateModel.TaskId,
                        Text = t.Text
                    });
                }
                this.DB.TaskMatrixChoices.AddRange(choices);

                await this.DB.SaveChangesAsync();

                // Questions
                var questionsToDelete = await DB.TaskMatrixQuestions.AsNoTracking().Where(Q => Q.TaskId == model.TaskId).ToListAsync();
                if (questionsToDelete != null)
                {
                    DB.TaskMatrixQuestions.RemoveRange(questionsToDelete);
                }

                List<TaskMatrixQuestions> questions = new List<TaskMatrixQuestions>();
                var i = 0;
                foreach (TaskMatrixQuestionFormModel t in model.TaskMatrixQuestions.OrderBy(Q => Q.Number).ToList())
                {
                    i++;
                    questions.Add(new TaskMatrixQuestions
                    {
                        TaskId = t.TaskId,
                        Question = t.Question,
                        Number = i
                    });
                }
                this.DB.TaskMatrixQuestions.AddRange(questions);
                await this.DB.SaveChangesAsync();
            }
            else
            {
                var taskNumber = await this.DB.Tasks.Where(Q => Q.CompetencyId == model.CompetencyId && Q.ModuleId == model.ModuleId && Q.EvaluationTypeId == model.EvaluationTypeId).CountAsync() + 1;

                updateModel.IsDeleted = true;
                updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                updateModel.UpdatedBy = userId;

                var createModel = new Tasks
                {
                    CompetencyId = model.CompetencyId,
                    ModuleId = model.ModuleId,
                    EvaluationTypeId = model.EvaluationTypeId,
                    QuestionTypeId = model.QuestionTypeId,
                    BlobId = model.BlobId,
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

                var createModel2 = new TaskMatrixTypes
                {
                    TaskId = createModel.TaskId,
                    Question = model.Question,
                    ScoreColumn1 = model.ScoreColumn1,
                    ScoreColumn2 = model.ScoreColumn2,
                    ScoreColumn3 = model.ScoreColumn3,
                    ScoreColumn4 = model.ScoreColumn4,
                    ScoreColumn5 = model.ScoreColumn5
                };

                this.DB.TaskMatrixTypes.Add(createModel2);

                List<TaskMatrixChoices> choices = new List<TaskMatrixChoices>();
                foreach (TaskMatrixChoiceFormModel t in model.TaskMatrixChoices)
                {
                    choices.Add(new TaskMatrixChoices
                    {
                        TaskId = createModel.TaskId,
                        Text = t.Text
                    });
                }
                this.DB.TaskMatrixChoices.AddRange(choices);
                await this.DB.SaveChangesAsync();

                List<TaskMatrixQuestions> questions = new List<TaskMatrixQuestions>();
                var i = 0;
                foreach (TaskMatrixQuestionFormModel t in model.TaskMatrixQuestions)
                {
                    i++;
                    questions.Add(new TaskMatrixQuestions
                    {
                        TaskId = createModel.TaskId,
                        Question = t.Question,
                        Number = i
                    });
                }
                this.DB.TaskMatrixQuestions.AddRange(questions);
                await this.DB.SaveChangesAsync();
            }

            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            await this.DB.SaveChangesAsync();

            return true;
        }
    }
}
