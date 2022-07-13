using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ApprovalContentFilterModel
    {
        //total item
        public int Page { get; set; }
        public int ItemPage { get; set; }

        //filter item
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string ContentName { get; set; }
        public string ContentCategory { get; set; }
        public string CreatedBy { get; set; }
        public string ContentPosition { get; set; }
        public string ApprovalStatus { get; set; }
        public string OrderBy { get; set; }

    }
}
