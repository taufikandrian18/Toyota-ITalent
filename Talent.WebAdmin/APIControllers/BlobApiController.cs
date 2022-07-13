using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/blob")]
    public class BlobApiController : Controller
    {
        private readonly IFileStorageService FileMan;
        private readonly BlobService BlobMan;
        private readonly TalentContext DB;

        public BlobApiController(IFileStorageService fileService, BlobService blobService, TalentContext db)
        {
            this.FileMan = fileService;
            this.BlobMan = blobService;
            this.DB = db;
        }
        /// <summary>
        /// Albert harap ubah !!!
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload-file", Name = "Upload-File")]
        public async Task<ActionResult<string>> UploadFile(IFormCollection file)
        {
            string fileName, fileExtension;

            fileName = file.Files[0].FileName;
            fileExtension = file.Files[0].ContentType.Substring(file.Files[0].ContentType.IndexOf('/') + 1, file.Files[0].ContentType.Length - file.Files[0].ContentType.IndexOf('/') - 1);
            var newGuid = this.FileMan.InsertBlob(fileName, fileExtension);
            // Sorry God for this sin T.T supaya jalan aja dulu (albert harus bener ke yang baruu ya)
            await DB.SaveChangesAsync();
            fileName = newGuid.ToString();
            using (var data = file.Files[0].OpenReadStream())
            {
                await this.FileMan.UploadFile(fileName, fileExtension, data);
            }
            //return Ok("FileName = " + fileName + " | FileExtension" + fileExtension);
            return Ok(fileName);
        }

        /// <summary>
        /// upload single file content
        /// </summary>
        [HttpPost("upload-filecontent", Name = "Upload-File-Content")]
        public async Task<ActionResult<string>> UploadFileContent([FromBody] FileContent file)
        {
            if (file != null)
            {
                var guid = await FileMan.UploadFileFromBase64(file);

                await DB.SaveChangesAsync();
                return Ok(guid.ToString());
            }
            else
            {
                return BadRequest("file not found");
            }
        }

        [HttpPost("upload-file-content", Name = "Upload-Content")]
        public async Task<ActionResult<string>> UploadContent([FromBody] FileContent file)
        {
                var guid = await FileMan.UploadFileFromBase64(file);
                await DB.SaveChangesAsync();
                return Ok(guid.ToString());
        }

        /// <summary>
        /// upload multiple file content
        /// </summary>
        [HttpPost("upload-filecontent-list", Name = "Upload-File-Content-List")]
        public async Task<ActionResult<List<string>>> UploadFileContentList([FromBody] List<FileContent> files)
        {
            if (files.Count > 0)
            {
                List<string> listGuid = new List<string>();

                foreach (var file in files)
                {
                    var guid = await FileMan.UploadFileFromBase64(file);
                    listGuid.Add(guid.ToString());
                }

                await DB.SaveChangesAsync();
                return Ok(listGuid);
            }
            else
            {
                return BadRequest("file not found");
            }
        }

        [HttpGet("get-image-url/{fileName}/{fileExtension}")]
        public async Task<ActionResult<string>> GetImageUrl(string fileName, string fileExtension)
        {
            var result = await this.FileMan.GetFileAsync(fileName, fileExtension);

            return Ok(result);
        }

        [HttpGet("get-image-detail/{id}")]
        public async Task<ActionResult<FileModel>> GetImageDetail(string id)
        {
            var result = await this.FileMan.GetFileDetailAsync(id);

            return Ok(result);
        }

        [HttpGet("get-blob-by-id/{id}")]
        public async Task<ActionResult<BlobModel>> GetBlobById(Guid id)
        {
            var data = await this.BlobMan.GetBlobById(id);
            return Ok(data);
        }

        [HttpDelete("delete-file/{fileName}/{fileExtension}")]
        public async Task<IActionResult> DeleteFile(Guid fileName, string fileExtension)
        {
            await this.BlobMan.DeleteBlob(fileName);
            await this.FileMan.DeleteFileAsync(fileName, fileExtension);

            return Ok();
        }

        [HttpGet("get-file-stream-url/{blobId}")]
        public async Task<ActionResult> GetFileStreamAsync(string blobId)
        {
            var result = await this.FileMan.DownloadFileAsync(blobId);

            if (result.Item3.ToLower() == "pdf")
            {
                return File(result.Item1, "application/pdf");
            }

            return Ok();
        }
    }
}
