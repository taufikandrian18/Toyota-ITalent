using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideServiceTipService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideServiceTipService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }
        public async Task<List<UserSideServiceTipPaginateModel>> GetServiceTipPaginate(Guid serviceTipMenuId)
        {
            var STMenuData = (from sl in this.DB.Cms_Menus
                              join st in this.DB.ServiceTips on sl.Cms_MenuId equals st.Cms_MenuId
                              join sm in this.DB.ServiceTipMenus on st.ServiceTipMenuId equals sm.ServiceTipMenuId
                              where sl.IsDeleted == false && sl.Cms_MenuCategory == "ServiceTips" && st.ServiceTipMenuId == serviceTipMenuId && st.ApprovedAt != null
                              select new UserSideServiceTipMenuModel
                              {
                                  STMenuId = sl.Cms_MenuId,
                                  STMenuName = sl.Cms_MenuName
                              }).Distinct().ToList();

            var ServiceTipModelView = new List<UserSideServiceTipPaginateModel>();


            foreach (var item in STMenuData)
            {
                var listSTInCategory = new UserSideServiceTipPaginateModel();

                var query2 = await (from c in DB.ServiceTips
                                    join d in DB.ServiceTipTypes on c.ServiceTipTypeId equals d.ServiceTipTypeId
                                    where c.IsDeleted == false && c.Cms_MenuId == item.STMenuId && c.ApprovedAt != null
                                    select new UserSideServiceTipViewModel
                                    {
                                        ServiceTipId = c.ServiceTipId,
                                        ServiceTipTypeId = c.ServiceTipTypeId,
                                        ServiceTipTypeName = d.ServiceTipTypeName,
                                        Cms_ContentId = c.Cms_ContentId,
                                        IsSequence = c.IsSequence
                                    }).OrderBy(Q => Q.IsSequence).AsNoTracking().ToListAsync();

                listSTInCategory.ServiceTipType = item.STMenuName;
                listSTInCategory.ServiceTipTitles = query2;

                ServiceTipModelView.Add(listSTInCategory);
            }

            return ServiceTipModelView;
        }
        public async Task<UserSideServiceTipModel> GetUserSideServiceTipDetail(Guid ServiceTipId)
        {
            var STData = await (from sl in this.DB.ServiceTips
                                  join mnu in this.DB.Cms_Menus on sl.Cms_MenuId equals mnu.Cms_MenuId
                                  join cat in this.DB.ServiceTipTypes on sl.ServiceTipTypeId equals cat.ServiceTipTypeId
                                  join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                  join scnt in this.DB.Cms_SubContents on sl.Cms_SubContentId equals scnt.Cms_SubContentId into grpjoin_sl_scnt
                                  from scnt in grpjoin_sl_scnt.DefaultIfEmpty()
                                  where sl.IsDeleted == false && sl.ServiceTipId == ServiceTipId && sl.ApprovedAt != null
                                  orderby sl.IsSequence
                                  select new UserSideServiceTipModel
                                  {
                                      ServiceTipId = sl.ServiceTipId,
                                      ServiceTipTypeId = sl.ServiceTipTypeId,
                                      ServiceTipTypeName = cat.ServiceTipTypeName,
                                      Cms_MenuId = sl.Cms_MenuId,
                                      Cms_MenuName = mnu.Cms_MenuName,
                                      Cms_ContentId = sl.Cms_ContentId,
                                      Cms_ContentName = cnt.Cms_ContentName,
                                      Cms_ContentBlobId = cnt.BlobId.HasValue ? cnt.BlobId.Value : Guid.Empty,
                                      Cms_ContentVideoBlobId = cnt.CmsContentVideoBlobId.HasValue ? cnt.CmsContentVideoBlobId.Value : Guid.Empty,
                                      Cms_ContentFileBlobId = cnt.CmsContentFileBlobId.HasValue ? cnt.CmsContentFileBlobId.Value : Guid.Empty,
                                      Cms_ContentDescription = cnt.Cms_ContentDescription,
                                      Cms_SubContentId = sl.Cms_SubContentId,
                                      Cms_SubContentName = scnt.Cms_SubContentName,
                                      Cms_SubContentBlobId = scnt.BlobId.HasValue ? scnt.BlobId.Value : Guid.Empty,
                                      Cms_SubContentVideoBlobId = scnt.CmsSubContentVideoBlobId.HasValue ? scnt.CmsSubContentVideoBlobId.Value : Guid.Empty,
                                      Cms_SubContentFileBlobId = scnt.CmsSubContentFileBlobId.HasValue ? scnt.CmsSubContentFileBlobId.Value : Guid.Empty,
                                      Cms_SubContentDescription = scnt.Cms_SubContentDescription,
                                      IsSequence = sl.IsSequence,
                                      CreatedAt = sl.CreatedAt,
                                      CreatedBy = sl.CreatedBy,
                                      UpdatedAt = sl.UpdatedAt,
                                      UpdatedBy = sl.UpdatedBy
                                  }).FirstOrDefaultAsync();

            var imageURL = "";
            var imageFileName = "";
            var videoURL = "";
            var videoFileName = "";
            var fileURL = "";
            var fileFileName = "";

            var subImageURL = "";
            var subImageFileName = "";
            var subVideoURL = "";
            var subVideoFileName = "";
            var subFileURL = "";
            var subFileFileName = "";


            var blobData = await this.BlobService.GetBlobById(STData.Cms_ContentBlobId);
            var blobVideo = await this.BlobService.GetBlobById(STData.Cms_ContentVideoBlobId);
            var blobFile = await this.BlobService.GetBlobById(STData.Cms_ContentFileBlobId);

            var subBlobData = await this.BlobService.GetBlobById(STData.Cms_SubContentBlobId);
            var subBlobVideo = await this.BlobService.GetBlobById(STData.Cms_SubContentVideoBlobId);
            var subBlobFile = await this.BlobService.GetBlobById(STData.Cms_SubContentFileBlobId);

            if (blobData != null)
            {
                imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                imageFileName = blobData.Name;
            }
                
            if (blobVideo != null)
            {
                videoURL = await this.FileService.GetFileAsync(blobVideo.BlobId.ToString(), blobVideo.Mime);
                videoFileName = blobVideo.Name;
            }
                
            if (blobFile != null)
            {
                fileURL = await this.FileService.GetFileAsync(blobFile.BlobId.ToString(), blobFile.Mime);
                fileFileName = blobFile.Name;
            }
                
            if (subBlobData != null)
            {
                subImageURL = await this.FileService.GetFileAsync(subBlobData.BlobId.ToString(), subBlobData.Mime);
                subImageFileName = subBlobData.Name;
            }
                
            if (subBlobVideo != null)
            {
                subVideoURL = await this.FileService.GetFileAsync(subBlobVideo.BlobId.ToString(), subBlobVideo.Mime);
                subVideoFileName = subBlobVideo.Name;
            }
                
            if (subBlobFile != null)
            {
                subFileURL = await this.FileService.GetFileAsync(subBlobFile.BlobId.ToString(), subBlobFile.Mime);
                subFileFileName = subBlobFile.Name;
            }

            STData.Cms_ContentBlobImageUrl = imageURL;
            STData.Cms_ContentBlobImageFileName = imageFileName;
            STData.Cms_ContentVideoBlobImageUrl = videoURL;
            STData.Cms_ContentVideoBlobImageFileName = videoFileName;
            STData.Cms_ContentFileBlobImageUrl = fileURL;
            STData.Cms_ContentFileBlobImageFileName = fileFileName;
            STData.Cms_SubContentBlobImageUrl = subImageURL;
            STData.Cms_SubContentBlobImageFileName = subImageFileName;
            STData.Cms_SubContentVideoBlobImageUrl = subVideoURL;
            STData.Cms_SubContentVideoBlobImageFileName = subVideoFileName;
            STData.Cms_SubContentFileBlobImageUrl = subFileURL;
            STData.Cms_SubContentFileBlobImageFileName = subFileFileName;

            return STData;
        }
        public async Task<List<UserSideServiceTipMenuAwalModel>> GetServiceTipMenuAwalPaginate()
        {
            var query2 = await (from c in DB.ServiceTipMenus
                                join d in DB.ServiceTips on c.ServiceTipMenuId equals d.ServiceTipMenuId
                                where c.IsDeleted == false && d.IsDeleted == false && d.ApprovedAt != null
                                orderby c.ServiceTipMenuSequence
                                select new UserSideServiceTipMenuAwalModel
                                {
                                    ServiceTipMenuAwalId = c.ServiceTipMenuId,
                                    ServiceTipMenuAwalName = c.ServiceTipMenuName,
                                    ServiceTipMenuAwalSequence = c.ServiceTipMenuSequence
                                }).AsNoTracking().Distinct().ToListAsync();

            return query2;
        }
    }
}
