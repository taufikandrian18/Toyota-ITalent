using System;
using System.Collections.Generic;
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
  public class ProductFeatureMappingService : Controller
  {
    private readonly TalentContext DB;
    private readonly IContextualService ContextMan;
    private readonly ClaimHelper HelperMan;
    private readonly FileService FileServiceMan;
    private readonly IFileStorageService FileMan;
    private readonly CmsFmbService FmbMan;
    private readonly CmsWorkPrincipalService WorkMan;
    private readonly CmsContentService ContentMan;
    private readonly CmsOperationService OperationMan;
    private readonly CmsSettingService SettingMan;
    private readonly PushNotificationService PNService;

    public ProductFeatureMappingService(TalentContext db, IContextualService contextual, FileService fs, ClaimHelper hm, IFileStorageService fm, CmsFmbService fmbMan, CmsWorkPrincipalService workMan, CmsContentService contentMan, CmsOperationService operationMan, CmsSettingService settingMan, PushNotificationService pn)
    {
      this.DB = db;
      this.ContextMan = contextual;
      this.FileServiceMan = fs;
      this.HelperMan = hm;
      this.FileMan = fm;
      this.FmbMan = fmbMan;
      this.WorkMan = workMan;
      this.ContentMan = contentMan;
      this.OperationMan = operationMan;
      this.SettingMan = settingMan;
      this.PNService = pn;
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
    public async Task<ProductFeatureMappingPaginate> GetAllProductFeatureMapping(ProductFeatureMappingGridFilter filter)
    {
      var queryList = new List<ProductFeatureMappingModel>();
      var query = (from sl in this.DB.ProductFeatureMappings
                   join emp in this.DB.EmployeePositionMappings on sl.CreatedBy equals emp.EmployeeId
                   join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                   join ot in this.DB.Outlets on em.OutletId equals ot.OutletId into Outs
                   from ot in Outs.DefaultIfEmpty()
                   join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                   join sla in this.DB.ProductTypes on sl.ProductTypeId equals sla.ProductTypeId
                   join slb in this.DB.ProductFeatures on sl.ProductFeatureId equals slb.ProductFeatureId
                   join slfc in this.DB.ProductFeatureCategories on sl.ProductFeatureCategoryId equals slfc.ProductFeatureCategoryId
                   join slc in this.DB.Cms_Fmbs on sl.Cms_FmbId equals slc.Cms_FmbId into fmbs
                   from slc in fmbs.DefaultIfEmpty()
                   join sld in this.DB.Cms_WorkPrincipals on sl.Cms_WorkPrincipalId equals sld.Cms_WorkPrincipalId into workPricipals
                   from sld in workPricipals.DefaultIfEmpty()
                   join sle in this.DB.Cms_Contents on sl.Cms_ContentId equals sle.Cms_ContentId
                   join slf in this.DB.Cms_Operations on sl.Cms_OperationId equals slf.Cms_OperationId into operations
                   from slf in operations.DefaultIfEmpty()
                   join slg in this.DB.Cms_Settings on sl.Cms_SettingId equals slg.Cms_SettingId into settings
                   from slg in settings.DefaultIfEmpty()
                   where sl.IsDeleted == false
                   select new ProductFeatureMappingModel
                   {
                     ProductFeatureMappingId = sl.ProductFeatureMappingId,
                     ProductId = sl.ProductId,
                     ContributorName = sl.CreatedBy,
                     OutletName = ot.Name,
                     PositionName = pos.PositionName,
                     ProductTypeId = sl.ProductTypeId,
                     ProductTypeName = DB.ProductTypes.Where(x => x.ProductTypeId == sl.ProductTypeId).Select(x => x.ProductTypeName).FirstOrDefault(),
                     ProductFeatureCategoryId = sl.ProductFeatureCategoryId,
                     ProductFeatureCategoryName = DB.ProductFeatureCategories.Where(x => x.ProductFeatureCategoryId == sl.ProductFeatureCategoryId).Select(x => x.ProductFeatureCategoryName).FirstOrDefault(),
                     ProductFeatureId = sl.ProductFeatureId,
                     ProductFeatureName = DB.ProductFeatures.Where(x => x.ProductFeatureId == sl.ProductFeatureId).Select(x => x.ProductFeatureName).FirstOrDefault(),
                     Cms_FmbId = sl.Cms_FmbId,
                     Cms_FmbDescription = DB.Cms_Fmbs.Where(x => x.Cms_FmbId == sl.Cms_FmbId).Select(x => x.Cms_FmbDescription).FirstOrDefault(),
                     Cms_WorkPrincipalId = sl.Cms_WorkPrincipalId,
                     Cms_WorkPrincipalDescription = DB.Cms_WorkPrincipals.Where(x => x.Cms_WorkPrincipalId == sl.Cms_WorkPrincipalId).Select(x => x.Cms_WorkPrincipalDescription).FirstOrDefault(),
                     Cms_ContentId = sl.Cms_ContentId,
                     Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                     Cms_ContentDescription = DB.Cms_Contents.Where(x => x.Cms_ContentId == sl.Cms_ContentId).Select(x => x.Cms_ContentDescription).FirstOrDefault(),
                     Cms_OperationId = sl.Cms_OperationId,
                     Cms_OperationDescription = DB.Cms_Operations.Where(x => x.Cms_OperationId == sl.Cms_OperationId).Select(x => x.Cms_OperationDescription).FirstOrDefault(),
                     Cms_SettingId = sl.Cms_SettingId,
                     Cms_SettingDescription = DB.Cms_Settings.Where(x => x.Cms_SettingId == sl.Cms_SettingId).Select(x => x.Cms_SettingDescription).FirstOrDefault(),
                     IsSpecial = sl.IsSpecial,
                     ProductFeatureMappingApprovalStatus = sl.ProductFeatureMappingApprovalStatus,
                     ApprovedAt = sl.ApprovedAt,
                     CreatedAt = sl.CreatedAt,
                     CreatedBy = sl.CreatedBy,
                     UpdatedBy = sl.UpdatedBy,
                     UpdatedAt = sl.UpdatedAt
                   }).AsNoTracking().AsQueryable();

      if (filter.StartDate != null && filter.EndDate != null)
      {
        var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
        var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

        var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
        var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

        query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                 (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
      }

      if (filter.ProductId != Guid.Empty)
      {
        query = query.Where(Q => Q.ProductId == filter.ProductId);
      }

      // search
      if (!string.IsNullOrEmpty(filter.Query))
      {
        query = query.Where(Q => Q.ProductTypeName.Contains(filter.Query) || Q.ProductFeatureName.Contains(filter.Query));
      }

      // published
      if (filter.ApprovedAt != null)
      {
        if (filter.ApprovedAt == true)
        {
          query = query.Where(Q => Q.ApprovedAt != null);
        }
        else
        {
          query = query.Where(Q => Q.ApprovedAt == null);
        }
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

      foreach(var item in query)
      {
        if(!queryList.Select(Q => Q.ProductFeatureName).ToList().Contains(item.ProductFeatureName))
        {
            queryList.Add(item);
        }
      }
      //var productFeatureMappings = queryList.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToList();

      //var totalData = queryList.Count();

      var productFeatureMappings = query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToList();

      var totalData = query.Count();

      var index = 0;
      foreach (var datum in productFeatureMappings)
      {
        // get fmb data
        var cmsFmbData = new CmsFmbModel();
        if (datum.Cms_FmbId != null)
        {
          cmsFmbData = await this.FmbMan.GetCmsFmbById(datum.Cms_FmbId ?? new Guid());
        }
        productFeatureMappings[index].Cms_Fmb = cmsFmbData;
        // get work data
        var cmsWorkData = new CmsWorkPrincipalModel();
        if (datum.Cms_WorkPrincipalId != null)
        {
          cmsWorkData = await this.WorkMan.GetCmsWorkPrincipalById(datum.Cms_WorkPrincipalId ?? new Guid());
        }
        productFeatureMappings[index].Cms_WorkPrincipal = cmsWorkData;
        // get content data
        var cmsContent = new CmsContentModel();
        if (datum.Cms_ContentId != null)
        {
          cmsContent = await this.ContentMan.GetCmsContentById(datum.Cms_ContentId ?? new Guid());
        }
        productFeatureMappings[index].Cms_Content = cmsContent;
        // get Operation data
        var cmsOperation = new CmsOperationModel();
        if (datum.Cms_OperationId != null)
        {
          cmsOperation = await this.OperationMan.GetCmsOperationById(datum.Cms_OperationId ?? new Guid());
        }
        productFeatureMappings[index].Cms_Operation = cmsOperation;
        // get Setting data
        var cmsSetting = new CmsSettingModel();
        if (datum.Cms_SettingId != null)
        {
          cmsSetting = await this.SettingMan.GetCmsSettingById(datum.Cms_SettingId ?? new Guid());
        }
        productFeatureMappings[index].Cms_Setting = cmsSetting;


        index++;
      }

      return new ProductFeatureMappingPaginate
      {
        ProductFeatureMappings = productFeatureMappings,
        TotalProductFeatureMapping = totalData
      };
    }
    public async Task<ProductFeatureMappingModel> GetProductFeatureMappingById(Guid ProductFeatureMappingId)
    {
      var data = await (from mo in DB.ProductFeatureMappings
                        join emp in this.DB.EmployeePositionMappings on mo.CreatedBy equals emp.EmployeeId
                        join em in this.DB.Employees on emp.EmployeeId equals em.EmployeeId
                        join ot in this.DB.Outlets on em.OutletId equals ot.OutletId
                        join pos in this.DB.Positions on emp.PositionId equals pos.PositionId
                        join sla in this.DB.ProductTypes on mo.ProductTypeId equals sla.ProductTypeId
                        join slb in this.DB.ProductFeatures on mo.ProductFeatureId equals slb.ProductFeatureId
                        join slfc in this.DB.ProductFeatureCategories on mo.ProductFeatureCategoryId equals slfc.ProductFeatureCategoryId
                        join slc in this.DB.Cms_Fmbs on mo.Cms_FmbId equals slc.Cms_FmbId
                        join sld in this.DB.Cms_WorkPrincipals on mo.Cms_WorkPrincipalId equals sld.Cms_WorkPrincipalId
                        join sle in this.DB.Cms_Contents on mo.Cms_ContentId equals sle.Cms_ContentId
                        join slf in this.DB.Cms_Operations on mo.Cms_OperationId equals slf.Cms_OperationId
                        join slg in this.DB.Cms_Settings on mo.Cms_SettingId equals slg.Cms_SettingId
                        where mo.IsDeleted == false && mo.ProductFeatureMappingId == ProductFeatureMappingId
                        select new ProductFeatureMappingModel
                        {
                          ProductFeatureMappingId = mo.ProductFeatureMappingId,
                          ProductId = mo.ProductId,
                          ContributorName = mo.CreatedBy,
                          OutletName = ot.Name,
                          PositionName = pos.PositionName,
                          ProductTypeId = mo.ProductTypeId,
                          ProductTypeName = DB.ProductTypes.Where(x => x.ProductTypeId == mo.ProductTypeId).Select(x => x.ProductTypeName).FirstOrDefault(),
                          ProductFeatureCategoryId = mo.ProductFeatureCategoryId,
                          ProductFeatureCategoryName = DB.ProductFeatureCategories.Where(x => x.ProductFeatureCategoryId == mo.ProductFeatureCategoryId).Select(x => x.ProductFeatureCategoryName).FirstOrDefault(),
                          ProductFeatureId = mo.ProductFeatureId,
                          ProductFeatureName = DB.ProductFeatures.Where(x => x.ProductFeatureId == mo.ProductFeatureId).Select(x => x.ProductFeatureName).FirstOrDefault(),
                          Cms_FmbId = mo.Cms_FmbId,
                          Cms_FmbDescription = DB.Cms_Fmbs.Where(x => x.Cms_FmbId == mo.Cms_FmbId).Select(x => x.Cms_FmbDescription).FirstOrDefault(),
                          Cms_WorkPrincipalId = mo.Cms_WorkPrincipalId,
                          Cms_WorkPrincipalDescription = DB.Cms_WorkPrincipals.Where(x => x.Cms_WorkPrincipalId == mo.Cms_WorkPrincipalId).Select(x => x.Cms_WorkPrincipalDescription).FirstOrDefault(),
                          Cms_ContentId = mo.Cms_ContentId,
                          Cms_ContentName = DB.Cms_Contents.Where(x => x.Cms_ContentId == mo.Cms_ContentId).Select(x => x.Cms_ContentName).FirstOrDefault(),
                          Cms_OperationId = mo.Cms_OperationId,
                          Cms_OperationDescription = DB.Cms_Operations.Where(x => x.Cms_OperationId == mo.Cms_OperationId).Select(x => x.Cms_OperationDescription).FirstOrDefault(),
                          Cms_SettingId = mo.Cms_SettingId,
                          Cms_SettingDescription = DB.Cms_Settings.Where(x => x.Cms_SettingId == mo.Cms_SettingId).Select(x => x.Cms_SettingDescription).FirstOrDefault(),
                          IsSpecial = mo.IsSpecial,
                          ProductFeatureMappingApprovalStatus = mo.ProductFeatureMappingApprovalStatus,
                          ApprovedAt = mo.ApprovedAt,
                          CreatedAt = mo.CreatedAt,
                          CreatedBy = mo.CreatedBy,
                          UpdatedBy = mo.UpdatedBy,
                          UpdatedAt = mo.UpdatedAt
                        }).FirstOrDefaultAsync();

      return data;
    }
    public async Task<ActionResult<BaseResponse>> InsertProductFeatureMappingAsync(ProductFeatureMappingCreateModel model)
    {
      try
      {
        var userLogin = this.HelperMan.GetLoginUserId();

        //var isExist = await ValidateProductFeatureMappingAsync(model.ProductId, model.ProductFeatureCategoryName, model.ProductFeatureId, model.ProductTypeId);

        //if (isExist)
        //{
        //  return false;
        //}

        var insertProductFeatureMappings = new ProductFeatureMappings
        {
          ProductId = model.ProductId,
          ProductTypeId = model.ProductTypeId,
          ProductFeatureCategoryId = model.ProductFeatureCategoryId,
          ProductFeatureId = model.ProductFeatureId,
          Cms_FmbId = model.Cms_FmbId,
          Cms_WorkPrincipalId = model.Cms_WorkPrincipalId,
          Cms_ContentId = model.Cms_ContentId,
          Cms_OperationId = model.Cms_OperationId,
          Cms_SettingId = model.Cms_SettingId,
          IsSpecial = model.IsSpecial,
          ProductFeatureMappingApprovalStatus = "Published",
          EnumContributorFlagging = "FromAdmin",
          ApprovedAt = model.ApprovedAt,
          CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
          CreatedBy = userLogin,
          UpdatedBy = userLogin,
        };

        this.DB.ProductFeatureMappings.Add(insertProductFeatureMappings);
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var getProductFeatureName = await this.DB.ProductFeatures.Where(Q => Q.ProductFeatureId == model.ProductFeatureId).Select(Q => Q.ProductFeatureName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "New Product Feature " + getProductFeatureName;
          PushNotificationMyTools.Body = "A new product feature in " + getProductName + " has been added.";
          PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
          PushNotificationMyTools.IsPublished = true;
          PushNotificationMyTools.GroupPositions = groupPositionList;
          PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

          PushNotificationDataMyTools.Category = "ProductProp";
          PushNotificationDataMyTools.DataID = model.ProductId;
          PushNotificationDataMyTools.DataSecondId = model.ProductTypeId;

          await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
        }
        await this.DB.SaveChangesAsync();
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateProductFeatureMappingAsync(Guid productId, Guid featureCategoryId, Guid productFeatureId, Guid productTypeId)
    {
      var isExist = await this
          .DB
          .ProductFeatureMappings
          .AsNoTracking()
          .Where(Q => Q.IsDeleted == false)
          .AnyAsync(Q => Q.ProductId == productId && Q.ProductFeatureCategoryId == featureCategoryId && Q.ProductFeatureId == productFeatureId && Q.ProductTypeId == productTypeId);

      return isExist;
    }
    public async Task<ActionResult<BaseResponse>> UpdateProductFeatureMapping(ProductFeatureMappingUpdateModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        //var isChangedAndExist = await ValidateUpdateProductFeatureMappingAsync(model.ProductFeatureMappingId, model.ProductId, model.ProductFeatureCategoryName, model.ProductFeatureId, model.ProductTypeId);

        //if (isChangedAndExist)
        //{
        //      return BadRequest(BaseResponse.BadRequest(model));
        //      //return BaseResponse.BadRequest(model);
        //      //return false;
        //}

        var productFeatureMappingToUpdate = await this.DB.ProductFeatureMappings.Where(Q => Q.ProductFeatureMappingId == model.ProductFeatureMappingId).FirstOrDefaultAsync();

        productFeatureMappingToUpdate.ProductFeatureMappingId = model.ProductFeatureMappingId;
        productFeatureMappingToUpdate.ProductId = model.ProductId;
        productFeatureMappingToUpdate.ProductTypeId = model.ProductTypeId;
        productFeatureMappingToUpdate.ProductFeatureId = model.ProductFeatureId;
        productFeatureMappingToUpdate.ProductFeatureCategoryId = model.ProductFeatureCategoryId;
        productFeatureMappingToUpdate.Cms_FmbId = model.Cms_FmbId;
        productFeatureMappingToUpdate.Cms_WorkPrincipalId = model.Cms_WorkPrincipalId;
        productFeatureMappingToUpdate.Cms_ContentId = model.Cms_ContentId;
        productFeatureMappingToUpdate.Cms_OperationId = model.Cms_OperationId;
        productFeatureMappingToUpdate.Cms_SettingId = model.Cms_SettingId;
        productFeatureMappingToUpdate.IsSpecial = model.IsSpecial;
        productFeatureMappingToUpdate.ApprovedAt = model.ApprovedAt;
        productFeatureMappingToUpdate.UpdatedAt = thisDate;
        productFeatureMappingToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

        await this.DB.SaveChangesAsync();
        if (model.ApprovedAt != null)
        {
          var getProductName = await this.DB.Products.Where(Q => Q.ProductId == model.ProductId).Select(Q => Q.ProductName).FirstOrDefaultAsync();
          var getProductFeatureName = await this.DB.ProductFeatures.Where(Q => Q.ProductFeatureId == model.ProductFeatureId).Select(Q => Q.ProductFeatureName).FirstOrDefaultAsync();
          var groupPositionList = new List<string>();
          var manPowerPositionList = new List<string>();
          var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
          var PushNotificationMyTools = new VMPushNotificationAdd();
          PushNotificationMyTools.Title = "Product Feature " + getProductFeatureName + " Update";
          PushNotificationMyTools.Body = "A product feature in " + getProductName + " has been updated.";
          PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
          PushNotificationMyTools.IsPublished = true;
          PushNotificationMyTools.GroupPositions = groupPositionList;
          PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

          PushNotificationDataMyTools.Category = "ProductProp";
          PushNotificationDataMyTools.DataID = model.ProductId;
          PushNotificationDataMyTools.DataSecondId = model.ProductTypeId;

          await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
        }
        return BaseResponse.ResponseOk();
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }
    public async Task<bool> ValidateUpdateProductFeatureMappingAsync(Guid productFeatureMappingId, Guid productId, Guid featureCategoryId, Guid productFeatureId, Guid productTypeId)
    {
      var data = await this
          .DB
          .ProductFeatureMappings
          .AsNoTracking()
          .Where(Q => Q.ProductFeatureMappingId == productFeatureMappingId)
          .FirstOrDefaultAsync();

      var isChange = data.ProductFeatureCategoryId != featureCategoryId;

      var isExist = await this.ValidateProductFeatureMappingAsync(productId, featureCategoryId, productFeatureId, productTypeId);

      var isTrue = isChange == true && isExist == true;

      return isTrue;
    }
    public async Task<ActionResult<BaseResponse>> DeleteProductFeatureMappingAsync(DeleteProductFeatureMappingModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductFeatureMappings.Where(Q => Q.ProductFeatureMappingId == model.ProductFeatureMappingId).FirstOrDefaultAsync();

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
    public async Task<ActionResult<BaseResponse>> UpdateApprovalProductFeatureMappingAsync(ProductFeatureMappingUpdateApprovalModel model)
    {
      try
      {
        var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

        var module = await this.DB.ProductFeatureMappings.Where(Q => Q.ProductFeatureMappingId == model.ProductFeatureMappingId).FirstOrDefaultAsync();
        if (model.isUpdateProductFeatureMappingApproval)
        {
          module.ProductFeatureMappingApprovalStatus = "Published";
          module.UpdatedAt = thisDate;
          module.UpdatedBy = this.HelperMan.GetLoginUserId();
        }
        else
        {
          module.ProductFeatureMappingApprovalStatus = "Draft";
          module.UpdatedAt = thisDate;
          module.UpdatedBy = this.HelperMan.GetLoginUserId();
        }

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
