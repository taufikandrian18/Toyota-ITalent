using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KeyPieceOfMindTypes")]
    public class KeyPieceOfMindTypeApiController : Controller
    {
        private readonly KeyPieceOfMindTypeService KeyPieceOfManTypeMan;

        public KeyPieceOfMindTypeApiController(KeyPieceOfMindTypeService keyPieceOfMindTypeService)
        {
            this.KeyPieceOfManTypeMan = keyPieceOfMindTypeService;
        }
        [HttpGet("get-all-key-piece-of-mind-type")]
        public async Task<ActionResult<KeyPieceOfMindTypePaginate>> GetAllKeyPieceOfMindTypeAsync([FromQuery] KeyPieceOfMindTypeGridFilter filter)
        {
            var data = await this.KeyPieceOfManTypeMan.GetAllKeyPieceOfMindType(filter);
            return Ok(data);
        }
        [HttpPost("insert-key-piece-of-mind-type", Name = "InsertKeyPieceOfMindTypeAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKeyPieceOfMindTypeAsync([FromBody] KeyPieceOfMindTypeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KeyPieceOfManTypeMan.InsertKeyPieceOfMindTypeAsync(model);

            return success;
        }
        [HttpGet("get-key-piece-of-mind-type-by-id", Name = "GetKeyPieceOfMindTypeByIdAsync")]
        public async Task<ActionResult<KeyPieceOfMindTypeModel>> GetKeyPieceOfMindTypeByIdAsync([FromQuery] Guid keyPieceOfMindTypeId)
        {
            var data = await this.KeyPieceOfManTypeMan.GetKeyPieceOfMindTypeById(keyPieceOfMindTypeId);

            return Ok(data);
        }
        [HttpPost("update-key-piece-of-mind-type-by-id", Name = "UpdateKeyPieceOfMindTypeAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKeyPieceOfMindTypeAsync([FromBody] KeyPieceOfMindTypeUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KeyPieceOfManTypeMan.UpdateKeyPieceOfMindTypeAsync(model);

            return success;
        }
        [HttpDelete("delete-key-piece-of-mind-type", Name = "DeleteKeyPieceOfMindTypeAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKeyPieceOfMindTypeAsync([FromBody] DeleteKeyPieceOfMindTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KeyPieceOfManTypeMan.DeleteKeyPieceOfMindTypeAsync(model);

            return getData;
        }
    }
}
