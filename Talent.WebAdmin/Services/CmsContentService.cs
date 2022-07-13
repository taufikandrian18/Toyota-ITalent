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
  public class CmsContentService
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public CmsContentService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<CmsContentPaginate> GetAllCmsContent(CmsContentGridFilter filter)
    {

      var query = (from sl in this.DB.Cms_Contents
                   where sl.IsDeleted == false
                   select new CmsContentModel
                   {
                     Cms_ContentId = sl.Cms_ContentId,
                     BlobId = sl.BlobId,
                     CmsContentVideoBlobId = sl.CmsContentVideoBlobId,
                     CmsContentFileBlobId = sl.CmsContentFileBlobId,
                     ProductId = sl.ProductId,
                     Cms_ContentName = sl.Cms_ContentName,
                     Cms_ContentDescription = sl.Cms_ContentDescription,
                     Cms_ContentCategory = sl.Cms_ContentCategory,
                     Cms_ContentSequence = sl.Cms_ContentSequence,
                     Cms_ContentIsSequence = sl.Cms_ContentIsSequence,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedAt = sl.UpdatedAt,
                     UpdatedBy = sl.UpdatedBy
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.Cms_ContentName))
      {
        query = query.Where(Q => Q.Cms_ContentName.Contains(filter.Cms_ContentName));
      }

      if (filter.ProductId != null || filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      if (!string.IsNullOrEmpty(filter.Cms_ContentCategory))
      {
        query = query.Where(Q => Q.Cms_ContentCategory == filter.Cms_ContentCategory);
      }

      //sort
      switch (filter.SortBy)
      {
        case "CmsContentNameAsc":
          query = query.OrderBy(Q => Q.Cms_ContentName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "CmsContentNameDesc":
          query = query.OrderByDescending(Q => Q.Cms_ContentName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.Cms_ContentName);
          break;
      }
      var cmsContents = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in cmsContents)
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

        if (datum.CmsContentFileBlobId != null)
        {
          fileBlobData = await this.BlobService.GetBlobById(datum.CmsContentFileBlobId);

          fileBlobUrl = await this.FileMan.GetFileAsync(fileBlobData.BlobId.ToString(), fileBlobData.Mime);
        }
        if (datum.CmsContentVideoBlobId != null)
        {
          videoBlobData = await this.BlobService.GetBlobById(datum.CmsContentVideoBlobId);

          videoBlobUrl = await this.FileMan.GetFileAsync(videoBlobData.BlobId.ToString(), videoBlobData.Mime);
        }

        cmsContents[index].BlobUrl = blobUrl;
        cmsContents[index].Blob = blobData;
        cmsContents[index].FileBlobUrl = fileBlobUrl;
        cmsContents[index].FileBlob = fileBlobData;
        cmsContents[index].VideoBlobUrl = videoBlobUrl;
        cmsContents[index].VideoBlob = videoBlobData;


        index++;
      }
      return new CmsContentPaginate
      {
        CmsContents = cmsContents,
        TotalCmsContents = totalData
      };
    }
    public async Task<Guid> InsertCmsContentAsync(CmsContentCreateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      //var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsContentPicContent);
      //var getUploadVideoBlob = await this.FileMan.UploadFileFromBase64(model.CmsContentVidContent);
      //var getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsContentFileContent);
      Guid? getUploadBlob = null;
      Guid? getUploadVideoBlob = null;
      Guid? getUploadFileBlob = null;

      if (model.CmsContentPicContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsContentPicContent.Base64) == false)
        {
          getUploadBlob = await this.FileMan.UploadFileFromBase64(model.CmsContentPicContent);
        }
      }

      if (model.CmsContentVidContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsContentVidContent.Base64) == false)
        {
          getUploadVideoBlob = await this.FileMan.UploadFileFromBase64(model.CmsContentVidContent);
        }
      }

      if (model.CmsContentFileContent != null)
      {
        if (string.IsNullOrEmpty(model.CmsContentFileContent.Base64) == false)
        {
          getUploadFileBlob = await this.FileMan.UploadFileFromBase64(model.CmsContentFileContent);
        }
      }

      var newCmsContents = new Cms_Contents
      {
        BlobId = getUploadBlob,
        CmsContentVideoBlobId = getUploadVideoBlob,
        CmsContentFileBlobId = getUploadFileBlob,
        ProductId = model.ProductId,
        Cms_ContentName = model.Cms_ContentName,
        Cms_ContentDescription = model.Cms_ContentDescription,
        Cms_ContentCategory = model.Cms_ContentCategory,
        Cms_ContentSequence = model.Cms_ContentSequence,
        Cms_ContentIsSequence = model.Cms_ContentIsSequence,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = this.HelperMan.GetLoginUserId(),
        UpdatedBy = this.HelperMan.GetLoginUserId()
      };

      this.DB.Cms_Contents.Add(newCmsContents);

      await this.DB.SaveChangesAsync();
      return newCmsContents.Cms_ContentId;
    }
    public async Task<CmsContentModel> GetCmsContentById(Guid Cms_ContentId)
    {
      var data = await (from mo in DB.Cms_Contents
                        where mo.Cms_ContentId == Cms_ContentId && mo.IsDeleted == false
                        select new CmsContentModel
                        {
                          Cms_ContentId = mo.Cms_ContentId,
                          BlobId = mo.BlobId,
                          CmsContentVideoBlobId = mo.CmsContentVideoBlobId,
                          CmsContentFileBlobId = mo.CmsContentFileBlobId,
                          ProductId = mo.ProductId,
                          Cms_ContentName = mo.Cms_ContentName,
                          Cms_ContentDescription = mo.Cms_ContentDescription,
                          Cms_ContentCategory = mo.Cms_ContentCategory,
                          Cms_ContentSequence = mo.Cms_ContentSequence,
                          Cms_ContentIsSequence = mo.Cms_ContentIsSequence,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedAt = mo.UpdatedAt,
                          UpdatedBy = mo.UpdatedBy
                        }).FirstOrDefaultAsync();

      var blobUrl = "";
      var blobData = new BlobModel();
      var fileBlobUrl = "";
      var fileBlobData = new BlobModel();
      var videoBlobUrl = "";
      var videoBlobData = new BlobModel();

      if (data.BlobId != null)
      {
        blobData = await this.BlobService.GetBlobById(data.BlobId);

        blobUrl = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
      }

      if (data.CmsContentFileBlobId != null)
      {
        fileBlobData = await this.BlobService.GetBlobById(data.CmsContentFileBlobId);

        fileBlobUrl = await this.FileMan.GetFileAsync(fileBlobData.BlobId.ToString(), fileBlobData.Mime);
      }
      if (data.CmsContentVideoBlobId != null)
      {
        videoBlobData = await this.BlobService.GetBlobById(data.CmsContentVideoBlobId);

        videoBlobUrl = await this.FileMan.GetFileAsync(videoBlobData.BlobId.ToString(), videoBlobData.Mime);
      }

      data.BlobUrl = blobUrl;
      data.Blob = blobData;
      data.FileBlobUrl = fileBlobUrl;
      data.FileBlob = fileBlobData;
      data.VideoBlobUrl = videoBlobUrl;
      data.VideoBlob = videoBlobData;

      return data;
    }
    public async Task<bool> UpdateCmsContentAsync(CmsContentUpdateModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var cmsContentToUpdate = await this.DB.Cms_Contents.Where(Q => Q.Cms_ContentId == model.Cms_ContentId).FirstOrDefaultAsync();

      if (model.CmsContentPicContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsContentPicContent.Base64))
        {
          cmsContentToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.CmsContentPicContent);
        }
        else if (model.isDeletePic == true)
        {
          cmsContentToUpdate.BlobId = (Guid?)null;
        }
      }

      if (model.CmsContentVidContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsContentVidContent.Base64))
        {
          cmsContentToUpdate.CmsContentVideoBlobId = await this.FileMan.UploadFileFromBase64(model.CmsContentVidContent);
        }
        else if (model.isDeleteVid == true)
        {
          cmsContentToUpdate.CmsContentVideoBlobId = (Guid?)null;
        }
      }

      if (model.CmsContentFileContent != null)
      {
        if (!String.IsNullOrEmpty(model.CmsContentFileContent.Base64))
        {
          cmsContentToUpdate.CmsContentFileBlobId = await this.FileMan.UploadFileFromBase64(model.CmsContentFileContent);
        }
        else if (model.isDeleteFile == true)
        {
          cmsContentToUpdate.CmsContentFileBlobId = (Guid?)null;
        }
      }

      cmsContentToUpdate.Cms_ContentId = model.Cms_ContentId;
      cmsContentToUpdate.Cms_ContentName = model.Cms_ContentName;
      cmsContentToUpdate.Cms_ContentDescription = model.Cms_ContentDescription;
      cmsContentToUpdate.Cms_ContentCategory = model.Cms_ContentCategory;
      cmsContentToUpdate.Cms_ContentSequence = model.Cms_ContentSequence;
      cmsContentToUpdate.Cms_ContentIsSequence = model.Cms_ContentIsSequence;
      cmsContentToUpdate.ProductId = model.ProductId;
      cmsContentToUpdate.UpdatedAt = thisDate;
      cmsContentToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task DeleteCmsContentAsync(DeleteCmsContentModel model)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var module = await this.DB.Cms_Contents.Where(Q => Q.Cms_ContentId == model.Cms_ContentId).FirstOrDefaultAsync();

      module.IsDeleted = true;
      module.UpdatedAt = thisDate;
      module.UpdatedBy = this.HelperMan.GetLoginUserId();


      await this.DB.SaveChangesAsync();
      return;
    }
  }
}
