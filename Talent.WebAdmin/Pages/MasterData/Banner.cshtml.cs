using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages.MasterData
{
    [Authorize]
    public class BannerModel : PageModel
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public BannerModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }

        [BindProperty(SupportsGet = true)]
        public int? BannerId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }

        public IActionResult OnGet(int? bannerId, bool? FromOutside)
        {
            this.BannerId = bannerId;
            this.FromOutside = FromOutside == null ? false : true;

            var access = this._PrivilegeMan.AccessPage(PageEnums.Banner);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}