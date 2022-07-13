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
    [Route("api/approval/content")]
    public class ApprovalContentApiController : Controller
    {
        private readonly ApprovalContentService ServiceMan;

        public ApprovalContentApiController(ApprovalContentService service)
        {
            this.ServiceMan = service;
        }

        /// <summary>
        /// api for get view approval list in paginate
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("get-paginate", Name = "Get-Approval-Content-Paginate-Async")]
        public async Task<ResponseApprovalContentModel> GetApprovalContentPaginateAsync([FromBody] ApprovalContentFilterModel filter)
        {
            var getData = await ServiceMan.GetApprovalContentPaginateAsync(filter);
            return getData;
        }

        /// <summary>
        /// api to change approval status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost("change-status/{id}/{type}", Name = "Change-Approval-Content-Status-Async")]
        public async Task<bool> ChangeApprovalContentStatusAsync(int id, string type)
        {
            var response = await ServiceMan.ChangeApprovalContentStatusAsync(id, type);
            return response;
        }

        /// <summary>
        /// Rejected Approval from Inbox
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reject-approval-inbox", Name = "reject-approval-inbox")]
        public async Task<ActionResult<bool>> RejectApprovalInbox([FromBody] InboxRejectedModel model)
        {
            var result = await this.ServiceMan.RejectedFromInbox(model);

            if (result == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Accept approval from Inbox
        /// </summary>
        /// <param name="approvalId"></param>
        /// <param name="inboxId"></param>
        /// <returns></returns>
        [HttpPost("accept-approval-inbox/{approvalId}/{inboxId}", Name = "accept-approval-inbox")]
        public async Task<ActionResult<bool>> AcceptApprovalInbox(int approvalId, int inboxId)
        {
            var result = await this.ServiceMan.AcceptRequestInbox(approvalId, inboxId);

            if (result == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
