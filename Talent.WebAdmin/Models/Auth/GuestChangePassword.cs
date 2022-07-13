using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class GuestChangePasswordModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string OldPaassword { get; set; }
    }
    
}
