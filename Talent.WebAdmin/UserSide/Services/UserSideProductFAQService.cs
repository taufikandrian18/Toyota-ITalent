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
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.UserSide.Services
{
  public class UserSideProductFAQService : Controller
  {
    private readonly TalentContext DB;
    private readonly IFileStorageService FileService;
    private readonly IFileStorageService FileMan;
    private readonly ClaimHelper HelperMan;
    private readonly BlobService BlobService;

    public UserSideProductFAQService(TalentContext context, IFileStorageService fileService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
    {
      this.DB = context;
      this.FileService = fileService;
      this.FileMan = fm;
      this.HelperMan = hm;
      this.BlobService = blobService;
    }
    public async Task<List<UserSideProductFAQPaginate>> GetProductFAQMainPageAsync(Guid ProductId)
    {
      var productFAQCategoryList = await (from ct in DB.ProductFAQCategories
                                          where ct.IsDeleted == false && ct.ProductId == ProductId
                                          select new UserSideProductFAQCategoryView
                                          {
                                            ProductFAQCategoryId = ct.ProductFaqCategoryId,
                                            ProductFAQCategoryName = ct.ProductFaqCategoryName
                                          }).ToListAsync();

      var productFAQPaginate = new List<UserSideProductFAQPaginate>();

      foreach (var item in productFAQCategoryList)
      {
        var getProductFAQ = await GetAllUserSideProductFAQ(ProductId, item.ProductFAQCategoryId);

        var model = new MainPageProductFAQModel
        {
          ProductFAQEventList = getProductFAQ,
        };

        var productFAQPaginateModel = new UserSideProductFAQPaginate
        {
          ProductFAQCategoryName = item.ProductFAQCategoryName,
          ProductFAQList = model
        };
        productFAQPaginate.Add(productFAQPaginateModel);
      }


      return productFAQPaginate;
    }
    public async Task<List<UserSideProductFAQModel>> GetAllUserSideProductFAQ(Guid ProductId, Guid ProductFAQCategoryId)
    {
      var query2 = await (from c in DB.ProductFAQs
                          where c.IsDeleted == false && c.ProductFaqCategoryId == ProductFAQCategoryId && c.ProductId == ProductId && c.ApprovedAt != null
                          orderby c.ProductFaqSequence
                          select new UserSideProductFAQModel
                          {
                            ProductFaqId = c.ProductFaqId,
                            ProductId = c.ProductId,
                            BlobId = c.BlobId,
                            ProductFaqCategoryId = c.ProductFaqCategoryId,
                            ProductFaqTitle = c.ProductFaqTitle,
                            ProductFaqSequence = c.ProductFaqSequence,
                            ProductFaqDescription = c.ProductFaqDescription,
                            CreatedAt = c.CreatedAt,
                            UpdatedAt = c.UpdatedAt
                          }).AsNoTracking().ToListAsync();

      var dataProductFiltered = new List<UserSideProductFAQModel>();

      if (ProductId != Guid.Empty)
      {
        dataProductFiltered = query2.Where(x => x.ProductId == ProductId)
        .Select(x => new UserSideProductFAQModel
        {
          ProductFaqId = x.ProductFaqId,
          ProductId = x.ProductId,
          BlobId = x.BlobId,
          ProductFaqCategoryId = x.ProductFaqCategoryId,
          ProductFaqTitle = x.ProductFaqTitle,
          ProductFaqSequence = x.ProductFaqSequence,
          ProductFaqDescription = x.ProductFaqDescription,
          CreatedAt = x.CreatedAt,
          UpdatedAt = x.UpdatedAt
        })
        .ToList();
      }
      else
      {
        dataProductFiltered = query2.Select(x => new UserSideProductFAQModel
        {
          ProductFaqId = x.ProductFaqId,
          ProductId = x.ProductId,
          BlobId = x.BlobId,
          ProductFaqCategoryId = x.ProductFaqCategoryId,
          ProductFaqTitle = x.ProductFaqTitle,
          ProductFaqSequence = x.ProductFaqSequence,
          ProductFaqDescription = x.ProductFaqDescription,
          CreatedAt = x.CreatedAt,
          UpdatedAt = x.UpdatedAt
        })
        .ToList();
      }

      var index = 0;
      foreach (var datum in dataProductFiltered)
      {

        var imageURL = "";
        var imageFileName = "";
        var blobData = await this.BlobService.GetBlobById(datum.BlobId);
        if (blobData != null)
        {
          imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
          imageFileName = blobData.Name;
          dataProductFiltered[index].ImageUrl = imageURL;
          dataProductFiltered[index].ImageFileName = imageFileName;
        }
        index++;
      }

      return dataProductFiltered;
    }
    public async Task<ActionResult<BaseResponse>> ContributeNewProductFAQUserAsync(UserSideProductFAQContributeModel model, string employeeId)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var newProductFAQUser = new ProductFAQUsers
        {
          ProductId = model.ProductId,
          ProductFaqCategoryId = model.ProductFaqCategoryId,
          ProductFAQUserQuestion = model.ProductFAQUserQuestion,
          CreatedAt = thisDate,
          UpdatedAt = thisDate,
          IsDeleted = false,
          CreatedBy = employeeId,
          UpdatedBy = employeeId
        };

        this.DB.ProductFAQUsers.Add(newProductFAQUser);

        await this.DB.SaveChangesAsync();

        return BaseResponse.ResponseOk(null);
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<List<UserSideProductFAQCategoryView>> GetAllUserSideProductFAQCategory(Guid productId)
    {
      var query = await this.DB.ProductFAQCategories.Where(Q => Q.IsDeleted == false && Q.ProductId == productId).Select(Q => new UserSideProductFAQCategoryView
      {
        ProductFAQCategoryId = Q.ProductFaqCategoryId,
        ProductFAQCategoryName = Q.ProductFaqCategoryName
      }).ToListAsync();

      return query;
    }
  }
}
