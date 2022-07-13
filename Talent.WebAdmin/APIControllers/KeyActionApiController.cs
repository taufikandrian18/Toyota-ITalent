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
    [Route("api/v1/key-action")]
    public class KeyActionApiController : Controller
    {
        private readonly KeyActionService KeyActionMan;

        public KeyActionApiController(KeyActionService keyActionService)
        {
            this.KeyActionMan = keyActionService;
        }

        /// <summary>
        /// Get All Key Action using filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("gets")]
        public async Task<ActionResult<KeyActionViewModel>> GetKeyActions([FromQuery] KeyActionFilter filter)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await this.KeyActionMan.GetKeyActions(filter);

            return Ok(result);
        }

        /// <summary>
        /// Checking if Key Action Code is Unique
        /// </summary>
        /// <param name="keyActionCode"></param>
        /// <returns></returns>
        [HttpGet("check-unique/{keyActionCode}")]
        public async Task<ActionResult<bool>> IsKeyActionCodeUnique(string keyActionCode)
        {
            if (keyActionCode == null)
            {
                return BadRequest("Key Action Code required");
            }

            var isUnique = await this.KeyActionMan.IsKeyActionCodeUnique(keyActionCode);

            if (isUnique == true)
            {
                return Ok(true);
            }

            return Ok(false);
        }

        /// <summary>
        /// Get Key Action By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-key-action-by-id/{id}")]
        public async Task<ActionResult<KeyActionModel>> GetKeyActionById(int id)
        {
            var result = await this.KeyActionMan.GetKeyActionById(id);
            if (result == null)
            {
                return BadRequest("Id not found");
            }
            return result;
        }

        /// <summary>
        /// To Get All KeyAction by Page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("get-all-key-action/{page}")]
        public async Task<ActionResult<KeyActionViewModel>> GetAllKeyActionController(int page)
        {
            var result = await this.KeyActionMan.GetAllKeyAction(page);
            return Ok(result);
        }

        [HttpGet("get-all-key-actions")]
        public async Task<ActionResult<List<KeyActionModel>>> GetAllKeyActions()
        {
            var result = await this.KeyActionMan.GetAllKeyActions();
            return Ok(result);
        }

        /// <summary>
        /// To Insert Key Action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("post-key-action", Name = "add-key-action")]
        public async Task<ActionResult<bool>> PostKeyAction([FromBody] KeyActionFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await KeyActionMan.InsertKeyAction(model);

            if (result == false)
            {
                return BadRequest("Failed to Insert, Key Action Duplicated");
            }

            return Ok(result);
        }

        /// <summary>
        /// To Delete Key Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-key-action/{id}")]
        public async Task<ActionResult> DeleteKeyAction(int id)
        {
            var isSuccess = await this.KeyActionMan.DeleteKeyActionAsync(id);

            if (isSuccess == false)
            {
                return BadRequest("Failed to Delete");
            }

            return Ok();
        }

        /// <summary>
        /// To Update Key Action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-key-action")]
        public async Task<ActionResult<bool>> UpdateKeyActionAsync([FromBody] KeyActionFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var isSuccess = await this.KeyActionMan.UpdateKeyActionsAsync(model);

            if (isSuccess == false)
            {
                return Ok(false);
            }

            return Ok(true);
        }
    }
}