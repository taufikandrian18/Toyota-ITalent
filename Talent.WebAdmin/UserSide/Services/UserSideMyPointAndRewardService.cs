using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    /// <summary>
    /// Service class for providing various methods for interacting with point and reward data.
    /// </summary>
    public class UserSideMyPointAndRewardService
    {
        private readonly TalentContext DB;

        public UserSideMyPointAndRewardService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        /// <summary>
        /// Get point histories based on employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<UserSideHistoryPointModel> GetHistoryPoint(string employeeId)
        {
            // Get current point;
            var currentPoint = await GetPointByEmployeeId(employeeId);

            // All point history.
            var historyPointIncome = await this.DB
                .EmployeePointHistories
                .Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower() && Q.PointTransactionTypeId == (int)PointTransactionTypeEnum.Income)
                .AsNoTracking()
                .ToListAsync();

            // All learning point history.
            var historyLearningPoint = historyPointIncome
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType)
                .GroupBy(Q => new
                {
                    Q.CreatedAt.Year,
                    Q.RewardPointTypeId
                })
                .Select(Q => new UserSideHistoryPointDetailModel
                {
                    Year = Q.Key.Year,
                    RewardPointTypeId = Q.Key.RewardPointTypeId,
                    TotalPoint = Q.Sum(Z => Z.Point)
                })
                .ToList();

            // All teaching point history.
            var historyTeachingPoint = historyPointIncome
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.TeachingPointType)
                .GroupBy(Q => new
                {
                    Q.CreatedAt.Year,
                    Q.RewardPointTypeId
                })
                .Select(Q => new UserSideHistoryPointDetailModel
                {
                    Year = Q.Key.Year,
                    RewardPointTypeId = Q.Key.RewardPointTypeId,
                    TotalPoint = Q.Sum(Z => Z.Point)
                })
                .ToList();

            // All total point history.
            var historyTotalPoint = historyPointIncome
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.TotalPointType)
                .GroupBy(Q => new
                {
                    Q.CreatedAt.Year,
                    Q.RewardPointTypeId
                })
                .Select(Q => new UserSideHistoryPointDetailModel
                {
                    Year = Q.Key.Year,
                    RewardPointTypeId = Q.Key.RewardPointTypeId,
                    TotalPoint = Q.Sum(Z => Z.Point)
                })
                .ToList();

            var pointHistories = new UserSideHistoryPointModel()
            {
                LearningPointHistories = historyLearningPoint,
                TeachingPointHistories = historyTeachingPoint,
                TotalPointHistories = historyTotalPoint,
                CurrentLearningPoint = currentPoint.LearningPoint,
                CurrentTeachingPoint = currentPoint.TeachingPoint,
                CurrentTotalPoint = currentPoint.TotalPoint
            };

            return pointHistories;
        }

        /// <summary>
        /// Get point for spesific employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<UserSideMyPointModel> GetPointByEmployeeId(string employeeId)
        {
            var points = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower())
                .Select(Q => new UserSideMyPointModel
                {
                    LearningPoint = Q.LearningPoint,
                    TeachingPoint = Q.TeachingPoint,
                    TotalPoint = (Q.LearningPoint + Q.TeachingPoint)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return points;
        }

        /// <summary>
        /// Get rewards
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<UserSideRewardModel>> GetRewards(UserSideRewardFilterModel filter)
        {
            var getTodayDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var query = from r in this.DB.Rewards.AsNoTracking()
                        join rt in this.DB.RewardTypes.AsNoTracking() on r.RewardTypeId equals rt.RewardTypeId
                        where r.IsActive == true && r.IsDeleted == false && (r.StartDate == null || (getTodayDate >= r.StartDate && getTodayDate <= r.EndDate))
                        select new
                        {
                            r.RewardId,
                            r.Name,
                            r.RewardTypeId,
                            rewardTypeName = rt.Name,
                            r.StartDate,
                            r.EndDate,
                            r.UpdatedAt
                        };

            // Filter by name.
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(Q => Q.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            // Filter by category.
            if (filter.TypeId != null)
            {
                query = query.Where(Q => Q.RewardTypeId == filter.TypeId);
            }

            // Sorting
            switch (filter.SortBy)
            {
                case "StartDateAsc":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "StartDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;
                case "NameAsc":
                    query = query.OrderBy(Q => Q.Name);
                    break;
                case "NameDesc":
                    query = query.OrderByDescending(Q => Q.Name);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var rewardsTemp = await query
                .Select(Q => new UserSideRewardModel
                {
                    RewardId = Q.RewardId,
                    Name = Q.Name,
                    RewardTypeId = Q.RewardTypeId,
                    RewardTypeName = Q.rewardTypeName,
                    EndDate = Q.EndDate,
                    StartDate = Q.StartDate
                })
                .Skip((filter.PageIndex - 1) * filter.ItemPerPage)
                .Take(filter.ItemPerPage)
                .ToListAsync();

            // Get ids of rewards
            var rewardIds = rewardsTemp
                .Select(Q => Q.RewardId)
                .ToList();

            // Get reward points based on reward ids
            var rewardPointsAll = await this.DB
                .RewardPoints
                .Where(Q => rewardIds.Contains(Q.RewardId))
                .AsNoTracking()
                .ToListAsync();

            foreach (var reward in rewardsTemp)
            {
                reward.LearningPoint = rewardPointsAll
                    .Where(Q => Q.RewardId == reward.RewardId && Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType)
                    .Select(Q => Q.Score)
                    .FirstOrDefault();

                reward.TeachingPoint = rewardPointsAll
                    .Where(Q => Q.RewardId == reward.RewardId && Q.RewardPointTypeId == RewardPointTypeEnum.TeachingPointType)
                    .Select(Q => Q.Score)
                    .FirstOrDefault();

                reward.TotalPoint = rewardPointsAll
                    .Where(Q => Q.RewardId == reward.RewardId && Q.RewardPointTypeId == RewardPointTypeEnum.TotalPointType)
                    .Select(Q => Q.Score)
                    .FirstOrDefault();
            }

            // Filter by point category.
            if (filter.IsUseTeachingPoint == true)
            {
                rewardsTemp = rewardsTemp
                    .Where(Q => Q.TeachingPoint != 0)
                    .ToList();
            }

            return rewardsTemp;
        }

        /// <summary>
        /// Get detail reward.
        /// </summary>
        /// <param name="rewardId"></param>
        /// <returns></returns>
        public async Task<UserSideRewardModel> GetDetailReward(int rewardId)
        {
            var detailReward = await (from r in this.DB.Rewards.AsNoTracking()
                                      join rt in this.DB.RewardTypes.AsNoTracking() on r.RewardTypeId equals rt.RewardTypeId
                                      where r.RewardId == rewardId
                                      select new UserSideRewardModel
                                      {
                                          RewardId = r.RewardId,
                                          Name = r.Name,
                                          RewardTypeId = r.RewardTypeId,
                                          RewardTypeName = rt.Name,
                                          StartDate = r.StartDate,
                                          EndDate = r.EndDate,
                                          Description = r.Description,
                                          HowToUse = r.HowToUse,
                                          TermsAndConditions = r.TermsAndConditions
                                      })
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

            // Get reward point types.
            var rewardPointTypes = await this.DB
                .RewardPointTypes
                .AsNoTracking()
                .ToListAsync();

            // Get reward points based on reward ids
            var rewardPoints = await this.DB
                .RewardPoints
                .Where(Q => Q.RewardId == detailReward.RewardId)
                .AsNoTracking()
                .ToListAsync();

            detailReward.LearningPoint = rewardPoints
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType)
                .Select(Q => Q.Score)
                .FirstOrDefault();

            detailReward.TeachingPoint = rewardPoints
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.TeachingPointType)
                .Select(Q => Q.Score)
                .FirstOrDefault();


            detailReward.TotalPoint = rewardPoints
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.TotalPointType)
                .Select(Q => Q.Score)
                .FirstOrDefault();

            return detailReward;
        }

        /// <summary>
        /// Redeem reward function and logic
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="rewardId"></param>
        /// <returns></returns>
        public async Task<string> RedeemReward(string employeeId, int rewardId)
        {
            // Get employee points
            var employee = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower())
                .FirstOrDefaultAsync();

            var employeeTotalPoint = employee.LearningPoint + employee.TeachingPoint;

            // Reward learning point
            var rewardLearningPoint = await this.DB
                .RewardPoints
                .Where(Q => Q.RewardId == rewardId && Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType)
                .Select(Q => Q.Score)
                .FirstOrDefaultAsync();

            // Reward teaching point
            var rewardTeachingPoint = await this.DB
                .RewardPoints
                .Where(Q => Q.RewardId == rewardId && Q.RewardPointTypeId == RewardPointTypeEnum.TeachingPointType)
                .Select(Q => Q.Score)
                .FirstOrDefaultAsync();

            // Reward total point
            var rewardTotalPoint = await this.DB
                .RewardPoints
                .Where(Q => Q.RewardId == rewardId && Q.RewardPointTypeId == RewardPointTypeEnum.TotalPointType)
                .Select(Q => Q.Score)
                .FirstOrDefaultAsync();

            // check employee point and reward point
            var lessPoints = new List<string>();
            if (rewardLearningPoint > employee.LearningPoint)
            {
                lessPoints.Add("Learning Point");
            }
            if (rewardTeachingPoint > employee.TeachingPoint)
            {
                lessPoints.Add("Teaching Point");
            }
            if (rewardTotalPoint > employeeTotalPoint)
            {
                lessPoints.Add("Total Point");
            }

            // if less point not null, return error
            if (lessPoints.Count > 0)
            {
                var lessPointsString = String.Join(", ", lessPoints.ToArray());
                return $"Please check your points.({lessPointsString})";
            }


            // Update employee point
            employee.LearningPoint = (employee.LearningPoint - rewardLearningPoint);
            employee.TeachingPoint = (employee.TeachingPoint - rewardTeachingPoint);

            // Check Reward mapping is exist or not.
            var isExistRewardMapping = await this.DB
                .EmployeeRewardMappings
                .Where(Q => Q.EmployeeId.ToLower() == employee.EmployeeId.ToLower() && Q.RewardId == rewardId)
                .AnyAsync();

            if (isExistRewardMapping == false)
            {
                var newRewardMapping = new EmployeeRewardMappings
                {
                    EmployeeId = employee.EmployeeId,
                    RewardId = rewardId,
                    RedeemedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                };
                DB.EmployeeRewardMappings.Add(newRewardMapping);

                if (rewardLearningPoint > 0)
                {
                    var newRewardLearningHistory = new EmployeePointHistories
                    {
                        EmployeeId = employee.EmployeeId,
                        RewardPointTypeId = RewardPointTypeEnum.LearningPointType,
                        PointTransactionTypeId = (int)PointTransactionTypeEnum.Outcome,
                        Point = rewardLearningPoint
                    };
                    DB.EmployeePointHistories.Add(newRewardLearningHistory);
                }

                if (rewardTeachingPoint > 0)
                {
                    var newRewardTeachingHistory = new EmployeePointHistories
                    {
                        EmployeeId = employee.EmployeeId,
                        RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                        PointTransactionTypeId = (int)PointTransactionTypeEnum.Outcome,
                        Point = rewardTeachingPoint
                    };
                    DB.EmployeePointHistories.Add(newRewardTeachingHistory);
                }

                await DB.SaveChangesAsync();
                return null;
            }

            return "You have already redeemed this reward.";
        }
    }
}
