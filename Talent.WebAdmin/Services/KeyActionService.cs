using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class KeyActionService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;

        public KeyActionService(TalentContext db, IContextualService contextual)
        {
            this.DB = db;
            this.ContextMan = contextual;
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

        /// <summary>
        /// Get Key Action using filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<KeyActionViewModel> GetKeyActions(KeyActionFilter filter)
        {
            var query = this.DB.KeyActions.Select(Q => new KeyActionModel
            {
                CreatedAt = Q.CreatedAt,
                KeyActionCode = Q.KeyActionCode,
                KeyActionDescription = Q.KeyActionDescription,
                KeyActionId = Q.KeyActionId,
                KeyActionName = Q.KeyActionName,
                UpdatedAt = Q.UpdatedAt
            }).AsNoTracking().AsQueryable();

            if (string.IsNullOrEmpty(filter.KeyActionName) == false)
            {
                query = query.Where(Q => Q.KeyActionName.ToLower().Contains(filter.KeyActionName.ToLower()));
            }

            if (string.IsNullOrEmpty(filter.KeyActionCode) == false)
            {
                query = query.Where(Q => Q.KeyActionCode.ToLower().Contains(filter.KeyActionCode.ToLower()));
            }

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate)));
            }

            switch (filter.SortBy)
            {
                case "keyActionCode":
                    query = query.OrderBy(Q => Q.KeyActionCode);
                    break;
                case "keyActionCodeDesc":
                    query = query.OrderByDescending(Q => Q.KeyActionCode);
                    break;
                case "keyActionName":
                    query = query.OrderBy(Q => Q.KeyActionName);
                    break;
                case "keyActionNameDesc":
                    query = query.OrderByDescending(Q => Q.KeyActionName);
                    break;
                case "keyActionDescription":
                    query = query.OrderBy(Q => Q.KeyActionDescription);
                    break;
                case "keyActionDescriptionDesc":
                    query = query.OrderByDescending(Q => Q.KeyActionDescription);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updateDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case "":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case null:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.CountAsync();
            var resultQuery = await query.Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var keyActionResult = new KeyActionViewModel
            {
                ListActionModel = resultQuery,
                TotalItem = totalData
            };

            return keyActionResult;
        }

        /// <summary>
        /// Checking if Key Action is Unique
        /// </summary>
        /// <param name="keyActionCode"></param>
        /// <returns></returns>
        public async Task<bool> IsKeyActionCodeUnique(string keyActionCode)
        {
            var searchResult = await this.DB.KeyActions.Where(Q => Q.KeyActionCode == keyActionCode).FirstOrDefaultAsync();

            if (searchResult == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get Key Action By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<KeyActionModel> GetKeyActionById(int id)
        {
            var search = await this.DB.KeyActions.Select(Q => new KeyActionModel
            {
                CreatedAt = Q.CreatedAt,
                KeyActionCode = Q.KeyActionCode,
                KeyActionDescription = Q.KeyActionDescription,
                KeyActionId = Q.KeyActionId,
                KeyActionName = Q.KeyActionName,
                UpdatedAt = Q.UpdatedAt
            }).Where(Q => Q.KeyActionId == id).FirstOrDefaultAsync();

            if (search == null)
            {
                return new KeyActionModel();
            }

            return search;
        }

        /// <summary>
        /// Get All Key All Action by page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<KeyActionViewModel> GetAllKeyAction(int page)
        {

            var allKeyActions = await this.DB.KeyActions.Select(Q => new KeyActionModel
            {
                CreatedAt = Q.CreatedAt,
                KeyActionCode = Q.KeyActionCode,
                KeyActionDescription = Q.KeyActionDescription,
                KeyActionId = Q.KeyActionId,
                KeyActionName = Q.KeyActionName,
                UpdatedAt = Q.UpdatedAt
            }).Skip((page - 1) * 10).Take(10).AsNoTracking().ToListAsync();

            var totalItem = await this.DB.KeyActions.CountAsync();

            var result = new KeyActionViewModel
            {
                ListActionModel = allKeyActions,
                TotalItem = totalItem
            };

            return result;
        }


        /// <summary>
        /// Insert Key Action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertKeyAction(KeyActionFormModel model)
        {

            var findKeyAction = await this.DB.KeyActions.Where(Q => Q.KeyActionCode == model.KeyActionCode).FirstOrDefaultAsync();

            if (findKeyAction != null)
            {
                return false;
            }

            var insertModel = new KeyActions
            {
                KeyActionCode = model.KeyActionCode,
                KeyActionName = model.KeyActionName,
                KeyActionDescription = model.KeyActionDescription,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = GetUserLogin(),
                UpdatedBy = GetUserLogin(),
            };

            this.DB.KeyActions.Add(insertModel);
            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Funtion to delete Key Action(Hard Delete)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKeyActionAsync(int id)
        {
            var deleteKeyAction = await this.DB.KeyActions.Where(Q => Q.KeyActionId == id).FirstOrDefaultAsync();

            if (deleteKeyAction == null)
            {
                return false;
            }

            this.DB.KeyActions.Remove(deleteKeyAction);
            try
            {
                await this.DB.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get All Key Action
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyActionModel>> GetAllKeyActions()
        {
            var allKeyActions = await this.DB.KeyActions.Select(Q => new KeyActionModel
            {
                CreatedAt = Q.CreatedAt,
                KeyActionCode = Q.KeyActionCode,
                KeyActionDescription = Q.KeyActionDescription,
                KeyActionId = Q.KeyActionId,
                KeyActionName = Q.KeyActionName,
                UpdatedAt = Q.UpdatedAt
            }).AsNoTracking().ToListAsync();

            return allKeyActions;
        }

        /// <summary>
        /// Update Key Actions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateKeyActionsAsync(KeyActionFormModel model)
        {
            var updateKeyAction = await this.DB.KeyActions.Where(Q => Q.KeyActionId == model.KeyActionId).FirstOrDefaultAsync();

            var searchKeyActionCode = await this.DB.KeyActions.Where(Q => Q.KeyActionCode == model.KeyActionCode).FirstOrDefaultAsync();

            if (updateKeyAction == null)
            {
                return false;
            }
            if (searchKeyActionCode != null)
            {
                if (updateKeyAction != searchKeyActionCode)
                {
                    return false;
                }
            }

            updateKeyAction.KeyActionName = model.KeyActionName;
            updateKeyAction.KeyActionCode = model.KeyActionCode;
            updateKeyAction.KeyActionDescription = model.KeyActionDescription;
            updateKeyAction.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateKeyAction.UpdatedBy = GetUserLogin();

            await this.DB.SaveChangesAsync();
            return true;
        }
    }
}
