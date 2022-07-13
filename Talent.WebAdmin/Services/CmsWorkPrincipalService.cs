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
  public class CmsWorkPrincipalService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public CmsWorkPrincipalService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<CmsWorkPrincipalPaginate> GetAllCmsWorkPrincipal(CmsWorkPrincipalGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_WorkPrincipals
                   where sl.IsDeleted == false
                   select new CmsWorkPrincipalModel
                   {
                     Cms_WorkPrincipalId = sl.Cms_WorkPrincipalId,
                     BlobId = sl.BlobId,
                     Cms_WorkPrincipalFileBlobId = sl.Cms_WorkPrincipalFileBlobId,
                     Cms_WorkPrincipalDescription = sl.Cms_WorkPrincipalDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     UpdatedBy = sl.UpdatedBy
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_WorkPrincipalDescription))
      {
        query = query.Where(Q => Q.Cms_WorkPrincipalDescription == filter.Cms_WorkPrincipalDescription);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsWorkPrincipalDescriptionAsc":
          query = query.OrderBy(Q => Q.Cms_WorkPrincipalDescription);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsWorkPrincipalDescriptionDesc":
          query = query.OrderByDescending(Q => Q.Cms_WorkPrincipalDescription);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_WorkPrincipalDescription);
          break;
      }
      var cmsWorkPrincipals = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new CmsWorkPrincipalPaginate
      {
        CmsWorkPrincipals = cmsWorkPrincipals,
        TotalCmsWorkPrincipals = totalData
      };
    }
    public async Task<Guid> InsertCmsWorkPrincipalAsync(CmsWorkPrincipalCreateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      //var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsWorkPrincipalPicContent);
      //var getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsWorkPrincipalFileContent);

      Guid? getUploadBlob = null;
      Guid? getUploadFileBlob = null;

      if (model.CmsWorkPrincipalPicContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsWorkPrincipalPicContent.Base64) == false)
        {
          getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsWorkPrincipalPicContent);
        }
      }

      if (model.CmsWorkPrincipalFileContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsWorkPrincipalFileContent.Base64) == false)
        {
          getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsWorkPrincipalFileContent);
        }
      }

      var newCmsWorkPrincipals = new Cms_WorkPrincipals
      {
        BlobId = getUploadBlob,
        Cms_WorkPrincipalFileBlobId = getUploadFileBlob,
        Cms_WorkPrincipalDescription = model.Cms_WorkPrincipalDescription,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = this.HelperMan.GetLoginUserId(),
        UpdatedBy = this.HelperMan.GetLoginUserId()
      };

      this.DB.Cms_WorkPrincipals.Add(newCmsWorkPrincipals);

      await this.DB.SaveChangesAsync();
      return newCmsWorkPrincipals.Cms_WorkPrincipalId;
    }
    public async Task<CmsWorkPrincipalModel> GetCmsWorkPrincipalById(Guid Cms_WorkPrincipalId)
    {
      var data = await (from mo in DB.Cms_WorkPrincipals
                        where mo.Cms_WorkPrincipalId == Cms_WorkPrincipalId && mo.IsDeleted == false
                        select new CmsWorkPrincipalModel
                        {
                          Cms_WorkPrincipalId = mo.Cms_WorkPrincipalId,
                          BlobId = mo.BlobId,
                          Cms_WorkPrincipalFileBlobId = mo.Cms_WorkPrincipalFileBlobId,
                          Cms_WorkPrincipalDescription = mo.Cms_WorkPrincipalDescription,
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

      if (data.Cms_WorkPrincipalFileBlobId != null)
      {
        fileBlobData = await this.BlobService.GetBlobById(data.Cms_WorkPrincipalFileBlobId);

        fileBlobUrl = await this.FileMan.GetFileAsync(fileBlobData.BlobId.ToString(), fileBlobData.Mime);
      }

      data.BlobUrl = blobUrl;
      data.Blob = blobData;
      data.FileBlobUrl = fileBlobUrl;
      data.FileBlob = fileBlobData;

      return data;
    }

    public async Task<bool> UpdateCmsWorkPrincipalAsync(CmsWorkPrincipalUpdateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var cmsWorkPrincipalToUpdate = await this.DB.Cms_WorkPrincipals.Where(Q => Q.Cms_WorkPrincipalId == model.Cms_WorkPrincipalId).FirstOrDefaultAsync();

      if (model.CmsWorkPrincipalPicContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsWorkPrincipalPicContent.Base64))
        {
          cmsWorkPrincipalToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.CmsWorkPrincipalPicContent);
        }
        else if (model.isDeletePic == true)
        {
          cmsWorkPrincipalToUpdate.BlobId = (Guid?)null;
        }
      }

      if (model.CmsWorkPrincipalFileContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsWorkPrincipalFileContent.Base64))
        {
          cmsWorkPrincipalToUpdate.Cms_WorkPrincipalFileBlobId = await this.FileMan.UploadFileFromBase64(model.CmsWorkPrincipalFileContent);
        }
        else if (model.isDeleteFile == true)
        {
          cmsWorkPrincipalToUpdate.Cms_WorkPrincipalFileBlobId = (Guid?)null;
        }
      }

      cmsWorkPrincipalToUpdate.Cms_WorkPrincipalId = model.Cms_WorkPrincipalId;
      cmsWorkPrincipalToUpdate.Cms_WorkPrincipalDescription = model.Cms_WorkPrincipalDescription;
      cmsWorkPrincipalToUpdate.UpdatedAt = thisDate;
      cmsWorkPrincipalToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task DeleteCmsWorkPrincipalAsync(DeleteCmsWorkPrincipalModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var module = await this.DB.Cms_WorkPrincipals.Where(Q => Q.Cms_WorkPrincipalId == model.Cms_WorkPrincipalId).FirstOrDefaultAsync();

      module.IsDeleted = true;
      module.UpdatedAt = thisDate;
      module.UpdatedBy = this.HelperMan.GetLoginUserId();


      await this.DB.SaveChangesAsync();
      return;
    }
  }
}
