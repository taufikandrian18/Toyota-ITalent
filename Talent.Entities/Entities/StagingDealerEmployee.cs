using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingDealerEmployee
    {
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(250)]

        public string ParentCode { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
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
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(10)]
        public string ManpowerTypeId { get; set; }
        [StringLength(1)]
        public string Sex { get; set; }
        [Column("KTP")]
        [StringLength(64)]
        public string Ktp { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? DateOfBirth { get; set; }
    }
}
