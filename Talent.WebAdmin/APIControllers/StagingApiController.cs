using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/staging")]
    [ApiController]
    public class StagingApiController : Controller
    {
        private readonly StagingToTableService TableMan;

        public StagingApiController(StagingToTableService stagingToTableService)
        {
            this.TableMan = stagingToTableService;
        }

        /// <summary>
        /// Execute service to fill EmployeePositionMappings
        /// </summary>
        /// <returns></returns>
        [HttpPost("employee-position-mapping")]
        public async Task<IActionResult> StagingToEmployeePositionMapping()
        {
            await this.TableMan.StageEmployeeToEmployeePositionMapping();

            return Ok();
        }

        /// <summary>
        /// Execute service to fill Positions
        /// </summary>
        /// <returns></returns>
        [HttpPost("position")]
        public async Task<IActionResult> StagingToPosition()
        {
            await this.TableMan.StageOrganizationObjectToPosition();
            await this.TableMan.StageManpowerPositionTypeToPosition();

            return Ok();
        }

        /// <summary>
        /// Execute service to fill Employees
        /// </summary>
        /// <returns></returns>
        [HttpPost("dealer-employee")]
        public async Task<IActionResult> StagingDealerEmployeeToEmployee()
        {
            await this.TableMan.StageManpowerTypeToDealerPeopleCategories();
            await this.TableMan.StageDealerEmployeeToEmployees();

            return Ok();
        }

        /// <summary>
        /// Execute service to fill Employees
        /// </summary>
        /// <returns></returns>
        [HttpPost("user")]
        public async Task<IActionResult> StagingUserToEmployee()
        {
            await this.TableMan.StageUserToEmployees();

            return Ok();
        }

        /// <summary>
        /// Execute service to fill outlet
        /// </summary>
        /// <returns></returns>
        [HttpPost("outlet")]
        public async Task<IActionResult> StagingToOutlet()
        {
            await this.TableMan.StageToCity();
            await this.TableMan.StageToProvince();
            await this.TableMan.StageToSalesArea();
            await this.TableMan.StageToAfterSalesArea();
            await this.TableMan.StageToDealer();
            await this.TableMan.StageToOutlet();

            return Ok();
        }

        /// <summary>
        /// Execute service to fill TamEmployeeStructure
        /// </summary>
        /// <returns></returns>
        [HttpPost("tam-employee-structure")]
        public async Task<IActionResult> StagingAosToTamEmployeeStructure()
        {
            await this.TableMan.StageToTamEmployeeStructure();
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> StagingToAll()
        {
            await this.TableMan.StageToAll();

            return Ok();
        }


    }
}
