using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CompetencySetupModel
    {
        public int CompetencyId { get; set; }

        public string PrefixCode { get; set; }

        public int CompetencyTypeId { get; set; }

        public string CompetencyTypeName { get; set; }
    }
}
