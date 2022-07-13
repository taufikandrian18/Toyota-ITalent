using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductTipModel
    {
        public Guid ProductTipId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BlobId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Guid ProductTipCategoryId { get; set; }
        public string OutletId { get; set; }
        public string ProductTipTitle { get; set; }
        public string ProductTipDescription { get; set; }
        public int IsSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserSideProductTipCategoryModel
    {
        public Guid ProductTipCategoryId { get; set; }
        public string ProductTipCategoryName { get; set; }
    }

    public class UserSideProductTipViewModel
    {
        public Guid ProductTipId { get; set; }
        public string ProductTipTitle { get; set; }
        public int IsSequence { get; set; }
    }

    public class UserSideProductTipPaginateModel
    {
        public Guid ProductTipCategoryId { get; set; }
        public string ProductTipCategory { get; set; }
        public List<UserSideProductTipViewModel> ProductTipTitles { get; set; }
    }

    public class UserSideContributeNewSalesTipCreateModel
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ProductTipCategoryId { get; set; }

        public FileContent ProductGalleryFileContent { get; set; }

        [Required]
        public int IsSequence { get; set; }

        [Required]
        public string ProductTipTitle { get; set; }

        [Required]
        public string ProductTipDescription { get; set; }
    }
}
