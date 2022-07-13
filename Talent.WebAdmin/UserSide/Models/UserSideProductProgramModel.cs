using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductProgramModel
    {
        public Guid ProductProgramMappingId { get; set; }
        public Guid ProductProgramCategoryId { get; set; }
        public string ProductProgramCategoryName { get; set; }
        public Guid ProductId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public string Cms_ContentName { get; set; }
        public Guid Cms_ContentBlobId { get; set; }
        public string Cms_ContentBlobImageUrl { get; set; }
        public string Cms_ContentBlobImageFileName { get; set; }
        public Guid Cms_ContentVideoBlobId { get; set; }
        public string Cms_ContentVideoBlobImageUrl { get; set; }
        public string Cms_ContentVideoBlobImageFileName { get; set; }
        public Guid Cms_ContentFileBlobId { get; set; }
        public string Cms_ContentFileBlobImageUrl { get; set; }
        public string Cms_ContentFileBlobImageFileName { get; set; }
        public string Cms_ContentDescription { get; set; }
        public int IsSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class UserSideProductProgramCategoryModel
    {
        public Guid ProductProgramCategoryId { get; set; }
        public string ProductProgramCategoryName { get; set; }
    }
    public class UserSideProductProgramViewModel
    {
        public Guid ProductProgramMappingId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public string Cms_ContentName { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideProductProgramPaginateModel
    {
        public string ProductProgramCategory { get; set; }
        public List<UserSideProductProgramViewModel> ProductProgramTitles { get; set; }
    }
}
