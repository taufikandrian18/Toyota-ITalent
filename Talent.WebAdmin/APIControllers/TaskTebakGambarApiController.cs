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
    [Route("api/v1/tebak-gambar")]
    public class TaskTebakGambarApiController : Controller
    {
        private readonly TaskTebakGambarService _TaskTebakGambarMan;

        public TaskTebakGambarApiController(TaskTebakGambarService taskTebakGambarService)
        {
            this._TaskTebakGambarMan = taskTebakGambarService;
        }

        [HttpGet("get-task-tebak-gambar-type-by-id/{id}")]
        public async Task<ActionResult<TaskTebakGambarTypesModel>> GetTaskTebakGambarTypeById(int id)
        {
            var data = await this._TaskTebakGambarMan.GetTaskTebakGambarTypeById(id);
            return Ok(data);
        }

        [HttpGet("get-task-tebak-gambar-picture-by-id/{id}")]
        public async Task<ActionResult<List<TaskTebakGambarPicturesModel>>> GetTaskTebakGambarPictureById(int id)
        {
            var data = await this._TaskTebakGambarMan.GetTaskTebakGambarPictureById(id);
            return Ok(data);
        }

        [HttpPost("insert-task-tebak-gambar")]
        public async Task<ActionResult<bool>> InsertTaskTebakGambar([FromBody] TaskTebakGambarModel t)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var data = await this._TaskTebakGambarMan.InsertTaskTebakGambar(t);
            return Ok(true);
        }

        [HttpPost("update-task-tebak-gambar/{id}")]
        public async Task<ActionResult<bool>> UpdateTaskTebakGambar(int id, [FromBody] TaskTebakGambarModel t)
        {

            if(ModelState.IsValid == false)
            {
                return BadRequest("Model State is not valid");
            }

            var data = await this._TaskTebakGambarMan.UpdateTaskTebakGambar(id, t);
            return Ok(true);
        }

    }
}
