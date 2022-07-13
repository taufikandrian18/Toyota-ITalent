using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class UserSideCourseData
    {
        public int TrainingId { get; set; }
        public int Batch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public int Points { get; set; }
        public int? CoursePrice { get; set; }
        public int? Time { get; set; }
        public string TopicName { get; set; }
        public Guid BlobId { get; set; }
        public string Mime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }        
    }
}
