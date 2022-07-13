using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideLoginService
    {
        private readonly TalentContext DB;
        private readonly IHttpClientFactory HttpClientFactoryMan;
        private readonly AppSettings Settings;
        private readonly UserSideAuthService AuthService;

        public UserSideLoginService(TalentContext db, IHttpClientFactory ihcs, AppSettings set, UserSideAuthService authService)
        {
            this.DB = db;
            this.HttpClientFactoryMan = ihcs;
            this.Settings = set;
            this.AuthService = authService;
        }

        public async Task<string> GetUserInfo(PassportJWTModel sso)
        {
            // Get Data From DB
            var user = await DB.Employees.FirstOrDefaultAsync(Q => Q.EmployeeId == sso.EmployeeId);

            if (user == null)
            {
                return null;
            }

            if(user.IsDeleted){
                return null;
            }

            user.LearningPoint = user.LearningPoint + 2;

            //var userPosition = await (from epm in DB.EmployeePositionMappings
            //                          join p in DB.Positions on epm.PositionId equals p.PositionId
            //                          where epm.EmployeeId == user.EmployeeId
            //                          select new UserSidePositionModel
            //                          {
            //                              PositionId = p.PositionId,
            //                              PositionName = p.PositionName,
            //                          }).ToListAsync();

            //userData.EmployeeId = user.EmployeeId;
            //userData.Email = user.EmployeeEmail;
            //userData.Name = user.EmployeeName;
            //userData.Position = userPosition;
            //userData.IsCoach = user.IsCoach;
            //userData.IsTeamLeader = user.IsTeamLeader;
            var MobileSSOClaim = new MobileTalentJWTModel
            {
                Aud = sso.Aud,
                Email = sso.Email,
                EmployeeId = sso.EmployeeId,
                Exp = sso.Exp,
                Expiration = sso.Expiration.AddYears(1),
                Iat = sso.Iat,
                Iss = "Talent",
                IssuedAt = sso.IssuedAt,
                Jti = sso.Jti,
                Roles = sso.Roles,
                Sub = sso.Sub,
                Unique_name = sso.Unique_name
            };

            var TalentToken = AuthService.CreateMobileToken(MobileSSOClaim);
            await DB.SaveChangesAsync();
            return TalentToken;
        }
    }
}
