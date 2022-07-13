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
  public class CmsFmbService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public CmsFmbService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<CmsFmbPaginate> GetAllCmsFmb(CmsFmbGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_Fmbs
                   where sl.IsDeleted == false
                   select new CmsFmbModel
                   {
                     Cms_FmbId = sl.Cms_FmbId,
                     BlobId = sl.BlobId,
                     CmsFmbFileBlobId = sl.CmsFmbFileBlobId,
                     Cms_FmbDescription = sl.Cms_FmbDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     UpdatedBy = sl.UpdatedBy
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_FmbDescription))
      {
        query = query.Where(Q => Q.Cms_FmbDescription == filter.Cms_FmbDescription);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsFmbDescriptionAsc":
          query = query.OrderBy(Q => Q.Cms_FmbDescription);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsFmbDescriptionDesc":
          query = query.OrderByDescending(Q => Q.Cms_FmbDescription);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_FmbDescription);
          break;
      }
      var cmsFmbs = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new CmsFmbPaginate
      {
        CmsFmbs = cmsFmbs,
        TotalCmsFmbs = totalData
      };
    }
    public async Task<Guid> InsertCmsFmbAsync(CmsFmbCreateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      //var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsFmbPicContent);
      //var getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsFmbFileContent);

      Guid? getUploadBlob = null;
      Guid? getUploadFileBlob = null;

      if (model.CmsFmbPicContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsFmbPicContent.Base64) == false)
        {
          getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsFmbPicContent);
        }
      }

      if (model.CmsFmbFileContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsFmbFileContent.Base64) == false)
        {
          getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsFmbFileContent);
        }
      }

      var newCmsFmbs = new Cms_Fmbs
      {
        BlobId = getUploadBlob,
        CmsFmbFileBlobId = getUploadFileBlob,
        Cms_FmbDescription = model.Cms_FmbDescription,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = this.HelperMan.GetLoginUserId(),
        UpdatedBy = this.HelperMan.GetLoginUserId()
      };

      this.DB.Cms_Fmbs.Add(newCmsFmbs);

      await this.DB.SaveChangesAsync();
      return newCmsFmbs.Cms_FmbId;
    }
    public async Task<CmsFmbModel> GetCmsFmbById(Guid Cms_FmbId)
    {
      var data = await (from mo in DB.Cms_Fmbs
                        where mo.Cms_FmbId == Cms_FmbId && mo.IsDeleted == false
                        select new CmsFmbModel
                        {
                          Cms_FmbId = mo.Cms_FmbId,
                          BlobId = mo.BlobId,
                          CmsFmbFileBlobId = mo.CmsFmbFileBlobId,
                          Cms_FmbDescription = mo.Cms_FmbDescription,
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

      if (data.CmsFmbFileBlobId != null)
      {
        fileBlobData = await this.BlobService.GetBlobById(data.CmsFmbFileBlobId);

        fileBlobUrl = await this.FileMan.GetFileAsync(fileBlobData.BlobId.ToString(), fileBlobData.Mime);
      }

      data.BlobUrl = blobUrl;
      data.Blob = blobData;
      data.FileBlobUrl = fileBlobUrl;
      data.FileBlob = fileBlobData;

      return data;
    }

    public async Task<bool> UpdateCmsFmbAsync(CmsFmbUpdateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var cmsFmbToUpdate = await this.DB.Cms_Fmbs.Where(Q => Q.Cms_FmbId == model.Cms_FmbId).FirstOrDefaultAsync();

      if (model.CmsFmbPicContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsFmbPicContent.Base64))
        {
          cmsFmbToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.CmsFmbPicContent);
        }
        else if (model.isDeletePic == true)
        {
          cmsFmbToUpdate.BlobId = (Guid?)null;
        }
      }

      if (model.CmsFmbFileContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsFmbFileContent.Base64))
        {
          cmsFmbToUpdate.CmsFmbFileBlobId = await this.FileMan.UploadFileFromBase64(model.CmsFmbFileContent);
        }
        else if (model.isDeleteFile == true)
        {
          cmsFmbToUpdate.CmsFmbFileBlobId = (Guid?)null;
        }
      }

      cmsFmbToUpdate.Cms_FmbId = model.Cms_FmbId;
      cmsFmbToUpdate.Cms_FmbDescription = model.Cms_FmbDescription;
      cmsFmbToUpdate.UpdatedAt = thisDate;
      cmsFmbToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task DeleteCmsFmbAsync(DeleteCmsFmbModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var module = await this.DB.Cms_Fmbs.Where(Q => Q.Cms_FmbId == model.Cms_FmbId).FirstOrDefaultAsync();

      module.IsDeleted = true;
      module.UpdatedAt = thisDate;
      module.UpdatedBy = this.HelperMan.GetLoginUserId();


      await this.DB.SaveChangesAsync();
      return;
    }
  }
}
