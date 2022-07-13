using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideCoachReviewModel
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public string CoachImageUrl { get; set; }
    }

    public class UserSideTrainingReviewSubmitModel
    {
        public int TrainingId { get; set; }
        public int RatingScore { get; set; }
        public string Review { get; set; }
    }

    public class UserSideCoachReviewSubmitModel
    {
        public int TrainingId { get; set; }
        public int CoachId { get; set; }
        public int RatingScore { get; set; }
        public string Review { get; set; }
    }
}
