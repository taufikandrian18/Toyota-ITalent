using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    /// <summary>
    /// Model for User Role
    /// </summary>
    public class UserRoleModelViewDetail
    {
        public int UserRoleId { get; set; }

        /// <summary>
        /// User Role's Name
        /// </summary>
        public string UserRoleName { get; set; }

        /// <summary>
        /// User Role's Description
        /// </summary>
        public string RoleDescription { get; set; }

        /// <summary>
        /// User Type, wether its TAM people / Dealer people (using enum)
        /// </summary>
        public bool TypeOfPeople { get; set; }

        /// <summary>
        /// TAM People's Position Id
        /// </summary>
        public PositionDropdownModel Position { get; set; }

        /// <summary>
        /// Dealer Category Name
        /// </summary>
        public CategoryDropdownModel DealerCategory { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
