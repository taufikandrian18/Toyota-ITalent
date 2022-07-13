using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages.Setup
{
    [Authorize]
    public class SetupTSLModel : PageModel
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public SetupTSLModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }
        public IActionResult OnGet()
        {
            var access = this._PrivilegeMan.AccessPage(PageEnums.SetupTSL);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}