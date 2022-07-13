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
    public class SurveyModel : PageModel
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        [BindProperty(SupportsGet = true)]
        public int? SurveyId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        public SurveyModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }
        public IActionResult OnGet(string type, int surveyId)
        {
            this.SurveyId = surveyId;
            this.Type = type;

            var access = this._PrivilegeMan.AccessPage(PageEnums.Survey);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();
        }
    }
}