using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideMyInsightModel
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public DateTime SurveyDueDate { get; set; }
    }

    public class UserSideMyInsightFilterModel
    {
        public string SurveyTitle { get; set; }
        public List<string> AreaIds { get; set; }
        public List<string> DealerIds { get; set; }
        public List<string> ProvinceIds { get; set; }
        public List<string> CityIds { get; set; }
        public List<string> OutletIds { get; set; }
        public DateTime? StartDueDate { get; set; }
        public DateTime? EndDueDate { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
