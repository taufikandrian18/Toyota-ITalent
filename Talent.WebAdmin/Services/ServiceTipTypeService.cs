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
  public class ServiceTipTypeService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public ServiceTipTypeService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bs)
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
    public async Task<ServiceTipTypePaginate> GetAllServiceTipType(ServiceTipTypeGridFilter filter)
    {
      var query = (from sl in this.DB.ServiceTipTypes
                   where sl.IsDeleted == false
                   select new ServiceTipTypeModel
                   {
                     ServiceTipTypeId = sl.ServiceTipTypeId,
                     BlobId = sl.BlobId,
                     ServiceTipTypeName = sl.ServiceTipTypeName,
                     ServiceTipTypeDescription = sl.ServiceTipTypeDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.ServiceTipTypeName))
      {
        query = query.Where(Q => Q.ServiceTipTypeName.Contains(filter.ServiceTipTypeName));
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
      var serviceTipTypes = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      var index = 0;
      foreach (var datum in serviceTipTypes)
      {

        var imageURL = "";
        var fileFormat = "";
        if (datum.BlobId != null)
        {
          var blobData = await this.BlobService.GetBlobById(datum.BlobId);

          imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
          fileFormat = blobData.Mime;
        }


        serviceTipTypes[index].ImageUrl = imageURL;
        serviceTipTypes[index].FileTypeBlob = fileFormat;
        index++;
      }

      return new ServiceTipTypePaginate
      {
        ServiceTipTypes = serviceTipTypes,
        TotalServiceTipTypes = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertServiceTipTypeAsync(ServiceTipTypeCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateServiceTipTypeByNameAsync(model.ServiceTipTypeName);

            if (isExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            Guid? getUploadBlob = null;

            if (model.ProductFileContent != null)
            {
                if (string.IsNullOrEmpty(model.ProductFileContent.Base64) == false)
                {
                    getUploadBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                }
            }

            var newServiceTipTypes = new ServiceTipTypes
            {
                ServiceTipTypeName = model.ServiceTipTypeName,
                ServiceTipTypeDescription = model.ServiceTipTypeDescription,
                BlobId = getUploadBlob,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.ServiceTipTypes.Add(newServiceTipTypes);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateServiceTipTypeByNameAsync(string serviceTipTypeName)
    {
      var isExist = await this
          .DB
          .ServiceTipTypes
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ServiceTipTypeName == serviceTipTypeName);

      return isExist;
    }
    public async Task<ServiceTipTypeModel> GetServiceTipTypeById(Guid serviceTipTypeId)
    {
      var data = await (from mo in DB.ServiceTipTypes
                        where mo.ServiceTipTypeId == serviceTipTypeId && mo.IsDeleted == false
                        select new ServiceTipTypeModel
                        {
                          ServiceTipTypeId = mo.ServiceTipTypeId,
                          BlobId = mo.BlobId,
                          ServiceTipTypeName = mo.ServiceTipTypeName,
                          ServiceTipTypeDescription = mo.ServiceTipTypeDescription,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateServiceTipTypeAsync(ServiceTipTypeUpdateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateServiceTipTypeNameAsync(model.ServiceTipTypeName, model.ServiceTipTypeId);

            if (isChangedAndExist)
            {
                return BadRequest(BaseResponse.BadRequest(model));
            }

            var serviceTipTypeToUpdate = await this.DB.ServiceTipTypes.Where(Q => Q.ServiceTipTypeId == model.ServiceTipTypeId).FirstOrDefaultAsync();

            if (model.ProductFileContent != null)
            {
                if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
                {
                    serviceTipTypeToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                }
            }

            serviceTipTypeToUpdate.ServiceTipTypeId = model.ServiceTipTypeId;
            serviceTipTypeToUpdate.ServiceTipTypeName = model.ServiceTipTypeName;
            serviceTipTypeToUpdate.ServiceTipTypeDescription = model.ServiceTipTypeDescription;
            serviceTipTypeToUpdate.UpdatedAt = thisDate;
            serviceTipTypeToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateUpdateServiceTipTypeNameAsync(string serviceTipTypeName, Guid serviceTipTypeId)
    {
      var data = await this
          .DB
          .ServiceTipTypes
          .AsNoTracking()
          .Where(Q => Q.ServiceTipTypeId == serviceTipTypeId)
          .FirstOrDefaultAsync();

      var isChange = data.ServiceTipTypeName != serviceTipTypeName;

      var isExist = await this.ValidateServiceTipTypeByNameAsync(serviceTipTypeName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteServiceTipTypeAsync(DeleteServiceTipTypeModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.ServiceTipTypes.Where(Q => Q.ServiceTipTypeId == model.ServiceTipTypeId).FirstOrDefaultAsync();

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
