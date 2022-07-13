using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class LoginClaims
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Username { get; set; }

        public string EmployeeId { get; set; }

        public Guid SessionId { get; set; }

        public string DealerId { get; set; }

        public List<string> BranchIds { get; set; }

        public List<string> BranchNames { get; set; }

        public List<string> Levels { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Pages { get; set; }

        public List<string> FunctionIds { get; set; }

        public List<PageCrudModel> PageIds { get; set; }

        public List<int> Positions { get; set; }

        public List<string> BranchSalesAreaIds { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var id = new ClaimsIdentity(ITalentAuthenticationSchemes.Cookie);

            id.AddClaim(new Claim(ClaimTypes.Sid, this.SessionId.ToString()));
            id.AddClaim(new Claim(ClaimTypes.PrimarySid, this.EmployeeId.ToString()));
            id.AddClaim(new Claim(ClaimTypes.Name, this.Name));
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.Username));

            var nameWord = this.Name.Split(" ");
            var alias = nameWord.ElementAtOrDefault(0).Substring(0, 1).ToUpper();

            //Check if second element is null
            if (!String.IsNullOrEmpty(nameWord.ElementAtOrDefault(1)))
            {
                alias += nameWord.ElementAtOrDefault(1).Substring(0, 1).ToUpper();
            }

            id.AddClaim(new Claim(ClaimTypes.GivenName, alias));

            if (string.IsNullOrEmpty(this.Email) == false)
            {
                id.AddClaim(new Claim(ClaimTypes.Email, this.Email));
            }

            foreach (var role in this.Roles)
            {
                id.AddClaim(new Claim(ClaimTypes.Surname, role));
            }

            if (this.Pages != null)
            {
                foreach (var page in this.Pages)
                {
                    id.AddClaim(new Claim(ClaimTypes.Role, page));
                }
            }
            return new ClaimsPrincipal(id);
        }
    }

    public class PageCrudModel : CrudModel
    {
        public string PageId { set; get; }

    }

    public class CrudModel
    {
        public bool IsCreate { set; get; }
        public bool IsRead { set; get; }
        public bool IsUpdate { set; get; }
        public bool IsDelete { set; get; }
    }

}
