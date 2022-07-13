using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages.Setup.Tasks
{
    [Authorize]
    public class SequenceModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? TaskId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public SequenceModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }
        public IActionResult OnGet(int? taskId, bool? fromOutside)
        {
            this.TaskId = taskId;
            this.FromOutside = fromOutside == null ? false : true;
            var access = this._PrivilegeMan.AccessPage(PageEnums.Task);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}