using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductSPWAService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideProductSPWAService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }
        public async Task<List<UserSideProductSPWAPaginateModel>> GetProductSPWAPaginate(Guid productId)
        {
            var SPWAMenuData = (from sl in this.DB.Cms_Menus
                                where sl.IsDeleted == false && sl.Cms_MenuCategory == "SPWA" || sl.Cms_MenuCategory == "TEST DRIVE"
                                select new UserSideProductSPWAMenuModel
                                {
                                    ProductSPWAMenuId = sl.Cms_MenuId,
                                    ProductSPWAMenuName = sl.Cms_MenuName
                                }).ToList();

            var ProductSPWAModelView = new List<UserSideProductSPWAPaginateModel>();


            foreach (var item in SPWAMenuData)
            {
                var listProductSPWAInCategory = new UserSideProductSPWAPaginateModel();

                var query2 = await (from c in DB.ProductSPWAMappings
                                    join d in DB.ProductSPWACategories on c.ProductSPWACategoryId equals d.ProductSPWACategoryId
                                    where c.IsDeleted == false && c.Cms_MenuId == item.ProductSPWAMenuId && c.ProductId == productId && c.ApprovedAt != null
                                    orderby d.ProductSPWACategoryName
                                    select new UserSideProductSPWAViewModel
                                    {
                                        ProductSPWACategoryId = c.ProductSPWACategoryId,
                                        ProductSPWACategoryName = d.ProductSPWACategoryName
                                    }).Distinct().AsNoTracking().ToListAsync();

                if(query2.Count() > 0)
                {
                    listProductSPWAInCategory.ProductId = productId;
                    listProductSPWAInCategory.ProductSPWAMenuId = item.ProductSPWAMenuId;
                    listProductSPWAInCategory.ProductSPWAMenuName = item.ProductSPWAMenuName;
                    listProductSPWAInCategory.ProductSPWACategories = query2;

                    ProductSPWAModelView.Add(listProductSPWAInCategory);
                }
                
            }

            return ProductSPWAModelView;
        }
        public async Task<UserSideProductSPWAContentViewModel> GetProductSPWAContentListView(Guid productSPWAMenuId,Guid productSPWACategoryId,Guid productId)
        {
            var SPWAContentList =   await (from sl in this.DB.ProductSPWAMappings
                                           join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                           where sl.IsDeleted == false && sl.Cms_MenuId == productSPWAMenuId && sl.ProductSPWACategoryId == productSPWACategoryId && sl.ProductId == productId && sl.ApprovedAt != null
                                           orderby sl.IsSequence
                                            select new UserSideProductSPWAContentNameList
                                            {
                                                ProductSPWAMappingId = sl.ProductSPWAMappingId,
                                                Cms_ContentId = sl.Cms_ContentId,
                                                ProducSPWAContentName = sl.IsSequence.ToString() + "." + cnt.Cms_ContentName,
                                                IsSequence = sl.IsSequence
                                            }).ToListAsync();

            var getBlob = this.DB.ProductSPWACategories.Where(Q => Q.ProductSPWACategoryId == productSPWACategoryId).Select(Q => Q.BlobId).FirstOrDefault();
            var getCategoryName = this.DB.ProductSPWACategories.Where(Q => Q.ProductSPWACategoryId == productSPWACategoryId).Select(Q => Q.ProductSPWACategoryName).FirstOrDefault();

            var imageURL = "";
            var imageFileName = "";
            var fileType = "";
            var blobCategory = Guid.Empty;

            if(getBlob != null)
            {
                var blobData = await this.BlobService.GetBlobById(getBlob);

                imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                fileType = blobData.Mime;
                imageFileName = blobData.Name;
                blobCategory = blobData.BlobId;
            }

            return new UserSideProductSPWAContentViewModel
            {
                ProductSPWACategoryName = getCategoryName,
                BlobId = blobCategory,
                BlobImageUrl = imageURL,
                FileType = fileType,
                BlobImageFileName = imageFileName,
                ProductSPWAContentNameList = SPWAContentList
            };
        }
        public async Task<UserSideProductSPWATestDriveContentViewModel> GetProductSPWATestDriveContentListView(Guid productSPWAMenuId, Guid productSPWACategoryId, Guid productId)
        {
            var SPWAContentList = await (from sl in this.DB.ProductSPWAMappings
                                         join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                         where sl.IsDeleted == false && sl.Cms_MenuId == productSPWAMenuId && sl.ProductSPWACategoryId == productSPWACategoryId && sl.ProductId == productId && sl.ApprovedAt != null
                                         orderby sl.IsSequence
                                         select new UserSideProductSPWAContentNameList
                                         {
                                             ProductSPWAMappingId = sl.ProductSPWAMappingId,
                                             Cms_ContentId = sl.Cms_ContentId,
                                             ProducSPWAContentName = cnt.Cms_ContentName,
                                             IsSequence = sl.IsSequence
                                         }).ToListAsync();

            var getCategoryName = this.DB.ProductSPWACategories.Where(Q => Q.ProductSPWACategoryId == productSPWACategoryId).Select(Q => Q.ProductSPWACategoryName).FirstOrDefault();
            var productSPWATDList = new List<UserSideProductSPWATestDriveBlobModel>();

            var index = 0;
            foreach(var item in SPWAContentList)
            {
                var blobTDModel = new UserSideProductSPWATestDriveBlobModel();
                var getContentBlob = await (from blb in this.DB.Cms_Contents
                                            where blb.IsDeleted == false && blb.Cms_ContentId == item.Cms_ContentId && blb.Cms_ContentCategory == "SPWA"
                                            select blb.BlobId).FirstOrDefaultAsync();

                if (getContentBlob != null)
                {
                    var blobData = await this.BlobService.GetBlobById(getContentBlob);

                    blobTDModel.BlobId = getContentBlob.HasValue ? getContentBlob.Value : Guid.Empty;
                    blobTDModel.BlobIndex = index + 1;
                    blobTDModel.BlobFileUrl = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                    blobTDModel.BlobFileType = blobData.Mime;
                    blobTDModel.BlobFileName = blobData.Name;

                    productSPWATDList.Add(blobTDModel);
                }
                index++;
            }

            return new UserSideProductSPWATestDriveContentViewModel
            {
                ProductSPWACategoryName = getCategoryName,
                BlobModels = productSPWATDList,
                ProductSPWAContentNameList = SPWAContentList
            };
        }
        public async Task<UserSideProductSPWAModel> GetUserSideProductSPWADetail(Guid ProductSPWAMappingId, Guid productId)
        {
            var productProgramData = await (from sl in this.DB.ProductSPWAMappings
                                            join mnu in this.DB.Cms_Menus on sl.Cms_MenuId equals mnu.Cms_MenuId
                                            join cat in this.DB.ProductSPWACategories on sl.ProductSPWACategoryId equals cat.ProductSPWACategoryId
                                            join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                            where sl.IsDeleted == false && sl.ProductSPWAMappingId == ProductSPWAMappingId && sl.ProductId == productId && sl.ApprovedAt != null
                                            orderby sl.IsSequence
                                            select new UserSideProductSPWAModel
                                            {
                                                ProductSPWAMappingId = sl.ProductSPWAMappingId,
                                                ProductId = sl.ProductId,
                                                ProductSPWACategoryId = sl.ProductSPWACategoryId,
                                                ProductSPWACategoryName = cat.ProductSPWACategoryName,
                                                Cms_MenuId = sl.Cms_MenuId,
                                                Cms_MenuName = mnu.Cms_MenuName,
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
