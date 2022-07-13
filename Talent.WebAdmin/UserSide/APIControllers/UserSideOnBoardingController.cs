using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/onboarding")]
    public class UserSideOnBoarding : Controller
    {
        private readonly UserSideOnBoardingService OnBoardingServices;


        public UserSideOnBoarding(UserSideOnBoardingService service)
        {
            OnBoardingServices = service;
        }

      
        [HttpPost("data")]
        public async Task<ActionResult<BaseResponse>> InsertData([FromBody] OnBoardings model)
        {
            var data = await OnBoardingServices.Insert(model);
            return data;
        }


        [HttpDelete("data")]
        public async Task<ActionResult<BaseResponse>> DeleteData([FromQuery]string id)
        {
            var data = await OnBoardingServices.Delete(id);
            return data;
        }


        [HttpPut("data")]
        public async Task<ActionResult<BaseResponse>> UpdateData([FromBody] OnBoardings model)
        {
            var data = await OnBoardingServices.Update(model);
            return data;
        }

        [HttpPut("order")]
        public async Task<ActionResult<BaseResponse>> UpdateOrder([FromBody] OnBoardingModelList model)
        {
            var data = await OnBoardingServices.UpdateOrder(model);
            return data;
        }


        [HttpGet("data")]
        public async Task<ActionResult<BaseResponse>> GetData(string id, bool? status)
        {
            var data = await OnBoardingServices.Get(id, status);
            return data;
        }
    }
}
