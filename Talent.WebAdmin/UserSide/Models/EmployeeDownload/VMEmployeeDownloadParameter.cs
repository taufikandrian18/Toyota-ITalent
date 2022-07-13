using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.UserSide.Models
{
    public class VMEmployeeDownloadParameter
    {
        public string GUID { get; set; }
        public string EmployeeID { get; set; }
        public string RelatedId { get; set; }
        public List<string> Category { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

}
