using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public interface IContextualService
    {
        string ClientIpAddress { get; }

        LoginClaims CookieClaims { get; }
    }

    public class WebContextualService : IContextualService
    {
        private LoginClaims _CookieClaims;
        private readonly IHttpContextAccessor ContextAccessor;
        private readonly AppSettings Settings;

        public WebContextualService(IHttpContextAccessor contextAccessor, AppSettings settings)
        {
            this.ContextAccessor = contextAccessor;
            this.Settings = settings;
        }

        public string ClientIpAddress
        {
            get
            {
                return ContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        public LoginClaims CookieClaims
        {
            get
            {
                var user = ContextAccessor.HttpContext.User;

                if (user.Identity.IsAuthenticated == false)
                {
                    return null;
                }

                if (_CookieClaims != null)
                {
                    return _CookieClaims;
                }

                var cookieClaims = user.Identities
                    .Where(Q => Q.AuthenticationType == ITalentAuthenticationSchemes.Cookie)
                    .FirstOrDefault();

                if (cookieClaims == null)
                {
                    return null;
                }

                var claims = new LoginClaims();
                claims.SessionId = Guid.Parse(cookieClaims.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.Sid).Value);
                claims.EmployeeId = cookieClaims.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.PrimarySid).Value;
                claims.Name = cookieClaims.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.Name).Value;
                claims.Alias = cookieClaims.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.GivenName).Value;
                claims.Username = cookieClaims.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.NameIdentifier).Value;
                claims.Email = cookieClaims.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.Email)?.Value;
                claims.Roles = cookieClaims.Claims.Where(Q => Q.Type == ClaimTypes.Surname).Select(Q => Q.Value).ToList();
                //claims.Pages = cookieClaims.Claims.Where(Q => Q.Type == ClaimTypes.Role).Select(Q => Q.Value).ToList();

                claims.FunctionIds = ContextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>("Functions");
                claims.Levels = ContextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>("Levels");

                try
                {
                    claims.PageIds = ContextAccessor.HttpContext.Session.GetObjectFromJson<List<PageCrudModel>>("Pages");
                    claims.DealerId = ContextAccessor.HttpContext.Session.GetString("DealerId");
                    claims.BranchIds = ContextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>("BranchIds");
                    claims.BranchNames = ContextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>("BranchNames");
                    claims.BranchSalesAreaIds = ContextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>("BranchSalesAreaIds");
                }
                catch (Exception)
                { }

                if (claims.BranchIds == null)
                {
                    claims.BranchIds = new List<string>();
                }

                // Lazy-loaded claims. Register service as Scoped for best effect.
                _CookieClaims = claims;
                return claims;
            }
        }
    }
}
