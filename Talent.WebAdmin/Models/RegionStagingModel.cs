using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class RegionStagingModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? ParentCode { get; set; }
        public string RegionType { get; set; }
        public string State { get; set; }
    }
}
