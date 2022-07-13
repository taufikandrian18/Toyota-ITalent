using System;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing message data.
    /// </summary>
    public class UserSideInboxModel
    {
        public int InboxId { set; get; }
        public int InboxTypeId { set; get; }
        public string InboxTypeName { set; get; }
        public string EmployeeId { set; get; }
        public string EmployeeName { set; get; }
        public string Title { set; get; }
        public string Message { set; get; }
        public string Notes { set; get; }
        public UserSideEmployeeResignModel EmployeeResign { set; get; }
        public UserSideTeamMemberRotationRequestModel RotateTeamMemberRequest { set; get; }
        public UserSideTeamMemberRequestModel TeamMemberRequest { set; get; }
        public bool IsRead { set; get; }
        public string FromImageUrl { get; set; }
        public string FromName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
