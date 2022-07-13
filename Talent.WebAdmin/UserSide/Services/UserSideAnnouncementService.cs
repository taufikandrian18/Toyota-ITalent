using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Microsoft.AspNetCore.Mvc;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideAnnouncementService : Controller
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly BlobService BlobService;


        public UserSideAnnouncementService(TalentContext db, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.FileService = fileService;
            this.BlobService = blobService;
        }

        public async Task<ActionResult<BaseResponse>> Insert(Announcements model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.AnnouncementID))
                {
                    model.AnnouncementID = Guid.NewGuid().ToString();
                }

                model.UpdatedAt = DateTime.Now;
                model.CreatedAt = DateTime.Now;

                var data = await DB.Announcements.AddAsync(model);
                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(Announcements model)
        {
            try
            {
                var data = await DB.Announcements.FirstOrDefaultAsync(Q => Q.AnnouncementID == model.AnnouncementID);
                if (data == null)
                {
                    var msg = new Message
                    {
                        Id = "Data tidak ditemukan",
                        En = "Data not found",
                    };
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                if (!String.IsNullOrWhiteSpace(model.Description))
                {
                    data.Description = model.Description;
                }
                if (!String.IsNullOrWhiteSpace(model.Title))
                {
                    data.Title = model.Title;
                }
                if (!String.IsNullOrWhiteSpace(model.AnnouncementFileID))
                {
                    data.AnnouncementFileID = model.AnnouncementFileID;
                }
                if (!String.IsNullOrWhiteSpace(model.AnnouncementFileType))
                {
                    data.AnnouncementFileType = model.AnnouncementFileType;
                }

                data.UpdatedAt = DateTime.Now;

                if (model.Status != null) {
                    data.Status = model.Status;
                }

                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> Delete(string id)
        {
            try
            {
                var data = await DB.Announcements.FirstOrDefaultAsync(Q => Q.AnnouncementID == id);
                if (data == null)
                {
                    var msg = new Message
                    {
                        Id = "Data tidak ditemukan",
                        En = "Data not found",
                    };
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                DB.Announcements.Remove(data);
                await DB.SaveChangesAsync();


                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Get(string id, bool? status)
        {
            try
            {
                var query = DB.Announcements.AsQueryable();
                if (!String.IsNullOrWhiteSpace(id))
                {
                    query = query.Where(Q => Q.AnnouncementID == id);
                }

                if (status != null) {
                    query = query.Where(Q => Q.Status == status.Value);
                }

                var count = query.Count();
                var data = await query.OrderByDescending(Q=> Q.UpdatedAt).ToListAsync();

                var index = 0;
                foreach (var datum in data) {
                    
                    var imageURL = "";
                    var blobData = await this.BlobService.GetBlobById(Guid.Parse(datum.AnnouncementFileID));

                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

                    data[index].AnnouncementFileID = imageURL;
                    index++;
                }


                return StatusCode(200, BaseResponse.ResponseOk(data, count));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
    }
}
