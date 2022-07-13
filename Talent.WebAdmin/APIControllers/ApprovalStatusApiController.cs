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
    [Route("api/v1/approval-status")]
    public class ApprovalStatusApiController : Controller
    {
        private readonly ApprovalStatusService ApprovalStatusMan;

        public ApprovalStatusApiController(ApprovalStatusService approvalStatusService)
        {
            this.ApprovalStatusMan = approvalStatusService;
        }

        [HttpGet("get-all-approval-statuses",Name = "get-all-approval-statuses")]
        public async Task<ActionResult<ApprovalStatusViewModel>> GetAllApprovalStatuses()
        {
            var result = await this.ApprovalStatusMan.GetAllApprovalStatuses();
            return Ok(result);
        }

        [HttpGet("get-approval-status-by-id/{id}", Name = "get-approval-status-by-id")]
        public async Task<ActionResult<ApprovalStatusModel>> GetApprovalStatusById(int id)
        {
            var result = await ApprovalStatusMan.GetApprovalStatusById(id);
            return Ok(result);
        }
    }
}
