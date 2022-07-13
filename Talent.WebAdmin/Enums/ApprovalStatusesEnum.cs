using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Enums
{
    public class ApprovalStatusesEnum
    {
        public static string Approve = "Approved";
        public static string Rejected = "Rejected";
        public static string Waiting = "Waiting for Approval";
        public static string Draft = "Draft";
        public const int ApproveId = 1;
        public const int WaitingId = 2;
        public const int DraftId = 3;
        public const int RejectedId = 4;
    }
}
