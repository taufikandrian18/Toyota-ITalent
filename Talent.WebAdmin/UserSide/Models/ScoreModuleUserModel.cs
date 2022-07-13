using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class ScoreModuleUserModel
    {
        public int TrainingId { get; set; }
        public string EmployeeName { get; set; }
        public string DealerName { get; set; }
        public string OutletName { get; set; }
        public List<ModuleScoreModel> ScoreModuleList { get; set; }
    }

    public class ModuleScoreModel
    {
        public int SetupModulId { get; set; }
        public string ModuleName { get; set; }
        public int Score { get; set; }
    }
}
