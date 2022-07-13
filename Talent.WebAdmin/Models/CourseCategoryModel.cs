using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class CourseCategoryModel
    {
        public int CourseCategoryId { get; set; }
        public string CourseCategoryName { get; set; }
    }

    public class CourseCategoryViewModel
    {
        public List<CourseCategoryModel> ListCourseCategoryModel { get; set; }
        public int TotalItem { get; set; }
    }
}
