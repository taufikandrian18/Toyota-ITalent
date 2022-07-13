using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing news data.
    /// </summary>
    public class UserSideNewsModel
    {
        public int NewsId { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public int NewsCategoryId { set; get; }
        public string NewsCategoryName { set; get; }
        public string Link { set; get; }
        public string Author { set; get; }
        public string ThumbnailUrl { set; get; }
        public string FileUrl { set; get; }
        public bool IsDownloadable { set; get; }
        public int? TotalView { set; get; }
        public DateTime? ApprovedAt { set; get; }
        public DateTime? CreatedAt { set; get; }
        public DateTime? UpdatedAt { set; get; }
        public bool IsDeleted { set; get; }
    }
}
