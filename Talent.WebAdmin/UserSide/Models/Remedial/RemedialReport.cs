using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class RemedialReportRequest{
        public string SuperiorEmployeeGuid {get;set;}
        public string DealerGUID {get;set;}
        public string Category {get;set;}
        public bool IsTamPeople {get;set;}
    }

    public class RemedialReportDetailRequest{
        public string CourseGUID {get;set;}
        public string Category {get;set;}
    }

    public class RemedialReportResponse{
         public string Category {get;set;}
        public List<RemedialReportDataResponse> AssesmentReports {get;set;}
        public List<RemedialReportDataResponse> ModuleReports {get;set;}
    }

    public class RemedialReportDataResponse{
        public string CourseGuid {get;set;}
        public string CourseName {get;set;}
        public List<RemedialReportDetailResponse> ScoringReportDataDetails{get;set;}
    }

    public class RemedialReportDetailResponse{
        public string EmployeeName {get;set;}
        public string EmployeeID {get;set;}
        public double Score {get;set;}
        public bool Status {get;set;}
    }

}