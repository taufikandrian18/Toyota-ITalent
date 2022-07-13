using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideEvaluationModel
    {
        public int TaskAnswerDetailId { get; set; }
        public int Score { get; set; }
    
    }
    public class TaskScoreTempEmpVM
    {
        public string EmployeeGUID { get; set; }
        public int TaskId { get; set; }
        public int TaskAnswerId { get; set; }
        public int Score { get; set; }
    }

    public class TaskAnswerModelScoreTemp
    {
        public TaskAnswerDetails TaskAnswerDetails { get; set; }
        public int TaskAnswerId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? TrainingId { get; set; }
        public string EmployeeId { get; set; }
    }
}
