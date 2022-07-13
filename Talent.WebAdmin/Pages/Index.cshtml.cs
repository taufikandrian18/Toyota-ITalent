using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        ILogger<IndexModel> logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            this.logger = logger;
        }
        
        public void OnGet()
        {
            this.logger.LogDebug("Log Debug: Index was called");
        }
    }
}
