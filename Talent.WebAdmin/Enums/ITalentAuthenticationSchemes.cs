using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Enums
{
    public class ITalentAuthenticationSchemes
    {
        public const string Cookie = "ITalentIdentity";

        public const string Token = "ITalentIdentityToken";

        public const string Dual = Cookie + "," + Token;
    }
}
