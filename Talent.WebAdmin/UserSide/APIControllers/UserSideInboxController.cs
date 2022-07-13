using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

namespace Talent.WebAdmin.UserSide.APIControllers
{
    /// <summary>
    /// User Side Inbox Web API controller for providing Web APIs such as retrieve inbox data.
    /// </summary>
    [Route("api/v1/userside-inboxes")]
    public class UserSideInboxController : Controller
    {
        private readonly UserSideInboxService Service;
        private readonly UserSideAuthService AuthService;

        public UserSideInboxController(UserSideInboxService userSideInboxService, UserSideAuthService userSideAuthService)
        {
            this.Service = userSideInboxService;
            this.AuthService = userSideAuthService;
        }

        /// <summary>
        /// Get all inbox for spesific employee.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<UserSideInboxModel>>> GetInboxes(int itemPerPage, int pageIndex, string search)
        {
            // Get employeeId from token
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

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var data = await this.Service.GetInboxesByEmployeeId(response.EmployeeId, itemPerPage, pageIndex, search);

            return Ok(data);
        }

        /// <summary>
        /// Get detail message.
        /// </summary>
        /// <param name="inboxId"></param>
        /// <returns></returns>
        [HttpGet("detail-inbox/{inboxId}")]
        public async Task<ActionResult<List<UserSideInboxModel>>> GetDetailInbox(int inboxId)
        {
            var data = await this.Service.GetDetailInbox(inboxId);

            return Ok(data);
        }

        /// <summary>
        /// Update status request rotation team member.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-request-rotation")]
        public async Task<ActionResult> UpdateRequestRotation([FromBody] UserSideUpdateStatusRequestRotationModel model)
        {
            await this.Service.UpdateStatusRequestRotation(model);

            return Ok();
        }

        /// <summary>
        /// Update status request  team member.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-request-team-member")]
        public async Task<ActionResult> UpdateRequestTeamMember([FromBody] UserSideUpdateStatusRequestTeamMemberModel model)
        {
            await this.Service.UpdateStatusRequestTeamMember(model);

            return Ok();
        }

        /// <summary>
        /// Check if current user has unread message in inbox return true else false
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-unread-inbox")]
        public async Task<ActionResult<bool>> CheckUnreadInbox()
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
                return BadRequest(ResponseMessageEnum.FailedTokenExipred);
            }

            var check = await this.Service.CheckUnreadInbox(response.EmployeeId);

            return check;
        }
    }
}