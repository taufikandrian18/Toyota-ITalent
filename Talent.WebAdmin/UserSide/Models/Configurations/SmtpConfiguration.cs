using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models.Configurations
{
    /// <summary>
    /// SMTP configuration based on JSON structure in SMTP section in appsettings.
    /// </summary>
    public class SmtpConfiguration
    {
        public string SenderName { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsSsl { get; set; }
    }
}
