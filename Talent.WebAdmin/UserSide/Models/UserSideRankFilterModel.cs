using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing rank filter data.
    /// </summary>
    public class UserSideRankFilterModel
    {
        public List<string> AreaIds { set; get; }
        public List<string> DealerIds { set; get; }
        public List<int?> PositionIds { set; get; }
        public bool IsAllTime { set; get; }
        public string SortBy { set; get; }
    }
}
