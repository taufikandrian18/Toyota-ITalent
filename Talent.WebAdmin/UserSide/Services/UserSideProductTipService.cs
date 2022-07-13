using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.UserSide.Services
{
  public class UserSideProductTipService
  {
    private readonly TalentContext DB;
    private readonly IFileStorageService FileService;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public UserSideProductTipService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
    {
      this.DB = context;
      this.FileService = fileService;
      this.FileMan = fm;
      this.HelperMan = hm;
      this.BlobService = blobService;
    }

    public async Task<List<UserSideProductTipPaginateModel>> GetProductTipPaginate(Guid productId)
    {
      var TipCategoryData = (from sl in this.DB.ProductTipCategories
                             where sl.IsDeleted == false && sl.ProductId == productId
                             select new UserSideProductTipCategoryModel
                             {
                               ProductTipCategoryId = sl.ProductTipCategoryId,
                               ProductTipCategoryName = sl.ProductTipCategoryName
                             }).ToList();

      var ProductTipModelView = new List<UserSideProductTipPaginateModel>();


      foreach (var item in TipCategoryData)
      {
        var listProductTipInCategory = new UserSideProductTipPaginateModel();

        var query2 = await (from c in DB.ProductTips
                            where c.IsDeleted == false && c.ProductTipCategoryId == item.ProductTipCategoryId && c.IsApproved == "Published" && c.ProductId == productId && c.ApprovedAt != null
                            select new UserSideProductTipViewModel
                            {
                              ProductTipId = c.ProductTipId,
                              ProductTipTitle = c.ProductTipTitle,
                              IsSequence = c.IsSequence
                            }).OrderBy(Q => Q.IsSequence).AsNoTracking().ToListAsync();

        if (query2.Count() != 0)
        {
          listProductTipInCategory.ProductTipCategoryId = item.ProductTipCategoryId;
          listProductTipInCategory.ProductTipCategory = item.ProductTipCategoryName;
          listProductTipInCategory.ProductTipTitles = query2;

          ProductTipModelView.Add(listProductTipInCategory);
        }
      }

      return ProductTipModelView;
    }
    public async Task<bool> ContributeNewSalesTipAsync(UserSideContributeNewSalesTipCreateModel model, string empId)
    {
      var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

      var isExist = await ValidateProductTipBySeqAsync(model.IsSequence, model.ProductTipTitle, model.ProductId, model.ProductTipCategoryId);

      if (isExist)
      {
        return false;
      }

      var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);

      if (model.ProductGalleryFileContent != null)
      {
        if (string.IsNullOrEmpty(model.ProductGalleryFileContent.Base64) == false)
        {
          getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductGalleryFileContent);
        }
      }

      var employeeId = empId;

      var outletId = await DB.Employees.Where(x => x.EmployeeId == employeeId).Select(x => x.OutletId).FirstOrDefaultAsync();

      var newProductTips = new ProductTips
      {
        ProductId = model.ProductId,
        ProductTipCategoryId = model.ProductTipCategoryId,
        BlobId = getUploadModuleBlob,
        OutletId = outletId,
        IsSequence = model.IsSequence,
        IsApproved = "Draft",
        ProductTipTitle = model.ProductTipTitle,
        ProductTipDescription = model.ProductTipDescription,
        CreatedAt = thisDate,
        UpdatedAt = thisDate,
        IsDeleted = false,
        CreatedBy = employeeId,
        UpdatedBy = employeeId
      };

      this.DB.ProductTips.Add(newProductTips);

      await this.DB.SaveChangesAsync();
      return true;
    }
    public async Task<bool> ValidateProductTipBySeqAsync(int IsSequence, string ProductTipTitle, Guid ProductId, Guid ProductTipCategoryId)
    {
      var isExist = await this
          .DB
          .ProductTips
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false && Q.ProductId == ProductId && Q.ProductTipCategoryId == ProductTipCategoryId)
          .AnyAsync(Q => Q.ProductTipTitle == ProductTipTitle);

      return isExist;
    }
    public async Task<UserSideProductTipModel> GetUserSideSalesTipDetail(Guid productTipId, Guid productId)
    {
      var salesTipData = await (from sl in this.DB.ProductTips
                                where sl.IsDeleted == false && sl.ProductTipId == productTipId && sl.ProductId == productId
                                orderby sl.IsSequence
                                select new UserSideProductTipModel
                                {
                                  ProductTipId = sl.ProductTipId,
                                  ProductId = sl.ProductId,
                                  ProductTipCategoryId = sl.ProductTipCategoryId,
                                  BlobId = sl.BlobId,
                                  OutletId = sl.OutletId,
                                  ProductTipTitle = sl.ProductTipTitle,
                                  ProductTipDescription = sl.ProductTipDescription,
                                  IsSequence = sl.IsSequence,
                                  CreatedAt = sl.CreatedAt,
                                  CreatedBy = sl.CreatedBy,
                                  UpdatedAt = sl.UpdatedAt,
                                  UpdatedBy = sl.UpdatedBy
                                }).FirstOrDefaultAsync();

      var fileURL = "";
      var fileName = "";
      var fileType = "";

      if (salesTipData.BlobId != Guid.Empty)
      {
        var blobData = await this.BlobService.GetBlobById(salesTipData.BlobId);
        if (blobData != null)
        {
          fileURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
          fileName = blobData.Name;
          fileType = blobData.Mime;

          salesTipData.FileUrl = fileURL;
          salesTipData.FileName = fileName;
          salesTipData.FileType = fileType;
        }
      }

      return salesTipData;
    }
  }
}
