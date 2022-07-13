using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages.MasterData
{
    public class NewsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int NewsId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public NewsModel(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }

        public IActionResult OnGet(string type, int newsId)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = "view";
            }
            else if (type.ToLower().Contains("add"))
            {
                type = "add";
            }
            else if (type.ToLower().Contains("update"))
            {
                type = "update";
            }
            else if (type.ToLower().Contains("detail"))
            {
                type = "detail";
            }

            this.Type = type;
            //this.NewsId = newsId.HasValue? 0: newsId.Value;
            this.NewsId = newsId;

            var access = this._PrivilegeMan.AccessPage(PageEnums.News);
            if (access == false)
            {
                return Redirect("~/Index");
            }
            return Page();

        }
    }
}