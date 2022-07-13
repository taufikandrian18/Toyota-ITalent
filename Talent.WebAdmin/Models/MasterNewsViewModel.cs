using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class MasterNewsViewModel
    {

        //filter item
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string NewsTitle { get; set; }
        public string NewsCategory { get; set; }
        public string NewsStatus { get; set; }

    }
}
