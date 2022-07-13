using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideSurveyInsertModel
    {
        public int SurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public Guid? BlobId { get; set; }
        public string TaskImageUrl { get; set; }
    }
}
