using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Models
{
    public class RequestAssesmentSkillCheckModel
    {
        public string GUID {get;set;}
        public string AssesmentGUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public float Order {get;set;}
    }

    public class ResponseAssesmentSkillCheckModel
    {
        public string GUID {get;set;}
        public string AssesmentGUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public float Order {get;set;}

        public List<SkillChecks> SkillChecks {get;set;}
        public List<Assesments> Assesments {get;set;}
    }

    public class ParamAssesmentSkillCheckModel
    {
        public string GUID {get;set;}
        public string AssesmentGUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public int Limit {get;set;}
        public int Page {get;set;}
    }
}