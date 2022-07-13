using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class CourseOverviewQueryModel
    {
        public int? SetupModuleId { get; set; }
        public int? ModuleId { get; set; }

        public float? Orders { get; set; }
        public string ContentName { get; set; }

        public string ModuleType { get; set; }
        public int? CourseId { get; set; }

        public Guid? BlobId { get; set; }
        public Guid? MaterialBlobId { get; set; }
        public string Mime { get; set; }
        public string MaterialMime { get; set; }

        public int? ModuleTimeLength { get; set; }
        public bool? IsForRemedial { get; set; }

        public int? ModuleTotalPoints { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime? ModuleStartTime { get; set; }
        public DateTime? ModuleEndTime { get; set; }
    }

    public class SetupModuleOverviewQueryModel
    {
        public int SetupModuleId { get; set; }
        public int ModuleId { get; set; }
        public float Orders { get; set; }

        public string ContentName { get; set; }

        public string ModuleType { get; set; }

        public Guid BlobId { get; set; }

        public string Mime { get; set; }

        public int? ModuleTimeLength { get; set; }
        public bool? IsForRemedial { get; set; }

        public int? ModuleTotalPoints { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class CourseOverviewQueryWithoutElModel
    {
        public int SetupModuleId { get; set; }
        public int? ModuleId { get; set; }
        public float Orders { get; set; }


        public string ContentName { get; set; }

        public string ModuleType { get; set; }

        public Guid BlobId { get; set; }

        public string Mime { get; set; }

        public int? ModuleTimeLength { get; set; }

        public int? ModuleTotalPoints { get; set; }
    }
}
