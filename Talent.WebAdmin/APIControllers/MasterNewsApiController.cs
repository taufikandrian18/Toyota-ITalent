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
    [Route("api/master/news")]
    public class MasterNewsApiController : Controller
    {
        private readonly MasterNewsService ServiceMan;

        public MasterNewsApiController(MasterNewsService service)
        {
            this.ServiceMan = service;
        }

        /// <summary>
        /// get paginate page
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("get-paginate", Name = "Get-News-Paginate-Async")]
        public async Task<ResponseMasterNewsModel> GetNewsPaginateAsync([FromBody] MasterNewsFilterModel filter)
        {
            var getData = await ServiceMan.GetNewsPaginateAsync(filter);
            return getData;
        }
        
        /// <summary>
        /// detail data news by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-detail/{id}", Name = "Get-Detail-News-Async")]
        public async Task<MasterNewsFormModel> GetDetailNewsAsync(int id)
        {
            var getData = await ServiceMan.GetDetailNewsByIdAsync(id);
            return getData;
        }

        /// <summary>
        /// insert news 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type">tipe save</param>
        /// <returns></returns>
        [HttpPost("insert/{type}", Name = "Insert-News-Async")]
        public async Task<string> InsertNewsAsync([FromBody] MasterNewsFormModel model,string type)
        {
            var response = await ServiceMan.InsertMasterNewsAsync(type,model);
            return response;
        }

        /// <summary>
        /// update news
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type">tipe save</param>
        /// <returns></returns>
        [HttpPost("update/{type}", Name = "Update-News-Async")]
        public async Task<string> UpdateNewsAsync([FromBody] MasterNewsFormModel model,string type)
        {
            var response = await ServiceMan.UpdateMasterNewsAsync(type,model);
            return response;
        }

        [HttpPost("change-status/{id}/{type}", Name = "Change-News-Status-Async")]
        public async Task<string> ChangeNewsStatusAsync(int id, string type)
        {
            var response = await ServiceMan.ChangeNewsStatusAsync(id, type);
            return response;
        }

        [HttpDelete("delete/{id}", Name = "Delete-News-Async")]
        public async Task<string> DeleteNewsAsync(int id)
        {
            var response = await ServiceMan.DeleteNewsAsync(id);
            return response;
        }
    }
}

