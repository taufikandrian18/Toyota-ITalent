using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models.Configurations
{
    public class SsoConfiguration
    {
        public string Uri { get; set; }
        public string AppId { get; set; }
        public string ApiKey { get; set; }
    }
}
