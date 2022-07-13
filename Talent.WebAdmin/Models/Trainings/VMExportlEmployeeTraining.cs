using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public partial class VMExportlEmployeeTrainingApprovalInput
    {
        public long? EnrollLearningId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DealerName { get; set; }
        public string OutletName { get; set; }
        public string PositionName { get; set; }
        public bool? IsJoined { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsDrafted { get; set; }
        public long? TrainingInvitationApprovalStatus { get; set; }

        public string  CreatedAt { get; set; }
    }

    public partial class VMExportlEmployeeTrainingApproval
    {
        public string  EnrollmentTime { get; set; }
        public string AccountType { get; set; }
        public string UserType { get; set; }
        public string Username { get; set; }
        public string OCEC { get; set; }
        public string ManpowerCode { get; set; }
        public string Ktp { get; set; }
        public string CompanyName { get; set; }
        public string OutletName { get; set; }
        public string PositionGroup { get; set; }
        public string Position { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Religion { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

}
