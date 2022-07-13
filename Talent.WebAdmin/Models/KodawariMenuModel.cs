using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class KodawariMenuModel
    {
        public Guid KodawariMenuId { get; set; }
        public string KodawariMenuName { get; set; }
        public int KodawariMenuSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class KodawariMenuPaginate
    {
        public List<KodawariMenuModel> KodawariMenus { get; set; }
        public int TotalKodawariMenus { get; set; }
    }

    public class KodawariMenusGridFilter : GridFilter
    {
        public string KodawariMenuName { get; set; }
    }

    public class KodawariMenuCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string KodawariMenuName { get; set; }

        [Required]
        public int KodawariMenuSequence { get; set; }

    }
    public class KodawariMenuUpdateModel
    {
        [Required]
        public Guid KodawariMenuId { get; set; }
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string KodawariMenuName { get; set; }

        [Required]
        public int KodawariMenuSequence { get; set; }

    }
    public class DeleteKodawariMenuModel
    {
        public Guid KodawariMenuId { get; set; }
        public bool isDeleteKodawariMenus { get; set; }
    }
}
