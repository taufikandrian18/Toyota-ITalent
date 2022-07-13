using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductSPWAModel
    {
        public Guid ProductSPWAMappingId { get; set; }
        public Guid ProductSPWACategoryId { get; set; }
        public string ProductSPWACategoryName { get; set; }
        public Guid ProductId { get; set; }
        public Guid Cms_MenuId { get; set; }
        public string Cms_MenuName { get; set; }
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
    public class UserSideProductSPWAMenuModel
    {
        public Guid ProductSPWAMenuId { get; set; }
        public string ProductSPWAMenuName { get; set; }
    }
    public class UserSideProductSPWAViewModel
    {
        public Guid ProductSPWACategoryId { get; set; }
        public string ProductSPWACategoryName { get; set; }
    }
    public class UserSideProductSPWAPaginateModel
    {
        public Guid ProductId { get; set; }
        public Guid ProductSPWAMenuId { get; set; }
        public string ProductSPWAMenuName { get; set; }
        public List<UserSideProductSPWAViewModel> ProductSPWACategories { get; set; }
    }
    public class UserSideProductSPWAContentNameList
    {
        public Guid ProductSPWAMappingId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public string ProducSPWAContentName { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideProductSPWAContentViewModel
    {
        public string ProductSPWACategoryName { get; set; }
        public Guid BlobId { get; set; }
        public string BlobImageUrl { get; set; }
        public string BlobImageFileName { get; set; }
        public string FileType { get; set; }
        public List<UserSideProductSPWAContentNameList> ProductSPWAContentNameList { get; set; }
    }

    public class UserSideProductSPWATestDriveBlobModel
    {
        public Guid BlobId { get; set; }
        public int BlobIndex { get; set; }
        public string BlobFileUrl { get; set; }
        public string BlobFileName { get; set; }
        public string BlobFileType { get; set; }
    }

    public class UserSideProductSPWATestDriveContentViewModel
    {
        public string ProductSPWACategoryName { get; set; }
        public List<UserSideProductSPWATestDriveBlobModel> BlobModels { get; set; }
        public List<UserSideProductSPWAContentNameList> ProductSPWAContentNameList { get; set; }
    }
}
