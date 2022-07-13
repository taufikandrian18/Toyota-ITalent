using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductGalleryModel
    {
        public class UserSideProductGalleryGridModel
        {
            public Guid ProductGalleryId { get; set; }
            public Guid ProductId { get; set; }
            public Guid BlobId { get; set; }
            public Guid ProductTypeId { get; set; }
            public string ProductGalleryColorCode { get; set; }
            public string ProductGalleryColorName { get; set; }
            public bool IsColor { get; set; }
            public string IsApproved { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CreatedBy { get; set; }
            public DateTime UpdatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public bool IsDeleted { get; set; }
            public FileContent UserSideProductGalleryFileContent { get; set; }
        }

        public class UserSideProductGalleryViewModel
        {
            public Guid ProductGalleryId { get; set; }
            public Guid ProductId { get; set; }
            public Guid BlobId { get; set; }
            public string ImageUrl { get; set; }
            public string ImageFileName { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

        public class UserSideProductGalleryPaginate
        {
            public List<UserSideProductGalleryViewModel> ProductGallery { get; set; }
            public int TotalProductGalleries { get; set; }
            public string ProductGalleryColorName { get; set; }
        }

        public class UserSideProductGalleryColorListView
        {
            public string ProductGalleryColorCode { get; set; }
        }

        public class UserSideProductGalleryContributeModel
        {
            [Required]
            public Guid ProductId { get; set; }

            [Required]
            public Guid ProductTypeId { get; set; }

            public string ProductGalleryColorCode { get; set; }

            public string ProductGalleryColorName { get; set; }

            public FileContent ProductGalleryFileContent { get; set; }

            [Required]
            public bool IsColor { get; set; }
        }
    }
}
