using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class SuperiorEmployee
    {
        public int OrderRank { get; set; }
        public string NoReg { get; set; }
        public string Name { get; set; }
        public string PostName { get; set; }
        public string JobName { get; set; }
        public int TalentLevel { get; set; }
    }
}
