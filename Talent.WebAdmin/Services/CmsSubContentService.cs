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
  public class CmsSubContentService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public CmsSubContentService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<CmsSubContentPaginate> GetAllCmsSubContent(CmsSubContentGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_SubContents
                   where sl.IsDeleted == false
                   select new CmsSubContentModel
                   {
                     Cms_SubContentId = sl.Cms_SubContentId,
                     BlobId = sl.BlobId,
                     CmsSubContentVideoBlobId = sl.CmsSubContentVideoBlobId,
                     CmsSubContentFileBlobId = sl.CmsSubContentFileBlobId,
                     Cms_SubContentName = sl.Cms_SubContentName,
                     Cms_SubContentDescription = sl.Cms_SubContentDescription,
                     Cms_SubContentCategory = sl.Cms_SubContentCategory,
                     Cms_SubContentSequence = sl.Cms_SubContentSequence,
                     Cms_SubContentIsSequence = sl.Cms_SubContentIsSequence,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     UpdatedBy = sl.UpdatedBy
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_SubContentName))
      {
        query = query.Where(Q => Q.Cms_SubContentName.Contains(filter.Cms_SubContentName));
      }

      if (!string.IsNullOrEmpty(filter.Cms_SubContentCategory))
      {
        query = query.Where(Q => Q.Cms_SubContentCategory == filter.Cms_SubContentCategory);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsSubContentNameAsc":
          query = query.OrderBy(Q => Q.Cms_SubContentName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsSubContentNameDesc":
          query = query.OrderByDescending(Q => Q.Cms_SubContentName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_SubContentName);
          break;
      }
      var cmsSubContents = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in cmsSubContents)
      {

        var blobUrl = "";
        var blobData = new BlobModel();
        var fileBlobUrl = "";
        var fileBlobData = new BlobModel();
        var videoBlobUrl = "";
        var videoBlobData = new BlobModel();

        if (datum.BlobId != null)
        {
          blobData = await this.BlobService.GetBlobById(datum.BlobId);

          blobUrl = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        }

        if (datum.CmsSubContentFileBlobId != null)
        {
          fileBlobData = await this.BlobService.GetBlobById(datum.CmsSubContentFileBlobId);

          fileBlobUrl = await this.FileMan.GetFileAsync(videoBlobData.BlobId.ToString(), videoBlobData.Mime);
        }
        if (datum.CmsSubContentVideoBlobId != null)
        {
          videoBlobData = await this.BlobService.GetBlobById(datum.CmsSubContentVideoBlobId);

          videoBlobUrl = await this.FileMan.GetFileAsync(videoBlobData.BlobId.ToString(), videoBlobData.Mime);
        }

        cmsSubContents[index].BlobUrl = blobUrl;
        cmsSubContents[index].Blob = blobData;
        cmsSubContents[index].FileBlobUrl = fileBlobUrl;
        cmsSubContents[index].FileBlob = fileBlobData;
        cmsSubContents[index].VideoBlobUrl = videoBlobUrl;
        cmsSubContents[index].VideoBlob = videoBlobData;


        index++;
      }


      return new CmsSubContentPaginate
      {
        CmsSubContents = cmsSubContents,
        TotalCmsSubContents = totalData
      };
    }
    public async Task<bool> InsertCmsSubContentAsync(CmsSubContentCreateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      //var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsSubContentPicContent);
      //var getUploadVideoBlob = await this.FileMan.UploadFileFromBase64(model.CmsSubContentVidContent);
      //var getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsSubContentFileContent);

      Guid? getUploadBlob = null;
      Guid? getUploadVideoBlob = null;
      Guid? getUploadFileBlob = null;

      if (model.CmsSubContentPicContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsSubContentPicContent.Base64) == false)
        {
          getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsSubContentPicContent);
        }
      }

      if (model.CmsSubContentVidContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsSubContentVidContent.Base64) == false)
        {
          getUploadVideoBlob = await this.FileMan.UploadFileFromBase64(model.CmsSubContentVidContent);
        }
      }

      if (model.CmsSubContentFileContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsSubContentFileContent.Base64) == false)
        {
          getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsSubContentFileContent);
        }
      }

      var newCmsSubContents = new Cms_SubContents
      {
        BlobId = getUploadBlob,
        CmsSubContentVideoBlobId = getUploadVideoBlob,
        CmsSubContentFileBlobId = getUploadFileBlob,
        Cms_SubContentName = model.Cms_SubContentName,
        Cms_SubContentDescription = model.Cms_SubContentDescription,
        Cms_SubContentCategory = model.Cms_SubContentCategory,
        Cms_SubContentSequence = model.Cms_SubContentSequence,
        Cms_SubContentIsSequence = model.Cms_SubContentIsSequence,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = this.HelperMan.GetLoginUserId(),
        UpdatedBy = this.HelperMan.GetLoginUserId()
      };

      this.DB.Cms_SubContents.Add(newCmsSubContents);

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task<CmsSubContentModel> GetCmsSubContentById(Guid Cms_SubContentId)
    {
      var data = await (from mo in DB.Cms_SubContents
                        where mo.Cms_SubContentId == Cms_SubContentId && mo.IsDeleted == false
                        select new CmsSubContentModel
                        {
                          Cms_SubContentId = mo.Cms_SubContentId,
                          BlobId = mo.BlobId,
                          CmsSubContentVideoBlobId = mo.CmsSubContentVideoBlobId,
                          CmsSubContentFileBlobId = mo.CmsSubContentFileBlobId,
                          Cms_SubContentName = mo.Cms_SubContentName,
                          Cms_SubContentDescription = mo.Cms_SubContentDescription,
                          Cms_SubContentCategory = mo.Cms_SubContentCategory,
                          Cms_SubContentSequence = mo.Cms_SubContentSequence,
                          Cms_SubContentIsSequence = mo.Cms_SubContentIsSequence,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<bool> UpdateCmsSubContentAsync(CmsSubContentUpdateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var cmsSubContentToUpdate = await this.DB.Cms_SubContents.Where(Q => Q.Cms_SubContentId == model.Cms_SubContentId).FirstOrDefaultAsync();

      if (model.CmsSubContentPicContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsSubContentPicContent.Base64))
        {
          cmsSubContentToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.CmsSubContentPicContent);
        }
        else if (model.isDeletePic == true)
        {
          cmsSubContentToUpdate.BlobId = (Guid?)null;
        }
      }

      if (model.CmsSubContentVidContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsSubContentVidContent.Base64))
        {
          cmsSubContentToUpdate.CmsSubContentVideoBlobId = await this.FileMan.UploadFileFromBase64(model.CmsSubContentVidContent);
        }
        else if (model.isDeleteVid == true)
        {
          cmsSubContentToUpdate.CmsSubContentVideoBlobId = (Guid?)null;
        }
        else if (model.isDeleteFile == true)
        {
          cmsSubContentToUpdate.CmsSubContentFileBlobId = (Guid?)null;
        }
      }

      if (model.CmsSubContentFileContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsSubContentFileContent.Base64))
        {
          cmsSubContentToUpdate.CmsSubContentFileBlobId = await this.FileMan.UploadFileFromBase64(model.CmsSubContentFileContent);
        }
      }



      cmsSubContentToUpdate.Cms_SubContentId = model.Cms_SubContentId;
      cmsSubContentToUpdate.Cms_SubContentName = model.Cms_SubContentName;
      cmsSubContentToUpdate.Cms_SubContentDescription = model.Cms_SubContentDescription;
      cmsSubContentToUpdate.Cms_SubContentCategory = model.Cms_SubContentCategory;
      cmsSubContentToUpdate.Cms_SubContentSequence = model.Cms_SubContentSequence;
      cmsSubContentToUpdate.Cms_SubContentIsSequence = model.Cms_SubContentIsSequence;
      cmsSubContentToUpdate.UpdatedAt = thisDate;
      cmsSubContentToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task DeleteCmsSubContentAsync(DeleteCmsSubContentModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var module = await this.DB.Cms_SubContents.Where(Q => Q.Cms_SubContentId == model.Cms_SubContentId).FirstOrDefaultAsync();

      module.IsDeleted = true;
      module.UpdatedAt = thisDate;
      module.UpdatedBy = this.HelperMan.GetLoginUserId();


      await this.DB.SaveChangesAsync();
      return;
    }
  }
}
