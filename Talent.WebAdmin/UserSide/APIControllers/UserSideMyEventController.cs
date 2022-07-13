using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/mobile/my-event")]
    public class UserSideMyEventController : Controller
    {   
        private readonly UserSideMyEventService EventserviceMan;
        private readonly UserSideAuthService UserSideMan;
        public UserSideMyEventController(UserSideMyEventService eventservice, UserSideAuthService userSide)
        {
            this.EventserviceMan = eventservice;
            this.UserSideMan = userSide;
        }
        // GET: api/<controller>
        [HttpGet("get-main-page")]
        public async Task<ActionResult<MainPageModel>> GetMainPageAsync([FromQuery] EventFilterModel filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getList = await EventserviceMan.GetMainPageAsync(response.EmployeeId, filter);
            return getList;
        }

        // GET: api/<controller>
        // TO DO: Albert 
        [HttpGet("get-save")]
        public async Task<ActionResult<List<SingleViewEventModel>>> GetSavedEventListAsync([FromQuery] EventFilterModel filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getList = await EventserviceMan.GetSavedEventAsync(response.EmployeeId, filter);
            return getList;
        }

        // GET: api/<controller>
        [HttpGet("get-popular")]
        public async Task<ActionResult<List<SingleViewEventModel>>> GetPopularEventListAsync([FromQuery] EventFilterModel filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
              return  BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getList = await EventserviceMan.GetPopularEventAsync(response.EmployeeId, filter);
            // var getList = await EventserviceMan.GetPopularEventAsync(null, filter);
            return getList;
        }

        // GET: api/<controller>
        [HttpGet("get-recomend")]
        public async Task<ActionResult<List<SingleViewEventModel>>> GetRecomendEventListAsync([FromQuery] EventFilterModel filter)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }
            
            var getList = await EventserviceMan.GetRecomendEvent(response.EmployeeId, filter);
            // var getList = await EventserviceMan.GetRecomendEvent(null, filter);
            return getList;
        }

        // GET api/<controller>/5
        [HttpGet("event-form/{id}")]
        public async Task<MyEventFormModel> GetDetailFormEventAsync(int id)
        {
            var getData = await EventserviceMan.GetMyEventByIdAsync(id);
            return getData;
        }

        // GET api/<controller>/5
        [HttpGet("detail-view/{id}")]
        public async Task<SingleViewEventModel> GetDetailVIewEventAsync(int id)
        {
            var getData = await EventserviceMan.GetDetailMyEventAsync(id);
            return getData;
        }

        // GET api/<controller>/5
        [HttpGet("get-outlet-list")]
        public async Task<ActionResult<List<DropDownStringModel>>> GetOutletListAsync([FromQuery] EventFilterModel filter)
        {   
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getData = await EventserviceMan.GetOutletListAsync(response.EmployeeId, filter);
            return getData;
        }

        [HttpGet("get-event-category-list")]
        public async Task<ActionResult<List<DropDownModel>>> GetEventCategoryListAsync()
        {
            var getData = await EventserviceMan.GetEventCategoryAsync();
            return getData;
        }


        // GET api/<controller>/5
        [HttpGet("invite-friend-event/{id}")]
        public async Task<ActionResult<List<DropdownWithImage>>> GetInviteFriendListAsync([FromQuery] EventFilterModel filter,int id)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var getData = await EventserviceMan.GetInviteUserListAsync(response.EmployeeId, filter,id);
            return getData;
        }

        // POST api/<controller>
        [HttpPost("insert")]
        public async Task<ActionResult> InsertMyEventAsync([FromBody]MyEventFormModel model)
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
              return  BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            //var getData = await EventserviceMan.GetInviteUserListAsync(response.EmployeeId, filter, id);

            var message = await EventserviceMan.InsertMyEventAsync(response.EmployeeId,model);

            if(message.Contains(ResponseMessageEnum.FailedBaseString) || string.IsNullOrEmpty(message)){
                return BadRequest(message);
            }

            return Ok(message);
        }

        [HttpPost("change-attadence")]
        public async Task<ActionResult> ChangeAttandenceEventAsync ([FromBody]EventAttendModel model)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                var err = "Request header JSON Web Token was not found!";
                return BadRequest(err);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.UserSideMan.VerifyMobileToken(token);

            if(response == null){
               return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var message = await EventserviceMan.MyEventAttendAsync(response.EmployeeId, model);

            if(message.Contains(ResponseMessageEnum.FailedBaseString)){
                return BadRequest(message);
            }

            return Ok(message);
        }

        [HttpPost("join-invited-employee/{id}")]
        public async Task<ActionResult> JoinInvitedEmployeeEventAsync (int id,[FromBody]List<String> employeeList)
        {
            // var hasToken = Request.Headers.ContainsKey("authorization");
            // if (hasToken == false)
            // {
            //     var err = "Request header JSON Web Token was not found!";
            //     return BadRequest(err);
            // }

            // var token = Request.Headers["authorization"].FirstOrDefault();

            // var response = this.UserSideMan.VerifyMobileToken(token);

            // if(response == null){
            //     BadRequest(ResponseMessageEnum.FailedTokenExipred);
            // }

            var message = await EventserviceMan.JoinInvitedEventAsync(id, employeeList);

            if(message.Contains(ResponseMessageEnum.FailedBaseString)){
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
