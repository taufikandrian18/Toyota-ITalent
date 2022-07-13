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
    [Route("api/v1/inbox")]
    public class InboxApiController : Controller
    {
        private readonly InboxService InboxMan;

        public InboxApiController(InboxService inboxService)
        {
            this.InboxMan = inboxService;
        }

        [HttpGet("get-inbox", Name = ("get-inbox"))]
        public async Task<ActionResult<InboxViewModel>> GetInboxUser([FromQuery] InboxFilterModel filter)
        {
            var result = await this.InboxMan.GetInbox(filter);

            return Ok(result);
        }

        [HttpGet("get-unread-inbox-count", Name = ("get-unread-inbox-count"))]
        public async Task<ActionResult<int>> GetUnreadInbox()
        {
            var result = await this.InboxMan.GetUnreadInboxCount();

            return Ok(result);
        }

        [HttpGet("get-approval/{approvalId}/{inboxId}", Name = "get-approval")]
        public async Task<ActionResult<ApprovalInboxModel>> GetApprovalsInbox(int approvalId, int inboxId)
        {
            var result = await this.InboxMan.GetApprovalDetailId(approvalId, inboxId);

            return Ok(result);
        }

    }
}
