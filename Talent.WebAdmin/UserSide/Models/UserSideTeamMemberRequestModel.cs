namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing team member request data.
    /// </summary>
    public class UserSideTeamMemberRequestModel
    {
        public int TeamMemberRequestId { set; get; }
        public string EmployeeId { set; get; }
        public string EmployeeName { set; get; }
        public int TeamId { set; get; }
        public string TeamLeaderId { set; get; }
        public string TeamLeaderName { set; get; }
        public string ImageUrl { set; get; }
        public string ApprovalStatus { set; get; }
        public string PositionName { get; set; }
        public string OutletName { get; set; }
    }
}
