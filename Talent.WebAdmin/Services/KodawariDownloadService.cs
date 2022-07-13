using System;
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
  public class KodawariDownloadService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public KodawariDownloadService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<KodawariDownloadPaginate> GetAllKodawariDownload(KodawariDownloadGridFilter filter)
    {
      var query = (from sl in this.DB.KodawariDownloads
                   join mn in this.DB.KodawariMenus on sl.KodawariMenuId equals mn.KodawariMenuId
                   where sl.IsDeleted == false
                   select new KodawariDownloadModel
                   {
                     KodawariDownloadId = sl.KodawariDownloadId,
                     KodawariMenuId = sl.KodawariMenuId,
                     KodawariMenuName = DB.KodawariMenus.Where(x => x.KodawariMenuId == sl.KodawariMenuId).Select(x => x.KodawariMenuName).FirstOrDefault(),
                     BlobId = sl.BlobId,
                     KodawariDownloadTitle = sl.KodawariDownloadTitle,
                     IsDownloadable = sl.IsDownloadable,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.KodawariDownloadTitle))
      {
        query = query.Where(Q => Q.KodawariDownloadTitle.Contains(filter.KodawariDownloadTitle));
      }

      //sort
      switch (filter.SortBy)
      {
        case "KodawariDownloadTitleAsc":
          query = query.OrderBy(Q => Q.KodawariDownloadTitle);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "KodawariDownloadTitleDesc":
          query = query.OrderByDescending(Q => Q.KodawariDownloadTitle);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.KodawariDownloadTitle);
          break;
      }
      var kodawariDownloads = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in kodawariDownloads)
      {

        var imageURL = "";
        var fileFormat = "";

        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        fileFormat = blobData.Mime;

        kodawariDownloads[index].ImageUrl = imageURL;
        kodawariDownloads[index].FileTypeBlob = fileFormat;
        index++;
      }

      return new KodawariDownloadPaginate
      {
        KodawariDownloads = kodawariDownloads,
        TotalKodawariDownloads = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertKodawariDownloadAsync(KodawariDownloadCreateModel model)
    {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isExist = await ValidateKodawariDownloadByNameAsync(model.KodawariDownloadTitle, model.KodawariMenuId);

                if (isExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                Guid getUploadBlob = Guid.Empty;

                if (model.KodawariDownloadFileContent != null)
                {
                    if (string.IsNullOrEmpty(model.KodawariDownloadFileContent.Base64) == false)
                    {
                        getUploadBlob = await this.FileMan.UploadFileFromBase64(model.KodawariDownloadFileContent);
                    }
                }

                var newKodawariDownloads = new KodawariDownloads
                {
                    KodawariDownloadTitle = model.KodawariDownloadTitle,
                    BlobId = getUploadBlob,
                    KodawariMenuId = model.KodawariMenuId,
                    IsDownloadable = model.IsDownloadable,
                    ApprovedAt = model.ApprovedAt,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = this.HelperMan.GetLoginUserId(),
                    UpdatedBy = this.HelperMan.GetLoginUserId()
                };

                this.DB.KodawariDownloads.Add(newKodawariDownloads);

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
    }
    public async Task<bool> ValidateKodawariDownloadByNameAsync(string kodawariDownloadTitle, Guid kodawariMenuId)
    {
      var isExist = await this
          .DB
          .KodawariDownloads
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.KodawariDownloadTitle == kodawariDownloadTitle && Q.KodawariMenuId == kodawariMenuId);

      return isExist;
    }
    public async Task<KodawariDownloadModel> GetKodawariDownloadById(Guid kodawariDownloadId)
    {
      var data = await (from mo in DB.KodawariDownloads
                        join mn in this.DB.KodawariMenus on mo.KodawariMenuId equals mn.KodawariMenuId
                        where mo.KodawariDownloadId == kodawariDownloadId && mo.IsDeleted == false
                        select new KodawariDownloadModel
                        {
                          KodawariDownloadId = mo.KodawariDownloadId,
                          KodawariMenuId = mo.KodawariMenuId,
                          KodawariMenuName = DB.KodawariMenus.Where(x => x.KodawariMenuId == mo.KodawariMenuId).Select(x => x.KodawariMenuName).FirstOrDefault(),
                          BlobId = mo.BlobId,
                          KodawariDownloadTitle = mo.KodawariDownloadTitle,
                          IsDownloadable = mo.IsDownloadable,
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

      data.ImageUrl = imageURL;
      data.FileTypeBlob = fileType;

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateKodawariDownloadAsync(KodawariDownloadUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateKodawariDownloadByNameAsync(model.KodawariMenuId, model.KodawariDownloadTitle, model.KodawariDownloadId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var kodawariDownloadToUpdate = await this.DB.KodawariDownloads.Where(Q => Q.KodawariDownloadId == model.KodawariDownloadId).FirstOrDefaultAsync();

            if (model.KodawariDownloadFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.KodawariDownloadFileContent.Base64))
                {
                    kodawariDownloadToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.KodawariDownloadFileContent);
                }
            }

            kodawariDownloadToUpdate.KodawariDownloadId = model.KodawariDownloadId;
            kodawariDownloadToUpdate.KodawariMenuId = model.KodawariMenuId;
            kodawariDownloadToUpdate.KodawariDownloadTitle = model.KodawariDownloadTitle;
            kodawariDownloadToUpdate.IsDownloadable = model.IsDownloadable;
            kodawariDownloadToUpdate.ApprovedAt = model.ApprovedAt;
            kodawariDownloadToUpdate.UpdatedAt = thisDate;
            kodawariDownloadToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateKodawariDownloadByNameAsync(Guid kodawariMenuId, string kodawariDownloadTitle, Guid kodawariDownloadId)
    {
      var data = await this
          .DB
          .KodawariDownloads
          .AsNoTracking()
          .Where(Q => Q.KodawariDownloadId == kodawariDownloadId)
          .FirstOrDefaultAsync();

      var isChange = data.KodawariDownloadTitle != kodawariDownloadTitle;

      var isExist = await this.ValidateKodawariDownloadByNameAsync(kodawariDownloadTitle, kodawariMenuId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteKodawariDownloadAsync(DeleteKodawariDownloadModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.KodawariDownloads.Where(Q => Q.KodawariDownloadId == model.KodawariDownloadId).FirstOrDefaultAsync();

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
    public async Task<ActionResult<BaseResponse>> UpdateKodawariDownloadByIsDownloadableAsync(UpdateKodawariDownloadStatusDownloadModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.KodawariDownloads.Where(Q => Q.KodawariDownloadId == model.KodawariDownloadId).FirstOrDefaultAsync();

            module.IsDownloadable = model.isThisFileDownloadAble;
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
