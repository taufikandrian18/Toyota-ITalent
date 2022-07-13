using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.UserSide.Models
{
    public class VMEmployeeDownload
    {
        public string GUID { get; set; }
        public string EmployeeID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Thumbnail { get; set; }
        public string Category { get; set; }
        public string RelatedId { get; set; }
    }

    public class LearningModel
    {
        public int? trainingId { get; set; }
        public int? courseId { get; set; }
        public int? setupModuleId { get; set; }
        public int? trainingBatch { get; set; }
        public string imageUrl { get; set; }
        public string trainingName { get; set; }
        public double? rating { get; set; }
        public string programTypeName { get; set; }
        public string materialTypeName { get; set; }
        public int? bronzeBadge { get; set; }
        public int? silverBadge { get; set; }
        public int? goldBadge { get; set; }
        public double? percentage { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public int? price { get; set; }
        public int? points { get; set; }
    }

    public class DownloadModel
    {
        public string id { get; set; }

        public string moduleId { get; set; }
        public string name { get; set; }
        public string moduleType { get; set; }
        public string contentType { get; set; }
        public string imageUrl { get; set; }
        public string courseImageUrl { get; set; }
        public string moduleImageUrl { get; set; }
        public int? duration { get; set; }
        public int? size { get; set; }
        public string path { get; set; }
        public string url { get; set; }
    }

    public class Root
    {
        public LearningModel learningModel { get; set; }
        public List<DownloadModel> downloadModel { get; set; }
    }

    public class RootNews {
        public long? NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string  ThumbnailUrl { get; set; }
        public long? NewsCategoryId { get; set; }
        public string NewsCategoryName { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }
        public bool? IsDownloadable { get; set; }
        public long? TotalView { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsOffline { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
