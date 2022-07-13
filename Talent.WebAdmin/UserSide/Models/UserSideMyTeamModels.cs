using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideMyTeamEmployeeFilterModel
    {
        public List<string> OutletId { get; set; }

        public List<string> PositionId { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public string EmployeeName { get; set; }
    }

    public class UserSideMyTeamEmployeeModel
    {
        public string EmployeeId { get; set; }
        public string BlobImageUrl { get; set; }
        public string PositionName { get; set; }
        public string EmployeeName { get; set; }
        public bool IsWaitingApproval { get; set; }
        public string BlobId { get; set; }
        public string Mime { get; set; }
    }

    public class UserSideValidAddModel
    {
        public bool ableToAdd { get; set; }
        public List<UserSideMyTeamEmployeeModel> nonTeamEmployees { get; set; }
    }
}
