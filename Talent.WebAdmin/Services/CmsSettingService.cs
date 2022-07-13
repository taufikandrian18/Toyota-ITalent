using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
  public class CmsSettingService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public CmsSettingService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
      this.FileMan = fm;
      this.BlobService = bs;
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
    public async Task<CmsSettingPaginate> GetAllCmsSetting(CmsSettingGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_Settings
                   where sl.IsDeleted == false
                   select new CmsSettingModel
                   {
                     Cms_SettingId = sl.Cms_SettingId,
                     BlobId = sl.BlobId,
                     Cms_SettingFileBlobId = sl.Cms_SettingFileBlobId,
                     Cms_SettingDescription = sl.Cms_SettingDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     UpdatedBy = sl.UpdatedBy
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_SettingDescription))
      {
        query = query.Where(Q => Q.Cms_SettingDescription == filter.Cms_SettingDescription);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsSettingDescriptionAsc":
          query = query.OrderBy(Q => Q.Cms_SettingDescription);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsSettingDescriptionDesc":
          query = query.OrderByDescending(Q => Q.Cms_SettingDescription);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_SettingDescription);
          break;
      }
      var cmsSettings = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new CmsSettingPaginate
      {
        CmsSettings = cmsSettings,
        TotalCmsSettings = totalData
      };
    }
    public async Task<Guid> InsertCmsSettingAsync(CmsSettingCreateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      //var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsSettingPicContent);
      //var getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsSettingFileContent);

      Guid? getUploadBlob = null;
      Guid? getUploadFileBlob = null;

      if (model.CmsSettingPicContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsSettingPicContent.Base64) == false)
        {
          getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsSettingPicContent);
        }
      }

      if (model.CmsSettingFileContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsSettingFileContent.Base64) == false)
        {
          getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsSettingFileContent);
        }
      }

      var newCmsSettings = new Cms_Settings
      {
        BlobId = getUploadBlob,
        Cms_SettingFileBlobId = getUploadFileBlob,
        Cms_SettingDescription = model.Cms_SettingDescription,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = this.HelperMan.GetLoginUserId(),
        UpdatedBy = this.HelperMan.GetLoginUserId()
      };

      this.DB.Cms_Settings.Add(newCmsSettings);

      await this.DB.SaveChangesAsync();
      return newCmsSettings.Cms_SettingId;
    }
    public async Task<CmsSettingModel> GetCmsSettingById(Guid Cms_SettingId)
    {
      var data = await (from mo in DB.Cms_Settings
                        where mo.Cms_SettingId == Cms_SettingId && mo.IsDeleted == false
                        select new CmsSettingModel
                        {
                          Cms_SettingId = mo.Cms_SettingId,
                          BlobId = mo.BlobId,
                          Cms_SettingFileBlobId = mo.Cms_SettingFileBlobId,
                          Cms_SettingDescription = mo.Cms_SettingDescription,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      var blobUrl = "";

      var blobData = new BlobModel();
      var fileBlobUrl = "";
      var fileBlobData = new BlobModel();

      if (data.BlobId != null)
      {
        blobData = await this.BlobService.GetBlobById(data.BlobId);

        blobUrl = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
      }

      if (data.Cms_SettingFileBlobId != null)
      {
        fileBlobData = await this.BlobService.GetBlobById(data.Cms_SettingFileBlobId);

        fileBlobUrl = await this.FileMan.GetFileAsync(fileBlobData.BlobId.ToString(), fileBlobData.Mime);
      }

      data.BlobUrl = blobUrl;
      data.Blob = blobData;
      data.FileBlobUrl = fileBlobUrl;
      data.FileBlob = fileBlobData;

      return data;
    }
    public async Task<bool> UpdateCmsSettingAsync(CmsSettingUpdateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var cmsSettingToUpdate = await this.DB.Cms_Settings.Where(Q => Q.Cms_SettingId == model.Cms_SettingId).FirstOrDefaultAsync();

      if (model.CmsSettingPicContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsSettingPicContent.Base64))
        {
          cmsSettingToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.CmsSettingPicContent);
        }
        else if (model.isDeletePic == true)
        {
          cmsSettingToUpdate.BlobId = (Guid?)null;
        }
      }

      if (model.CmsSettingFileContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsSettingFileContent.Base64))
        {
          cmsSettingToUpdate.Cms_SettingFileBlobId = await this.FileMan.UploadFileFromBase64(model.CmsSettingFileContent);
        }
        else if (model.isDeleteFile == true)
        {
          cmsSettingToUpdate.Cms_SettingFileBlobId = (Guid?)null;
        }
      }

      cmsSettingToUpdate.Cms_SettingId = model.Cms_SettingId;
      cmsSettingToUpdate.Cms_SettingDescription = model.Cms_SettingDescription;
      cmsSettingToUpdate.UpdatedAt = thisDate;
      cmsSettingToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task DeleteCmsSettingAsync(DeleteCmsSettingModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var module = await this.DB.Cms_Settings.Where(Q => Q.Cms_SettingId == model.Cms_SettingId).FirstOrDefaultAsync();

      module.IsDeleted = true;
      module.UpdatedAt = thisDate;
      module.UpdatedBy = this.HelperMan.GetLoginUserId();


      await this.DB.SaveChangesAsync();
      return;
    }
  }
}
