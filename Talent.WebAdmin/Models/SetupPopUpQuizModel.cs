using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SetupPopUpQuizCreateModel
    {
        public string QuizTitle { get; set; }
        public int ApprovalId { get; set; }
        public int TestTime { get; set; }
        public bool IsGrouping { get; set; }
        public int? CompetencyId { get; set; }
        public int? ModuleId { get; set; }
        public int? TotalParticipant { get; set; }
        public int TotalQuestion { get; set; }
        public int? QuestionPerParticipant { get; set; }
        public List<int> TaskIds { get; set; }
    }

    public class SetupPopQuizDetailModel
    {
        public int PopQuizId { get; set; }
        public string QuizTitle { get; set; }
        public int SetupTaskId { get; set; }
        public int TestTime { get; set; }
        public bool IsGrouping { get; set; }
        public int? CompetencyId { get; set; }
        public int? ModuleId { get; set; }
        public int? TotalParticipant { get; set; }
        public int? TotalQuestion { get; set; }
        public int? QuestionPerParticipant { get; set; }
        public List<TaskCodeSetupModel> TaskIds { get; set; }
    }

    public class SetupPopQuizUpdateModel
    {
        public int PopQuizId { get; set; }
        public string QuizTitle { get; set; }
        public int ApprovalId { get; set; }
        public int TestTime { get; set; }
        public bool IsGrouping { get; set; }
        public int? CompetencyId { get; set; }
        public int? ModuleId { get; set; }
        public int? TotalParticipant { get; set; }
        public int TotalQuestion { get; set; }
        public int? QuestionPerParticipant { get; set; }
        public List<int> TaskIds { get; set; }
    }
}
