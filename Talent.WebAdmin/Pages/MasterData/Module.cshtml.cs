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
    public class ModuleModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? ModuleId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }
        
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public ModuleModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }
        public IActionResult OnGet(int? moduleId, bool? fromOutside)
        {
            this.ModuleId = moduleId;
            this.FromOutside = fromOutside == null ? false : true;

            var access = this._PrivilegeMan.AccessPage(PageEnums.Module);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}