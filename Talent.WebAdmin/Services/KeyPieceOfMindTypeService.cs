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
  public class KeyPieceOfMindTypeService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;

    public KeyPieceOfMindTypeService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.HelperMan = hm;
      this.FileMan = fm;
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
    public async Task<KeyPieceOfMindTypePaginate> GetAllKeyPieceOfMindType(KeyPieceOfMindTypeGridFilter filter)
    {
      var query = (from sl in this.DB.KeyPieceOfMindTypes
                   where sl.IsDeleted == false
                   select new KeyPieceOfMindTypeModel
                   {
                     KeyPieceOfMindTypeId = sl.KeyPieceOfMindTypeId,
                     BlobId = sl.BlobId,
                     KeyPieceOfMindTypeName = sl.KeyPieceOfMindTypeName,
                     KeyPieceOfMindTypeDescription = sl.KeyPieceOfMindTypeDescription,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (!string.IsNullOrEmpty(filter.KeyPieceOfMindTypeName))
      {
        query = query.Where(Q => Q.KeyPieceOfMindTypeName.Contains(filter.KeyPieceOfMindTypeName));
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
      var keyPieceOfMindTypes = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

      var totalData = await query.CountAsync();

      return new KeyPieceOfMindTypePaginate
      {
        KeyPieceOfMindTypes = keyPieceOfMindTypes,
        TotalKeyPieceOfMindTypes = totalData
      };
    }
    public async Task<ActionResult<BaseResponse>> InsertKeyPieceOfMindTypeAsync(KeyPieceOfMindTypeCreateModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateProductKeyPieceOfMindTypeByNameAsync(model.KeyPieceOfMindTypeName);

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

            var newKeyPieceOfMindTypes = new KeyPieceOfMindTypes
            {
                KeyPieceOfMindTypeName = model.KeyPieceOfMindTypeName,
                KeyPieceOfMindTypeDescription = model.KeyPieceOfMindTypeDescription,
                BlobId = getUploadBlob,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.KeyPieceOfMindTypes.Add(newKeyPieceOfMindTypes);

            await this.DB.SaveChangesAsync();
            return BaseResponse.ResponseOk();
        }
        catch (Exception x)
        {
            return StatusCode(500, BaseResponse.Error(null, x));
        }
    }
    public async Task<bool> ValidateProductKeyPieceOfMindTypeByNameAsync(string keyPieceOfMindTypeName)
    {
      var isExist = await this
          .DB
          .KeyPieceOfMindTypes
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.KeyPieceOfMindTypeName == keyPieceOfMindTypeName);

      return isExist;
    }
    public async Task<KeyPieceOfMindTypeModel> GetKeyPieceOfMindTypeById(Guid keyPieceOfMindTypeId)
    {
      var data = await (from mo in DB.KeyPieceOfMindTypes
                        where mo.KeyPieceOfMindTypeId == keyPieceOfMindTypeId && mo.IsDeleted == false
                        select new KeyPieceOfMindTypeModel
                        {
                          KeyPieceOfMindTypeId = mo.KeyPieceOfMindTypeId,
                          BlobId = mo.BlobId,
                          KeyPieceOfMindTypeName = mo.KeyPieceOfMindTypeName,
                          KeyPieceOfMindTypeDescription = mo.KeyPieceOfMindTypeDescription,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> UpdateKeyPieceOfMindTypeAsync(KeyPieceOfMindTypeUpdateModel model)
    {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateKeyPieceOfMindTypeNameAsync(model.KeyPieceOfMindTypeName, model.KeyPieceOfMindTypeId);

                if (isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var keyPieceOfMindTypeToUpdate = await this.DB.KeyPieceOfMindTypes.Where(Q => Q.KeyPieceOfMindTypeId == model.KeyPieceOfMindTypeId).FirstOrDefaultAsync();

                if (model.ProductFileContent != null)
                {
                    if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
                    {
                        keyPieceOfMindTypeToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                    }
                }

                keyPieceOfMindTypeToUpdate.KeyPieceOfMindTypeId = model.KeyPieceOfMindTypeId;
                keyPieceOfMindTypeToUpdate.KeyPieceOfMindTypeName = model.KeyPieceOfMindTypeName;
                keyPieceOfMindTypeToUpdate.KeyPieceOfMindTypeDescription = model.KeyPieceOfMindTypeDescription;
                keyPieceOfMindTypeToUpdate.UpdatedAt = thisDate;
                keyPieceOfMindTypeToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
    }
    public async Task<bool> ValidateUpdateKeyPieceOfMindTypeNameAsync(string keyPieceOfMindTypeName, Guid keyPieceOfMindTypeId)
    {
      var data = await this
          .DB
          .KeyPieceOfMindTypes
          .AsNoTracking()
          .Where(Q => Q.KeyPieceOfMindTypeId == keyPieceOfMindTypeId)
          .FirstOrDefaultAsync();

      var isChange = data.KeyPieceOfMindTypeName != keyPieceOfMindTypeName;

      var isExist = await this.ValidateProductKeyPieceOfMindTypeByNameAsync(keyPieceOfMindTypeName);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteKeyPieceOfMindTypeAsync(DeleteKeyPieceOfMindTypeModel model)
    {
        try
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var module = await this.DB.KeyPieceOfMindTypes.Where(Q => Q.KeyPieceOfMindTypeId == model.KeyPieceOfMindTypeId).FirstOrDefaultAsync();

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
