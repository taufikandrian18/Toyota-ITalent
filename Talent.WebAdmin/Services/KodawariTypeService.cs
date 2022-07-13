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
  public class KodawariTypeService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public KodawariTypeService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<KodawariTypePaginate> GetAllKodawariType(KodawariTypeGridFilter filter)
    {
      var query = (from sl in this.DB.KodawariTypes
                   where sl.IsDeleted == false
                   select new KodawariTypeModel
                   {
                     KodawariTypeId = sl.KodawariTypeId,
                     BlobId = sl.BlobId,
                     KodawariTypeName = sl.KodawariTypeName,
                     KodawariTypeDescription = sl.KodawariTypeDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.KodawariTypeName))
      {
        query = query.Where(Q => Q.KodawariTypeName.Contains(filter.KodawariTypeName));
      }

      //sort
      switch (filter.SortBy)
      {
        case "KodawariTypeNameAsc":
          query = query.OrderBy(Q => Q.KodawariTypeName);
          break;
        case "CreatedAtAsc":
          query = query.OrderBy(Q => Q.CreatedAt);
          break;
        case "UpdatedAtAsc":
          query = query.OrderBy(Q => Q.UpdatedAt);
          break;
        case "KodawariTypeNameDesc":
          query = query.OrderByDescending(Q => Q.KodawariTypeName);
          break;
        case "CreatedAtDesc":
          query = query.OrderByDescending(Q => Q.CreatedAt);
          break;
        case "UpdatedAtDesc":
          query = query.OrderByDescending(Q => Q.UpdatedAt);
          break;
        default:
          query = query.OrderBy(Q => Q.KodawariTypeName);
          break;
      }
      var kodawariTypes = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in kodawariTypes)
      {

        var imageURL = "";
        var fileFormat = "";
        if (datum.BlobId != null)
        {
          var blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
          fileFormat = blobData.Mime;
        }

        kodawariTypes[index].ImageUrl = imageURL;
        kodawariTypes[index].FileTypeBlob = fileFormat;
        index++;
      }

      return new KodawariTypePaginate
      {
        KodawariTypes = kodawariTypes,
        TotalKodawariTypes = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertKodawariTypeAsync(KodawariTypeCreateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isExist = await ValidateKodawariTypeByNameAsync(model.KodawariTypeName);

        if (isExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        Guid? getUploadBlob = null;

        if (model.KodawariTypeFileContent != null)
        {
          if (string.IsNullOrEmpty(model.KodawariTypeFileContent.Base64) == false)
          {
            getUploadBlob = await this.FileMan.UploadFileFromBase64(model.KodawariTypeFileContent);
          }
        }

        var newKodawariTypes = new KodawariTypes
        {
          KodawariTypeName = model.KodawariTypeName,
          KodawariTypeDescription = model.KodawariTypeDescription,
          BlobId = getUploadBlob,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = this.HelperMan.GetLoginUserId(),
          UpdatedBy = this.HelperMan.GetLoginUserId()
        };

        this.DB.KodawariTypes.Add(newKodawariTypes);

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateKodawariTypeByNameAsync(string kodawariTypeName)
    {
      var isExist = await this
          .DB
          .KodawariTypes
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.KodawariTypeName == kodawariTypeName);

      return isExist;
    }
    public async Task<KodawariTypeModel> GetKodawariTypeById(Guid kodawariTypeId)
    {
      var data = await (from mo in DB.KodawariTypes
                        where mo.KodawariTypeId == kodawariTypeId && mo.IsDeleted == false
                        select new KodawariTypeModel
                        {
                          KodawariTypeId = mo.KodawariTypeId,
                          BlobId = mo.BlobId,
                          KodawariTypeName = mo.KodawariTypeName,
                          KodawariTypeDescription = mo.KodawariTypeDescription,
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
    public async Task<ActionResult<BaseResponse>> UpdateKodawariTypeAsync(KodawariTypeUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var isChangedAndExist = await ValidateUpdateKodawariTypeByNameAsync(model.KodawariTypeName, model.KodawariTypeId);

        if (isChangedAndExist)
        {
          return BadRequest(BaseResponse.BadRequest(model));
        }

        var kodawariTypeToUpdate = await this.DB.KodawariTypes.Where(Q => Q.KodawariTypeId == model.KodawariTypeId).FirstOrDefaultAsync();

        if (model.KodawariTypeFileContent != null)
        {
          if (!String.IsNullOrEmpty(model.KodawariTypeFileContent.Base64))
          {
            kodawariTypeToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.KodawariTypeFileContent);
          }
        }

        kodawariTypeToUpdate.KodawariTypeId = model.KodawariTypeId;
        kodawariTypeToUpdate.KodawariTypeName = model.KodawariTypeName;
        kodawariTypeToUpdate.KodawariTypeDescription = model.KodawariTypeDescription;
        kodawariTypeToUpdate.UpdatedAt = thisDate;
        kodawariTypeToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateUpdateKodawariTypeByNameAsync(string kodawariTypeName, Guid kodawariTypeId)
    {
      var data = await this
          .DB
          .KodawariTypes
          .AsNoTracking()
          .Where(Q => Q.KodawariTypeId == kodawariTypeId)
          .FirstOrDefaultAsync();

      var isChange = data.KodawariTypeName != kodawariTypeName;

      var isExist = await this.ValidateKodawariTypeByNameAsync(kodawariTypeName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteKodawariTypeAsync(DeleteKodawariTypeModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.KodawariTypes.Where(Q => Q.KodawariTypeId == model.KodawariTypeId).FirstOrDefaultAsync();

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
