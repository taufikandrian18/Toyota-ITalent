using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class OldUserSideTaskInsertModel
    {
        public int TaskId { get; set; }
        public int CompetencyId { get; set; }
        public int QuestionTypeId { get; set; }
        public int ModuleId { get; set; }
        public Guid? BlobId { get; set; }
        public string ImageUrl { get; set; }
        public int TaskNumber { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
        public int EvaluationTypeId { get; set; }
    }

    public class UserSideTaskInsertModel
    {
        public int TaskId { get; set; }
        public int QuestionTypeId { get; set; }
        public Guid? BlobId { get; set; }
        public string TaskImageUrl { get; set; }
        public int LayoutType { get; set; }
        public string StoryLineDescription { get; set; }
    }
}
