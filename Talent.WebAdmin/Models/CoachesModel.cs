using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CoachesModel
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public bool CoachIsActive { get; set; }
        public string Category { get; set; }
        public string EmployeeId { get; set; }
        public int? EbadgeId { get; set; }
        public int? CoachScheduleId { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string EbadgeName { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedAt { get; set; }
        public DateTime UpdatedAtDate { get; set; }
        public DateTime CreatedAtDate { get; set; }
        public DateTime? StartDateTimeDate { get; set; }
        public DateTime? EndDateTimeDate { get; set; }
    }

    public class CoachesViewModel
    {
        public List<CoachesModel> ListCoaches { get; set; }
        public int TotalData { get; set; }
    }

    public class CoachesFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DateFilterStart { get; set; }
        public DateTimeOffset? DateFilterEnd { get; set; }
        public string CoachName { get; set; }
        public string TopicName { get; set; }
        public bool? CoachIsActive { get; set; }
        public string EbadgeName { get; set; }
        public string Category { get; set; }
    }

    public class CoachFormModel
    {
        public int? CoachId { get; set; }
        [Required]
        [StringLength(64)]
        public string CoachName { get; set; }
        public bool CoachIsActive { get; set; }
        public string Category { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public List<CoachSchedulesFormModel> CoachSchedule { get; set; }
        [Required]
        public List<int> TopicId { get; set; }
    }

    public class CoachSchedulesFormModel
    {
        public int? CoachScheduleId { get; set; }
        public string StartDateTime { get; set; }
        public string StartDate { get; set; }
        public DateTime? StartDateTimeDate { get; set; }
        public string EndDateTime { get; set; }
        public DateTime? EndDateTimeDate { get; set; }
    }

    public class CoachViewDetail
    {
        public List<TopicsEbadgeJoinModelForCoach> ListTopicEbadge { get; set; }
        public List<CoachSchedulesFormModel> ListCoachSchedule { get; set; }
    }

    public class CoachDeleteFormModel
    {
        public List<int> ScheduleId { get; set; }
        public List<int> TopicId { get; set; }
        public int CoachId { get; set; }
        public bool DeleteAll { get; set; }
    }

}
