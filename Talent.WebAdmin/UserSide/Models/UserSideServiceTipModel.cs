using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideServiceTipModel
    {
        public Guid ServiceTipId { get; set; }
        public Guid ServiceTipTypeId { get; set; }
        public string ServiceTipTypeName { get; set; }
        public Guid ServiceTipMenuId { get; set; }
        public string ServiceTipMenuName { get; set; }
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
    public class UserSideServiceTipMenuAwalModel
    {
        public Guid ServiceTipMenuAwalId { get; set; }
        public string ServiceTipMenuAwalName { get; set; }
        public int ServiceTipMenuAwalSequence { get; set; }
    }
    public class UserSideServiceTipMenuModel
    {
        public Guid STMenuId { get; set; } //CMS_Menu
        public string STMenuName { get; set; } //CMS_Menu
    }
    public class UserSideServiceTipViewModel
    {
        public Guid ServiceTipId { get; set; }
        public Guid ServiceTipTypeId { get; set; }
        public string ServiceTipTypeName { get; set; }
        public Guid Cms_ContentId { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideServiceTipPaginateModel
    {
        public string ServiceTipType { get; set; }
        public List<UserSideServiceTipViewModel> ServiceTipTitles { get; set; }
    }
    public class UserSideServiceTipContentNameList
    {
        public Guid ServiceTipId { get; set; }
        public string ServiceTipContentName { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideServiceTipContentViewModel
    {
        public string ServiceTipTypeName { get; set; }
        public List<UserSideServiceTipContentNameList> ServiceTipContentNameList { get; set; }
    }
}
