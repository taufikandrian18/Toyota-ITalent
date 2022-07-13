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
    [Route("api/v1/approval-mapping")]
    public class ApprovalMappingApiController : Controller
    {
        private readonly ApprovalMappingService ApprovalMappingMan;

        public ApprovalMappingApiController(ApprovalMappingService approvalMappingService)
        {
            this.ApprovalMappingMan = approvalMappingService;
        }

        [HttpGet("get-approval-mapping-filtered", Name = "get-appoval-mapping-filtered")]
        public async Task<ActionResult<ApprovalMappingViewModel>> GetApprovalMappingsFiltered([FromQuery] ApprovalMappingFilterModel model)
        {
            var result = await this.ApprovalMappingMan.GetApprovalMappingFiltered(model);

            return Ok(result);
        }

        [HttpPost("insert-approval-mappings", Name = "insert-approval-mappings")]
        public async Task<ActionResult<bool>> InsertApprovalMappings([FromBody] ApprovalMappingFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }


            var result = await this.ApprovalMappingMan.InsertApprovalMapping(model);

            if (result == false)
            {
                return BadRequest("Failed to insert, make sure Page not duplicated");
            }

            return Ok(result);
        }

        [HttpDelete("delete-approval-mappings/{id}", Name = "delete-approval-mappings")]
        public async Task<ActionResult<bool>> DeleteApprovalMapping(int id)
        {
            var result = await this.ApprovalMappingMan.DeleteApprovalMapping(id);

            if (result == false)
            {
                return BadRequest("Failed to delete");
            }

            return Ok(result);
        }

        [HttpPost("update-approval-mappings", Name = "update-approval-mappings")]
        public async Task<ActionResult<bool>> UpdateApprovalMapping([FromBody] ApprovalMappingFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var result = await this.ApprovalMappingMan.UpdateApprovalMapping(model);

            if (result == false)
            {
                return BadRequest("Failed to Update");
            }

            return Ok(result);
        }

        [HttpGet("get-all-pages", Name = "get-all-pages")]
        public async Task<ActionResult<List<PagesModel>>> GetAllPages()
        {
            var result = await this.ApprovalMappingMan.GetAllPage();

            return Ok(result);
        }

        [HttpGet("get-all-approval-level", Name = "get-all-approval-level")]
        public async Task<ActionResult<List<ApprovalLevelModel>>> GetAllApprovalLevel()
        {
            var result = await this.ApprovalMappingMan.GetAllApprovalLevel();

            return Ok(result);
        }


        [HttpGet("get-uninserted-page", Name = "get-uninserted-page")]
        public async Task<ActionResult<List<PagesModel>>> GetUninsertedPage()
        {
            var result = await this.ApprovalMappingMan.GetPageForInsert();

            return Ok(result);
        }

    }
}
