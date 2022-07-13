using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing reward data.
    /// </summary>
    public class UserSideRewardModel
    {
        public int RewardId { set; get; }
        public int RewardTypeId { set; get; }
        public string Name { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public string RewardTypeName { set; get; }
        public string Description { set; get; }
        public string TermsAndConditions { set; get; }
        public string HowToUse { set; get; }
        public int TeachingPoint { set; get; }
        public int LearningPoint { set; get; }
        public int TotalPoint { set; get; }
    }
}
