using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class ReportService
    {
        private readonly TalentContext DB;

        public ReportService(TalentContext context)
        {
            this.DB = context;
        }

        public async Task<List<ReportModel>> GetAllItalentReport()
        {
            var allReports = await this.DB.ItalentReports.Select(Q => new ReportModel{
                ItalentReportId = Q.ItalentReportId,
                Name = Q.Name,
                Url = Q.Url
            }).ToListAsync();

            return allReports;
        }


    }
}
