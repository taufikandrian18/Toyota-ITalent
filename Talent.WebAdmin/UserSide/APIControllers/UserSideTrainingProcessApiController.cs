using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/mobile/training-process")]
    public class UserSideTrainingProcessApiController : Controller
    {
        private readonly UserSideTrainingProcessService TrainingProcessserviceMan;
        private readonly UserSideAuthService UserSideMan;
        public UserSideTrainingProcessApiController(UserSideTrainingProcessService traingingProcessService, UserSideAuthService userSide)
        {
            this.TrainingProcessserviceMan = traingingProcessService;
            this.UserSideMan = userSide;
        }

        /// <summary>
        /// API for get Training invitaion List.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("get-paginate",Name = "UserSide-Training-Process-GetPaginate")]
        public async Task<ActionResult<List<UserSideTrainingProcessViewModel>>> UserSideTrainingProcessGetPaginateAsync([FromQuery]UserSideTrainingProcessFilterModel filter)
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

            var getList = await TrainingProcessserviceMan.GetTrainingNotifListAsync(response.EmployeeId, filter);

            return getList;
        }

        /// <summary>
        /// API for get accomodation.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-accomodation", Name = "UserSide-GetAccomodation-Async")]
        public async Task<ActionResult<List<UserSideAcomodationListModel>>> UserSideGetAccomodationAsync()
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

            var getList = await TrainingProcessserviceMan.GetAccomodationListAsync();

            return getList;
        }

        /// <summary>
        /// API for get Detail of training invitation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-detail/{id}",Name = "UserSide-Training-Process-GetDetail")]
        public async Task<ActionResult<UserSideTrainingProcessDetailModel>> UserSideTrainingProcessGetDetailAsync(int id)
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

            var data = await TrainingProcessserviceMan.GetDetailTrainingProcessAsync(response.EmployeeId, id);

            return data;
        }

        /// <summary>
        /// API for confirmation training invitation.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("confirm-training",Name = "Set-Training-Process-Confirmation-Async")]
        public async Task<ActionResult<string>> SetTrainingConfirmationAsync([FromBody]UserSideTrainingProcessConfirmModel model)
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

            var message = await TrainingProcessserviceMan.SetTrainingConfirmationAsync(response.EmployeeId, model);

            return message;

        }
    }
}
