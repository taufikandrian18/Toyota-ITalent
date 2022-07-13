using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models.Auth;
using Talent.WebAdmin.UserSide.Models.Configurations;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideAuthService
    {
        private readonly SsoConfiguration SsoConfiguration;
        private readonly IHttpClientFactory ClientFactory;
        private readonly AppSettings Settings;

        public UserSideAuthService(IOptions<SsoConfiguration> ssoConfiguration, IHttpClientFactory factory, AppSettings set)
        {
            this.SsoConfiguration = ssoConfiguration.Value;
            this.ClientFactory = factory;
            this.Settings = set;
        }

        /// <summary>
        /// Verify Account For Login 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PassportJWTModel> VerifyTokenSSOAsync(string token)
        {
            var client = ClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json");
            var destinationUri = this.Settings.MobileSSOSettings.Uri + "/api/v1/verify";

            var ssoResponse = await client.PostAsync(destinationUri, content);
            if (ssoResponse.IsSuccessStatusCode == false)
            {
                //Logger.LogError("Token no longer valid");
                //var err = "Token no longer valid";
                //return BadRequest(err);
                return null;
            }

            PassportJWTModel returnToken;
            using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            using (var sr = new StreamReader(resultStream))
            using (var jr = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                returnToken = serializer.Deserialize<PassportJWTModel>(jr);
            }

            return returnToken;
        }

        /// <summary>
        /// Validate Token For Login
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string ValidateMobileTokenSSO(PassportJWTModel token)
        {
            if (token.Iss != "TAM.Passport")
            {
                return "This token was not issued by TAM.Passport!";
            }
            if (token.Aud != this.Settings.MobileSSOSettings.AppId)
            {
                return "Incorrect Application ID";
            }
            if (token.Expiration < DateTimeOffset.UtcNow)
            {
                return "Token expired";
            }
            //Should be deactive for Development mode
            //if (!token.Roles.Contains(MobilePageEnum.MobileUserRoles))
            //{
            //    return "Roles not Existed";
            //}

            return null;
        }

        /// <summary>
        /// Create Apps Token After Login
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string CreateMobileToken(MobileTalentJWTModel claims)
        {
            return Jose.JWT.Encode(claims, Settings.TokenSecretKeyBinary, Jose.JwsAlgorithm.HS256);
        }

        /// <summary>
        /// Decode Apps Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        private MobileTalentJWTModel TryDecodeToken(string token)
        {
            try
            {
                var claimsJSON = JWT.Decode(token, Settings.TokenSecretKeyBinary, JwsAlgorithm.HS256);
                return JsonConvert.DeserializeObject<MobileTalentJWTModel>(claimsJSON);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Verify Apps Token to get data
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public MobileTalentJWTModel VerifyMobileToken(string token)
        {
            var claims = TryDecodeToken(token);
            if (claims == null)
            {
                return null;
            }

            if (claims.Iss != "Talent")
            {
                return null;
            }

            // 5 minutes clock skew
            if (claims.Expiration.AddMinutes(5) < DateTimeOffset.UtcNow)
            {
                return null;
            }

            return claims;
        }

    }
}