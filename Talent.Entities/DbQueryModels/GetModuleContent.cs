using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetModuleContent
    {
        public int SetupModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public string FileUrl { get; set; }
        public string Mime { get; set; }
        public int point { get; set; }
        public int? Duration { get; set; }
        public string MaterialType { get; set; }
        public int? MinimumScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TestTime { get; set; }
    }
}
