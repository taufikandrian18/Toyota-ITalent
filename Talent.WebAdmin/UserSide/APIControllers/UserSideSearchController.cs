using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-search")]
    public class UserSideSearchController : Controller
    {
        private readonly UserSideSearch SearchingMan;
        private readonly UserSideAuthService AuthService;

        public UserSideSearchController(UserSideSearch service, UserSideAuthService authService)
        {
            this.SearchingMan = service;
            this.AuthService = authService;
        }

        [HttpGet("get-all-learnings")]
        public async Task<ActionResult<List<LearningViewModel>>> GetAllLearnings([FromQuery] LearningFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new List<LearningViewModel>();
                return Ok(retdata);
                //return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetAllLearnings(filter, response.EmployeeId);
            if (data == null)
            {
                data = new List<LearningViewModel>();
            }

            return Ok(data);
        }

        [HttpGet("get-all-coaches")]
        public async Task<ActionResult<List<CoachViewModel>>> GetAllCoaches([FromQuery] CoachFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetAllCoach(filter);

            return Ok(data);
        }

        [HttpGet("get-all-peoples")]
        public async Task<ActionResult<List<PeopleViewModel>>> GetAllPeoples([FromQuery] PeopleFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetAllPeople(filter);

            return Ok(data);
        }

        [HttpGet("get-all-events")]
        public async Task<ActionResult<List<EventModel>>> GetAllEvents([FromQuery] EventFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetAllEvent(filter, response.EmployeeId);

            return Ok(data);
        }

        [HttpGet("get-all-news")]
        public async Task<ActionResult<List<NewsViewModel>>> GetAllNews([FromQuery] NewsFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetNews(filter);

            return Ok(data);
        }

        [HttpGet("get-all-insights")]
        public async Task<ActionResult<List<NewsViewModel>>> GetAllInsights([FromQuery] InsightFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetInsight(filter, response.EmployeeId);

            return Ok(data);
        }

        /// <summary>
        /// belum dipakai
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("get-all-area")]
        public async Task<ActionResult<List<AreaModel>>> GetAllArea([FromQuery] AreaFilter filter)
        {
            var hastoken = Request.Headers.ContainsKey("authorization");
            if (hastoken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.SearchingMan.GetArea(filter);

            return Ok(data);
        }

        //[HttpGet("get-all-program-type")]
        //public async Task<ActionResult<TrainingProgramType>> GetLearningProgramType()
        //{
        //    var hastoken = Request.Headers.ContainsKey("authorization");
        //    if (hastoken == false)
        //    {
        //        return BadRequest(ErrorMessages.TokenHeaderNotFound);
        //    }

        //    var token = Request.Headers["authorization"].FirstOrDefault();

        //    var response = this.AuthService.VerifyMobileToken(token);

        //    if (response == null)
        //    {
        //        return BadRequest(ErrorMessages.TokenNotValid);
        //    }

        //    this.LearningMan.GetLearningProgramType();

        //    return Ok();
        //}
    }
}
