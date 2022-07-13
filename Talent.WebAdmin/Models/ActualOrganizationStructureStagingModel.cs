using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ActualOrganizationStructureStagingModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string NoReg { get; set; }
        public string PostCode { get; set; }
        public string PostName { get; set; }
        public decimal? Staffing { get; set; }
        public string State { get; set; }
    }
}