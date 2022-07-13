using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class CmsMenuModel
    {
        public Guid Cms_MenuId { get; set; }
        public string Cms_MenuName { get; set; }
        public string Cms_MenuCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CmsMenuPaginate
    {
        public List<CmsMenuModel> CmsMenus { get; set; }
        public int TotalCmsMenus { get; set; }
    }

    public class CmsMenuGridFilter : GridFilter
    {
        public string Cms_MenuName { get; set; }
        public string Cms_MenuCategory { get; set; }
    }

    public class CmsMenuCreateModel
    {
        [Required]
        public string Cms_MenuName { get; set; }

        [Required]
        public string Cms_MenuCategory { get; set; }
    }

    public class CmsMenuUpdateModel
    {
        [Required]
        public Guid Cms_MenuId { get; set; }

        [Required]
        public string Cms_MenuName { get; set; }

        [Required]
        public string Cms_MenuCategory { get; set; }
    }

    public class DeleteCmsMenuModel
    {
        public Guid Cms_MenuId { get; set; }
        public bool isDeleteCmsMenu { get; set; }
    }
}
