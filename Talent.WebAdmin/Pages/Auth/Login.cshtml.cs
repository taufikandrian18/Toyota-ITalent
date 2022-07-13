using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly AppSettings Settings;
        private readonly IHttpClientFactory HttpClientFactoryMan;
        private readonly AuthService AuthMan;

        [Required]
        [BindProperty]
        public string TAMSignOnToken { set; get; }

        public string Error { get; set; }

        public Guid AppId { get; set; }

        public string SsoUri { get; set; }

        public LoginModel(AppSettings set, IHttpClientFactory ihcs, AuthService asm)
        {
            this.Settings = set;
            this.HttpClientFactoryMan = ihcs;
            this.AuthMan = asm;
        }

        public void GetSettings()
        {
            this.AppId = Settings.SSOSettings.AppId;
            this.SsoUri = Settings.SSOSettings.Uri;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/");
            }
            this.GetSettings();
            return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl)
        {
            this.Error = null;
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/");
            }
            if (ModelState.IsValid == false)
            {
                return new BadRequestObjectResult("Token is required.");
            }
            this.GetSettings();

            var client = HttpClientFactoryMan.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(TAMSignOnToken), Encoding.UTF8, "application/json");
            var destinationUri = SsoUri + "/api/v1/verify";
            var response = await client.PostAsync(destinationUri, content);
            if (response.IsSuccessStatusCode == false)
            {
                return new BadRequestObjectResult("Failed to verify token!");
            }

            PassportJWTModel token;
            using (var resultStream = await response.Content.ReadAsStreamAsync())
            using (var sr = new StreamReader(resultStream))
            using (var jr = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                token = serializer.Deserialize<PassportJWTModel>(jr);
            }

            var isValid = ValidateToken(token);
            if (isValid == false)
            {
                this.Error = "Token invalid";
                return Page();
            }

            //Login
            var claims = await AuthMan.TryLogin(token);
            if(claims == null)
            {
                this.Error = "You have no access in iTalent";
                this.TAMSignOnToken = null;
                return Page();
            }

            var cookie = claims.ToClaimsPrincipal();
            var duration = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddHours(12),
                IsPersistent = false
            };

            await HttpContext.SignInAsync(ITalentAuthenticationSchemes.Cookie, cookie, duration);

            if (string.IsNullOrEmpty(returnUrl) == false && Url.IsLocalUrl(returnUrl) == true)
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/Index");
        }

        private bool ValidateToken(PassportJWTModel token)
        {
            if (token.Iss != "TAM.Passport")
            {
                return false;
            }
            if (token.Aud != this.AppId)
            {
                return false;
            }
            if (token.Expiration < DateTimeOffset.UtcNow)
            {
                return false;
            }
            return true;
        }
    }
}