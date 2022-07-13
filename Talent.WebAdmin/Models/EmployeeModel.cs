using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class EmployeeViewModel
    {
        public string EmployeeId { get; set; }
        public string OutletId { get; set; }
        public string EmployeeUsername { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeExperience { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public DateTime LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class EmployeeFormModel
    {
        [StringLength(64)]
        public string EmployeeId { get; set; }
        [StringLength(64)]
        public string OutletId { get; set; }
        [Required]
        [StringLength(255)]
        public string EmployeeUsername { get; set; }
        [Required]
        [StringLength(255)]
        public string EmployeeName { get; set; }
        public int EmployeeExperience { get; set; }
        [StringLength(255)]
        public string EmployeeEmail { get; set; }
        [StringLength(16)]
        public string EmployeePhone { get; set; }
        public DateTime LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class EmployeeListViewModel
    {
        public List<EmployeeViewModel> EmployeeList { get; set; }
    }

    public class EmployeeForCoachModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ListEmployeeForCoach
    {
        public List<EmployeeForCoachModel> ListEmployee { get; set; }
    }
}
