using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class UserRoleGridModel
    {
        public List<UserRoleModelView> UserRole { get; set; }
        public int TotalData { get; set; }
        public int TotalDataFilter { get; set; }
    }
}
