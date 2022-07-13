using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CompetencyTypeModel
    {
        public int CompetencyTypeId { get; set; }
        public string CompetencyTypeName { get; set; }
    }

    public class CompetencyTypeViewModel
    {
        public List<CompetencyTypeModel> ListCompetencyTypeModel { get; set; }
        public int TotalItem { get; set; }
    }
}
