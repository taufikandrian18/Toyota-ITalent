using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;

namespace Template.Pages
{
    [Authorize]
    public class GuideModel : PageModel
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        [BindProperty(SupportsGet = true)]
        public int? GuideId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }

        public GuideModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }

        public IActionResult OnGet(int? guideId, bool? fromOutside)
        {
            this.GuideId = guideId;
            this.FromOutside = fromOutside == null ? false : true;

            var access = this._PrivilegeMan.AccessPage(PageEnums.Guide);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}