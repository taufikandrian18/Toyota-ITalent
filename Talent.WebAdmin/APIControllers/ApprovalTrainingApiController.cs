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
    [Route("api/v1/approval-training")]
    public class ApprovalTrainingApiController : Controller
    {
        private readonly ApprovalTrainingService ApprovalTrainingMan;

        public ApprovalTrainingApiController(ApprovalTrainingService approvalTrainingService)
        {
            this.ApprovalTrainingMan = approvalTrainingService;
        }
        [HttpGet("get-approval-training", Name = "get-appoval-training")]
        public async Task<ActionResult<ApprovalTrainingViewModel>> GetApprovalTrainingFiltered([FromQuery] ApprovalTrainingFilter filter)
        {
            var trainings = await ApprovalTrainingMan.GetApprovalTraining(filter);
            return Ok(trainings);
        }
        [HttpGet("get-approval-training-by-id/{id}", Name = "get-approval-training-by-id")]
        public async Task<ActionResult<ApprovalTrainingDetailViewModel>> GetApprovalTrainingById(int id)
        {
            var result = await ApprovalTrainingMan.GetApprovalTrainingById(id);
            return Ok(result);
        }
        [HttpPut("save-approval-training", Name = "save-approval-training")]
        public async Task<ActionResult> SaveApprovalTraining([FromBody] List<ApprovalTrainingEmployee> model)
        {
            await ApprovalTrainingMan.SaveApprovalTraining(model);
            return Ok();
        }
        [HttpPut("send-invitation", Name = "send-invitation")]
        public async Task<ActionResult> SendInvitation([FromBody] TrainingInvitationModel model)
        {
            await ApprovalTrainingMan.SendInvitation(model);
            return Ok();
        }

        [HttpPut("send-invitation-new", Name = "send-invitation-new")]
        public async Task<ActionResult> SendInvitationNew([FromBody] TrainingInvitationModel model)
        {
            await ApprovalTrainingMan.SendInvitationNew(model);
            return Ok();
        }

        [HttpPost("export-employee", Name = "export-employee")]
            public async Task<ActionResult> ExportEmpoyee([FromBody] List<VMExportlEmployeeTrainingApprovalInput> model) {
            var data =await ApprovalTrainingMan.ExportExcel(model);
            return data;
        }
    }
}
