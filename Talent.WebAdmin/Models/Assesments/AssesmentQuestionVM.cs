using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class RequestAssesmentQuestionModel
    {
        public string GUID {get;set;}
        public string GUIDAssesmentQuestionType {get;set;}
        public string QuestionCode {get;set;}
        public string Question {get;set;}
        public string MediaBlobID {get;set;}
        public string EnumStoryLine {get;set;}
        public float TotalScore {get;set;}
        public float TotalPoints {get;set;} 
    }

    public class ResponseAssesmentQuestionModel
    {
        public string GUID {get;set;}
        public string GUIDAssesmentQuestionType {get;set;}
        public string QuestionCode {get;set;}
        public string Question {get;set;}
        public string MediaBlobID {get;set;}
        public string EnumStoryLine {get;set;}
        public float TotalScore {get;set;}
        public float TotalPoints {get;set;} 

        public DateTime CreatedAt {get;set;}
    }

    public class ParamAssesmentQuestionModel
    {
        public string GUID {get;set;}
        public string Query {get;set;}
        public int Limit {get;set;}
        public int Page {get;set;}
    }
}