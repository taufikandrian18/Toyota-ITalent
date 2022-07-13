using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideMyInsightAnswerService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;

        public UserSideMyInsightAnswerService(IFileStorageService fileStorageService, TalentContext talentContext)
        {
            this.DB = talentContext;
            this.FileMan = fileStorageService;
        }

        public async Task<bool> InsertSurveyAnswer(SurveyAnswerInsertModel SurveyAnswers, string employeeId)
        {
            var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();
            //TaskAnswer
            //SIMPEN JAWABAN DI DAN ANSWER
            var insertSurveyAnswer = new SurveyAnswers
            {
                EmployeeId = employeeId,
                SurveyId = SurveyAnswers.SurveyId,
                CreatedAt = dateNow,
            };
            this.DB.SurveyAnswers.Add(insertSurveyAnswer);


            //TaskAnswerDetail
            string blobId = null;
            var insertAnswerDetail = new List<TaskAnswerDetails>();

            //SIMPEN JAWABAN DI DAN ANSWER DETAIL
            foreach (var Insert in SurveyAnswers.Answer)
            {
                switch (Insert.QuestionTypeId)
                {
                    case 11:
                        if (Insert.File.Base64String == "" || Insert.File.Base64String == null)
                        {
                            this.DB.Add(new SurveyAnswerDetails
                            {
                                SurveyAnswerId = insertSurveyAnswer.SurveyAnswerId,
                                SurveyQuestionId = Insert.SurveyQuestionId,
                                Answer = Insert.Answer,
                            });
                            break;
                        }
                        string fileName, fileExtension;
                        fileName = Insert.File.FileName;
                        fileExtension = Insert.File.ContextType;
                        var newGuid = this.FileMan.InsertBlob(fileName, fileExtension);
                        fileName = newGuid.ToString();

                        Byte[] bytes = Convert.FromBase64String(Insert.File.Base64String);
                        using (MemoryStream fileStream = new MemoryStream(bytes))
                        //using (var fileStream = Insert.File.Files[0].OpenReadStream())
                        {
                            await this.FileMan.UploadFile(fileName, fileExtension, fileStream);
                        }
                        blobId = fileName;

                        this.DB.Add(new SurveyAnswerDetails
                        {
                            SurveyAnswerId = insertSurveyAnswer.SurveyAnswerId,
                            SurveyQuestionId = Insert.SurveyQuestionId,
                            Answer = Insert.Answer,
                            BlobId = newGuid
                        }
                        );
                        break;

                    case 12:
                    case 9:
                    case 8:
                    case 2:
                    case 5:
                        var detail = new SurveyAnswerDetails
                        {
                            SurveyAnswerId = insertSurveyAnswer.SurveyAnswerId,
                            SurveyQuestionId = Insert.SurveyQuestionId,
                            Answer = Insert.Answer,
                        };
                        this.DB.Add(detail);

                        //Special
                        var TaskSpecialAnswers = new List<SurveySpecialAnswers>();
                        foreach (var specialSurveyAnswer in Insert.Special)
                        {
                            var specialAnswers = new SurveySpecialAnswers
                            {
                                SurveyAnswerDetailId = detail.SurveyAnswerDetailId,
                                Number = specialSurveyAnswer.Number,
                                Answer = specialSurveyAnswer.Answer
                            };
                            TaskSpecialAnswers.Add(specialAnswers);
                        }
                        this.DB.AddRange(TaskSpecialAnswers);
                        break;

                    default:
                        this.DB.Add(new SurveyAnswerDetails
                        {
                            SurveyAnswerId = insertSurveyAnswer.SurveyAnswerId,
                            SurveyQuestionId = Insert.SurveyQuestionId,
                            Answer = Insert.Answer,
                        }
                        );
                        break;
                }
            }

            var totalPoint = SurveyPointEnum.Point;

            //Nambahin point history
            var data = new EmployeePointHistories
            {
                EmployeeId = employeeId,
                RewardPointTypeId = 1,
                PointTransactionTypeId = 1,
                Point = totalPoint,
                CreatedAt = DateTime.UtcNow.AddHours(7),
            };
            this.DB.Add(data);

            //Nambahin exp dan point
            var employee = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).FirstOrDefaultAsync();
            employee.EmployeeExperience = employee.EmployeeExperience + totalPoint;
            employee.LearningPoint = employee.LearningPoint + totalPoint;

            //CHECK REMED APA TIDAK
            await DB.SaveChangesAsync();
            return true;
        }

    }
}
