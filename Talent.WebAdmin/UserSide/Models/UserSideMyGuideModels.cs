using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideMyGuideModel
    {
        public string GuideName { get; set; }
        public string Description { get; set; }
        public int GuideTypeId { get; set; }
        public string GuideType { get; set; }
        public string ImageURL { get; set; }
    }

    public class UserSideMyGuideAPIModel
    {
        public List<UserSideMyGuideModel> GetStarted { get; set; }
        public List<UserSideMyGuideModel> FindYourPath { get; set; }
        public List<UserSideMyGuideModel> BuildYourExpertise { get; set; }
        public List<UserSideMyGuideModel> GrowYourCareer { get; set; }
        public List<UserSideMyGuideModel> ConnectWithPeople { get; set; }
        public List<UserSideMyGuideModel> RewardAndRecognition { get; set; }
        public List<UserSideMyGuideModel> Tutorial { get; set; }
    }

    public class UserSideTutorialGuideModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
