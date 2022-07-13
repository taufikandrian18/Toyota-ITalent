using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Pages.Auth
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task<ActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Clear();
                await this.HttpContext.SignOutAsync(ITalentAuthenticationSchemes.Cookie);
            }
            return RedirectToPage("/Index");
        }
    }
}
