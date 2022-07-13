using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SetupTaskCodes
    {
        public int SetupTaskId { get; set; }
        public int TaskId { get; set; }
        public int QuestionNumber { get; set; }

        [ForeignKey("SetupTaskId")]
        [InverseProperty("SetupTaskCodes")]
        public virtual SetupTasks SetupTask { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("SetupTaskCodes")]
        public virtual Tasks Task { get; set; }
    }
}
