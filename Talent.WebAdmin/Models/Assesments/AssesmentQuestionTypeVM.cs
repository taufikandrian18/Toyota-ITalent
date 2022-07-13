using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class RequestAssesmentQuestionTypeModel
    {
        public string GUID {get;set;}
        public string TypeName {get;set;}
    }

    public class ResponseAssesmentQuestionTypeModel
    {
        public string GUID {get;set;}
        public string TypeName {get;set;}
        public string CreatedAt {get;set;}
    }

    public class ParamAssesmentQuestionTypeModel
    {
        public string GUID {get;set;}
        public string Query {get;set;}
        public int Limit {get;set;}
        public int Page {get;set;}
    }
}