using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class HOGuidelineUploadService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;
        private readonly PushNotificationService PNService;

        public HOGuidelineUploadService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs, PushNotificationService pn)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.HelperMan = hm;
            this.FileMan = fm;
            this.BlobService = bs;
            this.PNService = pn;
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
        public async Task<HOGuidelineUploadPaginate> GetAllHOGuidelineUpload(HOGuidelineUploadGridFilter filter)
        {
            var query = (from sl in this.DB.HOGuidelines
                         where sl.IsDeleted == false
                         select new HOGuidelineUploadModel
                         {
                             HOGuidelineId = sl.HOGuidelineId,
                             BlobId = sl.BlobId,
                             HOGuidelineTitle = sl.HOGuidelineTitle,
                             HOGuidelineComment = sl.HOGuidelineComment,
                             HOGuidelineStatus = sl.HOGuideLineStatus,
                             ApprovedAt = sl.ApprovedAt,
                             CreatedAt = sl.CreatedAt,
                             CreatedBy = sl.CreatedBy,
                             UpdatedBy = sl.UpdatedBy,
                             UpdatedAt = sl.UpdatedAt
                         }).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(filter.HOGuidelineTitle))
            {
                query = query.Where(Q => Q.HOGuidelineTitle.Contains(filter.HOGuidelineTitle));
            }

            //sort
            switch (filter.SortBy)
            {
                case "CreatedAtAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
            }
            var hoGuidelineUploads = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            var index = 0;
            foreach (var datum in hoGuidelineUploads)
            {

                var imageURL = "";
                var fileFormat = "";

                var blobData = await this.BlobService.GetBlobById(datum.BlobId);

                imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                fileFormat = blobData.Mime;

                hoGuidelineUploads[index].BlobUrl = imageURL;
                hoGuidelineUploads[index].BlobFileType = fileFormat;
                index++;
            }

            return new HOGuidelineUploadPaginate
            {
                HOGuidelineUploads = hoGuidelineUploads,
                TotalHOGuidelineUploads = totalData
            };
        }
        public async Task<HOGuidelinePaginate> GetAllHOGuideline(HOGuidelineGridFilter filter)
        {
            var query = (from sl in this.DB.HOGuidelines
                         join emp in this.DB.EmployeePositionMappings on sl.CreatedBy equals emp.EmployeeId
                         join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                         join ot in this.DB.Outlets on em.OutletId equals ot.OutletId
                         where sl.IsDeleted == false
                         select new HOGuidelineModel
                         {
                             HOGuidelineId = sl.HOGuidelineId,
                             BlobId = sl.BlobId,
                             HOGuidelineTitle = sl.HOGuidelineTitle,
                             HOGuidelineComment = sl.HOGuidelineComment,
                             IsApproved = sl.IsApproved,
                             ApprovedAt = sl.ApprovedAt,
                             CreatedAt = sl.CreatedAt,
                             CreatedBy = sl.CreatedBy,
                             UpdatedBy = sl.UpdatedBy,
                             UpdatedAt = sl.UpdatedAt,
                             OutletName = ot.Name
                         }).AsNoTracking().AsQueryable();

            //search
            if (filter.HOGuidelineTitle != null)
            {
                query = query.Where(Q => Q.HOGuidelineTitle.Contains(filter.HOGuidelineTitle));
            }


            //sort
            switch (filter.SortBy)
            {
                case "CreatedAtAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
            }
            var hoGuidelines = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            var index = 0;
            foreach (var datum in hoGuidelines)
            {

                var imageURL = "";
                var fileFormat = "";

                var blobData = await this.BlobService.GetBlobById(datum.BlobId);

                imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                fileFormat = blobData.Mime;

                hoGuidelines[index].BlobUrl = imageURL;
                hoGuidelines[index].BlobFileType = fileFormat;
                index++;
            }

            return new HOGuidelinePaginate
            {
                HOGuidelines = hoGuidelines,
                TotalHOGuidelines = totalData
            };
        }
        public async Task<ActionResult<BaseResponse>> InsertHOGuidelineUploadAsync(HOGuidelineUploadCreateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isExist = await ValidateHOGuidelineUploadByTitleAsync(model.HOGuidelineTitle);

                if (isExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                Guid getUploadBlob = Guid.Empty;

                if (model.HOGuidelineFileContent != null)
                {
                    if (string.IsNullOrEmpty(model.HOGuidelineFileContent.Base64) == false)
                    {
                        getUploadBlob = await this.FileMan.UploadFileFromBase64(model.HOGuidelineFileContent);
                    }
                }

                var newHOGuidelines = new HOGuidelines
                {
                    HOGuidelineTitle = model.HOGuidelineTitle,
                    BlobId = getUploadBlob,
                    HOGuidelineComment = "",
                    HOGuideLineStatus = "Awaiting Approval",
                    IsApproved = false,
                    ApprovedAt = model.ApprovedAt,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = this.HelperMan.GetLoginUserId(),
                    UpdatedBy = this.HelperMan.GetLoginUserId()
                };

                this.DB.HOGuidelines.Add(newHOGuidelines);

                await this.DB.SaveChangesAsync();
                if (model.ApprovedAt != null)
                {
                    var getIdKodawari = newHOGuidelines.HOGuidelineId;
                    var groupPositionList = new List<string>();
                    var manPowerPositionList = new List<string>();
                    var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                    var PushNotificationMyTools = new VMPushNotificationAdd();
                    PushNotificationMyTools.Title = "New Guideline";
                    PushNotificationMyTools.Body = "New Guideline " + model.HOGuidelineTitle + " has been uploaded.";
                    PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                    PushNotificationMyTools.IsPublished = true;
                    PushNotificationMyTools.GroupPositions = groupPositionList;
                    PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                    PushNotificationDataMyTools.Category = "Kodawari";
                    PushNotificationDataMyTools.DataID = getIdKodawari;
                    PushNotificationDataMyTools.DataSecondId = Guid.Empty;

                    await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
                }
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<bool> ValidateHOGuidelineUploadByTitleAsync(string hoGuidelineTitle)
        {
            var isExist = await this
                .DB
                .HOGuidelines
                .AsNoTracking()
                .Where(Q => Q.IsDeleted == false)
                .AnyAsync(Q => Q.HOGuidelineTitle == hoGuidelineTitle);

            return isExist;
        }
        public async Task<HOGuidelineModel> GetHOGuidelineById(Guid hoGuidelineId)
        {
            var data = await (from mo in DB.HOGuidelines
                              where mo.HOGuidelineId == hoGuidelineId && mo.IsDeleted == false
                              select new HOGuidelineModel
                              {
                                  HOGuidelineId = mo.HOGuidelineId,
                                  BlobId = mo.BlobId,
                                  HOGuidelineTitle = mo.HOGuidelineTitle,
                                  HOGuidelineComment = mo.HOGuidelineComment,
                                  IsApproved = mo.IsApproved,
                                  ApprovedAt = mo.ApprovedAt,
                                  CreatedAt = mo.CreatedAt,
                                  CreatedBy = mo.CreatedBy,
                                  UpdatedBy = mo.UpdatedBy,
                                  UpdatedAt = mo.UpdatedAt
                              }).FirstOrDefaultAsync();

            var imageURL = "";
            var fileType = "";

            var blobData = await this.BlobService.GetBlobById(data.BlobId);

            imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
            fileType = blobData.Mime;

            data.BlobUrl = imageURL;
            data.BlobFileType = fileType;

            return data;
        }
        public async Task<ActionResult<BaseResponse>> UpdateHOGuidelineCommentAsync(HOGuidelineCommentUpdateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateHOGuidelineCommentByCommentAsync(model.HOGuidelineComment, model.HOGuidelineId);

                if (!isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var hoGuidelineCommentToUpdate = await this.DB.HOGuidelines.Where(Q => Q.HOGuidelineId == model.HOGuidelineId).FirstOrDefaultAsync();

                hoGuidelineCommentToUpdate.HOGuidelineId = model.HOGuidelineId;
                hoGuidelineCommentToUpdate.HOGuidelineComment = model.HOGuidelineComment;
                hoGuidelineCommentToUpdate.HOGuideLineStatus = "Revised";
                hoGuidelineCommentToUpdate.ApprovedAt = model.ApprovedAt;
                hoGuidelineCommentToUpdate.UpdatedAt = thisDate;
                hoGuidelineCommentToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                if (model.ApprovedAt != null)
                {
                    var getIdKodawari = hoGuidelineCommentToUpdate.HOGuidelineId;
                    var getHOGuidelinesTitle = await this.DB.HOGuidelines.Where(Q => Q.HOGuidelineId == hoGuidelineCommentToUpdate.HOGuidelineId).Select(Q => Q.HOGuidelineTitle).FirstOrDefaultAsync();
                    var groupPositionList = new List<string>();
                    var manPowerPositionList = new List<string>();
                    var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                    var PushNotificationMyTools = new VMPushNotificationAdd();
                    PushNotificationMyTools.Title = "Guideline Update";
                    PushNotificationMyTools.Body = "Guideline " + getHOGuidelinesTitle + " has been updated.";
                    PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                    PushNotificationMyTools.IsPublished = true;
                    PushNotificationMyTools.GroupPositions = groupPositionList;
                    PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                    PushNotificationDataMyTools.Category = "Kodawari";
                    PushNotificationDataMyTools.DataID = getIdKodawari;
                    PushNotificationDataMyTools.DataSecondId = Guid.Empty;

                    await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
                }
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<bool> ValidateUpdateHOGuidelineCommentByCommentAsync(string hoGuidelineComment, Guid hoGuidelineId)
        {
            var data = await this
                .DB
                .HOGuidelines
                .AsNoTracking()
                .Where(Q => Q.HOGuidelineId == hoGuidelineId)
                .FirstOrDefaultAsync();

            var isChange = data.HOGuidelineComment != hoGuidelineComment;

            var isExist = await this.ValidateHOGuidelineUploadByTitleAsync(data.HOGuidelineTitle);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }
        public async Task<ActionResult<BaseResponse>> UpdateHOGuidelineApproveAsync(HOGuidelineStatusApprovedUpdateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateHOGuidelineApprovalByBoolAsync(model.IsApproved, model.HOGuidelineId);

                if (isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var hoGuidelineCommentToUpdate = await this.DB.HOGuidelines.Where(Q => Q.HOGuidelineId == model.HOGuidelineId).FirstOrDefaultAsync();

                hoGuidelineCommentToUpdate.HOGuidelineId = model.HOGuidelineId;
                hoGuidelineCommentToUpdate.IsApproved = model.IsApproved;
                hoGuidelineCommentToUpdate.HOGuideLineStatus = "Approved";
                hoGuidelineCommentToUpdate.ApprovedAt = model.ApprovedAt;
                hoGuidelineCommentToUpdate.UpdatedAt = thisDate;
                hoGuidelineCommentToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UpdateHOGuidelineAsync(HOGuidelineUploadUpdateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateHOGuidelineAsync(model.HOGuidelineTitle, model.HOGuidelineId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var hoGuidelineCommentToUpdate = await this.DB.HOGuidelines.Where(Q => Q.HOGuidelineId == model.HOGuidelineId).FirstOrDefaultAsync();

            if (model.HOGuidelineFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.HOGuidelineFileContent.Base64))
                {
                    hoGuidelineCommentToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.HOGuidelineFileContent);
                }
            }

            hoGuidelineCommentToUpdate.HOGuidelineId = model.HOGuidelineId;
            hoGuidelineCommentToUpdate.HOGuidelineTitle = model.HOGuidelineTitle;
            hoGuidelineCommentToUpdate.HOGuideLineStatus = "Awaiting Approval";
            hoGuidelineCommentToUpdate.UpdatedAt = thisDate;
            hoGuidelineCommentToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        public async Task<bool> ValidateUpdateHOGuidelineAsync(string hoGuidelineComment, Guid hoGuidelineId)
        {
            var data = await this
                .DB
                .HOGuidelines
                .AsNoTracking()
                .Where(Q => Q.HOGuidelineId == hoGuidelineId)
                .FirstOrDefaultAsync();

            var isChange = false;

            var isExist = await this.ValidateHOGuidelineUploadByTitleAsync(hoGuidelineComment);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }

        public async Task<bool> ValidateUpdateHOGuidelineApprovalByBoolAsync(bool isApproved, Guid hoGuidelineId)
        {
            var data = await this
                .DB
                .HOGuidelines
                .AsNoTracking()
                .Where(Q => Q.HOGuidelineId == hoGuidelineId)
                .FirstOrDefaultAsync();

            var isChange = data.IsApproved != isApproved;

            var isExist = await this.ValidateHOGuidelineUploadByTitleAsync(data.HOGuidelineTitle);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }
        public async Task<ActionResult<BaseResponse>> DeleteHOGuidelineAsync(DeleteHOGuidelineModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var module = await this.DB.HOGuidelines.Where(Q => Q.HOGuidelineId == model.HOGuidelineId).FirstOrDefaultAsync();

                module.IsDeleted = true;
                module.UpdatedAt = thisDate;
                module.UpdatedBy = this.HelperMan.GetLoginUserId();


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
