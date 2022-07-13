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
    public class EventModel : PageModel
    {
        public int? EventId { get; set; }

        public bool? FromOutside { get; set; }

        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public EventModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }

        public IActionResult OnGet(int? eventId, bool? fromOutside)
        {
            this.EventId = eventId;
            this.FromOutside = fromOutside == null ? false : true;
            var access = this._PrivilegeMan.AccessPage(PageEnums.Event);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}