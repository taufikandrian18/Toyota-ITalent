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
    public class UserPrivilegeSettingsService
    {
        private readonly TalentContext _Db;
        private readonly IContextualService _ContextualMan;

        public UserPrivilegeSettingsService(TalentContext talentContext, IContextualService ics)
        {
            this._Db = talentContext;
            this._ContextualMan = ics;
        }

        public string GetUserLogin()
        {
            try
            {
                return _ContextualMan.CookieClaims.EmployeeId;
            }
            catch
            {
                return "SYSTEM";
            }
        }

        public IQueryable<UserPrivilegeSettingsModel> JoinTable()
        {
            var query = (from ppm in this._Db.PrivilegePageMappings
                         join r in this._Db.Roles on ppm.RoleId equals r.RoleId
                         join p in this._Db.Pages on ppm.PageId equals p.PageId
                         join m in this._Db.Menus on p.MenuId equals m.MenuId
                         select new UserPrivilegeSettingsModel
                         {
                             PrivilegeSettingsId = ppm.PrivilegePageMappingsId,
                             UserRoleId = r.RoleId,
                             UserRole = r.Name,
                             MenuId = m.MenuId,
                             Menu = m.Name,
                             PageId = p.PageId,
                             Page = p.Name,
                             Create = ppm.IsCreate,
                             Read = ppm.IsRead,
                             Update = ppm.IsUpdate,
                             Delete = ppm.IsDelete,
                             CreatedAt = ppm.CreatedAt,
                             UpdatedAt = ppm.UpdatedAt
                         }).AsNoTracking().AsQueryable();
            return query;
        }

        public async Task<UserPrivilegeSettingsPaginateModel> GetAllUserPrivilegeSettingsPaginate(UserPrivilegeSettingsGridFilter filter)
        {
            var query = JoinTable();

            //Search
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            if (filter.MenuId != null)
            {
                query = query.Where(Q => Q.MenuId == filter.MenuId);
            }
            if (filter.UserRole != null)
            {
                query = query.Where(Q => Q.UserRole.Contains(filter.UserRole));
            }
            if (filter.PageId != null)
            {
                query = query.Where(Q => Q.PageId == filter.PageId);
            }

            //SORT
            switch (filter.SortBy)
            {
                case "UserRoleAsc":
                    query = query.OrderBy(Q => Q.UserRole);
                    break;
                case "MenuAsc":
                    query = query.OrderBy(Q => Q.Menu);
                    break;
                case "PageAsc":
                    query = query.OrderBy(Q => Q.Page);
                    break;
                case "CreatedDateAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedDateAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "UserRoleDesc":
                    query = query.OrderByDescending(Q => Q.UserRole);
                    break;
                case "MenuDesc":
                    query = query.OrderByDescending(Q => Q.Menu);
                    break;
                case "PageDesc":
                    query = query.OrderByDescending(Q => Q.Page);
                    break;
                case "CreatedDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var data = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();
            return new UserPrivilegeSettingsPaginateModel
            {
                Data = data,
                TotalData = totalData
            };
        }

        public async Task<UserPrivilegeSettingGetByUserId> GetUserPrivilegeSettingsById(int userRoleId)
        {
            var menuPage = await (
                    from ppm in this._Db.PrivilegePageMappings
                    join r in this._Db.Roles on ppm.RoleId equals r.RoleId
                    join p in this._Db.Pages on ppm.PageId equals p.PageId
                    join m in this._Db.Menus on p.MenuId equals m.MenuId
                    where r.RoleId == userRoleId
                    select new UserPrivilegeSettingPageCRUD
                    {
                        IsPageCheck = true,
                        MenuId = m.MenuId,
                        PageId = p.PageId,
                        Page = p.Name,
                        Create = ppm.IsCreate,
                        Read = ppm.IsRead,
                        Update = ppm.IsUpdate,
                        Delete = ppm.IsDelete
                    }
                ).ToListAsync();

            var data = new UserPrivilegeSettingGetByUserId
            {
                UserRoleId = userRoleId,
                MenuPage = menuPage
            };

            return data;
        }

        public async Task<bool> InsertUserPrivilegeSettings(List<UserPrivilegeSettingsInsertModel> insert)
        {
            var privilegePageMappings = new List<PrivilegePageMappings>();

            foreach (var i in insert)
            {
                var data = new PrivilegePageMappings
                {
                    RoleId = i.UserRoleId,
                    PageId = i.PageId,
                    IsCreate = i.Create,
                    IsRead = i.Read,
                    IsUpdate = i.Update,
                    IsDelete = i.Delete,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = GetUserLogin(),
                    UpdatedBy = GetUserLogin(),
                };
                privilegePageMappings.Add(data);
            }

            this._Db.PrivilegePageMappings.AddRange(privilegePageMappings);
            await this._Db.SaveChangesAsync();
            return true;
        }
        //UserRoleNotYetSet
        public async Task<List<UserPrivilegeSettingsUserRoleModel>> GetAllUserPrivilegeSettingsUserRoleNotYetSet()
        {
            var all = await this._Db.Roles.Select(Q => Q.RoleId).ToListAsync();

            var exist = await this._Db.PrivilegePageMappings.Select(Q => Q.RoleId).Distinct<int>().ToListAsync();

            all = all.Except(exist).ToList();

            var data = await this._Db.Roles.Where(Q => all.Contains(Q.RoleId)).Select(Q => new UserPrivilegeSettingsUserRoleModel
            {
                UserRole = Q.Name,
                UserRoleId = Q.RoleId
            }).ToListAsync();

            return data;
        }
        //UserRoleNotYetSetExceptSelf
        public async Task<List<UserPrivilegeSettingsUserRoleModel>> GetAllUserPrivilegeSettingsUserRoleNotYetSetExceptSelf(int id)
        {
            var all = await this._Db.Roles.Select(Q => Q.RoleId).ToListAsync();

            var exist = await this._Db.PrivilegePageMappings.Select(Q => Q.RoleId).Distinct<int>().ToListAsync();
            exist.Remove(id);

            all = all.Except(exist).ToList();

            var data = await this._Db.Roles.Where(Q => all.Contains(Q.RoleId)).Select(Q => new UserPrivilegeSettingsUserRoleModel
            {
                UserRole = Q.Name,
                UserRoleId = Q.RoleId
            }).ToListAsync();

            return data;
        }
        //UserRole
        //public async Task<List<UserPrivilegeSettingsUserRoleModel>> GetAllUserPrivilegeSettingsUserRole(string name)
        //{
        //    var data = await this._Db.Roles.Select(Q => new UserPrivilegeSettingsUserRoleModel
        //    {
        //        UserRole = Q.Name,
        //        UserRoleId = Q.RoleId
        //    }).Where(Q => Q.UserRole.Contains(name)).Take(10).ToListAsync();
        //    return data;
        //}
        //Menu
        public async Task<List<UserPrivilegeSettingsMenuModel>> GetAllUserPrivilegeSettingsMenu()
        {
            var data = await this._Db.Menus.Select(Q => new UserPrivilegeSettingsMenuModel
            {
                Menu = Q.Name,
                MenuId = Q.MenuId
            }).ToListAsync();
            return data;
        }
        //Page
        public async Task<List<UserPrivilegeSettingsPageModel>> GetAllUserPrivilegeSettingsPage()
        {
            var data = await this._Db.Pages.Select(Q => new UserPrivilegeSettingsPageModel
            {
                Page = Q.Name,
                PageId = Q.PageId
            }).ToListAsync();
            return data;
        }
        //MenuAndPage
        public async Task<List<UserPrivilegeSettingPageCRUD>> GetAllUserPrivilegeSettingsMenuPageCrudById(string id)
        {
            var data = await (
                    from m in this._Db.Menus
                    join p in this._Db.Pages on m.MenuId equals p.MenuId
                    where m.MenuId == id
                    select new UserPrivilegeSettingPageCRUD
                    {
                        MenuId = m.MenuId,
                        PageId = p.PageId,
                        Page = p.Name,
                        Create = false,
                        Read = false,
                        Update = false,
                        Delete = false
                    }
                ).ToListAsync();
            return data;
        }

        public async Task<bool> UpdateUserPrivilegeSettings(int id, List<UserPrivilegeSettingsInsertModel> update)
        {
            //UserRoleId DI EDIT
            if (update[0].UserRoleId != id)
            {
                using (var transaction = this._Db.Database.BeginTransaction())
                {
                    var toBeDeleted = await this._Db.PrivilegePageMappings.Where(Q => Q.RoleId == id).ToListAsync();
                    this._Db.RemoveRange(toBeDeleted);
                    await this._Db.SaveChangesAsync();


                    //add new
                    await this.InsertUserPrivilegeSettings(update);
                    transaction.Commit();
                }
                return true;
            }

            //UserRoleId TIDAK DI EDIT
            var before = await this._Db.PrivilegePageMappings.Where(Q => Q.RoleId == id).ToListAsync();
            var forRemove = new List<PrivilegePageMappings>();
            var forAdd = new List<UserPrivilegeSettingsInsertModel>();
            var forUpdatePageIdOnly = new List<string>();
            var forUpdate = new List<UserPrivilegeSettingsInsertModel>();

            using (var transaction = this._Db.Database.BeginTransaction())
            {
                //remove
                foreach (var b in before)
                {
                    if (update.Find(Q => Q.PageId == b.PageId) == null)
                    {
                        forRemove.Add(b);
                    }
                }
                this._Db.RemoveRange(forRemove);
                await this._Db.SaveChangesAsync();

                //add
                foreach (var u in update)
                {
                    if (before.Find(Q => Q.PageId == u.PageId) == null)
                    {
                        forAdd.Add(u);
                    }
                    else
                    {
                        forUpdatePageIdOnly.Add(u.PageId);
                        forUpdate.Add(u);
                    }
                }

                //update
                var afterRemove = await this._Db.PrivilegePageMappings.Where(Q => forUpdatePageIdOnly.Contains(Q.PageId) && Q.RoleId == id).ToListAsync();

                foreach (var ar in afterRemove)
                {
                    var data = forUpdate.Find(Q => Q.PageId == ar.PageId);
                    ar.RoleId = id;
                    ar.PageId = data.PageId;
                    ar.IsCreate = data.Create;
                    ar.IsRead = data.Read;
                    ar.IsUpdate = data.Update;
                    ar.IsDelete = data.Delete;
                    ar.UpdatedAt = DateTime.UtcNow;
                    ar.UpdatedBy = GetUserLogin();
                }

                await this._Db.SaveChangesAsync();

                //add to db
                var privilegePageMappings = new List<PrivilegePageMappings>();
                foreach (var i in forAdd)
                {
                    var data = new PrivilegePageMappings
                    {
                        RoleId = i.UserRoleId,
                        PageId = i.PageId,
                        IsCreate = i.Create,
                        IsRead = i.Read,
                        IsUpdate = i.Update,
                        IsDelete = i.Delete,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = GetUserLogin(),
                        UpdatedBy = GetUserLogin()
                    };
                    privilegePageMappings.Add(data);
                }
                this._Db.PrivilegePageMappings.AddRange(privilegePageMappings);

                await this._Db.SaveChangesAsync();

                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> DeleteUserPrivilegeSettings(int userRoleId, List<string> id)
        {
            var data = await this._Db.PrivilegePageMappings.Where(Q => id.Contains(Q.PageId) && Q.RoleId == userRoleId).ToListAsync();
            this._Db.RemoveRange(data);
            await this._Db.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUserRoleIdFromPrivilegePageMappingsId(int id)
        {
            var data = await this._Db.PrivilegePageMappings.Where(Q => Q.PrivilegePageMappingsId == id).Select(Q => Q.RoleId).FirstOrDefaultAsync();
            return data;
        }

        //SETTING BUAT CHECK BOLEH ACCESS PAGE ATAU TIDAK
        public bool AccessPage(string pageId)
        {
            var pages = this._ContextualMan.CookieClaims.PageIds;

            foreach (var page in pages)
            {
                if (page.PageId == pageId)
                {
                    return true;
                }
            }
            return true;
        }
        //CRUD ACCESS

        //12/19/2019 3:33PM READ = VIEW DETAIL
        public UserAccessCRUD CRUDAccessPage(string pageId)
        {
            var pages = this._ContextualMan.CookieClaims.PageIds;
            foreach (var page in pages)
            {
                if (page.PageId == pageId)
                {
                    var crud = new UserAccessCRUD
                    {
                        Create = page.IsCreate,
                        Read = page.IsRead,
                        Update = page.IsUpdate,
                        Delete = page.IsDelete
                    };
                    return crud;
                }
            }

            var defaultcrud = new UserAccessCRUD
            {
                Create = false,
                Delete = false,
                Update = false,
                Read = false
            };
            return defaultcrud;
        }

        public List<string> PageAccess()
        {
            var pages = this._ContextualMan.CookieClaims.PageIds.Select(Q => Q.PageId).ToList();
            return pages;
        }

    }
}
