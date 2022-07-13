using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class ReportNPSService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly ClaimHelper ClaimMan;

        public ReportNPSService(TalentContext db, IContextualService contextual, ClaimHelper claim)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.ClaimMan = claim;
        }

        private string GetStatus(DateTime? startDate, DateTime? endDate, DateTime currentDate)
        {
            if (startDate > currentDate) return "Not Started";
            if (startDate < currentDate && endDate >= currentDate) return "On Going";
            if (endDate < currentDate) return "Done";
            return "Not Set";
        }
        public async Task<ReportNPSViewModel> GetPaginateReportNPS(ReportNPSFilterModel filterModel)
        {
            var query = DB.GetReportNPS().AsQueryable();
            if (filterModel.StartDate != null && filterModel.EndDate != null)
            {
                query = query.Where(Q => (Q.StartDate >= filterModel.StartDate && Q.StartDate < filterModel.EndDate.Value.AddDays(1)));
            }
            if (filterModel.ProgramTypeId != null)
            {
                query = query.Where(Q => Q.ProgramTypeId == filterModel.ProgramTypeId);
            }
            if (string.IsNullOrEmpty(filterModel.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filterModel.CourseName.ToLower()));
            }
            if (filterModel.Batch != null && filterModel.Batch != 0)
            {
                query = query.Where(Q => Q.Batch == filterModel.Batch);
            }
            if (string.IsNullOrEmpty(filterModel.Status) == false)
            {
                query = query.Where(Q => Q.Status == filterModel.Status);
            }
            switch (filterModel.SortBy)
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
            var baseUrlReport = await this.DB.ItalentReports.Where(Q => Q.ItalentReportId == 11).Select(Q => Q.Url).FirstOrDefaultAsync();
            var embedUrl = "?rs:Embed=true&filter=TrainingNPS%2FTrainingId%20eq%20";

            var totalData = await query.CountAsync();
            var resultQuery = query.Skip((filterModel.PageNumber - 1) * filterModel.PageSize).Take(filterModel.PageSize);
            var results = await resultQuery.Select(Q => new ReportNPSItemModel
            {
                TrainingId = Q.TrainingId,
                Batch = Q.Batch,
                CourseId = Q.CourseId,
                CourseName = Q.CourseName,
                EndDate = Q.EndDate,
                ProgramTypeName = Q.ProgramTypeName,
                StartDate = Q.StartDate,
                Status = Q.Status,
                ProgramTypeId = Q.ProgramTypeId,
                ViewCoachUrl = baseUrlReport + embedUrl + Q.TrainingId,
                ViewCourseUrl = baseUrlReport + embedUrl + Q.TrainingId
            }).ToListAsync();
            return new ReportNPSViewModel
            {
                ReportNPSItems = results,
                TotalData = totalData,
            };
        }

    }
}

