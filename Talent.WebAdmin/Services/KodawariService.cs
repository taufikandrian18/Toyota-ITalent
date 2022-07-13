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
  public class KodawariService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileService;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;
    private readonly PushNotificationService PNService;

    public KodawariService(TalentContext db, IContextualService contextual, ClaimHelper hm, BlobService bm, IFileStorageService fm,PushNotificationService pn)
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
    public async Task<KodawariPaginate> GetAllKodawari(KodawariGridFilter filter)
    {
      var query = (from sl in this.DB.Kodawaris
                   join slm in this.DB.KodawariMenus on sl.KodawariMenuId equals slm.KodawariMenuId
                   join sla in this.DB.KodawariTypes on sl.KodawariTypeId equals sla.KodawariTypeId
                   join slb in this.DB.Cms_Menus on sl.Cms_MenuId equals slb.Cms_MenuId
                   join sle in this.DB.Cms_Contents on sl.Cms_ContentId equals sle.Cms_ContentId
                   join sls in this.DB.Cms_SubContents on sl.Cms_SubContentId equals sls.Cms_SubContentId into grpjoin_sl_sls
                   from sls in grpjoin_sl_sls.DefaultIfEmpty()
                   where sl.IsDeleted == false
                   select new KodawariModel
                   {
                     KodawariId = sl.KodawariId,
                     KodawariMenuId = sl.KodawariMenuId,
                     KodawariMenuName = DB.KodawariMenus.Where(x => x.KodawariMenuId == sl.KodawariMenuId).Select(x => x.KodawariMenuName).FirstOrDefault(),
                     KodawariTypeId = sl.KodawariTypeId,
                     KodawariTypeName = DB.KodawariTypes.Where(x => x.KodawariTypeId == sl.KodawariTypeId).Select(x => x.KodawariTypeName).FirstOrDefault(),
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
                     KodawariSequence = sl.KodawariSequence,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.KodawariMenuName))
      {
        query = query.Where(Q => Q.KodawariMenuName.Contains(filter.KodawariMenuName));
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
          query = query.OrderBy(Q => Q.KodawariSequence);
          break;
      }
      var kodawaris = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in kodawaris)
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


        kodawaris[index].Cms_ContentBlobImageUrl = imageURL;
        kodawaris[index].Cms_ContentVideoBlobImageUrl = videoURL;
        kodawaris[index].Cms_ContentFileBlobImageUrl = fileURL;
        kodawaris[index].Cms_SubContentBlobImageUrl = subImageURL;
        kodawaris[index].Cms_SubContentVideoBlobImageUrl = subVideoURL;
        kodawaris[index].Cms_SubContentFileBlobImageUrl = subFileURL;
        index++;
      }

      return new KodawariPaginate
      {
        Kodawaris = kodawaris,
        TotalKodawaris = totalData
      };
    }
    public async Task<KodawariModel> GetKodawariById(Guid kodawariId)
    {
      var data = await (from mo in DB.Kodawaris
                        join slm in this.DB.KodawariMenus on mo.KodawariMenuId equals slm.KodawariMenuId
                        join sla in this.DB.KodawariTypes on mo.KodawariTypeId equals sla.KodawariTypeId
                        join slb in this.DB.Cms_Menus on mo.Cms_MenuId equals slb.Cms_MenuId
                        join sle in this.DB.Cms_Contents on mo.Cms_ContentId equals sle.Cms_ContentId
                        join sls in this.DB.Cms_SubContents on mo.Cms_SubContentId equals sls.Cms_SubContentId into grpjoin_mo_sls
                        from sls in grpjoin_mo_sls.DefaultIfEmpty()
                        where mo.IsDeleted == false && mo.KodawariId == kodawariId
                        select new KodawariModel
                        {
                          KodawariId = mo.KodawariId,
                          KodawariMenuId = mo.KodawariMenuId,
                          KodawariMenuName = DB.KodawariMenus.Where(x => x.KodawariMenuId == mo.KodawariMenuId).Select(x => x.KodawariMenuName).FirstOrDefault(),
                          KodawariTypeId = mo.KodawariTypeId,
                          KodawariTypeName = DB.KodawariTypes.Where(x => x.KodawariTypeId == mo.KodawariTypeId).Select(x => x.KodawariTypeName).FirstOrDefault(),
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
                          KodawariSequence = mo.KodawariSequence,
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
    public async Task<ActionResult<BaseResponse>> InsertKodawariAsync(KodawariCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var userLogin = this.HelperMan.GetLoginUserId();

            var isExist = await ValidateKodawariAsync(model.KodawariMenuId, model.KodawariTypeId, model.Cms_SubContentId.HasValue ? model.Cms_SubContentId.Value : Guid.Empty, model.Cms_MenuId, model.Cms_ContentId);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var insertKodawaris = new Kodawaris
            {
                KodawariMenuId = model.KodawariMenuId,
                KodawariTypeId = model.KodawariTypeId,
                Cms_SubContentId = model.Cms_SubContentId,
                Cms_MenuId = model.Cms_MenuId,
                Cms_ContentId = model.Cms_ContentId,
                KodawariSequence = model.KodawariSequence,
                ApprovedAt = model.ApprovedAt,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedBy = userLogin,
            };

            this.DB.Kodawaris.Add(insertKodawaris);

            await this.DB.SaveChangesAsync();
            if (model.ApprovedAt != null)
            {
                var getKodawariType = await this.DB.KodawariTypes.Where(Q => Q.KodawariTypeId == model.KodawariTypeId).Select(Q => Q.KodawariTypeName).FirstOrDefaultAsync();
                var getIdKodawari = insertKodawaris.KodawariId;
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "New Kodawari";
                PushNotificationMyTools.Body = "New Kodawari in " + getKodawariType + " has been added.";
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
    public async Task<bool> ValidateKodawariAsync(Guid kodawariMenuId, Guid kodawariTypeId, Guid Cms_SubContentId, Guid Cms_MenuId, Guid Cms_ContentId)
    {
      var isExist = await this
          .DB
          .Kodawaris
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.KodawariMenuId == kodawariMenuId && Q.KodawariTypeId == kodawariTypeId && Q.Cms_SubContentId == Cms_SubContentId && Q.Cms_MenuId == Cms_MenuId && Q.Cms_ContentId == Cms_ContentId);

      return isExist;
    }
    public async Task<ActionResult<BaseResponse>> UpdateKodawariAsync(KodawariUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateKodawariAsync(model.KodawariId, model.KodawariMenuId, model.KodawariTypeId, model.Cms_SubContentId.HasValue ? model.Cms_SubContentId.Value : Guid.Empty, model.Cms_MenuId, model.Cms_ContentId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var kodawariToUpdate = await this.DB.Kodawaris.Where(Q => Q.KodawariId == model.KodawariId).FirstOrDefaultAsync();

            kodawariToUpdate.KodawariId = model.KodawariId;
            kodawariToUpdate.KodawariMenuId = model.KodawariMenuId;
            kodawariToUpdate.KodawariTypeId = model.KodawariTypeId;
            kodawariToUpdate.Cms_SubContentId = model.Cms_SubContentId;
            kodawariToUpdate.Cms_MenuId = model.Cms_MenuId;
            kodawariToUpdate.Cms_ContentId = model.Cms_ContentId;
            kodawariToUpdate.KodawariSequence = model.KodawariSequence;
            kodawariToUpdate.ApprovedAt = model.ApprovedAt;
            kodawariToUpdate.UpdatedAt = thisDate;
            kodawariToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            if (model.ApprovedAt != null)
            {
                var getKodawariType = await this.DB.KodawariTypes.Where(Q => Q.KodawariTypeId == model.KodawariTypeId).Select(Q => Q.KodawariTypeName).FirstOrDefaultAsync();
                var getIdKodawari = kodawariToUpdate.KodawariId;
                var groupPositionList = new List<string>();
                var manPowerPositionList = new List<string>();
                var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                var PushNotificationMyTools = new VMPushNotificationAdd();
                PushNotificationMyTools.Title = "Kodawari Update";
                PushNotificationMyTools.Body = "Kodawari in " + getKodawariType + " has been updated.";
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
    public async Task<bool> ValidateUpdateKodawariAsync(Guid kodawariId, Guid kodawariMenuId, Guid kodawariTypeId, Guid Cms_SubContentId, Guid Cms_MenuId, Guid Cms_ContentId)
    {
      var data = await this
          .DB
          .Kodawaris
          .AsNoTracking()
          .Where(Q => Q.KodawariId == kodawariId)
          .FirstOrDefaultAsync();

      var isChange = data.Cms_ContentId != Cms_ContentId;

      var isExist = await this.ValidateKodawariAsync(kodawariMenuId, kodawariTypeId, Cms_SubContentId, Cms_MenuId, Cms_ContentId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteKodawariAsync(DeleteKodawariModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.Kodawaris.Where(Q => Q.KodawariId == model.KodawariId).FirstOrDefaultAsync();

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
