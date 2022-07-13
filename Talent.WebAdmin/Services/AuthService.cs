using Microsoft.AspNetCore.Http;
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
    public class AuthService
    {
        private readonly TalentContext DB;

        private readonly IContextualService ContextMan;
        private readonly IHttpContextAccessor HttpMan;

        public AuthService(TalentContext db, IContextualService ics, IHttpContextAccessor ihca
            )
        {
            this.DB = db;
            this.ContextMan = ics;
            this.HttpMan = ihca;
        }


        /// <summary>
        /// Used to check whether authenticated user is allowed to access a feature
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public bool IsUserAllowedToAccess(string functionId)
        {
            var functionIds = ContextMan.CookieClaims?.FunctionIds;
            if (functionIds == null || functionIds.Any() == false)
            {
                return false;
            }

            return functionIds.Contains(functionId);
        }

        public async Task<LoginClaims> TryLogin(PassportJWTModel token)
        {
            if (token.Roles.Any() == false)
            {
                return null;
            }

            // Get Data From DB
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeId == token.EmployeeId);

            //Fungsi ini untuk nambah user yang login dari SSO jika ga ada di DB.
            if (user == null)
            {
                return null;
                //user = new Employees
                //{
                //    EmployeeId = token.EmployeeId,
                //    EmployeeEmail = token.Email,
                //    EmployeeName = token.Unique_name,
                //    EmployeeUsername = token.Sub,

                //    LastSeenAt = DateTime.UtcNow,

                //    CreatedAt = DateTime.UtcNow,
                //    CreatedBy = "SSO",
                //    UpdatedAt = DateTime.UtcNow,
                //    UpdatedBy = "SSO"
                //};

                //DB.Add(user);
                //await DB.SaveChangesAsync();
            }

            //Get Previledge
            //var pages = GetAuthorizedPage(token.Roles);
            var pageIds = await GetAuthorizedPage(token.EmployeeId);

            //level if needed
            var positions = await GetPositionAsync(token.EmployeeId);


            //get outlet data
            var outletData = await DB.Employees
                .Where(Q => Q.EmployeeId == user.EmployeeId)
                .AsNoTracking()
                .Select(Q => new { Q.OutletId, Q.Outlet.Name })
                .AsNoTracking()
                .ToListAsync();

            var outletIds = outletData.Select(Q => Q.OutletId).ToList();
            var outletNames = outletData.Select(Q => Q.Name).ToList();

            await HttpMan.HttpContext.Session.LoadAsync();
            HttpMan.HttpContext.Session.SetObjectAsJson("Positions", positions);
            //Buat kasih tw bisa access apa aja dan privilegenya
            HttpMan.HttpContext.Session.SetObjectAsJson("Pages", pageIds);

            if (string.IsNullOrEmpty(GetDealerId(user.OutletId)) == false)
            {
                HttpMan.HttpContext.Session.SetString("DealerId", GetDealerId(user.OutletId));
            }
            if (outletIds != null && outletIds.Any())
            {
                HttpMan.HttpContext.Session.SetObjectAsJson("OutletIds", outletIds);
                HttpMan.HttpContext.Session.SetObjectAsJson("OutletNames", outletNames);
            }

            await HttpMan.HttpContext.Session.CommitAsync();

            var claims = new LoginClaims
            {
                EmployeeId = user.EmployeeId,
                Email = user.EmployeeEmail,
                Name = user.EmployeeName,

                Roles = token.Roles,

                DealerId = GetDealerId(user.OutletId),
                PageIds = pageIds,
                Positions = positions,

                SessionId = token.Jti,
                Username = token.Sub
            };


            return claims;
        }



        public async Task<List<int>> GetPositionAsync(string employeeId)
        {
            var query = (from epm in DB.EmployeePositionMappings
                         where epm.EmployeeId == employeeId
                         select epm.PositionId);

            return await query.ToListAsync();
        }

        public async Task<List<PageCrudModel>> GetAuthorizedPage(string employeeId)
        {

            var query = await DB.GetPrivilegePageMappingData(employeeId).ToListAsync();

            var result = query.Select(Q => new PageCrudModel
            {
                PageId = Q.PageId,
                IsCreate = Q.IsCreate,
                IsRead = Q.IsRead,
                IsUpdate = Q.IsUpdate,
                IsDelete = Q.IsDelete,
            }).ToList();

            return result;
        }

        public string GetDealerId(string outletId)
        {
            var dealerId = DB.Outlets.Where(Q => Q.OutletId == outletId)
                .Select(Q => Q.DealerId)
                .FirstOrDefault();

            return dealerId;
        }

    }
}
