using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideLoginReturnModel
    {
        public string EmployeeId { set; get; }
        public string Email { set; get; }
        public string Name { set; get; }
        public List<UserSidePositionModel> Position { set; get; }
        public bool IsCoach { set; get; }
        public bool IsTeamLeader { set; get; }
        public string TalentToken { set; get; }
    }
}
