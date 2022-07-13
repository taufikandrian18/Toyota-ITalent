using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages
{
    [Authorize]
    public class ComingSoonModel : PageModel
    {
        ILogger<ComingSoonModel> logger;

        public ComingSoonModel(ILogger<ComingSoonModel> logger)
        {
            this.logger = logger;
        }
        
        public void OnGet()
        {
            this.logger.LogDebug("Log Debug: Coming Soon was called");
        }
    }
}
