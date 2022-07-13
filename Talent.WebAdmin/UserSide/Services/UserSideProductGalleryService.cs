using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;
using static Talent.WebAdmin.UserSide.Models.UserSideProductGalleryModel;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductGalleryService : Controller
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideProductGalleryService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }
        /// <summary>
        /// GEt all Product + pagination
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        ///

        public async Task<UserSideProductGalleryPaginate> GetAllUserSideProductGalleryFiltered(int pageSize, int pageIndex, string productGalleryColorCode,string productTypeName,Guid productId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var dataProductFiltered = new List<UserSideProductGalleryViewModel>();

            var dataProductGalleryCount = 0;

            var dataProductGalleryColorName = "";

            if (productId == Guid.Empty)
            {
                return new UserSideProductGalleryPaginate
                {
                    ProductGallery = dataProductFiltered,
                    TotalProductGalleries = dataProductGalleryCount,
                    ProductGalleryColorName = dataProductGalleryColorName
                };
            }

            var query2 = await (from c in DB.ProductGalleries
                                where c.IsDeleted == false && c.ProductGalleryColorCode.ToUpper() == "#"+productGalleryColorCode.ToUpper() && c.ProductId == productId && c.IsApproved == "Published" && c.ApprovedAt != null
                                select new UserSideProductGalleryGridModel
                                {
                                    ProductGalleryId = c.ProductGalleryId,
                                    ProductId = c.ProductId,
                                    BlobId = c.BlobId,
                                    ProductTypeId = c.ProductTypeId,
                                    ProductGalleryColorCode = c.ProductGalleryColorCode,
                                    ProductGalleryColorName = c.ProductGalleryColorName,
                                    IsColor = c.IsColor,
                                    IsApproved = c.IsApproved,
                                    CreatedAt = c.CreatedAt,
                                    UpdatedAt = c.UpdatedAt
                                }).AsNoTracking().ToListAsync();

            var productTypeGuid = Guid.Empty;

            if(productTypeName != "All" && productTypeName != null)
            {
                productTypeGuid = (from d in DB.ProductTypes
                                   where d.IsDeleted == false && d.ProductTypeName == productTypeName.Trim() && d.ProductId == productId
                                   select d.ProductTypeId).FirstOrDefault();
            }
            
            dataProductGalleryColorName = query2.Where(x => x.ProductGalleryColorCode.ToUpper().Contains(productGalleryColorCode.ToUpper()))
                .Select(x => x.ProductGalleryColorName).FirstOrDefault();

            if(dataProductGalleryColorName != null)
            {
                if (productTypeGuid != Guid.Empty)
                {
                    dataProductFiltered = query2.Where(x => x.ProductGalleryColorCode.ToUpper().Contains(productGalleryColorCode.ToUpper()) && x.ProductTypeId == productTypeGuid)
                    .Select(x => new UserSideProductGalleryViewModel
                    {
                        ProductGalleryId = x.ProductGalleryId,
                        ProductId = x.ProductId,
                        BlobId = x.BlobId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .Skip((int)skipCount)
                    .Take(pageSize)
                    .ToList();
                }
                else
                {
                    dataProductFiltered = query2.Where(x => x.ProductGalleryColorCode.ToUpper().Contains(productGalleryColorCode.ToUpper()))
                    .Select(x => new UserSideProductGalleryViewModel
                    {
                        ProductGalleryId = x.ProductGalleryId,
                        ProductId = x.ProductId,
                        BlobId = x.BlobId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .Skip((int)skipCount)
                    .Take(pageSize)
                    .ToList();
                }
            }
            else
            {
                if (productTypeGuid != Guid.Empty)
                {
                    dataProductFiltered = query2.Where(x => x.ProductTypeId == productTypeGuid)
                    .Select(x => new UserSideProductGalleryViewModel
                    {
                        ProductGalleryId = x.ProductGalleryId,
                        ProductId = x.ProductId,
                        BlobId = x.BlobId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .Skip((int)skipCount)
                    .Take(pageSize)
                    .ToList();
                }
                else
                {
                    dataProductFiltered = query2
                    .Select(x => new UserSideProductGalleryViewModel
                    {
                        ProductGalleryId = x.ProductGalleryId,
                        ProductId = x.ProductId,
                        BlobId = x.BlobId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .Skip((int)skipCount)
                    .Take(pageSize)
                    .ToList();
                }
            }
   

            dataProductGalleryCount = dataProductFiltered.Count();

            var index = 0;
            foreach (var datum in dataProductFiltered)
            {

                var imageURL = "";
                var imageFileName = "";
                var blobData = await this.BlobService.GetBlobById(datum.BlobId);
                if(blobData != null)
                {
                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                    imageFileName = blobData.Name;
                    dataProductFiltered[index].ImageUrl = imageURL;
                    dataProductFiltered[index].ImageFileName = imageFileName;
                }
                index++;
            }

            return new UserSideProductGalleryPaginate
            {
                ProductGallery = dataProductFiltered,
                TotalProductGalleries = dataProductGalleryCount,
                ProductGalleryColorName = dataProductGalleryColorName
            };
        }

        public async Task<List<UserSideProductGalleryColorListView>> GetAllUserSideProductGalleryColorCode(Guid productId)
        {
            var query = this.DB.ProductGalleries.Where(Q => Q.ProductId == productId && Q.IsDeleted == false).Select(Q => new UserSideProductGalleryColorListView
            {
                ProductGalleryColorCode = Q.ProductGalleryColorCode
            }).Distinct();

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<ActionResult<BaseResponse>> ContributeNewPhotoGalleryAsync(UserSideProductGalleryContributeModel model, string employeeId)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);

                if (model.ProductGalleryFileContent != null)
                {
                    if (string.IsNullOrEmpty(model.ProductGalleryFileContent.Base64) == false)
                    {
                        getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
                    }
                }

                var newProductGalleries = new ProductGalleries
                {
                    ProductId = model.ProductId,
                    ProductTypeId = model.ProductTypeId,
                    BlobId = getUploadModuleBlob,
                    ProductGalleryColorCode = model.ProductGalleryColorCode,
                    ProductGalleryColorName = model.ProductGalleryColorName,
                    IsColor = model.IsColor,
                    IsApproved = "Draft",
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = employeeId,
                    UpdatedBy = employeeId
                };

                this.DB.ProductGalleries.Add(newProductGalleries);

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
    }
}
