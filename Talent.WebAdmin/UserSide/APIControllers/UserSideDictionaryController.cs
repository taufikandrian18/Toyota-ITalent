using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-dictionary")]
    public class UserSideDictionaryController : Controller
    {
        private readonly UserSideDictionaryService DictionaryMan;
        private readonly UserSideAuthService UserSideMan;

        public UserSideDictionaryController(UserSideDictionaryService service, UserSideAuthService userSide)
        {
            this.DictionaryMan = service;
            this.UserSideMan = userSide;
        }

        [HttpGet("get-all-dictionary-name-list")]
        public async Task<ActionResult<UserSideDictionaryPaginateListView>> GetAllDictionaryNameListAsync()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.DictionaryMan.GetDictionaryNameListViewAsync(response.EmployeeId);

            return data;
        }

        [HttpGet("get-dictionary-detail-view")]
        public async Task<ActionResult<List<UserSideDictionaryModel>>> GetAllDictionaryDetailViewAsync([FromQuery] Guid DictionaryId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var data = await this.DictionaryMan.GetDictionaryDetailByIdListViewAsync(DictionaryId,response.EmployeeId);

            return data;
        }
        [HttpPost("add-dictionary-isfavorite")]
        public async Task<ActionResult<BaseResponse>> AddDictionaryIsFavoriteAysnc([FromQuery] Guid dictionaryId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getData = await this.DictionaryMan.AddIsFavoriteDictionaryAsync(dictionaryId, response.EmployeeId);

            return getData;
        }
        [HttpDelete("remove-dictionary-isfavorite")]
        public async Task<ActionResult<BaseResponse>> RemoveDictionaryIsFavoriteAsync([FromQuery] Guid dictionaryId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getData = await this.DictionaryMan.RemoveIsFavoriteDictionaryAsync(dictionaryId, response.EmployeeId);

            return getData;
        }
    }
}
