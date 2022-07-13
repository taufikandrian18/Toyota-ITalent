using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class ProductFeatureCategoryModel
    {
        public Guid ProductFeatureCategoryId { get; set; }
        public string ProductFeatureCategoryName { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        //public bool IsDeleted { get; set; }
    }
    public class ProductFeatureCategoryPaginate
    {
        public List<ProductFeatureCategoryModel> ProductFeatureCategories { get; set; }
        public int TotalProductFeatureCategories { get; set; }
    }

    public class ProductFeatureCategoryGridFilter : GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string ProductFeatureCategoryName { get; set; }
        //public Guid ProductId { get; set; }
    }
    public class ProductFeatureCategoryCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>

        [Required]
        public string ProductFeatureCategoryName { get; set; }

    }
    public class ProductFeatureCategoryUpdateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>

        [Required]
        public Guid ProductFeatureCategoryId { get; set; }

        [Required]
        public string ProductFeatureCategoryName { get; set; }

    }
    public class DeleteProductFeatureCategoryModel
    {
        public Guid ProductFeatureCategoryId { get; set; }
        public bool isDeleteProductFeatureCategory { get; set; }
    }
}
