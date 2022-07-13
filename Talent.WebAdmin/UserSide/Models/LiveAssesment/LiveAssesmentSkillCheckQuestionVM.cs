using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.UserSide.Models
{
    public class RequestLiveAssesmentSkillCheckModel
    {
        public string GUID { get; set; }
        public string SkillcheckGUID { get; set; }
        public string EmployeeGUID { get; set; }
        public string Answer { get; set; }
        public string Text { get; set; }
        public float Point { get; set; }
        public float Score { get; set; }
        public bool IsScored { get; set; }
        public float Attempts { get; set; }
        public bool IsFinished { get; set; }
        public bool IsDraft { get; set; }
        public string AssesmentGUID {get;set;}
        public string ScorerGUID { get; set; }
        public int SetupModuleGuid { get; set; }
        public int TrainingID {get;set;}
    }

    public class ResponseLiveAssesmentSkillCheckModel
    {
       public string GUID {get;set;}
       public string SkillcheckGUID {get;set;}
       public string EmployeeGUID {get;set;}
       public string Answer {get;set;}
       public string Text {get;set;}
       public float Point {get;set;}
       public float Score {get;set;}
       public bool IsScored { get; set; }
       public float Attempts { get; set; }
        public bool IsFinished { get; set; }
        public bool IsDraft { get; set; }
        public string AssesmentGUID { get; set; }

    }

    public class ResponseAssesmentSkillCheckList
    {
        public string AssesmentGUID { get; set; }
        public string GUID { get; set; }
        public string SkillCheckGUID { get; set; }
        public float Order {get;set;}
        public ResponseSubAssesmnets Assesment { get; set; }
        public SkillChecks Skillcheck { get; set; }
        public List<SkillChecksScoreConfigs> Configurations { get; set; }
        public bool IsFinished {get;set;}
        public bool IsScored { get; set; }
        public bool IsDoing {get;set;}
        public double Orders {get;set;}
    }

    public class ResponseSubAssesmnets {
        public String GUID { get; set; }
        public String Name { get; set; }
        public String TrainingTypeID { get; set; }
        public String EnumScoringMethod { get; set; }
        public String EnumRemedialOption { get; set; }
        public float LearningTime { get; set; }
        public float RemedialLimit { get; set; }
        public bool IsByOption { get; set; }
        public float MinimumScore { get; set; }
    }

    public class ResponseSubSkillCheck {
        public String GUID { get; set; }
        public String Title { get; set; }
        public String Name { get; set; }
        public Boolean IsQuestion { get; set; }
        public float MinimumScore { get; set; }
        public String EnumFeedbackScore { get; set; }
        public String Description { get; set; }
        public String MediaBlobId { get; set; }
    }

    public class ResponseSubConfigurations {
    }


    public class ResponseAssesmentTeamMember
    {
        public List<ResponseAssesmentTeamMemberList> WaitingForScore { get; set; }
        public List<ResponseAssesmentTeamMemberList> Finish { get; set; }

    }

    public class ResponseAssesmentTeamMemberList {
        public string EmployeePhotos { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeGUID { get; set; }
    }

    public class ResponseAssesmentTeamMemberScoreList
    {
        public string EmployeePhotos { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeGUID { get; set; }
        public LiveAssesmentSkillChecks SkillCheck { get; set; }
    }

    public class ParamLiveAssesmentSkillCheckModel
    {
        public int? TrainingID { get; set; }
        public string GUID {get;set;}
        public string SkillcheckGUID {get;set;}
        public string AssesmentGUID { get; set; }
        public string QuestionGUID {get;set;}
        public string EmployeeGUID {get;set;}
        public string Point {get;set;}
        public float Attempts { get; set; }
        public string SuperiorGUID { get; set; }
        public string SetupModuleId {get;set;}

        public bool IsCoach {get;set;}
        public bool IsTamPeople {get;set;}
        public int Page {get;set;}
        public int Limt {get;set;}
    }
}