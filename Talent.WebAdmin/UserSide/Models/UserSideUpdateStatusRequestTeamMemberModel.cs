namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing update request team member data.
    /// </summary>
    public class UserSideUpdateStatusRequestTeamMemberModel
    {
        public int RequestTeamMemberId { set; get; }

        /// <summary>
        /// Status request rotation (1 = Approved, 4 = Rejected)
        /// </summary>
        public int Status { set; get; }
    }
}
