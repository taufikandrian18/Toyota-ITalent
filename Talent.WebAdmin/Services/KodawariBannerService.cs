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
  public class KodawariBannerService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public KodawariBannerService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<KodawariBannerPaginate> GetAllKodawariBanner(KodawariBannersGridFilter filter)
    {
      var query = (from sl in this.DB.KodawariBanners
                   where sl.IsDeleted == false
                   select new KodawariBannerModel
                   {
                     KodawariBannerId = sl.KodawariBannerId,
                     BlobId = sl.BlobId,
                     KodawariBannerTitle = sl.KodawariBannerTitle,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.KodawariBannerTitle))
      {
        query = query.Where(Q => Q.KodawariBannerTitle.Contains(filter.KodawariBannerTitle));
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
      var kodawariBanners = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in kodawariBanners)
      {

        var imageURL = "";
        var fileFormat = "";

        var blobData = await this.BlobService.GetBlobById(datum.BlobId);

        imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
        fileFormat = blobData.Mime;

        kodawariBanners[index].ImageUrl = imageURL;
        kodawariBanners[index].FileTypeBlob = fileFormat;
        index++;
      }

      return new KodawariBannerPaginate
      {
        KodawariBanners = kodawariBanners,
        TotalKodawariBanners = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertKodawariBannerAsync(KodawariBannerCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateKodawariBannerByNameAsync(model.KodawariBannerTitle);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            Guid getUploadBlob = Guid.Empty;

            if (model.KodawariBannerFileContent != null)
            {
                if (string.IsNullOrEmpty(model.KodawariBannerFileContent.Base64) == false)
                {
                    getUploadBlob = await this.FileMan.UploadFileFromBase64(model.KodawariBannerFileContent);
                }
            }

            var newKodawariBanners = new KodawariBanners
            {
                KodawariBannerTitle = model.KodawariBannerTitle,
                BlobId = getUploadBlob,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                ApprovedAt = model.ApprovedAt,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.KodawariBanners.Add(newKodawariBanners);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateKodawariBannerByNameAsync(string kodawariBannerTitle)
    {
      var isExist = await this
          .DB
          .KodawariBanners
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.KodawariBannerTitle == kodawariBannerTitle);

      return isExist;
    }
    public async Task<KodawariBannerModel> GetKodawariBannerById(Guid kodawariBannerId)
    {
      var data = await (from mo in DB.KodawariBanners
                        where mo.KodawariBannerId == kodawariBannerId && mo.IsDeleted == false
                        select new KodawariBannerModel
                        {
                          KodawariBannerId = mo.KodawariBannerId,
                          BlobId = mo.BlobId,
                          KodawariBannerTitle = mo.KodawariBannerTitle,
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
    public async Task<ActionResult<BaseResponse>> UpdateKodawariBannerAsync(KodawariBannerUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateKodawariBannerByNameAsync(model.KodawariBannerTitle, model.KodawariBannerId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var kodawariBannerToUpdate = await this.DB.KodawariBanners.Where(Q => Q.KodawariBannerId == model.KodawariBannerId).FirstOrDefaultAsync();

            if (model.KodawariBannerFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.KodawariBannerFileContent.Base64))
                {
                    kodawariBannerToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.KodawariBannerFileContent);
                }
            }

            kodawariBannerToUpdate.KodawariBannerId = model.KodawariBannerId;
            kodawariBannerToUpdate.KodawariBannerTitle = model.KodawariBannerTitle;
            kodawariBannerToUpdate.ApprovedAt = model.ApprovedAt;
            kodawariBannerToUpdate.UpdatedAt = thisDate;
            kodawariBannerToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateKodawariBannerByNameAsync(string kodawariBannerTitle, Guid kodawariBannerId)
    {
      var data = await this
          .DB
          .KodawariBanners
          .AsNoTracking()
          .Where(Q => Q.KodawariBannerId == kodawariBannerId)
          .FirstOrDefaultAsync();

      var isChange = data.KodawariBannerTitle != kodawariBannerTitle;

      var isExist = await this.ValidateKodawariBannerByNameAsync(kodawariBannerTitle);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteKodawariBannerAsync(DeleteKodawariBannerModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.KodawariBanners.Where(Q => Q.KodawariBannerId == model.KodawariBannerId).FirstOrDefaultAsync();

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
