using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Models
{
    public class InboxModel
    {
        public int InboxId { get; set; }
        public string EmployeeId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string LinkToPage { get; set; }
        public int? ApprovalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsRead { get; set; }
        public bool IsDone { get; set; }
        public string EmployeeName { get; set; }
    }

    public class InboxFormModel
    {
        public int? ApprovalId { get; set; }
        public string LinkToPage { get; set; }
        public string ApprovalTo { get; set; }
        public string ContentCategory { get; set; }
    }

    public class InboxViewModel
    {
        public List<InboxModel> ListInbox { get; set; }
        public int TotalData { get; set; }
    }

    public class InboxFilterModel : PageAbstract
    {
        public string Search { get; set; }
    }

    public class ApprovalInboxModel
    {
        public int ApprovalId { get; set; }
        public string ContentName { get; set; }
        public string ContentCategory { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LinkToPage { get; set; }
        public string Message { get; set; }
        public int InboxId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string UserPosition { get; set; }
        public bool IsDone { get; set; }
        public int ApprovalLevel { get; set; }

    }

    public class InboxRejectedModel
    {
        public int ApprovalId { get; set; }
        public string Message { get; set; }
    }

    public class UpdateInboxModel
    {
        public Inboxes Inbox { get; set; }
        public int ContentId { get; set; }
    }
}
