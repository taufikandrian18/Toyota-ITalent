using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/userside-profile")]
    [ApiController]
    public class UserSideProfileController : ControllerBase
    {
        private readonly UserSideProfileService profileService;
        private readonly UserSideAuthService AuthService;

        public UserSideProfileController(UserSideProfileService userSideProfileService, UserSideAuthService authService)
        {
            this.profileService = userSideProfileService;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get detail profile employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("get-profile/{employeeId}")]
        public async Task<ActionResult<ProfileViewModel>> GetProfileById(string employeeId)
        {
            var employeeData = await this.profileService.GetProfile(employeeId);

            return Ok(employeeData);
        }

        /// <summary>
        /// Get detail profile employee by token
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-profile")]
        public async Task<ActionResult<ProfileViewModel>> GetProfileByToken()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new ProfileViewModel();
                retdata.Employee = new UserSideEmployeeModel();
                retdata.Position = new List<UserSidePositionModel>();
                retdata.Outlet = new UserSideOutletModel();
                retdata.Area = new UserSideAreaModel();
                retdata.Province = new ProvinceViewModel();
                retdata.Dealer = new UserSideDealerModel();
                retdata.Hobbies = new List<UserSideHobbyModel>();
                retdata.Interests = new List<UserSideInterestModel>();
                retdata.Certificates = new List<UserSideCertificateModel>();
                retdata.TotalBadge = new UserSideTotalBadgeModel();

                return Ok(retdata);
                //return BadRequest(ErrorMessages.TokenNotValid);
            }

            var employeeData = await this.profileService.GetProfile(response.EmployeeId);
            if (employeeData == null)
            {
                employeeData = new ProfileViewModel()
                {
                    Employee = new UserSideEmployeeModel(),
                    Position = new List<UserSidePositionModel>(),
                    Outlet = new UserSideOutletModel(),
                    Area = new UserSideAreaModel(),
                    Province = new ProvinceViewModel(),
                    Dealer = new UserSideDealerModel(),
                    Hobbies = new List<UserSideHobbyModel>(),
                    Interests = new List<UserSideInterestModel>(),
                    Certificates = new List<UserSideCertificateModel>(),
                    TotalBadge = new UserSideTotalBadgeModel()
                };

            }

            return Ok(employeeData);
        }

        /// <summary>
        /// Update profile employee.
        /// </summary>
        /// <param name="newProfile"></param>
        /// <returns></returns>
        [HttpPut("update-profile")]
        public async Task<ActionResult> UpdateProfile([FromForm] ProfileUpdateModel newProfile)
        {
            try
            {
                var hasToken = Request.Headers.ContainsKey("authorization");
                if (hasToken == false)
                {
                    return BadRequest(ErrorMessages.TokenHeaderNotFound);
                }

                var token = Request.Headers["authorization"].FirstOrDefault();

                var response = this.AuthService.VerifyMobileToken(token);

                await this.profileService.UpdateProfile(newProfile,response.EmployeeId);
            }
            catch (System.Exception)
            {
            }

            return Ok();
        }

        /// <summary>
        /// Insert, Update & Delete interest for employee profile.
        /// Always send all <list type="int">interestIds</list> where want to input into db.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="interestIds">list of interestIds where want to input into db.</param>
        /// <returns></returns>
        [HttpPost("interest-profile/{employeeId}")]
        public async Task<ActionResult> InterestProfile(string employeeId, [FromBody]List<int> interestIds)
        {
            await this.profileService.InterestProfile(employeeId, interestIds);

            return Ok();
        }

        /// <summary>
        /// Insert, Update & Delete interest for employee profile.
        /// Always send all <list type="int">interestIds</list> where want to input into db.
        /// </summary>
        /// <param name="interestIds">list of interestIds where want to input into db.</param>
        /// <returns></returns>
        [HttpPost("interest-profile-by-token")]
        public async Task<ActionResult> InterestProfileByToken([FromBody]List<int> interestIds)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            await this.profileService.InterestProfile(response.EmployeeId, interestIds);

            return Ok();
        }

        /// <summary>
        /// Insert, Update & Delete hobbies for employee profile.
        /// Always send all <list type="int">hobbyIds</list> where want to input into db.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="hobbyIds">list of hobbyIds where want to input into db.</param>
        /// <returns></returns>
        [HttpPost("hobby-profile/{employeeId}")]
        public async Task<ActionResult> HobbyProfile(string employeeId, [FromBody]List<int> hobbyIds)
        {
            await this.profileService.HobbyProfile(employeeId, hobbyIds);

            return Ok();
        }

        /// <summary>
        /// Insert, Update & Delete hobbies for employee profile.
        /// Always send all <list type="int">hobbyIds</list> where want to input into db.
        /// </summary>
        /// <param name="hobbyIds">list of hobbyIds where want to input into db.</param>
        /// <returns></returns>
        [HttpPost("hobby-profile-by-token")]
        public async Task<ActionResult> HobbyProfileByToken([FromBody]List<int> hobbyIds)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            await this.profileService.HobbyProfile(response.EmployeeId, hobbyIds);

            return Ok();
        }

        /// <summary>
        /// Get all hobby for profile.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>list of <see cref="List{UserSideHobbyProfileFormModel}"/>.</returns>
        [HttpGet("hobby-profile/{employeeId}")]
        public async Task<ActionResult<List<UserSideHobbyProfileFormModel>>> GetHobbyEmployee(string employeeId)
        {
            var data = await this.profileService.GetHobbyEmployee(employeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all hobby for profile.
        /// </summary>
        /// <returns>list of <see cref="List{UserSideHobbyProfileFormModel}"/>.</returns>
        [HttpGet("hobby-profile-by-token")]
        public async Task<ActionResult<List<UserSideHobbyProfileFormModel>>> GetHobbyEmployeeByToken()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.profileService.GetHobbyEmployee(response.EmployeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all interest for profile.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>list of <see cref="List{UserSideInterestProfileFormModel}"/>.</returns>
        [HttpGet("interest-profile/{employeeId}")]
        public async Task<ActionResult<List<UserSideInterestProfileFormModel>>> GetInterestEmployee(string employeeId)
        {
            var data = await this.profileService.GetInterestEmployee(employeeId);

            return Ok(data);
        }

        /// <summary>
        /// Get all interest for profile.
        /// </summary>
        /// <returns>list of <see cref="List{UserSideInterestProfileFormModel}"/>.</returns>
        [HttpGet("interest-profile-by-token")]
        public async Task<ActionResult<List<UserSideInterestProfileFormModel>>> GetInterestEmployeeByToken()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.profileService.GetInterestEmployee(response.EmployeeId);

            return Ok(data);
        }

        [HttpGet("profile-home-page/{employeeId}")]
        public async Task<ActionResult<ProfileHomePage>> GetProfileHomePage(string employeeId)
        {
            var data = await this.profileService.GetProfileHomePage(employeeId);
            return Ok(data);
        }

        [HttpGet("profile-home-page-by-token")]
        public async Task<ActionResult<ProfileHomePage>> GetProfileHomePageByToken()
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                var retdata = new ProfileHomePage
                {
                    RankUser = new UserSideRankProfileHome()
                };
                return Ok(retdata);
                //return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.profileService.GetProfileHomePage(response.EmployeeId);
            if (data == null)
            {
                data = new ProfileHomePage
                {
                    RankUser = new UserSideRankProfileHome()
                };
            }
            return Ok(data);
        }

        [HttpGet("employee-level-min-value")]
        public async Task<ActionResult<ProfileHomePage>> GetEmployeeLevelMinValue(int point)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.profileService.GetEmployeeLevelMinValue(point);
            return Ok(data);
        }

        [HttpGet("get-profile-rank-by-token")]
        public async Task<ActionResult<ProfileRankModel>> GetProfileRankByToken(int teamId)
        {
            var hasToken = Request.Headers.ContainsKey("authorization");
            if (hasToken == false)
            {
                return BadRequest(ErrorMessages.TokenHeaderNotFound);
            }

            var token = Request.Headers["authorization"].FirstOrDefault();

            var response = this.AuthService.VerifyMobileToken(token);

            if (response == null)
            {
                return BadRequest(ErrorMessages.TokenNotValid);
            }

            var data = await this.profileService.GetProfileRank(response.EmployeeId, teamId);

            return Ok(data);
        }

        [HttpGet("get-profile-rank")]
        public async Task<ActionResult<ProfileRankModel>> GetProfileRank(string employeeId, int teamId)
        {
            var data = await this.profileService.GetProfileRank(employeeId, teamId);

            return Ok(data);
        }
    }
}