using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class RequestSkillCheckQuestionModel
    {
        public string GUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public string AssesmentQuestionGUID {get;set;}
    }

    public class ResponseSkillCheckQuestionModel
    {
        public string GUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public string AssesmentQuestionGUID {get;set;}

        public List<ResponseSkillCheckModel> SkillChecks {get;set;}

        public List<ResponseAssesmentQuestionModel> Questions {get;set;}
    }

    public class ParamSkillCheckQuestionModel
    {
         public string GUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public string AssesmentQuestionGUID {get;set;}
    }
}