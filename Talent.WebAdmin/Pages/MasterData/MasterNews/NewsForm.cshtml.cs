using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Talent.WebAdmin.Pages.MasterData.MasterNews
{
    public class NewsFormModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int NewsId { get; set; }
        public void OnGet()
        {

        }
    }
}