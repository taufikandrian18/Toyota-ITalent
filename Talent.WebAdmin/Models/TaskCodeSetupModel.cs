using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TaskCodeSetupModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int EvaluationTypeId { get; set; }
        public int CompetencyTypeId { get; set; }
        public int ModuleId { get; set; }

        public string TaskCode { get; set; }

        public int TaskNumber { get; set; }
        public string PrefixCode { get; set; }
        public string CompetencyTypeName { get; set; }
        public string EvaluationTypeName { get; set; }
        public string ModuleName { get; set; }
    }

}
