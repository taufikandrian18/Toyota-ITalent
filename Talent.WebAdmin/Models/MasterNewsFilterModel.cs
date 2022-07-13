using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class MasterNewsFilterModel
    {
        //total item
        public int Page { get; set; }
        public int ItemPage { get; set; }

        //filter item
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string NewsTitle { get; set; }
        public string NewsCategory { get; set; }
        public string NewsStatus { get; set; }

        //order
        public string OrderBy { get; set; }

    }
}
