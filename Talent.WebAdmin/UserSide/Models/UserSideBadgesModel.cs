using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSideBadgesModel
    {
        public string EmployeeId { get; set; }
        public int EBadgesId { get; set; }
        public string EBadgesName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }

    }

    public class UserSideBadgesViewModel
    {
        public List<UserSideBadgesModel> ListBadgesGold { get; set; }
        public List<UserSideBadgesModel> ListBadgesSilver { get; set; }
        public List<UserSideBadgesModel> ListBadgesBronze { get; set; }

    }

    public class UserSideTotalBadgeModel
    {
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
    }
}
