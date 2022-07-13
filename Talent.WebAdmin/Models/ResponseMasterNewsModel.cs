using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ResponseMasterNewsModel
    {
        public int TotalData { get; set; }
        public List<MasterNewsViewModel> ContentList { get; set; }
    }
}
