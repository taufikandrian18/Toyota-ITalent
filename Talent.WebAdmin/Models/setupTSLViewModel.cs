using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SetupTSLViewModel
    {
        public Sales Sales { get; set; }
        public AfterSales AfterSales { get; set; }
        public TotalSales Total { get; set; }
    }

    public class Sales
    {
        public int BasicLevel { get; set; }
        public int FundamentalLevel { get; set; }
        public int AdvanceLevel { get; set; }
    }

    public class AfterSales
    {
        public int BasicLevel { get; set; }
        public int FundamentalLevel { get; set; }
        public int AdvanceLevel { get; set; }
    }

    public class TotalSales
    {
        public int BasicLevel { get; set; }
        public int FundamentalLevel { get; set; }
        public int AdvanceLevel { get; set; }
    }
}
