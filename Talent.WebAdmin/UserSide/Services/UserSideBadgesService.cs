using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideBadgesService
    {
        private readonly TalentContext DB;

        public UserSideBadgesService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<UserSideBadgesViewModel> GetBadges(string employeeId)
        {
            var queryBadges = await (from eb in this.DB.EmployeeBadges.AsNoTracking()
                                     join b in this.DB.Ebadges.AsNoTracking()
                                     on eb.EbadgeId equals b.EbadgeId

                                     join t in this.DB.Topics.AsNoTracking()
                                     on eb.TopicId equals t.TopicId

                                     where eb.EmployeeId.ToLower() == employeeId.ToLower()
                                     select new UserSideBadgesModel
                                     {
                                         EmployeeId = eb.EmployeeId,
                                         EBadgesId = b.EbadgeId,
                                         EBadgesName = b.EbadgeName,
                                         TopicId = t.TopicId,
                                         TopicName = t.TopicName,
                                         TopicDescription = t.TopicDescription
                                     }).ToListAsync();

            var gold = queryBadges.Where(Q => Q.EBadgesId == (int)Enums.Badge.Gold).ToList();
            var silver = queryBadges.Where(Q => Q.EBadgesId == (int)Enums.Badge.Silver).ToList();
            var bronze = queryBadges.Where(Q => Q.EBadgesId == (int)Enums.Badge.Bronze).ToList();

            var goldTopicId = gold.Select(Q => Q.TopicId);
            var silverTopicId = silver.Select(Q => Q.TopicId);
            var bronzeTopicId = bronze.Select(Q => Q.TopicId);


            var coach = await this.DB.Coaches.Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower() && Q.CoachIsActive == true).FirstOrDefaultAsync();

            if (coach != null)
            {
                var coachQueryEbadges = await (
                    from ctm in this.DB.CoachTopicMappings
                    join t in this.DB.Topics on ctm.TopicId equals t.TopicId
                    join eb in this.DB.Ebadges on t.EbadgeId equals eb.EbadgeId
                    where ctm.CoachId == coach.CoachId
                    select new UserSideBadgesModel
                    {

                        EmployeeId = coach.EmployeeId,
                        EBadgesId = eb.EbadgeId,
                        EBadgesName = eb.EbadgeName,
                        TopicId = t.TopicId,
                        TopicName = t.TopicName,
                        TopicDescription = t.TopicDescription
                    }).ToListAsync();

                var goldCoach = coachQueryEbadges.Where(Q => Q.EBadgesId == (int)Enums.Badge.Gold && !goldTopicId.Contains(Q.TopicId)).ToList();
                var silverCoach = coachQueryEbadges.Where(Q => Q.EBadgesId == (int)Enums.Badge.Silver && !silverTopicId.Contains(Q.TopicId)).ToList();
                var bronzeCoach = coachQueryEbadges.Where(Q => Q.EBadgesId == (int)Enums.Badge.Bronze && !bronzeTopicId.Contains(Q.TopicId)).ToList();

                gold.AddRange(goldCoach);
                silver.AddRange(silverCoach);
                bronze.AddRange(bronzeCoach);
            }

            var dataBadges = new UserSideBadgesViewModel
            {
                ListBadgesGold = gold.Distinct().ToList(),
                ListBadgesSilver = silver.Distinct().ToList(),
                ListBadgesBronze = bronze.Distinct().ToList()
            };
            return dataBadges;
        }

        public async Task<UserSideTotalBadgeModel> GetTotalBadge(string employeeId)
        {
            var queryBadge = await this
                .DB
                .EmployeeBadges
                .AsNoTracking()
                .Where(Q => Q.EmployeeId == employeeId)
                .Select(Q => Q.EbadgeId)
                .ToListAsync();

            var dataTotalBadge = new UserSideTotalBadgeModel
            {
                Bronze = queryBadge.Where(Q => Q.Value == (int)Enums.Badge.Bronze).Count(),
                Silver = queryBadge.Where(Q => Q.Value == (int)Enums.Badge.Silver).Count(),
                Gold = queryBadge.Where(Q => Q.Value == (int)Enums.Badge.Gold).Count()
            };

            return dataTotalBadge;
        }

        public async Task<UserSideTotalBadgeModel> GetTotalBadgeCoach(string employeeId)
        {
            var queryBadge = await (from c in this.DB.Coaches
                                    join tm in this.DB.CoachTopicMappings.AsNoTracking() on c.CoachId equals tm.CoachId
                                    join t in this.DB.Topics.AsNoTracking() on tm.TopicId equals t.TopicId

                                    where c.EmployeeId.ToLower() == employeeId.ToLower()
                                    select new
                                    {
                                        t.EbadgeId
                                    }).ToListAsync();

            var dataTotalBadge = new UserSideTotalBadgeModel
            {
                Bronze = queryBadge.Where(Q => Q.EbadgeId == (int)Enums.Badge.Bronze).Count(),
                Silver = queryBadge.Where(Q => Q.EbadgeId == (int)Enums.Badge.Silver).Count(),
                Gold = queryBadge.Where(Q => Q.EbadgeId == (int)Enums.Badge.Gold).Count()
            };

            var employeeEbadges = await this.DB.EmployeeBadges.Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower()).Select(Q => Q.EbadgeId.Value).ToListAsync();

            dataTotalBadge.Bronze += employeeEbadges.Where(Q => Q == (int)Enums.Badge.Bronze).Count();
            dataTotalBadge.Silver += employeeEbadges.Where(Q => Q == (int)Enums.Badge.Silver).Count();
            dataTotalBadge.Gold += employeeEbadges.Where(Q => Q == (int)Enums.Badge.Gold).Count();

            return dataTotalBadge;
        }
    }
}
