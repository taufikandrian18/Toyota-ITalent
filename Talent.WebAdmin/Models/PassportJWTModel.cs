using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class PassportJWTModel
    {
        public string Iss { get; set; }

        public Guid Aud { get; set; }

        public string Sub { get; set; }

        public int Iat { get; set; }

        public DateTime IssuedAt { get; set; }

        public int Exp { get; set; }

        public DateTimeOffset Expiration { get; set; }

        public Guid Jti { get; set; }

        public string Unique_name { get; set; }

        public string Email { get; set; }

        public string EmployeeId { get; set; }

        public List<string> Roles { get; set; }
    }
}
