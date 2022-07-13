using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Excels.Lib.Models;
using TAM.Talent.Excels.Lib.Services;

namespace Talent.WebAdmin.Services
{
    public class TrainingScoreReportService
    {
        private readonly TalentContext DB;
        private readonly TrainingScoreReportGenerateExcelService ExcelGenerator;
        private readonly IFileStorageService FileService;

        public TrainingScoreReportService(TalentContext talentContext, TrainingScoreReportGenerateExcelService excelService, IFileStorageService fileStorageService)
        {
            this.DB = talentContext;
            this.ExcelGenerator = excelService;
            this.FileService = fileStorageService;
        }

        public async Task<ReportTrainingScoreViewModel> GetTrainingScoreReport(ReportTrainingScoreFilterModel filter)
        {

            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }

            var query = this.DB.GetAllTrainingScoreReportView();

            if (filter.DateStart != null && filter.DateEnd != null)
            {
                query = query.Where(Q => (Q.StartDate >= filter.DateStart && Q.StartDate <= filter.DateEnd.EndDayTime()) || (Q.EndDate >= filter.DateStart && Q.EndDate <= filter.DateEnd.EndDayTime()));
            }

            if (filter.Batch != null)
            {
                query = query.Where(Q => Q.Batch == filter.Batch);
            }

            if (filter.Participant != null)
            {
                query = query.Where(Q => Q.Participant == filter.Participant);
            }

            if (filter.ParticipantRate != null)
            {
                query = query.Where(Q => Q.ParticipantRate == filter.ParticipantRate);
            }

            if (filter.ProgramTypeId != null)
            {
                query = query.Where(Q => Q.ProgramTypeId == filter.ProgramTypeId);
            }

            if (string.IsNullOrEmpty(filter.Status) == false)
            {
                query = query.Where(Q => Q.Status.ToLower() == filter.Status.ToLower());
            }

            if (string.IsNullOrEmpty(filter.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.CourseName.ToLower()));
            }

            switch (filter.SortBy)
            {
                case "courseName":
                    query = query.OrderBy(Q => Q.CourseName);
                    break;
                case "courseNameDesc":
                    query = query.OrderByDescending(Q => Q.CourseName);
                    break;
                case "programType":
                    query = query.OrderBy(Q => Q.ProgramTypeName);
                    break;
                case "programTypeDesc":
                    query = query.OrderByDescending(Q => Q.ProgramTypeName);
                    break;
                case "batch":
                    query = query.OrderBy(Q => Q.Batch);
                    break;
                case "batchDesc":
                    query = query.OrderByDescending(Q => Q.Batch);
                    break;
                case "participant":
                    query = query.OrderBy(Q => Q.Participant);
                    break;
                case "participantDesc":
                    query = query.OrderByDescending(Q => Q.Participant);
                    break;
                case "status":
                    query = query.OrderBy(Q => Q.Status);
                    break;
                case "statusDesc":
                    query = query.OrderByDescending(Q => Q.Status);
                    break;
                case "endDate":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;
                case "participantRate":
                    query = query.OrderBy(Q => Q.ParticipantRate);
                    break;
                case "participantRateDesc":
                    query = query.OrderByDescending(Q => Q.ParticipantRate);
                    break;
                case "startDate":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;

                default:
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;
            }

            var baseUrlReport = await this.DB.ItalentReports.Where(Q => Q.ItalentReportId == 12).Select(Q => Q.Url).FirstOrDefaultAsync();
            var embedUrl = "?rs:Embed=true&filter=TrainingScoreReport/TrainingId eq ";

            var data = await query.Select(Q => new ReportTrainingScoreModel
            {
                Batch = Q.Batch,
                CourseName = Q.CourseName,
                EndTime = Q.EndDate.Value,
                Participant = Q.Participant,
                ParticipantRate = Q.ParticipantRate.Value * 100,
                ProgramType = Q.ProgramTypeName,
                StartTime = Q.StartDate.Value,
                Status = Q.Status,
                TrainingId = Q.TrainingId,
                UrlDetail = baseUrlReport + embedUrl + Q.TrainingId.ToString()
            }).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var totalData = await query.CountAsync();

            var result = new ReportTrainingScoreViewModel
            {
                ListReportTrainingScore = data,
                TotalData = totalData
            };

            return result;
        }

        public async Task<List<ProgramTypeModel>> GetAllProgramTypes()
        {
            var result = await this.DB.ProgramTypes.Select(Q => new ProgramTypeModel
            {
                ProgramTypeId = Q.ProgramTypeId,
                ProgramTypeName = Q.ProgramTypeName
            }).ToListAsync();

            return result;
        }

        public async Task<MemoryStream> GenerateReportTrainingScore(int trainingId)
        {
            var queryResult = new List<TrainingScoreReportDownloadQueryModel>();
            var skip = 0;
            var take = 1000;
            var count = 0;
            do
            {

                var queryResultTake = await this.DB.GetTrainingReportDownload(trainingId).Skip(skip).Take(take).ToListAsync();
                queryResult.AddRange(queryResultTake);
                count = queryResultTake.Count();
                skip += count;

            } while (count >= take);
            

            var result = queryResult.Select(async Q => new TrainingScoreReportExcellModel
            {
                Answer = Q.AnswerBlobId != null ? await FileService.GetFileAsync(Q.AnswerBlobId.ToString(), Q.MIME) : Q.Answer,
                Area = Q.Area,
                City = Q.City,
                CompetencyCode = Q.CompetencyCode,
                CourseName = Q.CourseName,
                Dealer = Q.Dealer,
                EmployeeName = Q.EmployeeName,
                ModuleName = Q.Module,
                Outlet = Q.Outlet,
                Province = Q.Province,
                Question = Q.Question,
                Timestamp = Q.Timestamp,
                TotalScore = Q.TotalScore,
                TotalScoreModule = Q.TotalScoreModule,
                TypeofQuestion = Q.TypeofQuestion,
                Attempts = Q.Attempts
            }).Select(Q => Q.Result).OrderBy(Q => Q.Timestamp).ToList();

            var excelWorkbook = ExcelGenerator.CreateExcelReport(result);

            MemoryStream MS = new MemoryStream();
            excelWorkbook.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }

    }
}
