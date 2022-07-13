using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// Training Web API controller for providing Web APIs such as retrieve training data.
    /// </summary>
    [Route("api/v1/userside-training")]
    public class UserSideTrainingController : ControllerBase
    {
        private readonly UserSideTrainingService TrainingMan;
        private readonly UserSideAuthService AuthService;

        public UserSideTrainingController(UserSideTrainingService service, UserSideAuthService authService)
        {
            this.TrainingMan = service;
            this.AuthService = authService;
        }

        /// <summary>
        /// Get all training data by training id.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        [HttpGet("get-training-by-id")]
        public async Task<ActionResult<UserSideTrainingModel>> GetTrainingByIdAsync([FromQuery] int trainingId)
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

            var data = await this.TrainingMan.GetTrainingById(trainingId, response.EmployeeId);

            return Ok(data);
        }

         [HttpGet("get-training-by-id-team-coach")]
        public async Task<ActionResult<UserSideTrainingModel>> GetTrainingByIdTeamAsync([FromQuery] int trainingId, bool IsCoach)
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

            var data = await this.TrainingMan.GetTrainingByIdTeamCoachAsync(trainingId, response.EmployeeId, IsCoach);

            return Ok(data);
        }

        /// <summary>
        /// Get all training data.
        /// </summary>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        [HttpGet("get-all-training")]
        public async Task<ActionResult<UserSideTrainingModel>> GetAllTraining()
        {
            var data = await this.TrainingMan.GetTraining();
            return Ok(data);
        }

        /// <summary>
        /// Get all question evaluate trainee by training id and employee id.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("get-evaluate-trainee")]
        public async Task<ActionResult<UserSideEmployeeTrainingModel>> GetEvaluateTrainee(int trainingId, string employeeId)
        {
            var data = await this.TrainingMan.GetEvaluateTrainee(trainingId, employeeId);
            return Ok(data);
        }

        /// <summary>
        /// Get all question evaluate trainee by training id and employee id.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        [HttpGet("get-evaluate-trainee-by-token")]
        public async Task<ActionResult<UserSideEmployeeTrainingModel>> GetEvaluateTraineeByToken(int trainingId)
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

            var data = await this.TrainingMan.GetEvaluateTrainee(trainingId, response.EmployeeId);
            return Ok(data);
        }


        /// <summary>
        /// Insert score.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("insert-score")]
        public async Task<ActionResult> InsertScore([FromBody] List<UserSideEvaluationModel> userSideEvaluationModels)
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

            var result = await this.TrainingMan.InsertScore(userSideEvaluationModels, response.EmployeeId);
            if(result!=null) return BadRequest(result);
            return Ok(true);
        }

        /// <summary>
        /// Get all training data by employee ID.
        /// </summary>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        [HttpGet("get-all-training-by-employeeId")]
        public async Task<ActionResult<UserSideTrainingModel>> GetAllTrainingByEmployeeId(string employeeId)
        {
            var data = await this.TrainingMan.GetTrainingByEmployeeId(employeeId);
            return Ok(data);
        }

        /// <summary>
        /// Get all training data by token.
        /// </summary>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        [HttpGet("get-all-training-by-token")]
        public async Task<ActionResult<UserSideTrainingModel>> GetAllTrainingByToken()
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

            var data = await this.TrainingMan.GetTrainingByEmployeeId(response.EmployeeId);
            return Ok(data);
        }
    }
}