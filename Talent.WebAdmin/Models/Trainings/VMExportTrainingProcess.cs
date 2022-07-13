using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class VMExportTrainingProcessInput
    {
        public string EmployeeId { get; set; }
        public decimal TrainingInvitationId { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
        public string Accommodation { get; set; }
        public decimal PositionId { get; set; }
        public string PositionName { get; set; }
        public decimal? Price { get; set; }
        public string DateofStayStart { get; set; }
        public DateTime? DateofStayEnd { get; set; }
        public string  IsJoined { get; set; }
        public bool? IsConfirmed { get; set; }
        public string CreatedAt { get; set; }
    }

    public class VMExportTrainingProcess { 
        public string EnrollmentTime { get; set; }
        public string AccountType { get; set; }
        public string UserType { get; set; }
        public string Username { get; set; }
        public string OCEC { get; set; }
        public string ManpowerCode { get; set; }
        public string Ktp { get; set; }
        public string CompanyName { get; set; }
        public string OutletName { get; set; }
        public string PositionGroup { get; set; }
        public string Position { get; set;  }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Religion { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
