using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Excels.Lib.Models;
using TAM.Talent.Excels.Lib.Services;

namespace Talent.WebAdmin.Services
{
    public class SurveyReportService
    {
        private readonly TalentContext DB;
        private readonly SurveyReportGenerateExcelService ExcelGenerator;
        private readonly IFileStorageService FileMan;

        public SurveyReportService(TalentContext talentContext, SurveyReportGenerateExcelService excelGenerator, IFileStorageService fileStorageService)
        {
            this.DB = talentContext;
            this.ExcelGenerator = excelGenerator;
            this.FileMan = fileStorageService;
        }

        public async Task<SurveyReportViewModel> GetAllSurveyReportData(SurveyReportFilter filter)
        {
            var query = this.DB.GetAllSurveyReports().AsQueryable();

            //search
            if (filter.StartDate != null && filter.EndDate != null)
            {
                query = query.Where(Q => (Q.StartDate >= filter.StartDate && Q.StartDate < filter.EndDate.Value.AddDays(1)) ||
                                         (Q.EndDate >= filter.StartDate && Q.EndDate < filter.EndDate.Value.AddDays(1)));
            }

            if (!string.IsNullOrEmpty(filter.SurveyTitle))
            {
                query = query.Where(Q => Q.SurveyTitle.Contains(filter.SurveyTitle));
            }

            if (filter.Respondent != null)
            {
                query = query.Where(Q => Q.Respondent == filter.Respondent);
            }

            if (filter.RespondentRate != null)
            {
                query = query.Where(Q => Q.RespondentRate == filter.RespondentRate);
            }

            if (filter.Status != null)
            {
                query = query.Where(Q => Q.Status == filter.Status);
            }

            //sort
            switch (filter.SortBy)
            {
                case "surveyTitle":
                    query = query.OrderBy(Q => Q.SurveyTitle);
                    break;
                case "surveyTitleDesc":
                    query = query.OrderByDescending(Q => Q.SurveyTitle);
                    break;
                case "respondent":
                    query = query.OrderBy(Q => Q.Respondent);
                    break;
                case "respondentDesc":
                    query = query.OrderByDescending(Q => Q.Respondent);
                    break;
                case "respondentRate":
                    query = query.OrderBy(Q => Q.RespondentRate);
                    break;
                case "respondentRateDesc":
                    query = query.OrderByDescending(Q => Q.RespondentRate);
                    break;
                case "status":
                    query = query.OrderBy(Q => Q.Status);
                    break;
                case "statusDesc":
                    query = query.OrderByDescending(Q => Q.Status);
                    break;
                case "startDate":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;
                case "endDate":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;
            }

            var baseUrlReport = await this.DB.ItalentReports.Where(Q => Q.ItalentReportId == 8).Select(Q => Q.Url).FirstOrDefaultAsync();
            var embedUrl = "?rs:Embed=true&filter=SurveyRespondent/SurveyId eq ";

            var listSurveyReport = await query
               .Skip((filter.PageNumber - 1) * 10).Take(10)
               .Select(Q => new SurveyReportModel
               {
                   SurveyId = Q.SurveyId,
                   SurveyTitle = Q.SurveyTitle,
                   Respondent = Q.Respondent,
                   RespondentRate = Q.RespondentRate,
                   Status = Q.Status,
                   StartDate = Q.StartDate,
                   EndDate = Q.EndDate,
                   UrlDetail = baseUrlReport + embedUrl + Q.SurveyId.ToString()
               })
               .ToListAsync();

            var totalItem = await query.CountAsync();

            var surveyReportDatas = new SurveyReportViewModel
            {
                SurveyReportList = listSurveyReport,
                TotalData = totalItem
            };

            return surveyReportDatas;
        }

        public async Task<MemoryStream> GenerateExcel(int surveyId)
        {
            //var surveyReportDatas = await (from s in this.DB.Surveys
            //                               join sa in this.DB.SurveyAnswers on s.SurveyId equals sa.SurveyId
            //                               join sad in this.DB.SurveyAnswerDetails on sa.SurveyAnswerId equals sad.SurveyAnswerId
            //                               join sq in this.DB.SurveyQuestions on sad.SurveyQuestionId equals sq.SurveyQuestionId
            //                               join sqt in this.DB.SurveyQuestionTypes on sq.SurveyQuestionTypeId equals sqt.SurveyQuestionTypeId
            //                               join e in this.DB.Employees on sa.EmployeeId equals e.EmployeeId

            //                               join o in this.DB.Outlets on e.OutletId equals o.OutletId into eo
            //                               from o in eo.DefaultIfEmpty()

            //                               join d in this.DB.Dealers on o.DealerId equals d.DealerId into od
            //                               from d in od.DefaultIfEmpty()

            //                               join a in this.DB.Areas on o.AreaId equals a.AreaId into oa
            //                               from a in oa.DefaultIfEmpty()

            //                               join p in this.DB.Provinces on o.ProvinceId equals p.ProvinceId into op
            //                               from p in op.DefaultIfEmpty()

            //                               join c in this.DB.Cities on o.CityId equals c.CityId into oc
            //                               from c in oc.DefaultIfEmpty()

            //                               join b in this.DB.Blobs on sad.BlobId equals b.BlobId into bsad
            //                               from b in bsad.DefaultIfEmpty()

            //                               where s.SurveyId == surveyId

            //                               select (new 
            //                               {
            //                                   timeStamp = sa.CreatedAt,
            //                                   surveyId = s.SurveyId,
            //                                   surveyTitle = s.Title,
            //                                   typeOfQuestion = sqt.Name,
            //                                   question = sq.Question,
            //                                   employeeName = e.EmployeeName,
            //                                   area = a.Name,
            //                                   province = p.ProvinceName,
            //                                   city = c.CityName,
            //                                   dealer = d.DealerName,
            //                                   outlet = o.Name,
            //                                   questionType = sqt,
            //                                   blob = b,
            //                                   surveyAnswerDetails = sad,
            //                               })
            //                              ).Distinct().ToListAsync();

            var query = await DB.GetSurveyReportDetailDownload(surveyId).Distinct().ToListAsync();

            var getSurveyReportDatas = query.Select(async Q => new SurveyReportExcelModel
            {
                TimeStamp = Q.TimeStamp,
                SurveyId = Q.SurveyId,
                SurveyTitle = Q.SurveyTitle,
                TypeOfQuestion = Q.TypeOfQuestion,
                Question = Q.Question,
                Answer = Q.SurveyQuestionTypeId == 11 
                    ? Q.BlobId == null 
                        ? ""
                        : await FileMan.GetFileAsync(Q.BlobId.ToString(), Q.Mime)
                    : Q.Answer,
                EmployeeName = Q.EmployeeName,
                Area = Q.Area,
                Province = Q.Province,
                City = Q.City,
                Dealer = Q.Dealer,
                Outlet = Q.Outlet
            }).Select(Q => Q.Result).ToList();

            //creating new excel workbook
            var wb = ExcelGenerator.CreateExcelReport(getSurveyReportDatas);

            //convert workbook into memorystream
            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }
    }
}
