using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class StagingDealerEmployeeModel
    {
        
        [StringLength(250)]
        public string Code { get; set; }

        [StringLength(250)]
        public string SupervisorId { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public decimal? OutletId { get; set; }
        [StringLength(10)]
        public string LastManpowerPositionTypeId { get; set; }
        [StringLength(10)]
        public string ManpowerStatusTypeId { get; set; }
        [StringLength(50)]
        public string EmployeeId { get; set; }
        [StringLength(64)]
        public string Phone { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(250)]
        public string State { get; set; }
        public int Id { get; set; }
        [StringLength(10)]
        public string ManpowerTypeId { get; set; }
        [StringLength(1)]
        public string Sex { get; set; }
        [StringLength(64)]
        public string Ktp { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? ResignedAt { get; set; }
    }
}
