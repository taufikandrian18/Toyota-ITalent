using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class UserStagingModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NoReg { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
    }
}
