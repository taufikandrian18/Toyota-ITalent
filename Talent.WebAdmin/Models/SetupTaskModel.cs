using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SetupTaskModel
    {
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

        public List<SetupTaskCodeModel> ListSetupTaskCodes { get; set; }
    }

    public class SetupTaskCodeModel
    {
        public int SetupTaskId { get; set; }
        public int TaskId { get; set; }
        public int QuestionNumber { get; set; }
    }
}
