using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TimePointsModel
    {
        public int TimePointId { get; set; }
        public int PointTypeId { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TimePointTaskModel
    {
        public int TimePointId { get; set; }
        public int? Score { get; set; }
        public int? Time { get; set; }
        public int Points { get; set; }
    }

    public class TaskViewForSetupModuleModel
    {

    }
}
