using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductProgramService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideProductProgramService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }
        public async Task<List<UserSideProductProgramPaginateModel>> GetProductProgramPaginate(Guid productId)
        {
            var ProgramCategoryData = (from sl in this.DB.ProductProgramCategories
                                   where sl.IsDeleted == false && sl.ProductId == productId
                                   select new UserSideProductProgramCategoryModel
                                   {
                                       ProductProgramCategoryId = sl.ProductProgramCategoryId,
                                       ProductProgramCategoryName = sl.ProductProgramCategoryName
                                   }).ToList();

            var ProductProgramModelView = new List<UserSideProductProgramPaginateModel>();


            foreach (var item in ProgramCategoryData)
            {
                var listProductProgramInCategory = new UserSideProductProgramPaginateModel();

                var query2 = await (from c in DB.ProductProgramMappings
                                    join d in DB.Cms_Contents on c.Cms_ContentId equals d.Cms_ContentId
                                    where c.IsDeleted == false && c.ProductProgramCategoryId == item.ProductProgramCategoryId && c.ProductId == productId && c.ApprovedAt != null
                                    select new UserSideProductProgramViewModel
                                    {
                                        ProductProgramMappingId = c.ProductProgramMappingId,
                                        Cms_ContentId = c.Cms_ContentId,
                                        Cms_ContentName = d.Cms_ContentName,
                                        IsSequence = c.IsSequence
                                    }).OrderBy(Q => Q.IsSequence).AsNoTracking().ToListAsync();

                if(query2.Count() != 0)
                {
                    listProductProgramInCategory.ProductProgramCategory = item.ProductProgramCategoryName;
                    listProductProgramInCategory.ProductProgramTitles = query2;

                    ProductProgramModelView.Add(listProductProgramInCategory);
                }
            }

            return ProductProgramModelView;
        }
        public async Task<UserSideProductProgramModel> GetUserSideProductProgramDetail(Guid ProductProgramMappingId, Guid productId)
        {
            var productProgramData = await (from sl in this.DB.ProductProgramMappings
                                      join cat in this.DB.ProductProgramCategories on sl.ProductProgramCategoryId equals cat.ProductProgramCategoryId
                                      join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                      where sl.IsDeleted == false && sl.ProductProgramMappingId == ProductProgramMappingId && sl.ProductId == productId && sl.ApprovedAt != null
                                      orderby sl.IsSequence
                                      select new UserSideProductProgramModel
                                      {
                                          ProductProgramMappingId = sl.ProductProgramMappingId,
                                          ProductId = sl.ProductId,
                                          ProductProgramCategoryId = sl.ProductProgramCategoryId,
                                          ProductProgramCategoryName = cat.ProductProgramCategoryName,
                                          Cms_ContentId = sl.Cms_ContentId,
                                          Cms_ContentName = cnt.Cms_ContentName,
                                          Cms_ContentBlobId = cnt.BlobId.HasValue ? cnt.BlobId.Value : Guid.Empty,
                                          Cms_ContentVideoBlobId = cnt.CmsContentVideoBlobId.HasValue ? cnt.CmsContentVideoBlobId.Value : Guid.Empty,
                                          Cms_ContentFileBlobId = cnt.CmsContentFileBlobId.HasValue ? cnt.CmsContentFileBlobId.Value : Guid.Empty,
                                          Cms_ContentDescription = cnt.Cms_ContentDescription,
                                          IsSequence = sl.IsSequence,
                                          CreatedAt = sl.CreatedAt,
                                          CreatedBy = sl.CreatedBy,
                                          UpdatedAt = sl.UpdatedAt,
                                          UpdatedBy = sl.UpdatedBy
                                      }).FirstOrDefaultAsync();

            var imageURL = "";
            var imageFileName = "";
            var videoUrl = "";
            var videoFileName = "";
            var fileUrl = "";
            var fileFileName = "";

            var blobData = await this.BlobService.GetBlobById(productProgramData.Cms_ContentBlobId);

            var blobVideoData = await this.BlobService.GetBlobById(productProgramData.Cms_ContentVideoBlobId);

            var blobFileData = await this.BlobService.GetBlobById(productProgramData.Cms_ContentFileBlobId);

            if(blobData != null)
            {
                imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                imageFileName = blobData.Name;
            }

            if(blobVideoData != null)
            {
                videoUrl = await this.FileService.GetFileAsync(blobVideoData.BlobId.ToString(), blobVideoData.Mime);
                videoFileName = blobVideoData.Name;
            }

            if(blobFileData != null)
            {
                fileUrl = await this.FileService.GetFileAsync(blobFileData.BlobId.ToString(), blobFileData.Mime);
                fileFileName = blobFileData.Name;
            }
            
            productProgramData.Cms_ContentBlobImageUrl = imageURL;
            productProgramData.Cms_ContentBlobImageFileName = imageFileName;
            productProgramData.Cms_ContentVideoBlobImageUrl = videoUrl;
            productProgramData.Cms_ContentVideoBlobImageFileName = videoFileName;
            productProgramData.Cms_ContentFileBlobImageUrl = fileUrl;
            productProgramData.Cms_ContentFileBlobImageFileName = fileFileName;

            return productProgramData;
        }
    }
}
