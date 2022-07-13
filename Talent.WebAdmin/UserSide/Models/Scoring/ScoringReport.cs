using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class ScoringReportRequest
    {
        public string SuperiorEmployeeGuid { get; set; }
        public string Category { get; set; }
        public string DealerGuid { get; set; }
        public bool IsTamPeople { get; set; }
        public string AssesmentGuid{get;set;}
        public int TrainingID { get; set; }
        public string SkillCheckGUID { get; set; }
        public bool IsCoach { get; set; }
        public string EmployeeId { get; set; }

        public int? Attempts {get;set;}
        public int Page {get;set;}
        public int Limit {get;set;}
    }

    public class ScoringReportResponse
    {
        public string Category { get; set; }
        public string EmployeeGuid { get; set; }
        public string EmployeeName { get; set; }
        public ScoringReportData ReportData { get; set; }

    }

    public partial class ScoringReportData
    {

        public float AssesmentPercentage { get; set; }
        public float ModulePercentage { get; set; }
        public List<ScoringReportDataDetail> AssesmentReports { get; set; }
        public List<ScoringReportDataDetail> ModuleReports { get; set; }

    }

    public partial class ScoringReportDataDetail
    {
        public string Name { get; set; }
        public string AssesmentId { get; set; }
        public int? ModuleId { get; set; }
        public double Score { get; set; }
        public double Attempts { get; set; }
        public string EnrollmentDate { get; set; }

    }


    public partial class RemedialReportData
    {
        public string Category { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeGUID { get; set; }
        public List<RemedialReportDataDetail> Detail { get; set; }
    }


    public partial class RemedialReportDataDetail {
        public string SkillCheckName {get;set;}
        public double Score {get;set;}
        public string Feedback {get;set;}
        public string CreatedAt {get;set;}
        public string ScoringBy {get;set;}
        public bool IsDraft { get; set; }
        public bool IsFinished { get; set; }
        public bool IsScored { get; set; }
        public float Attempts { get; set; }
        public string Guid { get; set; }
        public string SkillCheckGuid { get; set; }
        public string Description { get; set; }
        public string ScoreTitle { get; set; }
        public string ScoreDescription { get; set; }

        public double Orders {get;set;}

        public int? TrainingID {get;set;}

    }

    public partial class RemedialReportDatas
    {
        public string Category { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeGUID { get; set; }
        public List<RemedialReportDataDetailSkillCheck> Detail { get; set; }
    }

    public partial class RemedialReportDataDetailSkillCheck
    {
        public string SkillCheckName { get; set; }
        public string SkillCheckGUID { get; set; }
        public string AsssesmentGUID { get; set; }
        public float Order {get;set;}
        public List<RemedialReportDataDetailScore> Scores { get; set; }
    }

    public partial class RemedialReportDataDetailScore
    {
        public double Score { get; set; }
        public string Feedback { get; set; }
        public string CreatedAt { get; set; }
        public string ScoringBy { get; set; }
        public bool IsDraft { get; set; }
        public bool IsFinished { get; set; }
        public bool IsScored { get; set; }
        public float Attempts { get; set; }
    }
}