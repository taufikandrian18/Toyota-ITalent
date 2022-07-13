using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductPhotoService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly BlobService BlobService;

        public UserSideProductPhotoService(TalentContext context, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.BlobService = blobService;
        }

        public async Task<List<UserSideProductPhotoModel>> GetAllUserSideProductPhoto(Guid productId,bool isPhoto360)
        {
            if(productId != Guid.Empty)
            {
                var query2 = await (from c in DB.ProductPhotos
                                    join d in DB.ProductPhoto360Mappings on c.ProductPhotoId equals d.ProductPhotoId
                                    join e in DB.Products on d.ProductId equals e.ProductId
                                    where c.IsDeleted == false && d.ProductId == productId && c.Is360Photo == isPhoto360 && c.ApprovedAt != null
                                    select new UserSideProductPhotoModel
                                    {
                                        ProductPhotoId = c.ProductPhotoId,
                                        BlobId = c.BlobId,
                                        CreatedBy = c.CreatedBy,
                                        UpdatedBy = c.UpdatedBy,
                                        CreatedAt = c.CreatedAt,
                                        UpdatedAt = c.UpdatedAt
                                    }).AsNoTracking().ToListAsync();

                var index = 0;
                foreach (var datum in query2)
                {

                    var imageURL = "";
                    var imageFileName = "";
                    var imageFileType = "";
                    var videoUrl = "";
                    var videoFileName = "";
                    var videoFileType = "";

                    var blobData = await this.BlobService.GetBlobById(datum.BlobId);
                    if(blobData != null)
                    {
                        if (blobData.Mime.Trim() != "mp4")
                        {
                            imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                            imageFileName = blobData.Name;
                            imageFileType = blobData.Mime;
                            query2[index].ImageUrl = imageURL;
                            query2[index].ImageFileName = imageFileName;
                            query2[index].ImageFileType = imageFileType;
                        }
                        else
                        {
                            videoUrl = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                            videoFileName = blobData.Name;
                            videoFileType = blobData.Mime;
                            query2[index].VideoUrl = videoUrl;
                            query2[index].VideoFileName = videoFileName;
                            query2[index].VideoFileType = videoFileType;
                        }
                    }
                    index++;
                }

                return query2;
            }
            else
            {
                var query2 = await (from c in DB.ProductPhotos
                                    join d in DB.ProductPhoto360Mappings on c.ProductPhotoId equals d.ProductPhotoId
                                    join e in DB.Products on d.ProductId equals e.ProductId
                                    where c.IsDeleted == false && c.Is360Photo == isPhoto360 && c.ApprovedAt != null
                                    select new UserSideProductPhotoModel
                                    {
                                        ProductPhotoId = c.ProductPhotoId,
                                        BlobId = c.BlobId,
                                        CreatedBy = c.CreatedBy,
                                        UpdatedBy = c.UpdatedBy,
                                        CreatedAt = c.CreatedAt,
                                        UpdatedAt = c.UpdatedAt
                                    }).AsNoTracking().ToListAsync();

                var index = 0;
                foreach (var datum in query2)
                {

                    var imageURL = "";
                    var imageFileName = "";
                    var imageFileType = "";
                    var blobData = await this.BlobService.GetBlobById(datum.BlobId);
                    if(blobData != null)
                    {
                        imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                        imageFileName = blobData.Name;
                        imageFileType = blobData.Mime;
                        query2[index].ImageUrl = imageURL;
                        query2[index].ImageFileName = imageFileName;
                        query2[index].ImageFileType = imageFileType;
                    }
                    index++;
                }

                return query2;
            }
        }
    }
}
