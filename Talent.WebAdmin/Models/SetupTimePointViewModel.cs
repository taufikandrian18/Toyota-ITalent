using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TimePointViewModel
    {
        public int TimePointId { get; set; }
        public int PointTypeId { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
    public class SetupTimePoint
    {
        public int TimePointId { get; set; }
        public int PointTypeId { get; set; }
        public string PointTypeName { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public int Points { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class SetupTimePointPaginate
    {
        public List<SetupTimePoint> Data { get; set; }
        public int TotalData { get; set; }
    }

    public class SetupTimePointGridFilter: GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? Time { get; set; }
        public int? Points { get; set; }
        public string TypeofPoints { get; set; }
        public int? Score { get; set; }
    }

    public class SetupTimePointInsertModel
    {
        public int PointTypeId { get; set; }
        public int Time { get; set; }
        public int Score { get; set; }
        public int Points { get; set; }

    }
}
