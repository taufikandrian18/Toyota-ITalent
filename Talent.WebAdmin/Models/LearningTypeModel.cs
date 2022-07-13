using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class LearningTypeModel
    {
        public int LearningTypeId { get; set; }
        public string LearningName { get; set; }
    }
    public class LearningTypeViewModel
    {
        public List<LearningTypeModel> ListLearningTypeModel { get; set; }
        public int TotalItem { get; set; }
    }
}
