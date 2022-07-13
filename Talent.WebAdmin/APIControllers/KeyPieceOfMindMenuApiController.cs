using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/KeyPieceOfMindMenus")]
    public class KeyPieceOfMindMenuApiController : Controller
    {
        private readonly KeyPieceOfMindMenuService KeyPieceOfMindMenuMan;

        public KeyPieceOfMindMenuApiController(KeyPieceOfMindMenuService keyPieceOfMindService)
        {
            this.KeyPieceOfMindMenuMan = keyPieceOfMindService;
        }

        [HttpGet("get-all-key-piece-of-mind-menus")]
        public async Task<ActionResult<KeyPieceOfMindMenuPaginate>> GetAllKeyPieceOfMindMenuAsync([FromQuery] KeyPieceOfMindMenuGridFilter filter)
        {
            var data = await this.KeyPieceOfMindMenuMan.GetAllKeyPieceOfMindMenu(filter);
            return Ok(data);
        }
        [HttpPost("insert-key-piece-of-mind-menu", Name = "InsertKeyPieceOfMindMenuAsync")]
        public async Task<ActionResult<BaseResponse>> InsertKeyPieceOfMindMenuAsync([FromBody] KeyPieceOfMindMenuCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KeyPieceOfMindMenuMan.InsertKeyPieceOfMindMenuAsync(model);

            return success;
        }
        [HttpGet("get-key-piece-of-mind-menu-by-id", Name = "GetKeyPieceOfMindMenuByIdAsync")]
        public async Task<ActionResult<KeyPieceOfMindMenuModel>> GetKeyPieceOfMindMenuByIdAsync([FromQuery] Guid keyPieceOfMindMenuId)
        {
            var data = await this.KeyPieceOfMindMenuMan.GetKeyPieceOfMindMenuById(keyPieceOfMindMenuId);

            return Ok(data);
        }
        [HttpPost("update-key-piece-of-mind-menu-by-id", Name = "UpdateKeyPieceOfMindMenuAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateKeyPieceOfMindMenuAsync([FromBody] KeyPieceOfMindMenuUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.KeyPieceOfMindMenuMan.UpdateKeyPieceOfMindMenuAsync(model);

            return success;
        }
        [HttpDelete("delete-key-piece-of-mind-menu", Name = "DeleteSKeyPieceOfMindMenuAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteKeyPieceOfMindMenuAsync([FromBody] DeleteKeyPieceOfMindMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.KeyPieceOfMindMenuMan.DeleteKeyPieceOfMindMenuAsync(model);

            return getData;
        }
    }
}
