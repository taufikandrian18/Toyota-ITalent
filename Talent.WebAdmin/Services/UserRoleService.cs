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
    public class UserRoleService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;

        public UserRoleService(TalentContext talentContext, IContextualService contextual)
        {
            this.DB = talentContext;
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
        /// Insert User Role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertUserRole(UserRoleModelCreate model)
        {
            var findRole = await this.DB.Roles.Where(Q => Q.PositionId == model.Position.PositionId || Q.Name.ToLower() == model.UserRoleName.ToLower()).FirstOrDefaultAsync();

            if(findRole != null){
                return false;
            }

            var insertRole = new Roles
            {
                
                Name = model.UserRoleName,
                Description = model.RoleDescription,
                IsTamPeople = Convert.ToBoolean(model.TypeOfPeople),

                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = GetUserLogin(),
                UpdatedBy = GetUserLogin(),
            };

          

            if (model.TypeOfPeople == true)
            {
                insertRole.PositionId = model.Position.PositionId;
                this.DB.Roles.Add(insertRole);
            }

            else
            {
                insertRole.PositionId = model.Position.PositionId;
                insertRole.DealerPeopleCategoryCode = model.DealerCategory.CategoryId;

                this.DB.Roles.Add(insertRole);
            }
            await this.DB.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update / Edit User Role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserRole(UserRoleModelUpdate model)
        {
            var role = await this
                .DB
                .Roles
                .Where(Q => Q.RoleId == model.UserRoleId)
                .FirstOrDefaultAsync();

            var findDuplicate = await this.DB.Roles.Where(Q => Q.PositionId == model.Position.PositionId).FirstOrDefaultAsync();

            if(findDuplicate != null)
            {
                if(findDuplicate != role)
                {
                    return false;
                }
            }

            role.Name = model.UserRoleName;
            role.Description = model.RoleDescription;
            role.IsTamPeople = Convert.ToBoolean(model.TypeOfPeople);

            if (model.TypeOfPeople == false)
            {
                role.PositionId = model.Position.PositionId;
                role.DealerPeopleCategoryCode = null;
                role.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                role.UpdatedBy = GetUserLogin();
                this.DB.Roles.Update(role);
            }
            else
            {
                role.PositionId = model.Position.PositionId;
                role.DealerPeopleCategoryCode = model.DealerCategory.CategoryId;
                role.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                role.UpdatedBy = GetUserLogin();
                this.DB.Roles.Update(role);
            };
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<UserRoleModelUpdate> GetDataUpdate(int userRoleId)
        {
            var query = await (from r in DB.Roles
                               join p in DB.Positions on r.PositionId equals p.PositionId into rp
                               from p in rp.DefaultIfEmpty()
                               join c in DB.DealerPeopleCategories on r.DealerPeopleCategoryCode equals c.DealerPeopleCategoryCode into pc
                               from c in pc.DefaultIfEmpty()
                               where r.RoleId == userRoleId
                               select new UserRoleModelUpdate
                               {
                                   UserRoleId = r.RoleId,
                                   UserRoleName = r.Name,
                                   RoleDescription = r.Description,
                                   TypeOfPeople = r.IsTamPeople,
                                   UpdatedAt = r.UpdatedAt,
                                   Position = new PositionDropdownModel
                                   {
                                       PositionId = p.PositionId,
                                       PositionName = p.PositionName
                                   },
                                   DealerCategory = new CategoryDropdownModel
                                   {
                                       CategoryId = r.DealerPeopleCategoryCode,
                                       CategoryName = c.Name
                                   }
                               }).FirstOrDefaultAsync();
            return query;
        }

        /// <summary>
        /// Delete User Role by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserRole(int id)
        {
            var role = await this
                .DB
                .Roles
                .Where(Q => Q.RoleId == id)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return false;
            }

            try
            {
                this.DB.Roles.Remove(role);
                await this.DB.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get User Roles & Paginate
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<UserRoleGridModel> GetAllUserRoles(UserGridFilter roleFilter)
        {
            var query = (from r in DB.Roles
                         join p in DB.Positions on r.PositionId equals p.PositionId into rp
                         from p in rp.DefaultIfEmpty()
                         join c in DB.DealerPeopleCategories on r.DealerPeopleCategoryCode equals c.DealerPeopleCategoryCode into pc
                         from c in pc.DefaultIfEmpty()

                         select new
                         {
                             r.RoleId,
                             r.Name,
                             r.Description,
                             r.IsTamPeople,
                             r.CreatedAt,
                             r.UpdatedAt,
                             r.PositionId,
                             p.PositionName,
                             r.DealerPeopleCategoryCode,
                             categoryName = c.Name
                         }).OrderByDescending(Q => Q.UpdatedAt).AsQueryable();

            var totalData = await query.CountAsync();

            if (string.IsNullOrEmpty(roleFilter.UserRole) == false)
            {
                query = query.Where(Q => Q.Name.Contains(roleFilter.UserRole));
            }

            if (roleFilter.TypeofPeople != null)
            {
                query = query.Where(Q => Q.IsTamPeople == roleFilter.TypeofPeople);
            }

            if (string.IsNullOrEmpty(roleFilter.Position) == false)
            {
                query = query.Where(Q => Q.PositionName.Contains(roleFilter.Position));
            }

            if (roleFilter.CreatedAt != null && roleFilter.UpdatedAt != null)
            {
                var newStartDate = roleFilter.CreatedAt.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = roleFilter.UpdatedAt.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));

            }

            switch (roleFilter.SortBy)
            {
                case "userRole":
                    query = query.OrderBy(Q => Q.Name);
                    break;
                case "userRoleDesc":
                    query = query.OrderByDescending(Q => Q.Name);
                    break;
                case "typeofPeople":
                    query = query.OrderBy(Q => Q.IsTamPeople);
                    break;
                case "typeofPeopleDesc":
                    query = query.OrderByDescending(Q => Q.IsTamPeople);
                    break;
                case "position":
                    query = query.OrderBy(Q => Q.PositionName);
                    break;
                case "positionDesc":
                    query = query.OrderByDescending(Q => Q.PositionName);
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
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }


            var totalDataFilter = await query.CountAsync();
            var skipCount = Math.Ceiling((decimal)(roleFilter.PageIndex - 1) * roleFilter.PageSize);
            query = query.Skip((int)skipCount).Take(roleFilter.PageSize);

            var userRole = await query.Select(Q => new UserRoleModelView
            {
                UserRoleId = Q.RoleId,
                UserRoleName = Q.Name,
                TypeOfPeople = Q.IsTamPeople == true ? Constant.TypeofPeople.TAM : Constant.TypeofPeople.Dealer,
                Position = new PositionDropdownModel
                {
                    PositionId = Q.PositionId,
                    PositionName = Q.PositionName
                },
                Category = new CategoryDropdownModel
                {
                    CategoryId = Q.DealerPeopleCategoryCode,
                    CategoryName = Q.categoryName
                },
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt
            }).ToListAsync();

            var grid = new UserRoleGridModel
            {
                UserRole = userRole,
                TotalData = totalData,
                TotalDataFilter = totalDataFilter
            };

            return grid;
        }

        /// <summary>
        /// Get User Role's data for view detail
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        public async Task<UserRoleModelViewDetail> GetUserRoleDetail(int userRoleId)
        {
            var query = await (from r in DB.Roles
                               join p in DB.Positions on r.PositionId equals p.PositionId into rp
                               from p in rp.DefaultIfEmpty()
                               join c in DB.DealerPeopleCategories on r.DealerPeopleCategoryCode equals c.DealerPeopleCategoryCode into pc
                               from c in pc.DefaultIfEmpty()
                               where r.RoleId == userRoleId
                               select new UserRoleModelViewDetail
                               {
                                   UserRoleId = r.RoleId,
                                   UserRoleName = r.Name,
                                   RoleDescription = r.Description,
                                   TypeOfPeople = r.IsTamPeople,
                                   UpdatedAt = r.UpdatedAt,
                                   Position = new PositionDropdownModel
                                   {
                                       PositionId = p.PositionId,
                                       PositionName = p.PositionName
                                   },
                                   DealerCategory = new CategoryDropdownModel
                                   {
                                       CategoryId = r.DealerPeopleCategoryCode,
                                       CategoryName = c.Name
                                   }
                               }).FirstOrDefaultAsync();

            return query;
        }

        /// <summary>
        /// To check if Role's Name is exist when create
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> CheckRoleName(string roleName)
        {
            var userRoleName = await this
                .DB
                .Roles
                .Where(Q => Q.Name.ToLower() == roleName.ToLower())
                .AsNoTracking()
                .AnyAsync();

            return userRoleName;
        }

        /// <summary>
        /// to check role name when edit / update by checking the id
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> CheckRoleNameByRoleId(int roleId, string roleName)
        {
            var userRole = await this
                .DB
                .Roles
                .Where(Q => Q.RoleId == roleId)
                .FirstOrDefaultAsync();

            if (userRole.Name.ToLower() == roleName.ToLower())
            {
                return true;
            }

            else return false;
        }
    }
}
