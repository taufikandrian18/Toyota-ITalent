using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class TrainingRatings
    {
        [Key]
        public int TrainingRatingId { get; set; }
        [Required]
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int TrainingId { get; set; }
        public int? CourseId { get; set; }
        public int RatingScore { get; set; }
        [StringLength(512)]
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("TrainingRatings")]
        public virtual Courses Course { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("TrainingRatings")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TrainingId")]
        [InverseProperty("TrainingRatings")]
        public virtual Trainings Training { get; set; }
    }
}
