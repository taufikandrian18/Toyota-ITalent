using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing history point data.
    /// </summary>
    public class UserSideHistoryPointModel
    {
        public List<UserSideHistoryPointDetailModel> LearningPointHistories { set; get; }
        public List<UserSideHistoryPointDetailModel> TeachingPointHistories { set; get; }
        public List<UserSideHistoryPointDetailModel> TotalPointHistories { set; get; }

        public int CurrentLearningPoint { set; get; }
        public int CurrentTeachingPoint { set; get; }
        public int CurrentTotalPoint { set; get; }
    }

    /// <summary>
    /// Model class for storing history point detail data.
    /// </summary>
    public class UserSideHistoryPointDetailModel
    {
        public int Year { set; get; }
        public int RewardPointTypeId { set; get; }
        public int TotalPoint { set; get; }
    }
}
