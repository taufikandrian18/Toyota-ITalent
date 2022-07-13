using System;
namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideProductPhotoModel
    {
        public Guid ProductPhotoId { get; set; }
        public Guid BlobId { get; set; }
        public bool Is360Photos { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFileType { get; set; }
        public string VideoUrl { get; set; }
        public string VideoFileName { get; set; }
        public string VideoFileType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
