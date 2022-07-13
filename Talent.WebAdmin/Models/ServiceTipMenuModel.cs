using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class ServiceTipMenuModel
    {
        public Guid ServiceTipMenuId { get; set; }
        public string ServiceTipMenuName { get; set; }
        public int ServiceTipMenuSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ServiceTipMenuPaginate
    {
        public List<ServiceTipMenuModel> ServiceTipMenus { get; set; }
        public int TotalServiceTipMenus { get; set; }
    }

    public class ServiceTipMenuGridFilter : GridFilter
    {
        public string ServiceTipMenuName { get; set; }
    }

    public class ServiceTipMenuCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string ServiceTipMenuName { get; set; }

        [Required]
        public int ServiceTipMenuSequence { get; set; }

    }
    public class ServiceTipMenuUpdateModel
    {
        [Required]
        public Guid ServiceTipMenuId { get; set; }
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string ServiceTipMenuName { get; set; }

        [Required]
        public int ServiceTipMenuSequence { get; set; }

    }
    public class DeleteServiceTipMenuModel
    {
        public Guid ServiceTipMenuId { get; set; }
        public bool isDeleteServiceTipMenu { get; set; }
    }
}
