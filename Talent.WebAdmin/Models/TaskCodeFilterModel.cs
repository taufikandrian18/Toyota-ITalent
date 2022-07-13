using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TaskCodeFilterModel
    {
        public int? CompetencyId { get; set; }
        public int? ModuleId { get; set; }
        public string FilterName { get; set; }
    }
}
