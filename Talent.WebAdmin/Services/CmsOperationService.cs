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
  public class CmsOperationService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public CmsOperationService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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

    public async Task<CmsOperationPaginate> GetAllCmsOperation(CmsOperationGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_Operations
                   where sl.IsDeleted == false
                   select new CmsOperationModel
                   {
                     Cms_OperationId = sl.Cms_OperationId,
                     BlobId = sl.BlobId,
                     Cms_OperationFileBlobId = sl.Cms_OperationFileBlobId,
                     Cms_OperationDescription = sl.Cms_OperationDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     UpdatedBy = sl.UpdatedBy
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_OperationDescription))
      {
        query = query.Where(Q => Q.Cms_OperationDescription == filter.Cms_OperationDescription);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsOperationDescriptionAsc":
          query = query.OrderBy(Q => Q.Cms_OperationDescription);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsOperationDescriptionDesc":
          query = query.OrderByDescending(Q => Q.Cms_OperationDescription);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_OperationDescription);
          break;
      }
      var cmsOperations = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new CmsOperationPaginate
      {
        CmsOperations = cmsOperations,
        TotalCmsOperations = totalData
      };
    }
    public async Task<Guid> InsertCmsOperationAsync(CmsOperationCreateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      //var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsOperationPicContent);
      //var getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsOperationFileContent);

      Guid? getUploadBlob = null;
      Guid? getUploadFileBlob = null;

      if (model.CmsOperationPicContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsOperationPicContent.Base64) == false)
        {
          getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsOperationPicContent);
        }
      }

      if (model.CmsOperationFileContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsOperationFileContent.Base64) == false)
        {
          getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsOperationFileContent);
        }
      }

      var newCmsOperations = new Cms_Operations
      {
        BlobId = getUploadBlob,
        Cms_OperationFileBlobId = getUploadFileBlob,
        Cms_OperationDescription = model.Cms_OperationDescription,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = this.HelperMan.GetLoginUserId(),
        UpdatedBy = this.HelperMan.GetLoginUserId()
      };

      this.DB.Cms_Operations.Add(newCmsOperations);

      await this.DB.SaveChangesAsync();
      return newCmsOperations.Cms_OperationId;
    }
    public async Task<CmsOperationModel> GetCmsOperationById(Guid Cms_OperationId)
    {
      var data = await (from mo in DB.Cms_Operations
                        where mo.Cms_OperationId == Cms_OperationId && mo.IsDeleted == false
                        select new CmsOperationModel
                        {
                          Cms_OperationId = mo.Cms_OperationId,
                          BlobId = mo.BlobId,
                          Cms_OperationFileBlobId = mo.Cms_OperationFileBlobId,
                          Cms_OperationDescription = mo.Cms_OperationDescription,
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

      if (data.Cms_OperationFileBlobId != null)
      {
        fileBlobData = await this.BlobService.GetBlobById(data.Cms_OperationFileBlobId);

        fileBlobUrl = await this.FileMan.GetFileAsync(fileBlobData.BlobId.ToString(), fileBlobData.Mime);
      }

      data.BlobUrl = blobUrl;
      data.Blob = blobData;
      data.FileBlobUrl = fileBlobUrl;
      data.FileBlob = fileBlobData;

      return data;
    }
    public async Task<bool> UpdateCmsOperationAsync(CmsOperationUpdateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var cmsOperationToUpdate = await this.DB.Cms_Operations.Where(Q => Q.Cms_OperationId == model.Cms_OperationId).FirstOrDefaultAsync();

      if (model.CmsOperationPicContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsOperationPicContent.Base64))
        {
          cmsOperationToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.CmsOperationPicContent);
        }
        else if (model.isDeletePic == true)
        {
          cmsOperationToUpdate.BlobId = (Guid?)null;
        }
      }
      if (model.CmsOperationFileContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsOperationFileContent.Base64))
        {
          cmsOperationToUpdate.Cms_OperationFileBlobId = await this.FileMan.UploadFileFromBase64(model.CmsOperationFileContent);
        }
        else if (model.isDeleteFile == true)
        {
          cmsOperationToUpdate.Cms_OperationFileBlobId = (Guid?)null;
        }
      }

      cmsOperationToUpdate.Cms_OperationId = model.Cms_OperationId;
      cmsOperationToUpdate.Cms_OperationDescription = model.Cms_OperationDescription;
      cmsOperationToUpdate.UpdatedAt = thisDate;
      cmsOperationToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task DeleteCmsOperationAsync(DeleteCmsOperationModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var module = await this.DB.Cms_Operations.Where(Q => Q.Cms_OperationId == model.Cms_OperationId).FirstOrDefaultAsync();

      module.IsDeleted = true;
      module.UpdatedAt = thisDate;
      module.UpdatedBy = this.HelperMan.GetLoginUserId();


      await this.DB.SaveChangesAsync();
      return;
    }
  }
}
