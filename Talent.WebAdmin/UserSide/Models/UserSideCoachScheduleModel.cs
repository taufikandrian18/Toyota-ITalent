using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideCoachScheduleViewModel
    {
        public int CoachId { get; set; }
        public int CoachScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsBooked { get; set; }
    }

    public class UserSideCoachScheduleFormModel
    {
        [Required]
        public string ScheduleName { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
    }

    public class UserSideCoachScheduleUpdateModel
    {
        public int CoachScheduleId { get; set; }
        [Required]
        public string ScheduleName { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
    }

    public class UserSideCoachScheduleBookingModel
    {
        public int CoachId { get; set; }
        public string Employeeid { get; set; }
        public string ImageUrl { get; set; }
        public string EmployeeName { get; set; }
        public UserSideTotalBadgeModel TotalBadge { get; set; }
        public List<UserSideCoachScheduleViewModel> ScheduleList { get; set; }
    }

    public class UserSideScheduleBookingModel
    {
        public string CoachName { get; set; }
        public string RequesterName { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string Message { get; set; }
    }
}