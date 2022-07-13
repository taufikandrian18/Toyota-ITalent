using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class OutletStagingModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal DealerCompanyId { get; set; }
        public string OutletFunctionId { get; set; }
        public string OutletCode { get; set; }
        public string PhoneNumber { get; set; }
        public decimal KabupatenId { get; set; }
        public decimal RegionCode { get; set; }
        public decimal TamAreaId { get; set; }
        public decimal? TamAreaAfterSalesId { get; set; }
        public string State { get; set; }
    }
}
