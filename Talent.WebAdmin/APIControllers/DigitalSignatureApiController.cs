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
    [Route("api/v1/digital-signature")]
    public class DigitalSignatureApiController : Controller
    {
        private readonly DigitalSignatureService DigitalSignatureMan;
        public DigitalSignatureApiController(DigitalSignatureService digitalSignatureService)
        {
            this.DigitalSignatureMan = digitalSignatureService;
        }

        /// <summary>
        /// Get All Digital Signature
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("get-all-digital-signature")]
        public async Task<ActionResult<DigitalSignatureViewModel>> GetAllDigitalSignature([FromQuery] DigitalSignatureFilter filter)
        {
            var result = await this.DigitalSignatureMan.GetDigitalSignature(filter);

            return Ok(result);
        }

        /// <summary>
        /// To Soft Delete Digital Signature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-digital-signature")]
        public async Task<ActionResult<bool>> DeleteDigitalSignature([FromQuery] int id)
        {
            var result = await this.DigitalSignatureMan.DeleteDigitalSignature(id);
            if (result == false)
            {
                return BadRequest("Failed to delete");
            }

            return result;
        }

        /// <summary>
        /// To Insert Digital Signature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("insert-digital-signature")]
        public async Task<ActionResult<int>> InsertDigitalSignature([FromBody] DigitalSignatureFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var result = await this.DigitalSignatureMan.InsertDigitalSignature(model);

            if (result == -1)
            {
                return BadRequest("Blob ID Not Found");
            }
            if (result == -2)
            {
                return BadRequest("Employee can't be the same");
            }

            return Ok(result);
        }

        /// <summary>
        /// To Update Digital Signature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-digital-signature")]
        public async Task<ActionResult<bool>> UpdateInsertDigitalSignature([FromBody] DigitalSignatureFormModel model)
        {
            var result = await this.DigitalSignatureMan.UpdateDigitalSignature(model);

            if (result == false)
            {
                return BadRequest("Digital Signature Not Found or Employee ID Duplicate");
            }
            return Ok(true);
        }

        /// <summary>
        /// Validate if Digital Signature possible to be Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("validate-update/{id}/{employeeId}")]
        public async Task<ActionResult<bool>> ValidateUpdate(int id, string employeeId)
        {
            var result = await this.DigitalSignatureMan.ValidateUpdate(id, employeeId);
            return result;
        }
    }
}
