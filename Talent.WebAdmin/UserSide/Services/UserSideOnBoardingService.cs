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
    public class UserSideOnBoardingService : Controller
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly BlobService BlobService;


        public UserSideOnBoardingService(TalentContext db, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.FileService = fileService;
            this.BlobService = blobService;
        }

        public async Task<ActionResult<BaseResponse>> Insert(OnBoardings model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.OnBoardingID))
                {
                    model.OnBoardingID = Guid.NewGuid().ToString();
                }

                if (model.StatusView == null) {
                    model.StatusView = true;
                }
                var data = await DB.OnBoardings.AddAsync(model);
                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(OnBoardings model)
        {
            try
            {
                var data = await DB.OnBoardings.FirstOrDefaultAsync(Q => Q.OnBoardingID == model.OnBoardingID);
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
                if (model.SectionNumber != 0)
                {
                    data.SectionNumber = model.SectionNumber;
                }
                if (!String.IsNullOrWhiteSpace(model.OnBoardingFileURL))
                {
                    data.OnBoardingFileURL = model.OnBoardingFileURL;
                }
                if (model.StatusView != null)
                {
                    data.StatusView = model.StatusView;
                }
                if (!String.IsNullOrWhiteSpace(model.OnBoardingFileType))
                {
                    data.OnBoardingFileType = model.OnBoardingFileType;
                }

                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> UpdateOrder(OnBoardingModelList model)
        {
            try
            {

                foreach (var datum in model.onBoardings)
                {

                    var data = await DB.OnBoardings.FirstOrDefaultAsync(Q => Q.OnBoardingID == datum.ID);
                    if (data == null)
                    {
                        var msg = new Message
                        {
                            Id = "Data tidak ditemukan",
                            En = "Data not found",
                        };
                        return StatusCode(400, BaseResponse.BadRequest(null, msg));
                    }

                    data.SectionNumber = datum.Order;
                    data.StatusView = datum.Status;

                    await DB.SaveChangesAsync();
                }

                return StatusCode(200, BaseResponse.ResponseOk(model));
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
                var data = await DB.OnBoardings.FirstOrDefaultAsync(Q => Q.OnBoardingID == id);
                if (data == null)
                {
                    var msg = new Message
                    {
                        Id = "Data tidak ditemukan",
                        En = "Data not found",
                    };
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                DB.OnBoardings.Remove(data);
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
                var query = DB.OnBoardings.AsQueryable();
                if (!String.IsNullOrWhiteSpace(id))
                {
                    query = query.Where(Q => Q.OnBoardingID == id);
                }

                var data = await query.OrderBy(Q => Q.SectionNumber).ToListAsync();

                if (status != null) {
                    query = query.Where(Q => Q.StatusView == status);
                }

                var count = query.Count();

                var index = 0;
                foreach (var datum in data) {
                    
                    var imageURL = "";
                    var blobData = await this.BlobService.GetBlobById(Guid.Parse(datum.OnBoardingFileURL));

                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

                    data[index].OnBoardingFileURL = imageURL;
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
