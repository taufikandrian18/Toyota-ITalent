using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class GuestLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }


}
