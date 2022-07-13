using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Positions
    {
        public Positions()
        {
            EmployeePositionMappings = new HashSet<EmployeePositionMappings>();
            EventPositionMappings = new HashSet<EventPositionMappings>();
            PositionCompentencyMappings = new HashSet<PositionCompentencyMappings>();
            Roles = new HashSet<Roles>();
            SurveyPositionMappings = new HashSet<SurveyPositionMappings>();
            TrainingPositionMappings = new HashSet<TrainingPositionMappings>();
        }

        [Key]
        public int PositionId { get; set; }
        [Required]
        [StringLength(64)]
        public string PositionName { get; set; }
        [Required]
        public string PositionDescription { get; set; }
        public bool IsTamPeople { get; set; }
        [Required]
        [StringLength(250)]
        public string PositionCode { get; set; }

        public bool IsOtherDealer { get; set; }

        [InverseProperty("Position")]
        public virtual ApprovalPositionMappings ApprovalPositionMappings { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<EmployeePositionMappings> EmployeePositionMappings { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<EventPositionMappings> EventPositionMappings { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<PositionCompentencyMappings> PositionCompentencyMappings { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<Roles> Roles { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<SurveyPositionMappings> SurveyPositionMappings { get; set; }
        [InverseProperty("Position")]
        public virtual ICollection<TrainingPositionMappings> TrainingPositionMappings { get; set; }

        [InverseProperty("Position")]
        public virtual ICollection<TrainingPositionOnlyViewMappings> TrainingPositionOnlyViewMappings { get; set; }
    }
}
