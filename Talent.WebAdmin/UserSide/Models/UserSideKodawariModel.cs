using System;
using System.Collections.Generic;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideKodawariModel
    {
        public Guid KodawariId { get; set; }
        public Guid KodawariTypeId { get; set; }
        public string KodawariTypeName { get; set; }
        public Guid KodawariMenuId { get; set; }
        public string KodawariMenuName { get; set; }
        public Guid Cms_MenuId { get; set; }
        public string Cms_MenuName { get; set; }
        public Guid Cms_ContentId { get; set; }
        public string Cms_ContentName { get; set; }
        public string Cms_ContentDescription { get; set; }
        public Guid Cms_ContentBlobId { get; set; }
        public string Cms_ContentBlobImageUrl { get; set; }
        public string Cms_ContentBlobImageFileName { get; set; }
        public Guid Cms_ContentVideoBlobId { get; set; }
        public string Cms_ContentVideoBlobImageUrl { get; set; }
        public string Cms_ContentVideoBlobImageFileName { get; set; }
        public Guid Cms_ContentFileBlobId { get; set; }
        public string Cms_ContentFileBlobImageUrl { get; set; }
        public string Cms_ContentFileBlobImageFileName { get; set; }
        public Guid? Cms_SubContentId { get; set; }
        public string Cms_SubContentName { get; set; }
        public string Cms_SubContentDescription { get; set; }
        public Guid? Cms_SubContentBlobId { get; set; }
        public string Cms_SubContentBlobImageUrl { get; set; }
        public string Cms_SubContentBlobImageFileName { get; set; }
        public Guid? Cms_SubContentVideoBlobId { get; set; }
        public string Cms_SubContentVideoBlobImageUrl { get; set; }
        public string Cms_SubContentVideoBlobImageFileName { get; set; }
        public Guid? Cms_SubContentFileBlobId { get; set; }
        public string Cms_SubContentFileBlobImageUrl { get; set; }
        public string Cms_SubContentFileBlobImageFileName { get; set; }
        public int IsSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class UserSideKodawariBannerModel
    {
        public Guid KodawariBannerId { get; set; }
        public Guid BlobId { get; set; }
        public string ImageUrl { get; set; }
        public string FileTypeBlob { get; set; }
        public string FileNameBlob { get; set; }
        public string KodawariBannerTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class UserSideKodawariDownloadModel
    {
        public Guid KodawariDownloadId { get; set; }
        public Guid KodawariMenuId { get; set; }
        public string KodawariMenuName { get; set; }
        public Guid BlobId { get; set; }
        public string ImageUrl { get; set; }
        public string FileTypeBlob { get; set; }
        public string FileNameBlob { get; set; }
        public string KodawariDownloadTitle { get; set; }
        public bool IsDownloadable { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class UserSideKodawariMenuAwalModel
    {
        public Guid KodawariMenuAwalId { get; set; }
        public string KodawariMenuAwalName { get; set; }
        public int KodawariMenuAwalSequence { get; set; }
    }
    public class UserSideKodawariMenuModel
    {
        public Guid KDMenuId { get; set; } //Kodawari_Menu
        public string KDMenuName { get; set; } //Kodawari_Menu
    }

    public class UserSideKodawariCategoryMenuModel
    {
        public Guid Cms_MenuId { get; set; } //Cms_Menu
        public string Cms_MenuName { get; set; } //Cms_Menu
        public string Cms_MenuCategory { get; set; } //Cms_menu
    }

    public class UserSideKodawariCategoryMenuPaginateModel
    {
        public Guid KodawariMenuAwalId { get; set; }
        public List<UserSideKodawariCategoryMenuModel> KodawariCategoryModel { get; set; }
    }

    public class UserSideKodawariViewModel
    {
        public Guid KodawariId { get; set; }
        //public Guid KodawariTypeId { get; set; }
        //public string KodawariTypeName { get; set; }
        public Guid Cms_ContentId { get; set; }
        public string Cms_ContentName { get; set; }
        public int IsSequence { get; set; }
        public List<Cms_SubContents> Cms_SubContents { get; set; }
    }
    public class UserSideKodawariPaginateModel
    {
        public string KodawariType { get; set; }
        public List<UserSideKodawariViewModel> KodawariTitles { get; set; }
    }
    public class UserSideKodawariContentNameList
    {
        public Guid KodawariId { get; set; }
        public string KodawariContentName { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideKodawariContentViewModel
    {
        public string KodawariTypeName { get; set; }
        public List<UserSideKodawariContentNameList> KodawariContentNameList { get; set; }
    }
}
