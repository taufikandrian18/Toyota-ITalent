using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Models
{
    public class RequestAssesmentModel
    {
        public string GUID {get;set;}
        public string Name {get;set;}
        public string EnumTrainingType { get; set; }
        public string EnumScoringMethod {get;set;}
        public string EnumRemedialOption{get;set;}
        public float LearningTime {get;set;}
        public float RemedialLimit { get; set; }
        public bool IsByOption { get; set; }
        public List<RequestAssesmentSkillCheck> AssesmentSkillChecks { get; set; }
    }

    public class RequestAssesmentSkillCheck {
        public string SkillCheckGUID { get; set; }
        public float Order { get; set; }
    }

    public class ResponseAssesmentModel
    {
        public string GUID {get;set;}
        public string Name {get;set;}
        public string EnumScoringMethod {get;set;}
        public string EnumRemedialOption{get;set;}
        public string EnumTrainingType { get; set; }
        public float RemedialLimit { get; set; }
        public bool IsByOption { get; set; }
        public float LearningTime {get;set;}
        public List<AssesmentSkillChecks> SkillCheck { get; set; }
    }

    public class ParamAssesmentModel
    {
        public string GUID {get;set;}
        public string Query {get;set;}
        public string SortKey {get;set;}
        public string SortType {get;set;}
        public int Limit {get;set;}
        public int Page {get;set;}
    }

    public class RequestTrackingProgressModel {
        public string SuperiorGUID { get; set; }
        public int? CourseId { get; set; }
        public string DealerId { get; set; }
        public string IsTamPeople { get; set; }
    }

    public class ResponseTrackingProgressModel {
        public string DealerName { get; set; }
        public string OutletName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeGUID { get; set; }
        public string PositionName { get; set; }
        public List<ResponseTrackingProgressModelDetail> Assesments { get; set; }
        public List<ResponseTrackingProgressModelDetail> Module { get; set; }
        public List<ResponseTrackingProgressModelDetail> FileUpload { get; set; }
    }

    public class ResponseTrackingProgressModelDetail
    {
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int CourseId { get; set; }
        public float Score { get; set; }
        public bool Done { get; set; }
    }

    public class RequestTableOfContent {
        public string EmployeeID { get; set; }
        public string CourseID { get; set; }

        public int? TrainingId {get;set;}
    }

    public class ResponseTableOfContent {
        public double Percentage {get;set;}
        public List<ResposeTableOfContent> Data {get;set;}
        public int? RelatedTrainingId {get;set;}

    }

    public class ResposeTableOfContent {
        public string CategoryName { get; set; }
        public double Percentage { get; set; }

        public bool IsNA {get;set;}
        public List<SubResponseTableOfContent> Detail { get; set; }

    }

    public class SubResponseTableOfContent {
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int  Status { get; set; }
      // public bool Status {get;set;}
    }

     public class ExportLearningProgreess {
        public List<ExportLearningProgreessDetails> Modules {get;set;}
        public List<ExportLearningProgreessDetails> Assesments {get;set;}
    }

    public class ExportLearningProgreessDetails {
        public string ModuleName {get;set;}
        public List<ExportLearningProgreessMembers> Members {get;set;}
    }

    public class ExportLearningProgreessMembers {
        public string EmployeeName {get;set;}
        public string DealerName {get;set;}
        public string OutletName {get;set;}
    }
 }