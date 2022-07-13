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
    public class KeyPieceOfMindMenuService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly ClaimHelper HelperMan;

        public KeyPieceOfMindMenuService(TalentContext db, IContextualService contextual, ClaimHelper hm)
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
        public async Task<KeyPieceOfMindMenuPaginate> GetAllKeyPieceOfMindMenu(KeyPieceOfMindMenuGridFilter filter)
        {

            var query = (from sl in this.DB.KeyPieceOfMindMenus
                         where sl.IsDeleted == false
                         select new KeyPieceOfMindMenuModel
                         {
                             KeyPieceOfMindMenuId = sl.KeyPieceOfMindMenuId,
                             KeyPieceOfMindMenuName = sl.KeyPieceOfMindMenuName,
                             KeyPieceOfMindMenuSequence = sl.KeyPieceOfMindMenuSequence,
                             CreatedAt = sl.CreatedAt,
                             CreatedBy = sl.CreatedBy,
                             UpdatedBy = sl.UpdatedBy,
                             UpdatedAt = sl.UpdatedAt
                         }).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(filter.KeyPieceOfMindMenuName))
            {
                query = query.Where(Q => Q.KeyPieceOfMindMenuName == filter.KeyPieceOfMindMenuName);
            }

            //sort
            switch (filter.SortBy)
            {
                case "ServiceTipMenuNameAsc":
                    query = query.OrderBy(Q => Q.KeyPieceOfMindMenuName);
                    break;
                case "ServiceTipMenuSequenceAsc":
                    query = query.OrderBy(Q => Q.KeyPieceOfMindMenuSequence);
                    break;
                case "CreatedAtAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "ServiceTipMenuNameDesc":
                    query = query.OrderByDescending(Q => Q.KeyPieceOfMindMenuName);
                    break;
                case "ServiceTipMenuSequenceDesc":
                    query = query.OrderByDescending(Q => Q.KeyPieceOfMindMenuSequence);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderBy(Q => Q.KeyPieceOfMindMenuName);
                    break;
            }
            var keyPieceOfMindMenus = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new KeyPieceOfMindMenuPaginate
            {
                KeyPieceOfMindMenus = keyPieceOfMindMenus,
                TotalKeyPieceOfMindMenus = totalData
            };
        }
        public async Task<ActionResult<BaseResponse>> InsertKeyPieceOfMindMenuAsync(KeyPieceOfMindMenuCreateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isExist = await ValidateKeyPieceOfMindMenuByNameAsync(model.KeyPieceOfMindMenuName);

                if (isExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var newKeyPieceOfMindMenus = new KeyPieceOfMindMenus
                {
                    KeyPieceOfMindMenuName = model.KeyPieceOfMindMenuName,
                    KeyPieceOfMindMenuSequence = model.KeyPieceOfMindMenuSequence,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = this.HelperMan.GetLoginUserId(),
                    UpdatedBy = this.HelperMan.GetLoginUserId()
                };

                this.DB.KeyPieceOfMindMenus.Add(newKeyPieceOfMindMenus);

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<bool> ValidateKeyPieceOfMindMenuByNameAsync(string keyPieceOfMindMenuName)
        {
            var isExist = await this
                .DB
                .KeyPieceOfMindMenus
                .AsNoTracking()
                .Where(Q => Q.IsDeleted == false)
                .AnyAsync(Q => Q.KeyPieceOfMindMenuName == keyPieceOfMindMenuName);

            return isExist;
        }
        public async Task<KeyPieceOfMindMenuModel> GetKeyPieceOfMindMenuById(Guid keyPieceOfMindMenuId)
        {
            var data = await (from mo in DB.KeyPieceOfMindMenus
                              where mo.KeyPieceOfMindMenuId == keyPieceOfMindMenuId && mo.IsDeleted == false
                              select new KeyPieceOfMindMenuModel
                              {
                                  KeyPieceOfMindMenuId = mo.KeyPieceOfMindMenuId,
                                  KeyPieceOfMindMenuName = mo.KeyPieceOfMindMenuName,
                                  KeyPieceOfMindMenuSequence = mo.KeyPieceOfMindMenuSequence,
                                  CreatedAt = mo.CreatedAt,
                                  CreatedBy = mo.CreatedBy,
                                  UpdatedAt = mo.UpdatedAt,
                                  UpdatedBy = mo.UpdatedBy
                              }).FirstOrDefaultAsync();

            return data;
        }
        public async Task<ActionResult<BaseResponse>> UpdateKeyPieceOfMindMenuAsync(KeyPieceOfMindMenuUpdateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateKeyPieceOfMindMenuNameAsync(model.KeyPieceOfMindMenuName, model.KeyPieceOfMindMenuId);

                if (isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var keyPieceOfMindMenuToUpdate = await this.DB.KeyPieceOfMindMenus.Where(Q => Q.KeyPieceOfMindMenuId == model.KeyPieceOfMindMenuId).FirstOrDefaultAsync();

                keyPieceOfMindMenuToUpdate.KeyPieceOfMindMenuId = model.KeyPieceOfMindMenuId;
                keyPieceOfMindMenuToUpdate.KeyPieceOfMindMenuName = model.KeyPieceOfMindMenuName;
                keyPieceOfMindMenuToUpdate.KeyPieceOfMindMenuSequence = model.KeyPieceOfMindMenuSequence;
                keyPieceOfMindMenuToUpdate.UpdatedAt = thisDate;
                keyPieceOfMindMenuToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }

        }
        public async Task<bool> ValidateUpdateKeyPieceOfMindMenuNameAsync(string keyPieceOfMindMenuName, Guid keyPieceOfMindMenuId)
        {
            var data = await this
                .DB
                .KeyPieceOfMindMenus
                .AsNoTracking()
                .Where(Q => Q.KeyPieceOfMindMenuId == keyPieceOfMindMenuId)
                .FirstOrDefaultAsync();

            var isChange = data.KeyPieceOfMindMenuName != keyPieceOfMindMenuName;

            var isExist = await this.ValidateKeyPieceOfMindMenuByNameAsync(keyPieceOfMindMenuName);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }
        public async Task<ActionResult<BaseResponse>> DeleteKeyPieceOfMindMenuAsync(DeleteKeyPieceOfMindMenuModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var module = await this.DB.KeyPieceOfMindMenus.Where(Q => Q.KeyPieceOfMindMenuId == model.KeyPieceOfMindMenuId).FirstOrDefaultAsync();

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
