using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using static Talent.WebAdmin.UserSide.Enums.UserSideProductFeatureCategoryEnum;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductFeatureService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly BlobService BlobService;

        public UserSideProductFeatureService(TalentContext context, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.BlobService = blobService;
        }
        public async Task<List<UserSideProductFeatureCategoryFilter>> GetUserSideFeatureCategoryFilterList()
        {
            var enumData = await (from c in DB.ProductFeatureCategories
                                  where c.IsDeleted == false
                                  select new UserSideProductFeatureCategoryFilter
                                  {
                                        ProductFeatureCategoryId = c.ProductFeatureCategoryId,
                                        ProductFeatureCategoryName = c.ProductFeatureCategoryName
                                  }).AsNoTracking().ToListAsync();

            return enumData;
        }
        public async Task<List<UserSideProductFeatureFilter>> GetUserSideFeatureFilterList(Guid productFeatureCategoryId,Guid productId, Guid productTypeId)
        {
            var query2 = await (from c in DB.ProductFeatureMappings
                                join d in DB.ProductFeatures on c.ProductFeatureId equals d.ProductFeatureId
                                where c.IsDeleted == false && c.ProductId == productId && c.ProductTypeId == productTypeId && c.ProductFeatureCategoryId == productFeatureCategoryId && c.EnumContributorFlagging.Trim() == "FromAdmin" && c.ApprovedAt != null
                                orderby c.CreatedAt
                                select new UserSideProductFeatureFilter
                                {
                                    ProductFeatureMappingId = c.ProductFeatureMappingId,
                                    FilterId = d.ProductFeatureId,
                                    FilterName = d.ProductFeatureName
                                }).Distinct().AsNoTracking().ToListAsync();

            return query2;
        }

        public async Task<UserSideProductFeatureListModel> GetUserSideFeatureMappingDetail(Guid productFeatureCategoryId, Guid productId, Guid productTypeId, Guid productFeatureId)
        {
            var query2 = await (from c in DB.ProductFeatureMappings
                                join cnt in DB.Cms_Contents on c.Cms_ContentId equals cnt.Cms_ContentId into pa
                                from cnt in pa.DefaultIfEmpty()
                                join fmb in DB.Cms_Fmbs on c.Cms_FmbId equals fmb.Cms_FmbId into pb
                                from fmb in pb.DefaultIfEmpty()
                                join ope in DB.Cms_Operations on c.Cms_OperationId equals ope.Cms_OperationId into pc
                                from ope in pc.DefaultIfEmpty()
                                join wp in DB.Cms_WorkPrincipals on c.Cms_WorkPrincipalId equals wp.Cms_WorkPrincipalId into pd
                                from wp in pd.DefaultIfEmpty()
                                join stg in DB.Cms_Settings on c.Cms_SettingId equals stg.Cms_SettingId into pe
                                from stg in pe.DefaultIfEmpty()
                                where c.IsDeleted == false && c.ProductId == productId && c.ProductTypeId == productTypeId && c.ProductFeatureId == productFeatureId && c.ProductFeatureCategoryId == productFeatureCategoryId && c.ApprovedAt != null
                                orderby cnt.Cms_ContentSequence
                                select new UserSideProductFeatureListModel
                                {
                                    ProductFeatureMappingId = c.ProductFeatureMappingId,
                                    ProductId = c.ProductId,
                                    ProductTypeId = c.ProductTypeId,
                                    ProductFeatureId = c.ProductFeatureId,
                                    ProductFeatureCategoryId = c.ProductFeatureCategoryId,
                                    Cms_FmbId = c.Cms_FmbId,
                                    Cms_FmbBlobId = fmb.BlobId,
                                    Cms_FmbDescription = fmb.Cms_FmbDescription,
                                    Cms_WorkPrincipalId = c.Cms_WorkPrincipalId,
                                    Cms_WorkPrincipalBlobId = wp.BlobId,
                                    Cms_WorkPrincipalDescription = wp.Cms_WorkPrincipalDescription,
                                    Cms_ContentId = c.Cms_ContentId,
                                    Cms_ContentBlobId = cnt.BlobId,
                                    Cms_ContentVideoBlobId = cnt.CmsContentVideoBlobId,
                                    Cms_ContentFileBlobId = cnt.CmsContentFileBlobId,
                                    Cms_ContentName = cnt.Cms_ContentName,
                                    Cms_ContentDescription = cnt.Cms_ContentDescription,
                                    Cms_OperationId = c.Cms_OperationId,
                                    Cms_OperationBlobId = ope.BlobId,
                                    Cms_OperationDescription = ope.Cms_OperationDescription,
                                    Cms_SettingId = c.Cms_SettingId,
                                    Cms_SettingBlobId = stg.BlobId,
                                    Cms_SettingDescription = stg.Cms_SettingDescription,
                                    IsSpecial = c.IsSpecial
                                }).AsNoTracking().FirstOrDefaultAsync();

            var imageURL = "";
            var imageFileName = "";
            var videoUrl = "";
            var videoFileName = "";
            var fileUrl = "";
            var fileFileName = "";

            var blobData = await this.BlobService.GetBlobById(query2.Cms_ContentBlobId);

            var blobVideoData = await this.BlobService.GetBlobById(query2.Cms_ContentVideoBlobId);

            var blobFileData = await this.BlobService.GetBlobById(query2.Cms_ContentFileBlobId);

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

            if (blobFileData != null)
            {
                fileUrl = await this.FileService.GetFileAsync(blobFileData.BlobId.ToString(), blobFileData.Mime);
                fileFileName = blobFileData.Name;
            }

            var fmbImageUrl = "";
            var wpImageUrl = "";
            var opeImageUrl = "";
            var stgImageUrl = "";

            var fmbFileType = "";
            var wpFileType = "";
            var opeFileType = "";
            var stgFileType = "";

            var fmbFileName = "";
            var wpFileName = "";
            var opeFileName = "";
            var stgFileName = "";

            var fmbBlobData = await this.BlobService.GetBlobById(query2.Cms_FmbBlobId);
            var wpBlobData = await this.BlobService.GetBlobById(query2.Cms_WorkPrincipalBlobId);
            var stgBlobData = await this.BlobService.GetBlobById(query2.Cms_SettingBlobId);
            var opeBlobData = await this.BlobService.GetBlobById(query2.Cms_OperationBlobId);

            if (fmbBlobData != null)
            {
                fmbImageUrl = await this.FileService.GetFileAsync(fmbBlobData.BlobId.ToString(), fmbBlobData.Mime);
                fmbFileType = fmbBlobData.Mime;
                fmbFileName = fmbBlobData.Name;
            }

            if( wpBlobData != null)
            {
                wpImageUrl = await this.FileService.GetFileAsync(wpBlobData.BlobId.ToString(), wpBlobData.Mime);
                wpFileType = wpBlobData.Mime;
                wpFileName = wpBlobData.Name;
            }

            if(opeBlobData != null)
            {
                opeImageUrl = await this.FileService.GetFileAsync(opeBlobData.BlobId.ToString(), opeBlobData.Mime);
                opeFileType = opeBlobData.Mime;
                opeFileName = opeBlobData.Name;
            }

            if(stgBlobData != null)
            {
                stgImageUrl = await this.FileService.GetFileAsync(stgBlobData.BlobId.ToString(), stgBlobData.Mime);
                stgFileType = stgBlobData.Mime;
                stgFileName = stgBlobData.Name;
            }

            query2.Cms_FmbBlobUrl = fmbImageUrl;
            query2.Cms_FmbBlobFileType = fmbFileType;
            query2.Cms_FmbBlobFileName = fmbFileName;
            query2.Cms_WorkPrincipalBlobUrl = wpImageUrl;
            query2.Cms_WorkPrincipalFileType = wpFileType;
            query2.Cms_WorkPrincipalFileName = wpFileName;
            query2.Cms_OperationBlobUrl = opeImageUrl;
            query2.Cms_OperationFileType = opeFileType;
            query2.Cms_OperationFileName = opeFileName;
            query2.Cms_SettingBlobUrl = stgImageUrl;
            query2.Cms_SettingFileType = stgFileType;
            query2.Cms_SettingFileName = stgFileName;
            query2.Cms_ContentBlobImageUrl = imageURL;
            query2.Cms_ContentBlobImageFileName = imageFileName;
            query2.Cms_ContentVideoBlobImageUrl = videoUrl;
            query2.Cms_ContentVideoBlobImageFileName = videoFileName;
            query2.Cms_ContentFileBlobImageUrl = fileUrl;
            query2.Cms_ContentFileBlobImageFileName = fileFileName;

            return query2;
        }

    }
}
