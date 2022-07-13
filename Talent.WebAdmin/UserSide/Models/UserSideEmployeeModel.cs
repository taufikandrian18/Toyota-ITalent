using System;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideEmployeeModel
    {
        public string EmployeeId { get; set; }
        public string ImageUrl { get; set; }
        public string EmployeeUsername { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Religion { get; set; }
        public string ManpowerCode { get; set; }
        public string ManpowerStatus { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsCoach { get; set; }
        public bool IsTeamLeader { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsRequestUpgrade { get; set; }
        public int? CoachId { get; set; }
        public int? TeamId { get; set; }
        public string NIP { get; set; }
        public string MDMCode { get; set; }
        public string PositionNote { get; set; }
        public string Ktp { get; set; }
    }
}