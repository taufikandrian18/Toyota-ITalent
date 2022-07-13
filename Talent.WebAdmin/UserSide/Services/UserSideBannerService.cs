using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Enums;
namespace Talent.WebAdmin.UserSide.Services
{
    /// <summary>
    /// Service class for providing various methods for interacting with banner data.
    /// </summary>
    public class UserSideBannerService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideBannerService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        /// <summary>
        /// Get all banner.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideBannerModel>> GetBanners(int itemPerPage, int pageIndex)
        {
            var currentDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var bannersTemp = await (from b in this.DB.Banners.AsNoTracking()
                                     join mp in this.DB.MobilePages.AsNoTracking() on b.MobilePageId equals mp.MobilePageId into mpa
                                     from mpaa in mpa.DefaultIfEmpty()
                                     join bt in this.DB.BannerTypes.AsNoTracking() on b.BannerTypeId equals bt.BannerTypeId
                                     join bl in this.DB.Blobs.AsNoTracking() on b.BlobId equals bl.BlobId
                                     join c in this.DB.Coaches.AsNoTracking() on b.ContentId equals c.CoachId into clj
                                     from c in clj.DefaultIfEmpty()
                                     where b.ApprovedAt.HasValue && b.IsDeleted == false && (b.StartDate.HasValue == false || currentDate.Date >= b.StartDate.Value.Date && currentDate.Date <= b.EndDate.Value.Date)
                                     select new
                                     {
                                         b.ApprovedAt,
                                         BannerId = b.BannerId.ToString(),
                                         BannerTypeId = b.BannerTypeId.ToString(),
                                         BannerTypeName = bt.Name,
                                         BlobId = b.BlobId.ToString(),
                                         ContentId = b.ContentId.ToString(),
                                         CreatedDate = b.CreatedAt,
                                         b.StartDate,
                                         b.Name,
                                         b.Description,
                                         b.EndDate,
                                         b.MobilePageId,
                                         MobilePageRoute = mpaa.Route,
                                         bl.Mime,
                                         b.UpdatedAt,
                                         CoachEmployeeId = c.EmployeeId,
                                     })
                                 .OrderByDescending(Q => Q.UpdatedAt)
                                 .Skip((pageIndex - 1) * itemPerPage)
                                 .Take(itemPerPage)
                                 .ToListAsync();

            //bannersTemp yang Learning
            var setupLearningTemp = bannersTemp.Where(Q => Q.MobilePageId == 18).Select(Q => int.Parse(Q.ContentId)).ToList();
            //var setupLearnings = new List<LearningId>();
            //if (setupLearningTemp.Count() > 0)
            //{
            //Setup Learning
            var setupLearnings = await this.DB.SetupLearning.Where(Q => setupLearningTemp.Contains(Q.SetupLearningId)).Select(Q => new LearningId { CourseId = Q.CourseId, SetupModuleId = Q.SetupModuleId, PopQuizId = Q.PopQuizId, SetupLearningId = Q.SetupLearningId }).ToListAsync();

            var courseId = setupLearnings.Select(Q => Q.CourseId);

            //Get Training Id
            var trainings = await this.DB.Trainings.Where(Q => courseId.Contains(Q.CourseId) && Q.IsDeleted == false).Select(Q => new { Q.CourseId, Q.TrainingId }).OrderByDescending(Q => Q.TrainingId).ToListAsync();
            //}

            //Ampunin Dosa Saya --Albert
            foreach (var setupLearning in setupLearnings)
            {
                //kalau bukan course maka di lewatin aja!
                if (setupLearning.CourseId == null)
                {
                    continue;
                }

                foreach (var training in trainings)
                {
                    if (setupLearning.CourseId == training.CourseId)
                    {
                        setupLearning.TrainingId = training.TrainingId;
                        setupLearning.CourseId = training.CourseId;
                    }
                }
            }

            var banners = bannersTemp
                .Select(async Q => new UserSideBannerModel
                {
                    ApprovedAt = Q.ApprovedAt,
                    BannerId = Q.BannerId,
                    BannerTypeId = Q.BannerTypeId,
                    BannerTypeName = Q.BannerTypeName,
                    BlobId = Q.BlobId,
                    ImageUrl = await FileService.GetFileAsync(Q.BlobId, Q.Mime),
                    ContentId = Q.MobilePageId == MobilePageEnum.Coach ? Q.CoachEmployeeId : Q.ContentId,
                    CreatedDate = Q.CreatedDate,
                    StartDate = Q.StartDate,
                    Name = Q.Name,
                    Description = Q.Description,
                    EndDate = Q.EndDate,
                    MobilePageId = Q.MobilePageId,
                    MobilePageRoute = Q.MobilePageRoute,
                })
                .Select(Q => Q.Result)
                .ToList();

            List<string> flags = new List<string>();
            foreach (var banner in banners)
            {
                if (banner.MobilePageId == 18)
                {
                    var content = setupLearnings.Where(Q => Q.SetupLearningId == int.Parse(banner.ContentId)).FirstOrDefault();

                    if (content == null)
                    {
                        flags.Add(banner.BannerId);
                    }
                    else
                    {
                        banner.ContentId = null;
                        banner.CourseId = content.CourseId;
                        banner.SetupModuleId = content.SetupModuleId;
                        banner.PopQuizId = content.PopQuizId;
                        banner.TrainingId = content.TrainingId;
                    }
                }
            }

            banners.RemoveAll(Q => flags.Contains(Q.BannerId));

            return banners;
        }
    }
    public class LearningId
    {
        public int? TrainingId { get; set; }
        public int? CourseId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        public int? SetupLearningId { get; set; }
    }
}
