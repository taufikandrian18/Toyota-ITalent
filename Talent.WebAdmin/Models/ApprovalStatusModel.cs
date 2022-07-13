using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ApprovalStatusModel
    {

        public int ApprovalId { get; set; }
        public string ApprovalName { get; set; }
    }

    public class ApprovalStatusViewModel
    {
        public List<ApprovalStatusModel> ListApprovalStatusModel { get; set; }
        public int TotalItem { get; set; }
    }
}
