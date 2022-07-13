using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class UserForgetPassword
    {
        public string  To { get; set; }
        public List<string> CC { get; set; }
        public string Email { get; set; }
        public string EmployeeID { get; set; }
        public string Message { get; set; }
    }

   
}
