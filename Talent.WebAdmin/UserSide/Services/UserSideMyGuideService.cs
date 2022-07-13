using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideMyGuideService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;

        public UserSideMyGuideService(TalentContext context, IFileStorageService fileService)
        {
            this.DB = context;
            this.FileMan = fileService;
        }

        public async Task<UserSideMyGuideAPIModel> GetAllGuideAsync()
        {
            var query = from g in DB.Guides
                        join gt in DB.GuideTypes on g.GuideTypeId equals gt.GuideTypeId
                        join b in DB.Blobs on g.BlobId equals b.BlobId
                        where g.ApprovedAt != null
                        select new
                        {
                            g.Name,
                            g.Description,
                            gt.GuideTypeId,
                            GuideTypeName = gt.Name,
                            b.BlobId,
                            b.Mime
                        };

            var allGuides = await query.Select(Q => new UserSideMyGuideModel
            {
                GuideName = Q.Name,
                Description = Q.Description,
                GuideTypeId = Q.GuideTypeId,
                GuideType = Q.GuideTypeName,
                ImageURL = this.FileMan.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).ToListAsync();

            UserSideMyGuideAPIModel data = new UserSideMyGuideAPIModel();
            data.GetStarted = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.GetStarted)).ToList();
            data.FindYourPath = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.FindYourPath)).ToList();
            data.BuildYourExpertise = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.BuildYourExpertise)).ToList();
            data.GrowYourCareer = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.GrowYourCareer)).ToList();
            data.ConnectWithPeople = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.ConnectWithPeople)).ToList();
            data.RewardAndRecognition = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.RewardAndRecognition)).ToList();
            data.Tutorial = allGuides.Where(Q => Q.GuideTypeId == Convert.ToInt32(GuideType.Tutorial)).ToList();

            return data;
        }

        public async Task<UserSideTutorialGuideModel> GetTutorialAsync()
        {
            Random rand = new Random();

            var tutorialData = from g in DB.Guides
                               join b in DB.Blobs on g.BlobId equals b.BlobId
                               where g.GuideTypeId == (int)GuideType.Tutorial
                               select new
                               {
                                   g.Name,
                                   g.Description,
                                   b.BlobId,
                                   b.Mime
                               };

            var totalData = await tutorialData.CountAsync();
            int toSkip = rand.Next(0, totalData);

            var data = await tutorialData.Select(Q => new UserSideTutorialGuideModel
            {
                Title = Q.Name,
                Description = Q.Description,
                ImageUrl = this.FileMan.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).Skip(toSkip).Take(1).FirstOrDefaultAsync();

            return data;
        }
    }
}
