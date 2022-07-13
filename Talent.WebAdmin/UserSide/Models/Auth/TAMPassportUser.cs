using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models.Auth
{
    public class TAMPassportUser
    {
        public string Unique_Name { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public List<string> Roles { get; set; }
    }
}
