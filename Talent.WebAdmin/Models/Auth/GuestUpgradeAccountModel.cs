using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class GuestUpgradeAccountModel
    {
        public string DealerID { get; set; }
        public string OutletID { get; set; }
        public int PositionID { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
    }
}
