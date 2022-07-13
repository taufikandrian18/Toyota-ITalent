using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing employee resign data.
    /// </summary>
    public class UserSideTeamMemberRotationRequestModel
    {
        public int RotateTeamMemberId { set; get; }
        public string EmployeeId { set; get; }
        public string EmployeeName { set; get; }
        public int PreviousTeamId { set; get; }
        public string PreviousTeamLeaderId { set; get; }
        public string PreviousTeamLeaderName { set; get; }
        public string PreviousTeamLeaderOutletName { get; set; }
        public int NextTeamId { set; get; }
        public string NextTeamLeaderId { set; get; }
        public string NextTeamLeaderName { set; get; }
        public string ApprovalStatus { set; get; }
        public string ImageUrl { get; set; }
    }
}
