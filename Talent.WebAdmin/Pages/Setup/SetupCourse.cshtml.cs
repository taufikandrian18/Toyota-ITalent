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
    public class SetupCourseModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SetupCourseId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }

        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public SetupCourseModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }
        public IActionResult OnGet(int? setupCourseId, bool? fromOutside)
        {
            this.SetupCourseId = setupCourseId;
            this.FromOutside = fromOutside == null ? false : true;

            var access = this._PrivilegeMan.AccessPage(PageEnums.SetupLearning);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}