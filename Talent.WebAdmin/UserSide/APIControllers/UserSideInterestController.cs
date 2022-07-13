using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-interest")]
    [ApiController]
    public class UserSideInterestController : ControllerBase
    {
        private readonly UserSideInterestService InterestService;

        public UserSideInterestController(UserSideInterestService interestService)
        {
            this.InterestService = interestService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserSideInterestModel>>> GetInterest()
        {
            var dataInterests = await this.InterestService.GetInterest();

            return Ok(dataInterests);
        }
    }
}