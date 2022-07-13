using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{

    public class ParamIsAllowNPSModel
    {
      public string TrainingId {get;set;}
      public string SetupModuleId {get;set;} 
      public string EmployeeId {get;set;}
      public string AssesmentId {get;set;}
    }
    
    public class RequestLiveAssesmentSkillCheckQuestionModel
    {
       public String GUID {get;set;}
        public string LiveAssesmentGUID {get;set;}
        public string EmployeeGUID{get;set;}
        public string Answer {get;set;}
        public string Text {get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
        public bool IsFinished { get; set; }
    }

    public class ResponseLiveAssesmentSkillCheckQuestionModel
    {
        public String GUID {get;set;}
        public string LiveAssesmentGUID {get;set;}
        public string EmployeeGUID{get;set;}
        public string Answer {get;set;}
        public string Text {get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
    }

    public class ParamLiveAssesmentSkillCheckQuestionModel
    {
 public string GUID {get;set;}
        public string LiveAssesmentGUID {get;set;}
        public string EmployeeGUID {get;set;}
        public string Point {get;set;}
        public float Page {get;set;}
        public float Liimt {get;set;}
    }
}