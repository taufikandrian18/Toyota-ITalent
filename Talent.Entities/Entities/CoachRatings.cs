using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CoachRatings
    {
        [Key]
        public int CoachRatingId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int TrainingId { get; set; }
        public int CoachId { get; set; }
        public int RatingScore { get; set; }
        [StringLength(512)]
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CoachId")]
        [InverseProperty("CoachRatings")]
        public virtual Coaches Coach { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("CoachRatings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("CoachRatings")]
        public virtual Trainings Training { get; set; }
    }
}
