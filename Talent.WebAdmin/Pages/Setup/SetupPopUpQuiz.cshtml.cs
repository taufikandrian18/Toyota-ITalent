using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages.Setup.SetupLearning
{
    [Authorize]
    public class SetupPopUpQuizModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PopQuizId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }

        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public SetupPopUpQuizModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }

        public IActionResult OnGet(int? PopQuizId, bool? FromOutside)
        {
            this.PopQuizId = PopQuizId;
            this.FromOutside = FromOutside == null ? false : true;

            var access = this._PrivilegeMan.AccessPage(PageEnums.SetupLearning);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}