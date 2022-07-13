using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SetupTasks
    {
        public SetupTasks()
        {
            SetupTaskCodes = new HashSet<SetupTaskCodes>();
        }

        [Key]
        public int SetupTaskId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        public int? CompetencyId { get; set; }
        public int? ModuleId { get; set; }
        public int TestTime { get; set; }
        public bool IsGrouping { get; set; }
        public int? TotalParticipant { get; set; }
        public int? TotalQuestion { get; set; }
        public int? QuestionPerParticipant { get; set; }
        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("CompetencyId")]
        [InverseProperty("SetupTasks")]
        public virtual Competencies Competency { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("SetupTasks")]
        public virtual Modules Module { get; set; }
        [ForeignKey("PopQuizId")]
        [InverseProperty("SetupTasks")]
        public virtual PopQuizzes PopQuiz { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("SetupTasks")]
        public virtual SetupModules SetupModule { get; set; }
        [InverseProperty("SetupTask")]
        public virtual ICollection<SetupTaskCodes> SetupTaskCodes { get; set; }
    }
}
