using System;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductModel
    {
        public class UserSideProductGridModel
        {
            public Guid ProductId { get; set; }
            public Guid BlobId { get; set; }
            public string ImageUrl { get; set; }
            public string ProductName { get; set; }
            public string ProductCategory { get; set; }
            public int ProductSegment { get; set; }
            public bool IsCompetitor { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedAt { get; set; }
            public bool IsDeleted { get; set; }
            public FileContent UserSideProductFileContent { get; set; }

        }
        public class UserSideProductInformationModel
        {
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public Guid BlobId  { get; set; }
            public string ImageUrl { get; set; }
            public string ImageFileName { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}
