using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class GuideTypeModel
    {
        public int GuideTypeId { get; set; }
        public string Name { get; set; }
    }

    public class GuideTypeViewModel
    {
        public List<GuideTypeModel> ListGuideTypeModel { get; set; }
        public int TotalItem { get; set; }
    }
}
