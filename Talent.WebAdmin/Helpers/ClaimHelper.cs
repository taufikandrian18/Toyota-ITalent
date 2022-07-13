using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Helpers
{
    public class ClaimHelper
    {

        private readonly IContextualService ContextMan;

        public ClaimHelper(IContextualService contextual)
        {
            this.ContextMan = contextual;
        }

        public string GetLoginUserId()
        {
            try
            {
                return ContextMan.CookieClaims.EmployeeId;
            }
            catch
            {
                return "SYSTEM";
            }
        }

        public string GetLoginUserName()
        {
            try
            {
                return ContextMan.CookieClaims.Name;
            }
            catch
            {
                return "SYSTEM";
            }
        }

        public List<int> GetLoginUserPositions()
        {
            try
            {
                return ContextMan.CookieClaims.Positions;
            }
            catch
            {
                return new List<int>();
            }
        }
    }
}
