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
    public class UserRoleModelView
    {
        /// <summary>
        /// User Role's Id
        /// </summary>
        public int UserRoleId { get; set; }

        /// <summary>
        /// User Role's Name
        /// </summary>
        public string UserRoleName { get; set; }

        /// <summary>
        /// User Type, wether its TAM people / Dealer people (using enum)
        /// </summary>
        public string TypeOfPeople { get; set; }

        /// <summary>
        /// Position Name
        /// </summary>
        public PositionDropdownModel Position { get; set; }

        public CategoryDropdownModel Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}