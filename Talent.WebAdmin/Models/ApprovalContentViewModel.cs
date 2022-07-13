using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ApprovalContentViewModel
    {
        public int ApprovalId { get; set; }
        public int DetailId { get; set; }
        public string ContentName { get; set; }
        public int CurrentLevel { get; set; }
        public string ContentCategory { get; set; }
        public string CreatedBy { get; set; }
        public string Position { get; set; }
        public string ApprovalBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsApprovableByUser { get; set; }
    }
}
