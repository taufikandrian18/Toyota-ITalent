using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetContinuedLearningModel
    {
        public int? TrainingId { get; set; }
        public int? CourseId { get; set; }
        public int? SetupModuleId { get; set; }
        public string ModuleName { set; get; }
        public int? Batch { get; set; }
        public Guid BlobId { get; set; }
        public string MIME { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public string MaterialTypeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
