using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    [Table("HomePage_AllPopularTraining")]
    public class HomePage_AllPopularTraining
    {
        public HomePage_AllPopularTraining()
        {
        }
        [Key]
        public int Id { get; set; }
        public int? TrainingId { get; set; }
        public int? Batch { get; set; }
        public int? CourseId { get; set; }
        [StringLength(255)]
        public string CourseName { get; set; }
        [StringLength(128)]
        public string ProgramTypeName { get; set; }
        public int? SetupModuleId { get; set; }
        [StringLength(255)]
        public string ModuleName { get; set; }
        public Guid BloblId { get; set; }
        [StringLength(int.MaxValue)]
        public string Mime { get; set; }
        [StringLength(64)]
        public string MaterialTypeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ApprovedAt { get; set; }
        public int? Top5Course { get; set; }
        public int? WhoTookWhoLike { get; set; }
        public int? PositionId { get; set; }
        [StringLength(64)]
        public string OutletId { get; set; }

    }
}
