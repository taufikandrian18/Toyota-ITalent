using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-time-table")]
    [ApiController]
    public class UserSideTimeTableController : ControllerBase
    {
        private readonly UserSideTimeTableService TimeTableService;

        public UserSideTimeTableController(UserSideTimeTableService timeTableService)
        {
            this.TimeTableService = timeTableService;
        }

        /// <summary>
        /// Get Time Table With Paginate, Multiple FIlter & Sorting.
        /// </summary>
        /// <param name="filter">SortBy Value (name /or nameDesc /or date /or dateDesc) see more <seealso cref="UserSideTimeTableFilterModel"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<UserSideTimeTableResponseModel>> GetTimeTable([FromQuery]string filter)
        {
            var filterDeserialized = JsonConvert.DeserializeObject<UserSideTimeTableFilterModel>(filter);

            // Set data per-page.
            if (filterDeserialized.PageSize < 1)
            {
                filterDeserialized.PageSize = 10;
            }

            // Set page index (current page).
            if (filterDeserialized.PageIndex < 1)
            {
                filterDeserialized.PageIndex = 1;
            }

            var data = await this.TimeTableService.GetTimeTable(filterDeserialized);

            return Ok(data);
        }

        [HttpGet("get-time-table")]
        public async Task<ActionResult<UserSideTimeTableResponseModel>> GetTimeTable([FromQuery] UserSideTimeTableFilterModel filter)
        {
            // Set data per-page.
            if (filter.PageSize < 1)
            {
                filter.PageSize = 10;
            }

            // Set page index (current page).
            if (filter.PageIndex < 1)
            {
                filter.PageIndex = 1;
            }

            var data = await this.TimeTableService.GetTimeTable(filter);

            return Ok(data);
        }

        [HttpGet("get-filter-value")]
        public async Task<ActionResult<UserSideTimeTableFilterValueModel>> GetFilterValue()
        {
            var data = await this.TimeTableService.GetFilterValue();

            return data;
        }
    }
}