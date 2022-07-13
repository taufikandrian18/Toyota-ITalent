namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing update request rotation data.
    /// </summary>
    public class UserSideUpdateStatusRequestRotationModel
    {
        public int RotateTeamMemberId { set; get; }

        /// <summary>
        /// Status request rotation (1 = Approved, 4 = Rejected)
        /// </summary>
        public int Status { set; get; }
    }
}
