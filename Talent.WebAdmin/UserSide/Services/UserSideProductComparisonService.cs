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
using static Talent.WebAdmin.UserSide.Enums.UserSideProductFeatureCategoryEnum;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductComparisonService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserSideProductComparisonService(TalentContext context, IFileStorageService fileService, BlobService blobService, ClaimHelper hm)
        {
            this.DB = context;
            this.HelperMan = hm;
            this.FileService = fileService;
            this.BlobService = blobService;
        }

        public async Task<UserSideBaseProductComparisonModel> GetUserSideBaseProduct(Guid productId, Guid productTypeId)
        {
            var query2 = await (from c in DB.Products
                                join d in DB.ProductTypes on c.ProductId equals d.ProductId
                                where c.IsDeleted == false && d.ProductId == productId && d.ProductTypeId == productTypeId && c.ApprovedAt != null
                                select new UserSideBaseProductComparisonModel
                                {
                                    ProductId = c.ProductId,
                                    ProductName = c.ProductName,
                                    ProductBlobId = c.BlobId,
                                    ProductTypeId = d.ProductTypeId,
                                    ProductTypeName = d.ProductTypeName
                                }).AsNoTracking().FirstOrDefaultAsync();

            var imageURL = "";
            var imageFileName = "";
            var blobData = await this.BlobService.GetBlobById(query2.ProductBlobId);

            if (blobData != null)
            {
                imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                imageFileName = blobData.Name;
                query2.ImageUrl = imageURL;
                query2.ImageFileName = imageFileName;
            }

            return query2;
        }

        public async Task<UserSideProductComparisonModel> GetUserSideProductComparison(Guid productId, Guid productTypeId)
        {
            var query2 = await (from c in DB.ProductCompetitorMappings
                                join d in DB.ProductTypes on c.ProductCompetitorTypeId equals d.ProductTypeId
                                join e in DB.Products on c.ProductCompetitorId equals e.ProductId
                                join f in DB.Blobs on e.BlobId equals f.BlobId
                                where c.IsDeleted == false && c.ProductId == productId && c.ProductTypeId == productTypeId && c.ApprovedAt != null
                                select new UserSideProductComparisonModel
                                {
                                    ProductCompetitorId = c.ProductCompetitorId,
                                    ProductCompetitorName = e.ProductName,
                                    ProductCompetitorBlobId = f.BlobId,
                                    ProductCompetitorTypeId = c.ProductCompetitorTypeId,
                                    ProductCompetitorTypeName = d.ProductTypeName
                                }).AsNoTracking().FirstOrDefaultAsync();

            var imageURL = "";
            var imageFileName = "";

            var blobData = await this.BlobService.GetBlobById(query2.ProductCompetitorBlobId);

            if (blobData != null)
            {
                imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                imageFileName = blobData.Name;
                query2.ImageUrl = imageURL;
                query2.ImageFileName = imageFileName;
            }

            return query2;
        }

        public async Task<List<UserSideBaseProductComparisonModel>> GetAllUserSideBaseChangeType(int pageSize, int pageIndex, Guid productId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var dataProducts = new List<UserSideBaseProductComparisonModel>();

            dataProducts = await (from pt in DB.ProductTypes
                                  join pr in DB.Products on pt.ProductId equals pr.ProductId
                                  where pt.IsDeleted == false && pt.ProductId == productId && pt.ApprovedAt != null
                                  select new UserSideBaseProductComparisonModel
                                  {
                                      ProductId = pt.ProductId,
                                      ProductName = pr.ProductName,
                                      ProductBlobId = pt.BlobId,
                                      ProductTypeId = pt.ProductTypeId,
                                      ProductTypeName = pt.ProductTypeName
                                  }).Skip((int)skipCount).Take(pageSize).ToListAsync();

            var index = 0;
            foreach (var datum in dataProducts)
            {

                var imageURL = "";
                var imageFileName = "";

                var blobData = await this.BlobService.GetBlobById(datum.ProductBlobId);
                if (blobData != null)
                {
                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                    imageFileName = blobData.Name;
                    dataProducts[index].ImageUrl = imageURL;
                    dataProducts[index].ImageFileName = imageFileName;
                }
                index++;
            }

            return dataProducts;
        }

        public async Task<List<UserSideBaseProductComparisonModel>> GetAllUserSideCompetitorSameBrandProducts(int pageSize, int pageIndex, Guid productTypeId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var dataProducts = new List<UserSideBaseProductComparisonModel>();

            dataProducts = await (from pr in DB.Products
                                  join pt in DB.ProductTypes on pr.ProductId equals pt.ProductId
                                  where pr.IsDeleted == false && pt.IsDeleted == false && pt.ProductTypeId != productTypeId && pr.IsCompetitor == false && pr.ApprovedAt != null
                                  select new UserSideBaseProductComparisonModel
                                  {
                                      ProductId = pr.ProductId,
                                      ProductName = pr.ProductName,
                                      ProductBlobId = pt.BlobId,
                                      ProductTypeId = pt.ProductTypeId,
                                      ProductTypeName = pt.ProductTypeName
                                  }).Skip((int)skipCount).Take(pageSize).ToListAsync();

            var index = 0;
            foreach (var datum in dataProducts)
            {

                var imageURL = "";
                var imageFileName = "";
                var blobData = await this.BlobService.GetBlobById(datum.ProductBlobId);
                if (blobData != null)
                {
                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                    imageFileName = blobData.Name;
                    dataProducts[index].ImageUrl = imageURL;
                    dataProducts[index].ImageFileName = imageFileName;
                }
                index++;
            }

            return dataProducts;
        }

        public async Task<List<UserSideProductComparisonModel>> GetAllUserSideCompetitorBrandProducts(int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var dataCompetitorProducts = new List<UserSideProductComparisonModel>();

            dataCompetitorProducts = await (from d in DB.ProductTypes
                                            join e in DB.Products on d.ProductId equals e.ProductId
                                            join f in DB.Blobs on d.BlobId equals f.BlobId
                                            where e.IsDeleted == false && d.IsDeleted == false && e.IsCompetitor == true && d.ApprovedAt != null && e.ApprovedAt != null
                                            select new UserSideProductComparisonModel
                                            {
                                                ProductCompetitorId = d.ProductId,
                                                ProductCompetitorName = e.ProductName,
                                                ProductCompetitorBlobId = f.BlobId,
                                                ProductCompetitorTypeId = d.ProductTypeId,
                                                ProductCompetitorTypeName = d.ProductTypeName
                                            }).Skip((int)skipCount).Take(pageSize).ToListAsync();

            var index = 0;
            foreach (var datum in dataCompetitorProducts)
            {
                var imageURL = "";
                var imageFileName = "";
                var blobData = await this.BlobService.GetBlobById(datum.ProductCompetitorBlobId);
                if (blobData != null)
                {
                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                    imageFileName = blobData.Name;
                    dataCompetitorProducts[index].ImageUrl = imageURL;
                    dataCompetitorProducts[index].ImageFileName = imageFileName;
                }
                index++;
            }

            return dataCompetitorProducts;
        }

        public async Task<List<UserSideProductFeatureNameModel>> GetAllUserSideFeatureMapping(Guid productId, Guid productTypeId, Guid productCompetitorId, Guid productCompetitorTypeId)
        {
            //get interior exterior
            var enumData = await (from pfc in DB.ProductFeatureCategories
                                  where pfc.IsDeleted == false
                                  select new UserSideProductFeatureCategoryFilter
                                  {
                                      ProductFeatureCategoryId = pfc.ProductFeatureCategoryId,
                                      ProductFeatureCategoryName = pfc.ProductFeatureCategoryName
                                  }).ToListAsync();

            //header list feature
            var productFeatureList = new List<UserSideProductFeatureNameModel>();

            foreach (var enumVar in enumData) //di loop by interior exterior
            {
                var productFeature = new UserSideProductFeatureNameModel(); //object list feature

                var listTotalFeature = new List<UserSideProductFeatureModel>();

                var query1 = await (from pf in DB.ProductFeatures
                                    join pfm in DB.ProductFeatureMappings on pf.ProductFeatureId equals pfm.ProductFeatureId
                                    where pf.IsDeleted == false && pfm.ProductFeatureCategoryId == enumVar.ProductFeatureCategoryId && pfm.ProductFeatureMappingApprovalStatus == "Published" && pfm.ApprovedAt != null && pfm.ProductId == productId && pfm.ProductTypeId == productTypeId
                                    select new UserSideProductFeatureModel
                                    {
                                        ProductFeatureId = pf.ProductFeatureId,
                                        ProductFeatureName = pf.ProductFeatureName,
                                        BlobFeatureId = pf.BlobId
                                    }).ToListAsync();

                listTotalFeature.AddRange(query1);

                var query2 = await (from pf in DB.ProductFeatures
                                    join pfm in DB.ProductFeatureMappings on pf.ProductFeatureId equals pfm.ProductFeatureId
                                    where pf.IsDeleted == false && pfm.ProductFeatureCategoryId == enumVar.ProductFeatureCategoryId && pfm.ProductFeatureMappingApprovalStatus == "Published" && pfm.ApprovedAt != null && pfm.ProductId == productCompetitorId && pfm.ProductTypeId == productCompetitorTypeId
                                    select new UserSideProductFeatureModel
                                    {
                                        ProductFeatureId = pf.ProductFeatureId,
                                        ProductFeatureName = pf.ProductFeatureName,
                                        BlobFeatureId = pf.BlobId
                                    }).ToListAsync();

                listTotalFeature.AddRange(query2);

                var bar = listTotalFeature.GroupBy(x => x.ProductFeatureId).Select(x => x.First()).ToList();

                var contentFeatureList = new List<UserSideProductFeatureMappingModel>();
                //var contentFeatureCompList = new List<List<UserSideProductFeatureMappingModel>>();
                var contentFeatureCompList = new List<UserSideProductFeatureContentModelView>();
                if (bar.Count() > 0)
                {
                    var modelIndex = 0;
                    foreach (var featureData in bar)
                    {
                        var listCompModel = new List<UserSideProductFeatureContentModelView>();


                        var query = await (from pfm in DB.ProductFeatureMappings
                                           join cms in DB.Cms_Contents on pfm.Cms_ContentId equals cms.Cms_ContentId
                                           where pfm.IsDeleted == false && pfm.ProductId == productId && pfm.ProductTypeId == productTypeId && pfm.ProductFeatureCategoryId == enumVar.ProductFeatureCategoryId && pfm.ProductFeatureId == featureData.ProductFeatureId && pfm.ApprovedAt != null && pfm.ProductFeatureMappingApprovalStatus == "Published"
                                           select new UserSideProductFeatureMappingModel
                                           {
                                               ProductFeatureMappingId = pfm.ProductFeatureMappingId,
                                               Cms_ContentId = pfm.Cms_ContentId.HasValue ? pfm.Cms_ContentId.Value : Guid.Empty,
                                               Cms_ContentName = cms.Cms_ContentDescription,
                                               Cms_ContentBlobId = cms.BlobId.HasValue ? cms.BlobId.Value : Guid.Empty
                                           }).ToListAsync();

                        if (query.Count() > 0)
                        {
                            var cmpModelObj = new UserSideProductFeatureContentModelView();
                            //var index = 0;
                            //foreach (var datum in query)
                            //{

                            //    var imageURL = "";
                            //    var imageFileName = "";
                            //    var blobData = await this.BlobService.GetBlobById(datum.Cms_ContentBlobId);
                            //    if (blobData != null)
                            //    {
                            //        imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                            //        imageFileName = blobData.Name;
                            //        query[index].Cms_ContentImageUrl = imageURL;
                            //        query[index].Cms_ContentImageFileName = imageFileName;
                            //    }
                            //    index++;
                            //}
                            var listCompModelObj = new List<UserSideProductFeatureContentImageModelView>();
                            var listCompDescModelObj = new List<UserSideProductFeatureContentDescModelView>();
                            foreach (var compModelV in query)
                            {
                                var blobData = await this.BlobService.GetBlobById(compModelV.Cms_ContentBlobId);
                                if (blobData != null)
                                {
                                    var compModelObj = new UserSideProductFeatureContentImageModelView();
                                    compModelObj.ProductFeatureCmsContentBlob = blobData.BlobId;
                                    compModelObj.ProductFeatureCmsContentBlobUrl = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                                    compModelObj.ProductFeatureCmsContentBlobFileName = blobData.Name;
                                    compModelObj.ProductFeatureCmcContentBlobFileType = blobData.Mime;
                                    listCompModelObj.Add(compModelObj);
                                }
                                var compModelDescObj = new UserSideProductFeatureContentDescModelView();
                                compModelDescObj.ProductFeatureCmsContentDesc = compModelV.Cms_ContentName;
                                listCompDescModelObj.Add(compModelDescObj);
                                cmpModelObj.ContentDescView = listCompDescModelObj;
                                cmpModelObj.ContentImageView = listCompModelObj;
                            }
                            listCompModel.Add(cmpModelObj);
                            //contentFeatureCompList.Add(queryComp);
                            //contentFeatureCompList.AddRange(listCompModel);
                            //query2[modelIndex].ProductFeatureMappingCompModels = cmpModelObj;
                            contentFeatureCompList.AddRange(listCompModel);
                            bar[modelIndex].ProductFeatureMappingModels = cmpModelObj;
                            //productFeatureMappingObj.ProductFeatureMappingModels = contentFeatureList;
                        }

                        var queryComp = await (from pfm in DB.ProductFeatureMappings
                                               join cms in DB.Cms_Contents on pfm.Cms_ContentId equals cms.Cms_ContentId
                                               where pfm.IsDeleted == false && pfm.ProductId == productCompetitorId && pfm.ProductTypeId == productCompetitorTypeId && pfm.ProductFeatureCategoryId == enumVar.ProductFeatureCategoryId && pfm.ProductFeatureId == featureData.ProductFeatureId && pfm.ApprovedAt != null && pfm.ProductFeatureMappingApprovalStatus == "Published"
                                               select new UserSideProductFeatureMappingModel
                                               {
                                                   ProductFeatureMappingId = pfm.ProductFeatureMappingId,
                                                   Cms_ContentId = pfm.Cms_ContentId.HasValue ? pfm.Cms_ContentId.Value : Guid.Empty,
                                                   Cms_ContentName = cms.Cms_ContentDescription,
                                                   Cms_ContentBlobId = cms.BlobId.HasValue ? cms.BlobId.Value : Guid.Empty
                                               }).ToListAsync();

                        if (queryComp.Count() > 0)
                        {
                            var cmpModelObj = new UserSideProductFeatureContentModelView();
                            //var indexComp = 0;
                            //foreach (var datum in queryComp)
                            //{
                            //    var imageURL = "";
                            //    var imageFileName = "";
                            //    var blobData = await this.BlobService.GetBlobById(datum.Cms_ContentBlobId);
                            //    if (blobData != null)
                            //    {
                            //        imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                            //        imageFileName = blobData.Name;
                            //        queryComp[indexComp].Cms_ContentImageUrl = imageURL;
                            //        queryComp[indexComp].Cms_ContentImageFileName = imageFileName;
                            //    }
                            //    indexComp++;
                            //}
                            var listCompModelObj = new List<UserSideProductFeatureContentImageModelView>();
                            var listCompDescModelObj = new List<UserSideProductFeatureContentDescModelView>();
                            foreach (var compModelV in queryComp)
                            {
                                var blobData = await this.BlobService.GetBlobById(compModelV.Cms_ContentBlobId);
                                if (blobData != null)
                                {
                                    var compModelObj = new UserSideProductFeatureContentImageModelView();
                                    compModelObj.ProductFeatureCmsContentBlob = blobData.BlobId;
                                    compModelObj.ProductFeatureCmsContentBlobUrl = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                                    compModelObj.ProductFeatureCmsContentBlobFileName = blobData.Name;
                                    compModelObj.ProductFeatureCmcContentBlobFileType = blobData.Mime;
                                    listCompModelObj.Add(compModelObj);
                                }
                                var compModelDescObj = new UserSideProductFeatureContentDescModelView();
                                compModelDescObj.ProductFeatureCmsContentDesc = compModelV.Cms_ContentName;
                                listCompDescModelObj.Add(compModelDescObj);
                                cmpModelObj.ContentDescView = listCompDescModelObj;
                                cmpModelObj.ContentImageView = listCompModelObj;
                            }
                            listCompModel.Add(cmpModelObj);
                            //contentFeatureCompList.Add(queryComp);
                            contentFeatureCompList.AddRange(listCompModel);
                            bar[modelIndex].ProductFeatureMappingCompModels = cmpModelObj;
                            //productFeatureMappingObj.ProductFeatureMappingCompModels = contentFeatureCompList;
                        }

                        //if(query2.Count() > 0 || queryComp.Count() > 0)
                        //{
                        //    productFeatureMappingModel.Add(productFeatureMappingObj);
                        //    query2[modelIndex].ProductFeatureMappingModel = productFeatureMappingModel;
                        //}
                        modelIndex++;
                    }
                    var indexblob = 0;
                    foreach (var datum in listTotalFeature)
                    {
                        var imageURL = "";
                        var imageFileName = "";
                        var imageFileType = "";
                        var blobData = await this.BlobService.GetBlobById(datum.BlobFeatureId);
                        if (blobData != null)
                        {
                            imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                            imageFileName = blobData.Name;
                            imageFileType = blobData.Mime;
                            listTotalFeature[indexblob].BlobFeatureUrl = imageURL;
                            listTotalFeature[indexblob].BlobFeatureFileName = imageFileName;
                            listTotalFeature[indexblob].BlobFeatureFileType = imageFileType;
                        }
                        indexblob++;
                    }
                }

                productFeature.ProductFeatureCategoryId = enumVar.ProductFeatureCategoryId;
                productFeature.ProductFeatureCategoryName = enumVar.ProductFeatureCategoryName;
                productFeature.ProductFeatureModels = bar;


                //productFeature.ProductFeatureMappingModels = contentFeatureList;
                //productFeature.ProductFeatureMappingCompModels = contentFeatureCompList;

                productFeatureList.Add(productFeature);


            }

            return productFeatureList;
        }

        public async Task ContributeUpdatePhotoFeatureAsync(UserSideProductFeatureComparisonContributeModel model, string employeeId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var getUploadModuleBlob = await this.FileService.UploadFileFromBase64(model.ProductGalleryFileContent);

            if (model.ProductGalleryFileContent != null)
            {
                if (string.IsNullOrEmpty(model.ProductGalleryFileContent.Base64) == false)
                {
                    getUploadModuleBlob = await this.FileService.UploadFileFromBase64(model.ProductGalleryFileContent);
                }
            }
            var newCms_Contents = new Cms_Contents
            {
                BlobId = getUploadModuleBlob,
                CmsContentFileBlobId = null,
                CmsContentVideoBlobId = null,
                Cms_ContentName = "Content",
                Cms_ContentDescription = "",
                Cms_ContentCategory = "Feature",
                Cms_ContentSequence = 0,
                Cms_ContentIsSequence = true,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = employeeId,
                UpdatedBy = employeeId
            };

            this.DB.Cms_Contents.Add(newCms_Contents);

            var CmsContentId = newCms_Contents.Cms_ContentId;

            var newProductFeatureMappings = new ProductFeatureMappings
            {
                ProductFeatureId = model.ProductFeatureId,
                ProductFeatureCategoryId = model.ProductFeatureCategoryId,
                Cms_ContentId = CmsContentId,
                ProductId = model.ProductId,
                ProductTypeId = model.ProductTypeId,
                Cms_FmbId = null,
                Cms_WorkPrincipalId = null,
                Cms_OperationId = null,
                Cms_SettingId = null,
                IsSpecial = false,
                ProductFeatureMappingApprovalStatus = "Draft",
                EnumContributorFlagging = "FromUser",
                ApprovedAt = model.ApprovedAt,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = employeeId,
                UpdatedBy = employeeId
            };

            this.DB.ProductFeatureMappings.Add(newProductFeatureMappings);

            await this.DB.SaveChangesAsync();
        }
    }
}
