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
  public class ServiceTipService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileService;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public ServiceTipService(TalentContext db, IContextualService contextual, ClaimHelper hm, BlobService bm, IFileStorageService fm,PushNotificationService pn)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
      this.FileService = fm;
      this.BlobService = bm;
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
    public async Task<ServiceTipPaginate> GetAllServiceTips(ServiceTipGridFilter filter)
    {
      var query = (from sl in this.DB.ServiceTips
                   join slm in this.DB.ServiceTipMenus on sl.ServiceTipMenuId equals slm.ServiceTipMenuId
                   join sla in this.DB.ServiceTipTypes on sl.ServiceTipTypeId equals sla.ServiceTipTypeId
                   join slb in this.DB.Cms_Menus on sl.Cms_MenuId equals slb.Cms_MenuId
                   join sle in this.DB.Cms_Contents on sl.Cms_ContentId equals sle.Cms_ContentId
                   join sls in this.DB.Cms_SubContents on sl.Cms_SubContentId equals sls.Cms_SubContentId into grpjoin_sl_sls
                   from sls in grpjoin_sl_sls.DefaultIfEmpty()
                   where sl.IsDeleted == false
                   select new ServiceTipModel
                   {
                     ServiceTipId = sl.ServiceTipId,
                     ServiceTipMenuId = sl.ServiceTipMenuId,
                     ServiceTipMenuName = DB.ServiceTipMenus.Where(x => x.ServiceTipMenuId == sl.ServiceTipMenuId).Select(x => x.ServiceTipMenuName).FirstOrDefault(),
                     ServiceTipTypeId = sl.ServiceTipTypeId,
                     ServiceTipTypeName = DB.ServiceTipTypes.Where(x => x.ServiceTipTypeId == sl.ServiceTipTypeId).Select(x => x.ServiceTipTypeName).FirstOrDefault(),
                     Cms_ContentId = sl.Cms_ContentId,
                     Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                     Cms_ContentDescription = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.Cms_ContentDescription).FirstOrDefault(),
                     Cms_ContentBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_ContentVideoBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.CmsContentVideoBlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.CmsContentVideoBlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_ContentFileBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.CmsContentFileBlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.CmsContentFileBlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_SubContentId = sl.Cms_SubContentId,
                     Cms_SubContentName = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.Cms_SubContentName).FirstOrDefault(),
                     Cms_SubContentDescription = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.Cms_SubContentDescription).FirstOrDefault(),
                     Cms_SubContentBlobId = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_SubContentVideoBlobId = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.CmsSubContentVideoBlobId).FirstOrDefault().HasValue ? DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.CmsSubContentVideoBlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_SubContentFileBlobId = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.CmsSubContentFileBlobId).FirstOrDefault().HasValue ? DB.Cms_SubContents.Where(x => x.Cms_SubContentId == sl.Cms_SubContentId).Select(x => x.CmsSubContentFileBlobId).FirstOrDefault().Value : Guid.Empty,
                     Cms_MenuId = sl.Cms_MenuId,
                     Cms_MenuName = DB.Cms_Menus.Where(x => x.Cms_MenuId == sl.Cms_MenuId).Select(x => x.Cms_MenuName).FirstOrDefault(),
                     IsSequence = sl.IsSequence,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();


      if (!string.IsNullOrEmpty(filter.ServiceTipMenuName))
      {
        query = query.Where(Q => Q.ServiceTipMenuName.Contains(filter.ServiceTipMenuName));
      }

      if (filter.StartDate != null && filter.EndDate != null)
      {
        var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
        var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

        var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
        var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

        query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                 (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
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
          query = query.OrderBy(Q => Q.IsSequence);
          break;
      }
      var serviceTips = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in serviceTips)
      {

        var imageURL = "";
        var videoURL = "";
        var fileURL = "";

        var subImageURL = "";
        var subVideoURL = "";
        var subFileURL = "";


        var blobData = await this.BlobService.GetBlobById(datum.Cms_ContentBlobId);
        var blobVideo = await this.BlobService.GetBlobById(datum.Cms_ContentVideoBlobId);
        var blobFile = await this.BlobService.GetBlobById(datum.Cms_ContentFileBlobId);

        var subBlobData = await this.BlobService.GetBlobById(datum.Cms_SubContentBlobId);
        var subBlobVideo = await this.BlobService.GetBlobById(datum.Cms_SubContentVideoBlobId);
        var subBlobFile = await this.BlobService.GetBlobById(datum.Cms_SubContentFileBlobId);

        if (blobData != null)
          imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        if (blobVideo != null)
          videoURL = await this.FileService.GetFileAsync(blobVideo.BlobId.ToString(), blobVideo.Mime);
        if (blobFile != null)
          fileURL = await this.FileService.GetFileAsync(blobFile.BlobId.ToString(), blobFile.Mime);
        if (subBlobData != null)
          subImageURL = await this.FileService.GetFileAsync(subBlobData.BlobId.ToString(), subBlobData.Mime);
        if (subBlobVideo != null)
          subVideoURL = await this.FileService.GetFileAsync(subBlobVideo.BlobId.ToString(), subBlobVideo.Mime);
        if (subBlobFile != null)
          subFileURL = await this.FileService.GetFileAsync(subBlobFile.BlobId.ToString(), subBlobFile.Mime);


        serviceTips[index].Cms_ContentBlobImageUrl = imageURL;
        serviceTips[index].Cms_ContentVideoBlobImageUrl = videoURL;
        serviceTips[index].Cms_ContentFileBlobImageUrl = fileURL;
        serviceTips[index].Cms_SubContentBlobImageUrl = subImageURL;
        serviceTips[index].Cms_SubContentVideoBlobImageUrl = subVideoURL;
        serviceTips[index].Cms_SubContentFileBlobImageUrl = subFileURL;
        index++;
      }

      return new ServiceTipPaginate
      {
        ServiceTips = serviceTips,
        TotalServiceTips = totalData
      };
    }
    public async Task<ServiceTipModel> GetServiceTipById(Guid serviceTipId)
    {
      var data = await (from mo in DB.ServiceTips
                        join slm in this.DB.ServiceTipMenus on mo.ServiceTipMenuId equals slm.ServiceTipMenuId
                        join sla in this.DB.ServiceTipTypes on mo.ServiceTipTypeId equals sla.ServiceTipTypeId
                        join slb in this.DB.Cms_Menus on mo.Cms_MenuId equals slb.Cms_MenuId
                        join sle in this.DB.Cms_Contents on mo.Cms_ContentId equals sle.Cms_ContentId
                        join sls in this.DB.Cms_SubContents on mo.Cms_SubContentId equals sls.Cms_SubContentId into grpjoin_mo_sls
                        from sls in grpjoin_mo_sls.DefaultIfEmpty()
                        where mo.IsDeleted == false && mo.ServiceTipId == serviceTipId
                        select new ServiceTipModel
                        {
                          ServiceTipId = mo.ServiceTipId,
                          ServiceTipMenuId = mo.ServiceTipMenuId,
                          ServiceTipMenuName = DB.ServiceTipMenus.Where(x => x.ServiceTipMenuId == mo.ServiceTipMenuId).Select(x => x.ServiceTipMenuName).FirstOrDefault(),
                          ServiceTipTypeId = mo.ServiceTipTypeId,
                          ServiceTipTypeName = DB.ServiceTipTypes.Where(x => x.ServiceTipTypeId == mo.ServiceTipTypeId).Select(x => x.ServiceTipTypeName).FirstOrDefault(),
                          Cms_ContentId = mo.Cms_ContentId,
                          Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                          Cms_ContentDescription = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.Cms_ContentDescription).FirstOrDefault(),
                          Cms_ContentBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_ContentVideoBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.CmsContentVideoBlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.CmsContentVideoBlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_ContentFileBlobId = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.CmsContentFileBlobId).FirstOrDefault().HasValue ? DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.CmsContentFileBlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_SubContentId = mo.Cms_SubContentId,
                          Cms_SubContentName = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.Cms_SubContentName).FirstOrDefault(),
                          Cms_SubContentDescription = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.Cms_SubContentDescription).FirstOrDefault(),
                          Cms_SubContentBlobId = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.BlobId).FirstOrDefault().HasValue ? DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.BlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_SubContentVideoBlobId = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.CmsSubContentVideoBlobId).FirstOrDefault().HasValue ? DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.CmsSubContentVideoBlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_SubContentFileBlobId = DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.CmsSubContentFileBlobId).FirstOrDefault().HasValue ? DB.Cms_SubContents.Where(x => x.Cms_SubContentId == mo.Cms_SubContentId).Select(x => x.CmsSubContentFileBlobId).FirstOrDefault().Value : Guid.Empty,
                          Cms_MenuId = mo.Cms_MenuId,
                          Cms_MenuName = DB.Cms_Menus.Where(x => x.Cms_MenuId == mo.Cms_MenuId).Select(x => x.Cms_MenuName).FirstOrDefault(),
                          IsSequence = mo.IsSequence,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      var imageURL = "";
      var videoURL = "";
      var fileURL = "";

      var subImageURL = "";
      var subVideoURL = "";
      var subFileURL = "";


      var blobData = await this.BlobService.GetBlobById(data.Cms_ContentBlobId);
      var blobVideo = await this.BlobService.GetBlobById(data.Cms_ContentVideoBlobId);
      var blobFile = await this.BlobService.GetBlobById(data.Cms_ContentFileBlobId);

      var subBlobData = await this.BlobService.GetBlobById(data.Cms_SubContentBlobId);
      var subBlobVideo = await this.BlobService.GetBlobById(data.Cms_SubContentVideoBlobId);
      var subBlobFile = await this.BlobService.GetBlobById(data.Cms_SubContentFileBlobId);

      if (blobData != null)
        imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
      if (blobVideo != null)
        videoURL = await this.FileService.GetFileAsync(blobVideo.BlobId.ToString(), blobVideo.Mime);
      if (blobFile != null)
        fileURL = await this.FileService.GetFileAsync(blobFile.BlobId.ToString(), blobFile.Mime);
      if (subBlobData != null)
        subImageURL = await this.FileService.GetFileAsync(subBlobData.BlobId.ToString(), subBlobData.Mime);
      if (subBlobVideo != null)
        subVideoURL = await this.FileService.GetFileAsync(subBlobVideo.BlobId.ToString(), subBlobVideo.Mime);
      if (subBlobFile != null)
        subFileURL = await this.FileService.GetFileAsync(subBlobFile.BlobId.ToString(), subBlobFile.Mime);


      data.Cms_ContentBlobImageUrl = imageURL;
      data.Cms_ContentVideoBlobImageUrl = videoURL;
      data.Cms_ContentFileBlobImageUrl = fileURL;
      data.Cms_SubContentBlobImageUrl = subImageURL;
      data.Cms_SubContentVideoBlobImageUrl = subVideoURL;
      data.Cms_SubContentFileBlobImageUrl = subFileURL;

      return data;
    }
    public async Task<ActionResult<BaseResponse>> InsertServiceTipAsync(ServiceTipCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var userLogin = this.HelperMan.GetLoginUserId();

            var isExist = await ValidateServiceTipAsync(model.ServiceTipMenuId, model.ServiceTipTypeId, model.Cms_SubContentId.HasValue ? model.Cms_SubContentId.Value : Guid.Empty, model.Cms_MenuId, model.Cms_ContentId);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var insertServiceTips = new ServiceTips
            {
                ServiceTipMenuId = model.ServiceTipMenuId,
                ServiceTipTypeId = model.ServiceTipTypeId,
                Cms_SubContentId = model.Cms_SubContentId,
                Cms_MenuId = model.Cms_MenuId,
                Cms_ContentId = model.Cms_ContentId,
                IsSequence = model.IsSequence,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedBy = userLogin,
            };

            this.DB.ServiceTips.Add(insertServiceTips);

            await this.DB.SaveChangesAsync();
            if (model.ApprovedAt != null)
            {
                var getServiceTipType = await this.DB.ServiceTipTypes.Where(Q => Q.ServiceTipTypeId == model.ServiceTipTypeId).Select(Q => Q.ServiceTipTypeName).FirstOrDefaultAsync();
                var getIdServiceTip = insertServiceTips.ServiceTipId;
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Service Tip";
                PushNotificationMyTools.Body = "New Service Tip in " + getServiceTipType + " has been added.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "Service Tip";
                PushNotificationDataMyTools.DataID = getIdServiceTip;
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
    public async Task<bool> ValidateServiceTipAsync(Guid serviceTipMenuId, Guid serviceTipTypeId, Guid Cms_SubContentId, Guid Cms_MenuId, Guid Cms_ContentId)
    {
      var isExist = await this
          .DB
          .ServiceTips
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ServiceTipMenuId == serviceTipMenuId && Q.ServiceTipTypeId == serviceTipTypeId && Q.Cms_SubContentId == Cms_SubContentId && Q.Cms_MenuId == Cms_MenuId && Q.Cms_ContentId == Cms_ContentId);

      return isExist;
    }
    public async Task<ActionResult<BaseResponse>> UpdateServiceTipAsync(ServiceTipUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateServiceTipAsync(model.ServiceTipId, model.ServiceTipMenuId, model.ServiceTipTypeId, model.Cms_SubContentId.HasValue ? model.Cms_SubContentId.Value : Guid.Empty, model.Cms_MenuId, model.Cms_ContentId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var serviceTipToUpdate = await this.DB.ServiceTips.Where(Q => Q.ServiceTipId == model.ServiceTipId).FirstOrDefaultAsync();

            serviceTipToUpdate.ServiceTipId = model.ServiceTipId;
            serviceTipToUpdate.ServiceTipMenuId = model.ServiceTipMenuId;
            serviceTipToUpdate.ServiceTipTypeId = model.ServiceTipTypeId;
            serviceTipToUpdate.Cms_SubContentId = model.Cms_SubContentId;
            serviceTipToUpdate.Cms_MenuId = model.Cms_MenuId;
            serviceTipToUpdate.Cms_ContentId = model.Cms_ContentId;
            serviceTipToUpdate.IsSequence = model.IsSequence;
            serviceTipToUpdate.ApprovedAt = model.ApprovedAt;
            serviceTipToUpdate.UpdatedAt = thisDate;
            serviceTipToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            if (model.ApprovedAt != null)
            {
                var getServiceTipType = await this.DB.ServiceTipTypes.Where(Q => Q.ServiceTipTypeId == model.ServiceTipTypeId).Select(Q => Q.ServiceTipTypeName).FirstOrDefaultAsync();
                var getIdServiceTip = serviceTipToUpdate.ServiceTipId;
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "Service Tip Update";
                PushNotificationMyTools.Body = "Service Tip in " + getServiceTipType + " has been updated.";
                PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                PushNotificationMyTools.IsPublished = true;
                PushNotificationMyTools.GroupPositions = groupPositionList;
                PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                PushNotificationDataMyTools.Category = "Service Tip";
                PushNotificationDataMyTools.DataID = getIdServiceTip;
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
    public async Task<bool> ValidateUpdateServiceTipAsync(Guid serviceTipId, Guid serviceTipMenuId, Guid serviceTipTypeId, Guid Cms_SubContentId, Guid Cms_MenuId, Guid Cms_ContentId)
    {
      var data = await this
          .DB
          .ServiceTips
          .AsNoTracking()
          .Where(Q => Q.ServiceTipId == serviceTipId)
          .FirstOrDefaultAsync();

      var isChange = data.Cms_ContentId != Cms_ContentId;

      var isExist = await this.ValidateServiceTipAsync(serviceTipMenuId, serviceTipTypeId, Cms_SubContentId, Cms_MenuId, Cms_ContentId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteServiceTipAsync(DeleteServiceTipModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ServiceTips.Where(Q => Q.ServiceTipId == model.ServiceTipId).FirstOrDefaultAsync();

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
