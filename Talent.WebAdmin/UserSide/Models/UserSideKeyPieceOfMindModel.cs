using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideKeyPieceOfMindModel
    {
        public Guid KeyPieceOfMindId { get; set; }
        public Guid KeyPieceOfMindTypeId { get; set; }
        public string KeyPieceOfMindTypeName { get; set; }
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
    public class UserSideKeyPieceOfMindMenuAwalModel
    {
        public Guid KeyPieceOfMindMenuAwalId { get; set; }
        public string KeyPieceOfMindMenuAwalName { get; set; }
        public int KeyPieceOfMindMenuAwalSequence { get; set; }
    }
    public class UserSideKeyPieceOfMindMenuModel
    {
        public Guid KeyPieceOfMindMenuId { get; set; }
        public string KeyPieceOfMindMenuName { get; set; }
    }
    public class UserSideKeyPieceOfMindViewModel
    {
        public Guid KeyPieceOfMindId { get; set; }
        public Guid KeyPieceOfMindTypeId { get; set; }
        public string KeyPieceOfMindTypeName { get; set; }
        public Guid Cms_ContentId { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideKeyPieceOfMindPaginateModel
    {
        public string KeyPieceOfMindType { get; set; }
        public List<UserSideKeyPieceOfMindViewModel> KeyPieceOfMindTitles { get; set; }
    }
    public class UserSideKeyPieceOfMindContentNameList
    {
        public Guid KeyPieceOfMindId { get; set; }
        public string KeyPieceOfMindContentName { get; set; }
        public int IsSequence { get; set; }
    }
    public class UserSideKeyPieceOfMindContentViewModel
    {
        public string KeyPieceOfMindTypeName { get; set; }
        public List<UserSideKeyPieceOfMindContentNameList> KeyPieceOfMindContentNameList { get; set; }
    }
}
