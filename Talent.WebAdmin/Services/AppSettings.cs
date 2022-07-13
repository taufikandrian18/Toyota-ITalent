using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.WebAdmin.Services
{
    public class AppSettings
    {
        public string AppUrl { set; get; }

        public string TokenSecretKey { set; get; }
        public byte[] TokenSecretKeyBinary => Encoding.UTF8.GetBytes(TokenSecretKey);

        public string SlackLogHooks { get; set; }

        public SSOSettings SSOSettings { get; set; }

        public SSOSettings MobileSSOSettings { get; set; } 

        public RedisSettings RedisSettings { get; set; }
    }

    public class SSOSettings
    {
        public string Uri { get; set; }

        public string ApiKey { get; set; }

        public Guid AppId { get; set; }
    }

    public class RedisSettings
    {
        public string IP { get; set; }

        public string InstanceName { get; set; }
    }
}
