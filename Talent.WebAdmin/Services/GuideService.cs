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
    public class GuideService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly IFileStorageService FileMan;
        private readonly ApprovalService ApprovalMan;
        private readonly ClaimHelper ClaimMan;



        public GuideService(TalentContext db, IContextualService contextual, IFileStorageService fileService, ApprovalService approvalService, ClaimHelper claimHelper)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.FileMan = fileService;
            this.ApprovalMan = approvalService;
            this.ClaimMan = claimHelper;
        }

        public string GetUserLogin()
        {
            try
            {
                return ContextMan.CookieClaims.EmployeeId;
            }
            catch
            {
                return "SYSTEM";
            }
        }

        public async Task<GuideViewModel> GetAllGuides()
        {
            var allGuides = await this.DB.Guides.Select(Q => new GuideModel
            {
                GuideId = Q.GuideId,
                GuideTypeId = Q.GuideTypeId,
                BlobId = Q.BlobId,
                Name = Q.Name,
                Description = Q.Description,
                CreatedAt = Q.CreatedAt,
                CreatedBy = Q.CreatedBy,
                UpdatedAt = Q.UpdatedAt,
                ApprovedAt = Q.ApprovedAt
            }).ToListAsync();

            var totalItem = await this.DB.Guides.CountAsync();

            var result = new GuideViewModel
            {
                ListGuideModel = allGuides,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<GuideModel> GetGuideById(int id)
        {
            var result = await DB.Guides.AsNoTracking().Select(Q => new GuideModel
            {
                GuideId = Q.GuideId,
                Name = Q.Name,
                Description = Q.Description,
                GuideTypeId = Q.GuideTypeId,
                BlobId = Q.BlobId,
                ApprovedAt = Q.ApprovedAt,
                CreatedBy = Q.CreatedBy,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt
            }).Where(Q => Q.GuideId == id).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new GuideModel();
            }

            return result;
        }

        public async Task<int> CreateGuide(GuideFormModel model)
        {
            var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);

            Guides createGuideModel = new Guides
            {
                GuideTypeId = model.GuideTypeId,
                //BlobId = model.BlobId,
                BlobId = getUploadBlob,
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = GetUserLogin(),
                UpdatedBy = GetUserLogin(),
                ApprovedAt = null
            };
            this.DB.Guides.Add(createGuideModel);
            var newApproval = new ApprovalCreateFormModel
            {
                ContentName = createGuideModel.Name,
                ContentCategory = ContentCategoryEnums.Guide,
                ApprovalStatusId = model.ApprovalId,
                ContentId = createGuideModel.GuideId,
                PageEnum = PageEnums.Guide,
            };

            var approval = await ApprovalMan.CreateNewApproval(newApproval);

            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                createGuideModel.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            await this.DB.SaveChangesAsync();
            return createGuideModel.GuideId;
        }

        public async Task<bool> UpdateGuide(GuideFormModel model)
        {
            var updateModel = await this.DB.Guides.FindAsync(model.GuideId);

            if (updateModel == null)
            {
                return false;
            }

            Guid? getUploadBlob = model.BlobId;

            if (model.FileContent != null)
            {
                getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);
                updateModel.BlobId = getUploadBlob.GetValueOrDefault();
                this.DB.Guides.Update(updateModel);

                await FileMan.DeleteFileAsync(model.BlobId.GetValueOrDefault(), model.FileContent.ContentType);
                var BlobId = await this.DB.Blobs.Where(Q => Q.BlobId == model.BlobId).FirstOrDefaultAsync();
                this.DB.Blobs.Remove(BlobId);
            }

            updateModel.GuideTypeId = model.GuideTypeId;
            updateModel.Name = model.Name;
            updateModel.Description = model.Description;
            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateModel.UpdatedBy = ClaimMan.GetLoginUserId();

            var currentApproval = await DB.ApprovalToGuides.Where(Q => Q.GuideId == model.GuideId).FirstOrDefaultAsync();

            var newApproval = new ApprovalUpdateFormModel
            {
                ApprovalId = currentApproval.ApprovalId,
                PageEnum = PageEnums.Guide,
                ApprovalStatusId = model.ApprovalId,
                ContentName = updateModel.Name,
                ContentCategory = ContentCategoryEnums.Guide
            };
            var approval = await ApprovalMan.UpdateNewApproval(newApproval);
            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId) // submit
            {
                updateModel.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else
            {
                updateModel.ApprovedAt = null;
            }

            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGuide(int id)
        {

            var approvalsToGuide = await this.DB.ApprovalToGuides.Where(Q => Q.GuideId == id).FirstOrDefaultAsync();


            if (approvalsToGuide == null)
            {
                return false;
            }

            this.DB.ApprovalToGuides.Remove(approvalsToGuide);
            var deleteGuide = await this.DB.Guides.FindAsync(id);

            var isDeleted = await this.ApprovalMan.DeleteApproval(approvalsToGuide.ApprovalId);

            if (isDeleted == false)
            {
                return false;
            }

            this.DB.Guides.Remove(deleteGuide);
            await this.DB.SaveChangesAsync();

            return true;
        }
    }

    public class GuideJoinService
    {
        private readonly TalentContext DB;
        private readonly GuideService GuideMan;
        private readonly GuideTypeService GuideTypeMan;
        private readonly BlobService BlobMan;

        public GuideJoinService(TalentContext db, GuideService guideService, GuideTypeService guideTypeService, BlobService blobService)
        {
            this.DB = db;
            GuideMan = guideService;
            GuideTypeMan = guideTypeService;
            BlobMan = blobService;
        }

        public async Task<GuideJoinViewModel> GetAllGuides(GuideFilter filter)
        {
            var query = (
                from g in DB.Guides
                join t in DB.GuideTypes on g.GuideTypeId equals t.GuideTypeId
                join b in DB.Blobs on g.BlobId equals b.BlobId
                join atg in DB.ApprovalToGuides on g.GuideId equals atg.GuideId into latg
                from atg in latg.DefaultIfEmpty()
                join a in DB.Approvals on atg.ApprovalId equals a.ApprovalId into la
                from a in la.DefaultIfEmpty()
                join s in DB.ApprovalStatus on a.ApprovalStatusId equals s.ApprovalStatusId into ls
                from s in ls.DefaultIfEmpty()
                join e in DB.Employees on g.CreatedBy equals e.EmployeeId
                select new GuideJoinModel
                {
                    GuideId = g.GuideId,
                    GuideTypeId = g.GuideTypeId,
                    GuideTypeName = t.Name,
                    Name = g.Name,
                    Description = g.Description,
                    BlobId = g.BlobId,
                    FileName = b.Name,
                    Mime = b.Mime,
                    CreatedAt = g.CreatedAt,
                    CreatedBy = e.EmployeeName,
                    UpdatedAt = g.UpdatedAt,
                    ApprovedAt = g.ApprovedAt,
                    ApprovalStatus = g.ApprovedAt != null ? ApprovalStatusesEnum.Approve : (s.ApprovalName == null ? ApprovalStatusesEnum.Approve : s.ApprovalName)
                });

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (string.IsNullOrEmpty(filter.GuideName) == false)
            {
                query = query.Where(Q => Q.Name.Contains(filter.GuideName));
            }

            if (string.IsNullOrEmpty(filter.GuideTypeName) == false)
            {
                query = query.Where(Q => Q.GuideTypeName.Equals(filter.GuideTypeName));
            }

            if (string.IsNullOrEmpty(filter.CreatedBy) == false)
            {
                query = query.Where(Q => Q.CreatedBy.Contains(filter.CreatedBy));
            }

            if (string.IsNullOrEmpty(filter.ApprovalStatus) == false)
            {
                query = query.Where(Q => Q.ApprovalStatus.Contains(filter.ApprovalStatus));
            }

            query = query.OrderByDescending(Q => Q.UpdatedAt);

            switch (filter.SortBy)
            {
                case "guideName":
                    query = query.OrderBy(Q => Q.Name).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "guideNameDesc":
                    query = query.OrderByDescending(Q => Q.Name).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "guideType":
                    query = query.OrderBy(Q => Q.GuideTypeName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "guideTypeDesc":
                    query = query.OrderByDescending(Q => Q.GuideTypeName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdBy":
                    query = query.OrderBy(Q => Q.CreatedBy).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdByDesc":
                    query = query.OrderByDescending(Q => Q.CreatedBy).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "approvalStatus":
                    query = query.OrderBy(Q => Q.ApprovalStatus).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "approvalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var resultQuery = await query.Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();
            var totalData = await query.CountAsync();

            var result = new GuideJoinViewModel
            {
                ListGuideJoinModel = resultQuery,
                TotalItem = totalData
            };

            return result;
        }

        public async Task<GuideJoinModel> GetGuideJoinById(int id)
        {
            var guide = await GuideMan.GetGuideById(id);

            if (guide.Name == null)
            {
                return new GuideJoinModel();
            }

            var guideType = await GuideTypeMan.GetGuideTypeById(guide.GuideTypeId);
            var blob = await BlobMan.GetBlobById(guide.BlobId);

            var result = new GuideJoinModel
            {
                GuideId = guide.GuideId,
                GuideTypeId = guide.GuideTypeId,
                GuideTypeName = guideType.Name,
                BlobId = guide.BlobId,
                FileName = blob.Name,
                Mime = blob.Mime,
                Name = guide.Name,
                Description = guide.Description,
                CreatedAt = guide.CreatedAt,
                CreatedBy = guide.CreatedBy,
                UpdatedAt = guide.UpdatedAt,
                ApprovedAt = guide.ApprovedAt
            };

            return result;
        }
    }
}
