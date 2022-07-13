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
    [Route("api/v1/dropdown")]
    public class DropdownController : Controller
    {
        private readonly DropdownService dropMan;

        public DropdownController(DropdownService service)
        {
            this.dropMan = service;
        }

        [HttpGet("get-all-reward-type-dropdown")]
        public async Task<ActionResult<List<RewardTypeDropdownModel>>> GetAllRewardTypesAsync()
        {
            var data = await this.dropMan.GetRewardTypesForRewardAsync();

            return data;
        }

        [HttpGet("get-all-module-dropdown")]
        public async Task<ActionResult<List<ModuleDropdownModel>>> GetAllModulesAsync()
        {
            var data = await this.dropMan.GetModulesForRewardAsync();

            return data;
        }

        [HttpGet("get-all-coach-dropdown")]
        public async Task<ActionResult<List<CoachDropdownModel>>> GetAllCoachesAsync()
        {
            var data = await this.dropMan.GetCoachesForRewardAsync();

            return data;
        }

        [HttpGet("get-all-event-dropdown")]
        public async Task<ActionResult<List<EventDropdownModel>>> GetAllEventsAsync()
        {
            var data = await this.dropMan.GetEventsForRewardAsync();

            return data;
        }

        [HttpGet("get-all-reward-point-type-dropdown")]
        public async Task<ActionResult<List<RewardPointTypeDropdown>>> GetAllRewardPointTypeDropdownAsync()
        {
            var data = await this.dropMan.GetRewardPointTypeForRewardAsync();

            return data;
        }

        [HttpGet("get-dealer-dropdown")]
        public async Task<ActionResult<List<DealerDropdownModel>>> GetDealerDropdownAsync()
        {
            var data = await this.dropMan.GetDealerForAssessment();

            return data;
        }

        [HttpGet("get-outlet-dropdown")]
        public async Task<ActionResult<List<OutletDropdownModel>>> GetOutletDropdownAsync([FromQuery] string dealerId)
        {
            var data = await this.dropMan.GetOutletForAssessment(dealerId);

            return data;
        }

        [HttpGet("get-position-dropdown")]
        public async Task<ActionResult<List<PositionDropdownModel>>> GetPositionDropdownAsync([FromQuery] string outletId)
        {
            var data = await this.dropMan.GetPositionForAssessment(outletId);

            return data;
        }

        [HttpGet("get-employee-dropdown")]
        public async Task<ActionResult<List<EmployeeDropdownModel>>> GetEmployeeDropdownAsync([FromQuery] int positionId, string outletId)
        {
            var data = await this.dropMan.GetEmployeeForAssessment(positionId, outletId);

            return data;
        }

        [HttpGet("get-material-type-dropdown")]
        public async Task<ActionResult<List<MaterialTypeDropdownModel>>> GetMaterialTypeDropdownAsync()
        {
            var data = await this.dropMan.GetMaterialTypeForModuleAsync();

            return data;
        }

        [HttpGet("get-topic-dropdown")]
        public async Task<ActionResult<List<TopicDropdownModel>>> GetTopicDropdownModel()
        {
            var data = await this.dropMan.GetTopicForModuleAsync();

            return data;
        }

        [HttpGet("get-approval-status-dropdown")]
        public async Task<ActionResult<List<ApprovalStatusDropdownModel>>> GetApprovalStatusDropdownAsync()
        {
            var data = await this.dropMan.GetApprovalStatusForModuleAsync();

            return data;
        }

        [HttpGet("get-approval-status",Name="Get-Approval-Status-List")]
        public List<string> GetApprovalStatusList()
        {
            var data = this.dropMan.GetApprovalStatusList();
            return data;
        }

        [HttpGet("get-content-category",Name="Get-Content-Category-List")]
        public List<string> GetContentCategoryList()
        {
            var data = this.dropMan.GetContentCategoryList();
            return data;
        }

        [HttpGet("get-news-list",Name="Get-Master-News-List")]
        public async Task<List<DropDownModel>> GetMasterNewsListAsync()
        {
            var data = await this.dropMan.GetMasterNewsList();
            return data;
        }

        [HttpGet("get-competency", Name = "Get-Competency-List")]
        public async Task<List<DropDownModel>> GetCompetencyListAsync()
        {
            var data = await this.dropMan.GetCompetencyListAsync();
            return data;
        }

        [HttpGet("get-product-category", Name = "Get-Product-Category-List")]
        public List<string> GetProductCategoryList()
        {
            var data = this.dropMan.GetProductCategoryList();
            return data;
        }

        [HttpGet("get-product-segment", Name = "Get-Product-Segment-List")]
        public List<DropDownModel> GetProductSegmentList()
        {
            var data = this.dropMan.GetProductSegmentList();
            return data;
        }

        [HttpGet("get-product-approval", Name = "Get-Product-Approval-List")]
        public List<DropDownModel> GetProductApprovalList()
        {
            var data = this.dropMan.GetProductGalleryApprovalList();
            return data;
        }

        [HttpGet("get-competency-priority", Name = "Get-Competency-Priority-List")]
        public List<string> GetCompetencyPriorityList()
        {
            var data = this.dropMan.GetCompetencyPriorityList();
            return data;
        }

        [HttpGet("get-competency-Proficiency", Name = "Get-Proficiency-List")]
        public List<decimal> GetCompetencyProficiencyList()
        {
            var data = this.dropMan.GetCompetencyProficiencyList();
            return data;
        }

        [HttpGet("get-position", Name = "Get-Position-List")]
        public async Task<List<DropDownModel>> GetPositionListAsync()
        {
            var data = await this.dropMan.GetPositionListAsync();
            return data;
        }
    }
}
