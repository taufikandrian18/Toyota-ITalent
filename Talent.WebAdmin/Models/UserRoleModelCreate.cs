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
    public class UserRoleModelCreate
    {
        /// <summary>
        /// User Role's Name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserRoleName { get; set; }

        /// <summary>
        /// User Role's Description
        /// </summary>
        [MaxLength(255)]
        public string RoleDescription { get; set; }

        /// <summary>
        /// User Type, wether its TAM people / Dealer people (using enum)
        /// </summary>
        [Required]
        public bool TypeOfPeople { get; set; }

        /// <summary>
        /// Position Id
        /// </summary>
        [Required]
        public PositionDropdownModel Position { get; set; }

        /// <summary>
        /// Dealer Category Id
        /// </summary>
        public CategoryDropdownModel DealerCategory { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}