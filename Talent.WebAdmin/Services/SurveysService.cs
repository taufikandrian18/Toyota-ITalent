using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Services
{
    public class SurveysService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private readonly IFileStorageService FileMan;
        private readonly ApprovalService ApprovalMan;

        public SurveysService(TalentContext talentContext, ClaimHelper claimHelper, IFileStorageService fileStorageService, ApprovalService approvalService)
        {
            this.DB = talentContext;
            this.ClaimMan = claimHelper;
            this.FileMan = fileStorageService;
            this.ApprovalMan = approvalService;
        }
        //12/4/2019/3:24 survey hapus semua
        public async Task<SurveysPaginate> GetAllSurveyPaginateAsync(SurveysGridFilter filter)
        {
            var query = (from s in this.DB.Surveys
                             // join spm in this.DB.SurveyPositionMappings on s.SurveyId equals spm.SurveyId
                             // join p in this.DB.Positions on spm.PositionId equals p.PositionId
                             // join som in this.DB.SurveyOutletMappings on s.SurveyId equals som.SurveyId
                             // join o in this.DB.Outlets on som.OutletId equals o.OutletId
                         join ats in this.DB.ApprovalToSurveys on s.SurveyId equals ats.SurveyId
                         join a in this.DB.Approvals on ats.ApprovalId equals a.ApprovalId
                         join ast in this.DB.ApprovalStatus on a.ApprovalStatusId equals ast.ApprovalStatusId
                         where s.IsDeleted == false
                         select new SurveysModel
                         {
                             SurveyId = s.SurveyId,
                             Title = s.Title,
                             // Position = p.PositionName,
                             // Outlet = o.Name,
                             StatusId = ast.ApprovalStatusId,
                             Status = ast.ApprovalName,
                             CreatedAt = s.CreatedAt,
                             UpdatedAt = s.UpdatedAt,
                             IsDeleted = s.IsDeleted
                         }).AsNoTracking().AsQueryable();

            //search
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (filter.Title != null)
            {
                query = query.Where(Q => Q.Title.Contains(filter.Title));
            }

            if (filter.Position != null)
            {
                query = query.Where(Q => Q.Position.Contains(filter.Position));
            }

            if (filter.Outlet != null)
            {
                query = query.Where(Q => Q.Outlet.Contains(filter.Outlet));
            }

            if (filter.StatusId != null)
            {
                query = query.Where(Q => Q.StatusId == filter.StatusId);
            }

            //sort
            switch (filter.SortBy)
            {
                case "TitleAsc":
                    query = query.OrderBy(Q => Q.Title);
                    break;
                //case "PositionAsc":
                //    query = query.OrderBy(Q => Q.Position);
                //    break;
                //case "OutletAsc":
                //    query = query.OrderBy(Q => Q.Outlet);
                //    break;
                case "StatusAsc":
                    query = query.OrderBy(Q => Q.Status);
                    break;
                case "CreatedDateAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedDateAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;

                case "TitleDesc":
                    query = query.OrderByDescending(Q => Q.Title);
                    break;
                //case "PositionDesc":
                //    query = query.OrderByDescending(Q => Q.Position);
                //    break;
                //case "OutletDesc":
                //    query = query.OrderByDescending(Q => Q.Outlet);
                //    break;
                case "StatusDesc":
                    query = query.OrderByDescending(Q => Q.Status);
                    break;
                case "CreatedDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var surveys = await query.Where(Q => Q.IsDeleted == false).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.Where(Q => Q.IsDeleted == false).CountAsync();

            return new SurveysPaginate
            {
                Data = surveys,
                TotalData = totalData
            };
        }

        public async Task<List<ApprovalStatusModel>> GetAllApprovalStatus()
        {
            var data = await this.DB.ApprovalStatus.Select(Q => new ApprovalStatusModel
            {
                ApprovalId = Q.ApprovalStatusId,
                ApprovalName = Q.ApprovalName
            }).ToListAsync();

            return data;
        }

        private async Task<SurveyMatchingChoices> ProcessSubmitSurveyMatchingChoice(SurveyMatchingChoiceFormModel surveyMatchingChoice, int surveyQuestionId)
        {
            var currentBlobId = surveyMatchingChoice.BlobId;

            if (surveyMatchingChoice.Type == "image")
            {
                var choiceImage = surveyMatchingChoice.ImageUpload;

                // No previous image, new image coming
                if (currentBlobId == null && choiceImage?.Base64 != null)
                {
                    currentBlobId = await FileMan.UploadFileFromBase64(choiceImage);
                }
                // Old Image exists, new image coming
                else if (currentBlobId != null && choiceImage?.Base64 != null)
                {
                    var blob = await this.DB.Blobs.Where(Q => Q.BlobId == currentBlobId).FirstOrDefaultAsync();
                    if (blob != null)
                    {
                        await FileMan.DeleteFileAsync(currentBlobId.GetValueOrDefault(), blob.Mime);
                    }
                    currentBlobId = await FileMan.UploadFileFromBase64(choiceImage);
                }

                surveyMatchingChoice.Text = null;
            }
            else
            {
                if (currentBlobId != null)
                {
                    var blob = await this.DB.Blobs.Where(Q => Q.BlobId == currentBlobId).FirstOrDefaultAsync();
                    if (blob != null)
                    {
                        await FileMan.DeleteFileAsync(currentBlobId.GetValueOrDefault(), blob.Mime);
                    }
                    currentBlobId = null;
                }
            }
            return new SurveyMatchingChoices
            {
                BlobId = currentBlobId,
                SurveyMatchingChoiceCode = surveyMatchingChoice.SurveyMatchingChoiceCode,
                SurveyQuestionId = surveyQuestionId,
                Text = surveyMatchingChoice.Text,
            };
        }
        public async Task<bool> SubmitSurvey(SurveyInsert insert)
        {
            //Surveys
            var surveys = new Surveys
            {
                Title = insert.Title,
                StartDate = insert.StartDate.ToIndonesiaTimeZone(),
                EndDate = insert.EndDate.ToIndonesiaTimeZone().AddHours(23).AddMinutes(59).AddSeconds(59),
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
                IsDeleted = false
            };

            this.DB.Add(surveys);

            //Survey Questions
            var surveyQuestions = new List<SurveyQuestions>();

            //Survey Choices
            var surveyChoices = new List<SurveyChoices>();

            foreach (var q in insert.Question.Select((value, i) => new { i, value }))
            {
                //Upload Question Image if Exist
                var questionFileContent = q.value.FileContent;
                Guid? questionFileBlobId = q.value.BlobId;
                if (string.IsNullOrEmpty(questionFileContent?.Base64) == false)
                {
                    if (questionFileBlobId != null)
                    {
                        await FileMan.DeleteFileAsync(questionFileBlobId.GetValueOrDefault(), questionFileContent.ContentType);
                    }

                    questionFileBlobId = await FileMan.UploadFileFromBase64(questionFileContent);
                }
                var sq = new SurveyQuestions
                {
                    SurveyId = surveys.SurveyId,
                    Question = q.value.Question,
                    QuestionNumber = q.value.QuestionNumber,
                    SurveyQuestionTypeId = q.value.SurveyQuestionTypeId,
                    BlobId = questionFileBlobId
                };
                this.DB.Add(sq);

                if (q.value.SurveyQuestionTypeId == 12)
                {
                    var listNewMatrixQuestions = new List<SurveyMatrixQuestions>();
                    var listNewMatrixChoices = new List<SurveyMatrixChoices>();

                    //Taking Matrix Options
                    var insertMatrixOptions = q.value.MatrixChoice;
                    var insertMatrixQuestion = insertMatrixOptions.MatrixQuestion;
                    var insertMatrixChoices = insertMatrixOptions.MatrixChoice.OrderBy(Q => Q.Number).ToList();

                    foreach (var question in insertMatrixQuestion)
                    {
                        var newQuestion = new SurveyMatrixQuestions
                        {
                            Number = question.Number,
                            Question = question.Question,
                            SurveyQuestionId = sq.SurveyQuestionId
                        };

                        listNewMatrixQuestions.Add(newQuestion);
                    }

                    this.DB.AddRange(listNewMatrixQuestions);

                    foreach (var choice in insertMatrixChoices)
                    {
                        var newChoice = new SurveyMatrixChoices
                        {
                            SurveyQuestionId = sq.SurveyQuestionId,
                            Text = choice.Text,
                            Number = choice.Number
                        };

                        listNewMatrixChoices.Add(newChoice);
                    }
                    this.DB.AddRange(listNewMatrixChoices);
                }
                else if (q.value.SurveyQuestionTypeId == 2)
                {
                    //var currentMatching = this.DB.SurveyQuestions.
                    var matchingChoices = q.value.MatchingChoices;

                    List<SurveyMatchingChoices> matchingChoicesToDB = new List<SurveyMatchingChoices>();
                    for (int i = 0; i < q.value.MatchingChoices.LeftChoices.Count; i++)
                    {
                        var leftChoice = q.value.MatchingChoices.LeftChoices[i];
                        var rightChoice = q.value.MatchingChoices.RightChoices[i];

                        matchingChoicesToDB.Add(await ProcessSubmitSurveyMatchingChoice(leftChoice, sq.SurveyQuestionId));
                        matchingChoicesToDB.Add(await ProcessSubmitSurveyMatchingChoice(rightChoice, sq.SurveyQuestionId));

                    }
                    this.DB.SurveyMatchingChoices.AddRange(matchingChoicesToDB);
                }
                else
                {
                    foreach (var c in q.value.Choice.Select((value2, j) => new { j, value2 }))
                    {
                        var choiceModel = insert.Question[q.i].Choice[c.j];
                        Guid? getUploadBlob = choiceModel.BlobId;

                        if (string.IsNullOrEmpty(choiceModel.FileContent?.Base64) == false)
                        {
                            if (choiceModel.FileContent != null)
                            {
                                if (getUploadBlob != null)
                                {
                                    await FileMan.DeleteFileAsync(getUploadBlob.GetValueOrDefault(), choiceModel.FileContent.ContentType);
                                }

                                getUploadBlob = await FileMan.UploadFileFromBase64(choiceModel.FileContent);
                            }
                        }

                        var sc = new SurveyChoices
                        {
                            SurveyQuestionId = sq.SurveyQuestionId,
                            Choice = c.value2.Choice,
                            BlobId = getUploadBlob,
                        };
                        surveyChoices.Add(sc);
                    }
                }
                surveyQuestions.Add(sq);
            }

            this.DB.AddRange(surveyChoices);

            //Position
            var surveyPosition = new List<SurveyPositionMappings>();
            foreach (var p in insert.Position)
            {
                var position = new SurveyPositionMappings
                {
                    SurveyId = surveys.SurveyId,
                    PositionId = p.PositionId
                };
                surveyPosition.Add(position);
            }

            this.DB.AddRange(surveyPosition);

            //Outlet
            var surveyOutlet = new List<SurveyOutletMappings>();
            foreach (var o in insert.Outlet)
            {
                var outlet = new SurveyOutletMappings
                {
                    SurveyId = surveys.SurveyId,
                    OutletId = o.OutletId
                };
                surveyOutlet.Add(outlet);
            }

            this.DB.AddRange(surveyOutlet);

            var newApproval = new ApprovalCreateFormModel
            {
                ApprovalStatusId = ApprovalStatusesEnum.ApproveId,
                ContentCategory = ContentCategoryEnums.Survey,
                ContentId = surveys.SurveyId,
                ContentName = surveys.Title,
                PageEnum = PageEnums.Survey
            };

            var datenow = DateTime.UtcNow;

            var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

            if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                surveys.ApprovedAt = datenow;
            }

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveSurvey(SurveyInsert insert)
        {
            //Surveys
            var surveys = new Surveys
            {
                Title = insert.Title,
                StartDate = insert.StartDate.ToIndonesiaTimeZone(),
                EndDate = insert.EndDate.ToIndonesiaTimeZone().AddHours(23).AddMinutes(59).AddSeconds(59),
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
                IsDeleted = false
            };

            this.DB.Add(surveys);

            //Survey Questions
            var surveyQuestions = new List<SurveyQuestions>();

            //Survey Choices
            var surveyChoices = new List<SurveyChoices>();

            foreach (var q in insert.Question.Select((value, i) => new { i, value }))
            {
                //Upload Question Image if Exist
                var questionFileContent = q.value.FileContent;
                Guid? questionFileBlobId = q.value.BlobId;
                if (string.IsNullOrEmpty(questionFileContent?.Base64) == false)
                {
                    if (questionFileBlobId != null)
                    {
                        await FileMan.DeleteFileAsync(questionFileBlobId.GetValueOrDefault(), questionFileContent.ContentType);
                    }

                    questionFileBlobId = await FileMan.UploadFileFromBase64(questionFileContent);
                }

                var sq = new SurveyQuestions
                {
                    SurveyId = surveys.SurveyId,
                    Question = q.value.Question,
                    QuestionNumber = q.value.QuestionNumber,
                    SurveyQuestionTypeId = q.value.SurveyQuestionTypeId,
                    BlobId = questionFileBlobId
                };

                this.DB.Add(sq);

                //Do something here, untuk memisahkan matrix dan matching?

                if (q.value.SurveyQuestionTypeId == 12)
                {
                    var listNewMatrixQuestions = new List<SurveyMatrixQuestions>();
                    var listNewMatrixChoices = new List<SurveyMatrixChoices>();

                    //Taking Matrix Options
                    var insertMatrixOptions = q.value.MatrixChoice;
                    var insertMatrixQuestion = insertMatrixOptions.MatrixQuestion;
                    var insertMatrixChoices = insertMatrixOptions.MatrixChoice.OrderBy(Q => Q.Number).ToList();

                    foreach (var question in insertMatrixQuestion)
                    {
                        var newQuestion = new SurveyMatrixQuestions
                        {
                            Number = question.Number,
                            Question = question.Question,
                            SurveyQuestionId = sq.SurveyQuestionId
                        };

                        listNewMatrixQuestions.Add(newQuestion);
                    }

                    this.DB.AddRange(listNewMatrixQuestions);

                    foreach (var choice in insertMatrixChoices)
                    {
                        var newChoice = new SurveyMatrixChoices
                        {
                            SurveyQuestionId = sq.SurveyQuestionId,
                            Text = choice.Text,
                            Number = choice.Number
                        };

                        listNewMatrixChoices.Add(newChoice);
                    }

                    this.DB.AddRange(listNewMatrixChoices);
                }
                else if (q.value.SurveyQuestionTypeId == 2)
                {
                    //var currentMatching = this.DB.SurveyQuestions.
                    var matchingChoices = q.value.MatchingChoices;

                    List<SurveyMatchingChoices> matchingChoicesToDB = new List<SurveyMatchingChoices>();
                    for (int i = 0; i < q.value.MatchingChoices.LeftChoices.Count; i++)
                    {
                        var leftChoice = q.value.MatchingChoices.LeftChoices[i];
                        var rightChoice = q.value.MatchingChoices.RightChoices[i];

                        matchingChoicesToDB.Add(await ProcessSubmitSurveyMatchingChoice(leftChoice, sq.SurveyQuestionId));
                        matchingChoicesToDB.Add(await ProcessSubmitSurveyMatchingChoice(rightChoice, sq.SurveyQuestionId));

                    }
                    this.DB.SurveyMatchingChoices.AddRange(matchingChoicesToDB);
                }
                else
                {
                    foreach (var c in q.value.Choice.Select((value2, j) => new { j, value2 }))
                    {
                        var choiceModel = insert.Question[q.i].Choice[c.j];
                        Guid? getUploadBlob = choiceModel.BlobId;
                        if (string.IsNullOrEmpty(choiceModel.FileContent?.Base64) == false)
                        {
                            if (getUploadBlob != null)
                            {
                                await FileMan.DeleteFileAsync(getUploadBlob.GetValueOrDefault(), choiceModel.FileContent.ContentType);
                            }

                            getUploadBlob = await FileMan.UploadFileFromBase64(choiceModel.FileContent);
                        }

                        var sc = new SurveyChoices
                        {
                            SurveyQuestionId = sq.SurveyQuestionId,
                            Choice = c.value2.Choice,
                            BlobId = getUploadBlob,
                        };
                        surveyChoices.Add(sc);
                    }
                }
                surveyQuestions.Add(sq);
            }

            this.DB.AddRange(surveyChoices);
            //Position
            var surveyPosition = new List<SurveyPositionMappings>();
            foreach (var p in insert.Position)
            {
                var position = new SurveyPositionMappings
                {
                    SurveyId = surveys.SurveyId,
                    PositionId = p.PositionId
                };
                surveyPosition.Add(position);
            }

            this.DB.AddRange(surveyPosition);

            //Outlet
            var surveyOutlet = new List<SurveyOutletMappings>();
            foreach (var o in insert.Outlet)
            {
                var outlet = new SurveyOutletMappings
                {
                    SurveyId = surveys.SurveyId,
                    OutletId = o.OutletId
                };
                surveyOutlet.Add(outlet);
            }

            this.DB.AddRange(surveyOutlet);

            var datenow = DateTime.UtcNow;

            var newApproval = new ApprovalCreateFormModel
            {
                ApprovalStatusId = ApprovalStatusesEnum.DraftId,
                ContentCategory = ContentCategoryEnums.Survey,
                ContentId = surveys.SurveyId,
                ContentName = surveys.Title,
                PageEnum = PageEnums.Survey
            };

            var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

            await this.DB.SaveChangesAsync();

            return true;
        }


        //logic soft delete
        //submit edit survey
        public async Task<bool> SubmitEditSurvey(int surveyId, SurveyInsert insert)
        {
            var submit = await SubmitSurvey(insert);

            if (submit == false)
            {
                return false;
            }

            var deleteSurvey = await DeleteSurveyById(surveyId);

            if (deleteSurvey == false)
            {
                return false;
            }


            return true;
        }

        //SaveEditSurvey
        public async Task<bool> SaveEditSurvey(SurveyInsert insert, int surveyId)
        {
            var save = await SaveSurvey(insert);

            if (save == false)
            {
                return false;
            }

            var deleteSurvey = await DeleteSurveyById(surveyId);

            if (deleteSurvey == false)
            {
                return false;
            }

            return true;
        }

        public async Task<List<SurveyQuestionType>> GetAllSurveyQuestionType()
        {
            var data = await this.DB.SurveyQuestionTypes.Select(Q => new SurveyQuestionType
            {
                Name = Q.Name,
                SurveyQuestionTypeId = Q.SurveyQuestionTypeId
            }).ToListAsync();
            return data;
        }

        public async Task<SurveyInitialize> GetSurveyById(int surveyId)
        {
            //Suvey
            var survey = await (from s in this.DB.Surveys
                                join sq in this.DB.SurveyQuestions on s.SurveyId equals sq.SurveyId

                                join sc in this.DB.SurveyChoices on sq.SurveyQuestionId equals sc.SurveyQuestionId into scnull
                                from sc in scnull.DefaultIfEmpty()

                                join smc in this.DB.SurveyMatrixChoices on sq.SurveyQuestionId equals smc.SurveyQuestionId into lsmc
                                from smc in lsmc.DefaultIfEmpty()

                                join smq in this.DB.SurveyMatrixQuestions on sq.SurveyQuestionId equals smq.SurveyQuestionId into lsmq
                                from smq in lsmq.DefaultIfEmpty()

                                join b in this.DB.Blobs on sc.BlobId equals b.BlobId into bsc
                                from b in bsc.DefaultIfEmpty()

                                join qb in this.DB.Blobs on sq.BlobId equals qb.BlobId into lbq
                                from qb in lbq.DefaultIfEmpty()

                                where s.SurveyId == surveyId
                                select (new SurveyGet
                                {
                                    SurveyId = s.SurveyId,
                                    Title = s.Title,
                                    StartDate = s.StartDate.GetValueOrDefault(),
                                    EndDate = s.EndDate.GetValueOrDefault(),
                                    SurveyQuestionId = sq.SurveyQuestionId,
                                    SurveyQuestionTypeId = sq.SurveyQuestionTypeId,
                                    QuestionNumber = sq.QuestionNumber,
                                    Question = sq.Question,
                                    QuestionBlobId = sq.BlobId,
                                    SurveyChoiceId = sc.SurveyChoiceId,
                                    Choice = sc.Choice,
                                    ChoiceBlobId = sc.BlobId,
                                    FileName = b.Name,
                                    Mime = b.Mime,
                                    NumberMatrix = smq.Number,
                                    NumberMatrixChoice = smc.Number,
                                    QuestionMatrix = smq.Question,
                                    SurveyMatrixChoiceId = smc.SurveyMatrixChoiceId,
                                    SurveyMatrixQuestionId = smq.SurveyMatrixQuestionId,
                                    TextMatrix = smc.Text,
                                    QuestionFileName = qb.Name,
                                })
                              ).ToListAsync();

            if (survey.Count() < 1)
            {
                return null;
            }

            //Position
            var position = await (from spm in this.DB.SurveyPositionMappings
                                  join p in this.DB.Positions on spm.PositionId equals p.PositionId
                                  where spm.SurveyId == surveyId
                                  select (new PositionViewModel
                                  {
                                      PositionId = p.PositionId,
                                      PositionName = p.PositionName
                                  })
                                  ).ToListAsync();

            //Outlet
            var outlet = await (from som in this.DB.SurveyOutletMappings
                                join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                where som.SurveyId == surveyId
                                select (new OutletViewModel
                                {
                                    OutletId = o.OutletId,
                                    Name = o.Name
                                })
                                  ).ToListAsync();

            //Area
            var listArea = await (from som in this.DB.SurveyOutletMappings
                                  join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                  join a in this.DB.Areas on o.AreaId equals a.AreaId
                                  where som.SurveyId == surveyId
                                  select new AreaViewModel
                                  {
                                      AreaId = a.AreaId,
                                      Name = a.Name
                                  }).Distinct().AsNoTracking().ToListAsync();

            //Province
            var listProvince = await (from som in this.DB.SurveyOutletMappings
                                      join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                      join p in this.DB.Provinces on o.ProvinceId equals p.ProvinceId
                                      where som.SurveyId == surveyId
                                      select new ProvinceViewModel
                                      {
                                          ProvinceId = p.ProvinceId,
                                          ProvinceName = p.ProvinceName
                                      }).Distinct().AsNoTracking().ToListAsync();
            //City
            var listCity = await (from som in this.DB.SurveyOutletMappings
                                  join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                  join c in this.DB.Cities on o.CityId equals c.CityId
                                  where som.SurveyId == surveyId
                                  select new CityViewModel
                                  {
                                      CityId = c.CityId,
                                      CityName = c.CityName
                                  }).Distinct().AsNoTracking().ToListAsync();
            //Dealer
            var listDealer = await (from som in this.DB.SurveyOutletMappings
                                    join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                    join d in this.DB.Dealers on o.DealerId equals d.DealerId
                                    where som.SurveyId == surveyId
                                    select new DealerViewModel
                                    {
                                        DealerId = d.DealerId,
                                        DealerName = d.DealerName
                                    }).Distinct().AsNoTracking().ToListAsync();


            var surveyInitialize = new SurveyInitialize
            {
                SurveyId = survey[0].SurveyId,
                Title = survey[0].Title,
                EndDate = survey[0].EndDate,
                StartDate = survey[0].StartDate,
                Outlet = outlet,
                Position = position,
                Area = listArea,
                City = listCity,
                Dealer = listDealer,
                Province = listProvince,
                Question = new List<SurveyQuestionGet>()
            };


            //Question
            var surveyQuestion = new List<SurveyQuestionGet>();
            var listSurveyQuestionId = survey.Select(Q => Q.SurveyQuestionId).Distinct().ToList();
            foreach (var id in listSurveyQuestionId)
            {
                //Choice
                var choice = survey.Where(Q => Q.SurveyQuestionId == id).Select(Q => new SurveyChoiceGet
                {
                    SurveyChoiceId = Q.SurveyChoiceId.GetValueOrDefault(),
                    Choice = Q.Choice,
                    BlobId = Q.ChoiceBlobId,
                    FileName = Q.FileName
                }).Distinct().ToList();
                var allMatchingChoices = await (from smc in DB.SurveyMatchingChoices
                                                join blob in DB.Blobs on smc.BlobId equals blob.BlobId into blobnull
                                                from blob in blobnull.DefaultIfEmpty()
                                                where smc.SurveyQuestionId == id
                                                select new
                                                {
                                                    SurveyMatchingChoiceId = smc.SurveyMatchingChoiceId,
                                                    SurveyQuestionId = smc.SurveyQuestionId,
                                                    BlobId = smc.BlobId,
                                                    SurveyMatchingChoiceCode = smc.SurveyMatchingChoiceCode,
                                                    Text = smc.Text,
                                                    ImageUpload = new FileContent
                                                    {
                                                        Base64 = null,
                                                        ContentType = blob.Mime,
                                                        FileName = blob.Name
                                                    }
                                                }).AsNoTracking().ToListAsync();
                var matchingChoiceToAdd = new SurveyMatchingChoice
                {
                    LeftChoices = allMatchingChoices
                                .Where(Q => Q.SurveyMatchingChoiceCode.ToLower().Contains('a'))
                                .OrderBy(Q => Q.SurveyMatchingChoiceCode)
                                .Select(Q => new SurveyMatchingChoiceFormModel
                                {
                                    BlobId = Q.BlobId,
                                    ImageUpload = Q.ImageUpload,
                                    SurveyMatchingChoiceCode = Q.SurveyMatchingChoiceCode,
                                    SurveyMatchingChoiceId = Q.SurveyMatchingChoiceId,
                                    Text = Q.Text,
                                    Type = Q.BlobId == null ? "text" : "image"
                                }).ToList(),
                    RightChoices = allMatchingChoices
                                .Where(Q => Q.SurveyMatchingChoiceCode.ToLower().Contains('b'))
                                .OrderBy(Q => Q.SurveyMatchingChoiceCode)
                                .Select(Q => new SurveyMatchingChoiceFormModel
                                {
                                    BlobId = Q.BlobId,
                                    ImageUpload = Q.ImageUpload,
                                    SurveyMatchingChoiceCode = Q.SurveyMatchingChoiceCode,
                                    SurveyMatchingChoiceId = Q.SurveyMatchingChoiceId,
                                    Text = Q.Text,
                                    Type = Q.BlobId == null ? "text" : "image"
                                }).ToList(),
                };
                //Question
                var question = survey.Where(Q => Q.SurveyQuestionId == id).Select(Q => new SurveyQuestionGet
                {
                    SurveyQuestionId = Q.SurveyQuestionId,
                    Question = Q.Question,
                    BlobId = Q.QuestionBlobId,
                    QuestionNumber = Q.QuestionNumber,
                    SurveyQuestionTypeId = Q.SurveyQuestionTypeId,
                    Choice = Q.SurveyQuestionTypeId == 2 ? new List<SurveyChoiceGet>() : choice,
                    MatchingChoices = matchingChoiceToAdd,
                    FileName = Q.QuestionFileName
                }).Distinct().FirstOrDefault();

                //Matrix
                if (question.SurveyQuestionTypeId == 12)
                {
                    var matrixChoice = survey.Where(Q => Q.SurveyQuestionId == id).GroupBy(Q => Q.SurveyMatrixChoiceId).Select(Q => new SurveyMatrixChoicesModel
                    {
                        SurveyMatrixChoiceId = Q.First().SurveyMatrixChoiceId.Value,
                        SurveyQuestionId = Q.First().SurveyQuestionId,
                        Text = Q.First().TextMatrix,
                        Number = Q.First().NumberMatrixChoice.GetValueOrDefault()
                    }).Distinct().OrderBy(Q => Q.Number).ToList();

                    var matrixQuestion = survey.Where(Q => Q.SurveyQuestionId == id).GroupBy(Q => Q.NumberMatrix).Select(Q => new SurveyMatrixQuestionsModel
                    {
                        Number = Q.First().NumberMatrix,
                        Question = Q.First().QuestionMatrix,
                        SurveyQuestionId = Q.First().SurveyQuestionId
                    }).Distinct().OrderBy(Q => Q.Number).ToList();

                    var matrixOption = new SurveyMatrixModel
                    {
                        MatrixChoice = matrixChoice,
                        MatrixQuestion = matrixQuestion
                    };

                    question.MatrixChoice = matrixOption;
                }
                else if (question.SurveyQuestionTypeId != 2)
                {
                    //Choice
                    var choiceGet = survey.Where(Q => Q.SurveyQuestionId == id).Select(Q => new SurveyChoiceGet
                    {
                        SurveyChoiceId = Q.SurveyChoiceId.GetValueOrDefault(),
                        Choice = Q.Choice,
                        BlobId = Q.ChoiceBlobId,
                        FileName = Q.FileName
                    }).Distinct().ToList();

                    question.Choice = choiceGet;
                }

                surveyInitialize.Question.Add(question);
            }

            surveyInitialize.Question = surveyInitialize.Question.OrderBy(Q => Q.QuestionNumber).ToList();

            return surveyInitialize;
        }

        public async Task<List<AreaViewModel>> GetAreaById(int id)
        {
            var listArea = await (from som in this.DB.SurveyOutletMappings
                                  join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                  join a in this.DB.Areas on o.AreaId equals a.AreaId
                                  where som.SurveyId == id
                                  select new AreaViewModel
                                  {
                                      AreaId = a.AreaId,
                                      Name = a.Name
                                  }).Distinct().AsNoTracking().ToListAsync();
            return listArea;
        }

        public async Task<List<RegionViewModel>> GetRegionById(int id)
        {
            var listRegion = await (from som in this.DB.SurveyOutletMappings
                                    join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                    join r in this.DB.Regions on o.RegionId equals r.RegionId
                                    where som.SurveyId == id
                                    select new RegionViewModel
                                    {
                                        RegionId = r.RegionId,
                                        RegionName = r.RegionName
                                    }).Distinct().AsNoTracking().ToListAsync();
            return listRegion;
        }

        public async Task<List<DealerViewModel>> GetDealerById(int id)
        {
            var listDealer = await (from som in this.DB.SurveyOutletMappings
                                    join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                    join d in this.DB.Dealers on o.DealerId equals d.DealerId
                                    where som.SurveyId == id
                                    select new DealerViewModel
                                    {
                                        DealerId = d.DealerId,
                                        DealerName = d.DealerName
                                    }).Distinct().AsNoTracking().ToListAsync();
            return listDealer;
        }

        public async Task<List<ProvinceViewModel>> GetProvinceById(int id)
        {
            var listProvince = await (from som in this.DB.SurveyOutletMappings
                                      join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                      join p in this.DB.Provinces on o.ProvinceId equals p.ProvinceId
                                      where som.SurveyId == id
                                      select new ProvinceViewModel
                                      {
                                          ProvinceId = p.ProvinceId,
                                          ProvinceName = p.ProvinceName
                                      }).Distinct().AsNoTracking().ToListAsync();
            return listProvince;
        }

        public async Task<List<CityViewModel>> GetCityById(int id)
        {
            var listCity = await (from som in this.DB.SurveyOutletMappings
                                  join o in this.DB.Outlets on som.OutletId equals o.OutletId
                                  join c in this.DB.Cities on o.CityId equals c.CityId
                                  where som.SurveyId == id
                                  select new CityViewModel
                                  {
                                      CityId = c.CityId,
                                      CityName = c.CityName
                                  }).Distinct().AsNoTracking().ToListAsync();
            return listCity;
        }

        public async Task<bool> DeleteSurveyById(int surveyId)
        {
            var searchSurveyById = await DB.Surveys.Where(Q => Q.SurveyId == surveyId && Q.IsDeleted == false).FirstOrDefaultAsync();

            if (searchSurveyById == null)
            {
                return false;
            }

            var approvalToSurvey = await this.DB.ApprovalToSurveys.Where(Q => Q.SurveyId == surveyId).FirstOrDefaultAsync();

            if (approvalToSurvey == null)
            {
                return false;
            }

            this.DB.ApprovalToSurveys.Remove(approvalToSurvey);
            var isDeleted = await this.ApprovalMan.DeleteApproval(approvalToSurvey.ApprovalId);

            if (isDeleted == false)
            {
                return false;
            }

            searchSurveyById.IsDeleted = true;
            searchSurveyById.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            searchSurveyById.UpdatedBy = ClaimMan.GetLoginUserId();

            await DB.SaveChangesAsync();

            return true;
        }
    }
}
