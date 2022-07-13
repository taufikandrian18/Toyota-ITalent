using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;
using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/guest-account-data")]
    public class DashboardGuestAccountApiController : Controller
    {
        private readonly DashboardGuestAccountService GuestAccountService;


        public DashboardGuestAccountApiController(DashboardGuestAccountService service)
        {
            this.GuestAccountService = service;
        }

      
        [HttpGet("get")]
        public async Task<ActionResult<BaseResponse>> Get(string id)
        {
            var data = await this.GuestAccountService.GetDetail(id);
            return data;
        }


        [HttpGet("get-list-guest")]
        public async Task<ActionResult<BaseResponse>> GetList(GetAccountParameter model)
        {

            var data = await this.GuestAccountService.GetListGuestAccount(model);
            return data;
        }

        [HttpGet("export-list-guest")]
        public async Task<ActionResult<BaseResponse>> GetDataExport(GetAccountParameter model, string filetype)
        {

            var data = await this.GuestAccountService.ExportExcel(model, filetype);
            return data;
        }

        [HttpGet("get-list-account")]
        public async Task<ActionResult<BaseResponse>> GetListAccount(GetAccountParameter model)
        {

            var data = await this.GuestAccountService.GetListAccount(model);
            return data;
        }


        [HttpGet("user-summary")]
        public async Task<ActionResult<BaseResponse>> GetSummaryUser(List<string> dealerId)
        {
            var data = await this.GuestAccountService.GetSummaryTotalGuestAccount(dealerId);
            return data;
        }

        [HttpGet("user-summary-dealer")]
        public async Task<ActionResult<BaseResponse>> GetSummaryUserDealer(List<string> id)
        {

            var data = await this.GuestAccountService.GetSummaryGuestDealer(id);
            return data;
        }

        [HttpGet("user-summary-outlet")]
        public async Task<ActionResult<BaseResponse>> GetSummaryUserOutlet(List<string> id, List<string> dealerId)
        {
            var data = await this.GuestAccountService.GetSummaryGuestOutlet(id, dealerId);
            return data;
        }


        [HttpGet("user-summary-other")]
        public async Task<ActionResult<BaseResponse>> GetSummaryUserOther(List<string> id)
        {
            var data = await this.GuestAccountService.GetSummaryGuestOther(id);
            return data;
        }

        [HttpPut("update")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]EmployeeGuestModel  model)
        {
            var data = await this.GuestAccountService.UpdateEmployeeGuesstAccount(model);
            return data;
        }

        [HttpPut("update-status")]
        public async Task<ActionResult<BaseResponse>> UpdateStatus([FromBody] EmployeeUpdateStatusVM model)
        {
            var data = await this.GuestAccountService.UpdateEmployeeStatus(model);
            return data;
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<BaseResponse>> Delete(string id)
        {
            var data = await this.GuestAccountService.DeleteDataEmployeeGuesstAccount(id);
            return data;
        }


    }
}
