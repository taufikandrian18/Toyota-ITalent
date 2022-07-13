using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class LevelModel
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
    }

    public class LevelViewModel
    {
        public List<LevelModel> ListLevelModel { get; set; }
        public int TotalItem { get; set; }
    }
}
