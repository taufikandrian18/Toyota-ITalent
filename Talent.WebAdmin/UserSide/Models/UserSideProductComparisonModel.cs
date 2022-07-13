using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductComparisonModel
    {
        
        public Guid ProductCompetitorId { get; set; }
        public string ProductCompetitorName { get; set; }
        public Guid ProductCompetitorBlobId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public Guid ProductCompetitorTypeId { get; set; }
        public string ProductCompetitorTypeName { get; set; }
    }
    public class UserSideBaseProductComparisonModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid ProductBlobId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public Guid ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
    }

    public class UserSideProductFeatureMappingModel
    {
        public Guid ProductFeatureMappingId { get; set; }
        public Guid Cms_ContentId { get; set; }
        public string Cms_ContentName { get; set; }
        public Guid Cms_ContentBlobId { get; set; }
        public string Cms_ContentImageUrl { get; set; }
        public string Cms_ContentImageFileName { get; set; }
    }

    public class UserSideProductFeatureContentImageModelView
    {
        public Guid ProductFeatureCmsContentBlob { get; set; }
        public string ProductFeatureCmsContentBlobUrl { get; set; }
        public string ProductFeatureCmsContentBlobFileName { get; set; }
        public string ProductFeatureCmcContentBlobFileType { get; set; }
    }

    public class UserSideProductFeatureContentDescModelView
    {
        public string ProductFeatureCmsContentDesc { get; set; }
    }

    public class UserSideProductFeatureContentModelView
    {
        public List<UserSideProductFeatureContentImageModelView> ContentImageView { get; set; }
        public List<UserSideProductFeatureContentDescModelView> ContentDescView { get; set; }
    }

    public class UserSideProductFeatureModel
    {
        public Guid ProductFeatureId { get; set; }
        public string ProductFeatureName { get; set; }
        public Guid BlobFeatureId { get; set; }
        public string BlobFeatureUrl { get; set; }
        public string BlobFeatureFileName { get; set; }
        public string BlobFeatureFileType { get; set; }
        public UserSideProductFeatureContentModelView ProductFeatureMappingModels { get; set; }
        public UserSideProductFeatureContentModelView ProductFeatureMappingCompModels { get; set; }
        //public List<List<UserSideProductFeatureMappingModel>> ProductFeatureMappingCompModels { get; set; }
        //public List<UserSideProductFeatureDetailModel> ProductFeatureMappingModel { get; set; }
    }

    public class UserSideProductFeatureNameModel
    {
        public Guid ProductFeatureCategoryId { get; set; }
        public string ProductFeatureCategoryName { get; set; }
        public List<UserSideProductFeatureModel> ProductFeatureModels { get; set; }
        //public List<List<UserSideProductFeatureMappingModel>> ProductFeatureMappingModels { get; set; }
        //public List<List<UserSideProductFeatureMappingModel>> ProductFeatureMappingCompModels { get; set; }
    }

    //public class UserSideProductFeatureDetailModel
    //{
    //    public List<List<UserSideProductFeatureMappingModel>> ProductFeatureMappingModels { get; set; }
    //    public List<List<UserSideProductFeatureMappingModel>> ProductFeatureMappingCompModels { get; set; }
    //}

    public class UserSideProductFeatureComparisonContributeModel
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ProductTypeId { get; set; }

        [Required]
        public Guid ProductFeatureId { get; set; }

        [Required]
        public Guid ProductFeatureCategoryId { get; set; }

        public FileContent ProductGalleryFileContent { get; set; }

        public DateTime? ApprovedAt { get; set; }

    }
}
