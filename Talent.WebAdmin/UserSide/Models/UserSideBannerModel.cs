using System;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing banner data.
    /// </summary>
    public class UserSideBannerModel
    {
        public string BannerId { set; get; }
        public string BannerTypeId { set; get; }
        public string BannerTypeName { set; get; }
        public string BlobId { set; get; }
        public string ImageUrl { set; get; }
        public string Name { set; get; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public int? MobilePageId { set; get; }
        public string MobilePageRoute { set; get; }
        public string ContentId { set; get; }
        public string Description { set; get; }
        public DateTime? ApprovedAt { set; get; }
        public int? TrainingId { set; get; }
        public int? CourseId { set; get; }
        public int? SetupModuleId { set; get; }
        public int? PopQuizId { set; get; }
    }
}
