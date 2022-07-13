using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class RequestScoreConfigModel
    {
        public string GUID {get;set;}
        public string SkillCheckGUID {get;set;}
        public string Text {get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
        public string Description { get; set; }
    }

    public class ResponseScoreConfigModel
    {
        public string GUID {get;set;}
        public string SkillCheckGUID{get;set;}
        public string Text {get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
    }

    public class ParamScoreConfigModel
    {
        public string GUID {get;set;}
        public string SkillCheckGUID{get;set;}
        public string Query {get;set;}
        public string SortKey {get;set;}
        public string SortType {get;set;}

        public int Limit {get;set;}
        public int Page {get;set;}

    }
}