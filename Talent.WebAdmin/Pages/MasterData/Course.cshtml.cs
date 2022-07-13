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
    public class CourseModel : PageModel
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public CourseModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }
        [BindProperty(SupportsGet = true)]
        public int? CourseId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? FromOutside { get; set; }
        public IActionResult OnGet(int? courseId, bool? fromOutside)
        {
            this.CourseId = courseId;
            this.FromOutside = fromOutside == null ? false : true;
            var access = this._PrivilegeMan.AccessPage(PageEnums.Course);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}