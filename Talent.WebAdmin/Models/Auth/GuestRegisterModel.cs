using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class GuestRegisterModel
    {
        public string Fullname { get; set; }
        public string NIK { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GuestUserType { get; set; }
        public GuestUserType GuestUserData { get; set; }
    }

    public class GuestUserType
    {
        public string EmployeeID { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
        public string PositionID { get; set; }
        public string PositionName { get; set; }
        public string Category { get; set; }
    }

}
