using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class GetAccountParameter
    {
        public List<string> Status { get; set; }
        public DateTime? RegisterdDateStart { get; set; }
        public DateTime? RegisterdDateEnd { get; set; }
        public string DealerID { get; set; }
        public string OutletID { get; set; }
        public string GUID { get; set; }

        public string PositionID { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public bool? IsSuspended { get; set; }
        public bool? IsRequetUpgrade { get; set; }

        public bool? IsDataValidation { get; set; }
        public bool Ascending { get; set; }
        public string SortBy { get; set; }
        public string SearchQuery { get; set; }
        public string Manpower { get; set; }
    }

    public class SummaryTotalCustomer
    {
        public string Status { get; set; }
        public int SumDone { get; set; }
        public int SumRequestUpgrade { get; set; }
        public int SumIsSuspended { get; set; }
    }

    public class SummaryTotalCustomerGroup
    {
        public string Status { get; set; }
        public bool Approved { get; set; }
        public bool Suspended { get; set; }
        public int Count { get; set; }
    }

    public class ListSummaryTotalCustomer { 
        public List<SummaryTotalCustomer> List { get; set; }
    }

    public class EmployeeListGuestAccount { 
    public List<EmployeeGuestModel> listEmployeeGuestAccount { get; set; }
    }

    public class EmployeeGuestModel
    {
        public string EmployeeId { get; set; }

        public string NIP { get; set; }
        public string Status { get; set; }
        public string ManpowerCode { get; set; }
        public string OutletId { get; set; }
        public string OutletCode { get; set; }
        public string OutletName { get; set; }
        public string OutletPhone { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string EmployeeUsername { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeExperience { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string Ktp { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsRequestUpgrade { get; set; }

        public bool? IsSuspended { get; set; }
        //public string IdCard { get; set; }
        public string ManpowerStatusType { get; set; }
        public string DealerPeopleCategoryCode { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionCode { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Picture { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsDataValidation { get; set; }
        public DateTime? AccountValid { get; set; }
    }

    public class EmployeeUpdateStatusVM
    {
        public string EmployeeId { get; set; }
        public string Status { get; set; }
        public bool Extend { get; set; }
        public bool? IsRequestUpgrade { get; set; }
        public bool? IsDataValidation { get; set; }
        public bool? IsSuspend { get; set; }
    }



}
