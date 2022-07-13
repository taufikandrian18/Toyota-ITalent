using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CompetencyKeyActionMappingModel
    {
        public int CompetencyId { get; set; }
        public int KeyActionId { get; set; }
    }

    public class CompetencyKeyActionMappingJoinModel
    {
        public int CompetencyId { get; set; }
        public int KeyActionId { get; set; }
        public string KeyActionCode { get; set; }
    }
}
