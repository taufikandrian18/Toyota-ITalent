using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTeamViewModel
    {
        public string EmployeeId { get; set; }
        public bool IsTeamLeader { get; set; }
        public List<UserSideTeamModel> ListTeamContract { get; set; }
        public List<UserSideTeamModel> ListTeamPermanent { get; set; }
        public int TotalTeam { get; set; }
        
        
    }
    public class UserSideTeamModel
    {
        public string EmployeeId { get; set; }
        public int TeamId { get; set; }
        public string ImageUrl { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }

        public string StatusContract { get; set; }
        public bool IsTeamLeader { get; set; }
        public bool IsRotated { get; set; }
        public bool IsAssign { get; set; }
    }

    public class UserSideTeamResponseModel
    {
        public List<UserSideTeamModel> ListTeam { get; set; }
        public int TotalData { get; set; }
    }

    public class UserSideTeamFormModel
    {
        public bool IsRotated { get; set; }
        public bool IsChecked { get; set; }
        public string EmployeeId { get; set; }
    }

    public class UserSideTeamFilterModel
    {
        /// <summary>
        /// Page Index (current page).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size (data per-page).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Keyword (search name)
        /// </summary>
        public string Keyword { get; set; }
    }
}
