using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/mdm")]
    [ApiController]
    public class MDMApiController : Controller
    {
        private readonly StagingService StagingMan;
        private readonly AppSettings Settings;
        private readonly IHttpClientFactory ClientFactory;

        public MDMApiController(StagingService service, AppSettings settings, IHttpClientFactory ihcf)
        {
            this.StagingMan = service;
            this.Settings = settings;
            this.ClientFactory = ihcf;
        }

        /// <summary>
        /// insert data from MDM to StagingUser
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("user")]
        public async Task<IActionResult> PostUser([FromBody]List<StagingUser> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageUser(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingActualOrganizationStructure
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("actual-organization-structure")]
        public async Task<IActionResult> PostActualOrganizationStructure([FromBody]List<StagingActualOrganizationStructure> models)
        {
            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();

            //if (ModelState.IsValid == false)
            //{
            //    return BadRequest(errors);
            //}

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageActualOrganizationStructure(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingOrganizationObject
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("organization-object")]
        public async Task<IActionResult> PostOrganizationObject([FromBody]List<StagingOrganizationObject> models)
        {
            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();

            //if (ModelState.IsValid == false)
            //{
            //    return BadRequest(errors);
            //}

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageOrganizationObject(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingDealerEmployee
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("dealer-employee")]
        public async Task<IActionResult> PostDealerEmployee([FromBody]List<StagingDealerEmployeeModel> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageDealerEmployee(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingManpowerPositionType
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("manpower-position-type")]
        public async Task<IActionResult> PostManpowerPositionType([FromBody]List<StagingManpowerPositionType> models)
        {
            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();

            //if (ModelState.IsValid == false)
            //{
            //    return BadRequest(errors);
            //}

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageManpowerPositionType(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingManpowerPositionType
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("manpower-type")]
        public async Task<IActionResult> PostManpowerType([FromBody]List<StagingManpowerType> models)
        {
            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();

            //if (ModelState.IsValid == false)
            //{
            //    return BadRequest(errors);
            //}

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageManpowerType(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingOutlet
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("outlet")]
        public async Task<IActionResult> PostOutlet([FromBody]List<StagingOutlet> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageOutlet(models);
            return Ok();
        }

        // Sempat error karena data dari MDM yang dikasih kebanyakan
        // Solusi : Sekarang data yang dikasih dari MDM dibagi per batch
        /// <summary>
        /// insert data from MDM to StagingRegion
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("region")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> PostRegion([FromBody]List<StagingRegion> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageRegion(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingSalesArea
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("sales-area")]
        public async Task<IActionResult> PostSalesArea([FromBody]List<StagingSalesArea> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageSalesArea(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to to StagingAfterSalesArea
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("after-sales-area")]
        public async Task<IActionResult> PostAfterSalesArea([FromBody]List<StagingAfterSalesArea> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageAfterSalesArea(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingDealerGroup
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("dealer-group")]
        public async Task<IActionResult> PostDealerGroup([FromBody]List<StagingDealerGroup> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageDealerGroup(models);
            return Ok();
        }

        /// <summary>
        /// insert data from MDM to StagingDealerCompany
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("dealer-company")]
        public async Task<IActionResult> PostDealerCompany([FromBody]List<StagingDealerCompany> models)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid == false)
            {
                return BadRequest(errors);
            }

            //var hasJwt = Request.Headers.ContainsKey("SsoJwt");
            //if (hasJwt == false)
            //{
            //    //Logger.LogError("TAM.Passport JSON Web Token not found!");
            //    var err = "Request header JSON Web Token was not found!";
            //    return BadRequest(err);
            //}

            ////JWT We Received From MDM
            //var tamSignOnToken = Request.Headers["SsoJwt"].FirstOrDefault();
            //var ssoUri = Settings.SSOSettings.Uri;

            ////Send to SSO
            //var client = ClientFactory.CreateClient();
            //var content = new StringContent(JsonConvert.SerializeObject(tamSignOnToken), Encoding.UTF8, "application/json");
            //var destinationUri = ssoUri + "/api/v1/verify";
            //var ssoResponse = await client.PostAsync(destinationUri, content);
            //if (ssoResponse.IsSuccessStatusCode == false)
            //{
            //    //Logger.LogError("Token no longer valid");
            //    var err = "Token no longer valid";
            //    return BadRequest(err);
            //}

            //PassportJWTModel token;
            //using (var resultStream = await ssoResponse.Content.ReadAsStreamAsync())
            //using (var sr = new StreamReader(resultStream))
            //using (var jr = new JsonTextReader(sr))
            //{
            //    var serializer = new JsonSerializer();
            //    token = serializer.Deserialize<PassportJWTModel>(jr);
            //}

            //var error = ValidateForError(token);
            //if (string.IsNullOrEmpty(error) == false)
            //{
            //    //Logger.LogError(error);
            //    return BadRequest(error);
            //}

            ////Should contain our role
            //var isMdmRole = token.Roles.Contains("MDM Integration");
            //if (isMdmRole == false)
            //{
            //    //Logger.LogError("MDM attempting to log in without having \"MDM Integration\" Role");
            //    var err = "MDM attempting to log in without having \"MDM Integration\" Role";
            //    return BadRequest(err);
            //}

            await StagingMan.StageDealerCompany(models);
            return Ok();
        }
        private string ValidateForError(PassportJWTModel token)
        {
            if (token.Iss != "TAM.Passport")
            {
                return "This token was not issued by TAM.Passport!";
            }

            var appId = Settings.SSOSettings.AppId;
            if (token.Aud != appId)
            {
                return "Incorrect Application ID";
            }

            if (token.Expiration < DateTimeOffset.UtcNow)
            {
                return "Token expired";
            }
            return null;
        }
    }
}
