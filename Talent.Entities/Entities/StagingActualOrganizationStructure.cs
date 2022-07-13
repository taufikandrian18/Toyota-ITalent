using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingActualOrganizationStructure
    {
        [Column("ID")]
        public int Id { get; set; }
        [Key]
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(20)]
        public string NoReg { get; set; }
        [StringLength(20)]
        public string PostCode { get; set; }
        [StringLength(150)]
        public string PostName { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? Staffing { get; set; }
        [StringLength(250)]
        public string State { get; set; }
        [StringLength(20)]
        public string OrgCode { get; set; }
        [StringLength(20)]
        public string ParentOrgCode { get; set; }
        [StringLength(150)]
        public string OrgName { get; set; }
        [StringLength(20)]
        public string JobCode { get; set; }
        [StringLength(150)]
        public string JobName { get; set; }
        [Column("MUID")]
        public Guid? Muid { get; set; }
        [StringLength(50)]
        public string VersionName { get; set; }
        public int? VersionNumber { get; set; }
        [Column("Version_ID")]
        public int? VersionId { get; set; }
        [StringLength(50)]
        public string VersionFlag { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public int? ChangeTrackingMask { get; set; }
        [StringLength(50)]
        public string Service { get; set; }
        [StringLength(10)]
        public string EmployeeGroup { get; set; }
        [StringLength(150)]
        public string EmployeeGroupText { get; set; }
        [StringLength(50)]
        public string EmployeeSubgroup { get; set; }
        [StringLength(150)]
        public string EmployeeSubgroupText { get; set; }
        [StringLength(10)]
        public string WorkContract { get; set; }
        [StringLength(150)]
        public string WorkContractText { get; set; }
        [StringLength(10)]
        public string PersonalArea { get; set; }
        [StringLength(10)]
        public string PersonalSubarea { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? DepthLevel { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? Chief { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? OrgLevel { get; set; }
        public DateTime? Period { get; set; }
        [Column(TypeName = "decimal(38, 0)")]
        public decimal? Vacant { get; set; }
        [StringLength(500)]
        public string Structure { get; set; }
        [StringLength(50)]
        public string ObjectDescription { get; set; }
        public DateTime? EnterDateTime { get; set; }
        [StringLength(100)]
        public string EnterUserName { get; set; }
        public int? EnterVersionNumber { get; set; }
        public DateTime? LastChgDateTime { get; set; }
        [StringLength(100)]
        public string LastChgUserName { get; set; }
        public int? LastChgVersionNumber { get; set; }
        [StringLength(250)]
        public string ValidationStatus { get; set; }
    }
}
