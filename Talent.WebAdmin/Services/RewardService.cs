using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class RewardService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public RewardService(TalentContext context, ClaimHelper claim)
        {
            this.DB = context;
            this.ClaimMan = claim;
        }

        /// <summary>
        /// get all data based on filter
        /// </summary>
        /// <param name="filter">filter data</param>
        /// <returns>grid view reward</returns>
        public async Task<RewardGridModel> GetAllRewardAsync(RewardFilterModel filter)
        {
            var query = from r in DB.Rewards
                        join rt in DB.RewardTypes on r.RewardTypeId equals rt.RewardTypeId
                        join rp in DB.RewardPoints on r.RewardId equals rp.RewardId
                        join rpt in DB.RewardPointTypes on rp.RewardPointTypeId equals rpt.RewardPointTypeId
                        where r.IsDeleted == false
                        select new
                        {
                            rewards = r,
                            rewardTypes = rt,
                            rewardPoints = rp,
                            rewardPointTypes = rpt
                        };

            if (filter.DateStart != null && filter.DateEnd != null)
            {
                var newStartDate = filter.DateStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.rewards.CreatedAt >= startDate && Q.rewards.CreatedAt <= endDate) || (Q.rewards.UpdatedAt >= startDate && Q.rewards.UpdatedAt <= endDate));
            }

            if (filter.RewardTypeId != null)
            {
                query = query.Where(Q => Q.rewards.RewardTypeId == filter.RewardTypeId);
            }

            if (filter.IsActive != null)
            {
                query = query.Where(Q => Q.rewards.IsActive == filter.IsActive);
            }

            if (!String.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(Q => Q.rewards.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.TypeOfPoints != null)
            {
                query = query.Where(Q => Q.rewardPoints.RewardPointTypeId == filter.TypeOfPoints);
            }

            var totalDataFilter = await query.CountAsync();

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.rewards.Name);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.rewards.Name);
                    break;
                case "typeOfReward":
                    query = query.OrderBy(Q => Q.rewardTypes.Name);
                    break;
                case "typeOfRewardDesc":
                    query = query.OrderByDescending(Q => Q.rewardTypes.Name);
                    break;
                case "status":
                    query = query.OrderBy(Q => Q.rewards.IsActive);
                    break;
                case "statusDesc":
                    query = query.OrderByDescending(Q => Q.rewards.IsActive);
                    break;
                case "points":
                    query = query.OrderBy(Q => Q.rewardPoints.RewardPointTypeId);
                    break;
                case "pointsDesc":
                    query = query.OrderByDescending(Q => Q.rewardPoints.RewardPointTypeId);
                    break;
                case "CreatedAt":
                    query = query.OrderBy(Q => Q.rewards.CreatedAt);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.rewards.CreatedAt);
                    break;
                case "UpdatedAt":
                    query = query.OrderBy(Q => Q.rewards.UpdatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.rewards.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.rewards.UpdatedAt);
                    break;
            }

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            query = query.Skip((int)skipCount).Take(filter.PageSize);

            var rewards = await query.Select(Q => new RewardGridViewModel
            {
                RewardId = Q.rewards.RewardId,
                RewardName = Q.rewards.Name,
                TypeOfReward = Q.rewardTypes.Name,
                TypeOfPoint = Q.rewardPointTypes.Name,
                Status = Q.rewards.IsActive == true ? ActiveEnum.Active : ActiveEnum.InActive,
                CreatedAt = Q.rewards.CreatedAt,
                UpdateAt = Q.rewards.UpdatedAt
            }).ToListAsync();

            var grid = new RewardGridModel
            {
                grid = rewards,
                TotalFilterData = totalDataFilter
            };

            return grid;
        }

        /// <summary>
        /// insert reward to database
        /// </summary>
        /// <param name="model">reward form</param>
        /// <returns>boolean, true if success and otherwise</returns>
        public async Task<bool> InsertRewardAsync(RewardCreateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await this.DB.Rewards.AnyAsync(Q => Q.Name.ToLower() == model.RewardName.ToLower() && Q.IsDeleted == false);
            if (isExist)
            {
                return false;
            }

            var newReward = new Rewards
            {
                Name = model.RewardName,
                RewardTypeId = model.RewardTypeId,
                ModuleId = model.ModuleId,
                CoachId = model.CoachId,
                EventId = model.EventId,
                StartDate = model.StartDate == null ? (DateTime?)null : model.StartDate.Value.ToIndonesiaTimeZone(),
                EndDate = model.EndDate == null ? (DateTime?)null : model.EndDate.Value.ToIndonesiaTimeZone(),
                IsActive = model.IsActive,
                Description = model.Description,
                TermsAndConditions = model.TermsAndConditions,
                HowToUse = model.HowToUse,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
            };

            this.DB.Rewards.Add(newReward);
            //await this.DB.SaveChangesAsync();

            if (model.RewardRequiredLearningPoints > 0)
            {
                this.DB.RewardPoints.Add(new RewardPoints
                {
                    RewardId = newReward.RewardId,
                    RewardPointTypeId = RewardPointTypeEnum.LearningPointType,
                    Score = model.RewardRequiredLearningPoints.GetValueOrDefault()
                });
            }

            if (model.RewardRequiredTeachingPoints > 0)
            {
                this.DB.RewardPoints.Add(new RewardPoints
                {
                    RewardId = newReward.RewardId,
                    RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                    Score = model.RewardRequiredTeachingPoints.GetValueOrDefault()
                });
            }

            if (model.RewardRequiredTotalPoints > 0)
            {
                this.DB.RewardPoints.Add(new RewardPoints
                {
                    RewardId = newReward.RewardId,
                    RewardPointTypeId = RewardPointTypeEnum.TotalPointType,
                    Score = model.RewardRequiredTotalPoints.GetValueOrDefault()
                });
            }

            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Get reward by id
        /// </summary>
        /// <param name="rewardId"></param>
        /// <returns></returns>
        public async Task<RewardViewDetailModel> GetRewardById(int rewardId)
        {
            var data = await (from r in DB.Rewards

                              join m in DB.Modules on r.ModuleId equals m.ModuleId into mNUll
                              from m1 in mNUll.DefaultIfEmpty()

                              join c in DB.Coaches on r.CoachId equals c.CoachId into cNull
                              from c1 in cNull.DefaultIfEmpty()
                              join em in DB.Employees on c1.EmployeeId equals em.EmployeeId into emNull
                              from em1 in emNull.DefaultIfEmpty()

                              join ev in DB.Events on r.EventId equals ev.EventId into eNull
                              from ev1 in eNull.DefaultIfEmpty()

                              join rt in DB.RewardTypes on r.RewardTypeId equals rt.RewardTypeId

                              where r.RewardId == rewardId

                              select new RewardViewDetailModel
                              {
                                  RewardId = r.RewardId,
                                  RewardType = new RewardTypeDropdownModel
                                  {
                                      RewardTypeId = rt.RewardTypeId,
                                      RewardTypeName = rt.Name
                                  },
                                  Module = new ModuleDropdownModel
                                  {
                                      ModuleId = r.ModuleId.GetValueOrDefault(),
                                      ModuleName = m1.ModuleName
                                  },
                                  Coach = new CoachDropdownModel
                                  {
                                      CoachId = r.CoachId.GetValueOrDefault(),
                                      CoachName = em1.EmployeeName
                                  },
                                  Event = new EventDropdownModel
                                  {
                                      EventId = r.EventId.GetValueOrDefault(),
                                      EventName = ev1.Name
                                  },
                                  StartDate = r.StartDate,
                                  EndDate = r.EndDate,
                                  Description = r.Description,
                                  HowToUse = r.HowToUse,
                                  TermsAndConditions = r.TermsAndConditions,
                                  RewardName = r.Name,
                                  IsActive = r.IsActive,
                                  RewardRequiredLearningPoints = 0,
                                  RewardRequiredTeachingPoints = 0,
                                  RewardRequiredTotalPoints = 0
                              }).FirstOrDefaultAsync();

            data.RewardRequiredLearningPoints = await this.DB.RewardPoints
                .Where(Q => Q.RewardId == rewardId && Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType)
                .Select(Q => Q.Score)
                .FirstOrDefaultAsync();

            data.RewardRequiredTeachingPoints = await this.DB.RewardPoints
                .Where(Q => Q.RewardId == rewardId && Q.RewardPointTypeId == RewardPointTypeEnum.TeachingPointType)
                .Select(Q => Q.Score)
                .FirstOrDefaultAsync();

            data.RewardRequiredTotalPoints = await this.DB.RewardPoints
                .Where(Q => Q.RewardId == rewardId && Q.RewardPointTypeId == RewardPointTypeEnum.TotalPointType)
                .Select(Q => Q.Score)
                .FirstOrDefaultAsync();

            return data;
        }

        /// <summary>
        /// update the reward
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRewardAsync(RewardUpdateModel model)
        {
            var data = await this.DB.Rewards.Where(Q => Q.RewardId == model.RewardId).FirstOrDefaultAsync();
            var isChanged = data.Name.ToLower() != model.RewardName.ToLower();
            var isExist = await this.DB.Rewards.AsNoTracking().AnyAsync(Q => Q.Name.ToLower() == model.RewardName.ToLower() && Q.IsDeleted == false);

            if (isChanged && isExist)
            {
                return false;
            }


            data.Name = model.RewardName;
            data.RewardTypeId = model.RewardTypeId;
            data.ModuleId = model.ModuleId;
            data.CoachId = model.CoachId;
            data.EventId = model.EventId;
            data.Description = model.Description;
            data.StartDate = model.StartDate == null ? (DateTime?)null : model.StartDate.Value.ToIndonesiaTimeZone();
            data.EndDate = model.EndDate == null ? (DateTime?)null : model.EndDate.Value.ToIndonesiaTimeZone();
            data.TermsAndConditions = model.TermsAndConditions;
            data.HowToUse = model.HowToUse;
            data.IsActive = model.IsActive;
            data.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            data.UpdatedBy = ClaimMan.GetLoginUserId();

            //await this.DB.SaveChangesAsync();

            var oldDataPoint = await this.DB.RewardPoints.Where(Q => Q.RewardId == model.RewardId).ToListAsync();
            this.DB.RemoveRange(oldDataPoint);
            //await this.DB.SaveChangesAsync();

            if (model.RewardRequiredLearningPoints > 0)
            {
                this.DB.RewardPoints.Add(new RewardPoints
                {
                    RewardId = model.RewardId,
                    RewardPointTypeId = RewardPointTypeEnum.LearningPointType,
                    Score = model.RewardRequiredLearningPoints.GetValueOrDefault()
                });
            }

            if (model.RewardRequiredTeachingPoints > 0)
            {
                this.DB.RewardPoints.Add(new RewardPoints
                {
                    RewardId = model.RewardId,
                    RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                    Score = model.RewardRequiredTeachingPoints.GetValueOrDefault()
                });
            }

            if (model.RewardRequiredTotalPoints > 0)
            {
                this.DB.RewardPoints.Add(new RewardPoints
                {
                    RewardId = model.RewardId,
                    RewardPointTypeId = RewardPointTypeEnum.TotalPointType,
                    Score = model.RewardRequiredTotalPoints.GetValueOrDefault()
                });
            }

            await this.DB.SaveChangesAsync();


            return true;
        }

        /// <summary>
        /// soft delete reward data
        /// </summary>
        /// <param name="rewardId"></param>
        /// <returns></returns>
        public async Task DeleteRewardAsync(int rewardId)
        {
            var rewardData = await this.DB.Rewards.Where(Q => Q.RewardId == rewardId).FirstOrDefaultAsync();

            rewardData.IsDeleted = true;

            await this.DB.SaveChangesAsync();
        }
    }
}
