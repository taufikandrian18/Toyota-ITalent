using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/employee")]
    public class EmployeeApiController : Controller
    {
        private readonly EmployeeService EmployeeMan;

        public EmployeeApiController(EmployeeService employeeService)
        {
            this.EmployeeMan = employeeService;
        }

        [HttpGet("get/{employeeId}")]
        public async Task<IActionResult> Get(string employeeId)
        {
            var data = await EmployeeMan.GetName(employeeId);
            return Ok(data);
        }

        [HttpGet("get-employee-name-for-digital-signature/{employeeName}")]
        public async Task<ActionResult<EmployeeListViewModel>> GetEmployeeNameForDigitalSignature(string employeeName)
        {
            var data = await EmployeeMan.GetMultipleEmployeeForDigitalSignature(employeeName);

            return Ok(data);
        }

        [HttpGet("get-employee-name/{employeeName}")]
        public async Task<ActionResult<EmployeeListViewModel>> GetEmployee(string employeeName)
        {
            var data = await EmployeeMan.GetMultiEmployee(employeeName);

            return Ok(data);
        }

        [HttpPost("insert-employee-test-api")]
        public async Task<ActionResult<bool>> InsertEmployeeTestApi([FromBody] EmployeeFormModel model)
        {
            var isSuccess = await this.EmployeeMan.InsertEmployee(model);

            return Ok(true);
        }
    }
}
