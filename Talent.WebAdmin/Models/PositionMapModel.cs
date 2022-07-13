using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    //all position mapping models
    public class PositionMapFilterModel
    {
        //total page
        public int Page { get; set; }
        public int ItemPage { get; set; }

        //filter item
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string PositionName { get; set; }
        public string CompetencyName { get; set; }
        public string Priority { get; set; }
        public int ProficiencyLevel { get; set; }

        //order
        public string OrderBy { get; set; }
    }

    public class PositionMapViewModel
    {
        public int PosCompetencyId { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string CompetencyName { get; set; }
        public string Priority { get; set; }
        public int ProficiencyLevel { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }

    public class PositionMapFormModel
    {
        public int PositionMapId { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
        public List<PosCompetencyModel> CompetencyList { get; set; }
    }

    public class PositionMapDeleteModel
    {
        public int PositionId { get; set; }

        public List<int> CompetencyListId { get; set; }
    }

    public class PosCompetencyModel
    {
        public int CompetencyId { get; set; }
        public string CompetencyName { get; set; }
        public string Priority { get; set; }
        public int ProficiencyLevel { get; set; }
    }

    public class ResponsePositionMapModel
    {
        public int TotalData { get; set; }
        public List<PositionMapViewModel> ContentList { get; set; }
    }
}
