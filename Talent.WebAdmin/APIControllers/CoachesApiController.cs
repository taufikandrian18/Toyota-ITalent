using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/coach")]
    public class CoachesApiController : Controller
    {
        private readonly CoachesService CoachesMan;

        public CoachesApiController(CoachesService coachesService)
        {
            this.CoachesMan = coachesService;
        }

        /// <summary>
        /// To Get All the Coach using filtered advance search and sort by
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("get-all-coaches-filter")]
        public async Task<ActionResult<CoachesViewModel>> GetAllCoaches(CoachesFilter filter)
        {
            var allCoaches = await this.CoachesMan.GetAllCoachesViewFiltered(filter);

            return Ok(allCoaches);
        }

        /// <summary>
        /// To Get List of Coach that avalaible to be add as a coach
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        [HttpGet("get-all-employee-for-add/{employeeName}")]
        public async Task<ActionResult<ListEmployeeForCoach>> GetAllEmployeeForAddCoach(string employeeName, string IsTam, string IsDealer)
        {
            var allEmployee = await this.CoachesMan.GetAllEmployeeForAddCoach(employeeName,IsTam,IsDealer);

            return Ok(allEmployee);
        }

        /// <summary>
        /// Auto suggest list for employee
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        [HttpGet("get-all-employee/{employeeName}")]
        public async Task<ActionResult<ListEmployeeForCoach>> GetAllEmployee(string employeeName, string IsTam, string IsDealer)
        {
            var allEmployee = await this.CoachesMan.GetAllEmployee(employeeName, IsTam, IsDealer);

            return Ok(allEmployee);
        }

        /// <summary>
        /// Get all topics
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-topics")]
        public async Task<ActionResult<List<TopicsEbadgeJoinModelForCoach>>> GetAllTopicsEbadge()
        {
            var topicsJoinEbadge = await this.CoachesMan.GetTopicsAndEbadge();

            return Ok(topicsJoinEbadge);
        }

        /// <summary>
        /// To insert coach
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("insert-master-coach")]
        public async Task<ActionResult<bool>> InsertMasterCoach([FromBody] CoachFormModel model)
        {
            var result = await this.CoachesMan.InsertCoaches(model);

            if (result == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get Coach DetailById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-coach-detail/{id}")]
        public async Task<ActionResult<CoachViewDetail>> GetCoachViewDetail(int id)
        {
            var result = await this.CoachesMan.GetCoachViewDetail(id);
            return Ok(result);
        }

        /// <summary>
        /// Update Coach
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-coach-detail")]
        public async Task<ActionResult<bool>> UpdateCoach([FromBody] CoachFormModel model)
        {
            var result = await this.CoachesMan.UpdateCoach(model);

            if (result == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete Coach
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("delete-coach")]
        public async Task<ActionResult<bool>> DeleteCoach([FromBody] CoachDeleteFormModel model)
        {
            var result = await this.CoachesMan.DeleteCoachMapping(model);

            if (result == false)
            {
                return BadRequest(result);
            }
            return Ok(true);
        }

        /// <summary>
        /// Validate Coach before update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("validate-update-coach/{id}/{employeeId}")]
        public async Task<ActionResult<bool>> ValidateUpdate(int id, string employeeId)
        {
            var result = await this.CoachesMan.ValidateUpdate(id, employeeId);
            return result;
        }
    }
}
