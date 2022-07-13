using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class ProductCategoryModel
    {
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ProductCategoryPaginate
    {
        public List<ProductCategoryModel> ProductCategories { get; set; }
        public int TotalProductCategories { get; set; }
    }

    public class ProductCategoryGridFilter : GridFilter
    {
        public string ProductCategoryName { get; set; }
    }

    public class ProductCategoryCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string ProductCategoryName { get; set; }

    }
    public class ProductCategoryUpdateModel
    {
        [Required]
        public Guid ProductCategoryId { get; set; }
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string ProductCategoryName { get; set; }

    }
    public class DeleteProductCategoryModel
    {
        public Guid ProductCategoryId { get; set; }
        public bool isDeleteProductCategory { get; set; }
    }
}
