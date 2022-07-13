using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing employee resign data.
    /// </summary>
    public class UserSideEmployeeResignModel
    {
        public string EmployeeId { set; get; }
        public string Name { set; get; }
        public string PositionName { set; get; }
        public string OutletName { set; get; }
        public string TeamLeaderName { set; get; }
        public string ImageUrl { set; get; }
    }
}
