using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetUserSideRankModel
    {
        public int Rank { set; get; }
        public string EmployeeId { set; get; }
        public int EmployeeExperience { set; get; }
    }
}
