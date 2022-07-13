using System;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing news query data.
    /// </summary>
    public class UserSideNewsQueryModel
    {
        public int NewsId { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public int NewsCategoryId { set; get; }
        public string NewsCategoryName { set; get; }
        public string Link { set; get; }
        public string Author { set; get; }
        public string ThumbnailBlobId { set; get; }
        public string ThumbnailMime { set; get; }
        public string FileBlobId { set; get; }
        public string FileMime { set; get; }
        public bool IsDownloadable { set; get; }
        public int? TotalView { set; get; }
        public DateTime? ApprovedAt { set; get; }
        public DateTime? CreatedAt { set; get; }
        public DateTime? UpdateAt { set; get; }
        public bool IsDeleted { set; get; }
        public bool IsInternal{set;get;}
    }
}
