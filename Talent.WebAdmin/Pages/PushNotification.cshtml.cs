using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Newtonsoft.Json;

namespace Talent.WebAdmin.Pages
{
  [Authorize]
  public class PushNotificationModel : PageModel
  {
    private readonly UserPrivilegeSettingsService _PrivilegeMan;
    private readonly IContextualService _ContextMan;

    public PushNotificationModel(UserPrivilegeSettingsService userPrivilegeSettingsService, IContextualService ics)
    {
      this._PrivilegeMan = userPrivilegeSettingsService;
      this._ContextMan = ics;
    }

    public string claims { get; set; }
    public IActionResult OnGet()
    {
      var access = this._PrivilegeMan.AccessPage(PageEnums.Operation);
      this.claims = JsonConvert.SerializeObject(this._ContextMan.CookieClaims);
      // if (access == false)
      // {
      //     return Redirect("~/Index");
      // }
      return Page();
    }
  }
}