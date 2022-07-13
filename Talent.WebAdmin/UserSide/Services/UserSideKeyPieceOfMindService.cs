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
    public class UserSideKeyPieceOfMindService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideKeyPieceOfMindService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }
        public async Task<List<UserSideKeyPieceOfMindPaginateModel>> GetKeyPieceOfMindPaginate(Guid keyPieceOfMindMenuId)
        {
            var KPOMMenuData = (from sl in this.DB.Cms_Menus
                                join kp in this.DB.KeyPieceOfMinds on sl.Cms_MenuId equals kp.Cms_MenuId
                                join kpm in this.DB.KeyPieceOfMindMenus on kp.KeyPieceOfMindMenuId equals kpm.KeyPieceOfMindMenuId
                                where sl.IsDeleted == false && sl.Cms_MenuCategory == "KPOM" && kp.KeyPieceOfMindMenuId == keyPieceOfMindMenuId && kp.ApprovedAt != null
                                select new UserSideKeyPieceOfMindMenuModel
                                {
                                    KeyPieceOfMindMenuId = sl.Cms_MenuId,
                                    KeyPieceOfMindMenuName = sl.Cms_MenuName
                                }).Distinct().ToList();

            var KeyPieceOfMindModelView = new List<UserSideKeyPieceOfMindPaginateModel>();


            foreach (var item in KPOMMenuData)
            {
                var listKPOMInCategory = new UserSideKeyPieceOfMindPaginateModel();

                var query2 = await (from c in DB.KeyPieceOfMinds
                                    join d in DB.KeyPieceOfMindTypes on c.KeyPieceOfMindTypeId equals d.KeyPieceOfMindTypeId
                                    where c.IsDeleted == false && c.Cms_MenuId == item.KeyPieceOfMindMenuId && c.ApprovedAt != null
                                    select new UserSideKeyPieceOfMindViewModel
                                    {
                                        KeyPieceOfMindId = c.KeyPieceOfMindId,
                                        KeyPieceOfMindTypeId = c.KeyPieceOfMindTypeId,
                                        KeyPieceOfMindTypeName = d.KeyPieceOfMindTypeName,
                                        Cms_ContentId = c.Cms_ContentId,
                                        IsSequence = c.IsSequence
                                    }).OrderBy(Q => Q.IsSequence).AsNoTracking().ToListAsync();

                listKPOMInCategory.KeyPieceOfMindType = item.KeyPieceOfMindMenuName;
                listKPOMInCategory.KeyPieceOfMindTitles = query2;

                KeyPieceOfMindModelView.Add(listKPOMInCategory);
            }

            return KeyPieceOfMindModelView;
        }
        public async Task<UserSideKeyPieceOfMindModel> GetUserSideKeyPieceOfMindDetail(Guid KeyPieceOfMindId)
        {
            var kpomData = await (from sl in this.DB.KeyPieceOfMinds
                                            join mnu in this.DB.Cms_Menus on sl.Cms_MenuId equals mnu.Cms_MenuId
                                            join cat in this.DB.KeyPieceOfMindTypes on sl.KeyPieceOfMindTypeId equals cat.KeyPieceOfMindTypeId
                                            join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                            join scnt in this.DB.Cms_SubContents on sl.Cms_SubContentId equals scnt.Cms_SubContentId into grpjoin_sl_scnt
                                            from scnt in grpjoin_sl_scnt.DefaultIfEmpty()
                                            where sl.IsDeleted == false && sl.KeyPieceOfMindId == KeyPieceOfMindId && sl.ApprovedAt != null
                                            orderby sl.IsSequence
                                            select new UserSideKeyPieceOfMindModel
                                            {
                                                KeyPieceOfMindId = sl.KeyPieceOfMindId,
                                                KeyPieceOfMindTypeId = sl.KeyPieceOfMindTypeId,
                                                KeyPieceOfMindTypeName = cat.KeyPieceOfMindTypeName,
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


            var blobData = await this.BlobService.GetBlobById(kpomData.Cms_ContentBlobId);
            var blobVideo = await this.BlobService.GetBlobById(kpomData.Cms_ContentVideoBlobId);
            var blobFile = await this.BlobService.GetBlobById(kpomData.Cms_ContentFileBlobId);

            var subBlobData = await this.BlobService.GetBlobById(kpomData.Cms_SubContentBlobId);
            var subBlobVideo = await this.BlobService.GetBlobById(kpomData.Cms_SubContentVideoBlobId);
            var subBlobFile = await this.BlobService.GetBlobById(kpomData.Cms_SubContentFileBlobId);

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
                
            kpomData.Cms_ContentBlobImageUrl = imageURL;
            kpomData.Cms_ContentBlobImageFileName = imageFileName;
            kpomData.Cms_ContentVideoBlobImageUrl = videoURL;
            kpomData.Cms_ContentVideoBlobImageFileName = videoFileName;
            kpomData.Cms_ContentFileBlobImageUrl = fileURL;
            kpomData.Cms_ContentFileBlobImageFileName = fileFileName;
            kpomData.Cms_SubContentBlobImageUrl = subImageURL;
            kpomData.Cms_SubContentBlobImageFileName = subImageFileName;
            kpomData.Cms_SubContentVideoBlobImageUrl = subVideoURL;
            kpomData.Cms_SubContentVideoBlobImageFileName = subVideoFileName;
            kpomData.Cms_SubContentFileBlobImageUrl = subFileURL;
            kpomData.Cms_SubContentFileBlobImageFileName = subFileFileName;

            return kpomData;
        }
        public async Task<List<UserSideKeyPieceOfMindMenuAwalModel>> GetUserSideKeyPieceOfMindMenuAwalPaginate()
        {
            var query2 = await (from c in DB.KeyPieceOfMindMenus
                                join d in DB.KeyPieceOfMinds on c.KeyPieceOfMindMenuId equals d.KeyPieceOfMindMenuId
                                where c.IsDeleted == false && d.IsDeleted == false && d.ApprovedAt != null
                                orderby c.CreatedAt
                                select new UserSideKeyPieceOfMindMenuAwalModel
                                {
                                    KeyPieceOfMindMenuAwalId = c.KeyPieceOfMindMenuId,
                                    KeyPieceOfMindMenuAwalName = c.KeyPieceOfMindMenuName,
                                    KeyPieceOfMindMenuAwalSequence = c.KeyPieceOfMindMenuSequence
                                }).AsNoTracking().Distinct().ToListAsync();

            return query2;
        }
    }
}
