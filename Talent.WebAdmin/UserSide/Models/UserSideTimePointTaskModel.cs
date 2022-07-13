using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTimePointTaskModel
    {
        public int TimePointId { get; set; }
        public int? Score { get; set; }
        public int Points { get; set; }
    }

    public class UserSideTimeAndPointModel
    {
        public int? Point { get; set; }
        public int? Time { get; set; }
    }
}
