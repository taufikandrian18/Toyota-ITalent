using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Pages.Setup
{
    public class TrainingProcessModel : PageModel
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;
        public TrainingProcessModel(UserPrivilegeSettingsService settingsService)
        {
            this._PrivilegeMan = settingsService;
        }
        public IActionResult OnGet()
        {
            var access = this._PrivilegeMan.AccessPage(PageEnums.TrainingProcess);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}