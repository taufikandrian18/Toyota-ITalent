using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KeyPieceOfMinds")]
    public class KeyPieceOfMindApiController : Controller
    {
        private readonly KeyPieceOfMindService KeyPieceOfMindMan;

        public KeyPieceOfMindApiController(KeyPieceOfMindService keyPieceOfMindService)
        {
            this.KeyPieceOfMindMan = keyPieceOfMindService;
        }
        [HttpGet("get-all-key-piece-of-minds")]
        public async Task<ActionResult<KeyPieceOfMindPaginate>> GetAllKeyPieceOfMindAsync([FromQuery] KeyPieceOfMindGridFilter filter)
        {
            var data = await this.KeyPieceOfMindMan.GetAllKeyPieceOfMinds(filter);
            return Ok(data);
        }
        [HttpPost("insert-key-piece-of-minds", Name = "InsertKeyPieceOfMindAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKeyPieceOfMindAsync([FromBody] KeyPieceOfMindCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KeyPieceOfMindMan.InsertKeyPieceOfMindAsync(model);

            return success;
        }
        [HttpGet("get-key-piece-of-mind-by-id", Name = "GetKeyPieceOfMindByIdAsync")]
        public async Task<ActionResult<KeyPieceOfMindModel>> GetKeyPieceOfMindByIdAsync([FromQuery] Guid keyPieceOfMindId)
        {
            var data = await this.KeyPieceOfMindMan.GetKeyPieceOfMindById(keyPieceOfMindId);

            return Ok(data);
        }
        [HttpPost("update-key-piece-of-mind-by-id", Name = "UpdateKeyPieceOfMindAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKeyPieceOfMindAsync([FromBody] KeyPieceOfMindUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KeyPieceOfMindMan.UpdateKeyPieceOfMindAsync(model);

            return success;
        }
        [HttpDelete("delete-key-piece-of-minds", Name = "DeleteKeyPieceOfMindAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKeyPieceOfMindAsync([FromBody] DeleteKeyPieceOfMindModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KeyPieceOfMindMan.DeleteKeyPieceOfMindAsync(model);

            return getData;
        }
    }
}
