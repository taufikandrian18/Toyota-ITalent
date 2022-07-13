using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Models
{
    public class RewardCreateModel
    {
        /// <summary>
        /// Reward Type Id
        /// </summary>
        [Required]
        public int RewardTypeId { get; set; }

        /// <summary>
        /// Module Id
        /// </summary>
        public int? ModuleId { get; set; }

        /// <summary>
        /// Coach Id
        /// </summary>
        public int? CoachId { get; set; }

        /// <summary>
        /// Event Id
        /// </summary>
        public int? EventId { get; set; }

        /// <summary>
        /// Reward Name
        /// </summary>
        [Required]
        public string RewardName { get; set; }

        /// <summary>
        /// time when the reward is available
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// time when the reward become unavailable
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// teaching point required
        /// </summary>
        public int? RewardRequiredTeachingPoints { get; set; }

        /// <summary>
        /// total point required
        /// </summary>
        public int? RewardRequiredTotalPoints { get; set; }

        /// <summary>
        /// learning point required
        /// </summary>
        public int? RewardRequiredLearningPoints { get; set; }

        /// <summary>
        /// is reward active, usable when active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// reward descriptions
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// reward Terms And Conditions
        /// </summary>
        [Required]
        public string TermsAndConditions { get; set; }

        /// <summary>
        /// reward description on how to use
        /// </summary>
        [Required]
        public string HowToUse { get; set; }
    }

    public class RewardFilterModel : GridFilter
    {
        /// <summary>
        /// starting filter date
        /// </summary>
        public DateTimeOffset? DateStart { get; set; }

        /// <summary>
        /// filter date end
        /// </summary>
        public DateTimeOffset? DateEnd { get; set; }

        /// <summary>
        /// filter by type of reward
        /// </summary>
        public int? RewardTypeId { get; set; }

        /// <summary>
        /// filter active and not active
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// filter by name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// filter by type of points
        /// </summary>
        public int? TypeOfPoints { get; set; }
    }

    public class RewardGridViewModel
    {
        /// <summary>
        /// reward id
        /// </summary>
        public int RewardId { get; set; }

        /// <summary>
        /// reward name
        /// </summary>
        public string RewardName { get; set; }

        /// <summary>
        /// reward type
        /// </summary>
        public string TypeOfReward { get; set; }

        /// <summary>
        /// reward point type
        /// </summary>
        public string TypeOfPoint { get; set; }

        /// <summary>
        /// active reward or not
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// reward created at time
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// reward updated at time
        /// </summary>
        public DateTime UpdateAt { get; set; }
    }

    public class RewardGridModel
    {
        /// <summary>
        /// reward gridmodel
        /// </summary>
        public List<RewardGridViewModel> grid { get; set; }

        /// <summary>
        /// total data after filter
        /// </summary>
        public int TotalFilterData { get; set; }
    }

    public class RewardViewDetailModel
    {
        /// <summary>
        /// reward id
        /// </summary>
        public int RewardId { get; set; }

        /// <summary>
        /// Reward Type detail
        /// </summary>
        public RewardTypeDropdownModel RewardType { get; set; }

        /// <summary>
        /// Module detail
        /// </summary>
        public ModuleDropdownModel Module { get; set; }

        /// <summary>
        /// Coach detail
        /// </summary>
        public CoachDropdownModel Coach { get; set; }

        /// <summary>
        /// Event detail
        /// </summary>
        public EventDropdownModel Event { get; set; }

        /// <summary>
        /// Reward Name
        /// </summary>
        public string RewardName { get; set; }

        /// <summary>
        /// time when the reward is available
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// time when the reward become unavailable
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// teaching point required
        /// </summary>
        public int? RewardRequiredTeachingPoints { get; set; }

        /// <summary>
        /// total point required
        /// </summary>
        public int? RewardRequiredTotalPoints { get; set; }

        /// <summary>
        /// learning point required
        /// </summary>
        public int? RewardRequiredLearningPoints { get; set; }

        /// <summary>
        /// is reward active, usable when active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// reward descriptions
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// reward Terms And Conditions
        /// </summary>
        public string TermsAndConditions { get; set; }

        /// <summary>
        /// reward description on how to use
        /// </summary>
        public string HowToUse { get; set; }
    }

    public class RewardUpdateModel
    {
        /// <summary>
        /// reward id
        /// </summary>
        public int RewardId { get; set; }

        /// <summary>
        /// Reward Type Id
        /// </summary>
        [Required]
        public int RewardTypeId { get; set; }

        /// <summary>
        /// Module Id
        /// </summary>
        public int? ModuleId { get; set; }

        /// <summary>
        /// Coach Id
        /// </summary>
        public int? CoachId { get; set; }

        /// <summary>
        /// Event Id
        /// </summary>
        public int? EventId { get; set; }

        /// <summary>
        /// Reward Name
        /// </summary>
        [Required]
        public string RewardName { get; set; }

        /// <summary>
        /// time when the reward is available
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// time when the reward become unavailable
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// teaching point required
        /// </summary>
        public int? RewardRequiredTeachingPoints { get; set; }

        /// <summary>
        /// total point required
        /// </summary>
        public int? RewardRequiredTotalPoints { get; set; }

        /// <summary>
        /// learning point required
        /// </summary>
        public int? RewardRequiredLearningPoints { get; set; }

        /// <summary>
        /// is reward active, usable when active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// reward descriptions
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// reward Terms And Conditions
        /// </summary>
        [Required]
        public string TermsAndConditions { get; set; }

        /// <summary>
        /// reward description on how to use
        /// </summary>
        [Required]
        public string HowToUse { get; set; }
    }
}
