using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ResponseApprovalContentModel
    {
        public int TotalData { get; set; }
        public List<ApprovalContentViewModel> ContentList { get; set; }
    }
}
