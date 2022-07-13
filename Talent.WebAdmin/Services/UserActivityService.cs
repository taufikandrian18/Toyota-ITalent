using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models.UserLogActivity;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class UserActivityService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;

        public UserActivityService(TalentContext db, IContextualService contextual, ClaimHelper hm, IFileStorageService fm, BlobService bm)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.HelperMan = hm;
            this.FileMan = fm;
            this.BlobService = bm;
        }

        /// <summary>
        /// insert user log activity to database
        /// </summary>
        /// <param name="model">create form topic</param>
        /// <returns></returns>
        public async Task InsertUserActivityLog(UserLogActivities model)
        {
            var now = DateTime.UtcNow.ToIndonesiaTimeZone();
            Guid guid = Guid.NewGuid();

            var userLog = new UserLogActivities
            {
                Guid = guid.ToString(),
                EmployeeId = model.EmployeeId,
                Type = model.Type,
                Content = model.Content,
                ContentId = model.ContentId,
                CreatedAt = now
            };

            this.DB.UserLogActivities.Add(userLog);
            await this.DB.SaveChangesAsync();
            return;
        }

        public async Task<List<GeneralUserLogModel>> GetUserActivityLog(string contentType)
        {
            var response = new List<GeneralUserLogModel>();

            if (contentType == "my_tools")
            {
                response.Add(new GeneralUserLogModel
                {
                    Type = "Customer Profile",
                    TotalAccess = 30,
                    Portions = 27.27F
                });

                response.Add(new GeneralUserLogModel
                {
                    Type = "Spec & Comparison",
                    TotalAccess = 10,
                    Portions = 9.09F
                });

                response.Add(new GeneralUserLogModel
                {
                    Type = "SPWA & Test Drive",
                    TotalAccess = 10,
                    Portions = 63.64F
                });
            }
            else if (contentType == "my_learning")
            {
                response.Add(new GeneralUserLogModel
                {
                    Type = "Marketing via Media Social",
                    TotalAccess = 30,
                    Portions = 27.27F
                });

                response.Add(new GeneralUserLogModel
                {
                    Type = "Smart Contract",
                    TotalAccess = 10,
                    Portions = 9.09F
                });

                response.Add(new GeneralUserLogModel
                {
                    Type = "Super Course",
                    TotalAccess = 10,
                    Portions = 63.64F
                });
            }



            return response;
        }
    }
}
