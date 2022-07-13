using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class ServiceTipMenuService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly ClaimHelper HelperMan;

        public ServiceTipMenuService(TalentContext db, IContextualService contextual, ClaimHelper hm)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.HelperMan = hm;
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
        public async Task<ServiceTipMenuPaginate> GetAllServiceTipMenu(ServiceTipMenuGridFilter filter)
        {

            var query = (from sl in this.DB.ServiceTipMenus
                         where sl.IsDeleted == false
                         select new ServiceTipMenuModel
                         {
                             ServiceTipMenuId = sl.ServiceTipMenuId,
                             ServiceTipMenuName = sl.ServiceTipMenuName,
                             ServiceTipMenuSequence = sl.ServiceTipMenuSequence,
                             CreatedAt = sl.CreatedAt,
                             CreatedBy = sl.CreatedBy,
                             UpdatedBy = sl.UpdatedBy,
                             UpdatedAt = sl.UpdatedAt
                         }).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(filter.ServiceTipMenuName))
            {
                query = query.Where(Q => Q.ServiceTipMenuName == filter.ServiceTipMenuName);
            }

            //sort
            switch (filter.SortBy)
            {
                case "ServiceTipMenuNameAsc":
                    query = query.OrderBy(Q => Q.ServiceTipMenuName);
                    break;
                case "ServiceTipMenuSequenceAsc":
                    query = query.OrderBy(Q => Q.ServiceTipMenuSequence);
                    break;
                case "CreatedAtAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "ServiceTipMenuNameDesc":
                    query = query.OrderByDescending(Q => Q.ServiceTipMenuName);
                    break;
                case "ServiceTipMenuSequenceDesc":
                    query = query.OrderByDescending(Q => Q.ServiceTipMenuSequence);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderBy(Q => Q.ServiceTipMenuName);
                    break;
            }
            var serviceTipMenus = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new ServiceTipMenuPaginate
            {
                ServiceTipMenus = serviceTipMenus,
                TotalServiceTipMenus = totalData
            };
        }
        public async Task<ActionResult<BaseResponse>> InsertServiceTipMenuAsync(ServiceTipMenuCreateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isExist = await ValidateServiceTipMenuByNameAsync(model.ServiceTipMenuName);

                if (isExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var newServiceTipMenus = new ServiceTipMenus
                {
                    ServiceTipMenuName = model.ServiceTipMenuName,
                    ServiceTipMenuSequence = model.ServiceTipMenuSequence,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = this.HelperMan.GetLoginUserId(),
                    UpdatedBy = this.HelperMan.GetLoginUserId()
                };

                this.DB.ServiceTipMenus.Add(newServiceTipMenus);

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<bool> ValidateServiceTipMenuByNameAsync(string serviceTipMenuName)
        {
            var isExist = await this
                .DB
                .ServiceTipMenus
                .AsNoTracking()
                .Where(Q => Q.IsDeleted == false)
                .AnyAsync(Q => Q.ServiceTipMenuName == serviceTipMenuName);

            return isExist;
        }
        public async Task<ServiceTipMenuModel> GetServiceTipMenuById(Guid serviceTipMenuId)
        {
            var data = await (from mo in DB.ServiceTipMenus
                              where mo.ServiceTipMenuId == serviceTipMenuId && mo.IsDeleted == false
                              select new ServiceTipMenuModel
                              {
                                  ServiceTipMenuId = mo.ServiceTipMenuId,
                                  ServiceTipMenuName = mo.ServiceTipMenuName,
                                  ServiceTipMenuSequence = mo.ServiceTipMenuSequence,
                                  CreatedAt = mo.CreatedAt,
                                  CreatedBy = mo.CreatedBy,
                                  UpdatedAt = mo.UpdatedAt,
                                  UpdatedBy = mo.UpdatedBy
                              }).FirstOrDefaultAsync();

            return data;
        }
        public async Task<ActionResult<BaseResponse>> UpdateServiceTipMenuAsync(ServiceTipMenuUpdateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateServiceTipMenuNameAsync(model.ServiceTipMenuName, model.ServiceTipMenuId);

                if (isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var serviceTipMenuToUpdate = await this.DB.ServiceTipMenus.Where(Q => Q.ServiceTipMenuId == model.ServiceTipMenuId).FirstOrDefaultAsync();

                serviceTipMenuToUpdate.ServiceTipMenuId = model.ServiceTipMenuId;
                serviceTipMenuToUpdate.ServiceTipMenuName = model.ServiceTipMenuName;
                serviceTipMenuToUpdate.ServiceTipMenuSequence = model.ServiceTipMenuSequence;
                serviceTipMenuToUpdate.UpdatedAt = thisDate;
                serviceTipMenuToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
            
        }
        public async Task<bool> ValidateUpdateServiceTipMenuNameAsync(string serviceTipMenuName, Guid serviceTipMenuId)
        {
            var data = await this
                .DB
                .ServiceTipMenus
                .AsNoTracking()
                .Where(Q => Q.ServiceTipMenuId == serviceTipMenuId)
                .FirstOrDefaultAsync();

            var isChange = data.ServiceTipMenuName != serviceTipMenuName;

            var isExist = await this.ValidateServiceTipMenuByNameAsync(serviceTipMenuName);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }
        public async Task<ActionResult<BaseResponse>> DeleteServiceTipMenuAsync(DeleteServiceTipMenuModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var module = await this.DB.ServiceTipMenus.Where(Q => Q.ServiceTipMenuId == model.ServiceTipMenuId).FirstOrDefaultAsync();

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
