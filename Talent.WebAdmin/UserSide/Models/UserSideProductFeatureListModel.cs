using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductFeatureListModel
    {
        public Guid ProductFeatureMappingId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductTypeId { get; set; }
        public Guid ProductFeatureId { get; set; }
        public Guid ProductFeatureCategoryId { get; set; }
        public Guid? Cms_FmbId { get; set; }
        public Guid? Cms_FmbBlobId { get; set; }
        public string Cms_FmbBlobUrl { get; set; }
        public string Cms_FmbBlobFileType { get; set; }
        public string Cms_FmbBlobFileName { get; set; }
        public string Cms_FmbDescription { get; set; }
        public Guid? Cms_WorkPrincipalId { get; set; }
        public Guid? Cms_WorkPrincipalBlobId { get; set; }
        public string Cms_WorkPrincipalBlobUrl { get; set; }
        public string Cms_WorkPrincipalFileType { get; set; }
        public string Cms_WorkPrincipalFileName { get; set; }
        public string Cms_WorkPrincipalDescription { get; set; }
        public Guid? Cms_ContentId { get; set; }
        public Guid? Cms_ContentBlobId { get; set; }
        public Guid? Cms_ContentVideoBlobId { get; set; }
        public Guid? Cms_ContentFileBlobId { get; set; }
        public string Cms_ContentBlobImageUrl { get; set; }
        public string Cms_ContentBlobImageFileName { get; set; }
        public string Cms_ContentVideoBlobImageUrl { get; set; }
        public string Cms_ContentVideoBlobImageFileName { get; set; }
        public string Cms_ContentFileBlobImageUrl { get; set; }
        public string Cms_ContentFileBlobImageFileName { get; set; }
        public string Cms_ContentName { get; set; }
        public string Cms_ContentDescription { get; set; }
        public Guid? Cms_OperationId { get; set; }
        public Guid? Cms_OperationBlobId { get; set; }
        public string Cms_OperationBlobUrl { get; set; }
        public string Cms_OperationFileType { get; set; }
        public string Cms_OperationFileName { get; set; }
        public string Cms_OperationDescription { get; set; }
        public Guid? Cms_SettingId { get; set; }
        public Guid? Cms_SettingBlobId { get; set; }
        public string Cms_SettingBlobUrl { get; set; }
        public string Cms_SettingFileType { get; set; }
        public string Cms_SettingFileName { get; set; }
        public string Cms_SettingDescription { get; set; }
        public bool? IsSpecial { get; set; }
    }

    public class UserSideProductFeatureFilter
    {
        public Guid ProductFeatureMappingId { get; set; }
        public Guid FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideProductFeatureCategoryFilter
    {
        public Guid ProductFeatureCategoryId { get; set; }
        public string ProductFeatureCategoryName { get; set; }
    }

    public class UserSideProductFeatureViewModel
    {
        public List<UserSideProductFeatureCategoryFilter> FeatureCategoryList { get; set; }
        public List<UserSideProductFeatureFilter> FeatureList { get; set; }
    }
}
