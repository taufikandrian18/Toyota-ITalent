using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideCoachViewModel
    {
        /// <summary>
        /// Coach ID.
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// Coach is Active
        /// </summary>
        public int CoachIsActive { get; set; }
    }

    public class UserSideCoachModel
    {
        public int CoachId { get; set; }
        public int CoachIsActive { get; set; }
        public string EmployeeId { get; set; }
        public string ImageUrl { get; set; }
        public string EmployeeName { get; set; }

        public List<UserSideTopicModel> CoachTopics { get; set; }
        public UserSideTotalBadgeModel TotalBadge { get; set; }
    }

    public class UserSideTopicModel
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int? EbadgesId { get; set; }
        public Guid? BlobId { get; set; }
    }

    public class UserSideCoachResponseModel
    {
        public List<UserSideCoachModel> CoachList { get; set; }
        public int TotalData { get; set; }
    }

    public class UserSideCoachFilterModel
    {
        /// <summary>
        /// Page Index (current page).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size (data per-page).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Keyword (search name)
        /// </summary>
        public string Keyword { get; set; }
    }
}