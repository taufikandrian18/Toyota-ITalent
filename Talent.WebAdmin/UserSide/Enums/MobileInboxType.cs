using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Enums
{
    /// <summary>
    /// Store Mobile Inbox type enum id.
    /// </summary>
    public enum MobileInboxType
    {
        /// <summary>
        /// Id Resign
        /// </summary>
        Resign = 1,

        /// <summary>
        /// Id team member rotation request
        /// </summary>
        TeamMemberRotationRequest,

        /// <summary>
        /// Id Rotation Accepted
        /// </summary>
        RotationAccepted,
        
        /// <summary>
        /// Id Rotation Rejected
        /// </summary>
        RotationRejected,

        /// <summary>
        /// Id team member request.
        /// </summary>
        TeamMemberRequest,
        /// <summary>
        /// Id for remedial inboxes.
        /// </summary>
        Remedial,
        /// <summary>
        /// Id for Assign Learning.
        /// </summary>
        AssignLearning,
        /// <summary>
        /// Id for Team Member Request Result.
        /// </summary>
        TeamMemberRequestResult,
        /// <summary>
        /// Id for Training Invitation inboxes.
        /// </summary>
        TrainingInvitation
    }
}
