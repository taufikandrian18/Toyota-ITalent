using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductFAQModel
    {
        public Guid ProductFaqId { get; set; }
        public Guid ProductFaqCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BlobId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public int ProductFaqSequence { get; set; }
        public string ProductFaqTitle { get; set; }
        public string ProductFaqDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class MainPageProductFAQModel
    {

        public List<UserSideProductFAQModel> ProductFAQEventList { get; set; }

    }

    public class UserSideProductFAQCategoryView
    {
        public Guid ProductFAQCategoryId { get; set; }
        public string ProductFAQCategoryName { get; set; }
    }

    public class UserSideProductFAQPaginate
    {
        public string ProductFAQCategoryName { get; set; }
        public MainPageProductFAQModel ProductFAQList { get; set; }
    }

    public class UserSideProductFAQContributeModel
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ProductFaqCategoryId { get; set; }

        [Required]
        public string ProductFAQUserQuestion { get; set; }
    }
}
