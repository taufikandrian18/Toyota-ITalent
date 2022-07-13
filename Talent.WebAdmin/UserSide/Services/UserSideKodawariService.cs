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
    public class UserSideKodawariService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideKodawariService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }

        public async Task<List<UserSideKodawariBannerModel>> GetAllUserSideKodawariBanner()
        {
            var query2 = await (from c in DB.KodawariBanners
                                where c.IsDeleted == false && c.ApprovedAt != null
                                select new UserSideKodawariBannerModel
                                {
                                    KodawariBannerId = c.KodawariBannerId,
                                    BlobId = c.BlobId,
                                    KodawariBannerTitle = c.KodawariBannerTitle,
                                    CreatedAt = c.CreatedAt,
                                    CreatedBy = c.CreatedBy,
                                    UpdatedAt = c.UpdatedAt,
                                    UpdatedBy = c.UpdatedBy
                                }).AsNoTracking().ToListAsync();

            var index = 0;
            foreach (var datum in query2)
            {

                var imageURL = "";
                var fileFormat = "";
                var blobFileName = "";

                var blobData = await this.BlobService.GetBlobById(datum.BlobId);

                imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                fileFormat = blobData.Mime;
                blobFileName = blobData.Name;

                query2[index].ImageUrl = imageURL;
                query2[index].FileTypeBlob = fileFormat;
                query2[index].FileNameBlob = blobFileName;
                index++;
            }

            return query2;
        }

        public async Task<List<UserSideKodawariPaginateModel>> GetKodawariPaginate(Guid cms_MenuId, Guid kodawariMenuAwalId)
        {
            var KDMenuData = (from st in this.DB.Kodawaris
                              join sm in this.DB.KodawariTypes on st.KodawariTypeId equals sm.KodawariTypeId
                              where st.IsDeleted == false && st.Cms_MenuId == cms_MenuId && st.KodawariMenuId == kodawariMenuAwalId
                              select new UserSideKodawariMenuModel
                              {
                                  KDMenuId = sm.KodawariTypeId,
                                  KDMenuName = sm.KodawariTypeName
                              }).Distinct().ToList();

            var KodawariModelView = new List<UserSideKodawariPaginateModel>();


            foreach (var item in KDMenuData)
            {
                var listKDInCategory = new UserSideKodawariPaginateModel();

                var query2 = await (from c in DB.Kodawaris
                                    join e in DB.Cms_Contents on c.Cms_ContentId equals e.Cms_ContentId
                                    join d in DB.Cms_SubContents on c.Cms_SubContentId equals d.Cms_SubContentId into grpjoin_sl_scnt
                                    from scnt in grpjoin_sl_scnt.DefaultIfEmpty()
                                    where c.IsDeleted == false && c.KodawariTypeId == item.KDMenuId && c.Cms_MenuId == cms_MenuId && c.KodawariMenuId == kodawariMenuAwalId && c.ApprovedAt != null
                                    select new UserSideKodawariViewModel
                                    {
                                        KodawariId = c.KodawariId,
                                        //KodawariTypeId = c.KodawariTypeId,
                                        //KodawariTypeName = d.KodawariTypeName,
                                        Cms_ContentId = c.Cms_ContentId,
                                        Cms_ContentName = e.Cms_ContentName,
                                        Cms_SubContents = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == c.Cms_SubContentId).ToList(),
                                        IsSequence = c.KodawariSequence
                                    }).OrderBy(Q => Q.IsSequence).AsNoTracking().ToListAsync();

                listKDInCategory.KodawariType = item.KDMenuName;
                listKDInCategory.KodawariTitles = query2;

                KodawariModelView.Add(listKDInCategory);
            }

            return KodawariModelView;
        }
        public async Task<UserSideKodawariModel> GetUserSideKodawariDetail(Guid kodawariId)
        {
            var STData = await (from sl in this.DB.Kodawaris
                                join mnu in this.DB.Cms_Menus on sl.Cms_MenuId equals mnu.Cms_MenuId
                                join cat in this.DB.KodawariTypes on sl.KodawariTypeId equals cat.KodawariTypeId
                                join cnt in this.DB.Cms_Contents on sl.Cms_ContentId equals cnt.Cms_ContentId
                                join scnt in this.DB.Cms_SubContents on sl.Cms_SubContentId equals scnt.Cms_SubContentId into grpjoin_sl_scnt
                                from scnt in grpjoin_sl_scnt.DefaultIfEmpty()
                                where sl.IsDeleted == false && sl.KodawariId == kodawariId && sl.ApprovedAt != null
                                select new UserSideKodawariModel
                                {
                                    KodawariId = sl.KodawariId,
                                    KodawariTypeId = sl.KodawariTypeId,
                                    KodawariTypeName = cat.KodawariTypeName,
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
                                    IsSequence = sl.KodawariSequence,
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
            STData.Cms_SubContentFileBlobImageFileName = fileFileName;
            STData.Cms_SubContentBlobImageUrl = subImageURL;
            STData.Cms_SubContentBlobImageFileName = subImageFileName;
            STData.Cms_SubContentVideoBlobImageUrl = subVideoURL;
            STData.Cms_SubContentVideoBlobImageFileName = subVideoFileName;
            STData.Cms_SubContentFileBlobImageUrl = subFileURL;
            STData.Cms_SubContentFileBlobImageFileName = subFileFileName;

            return STData;
        }
        public async Task<List<UserSideKodawariMenuAwalModel>> GetKodawariMenuAwalPaginate()
        {
            var query2 = await (from c in DB.KodawariMenus
                                join d in DB.Kodawaris on c.KodawariMenuId equals d.KodawariMenuId
                                where c.IsDeleted == false && d.IsDeleted == false && d.ApprovedAt != null
                                orderby c.KodawariMenuSequence
                                select new UserSideKodawariMenuAwalModel
                                {
                                    KodawariMenuAwalId = c.KodawariMenuId,
                                    KodawariMenuAwalName = c.KodawariMenuName,
                                    KodawariMenuAwalSequence = c.KodawariMenuSequence
                                }).AsNoTracking().Distinct().ToListAsync();

            return query2;
        }
        public async Task<UserSideKodawariCategoryMenuPaginateModel> GetKodawariMenuCategoryPaginate(Guid kodawariMenuId)
        {

            var query2 = await (from c in DB.Cms_Menus
                                join d in DB.Kodawaris on c.Cms_MenuId equals d.Cms_MenuId
                                where c.IsDeleted == false && d.KodawariMenuId == kodawariMenuId && d.ApprovedAt != null
                                select new UserSideKodawariCategoryMenuModel
                                {
                                    Cms_MenuId = c.Cms_MenuId,
                                    Cms_MenuName = c.Cms_MenuName,
                                    Cms_MenuCategory = c.Cms_MenuCategory
                                }).AsNoTracking().Distinct().ToListAsync();

            return new UserSideKodawariCategoryMenuPaginateModel
            {
                KodawariMenuAwalId = kodawariMenuId,
                KodawariCategoryModel = query2
            };
        }
        public async Task<UserSideKodawariDownloadModel> GetKodawariDownloadDetail(Guid kodawariMenuId)
        {
            var data = await (from mo in DB.KodawariDownloads
                              join mn in this.DB.KodawariMenus on mo.KodawariMenuId equals mn.KodawariMenuId
                              where mo.KodawariMenuId == kodawariMenuId && mo.IsDeleted == false && mo.ApprovedAt != null
                              select new UserSideKodawariDownloadModel
                              {
                                  KodawariDownloadId = mo.KodawariDownloadId,
                                  KodawariMenuId = mo.KodawariMenuId,
                                  KodawariMenuName = DB.KodawariMenus.Where(x => x.KodawariMenuId == mo.KodawariMenuId).Select(x => x.KodawariMenuName).FirstOrDefault(),
                                  BlobId = mo.BlobId,
                                  KodawariDownloadTitle = mo.KodawariDownloadTitle,
                                  IsDownloadable = mo.IsDownloadable,
                                  CreatedAt = mo.CreatedAt,
                                  CreatedBy = mo.CreatedBy,
                                  UpdatedBy = mo.UpdatedBy,
                                  UpdatedAt = mo.UpdatedAt
                              }).FirstOrDefaultAsync();

            var imageURL = "";
            var fileType = "";
            var fileName = "";

            if(data != null)
            {
                var blobData = await this.BlobService.GetBlobById(data.BlobId);

                imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                fileType = blobData.Mime;
                fileName = blobData.Name;
                data.ImageUrl = imageURL;
                data.FileTypeBlob = fileType;
                data.FileNameBlob = fileName;
            }

            return data;
        }
    }
}
