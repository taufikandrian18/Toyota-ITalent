using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Constant
{
    /// <summary>
    /// Store ApprovalStatus constant ID.
    /// </summary>
    public class ApprovalStatus
    {
        public const int Approved = 1;
        public const int WaitingForApproval = 2;
        public const int Draft = 3;
        public const int Rejected = 4;
    }
}
