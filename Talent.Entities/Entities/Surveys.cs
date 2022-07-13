using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Surveys
    {
        public Surveys()
        {
            ApprovalToSurveys = new HashSet<ApprovalToSurveys>();
            SurveyAnswers = new HashSet<SurveyAnswers>();
            SurveyOutletMappings = new HashSet<SurveyOutletMappings>();
            SurveyPositionMappings = new HashSet<SurveyPositionMappings>();
            SurveyQuestions = new HashSet<SurveyQuestions>();
        }

        [Key]
        public int SurveyId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [InverseProperty("Survey")]
        public virtual ICollection<ApprovalToSurveys> ApprovalToSurveys { get; set; }
        [InverseProperty("Survey")]
        public virtual ICollection<SurveyAnswers> SurveyAnswers { get; set; }
        [InverseProperty("Survey")]
        public virtual ICollection<SurveyOutletMappings> SurveyOutletMappings { get; set; }
        [InverseProperty("Survey")]
        public virtual ICollection<SurveyPositionMappings> SurveyPositionMappings { get; set; }
        [InverseProperty("Survey")]
        public virtual ICollection<SurveyQuestions> SurveyQuestions { get; set; }
    }
}
