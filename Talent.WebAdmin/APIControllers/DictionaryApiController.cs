using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/Dictionaries")]
    public class DictionaryApiController : Controller
    {
        private readonly DictionaryService DictionaryMan;

        public DictionaryApiController(DictionaryService dictionaryService)
        {
            this.DictionaryMan = dictionaryService;
        }

        [HttpGet("get-all-dictionaries")]
        public async Task<ActionResult<DictionaryPaginate>> GetAllDictionaryAsync([FromQuery] DictionaryGridFilter filter)
        {
            var data = await this.DictionaryMan.GetAllDictionary(filter);
            return Ok(data);
        }
        [HttpPost("insert-dictionary", Name = "InsertDictionaryAsync")]
        public async Task<ActionResult<BaseResponse>> InsertDictionaryAsync([FromBody] DictionaryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.DictionaryMan.InsertDictionaryAsync(model);

            return success;
        }
        [HttpGet("get-dictionary-by-id", Name = "GetDictionaryByIdAsync")]
        public async Task<ActionResult<DictionaryModel>> GetDictionaryByIdAsync([FromQuery] Guid dictionaryId)
        {
            var data = await this.DictionaryMan.GetDictionaryById(dictionaryId);

            return Ok(data);
        }
        [HttpPost("update-dictionary-by-id", Name = "UpdateDictionaryAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateDictionaryAsync([FromBody] DictionaryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.DictionaryMan.UpdateDictionaryAsync(model);

            return success;
        }
        [HttpDelete("delete-dictionary", Name = "DeleteDictionaryAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteDictionaryAsync([FromBody] DeleteDictionaryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.DictionaryMan.DeleteDictionaryAsync(model);

            return getData;
        }
    }
}
