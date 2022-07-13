using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class BannerService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper ClaimMan;
        private readonly FileService FileServiceMan;
        private readonly ApprovalService ApprovalMan;


        public BannerService(TalentContext talentContext, IFileStorageService fileService, ClaimHelper claimHelper, FileService fs, ApprovalService approvalService)
        {
            this.DB = talentContext;
            this.FileMan = fileService;
            this.ClaimMan = claimHelper;
            this.FileServiceMan = fs;
            this.ApprovalMan = approvalService;
        }

        public async Task<BannerViewModel> GetBannerFiltered(BannerFilterModel filter)
        {
            var query = (from b in DB.Banners
                         join bt in DB.BannerTypes on b.BannerTypeId equals bt.BannerTypeId

                         join atb in DB.ApprovalToBanners on b.BannerId equals atb.BannerId into ljb
                         from atb in ljb.DefaultIfEmpty()

                         join a in DB.Approvals on atb.ApprovalId equals a.ApprovalId into ljap
                         from a in ljap.DefaultIfEmpty()

                         join asn in DB.ApprovalStatus on a.ApprovalStatusId equals asn.ApprovalStatusId into ljasn
                         from asn in ljasn.DefaultIfEmpty()

                         join bl in DB.Blobs on b.BlobId equals bl.BlobId

                         join mb in DB.MobilePages on b.MobilePageId equals mb.MobilePageId into ljmp
                         from mb in ljmp.DefaultIfEmpty()

                         join e in DB.Employees on b.CreatedBy equals e.EmployeeId into le
                         from e in le.DefaultIfEmpty()

                         where b.IsDeleted == false

                         select new BannerModel
                         {
                             BannerId = b.BannerId,
                             BannerTypeId = b.BannerTypeId,
                             BannerTypeName = bt.Name,
                             BlobId = b.BlobId,
                             Name = b.Name,
                             CreatedAt = b.CreatedAt,
                             CreatedBy = e.EmployeeName,
                             Description = b.Description,
                             EndDate = b.EndDate,
                             StartDate = b.StartDate,
                             UpdatedAt = b.UpdatedAt,
                             ApprovalStatusId = a.ApprovalStatusId,
                             ActionBy = a.ActionBy == null ? "" : a.ActionBy,
                             ApprovalStatus = ((int?)a.ApprovalStatusId).HasValue ? asn.ApprovalName : (b.ApprovedAt == DateTime.MinValue ? "" : ApprovalStatusesEnum.Approve),
                             ActionAt = a.ActionAt.GetValueOrDefault(),
                             ContentId = b.ContentId,
                             MobilePageId = b.MobilePageId,
                             Mime = bl.Mime,
                             BlobName = bl.Name,
                             PageName = mb.Name != null ? mb.Name : ""

                         });

            if (filter.DateFilterStart != null && filter.DateFilterEnd != null)
            {
                var newStartDate = filter.DateFilterStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateFilterEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (filter.StartDate != null)
            {

                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var date = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0).Date;

                query = query.Where(Q => Q.StartDate.Value.Date == date);
            }

            if (filter.EndDate != null)
            {
                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var date = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 0, 0, 0).Date;

                query = query.Where(Q => Q.EndDate.Value.Date == date);
            }

            if (string.IsNullOrEmpty(filter.ActionBy) == false)
            {
                query = query.Where(Q => Q.CreatedBy.ToLower().Contains(filter.ActionBy.ToLower()));
            }

            if (string.IsNullOrEmpty(filter.Name) == false)
            {
                query = query.Where(Q => Q.Name.Contains(filter.Name));
            }

            if (filter.BannerTypeId != null)
            {
                query = query.Where(Q => Q.BannerTypeId == filter.BannerTypeId);
            }
            if (string.IsNullOrEmpty(filter.ApprovalStatus) == false)
            {
                query = query.Where(Q => Q.ApprovalStatus == filter.ApprovalStatus);
            }

            switch (filter.SortBy)
            {
                case "bannerName":
                    query = query.OrderBy(Q => Q.Name);
                    break;
                case "bannerNameDesc":
                    query = query.OrderByDescending(Q => Q.Name);
                    break;
                case "bannerType":
                    query = query.OrderBy(Q => Q.BannerTypeId);
                    break;
                case "bannerTypeDesc":
                    query = query.OrderByDescending(Q => Q.BannerTypeId);
                    break;
                case "startDate":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;
                case "endDate":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;
                case "status":
                    query = query.OrderBy(Q => Q.ApprovalStatus);
                    break;
                case "statusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus);
                    break;
                case "approvedBy":
                    query = query.OrderBy(Q => Q.CreatedBy);
                    break;
                case "approvedByDesc":
                    query = query.OrderByDescending(Q => Q.CreatedBy);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case "":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case null:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.CountAsync();

            var resultQuery = await query.Select(Q => new BannerModel
            {
                ActionBy = Q.ActionBy,
                BannerId = Q.BannerId,
                BannerTypeId = Q.BannerTypeId,
                BannerTypeName = Q.BannerTypeName,
                BlobId = Q.BlobId,
                ActionAt = Q.ActionAt,
                CreatedAt = Q.CreatedAt,
                CreatedBy = Q.CreatedBy,
                Description = Q.Description,
                EndDate = Q.EndDate,
                Name = Q.Name,
                MobilePageId = Q.MobilePageId,
                ApprovalStatus = Q.ApprovalStatus,
                ApprovalStatusId = Q.ApprovalStatusId,
                ContentId = Q.ContentId,
                StartDate = Q.StartDate,
                UpdatedAt = Q.UpdatedAt,
                Mime = Q.Mime,
                BlobName = Q.BlobName,
                PageName = Q.PageName
            }).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();


            var bannerList = new BannerViewModel();
            bannerList.ListBanner = resultQuery;
            bannerList.TotalData = totalData;

            return bannerList;

        }

        public async Task<BannerViewModel> GetBannerByIdFromOutside(int id)
        {
            var query = (from b in DB.Banners
                         join bt in DB.BannerTypes on b.BannerTypeId equals bt.BannerTypeId

                         join atb in DB.ApprovalToBanners on b.BannerId equals atb.BannerId into ljb
                         from atb in ljb.DefaultIfEmpty()

                         join a in DB.Approvals on atb.ApprovalId equals a.ApprovalId into ljap
                         from a in ljap.DefaultIfEmpty()

                         join asn in DB.ApprovalStatus on a.ApprovalStatusId equals asn.ApprovalStatusId into ljasn
                         from asn in ljasn.DefaultIfEmpty()

                         join bl in DB.Blobs on b.BlobId equals bl.BlobId

                         join mb in DB.MobilePages on b.MobilePageId equals mb.MobilePageId into ljmp
                         from mb in ljmp.DefaultIfEmpty()

                         where b.BannerId == id

                         select new BannerModel
                         {
                             BannerId = b.BannerId,
                             BannerTypeId = b.BannerTypeId,
                             BannerTypeName = bt.Name,
                             BlobId = b.BlobId,
                             Name = b.Name,
                             CreatedAt = b.CreatedAt,
                             CreatedBy = b.CreatedBy,
                             Description = b.Description,
                             EndDate = b.EndDate,
                             StartDate = b.StartDate,
                             UpdatedAt = b.UpdatedAt,
                             ApprovalStatusId = a.ApprovalStatusId,
                             ActionBy = a.ActionBy == null ? "" : a.ActionBy,
                             ApprovalStatus = ((int?)a.ApprovalStatusId).HasValue ? asn.ApprovalName : (b.ApprovedAt == DateTime.MinValue ? "" : ApprovalStatusesEnum.Approve),
                             ActionAt = a.ActionAt.GetValueOrDefault(),
                             ContentId = b.ContentId,
                             MobilePageId = b.MobilePageId,
                             Mime = bl.Mime,
                             BlobName = bl.Name,
                             PageName = mb.Name != null ? mb.Name : ""

                         });

            var totalData = await query.CountAsync();

            var resultQuery = await query.Select(Q => new BannerModel
            {
                ActionBy = Q.ActionBy,
                BannerId = Q.BannerId,
                BannerTypeId = Q.BannerTypeId,
                BannerTypeName = Q.BannerTypeName,
                BlobId = Q.BlobId,
                ActionAt = Q.ActionAt,
                CreatedAt = Q.CreatedAt,
                CreatedBy = Q.CreatedBy,
                Description = Q.Description,
                EndDate = Q.EndDate,
                Name = Q.Name,
                MobilePageId = Q.MobilePageId,
                ApprovalStatus = Q.ApprovalStatus,
                ApprovalStatusId = Q.ApprovalStatusId,
                ContentId = Q.ContentId,
                StartDate = Q.StartDate,
                UpdatedAt = Q.UpdatedAt,
                Mime = Q.Mime,
                BlobName = Q.BlobName,
                PageName = Q.PageName
            }).ToListAsync();


            var bannerList = new BannerViewModel();
            bannerList.ListBanner = resultQuery;
            bannerList.TotalData = totalData;

            return bannerList;

        }


        public async Task<bool> InsertBanner(BannerFormModel model)
        {
            try
            {
                var userLogin = this.ClaimMan.GetLoginUserId();

                var newBanner = new Banners
                {
                    //BlobId = model.BlobId.GetValueOrDefault(),
                    BannerTypeId = model.BannerTypeId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = userLogin,
                    UpdatedBy = userLogin,
                    Description = model.Description,
                    EndDate = model.EndDate == null ? (DateTime?)null : model.EndDate.GetValueOrDefault().ToIndonesiaTimeZone(),
                    Name = model.Name,
                    StartDate = model.StartDate == null ? (DateTime?)null : model.StartDate.GetValueOrDefault().ToIndonesiaTimeZone(),
                    ApprovedAt = null,
                    MobilePageId = model.MobilePageId,
                    ContentId = model.ContentId,
                };

                if (model.ImageUpload != null)
                {
                    if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);
                        newBanner.BlobId = blobId;
                    }
                }

                this.DB.Banners.Add(newBanner);

                if (model.InsertType == 2 && model.BannerTypeId == 1)
                {
                    var newApproval = new Approvals
                    {
                        ContentName = newBanner.Name,
                        ApprovalLevel = 0,
                        ApprovalStatusId = 3,
                        CreatedBy = userLogin,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        ContentCategory = ContentCategoryEnums.Banner,
                    };

                    var newApprovalBannerMapping = new ApprovalToBanners();

                    newApproval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                    newApproval.ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    newApproval.ActionBy = userLogin;

                    this.DB.Approvals.Add(newApproval);

                    newApprovalBannerMapping.ApprovalId = newApproval.ApprovalId;
                    newApprovalBannerMapping.BannerId = newBanner.BannerId;

                    this.DB.ApprovalToBanners.Add(newApprovalBannerMapping);

                    newBanner.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                }
                else
                {
                    var newApproval = new ApprovalCreateFormModel
                    {
                        ApprovalStatusId = model.InsertType == 1 ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                        ContentCategory = ContentCategoryEnums.Banner,
                        ContentId = newBanner.BannerId,
                        ContentName = newBanner.Name,
                        PageEnum = PageEnums.Banner
                    };

                    var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

                    if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                    {
                        newBanner.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }

                await this.DB.SaveChangesAsync();

                return true;

            }
            catch (Exception e)
            {
                var error = e.Data;
                var deleteBlob = await this.DB.Blobs.Where(Q => Q.BlobId == model.BlobId).FirstOrDefaultAsync();

                if (deleteBlob != null)
                {
                    await this.FileMan.DeleteFileAsync(deleteBlob.BlobId, deleteBlob.Mime);
                    this.DB.Remove(deleteBlob);
                    await this.DB.SaveChangesAsync();
                }

                return false;
            }
        }

        public async Task<bool> DeleteBanner(int id)
        {
            var deleteBanner = await this.DB.Banners.Where(Q => Q.BannerId == id).FirstOrDefaultAsync();

            if (deleteBanner == null)
            {
                return false;
            }

            var deleteApprovalToBanner = await this.DB.ApprovalToBanners.Where(Q => Q.BannerId == id).FirstOrDefaultAsync();

            this.DB.ApprovalToBanners.Remove(deleteApprovalToBanner);

            var isDeleted = await this.ApprovalMan.DeleteApproval(deleteApprovalToBanner.ApprovalId);

            if (isDeleted == false)
            {
                return false;
            }

            deleteBanner.IsDeleted = true;
            deleteBanner.ApprovedAt = null;

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<BannerFormModel> GetBannerById(int id)
        {
            var findBanner = await this.DB.Banners.Where(Q => Q.BannerId == id).Select(Q => new BannerFormModel
            {
                BannerId = Q.BannerId,
                BannerTypeId = Q.BannerTypeId,
                BlobId = Q.BlobId,
                CreatedAt = Q.CreatedAt,
                CreatedBy = Q.CreatedBy,
                Description = Q.Description,
                EndDate = Q.EndDate,
                Name = Q.Name,
                StartDate = Q.StartDate,
                UpdatedAt = Q.UpdatedAt,
                ContentId = Q.ContentId,
                MobilePageId = Q.MobilePageId
            }).AsNoTracking().FirstOrDefaultAsync();

            if (findBanner == null)
            {
                findBanner = new BannerFormModel();
            }

            return findBanner;
        }

        public async Task<bool> UpdateBanner(BannerFormModel model)
        {

            var findBanner = await this.DB.Banners.Where(Q => Q.BannerId == model.BannerId).FirstOrDefaultAsync();

            try
            {
                if (findBanner == null)
                {
                    return false;
                }

                Blobs blobs = null;

                if (findBanner.BlobId != null)
                {
                    blobs = await this.DB.Blobs.Where(Q => Q.BlobId == findBanner.BlobId).FirstOrDefaultAsync();
                }

                var userLogIn = this.ClaimMan.GetLoginUserId();

                var approvalMappingId = await this.DB.ApprovalMappings.Where(Q => Q.PageId == PageEnums.Banner).Select(Q => Q.ApprovalMappingid).FirstOrDefaultAsync();

                var findBannerApproval = await (from b in this.DB.Banners
                                                join atb in this.DB.ApprovalToBanners on b.BannerId equals atb.BannerId
                                                join a in this.DB.Approvals on atb.ApprovalId equals a.ApprovalId
                                                where b.BannerId == model.BannerId
                                                select new
                                                {
                                                    BannerId = b.BannerId,
                                                    ApprovalStatusId = a.ApprovalStatusId,
                                                    ApprovalId = a.ApprovalId
                                                }
                                   ).FirstOrDefaultAsync();

                var findApproval = await this.DB.Approvals.Where(Q => Q.ApprovalId == findBannerApproval.ApprovalId).FirstOrDefaultAsync();

                //Waiting for Approval, can't be edited!
                if (findBannerApproval.ApprovalStatusId == 2)
                {
                    return false;
                }

                if (model.BlobId != findBanner.BlobId)
                {
                    //Delete Image blob dan update
                    var findBlob = await this.DB.Blobs.Where(Q => Q.BlobId == findBanner.BlobId).FirstOrDefaultAsync();
                    await FileMan.DeleteFileAsync(findBlob.BlobId, findBlob.Mime);
                    this.DB.Blobs.Remove(findBlob);
                }

                findBanner.BannerTypeId = model.BannerTypeId;
                //findBanner.BlobId = model.BlobId.GetValueOrDefault();
                findBanner.MobilePageId = model.MobilePageId;
                findBanner.ContentId = model.ContentId;
                findBanner.Description = model.Description;
                findBanner.EndDate = model.EndDate == null ? (DateTime?)null : model.EndDate.GetValueOrDefault().ToIndonesiaTimeZone();
                findBanner.Name = model.Name;
                findBanner.StartDate = model.StartDate == null ? (DateTime?)null : model.StartDate.GetValueOrDefault().ToIndonesiaTimeZone();
                findBanner.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                findBanner.UpdatedBy = userLogIn;

                if (model.ImageUpload != null)
                {
                    if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                    {
                        var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);

                        if (blobs != null)
                        {
                            await FileServiceMan.DeleteFileAsync(blobs.BlobId, blobs.Mime);
                        }

                        findBanner.BlobId = blobId;
                    }
                }

                if (model.InsertType == 2 && model.BannerTypeId == 1)
                {
                    //Update approval to Approve
                    findApproval.ContentName = model.Name;
                    findApproval.ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    findApproval.ActionBy = userLogIn;
                    findApproval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;

                    findBanner.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                }
                else
                {
                    var updateApproval = new ApprovalUpdateFormModel
                    {
                        ApprovalId = findApproval.ApprovalId,
                        ApprovalStatusId = model.InsertType == 1 ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                        ContentName = model.Name,
                        PageEnum = PageEnums.Banner,
                        ContentId = findBanner.BannerId,
                        ContentCategory = ContentCategoryEnums.Banner
                    };

                    var approvals = await this.ApprovalMan.UpdateNewApproval(updateApproval);

                    if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                    {
                        findBanner.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                    else
                    {
                        findBanner.ApprovedAt = null;
                    }
                }

                await this.DB.SaveChangesAsync();
            }
            catch
            {
                if (model.BlobId != findBanner.BlobId)
                {
                    var deleteBlob = await this.DB.Blobs.Where(Q => Q.BlobId == model.BlobId).FirstOrDefaultAsync();

                    if (deleteBlob != null)
                    {
                        await this.FileMan.DeleteFileAsync(deleteBlob.BlobId, deleteBlob.Mime);
                        this.DB.Remove(deleteBlob);
                        await this.DB.SaveChangesAsync();
                    }
                }
                return false;

            }
            return true;
        }

        public async Task<List<MobileMappingPage>> GetAllContentType()
        {
            var allContentType = await this.DB.MobilePages.Where(Q => Q.IsForBanner == true).Select(Q => new MobileMappingPage
            {
                MobilePageId = Q.MobilePageId,
                Name = Q.Name,
            }).ToListAsync();


            if (allContentType == null)
            {
                allContentType = new List<MobileMappingPage>();
            }

            return allContentType;

        }

        public async Task<List<ContentModel>> SearchContent(ContentFilterModel model)
        {
            var time = DateTime.UtcNow.Date;
            var result = new List<ContentModel>();

            switch (model.TypeContent)
            {
                case MobilePageEnum.Reward:
                    //Rewards
                    result = await this.DB.Rewards.Where(Q => Q.Name.ToLower().Contains(model.Name.ToLower()) && Q.IsActive == true && (Q.StartDate == null || (time >= Q.StartDate.Value.Date && time <= Q.EndDate.Value.Date))).Select(Q => new ContentModel
                    {
                        Name = Q.Name,
                        Id = Q.RewardId
                    }).Take(10).ToListAsync();

                    break;

                case MobilePageEnum.EventDetail:
                    //Events
                    result = await this.DB.Events.Where(Q => Q.Name.ToLower().Contains(model.Name.ToLower()) && Q.IsDeleted == false && Q.ApprovedAt != null && (Q.StartDateTime == null || (time >= Q.StartDateTime.Date && time <= Q.EndDateTime.Date))).Select(Q => new ContentModel
                    {
                        Name = Q.Name,
                        Id = Q.EventId
                    }).Take(10).ToListAsync();

                    break;

                //case 4:
                //    //Banner Detail
                //    result = await this.DB.Banners.Where(Q => Q.Name.ToLower().Contains(model.Name.ToLower()) && Q.ApprovedAt != null && Q.IsDeleted == false).Select(Q => new ContentModel
                //    {
                //        Name = Q.Name,
                //        Id = Q.BannerId
                //    }).Take(10).ToListAsync();

                //    break;

                case MobilePageEnum.NewsDetail:
                    //News
                    result = await this.DB.News.Where(Q => Q.Title.ToLower().Contains(model.Name.ToLower()) && Q.ApprovedAt != null && Q.IsDeleted == false).Select(Q => new ContentModel
                    {
                        Name = Q.Title,
                        Id = Q.NewsId
                    }).Take(10).ToListAsync();

                    break;

                case MobilePageEnum.Guide:
                    //Guide
                    result = await this.DB.Guides.Where(Q => Q.Name.ToLower().Contains(model.Name.ToLower()) && Q.ApprovedAt != null).Select(Q => new ContentModel
                    {
                        Name = Q.Name,
                        Id = Q.GuideId
                    }).Take(10).ToListAsync();

                    break;

                case MobilePageEnum.Coach:
                    //Coaches
                    result = await (from c in this.DB.Coaches
                                    join emp in this.DB.Employees on c.EmployeeId equals emp.EmployeeId
                                    where emp.EmployeeName.ToLower().Contains(model.Name.ToLower())
                                    select new ContentModel
                                    {
                                        Id = c.CoachId,
                                        Name = emp.EmployeeName
                                    }).Take(10).ToListAsync();
                    break;
                case MobilePageEnum.Insight:
                    //Surveys
                    result = await this.DB.Surveys.Where(Q => Q.Title.ToLower().Contains(model.Name.ToLower()) && Q.IsDeleted == false && Q.ApprovedAt != null && (Q.StartDate == null || (time >= Q.StartDate.Value.Date && time <= Q.EndDate.Value.Date))).Select(Q => new ContentModel
                    {
                        Name = Q.Title,
                        Id = Q.SurveyId
                    }).Take(10).ToListAsync();

                    break;

                case MobilePageEnum.LearningDetail:
                    //Release Training
                    result = await this.DB.SetupLearning.Where(Q => Q.LearningName.ToLower().Contains(model.Name.ToLower()) && Q.ApprovedAt != null).Select(Q => new ContentModel
                    {
                        Name = Q.LearningName,
                        Id = Q.SetupLearningId
                    }).Take(10).ToListAsync();
                    break;
            }

            return result;
        }

        public async Task<ContentModel> SearchContentWithId(ContentWithIdFilterModel model)
        {

            var result = new ContentModel();

            switch (model.TypeContent)
            {
                case MobilePageEnum.Reward:
                    //Rewards
                    result = await this.DB.Rewards.Where(Q => Q.RewardId == model.ContentId).Select(Q => new ContentModel
                    {
                        Name = Q.Name,
                        Id = Q.RewardId
                    }).FirstOrDefaultAsync();

                    break;

                case MobilePageEnum.EventDetail:
                    //Event

                    result = await this.DB.Events.Where(Q => Q.EventId == model.ContentId).Select(Q => new ContentModel
                    {
                        Name = Q.Name,
                        Id = Q.EventId
                    }).FirstOrDefaultAsync();

                    break;

                //case 4:
                //    //Banner Detail

                //    result = await this.DB.Banners.Where(Q => Q.BannerId == model.ContentId).Select(Q => new ContentModel
                //    {
                //        Name = Q.Name,
                //        Id = Q.BannerId
                //    }).FirstOrDefaultAsync();

                //    break;

                case MobilePageEnum.NewsDetail:
                    //News
                    result = await this.DB.News.Where(Q => Q.NewsId == model.ContentId).Select(Q => new ContentModel
                    {
                        Name = Q.Title,
                        Id = Q.NewsId
                    }).FirstOrDefaultAsync();

                    break;
                case MobilePageEnum.Guide:

                    //Guide
                    result = await this.DB.Guides.Where(Q => Q.GuideId == model.ContentId).Select(Q => new ContentModel
                    {
                        Name = Q.Name,
                        Id = Q.GuideId
                    }).FirstOrDefaultAsync();

                    break;
                case MobilePageEnum.Coach:

                    result = await (from c in this.DB.Coaches
                                    join emp in this.DB.Employees on c.EmployeeId equals emp.EmployeeId
                                    where c.CoachId == model.ContentId
                                    select new ContentModel
                                    {
                                        Id = c.CoachId,
                                        Name = emp.EmployeeName
                                    }).FirstOrDefaultAsync();

                    break;
                case MobilePageEnum.Insight:
                    //Surveys
                    result = await this.DB.Surveys.Where(Q => Q.SurveyId == model.ContentId).Select(Q => new ContentModel
                    {
                        Name = Q.Title,
                        Id = Q.SurveyId
                    }).FirstOrDefaultAsync();
                    break;

                case MobilePageEnum.LearningDetail:
                    //Release Training
                    result = await this.DB.SetupLearning.Where(Q => Q.SetupLearningId == model.ContentId).Select(Q => new ContentModel
                    {
                        Name = Q.LearningName,
                        Id = Q.SetupLearningId
                    }).FirstOrDefaultAsync();
                    break;
            }

            return result;
        }

        public async Task<List<ApprovalStatusForBannerModel>> GetApprovalStatus()
        {
            var result = await this.DB.ApprovalStatus.Select(Q => new ApprovalStatusForBannerModel
            {
                ApprovalName = Q.ApprovalName,
                ApprovalStatusId = Q.ApprovalStatusId
            }).ToListAsync();

            return result;
        }

    }
}
