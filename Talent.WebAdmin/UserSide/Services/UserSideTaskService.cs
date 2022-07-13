using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTaskService
    {
        private readonly TalentContext DB;
        private readonly BlobService BlobMan;
        private readonly IFileStorageService FileMan;
        private readonly UserSideTimePointService TimePointMan;

        public UserSideTaskService(TalentContext talentContext, BlobService blobService, IFileStorageService FileService, UserSideTimePointService userSideTimePointService)
        {
            this.DB = talentContext;
            this.BlobMan = blobService;
            this.FileMan = FileService;
            this.TimePointMan = userSideTimePointService;
        }

        public async Task<List<int>> GetTaskIdsFromSetupModule(int setupModuleId)
        {
            var taskIds = await (from t in DB.Tasks
                                 join stc in DB.SetupTaskCodes on t.TaskId equals stc.TaskId
                                 join st in DB.SetupTasks on stc.SetupTaskId equals st.SetupTaskId
                                 where st.SetupModuleId == setupModuleId
                                 orderby stc.QuestionNumber
                                 select t.TaskId).ToListAsync();

            return taskIds;
        }

        public async Task<List<int>> GetSetupTaskIdsFromPopQuiz(int popQuizId)
        {
            var taskIds = await (from t in DB.Tasks
                                 join stc in DB.SetupTaskCodes on t.TaskId equals stc.TaskId
                                 join st in DB.SetupTasks on stc.SetupTaskId equals st.SetupTaskId
                                 where st.PopQuizId == popQuizId
                                 orderby stc.QuestionNumber
                                 select t.TaskId).ToListAsync();

            return taskIds;
        }

        public async Task<string> GenerateImageURL(Guid blobId)
        {
            var blob = await this.BlobMan.GetBlobById(blobId);
            var ImageUrl = await this.FileMan.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
            return ImageUrl;
        }

        //TASK
        public async Task<UserSideTaskInsertModel> GetTaskById(int id)
        {
            var data = await this.DB.Tasks.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskInsertModel
            {
                BlobId = Q.BlobId,
                TaskId = Q.TaskId,
                LayoutType = Q.LayoutType,
                QuestionTypeId = Q.QuestionTypeId,
                StoryLineDescription = Q.StoryLineDescription
            }).FirstOrDefaultAsync();

            if (data.BlobId != null)
            {
                data.TaskImageUrl = await this.GenerateImageURL(data.BlobId.GetValueOrDefault());
            }
            return data;
        }

        //1-TRUE FALSE
        public async Task<UserSideTaskTrueFalseModel> GetTaskTrueFalseById(int id, UserSideTaskInsertModel task)
        {
            var type = await this.DB.TaskTrueFalseTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskTrueFalseTypeModel
            {
                Question = Q.Question,
                Answer = Q.Answer,
                Score = Q.Score
            }).FirstOrDefaultAsync();

            var timePointId = type.Score;

            type.Score = await this.GetScoreByTimePointId(timePointId);

            var content = new UserSideTaskTrueFalseContentModel
            {
                Answer = type.Answer,
                QuestionTypeId = task.QuestionTypeId,
                Score = type.Score,
                Point = await this.TimePointMan.GetPointByTimePointId(timePointId)
            };

            return new UserSideTaskTrueFalseModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                Content = content
            };
        }

        //2-MATCHING
        public async Task<UserSideTaskMatchingModel> GetTaskMatchingById(int id, UserSideTaskInsertModel task)
        {
            var type = await this.DB.TaskMatchingTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskMatchingTypeModel
            {
                Question = Q.Question,
                Answer = Q.Answer,
                Score = Q.Score
            }).FirstOrDefaultAsync();

            var timePointId = type.Score;

            type.Score = await this.GetScoreByTimePointId(timePointId);

            var point = await this.TimePointMan.GetPointByTimePointId(timePointId);

            var choicesGet = await this.DB.TaskMatchingChoices.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskMatchingChoiceGetModel
            {
                BlobId = Q.BlobId,
                TaskMatchingChoiceCode = Q.TaskMatchingChoiceCode,
                Text = Q.Text
            }).OrderBy(Q=>Q.TaskMatchingChoiceCode).ToListAsync();

            var tempChoices = new UserSideTaskMatchingChoiceModel();
            var right = new List<UserSideTaskMatchingChoiceModel>();
            var left = new List<UserSideTaskMatchingChoiceModel>();

            var choices = new List<UserSideTaskMatchingLeftRightChoiceModel>();
            choices.Add(new UserSideTaskMatchingLeftRightChoiceModel());
            //12/13/2019 1:57 hilang semua kalo delete untuk survey

            foreach (var choice in choicesGet)
            {
                //Check Image or Text
                if (choice.BlobId != null)
                {
                    var imgurl = await this.GenerateImageURL(choice.BlobId.GetValueOrDefault());
                    tempChoices.ImageUrl = imgurl;
                    tempChoices.Number = Int32.Parse(choice.TaskMatchingChoiceCode.Substring(0, choice.TaskMatchingChoiceCode.Length - 1));
                }
                else
                {
                    tempChoices.Text = choice.Text;
                    tempChoices.Number = Int32.Parse(choice.TaskMatchingChoiceCode.Substring(0, choice.TaskMatchingChoiceCode.Length - 1));
                }

                //Check Left or Right
                if (choice.TaskMatchingChoiceCode.ElementAt(choice.TaskMatchingChoiceCode.Length - 1) == 'A')
                {
                    //Check Image or Text
                    if (tempChoices.ImageUrl != null)
                    {
                        left.Add(new UserSideTaskMatchingChoiceModel
                        {
                            ImageUrl = tempChoices.ImageUrl,
                            Number = tempChoices.Number
                        });
                    }
                    else
                    {
                        left.Add(new UserSideTaskMatchingChoiceModel
                        {
                            Number = tempChoices.Number,
                            Text = tempChoices.Text
                        });

                    }

                    if (choices.Count == 0)
                    {
                        choices.Add(new UserSideTaskMatchingLeftRightChoiceModel());
                    }

                    //Assign value to db
                    if (choices[choices.Count - 1].Left != null)
                    {
                        choices.Add(new UserSideTaskMatchingLeftRightChoiceModel());
                        choices[choices.Count -1 ].Left = left[left.Count - 1];
                    }
                    else
                    {
                        choices[choices.Count - 1].Left = left[left.Count - 1];
                    }
                }
                else
                {
                    //Check Image or Text
                    if (tempChoices.ImageUrl != null)
                    {
                        right.Add(new UserSideTaskMatchingChoiceModel
                        {
                            ImageUrl = tempChoices.ImageUrl,
                            Number = tempChoices.Number
                        });
                    }
                    else
                    {
                        right.Add(new UserSideTaskMatchingChoiceModel
                        {
                            Number = tempChoices.Number,
                            Text = tempChoices.Text
                        });
                    }

                    //Assign value to db
                    if (choices[choices.Count - 1].Right != null)
                    {
                        choices.Add(new UserSideTaskMatchingLeftRightChoiceModel());
                        choices[choices.Count - 1].Right = right[right.Count - 1];
                    }
                    else
                    {
                        choices[choices.Count - 1].Right = right[right.Count - 1];
                    }
                }
                tempChoices.ImageUrl = null;
                tempChoices.Text = null;
            }

            var content = new UserSideTaskMatchingContentModel
            {
                QuestionTypeId = task.QuestionTypeId,
                Answer = type.Answer,
                Point = point,
                Score = type.Score,
                Choice = choices,
            };

            return new UserSideTaskMatchingModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                //Content
                Content = content
            };

        }

        //3-SEQUENCE
        public async Task<UserSideTaskSequenceModel> GetTaskSequenceById(int id, UserSideTaskInsertModel task)
        {
            var type = await this.DB.TaskSequenceTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskSequenceTypeModel
            {
                Question = Q.Question,
                Answer = Q.Answer,
            }).FirstOrDefaultAsync();

            var choicesGet = await this.DB.TaskSequenceChoices.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskSequenceChoiceGetModel
            {
                Number = Q.Number,
                Score = Q.Score,
                Text = Q.Text
            }).ToListAsync();

            var choices = new List<UserSideTaskSequenceChoiceModel>();

            foreach (var choice in choicesGet)
            {
                var timePointId = choice.Score;
                choice.Score = await this.GetScoreByTimePointId(timePointId);
                choices.Add(new UserSideTaskSequenceChoiceModel
                {
                    Number = choice.Number,
                    Text = choice.Text,
                    Point = await this.TimePointMan.GetPointByTimePointId(timePointId),
                    Score = choice.Score
                });
            }

            var content = new UserSideTaskSequenceContentModel
            {
                Answer = type.Answer,
                QuestionTypeId = task.QuestionTypeId,
                Choice = choices
            };

            return new UserSideTaskSequenceModel
            {
                //Task
                TaskId = task.TaskId,
                StoryLineDescription = task.StoryLineDescription,
                LayoutType = task.LayoutType,
                Question = type.Question,
                TaskImageUrl = task.TaskImageUrl,
                //Content
                Content = content
            };
        }

        //4-TEBAK GAMBAR
        public async Task<UserSideTaskTebakGambarModel> GetTaskTebakGambarById(int id, UserSideTaskInsertModel task)
        {

            var type = await this.DB.TaskTebakGambarTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskTebakGambarTypesModel
            {
                Question = Q.Question,
                Answer = Q.Answer,
                Score = Q.Score
            }).FirstOrDefaultAsync();

            var timePointId = type.Score;
            type.Score = await this.GetScoreByTimePointId(timePointId);

            var point = await this.TimePointMan.GetPointByTimePointId(timePointId);

            var choicesGet = await this.DB.TaskTebakGambarPictures.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskTebakGambarPicturesGetModel
            {
                Number = Q.Number,
                BlobId = Q.BlobId
            }).ToListAsync();

            var choices = new List<UserSideTaskTebakGambarPicturesModel>();
            foreach (var choice in choicesGet)
            {
                choices.Add(new UserSideTaskTebakGambarPicturesModel
                {
                    ImageUrl = await this.GenerateImageURL(choice.BlobId),
                    Number = choice.Number
                });
            }

            var content = new UserSideTaskTebakGambarContentModel
            {
                QuestionTypeId = task.QuestionTypeId,
                Answer = type.Answer,
                Point = point,
                Score = type.Score,
                Choice = choices
            };

            return new UserSideTaskTebakGambarModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                Content = content
            };

        }

        //5-HOTSPOT
        public async Task<UserSideTaskHotSpotModel> GetTaskHotSpotById(int id, UserSideTaskInsertModel task)
        {

            var type = await this.DB.TaskHotSpotTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskHotSpotTypeModel
            {
                Question = Q.Question,
                BlobId = Q.BlobId
            }).FirstOrDefaultAsync();

            if (type.BlobId != null)
            {
                type.ImageUrl = await this.GenerateImageURL(type.BlobId);
            }

            //GetChoiceWithScore
            var choicesGet = await this.DB.TaskHotSpotAnswers.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskHotSpotAnswerGetModel
            {
                Number = Q.Number,
                Answer = Q.Answer,
                Score = Q.Score
            }).ToListAsync();

            //GenerateChoiceWithPoint
            var choices = new List<UserSideTaskHotSpotAnswerModel>();
            foreach (var choice in choicesGet)
            {
                var timePointId = choice.Score;

                choice.Score = await this.GetScoreByTimePointId(timePointId);
                choices.Add(new UserSideTaskHotSpotAnswerModel
                {
                    Answer = choice.Answer,
                    Number = choice.Number,
                    Point = await this.TimePointMan.GetPointByTimePointId(timePointId),
                    Score = choice.Score
                });
            }

            var content = new UserSideTaskHotSpotContentModel
            {
                ImageUrl = type.ImageUrl,
                QuestionTypeId = task.QuestionTypeId,
                Choice = choices
            };

            return new UserSideTaskHotSpotModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                //Content
                Content = content
            };
        }

        //6-SHORT ANSWER
        public async Task<UserSideTaskShortAnswerModel> GetTaskShortAnswerById(int id, UserSideTaskInsertModel task)
        {

            var type = await this.DB.TaskShortAnswerTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskShortAnswerTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideTaskShortAnswerContentModel
            {
                QuestionTypeId = task.QuestionTypeId
            };

            return new UserSideTaskShortAnswerModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                Content = content
            };

        }

        //7-ESSAY
        public async Task<UserSideTaskEssayModel> GetTaskEssayById(int id, UserSideTaskInsertModel task)
        {
            var type = await this.DB.TaskEssayTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskEssayTypesModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideTaskEssayContentModel
            {
                QuestionTypeId = task.QuestionTypeId,
            };

            return new UserSideTaskEssayModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                //Content
                Content = content
            };

        }

        //8-CHECKLIST
        public async Task<UserSideTaskChecklistModel> GetTaskChecklistById(int id, UserSideTaskInsertModel task)
        {

            var type = await this.DB.TaskChecklistTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskChecklistTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            //Get Choice With Score
            var choicesGet = await this.DB.TaskChecklistChoices.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskChecklistChoiceGetModel
            {
                Text = Q.Text,
                IsAnswer = Q.IsAnswer,
                Number = Q.Number,
                Score = Q.Score
            }).ToListAsync();

            //Generate Point Cara 2 lebih baik
            var choices = new List<UserSideTaskChecklistChoiceModel>();
            foreach (var choice in choicesGet)
            {
                var timePointId = choice.Score;

                choice.Score = await this.GetScoreByTimePointId(timePointId);
                choices.Add(new UserSideTaskChecklistChoiceModel
                {
                    IsAnswer = choice.IsAnswer,
                    Number = choice.Number,
                    Text = choice.Text,
                    Point = await this.TimePointMan.GetPointByTimePointId(timePointId),
                    Score = choice.Score
                });
            }

            var content = new UserSideTaskChecklistContentModel
            {
                //Type
                QuestionTypeId = task.QuestionTypeId,
                //Choice
                Choice = choices
            };

            return new UserSideTaskChecklistModel
            {
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                Content = content,
            };

        }

        //9-RATING
        public async Task<UserSideTaskRatingModel> GetTaskRatingById(int id, UserSideTaskInsertModel task)
        {

            var type = await this.DB.TaskRatingTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskRatingTypeModel
            {
                Question = Q.Question,
                Score0To20 = Q.Score0To20,
                Score21To40 = Q.Score21To40,
                Score41To60 = Q.Score41To60,
                Score61To80 = Q.Score61To80,
                Score81To100 = Q.Score81To100
            }).FirstOrDefaultAsync();

            //Convert id to score
            var timePointId0To20 = type.Score0To20;
            var timePointId21To40 = type.Score21To40;
            var timePointId41To60 = type.Score41To60;
            var timePointId61To80 = type.Score61To80;
            var timePointId81To100 = type.Score81To100;
            type.Score0To20 = await this.GetScoreByTimePointId(timePointId0To20);
            type.Score21To40 = await this.GetScoreByTimePointId(timePointId21To40);
            type.Score41To60 = await this.GetScoreByTimePointId(timePointId41To60);
            type.Score61To80 = await this.GetScoreByTimePointId(timePointId61To80);
            type.Score81To100 = await this.GetScoreByTimePointId(timePointId81To100);

            var choices = await this.DB.TaskRatingChoices.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskRatingChoicesModel
            {
                Number = Q.Number,
                Text = Q.Text
            }).ToListAsync();

            var content = new UserSideTaskRatingContentModel
            {
                QuestionTypeId = task.QuestionTypeId,
                Score0To20 = type.Score0To20,
                Score21To40 = type.Score21To40,
                Score41To60 = type.Score41To60,
                Score61To80 = type.Score61To80,
                Score81To100 = type.Score81To100,
                Point0To20 = await this.TimePointMan.GetPointByTimePointId(timePointId0To20),
                Point21To40 = await this.TimePointMan.GetPointByTimePointId(timePointId21To40),
                Point41To60 = await this.TimePointMan.GetPointByTimePointId(timePointId41To60),
                Point61To80 = await this.TimePointMan.GetPointByTimePointId(timePointId61To80),
                Point81To100 = await this.TimePointMan.GetPointByTimePointId(timePointId81To100),
                Choice = choices
            };

            return new UserSideTaskRatingModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                //Content
                Content = content
            };

        }

        //10-MULTIPLE CHOICE
        public async Task<UserSideTaskMultipleChoiceModel> GetTaskMultipleChoiceById(int id, UserSideTaskInsertModel task)
        {
            var type = await this.DB.TaskMultipleChoiceTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskMultipleChoiceTypeModel
            {
                AnswerId = Q.AnswerId,
                Question = Q.Question,
                Score = Q.Score
            }).FirstOrDefaultAsync();

            var timePointId = type.Score;
            type.Score = await this.GetScoreByTimePointId(timePointId);
            var point = await this.TimePointMan.GetPointByTimePointId(timePointId);

            var choices = await this.DB.TaskMultipleChoiceChoices.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskMultipleChoiceChoiceModel
            {
                Number = Q.Number,
                TaskMultipleChoiceChoiceId = Q.TaskMultipleChoiceChoiceId,
                Text = Q.Text
            }).ToListAsync();

            var content = new UserSideTaskMultipleChoiceContentModel
            {
                Answer = type.AnswerId,
                Point = point,
                Score = type.Score,
                QuestionTypeId = task.QuestionTypeId,
                Choice = choices
            };

            return new UserSideTaskMultipleChoiceModel
            {
                //Task
                TaskId = task.TaskId,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                Question = type.Question,
                //Content
                Content = content
            };

        }

        //11-FILE UPLOAD
        public async Task<UserSideTaskFileUploadModel> GetTaskFileUploadById(int id, UserSideTaskInsertModel task)
        {
            var type = await this.DB.TaskFileUploadTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskFileUploadTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideTaskFileUploadContentModel
            {
                QuestionTypeId = task.QuestionTypeId
            };

            return new UserSideTaskFileUploadModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                //Type
                Content = content
            };

        }

        //12-MATRIX
        public async Task<UserSideTaskMatrixModel> GetTaskMatrixById(int id, UserSideTaskInsertModel task)
        {
            //GetScore
            var typeGet = await this.DB.TaskMatrixTypes.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskMatrixTypeGetModel
            {
                Question = Q.Question,
                ScoreColumn1 = Q.ScoreColumn1,
                ScoreColumn2 = Q.ScoreColumn2,
                ScoreColumn3 = Q.ScoreColumn3,
                ScoreColumn4 = Q.ScoreColumn4,
                ScoreColumn5 = Q.ScoreColumn5
            }).FirstOrDefaultAsync();

            //GetScore
            var timePointIdColumn1 = typeGet.ScoreColumn1;
            var timePointIdColumn2 = typeGet.ScoreColumn2;
            var timePointIdColumn3 = typeGet.ScoreColumn3;
            var timePointIdColumn4 = typeGet.ScoreColumn4;
            var timePointIdColumn5 = typeGet.ScoreColumn5;
            typeGet.ScoreColumn1 = await this.GetScoreByTimePointId(timePointIdColumn1);
            typeGet.ScoreColumn2 = await this.GetScoreByTimePointId(timePointIdColumn2);
            typeGet.ScoreColumn3 = await this.GetScoreByTimePointId(timePointIdColumn3);
            typeGet.ScoreColumn4 = await this.GetScoreByTimePointId(timePointIdColumn4);
            typeGet.ScoreColumn5 = await this.GetScoreByTimePointId(timePointIdColumn5);

            //GetPoint
            var type = new UserSideTaskMatrixTypeModel
            {
                Question = typeGet.Question,
                PointColumn1 = await this.TimePointMan.GetPointByTimePointId(timePointIdColumn1),
                PointColumn2 = await this.TimePointMan.GetPointByTimePointId(timePointIdColumn2),
                PointColumn3 = await this.TimePointMan.GetPointByTimePointId(timePointIdColumn3),
                PointColumn4 = await this.TimePointMan.GetPointByTimePointId(timePointIdColumn4),
                PointColumn5 = await this.TimePointMan.GetPointByTimePointId(timePointIdColumn5),
            };

            var header = await this.DB.TaskMatrixChoices.Where(Q => Q.TaskId == id).Select(Q => Q.Text).ToListAsync();

            var question = await this.DB.TaskMatrixQuestions.Where(Q => Q.TaskId == id).Select(Q => new UserSideTaskMatrixQuestionsModel
            {
                Number = Q.Number,
                Question = Q.Question
            }).ToListAsync();

            var content = new UserSideTaskMatrixContentModel
            {
                QuestionTypeId = task.QuestionTypeId,

                PointColumn1 = type.PointColumn1,
                PointColumn2 = type.PointColumn2,
                PointColumn3 = type.PointColumn3,
                PointColumn4 = type.PointColumn4,
                PointColumn5 = type.PointColumn5,
                ScoreColumn1 = typeGet.ScoreColumn1,
                ScoreColumn2 = typeGet.ScoreColumn2,
                ScoreColumn3 = typeGet.ScoreColumn3,
                ScoreColumn4 = typeGet.ScoreColumn4,
                ScoreColumn5 = typeGet.ScoreColumn5,
                Headers = header,
                Questions = question
            };

            return new UserSideTaskMatrixModel
            {
                //Task
                TaskId = task.TaskId,
                Question = type.Question,
                LayoutType = task.LayoutType,
                StoryLineDescription = task.StoryLineDescription,
                TaskImageUrl = task.TaskImageUrl,
                //Content
                Content = content
            };

        }

        //SERVICE
        public async Task<object> UserSideGetTaskById(int id)
        {
            var task = await this.GetTaskById(id);
            switch (task.QuestionTypeId)
            {
                //TRYE FALSE
                case 1:
                    var truefalse = await GetTaskTrueFalseById(id, task);
                    return truefalse;
                //MATCHING
                case 2:
                    var matching = await GetTaskMatchingById(id, task);
                    return matching;
                //SEQUENCE
                case 3:
                    var sequence = await GetTaskSequenceById(id, task);
                    return sequence;
                //TEBAK GAMBAR
                case 4:
                    var tebakgambar = await GetTaskTebakGambarById(id, task);
                    return tebakgambar;
                //HOTSPOT
                case 5:
                    var hotspot = await GetTaskHotSpotById(id, task);
                    return hotspot;
                //SHORT ANSWER
                case 6:
                    var shortanswer = await GetTaskShortAnswerById(id, task);
                    return shortanswer;
                //ESSAY
                case 7:
                    var essay = await GetTaskEssayById(id, task);
                    return essay;
                //CHECKLIST
                case 8:
                    var checklist = await GetTaskChecklistById(id, task);
                    return checklist;
                //RATING
                case 9:
                    var rating = await GetTaskRatingById(id, task);
                    return rating;
                //MULTIPLE CHOICE
                case 10:
                    var multipleChoice = await GetTaskMultipleChoiceById(id, task);
                    return multipleChoice;
                //FILE UPLOAD
                case 11:
                    var fileupload = await GetTaskFileUploadById(id, task);
                    return fileupload;
                //MATRIX
                case 12:
                    var matrix = await GetTaskMatrixById(id, task);
                    return matrix;
                default:
                    return new object { };
            }
        }

        //Get Score and Point by Id
        public async Task<int> GetScoreByTimePointId(int id)
        {
            var data = await this.DB.TimePoints.Where(Q => Q.TimePointId == id).Select(Q => Q.Score.GetValueOrDefault()).FirstOrDefaultAsync();
            return data;
        }
    }
}
