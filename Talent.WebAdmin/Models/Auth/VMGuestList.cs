using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class VMGuestList
    {
        public string EmployeeID { get; set; }
        public string NIP { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string DealerName { get; set; }
        public string OutletName { get; set; }
        public string ManPowerStatus { get; set; }
        public string Status { get; set; }
        public bool IsSuspend { get; set; }
        public bool IsRequestUpgrade { get; set; }

        public bool IsDataValidation { get; set; }
        public bool IsApproved { get; set; }
        public DateTime RegisteredDate { get; set; }
    }

    public class VMGuestExport
    {
        public string EmployeeID { get; set; }
        public string NIP { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string DealerName { get; set; }
        public string OutletName { get; set; }
        public string ManPowerStatus { get; set; }
        public string ManPowerCode { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string PositionName { get; set; }
        public string Gender { get; set; }
        public string Religiion { get; set; }
        public string IdCard { get; set; }

        public string PositionNotes { get; set; }

        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisteredDate { get; set; }
    }


}
