using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Blobs
    {
        public Blobs()
        {
            Assessments = new HashSet<Assessments>();
            Banners = new HashSet<Banners>();
            Courses = new HashSet<Courses>();
            CmsFmbBlob = new HashSet<Cms_Fmbs>();
            CmsFmbFileBlob = new HashSet<Cms_Fmbs>();
            CmsOperationBlob = new HashSet<Cms_Operations>();
            CmsOperationFileBlob = new HashSet<Cms_Operations>();
            CmsWorkPrincipalBlob = new HashSet<Cms_WorkPrincipals>();
            CmsWorkPrincipalFileBlob = new HashSet<Cms_WorkPrincipals>();
            CmsContentBlob = new HashSet<Cms_Contents>();
            CmsContentVideoBlob = new HashSet<Cms_Contents>();
            CmsContentFileBlob = new HashSet<Cms_Contents>();
            CmsSettingBlob = new HashSet<Cms_Settings>();
            CmsSettingFileBlob = new HashSet<Cms_Settings>();
            CmsSubContentBlob = new HashSet<Cms_SubContents>();
            CmsSubContentVideoBlob = new HashSet<Cms_SubContents>();
            CmsSubContentFileBlob = new HashSet<Cms_SubContents>();
            DigitalSignatures = new HashSet<DigitalSignatures>();
            Dictionaries = new HashSet<Dictionaries>();
            EmployeeCertificates = new HashSet<EmployeeCertificates>();
            Employees = new HashSet<Employees>();
            Events = new HashSet<Events>();
            Guides = new HashSet<Guides>();
            HOGuidelines = new HashSet<HOGuidelines>();
            KeyPieceOfMindTypes = new HashSet<KeyPieceOfMindTypes>();
            KodawariBanners = new HashSet<KodawariBanners>();
            KodawariTypes = new HashSet<KodawariTypes>();
            KodawariDownloads = new HashSet<KodawariDownloads>();
            ModulesBlob = new HashSet<Modules>();
            ModulesMaterialBlob = new HashSet<Modules>();
            NewsFileBlob = new HashSet<News>();
            NewsThumbnailBlob = new HashSet<News>();
            Products = new HashSet<Products>();
            ProductTypes = new HashSet<ProductTypes>();
            ProductGalleries = new HashSet<ProductGalleries>();
            ProductPhotos = new HashSet<ProductPhotos>();
            ProductFaqs = new HashSet<ProductFAQs>();
            ProductFeatures = new HashSet<ProductFeatures>();
            ProductTips = new HashSet<ProductTips>();
            ProductProgramCategories = new HashSet<ProductProgramCategories>();
            ProductSPWACategories = new HashSet<ProductSPWACategories>();
            SurveyAnswerDetails = new HashSet<SurveyAnswerDetails>();
            SurveyChoices = new HashSet<SurveyChoices>();
            SurveyMatchingChoices = new HashSet<SurveyMatchingChoices>();
            SurveyQuestions = new HashSet<SurveyQuestions>();
            ServiceTipTypes = new HashSet<ServiceTipTypes>();
            TaskAnswerDetails = new HashSet<TaskAnswerDetails>();
            TaskHotSpotTypes = new HashSet<TaskHotSpotTypes>();
            TaskMatchingChoices = new HashSet<TaskMatchingChoices>();
            TaskTebakGambarPictures = new HashSet<TaskTebakGambarPictures>();
            Tasks = new HashSet<Tasks>();
            Topics = new HashSet<Topics>();
        }

        [Key]
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        [Required]
        [Column("MIME")]
        public string Mime { get; set; }
        public DateTime CreatedAt { get; set; }

        [InverseProperty("Blob")]
        public virtual ICollection<Assessments> Assessments { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Banners> Banners { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Courses> Courses { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Cms_Contents> CmsContentBlob { get; set; }
        [InverseProperty("ContentVideoBlob")]
        public virtual ICollection<Cms_Contents> CmsContentVideoBlob { get; set; }
        [InverseProperty("ContentFileBlob")]
        public virtual ICollection<Cms_Contents> CmsContentFileBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Cms_Fmbs> CmsFmbBlob { get; set; }
        [InverseProperty("FmbFileBlob")]
        public virtual ICollection<Cms_Fmbs> CmsFmbFileBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Cms_Operations> CmsOperationBlob { get; set; }
        [InverseProperty("OperationFileBlob")]
        public virtual ICollection<Cms_Operations> CmsOperationFileBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Cms_Settings> CmsSettingBlob { get; set; }
        [InverseProperty("SettingFileBlob")]
        public virtual ICollection<Cms_Settings> CmsSettingFileBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Cms_SubContents> CmsSubContentBlob { get; set; }
        [InverseProperty("SubContentVideoBlob")]
        public virtual ICollection<Cms_SubContents> CmsSubContentVideoBlob { get; set; }
        [InverseProperty("SubContentFileBlob")]
        public virtual ICollection<Cms_SubContents> CmsSubContentFileBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Cms_WorkPrincipals> CmsWorkPrincipalBlob { get; set; }
        [InverseProperty("WorkPrincipalFileBlob")]
        public virtual ICollection<Cms_WorkPrincipals> CmsWorkPrincipalFileBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<DigitalSignatures> DigitalSignatures { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Dictionaries> Dictionaries { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<EmployeeCertificates> EmployeeCertificates { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Employees> Employees { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Events> Events { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Guides> Guides { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<HOGuidelines> HOGuidelines { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<KeyPieceOfMindTypes> KeyPieceOfMindTypes { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<KodawariBanners> KodawariBanners { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<KodawariTypes> KodawariTypes { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<KodawariDownloads> KodawariDownloads { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Modules> ModulesBlob { get; set; }
        [InverseProperty("MaterialBlob")]
        public virtual ICollection<Modules> ModulesMaterialBlob { get; set; }
        [InverseProperty("FileBlob")]
        public virtual ICollection<News> NewsFileBlob { get; set; }
        [InverseProperty("ThumbnailBlob")]
        public virtual ICollection<News> NewsThumbnailBlob { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Products> Products { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductPhotos> ProductPhotos { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductTypes> ProductTypes { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductFAQs> ProductFaqs { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductFeatures> ProductFeatures { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductTips> ProductTips { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductProgramCategories> ProductProgramCategories { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductSPWACategories> ProductSPWACategories { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ProductGalleries> ProductGalleries { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<ServiceTipTypes> ServiceTipTypes { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<SurveyAnswerDetails> SurveyAnswerDetails { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<SurveyChoices> SurveyChoices { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<SurveyMatchingChoices> SurveyMatchingChoices { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<SurveyQuestions> SurveyQuestions { get; set; }
        [InverseProperty("AnswerBlob")]
        public virtual ICollection<TaskAnswerDetails> TaskAnswerDetails { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<TaskHotSpotTypes> TaskHotSpotTypes { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<TaskMatchingChoices> TaskMatchingChoices { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<TaskTebakGambarPictures> TaskTebakGambarPictures { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Tasks> Tasks { get; set; }
        [InverseProperty("Blob")]
        public virtual ICollection<Topics> Topics { get; set; }
    }
}
