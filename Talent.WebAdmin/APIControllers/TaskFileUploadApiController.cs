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
    [Route("api/v1/file-upload")]
    public class TaskFileUploadApiController : Controller
    {
        private readonly TaskUploadFileService TaskFileUploadMan;
        public TaskFileUploadApiController(TaskUploadFileService taskUploadFileService)
        {
            this.TaskFileUploadMan = taskUploadFileService;
        }

        /// <summary>
        /// Insert Task File Upload
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("insert-file-upload")]
        public async Task<ActionResult<bool>> InsertTaskFileUpload([FromBody] TaskFileUploadFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var result = await this.TaskFileUploadMan.InsertTaskFileUpload(model);

            return Ok(result);
        }

        /// <summary>
        /// Get Task File Upload
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-file-upload-details/{id}")]
        public async Task<ActionResult<TaskFileUploadViewDetail>> GetTaskFileUploadDetail(int id)
        {
            var result = await this.TaskFileUploadMan.GetTaskFileUploadDetail(id);

            return Ok(result);
        }

        /// <summary>
        /// Update File Upload Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update-file-upload")]
        public async Task<ActionResult<bool>> UpdateTaskFileUpload([FromBody] TaskFileUploadFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model state is not valid");
            }

            var result = await this.TaskFileUploadMan.UpdateTaskFileUpload(model);

            return Ok(result);
        }

    }
}
