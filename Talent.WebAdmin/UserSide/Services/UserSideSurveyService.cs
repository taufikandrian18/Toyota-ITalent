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
    public class UserSideSurveyService
    {
        private readonly TalentContext DB;
        private readonly BlobService BlobMan;
        private readonly IFileStorageService FileMan;
        private readonly UserSideTimePointService TimePointMan;

        public UserSideSurveyService(TalentContext talentContext, BlobService blobService, IFileStorageService FileService, UserSideTimePointService userSideTimePointService)
        {
            this.DB = talentContext;
            this.BlobMan = blobService;
            this.FileMan = FileService;
            this.TimePointMan = userSideTimePointService;
        }

        public async Task<string> GenerateImageURL(Guid blobId)
        {
            var blob = await this.BlobMan.GetBlobById(blobId);
            var ImageUrl = await this.FileMan.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
            return ImageUrl;
        }

        //SURVEY
        public async Task<UserSideSurveyInsertModel> GetSurveyQuestionById(int id)
        {
            var data = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyInsertModel
            {
                BlobId = Q.BlobId,
                SurveyQuestionId = Q.SurveyQuestionId,
                SurveyQuestionTypeId = Q.SurveyQuestionTypeId
            }).FirstOrDefaultAsync();

            if (data.BlobId != null)
            {
                data.TaskImageUrl = await this.GenerateImageURL(data.BlobId.GetValueOrDefault());
            }
            return data;
        }

        //1 TRUE FALSE
        //ID = SURVEY QUESTION ID
        public async Task<UserSideSurveyTrueFalseModel> GetSurveyTrueFalseById(int id, UserSideSurveyInsertModel survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyTrueFalseTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideSurveyTrueFalseContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
            };

            return new UserSideSurveyTrueFalseModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = survey.TaskImageUrl,
                Content = content
            };
        }

        //2 MATCHING
        public async Task<UserSideSurveyMatchingModel> GetSurveyMatchingById(int id, UserSideSurveyInsertModel survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyMatchingTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();


            var choicesGet = await this.DB.SurveyMatchingChoices.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyMatchingChoiceGetModel
            {
                BlobId = Q.BlobId,
                SurveyMatchingChoiceCode = Q.SurveyMatchingChoiceCode,
                Text = Q.Text
            }).OrderBy(Q => Q.SurveyMatchingChoiceCode).ToListAsync();

            var tempChoices = new UserSideSurveyMatchingChoiceModel();
            var right = new List<UserSideSurveyMatchingChoiceModel>();
            var left = new List<UserSideSurveyMatchingChoiceModel>();

            var choices = new List<UserSideSurveyMatchingLeftRightChoiceModel>();
            choices.Add(new UserSideSurveyMatchingLeftRightChoiceModel());
            //12/13/2019 1:57 hilang semua kalo delete untuk survey

            foreach (var choice in choicesGet)
            {
                //Check Image or Text
                if (choice.BlobId != null)
                {
                    var imgurl = await this.GenerateImageURL(choice.BlobId.GetValueOrDefault());
                    tempChoices.ImageUrl = imgurl;
                    tempChoices.Number = Int32.Parse(choice.SurveyMatchingChoiceCode.Substring(0, choice.SurveyMatchingChoiceCode.Length - 1));
                }
                else
                {
                    tempChoices.Text = choice.Text;
                    tempChoices.Number = Int32.Parse(choice.SurveyMatchingChoiceCode.Substring(0, choice.SurveyMatchingChoiceCode.Length - 1));
                }

                //Check Left or Right
                if (choice.SurveyMatchingChoiceCode.ElementAt(choice.SurveyMatchingChoiceCode.Length - 1) == 'A')
                {
                    //Check Image or Text
                    if (tempChoices.ImageUrl != null)
                    {
                        left.Add(new UserSideSurveyMatchingChoiceModel
                        {
                            ImageUrl = tempChoices.ImageUrl,
                            Number = tempChoices.Number
                        });
                    }
                    else
                    {
                        left.Add(new UserSideSurveyMatchingChoiceModel
                        {
                            Number = tempChoices.Number,
                            Text = tempChoices.Text
                        });

                    }

                    if (choices.Count == 0)
                    {
                        choices.Add(new UserSideSurveyMatchingLeftRightChoiceModel());
                    }

                    //Assign value to db
                    if (choices[choices.Count - 1].Left != null)
                    {
                        choices.Add(new UserSideSurveyMatchingLeftRightChoiceModel());
                        choices[choices.Count - 1].Left = left[left.Count - 1];
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
                        right.Add(new UserSideSurveyMatchingChoiceModel
                        {
                            ImageUrl = tempChoices.ImageUrl,
                            Number = tempChoices.Number
                        });
                    }
                    else
                    {
                        right.Add(new UserSideSurveyMatchingChoiceModel
                        {
                            Number = tempChoices.Number,
                            Text = tempChoices.Text
                        });
                    }

                    //Assign value to db
                    if (choices[choices.Count - 1].Right != null)
                    {
                        choices.Add(new UserSideSurveyMatchingLeftRightChoiceModel());
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

            var content = new UserSideSurveyMatchingContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
                Choice = choices,
            };

            return new UserSideSurveyMatchingModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                Content = content
            };

        }
        //3 SEQUENCE
        public async Task<UserSideSurveySequenceModel> GetSurveySequenceById(int id, UserSideSurveyInsertModel survey)
        {

            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveySequenceTypeModel
            {
                Question = Q.Question,
            }).FirstOrDefaultAsync();

            var choices = await this.DB.SurveyChoices
                                    .Where(Q => Q.SurveyQuestionId == id)
                                    .OrderBy(Q => Q.SurveyChoiceId)
                                    .Select(Q => new UserSideSurveySequenceChoiceModel

                                    {
                                        Number = Q.SurveyChoiceId,
                                        Text = Q.Choice
                                    })
                                    .ToListAsync();
            //Generate Number

            var number = 1;
            foreach (var c in choices)
            {
                c.Number = number;
                number++;
            }

            var content = new UserSideSurveySequenceContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
                Choice = choices
            };

            return new UserSideSurveySequenceModel
            {
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                Content = content
            };

        }
        //4 TEBAK GAMBAR
        public async Task<UserSideSurveyTebakGambarModel> GetSurveyTebakGambarById(int id, UserSideSurveyInsertModel Survey)
        {

            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyTebakGambarTypesModel
            {
                Question = Q.Question,
            }).FirstOrDefaultAsync();

            var choicesGet = await this.DB.SurveyChoices.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyTebakGambarPicturesGetModel
            {
                BlobId = Q.BlobId.GetValueOrDefault()
            }).ToListAsync();

            var choices = new List<UserSideSurveyTebakGambarPicturesModel>();
            var number = 1;
            foreach (var choice in choicesGet)
            {
                choices.Add(new UserSideSurveyTebakGambarPicturesModel
                {
                    ImageUrl = await this.GenerateImageURL(choice.BlobId),
                    Number = number
                });
                number++;
            }

            var content = new UserSideSurveyTebakGambarContentModel
            {
                QuestionTypeId = Survey.SurveyQuestionTypeId,
                Choice = choices
            };

            return new UserSideSurveyTebakGambarModel
            {
                //Task
                SurveyQuestionId = Survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = Survey.TaskImageUrl,
                Content = content
            };

        }


        //5 HOTSPOT
        public async Task<UserSideSurveyHotSpotModel> GetSurveyHotSpotById(int id, UserSideSurveyInsertModel survey)
        {

            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyHotSpotTypeModel
            {
                Question = Q.Question,
            }).FirstOrDefaultAsync();

            //GetChoiceWithScore
            var choicesGet = await this.DB.SurveyChoices.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyHotSpotAnswerGetModel
            {
                Number = Q.SurveyQuestionId
            }).ToListAsync();

            //GenerateChoiceWithPoint
            var choices = new List<UserSideSurveyHotSpotAnswerModel>();
            for (int i = 0; i < choicesGet.Count; i++)
            {
                choices.Add(new UserSideSurveyHotSpotAnswerModel
                {
                    Number = i + 1,
                });
            }


            var content = new UserSideSurveyHotSpotContentModel
            {
                ImageUrl = survey.TaskImageUrl,
                QuestionTypeId = survey.SurveyQuestionTypeId,
                Choice = choices
            };

            return new UserSideSurveyHotSpotModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                //Content
                Content = content
            };
        }

        //6 SHORT ANSWER
        public async Task<UserSideSurveyShortAnswerModel> GetSurveyShortAnswerById(int id, UserSideSurveyInsertModel survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyShortAnswerTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideSurveyShortAnswerContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
            };

            return new UserSideSurveyShortAnswerModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = survey.TaskImageUrl,
                Content = content
            };
        }

        //7 ESSAY
        public async Task<UserSideSurveyEssayModel> GetSurveyEssayById(int id, UserSideSurveyInsertModel survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyEssayTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideSurveyEssayContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
            };

            return new UserSideSurveyEssayModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = survey.TaskImageUrl,
                Content = content
            };
        }

        //8 CHECKLIST
        public async Task<UserSideSurveyChecklistModel> GetSurveyChecklistById(int id, UserSideSurveyInsertModel survey)
        {

            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyChecklistTypeModel
            {
                Question = Q.Question,
            }).FirstOrDefaultAsync();

            var choices = await this.DB.SurveyChoices.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyChecklistChoiceModel
            {
                Number = Q.SurveyChoiceId,
                Text = Q.Choice
            }).ToListAsync();
            //Generate Number

            var number = 1;
            foreach (var c in choices)
            {
                c.Number = number;
                number++;
            }

            var content = new UserSideSurveyChecklistContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
                Choice = choices
            };

            return new UserSideSurveyChecklistModel
            {
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = survey.TaskImageUrl,
                Content = content
            };

        }

        //9 RATING
        public async Task<UserSideSurveyRatingModel> GetSurveyRatingById(int id, UserSideSurveyInsertModel survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyRatingTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var choices = await this.DB.SurveyChoices.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyRatingChoiceModel
            {
                Number = Q.SurveyChoiceId,
                Text = Q.Choice
            }).ToListAsync();

            var number = 1;
            foreach (var c in choices)
            {
                c.Number = number;
                number++;
            }

            var content = new UserSideSurveyRatingContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
                Choice = choices
            };

            return new UserSideSurveyRatingModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = survey.TaskImageUrl,
                Content = content
            };
        }

        //10 MULTIPLE CHOICE
        public async Task<UserSideSurveyMultipleChoiceModel> GetSurveyMultipleChoiceById(int id, UserSideSurveyInsertModel Survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyMultipleChoiceTypeModel
            {
                Question = Q.Question,
            }).FirstOrDefaultAsync();

            var choices = await this.DB.SurveyChoices.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyMultipleChoiceChoiceModel
            {
                Number = Q.SurveyChoiceId,
                Text = Q.Choice
            }).ToListAsync();

            var number = 1;
            foreach (var c in choices)
            {
                c.Number = number;
                number++;
            }

            var content = new UserSideSurveyMultipleChoiceContentModel
            {
                QuestionTypeId = Survey.SurveyQuestionTypeId,
                Choice = choices
            };

            return new UserSideSurveyMultipleChoiceModel
            {
                SurveyQuestionId = Survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = Survey.TaskImageUrl,
                Content = content
            };

        }
        //11 FILE UPLOAD
        public async Task<UserSideSurveyFileUploadModel> GetSurveyFileUploadById(int id, UserSideSurveyInsertModel survey)
        {
            var type = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id).Select(Q => new UserSideSurveyFileUploadTypeModel
            {
                Question = Q.Question
            }).FirstOrDefaultAsync();

            var content = new UserSideSurveyFileUploadContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
            };

            return new UserSideSurveyFileUploadModel
            {
                //Task
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = type.Question,
                TaskImageUrl = survey.TaskImageUrl,
                Content = content
            };
        }

        //12 MATRIX
        public async Task<UserSideSurveyMatrixModel> GetSurveyMatrixById(int id, UserSideSurveyInsertModel survey)
        {
            //GetScore
            var typeGet = await this.DB.SurveyQuestions.Where(Q => Q.SurveyQuestionId == id)
                                    .Select(Q => new UserSideTaskMatrixTypeModel
                                    {
                                        Question = Q.Question,
                                    })
                                    .FirstOrDefaultAsync();




            var header = await this.DB.SurveyMatrixChoices
            .Where(Q => Q.SurveyQuestionId == id).OrderBy(Q => Q.Number)
            .Select(Q => Q.Text)
            .ToListAsync();

            var question = await this.DB.SurveyMatrixQuestions
            .Where(Q => Q.SurveyQuestionId == id)
            .Select(Q => new UserSideSurveyMatrixQuestionsModel
            {
                Number = Q.Number,
                Question = Q.Question,

            }).OrderBy(Q => Q.Number).ToListAsync();

            var content = new UserSideSurveyMatrixContentModel
            {
                QuestionTypeId = survey.SurveyQuestionTypeId,
                Headers = header,
                Questions = question
            };

            return new UserSideSurveyMatrixModel
            {
                SurveyQuestionId = survey.SurveyQuestionId,
                Question = typeGet.Question,
                Content = content
            };

        }

        public async Task<object> UserSideGetSurveyById(int id)
        {
            var task = await this.GetSurveyQuestionById(id);
            switch (task.SurveyQuestionTypeId)
            {
                //TRYE FALSE
                case 1:
                    var truefalse = await GetSurveyTrueFalseById(id, task);
                    return truefalse;
                //MATCHING
                case 2:
                    return await GetSurveyMatchingById(id, task);
                //SEQUENCE
                case 3:
                    var sequence = await GetSurveySequenceById(id, task);
                    return sequence;
                //TEBAK GAMBAR
                case 4:
                    var tebakgambar = await GetSurveyTebakGambarById(id, task);
                    return tebakgambar;
                //HOTSPOT
                case 5:
                    return await GetSurveyHotSpotById(id, task);
                //SHORT ANSWER
                case 6:
                    var shortanswer = await GetSurveyShortAnswerById(id, task);
                    return shortanswer;
                //ESSAY
                case 7:
                    var essay = await GetSurveyEssayById(id, task);
                    return essay;
                //CHECKLIST
                case 8:
                    var checklist = await GetSurveyChecklistById(id, task);
                    return checklist;
                //RATING
                case 9:
                    var rating = await GetSurveyRatingById(id, task);
                    return rating;
                //MULTIPLE CHOICE
                case 10:
                    var multipleChoice = await GetSurveyMultipleChoiceById(id, task);
                    return multipleChoice;
                //FILE UPLOAD
                case 11:
                    var fileupload = await GetSurveyFileUploadById(id, task);
                    return fileupload;
                //MATRIX
                case 12:
                    return await GetSurveyMatrixById(id, task);
                default:
                    return new object { };
            }
        }
    }
}
