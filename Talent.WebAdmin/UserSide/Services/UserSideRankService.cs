using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideRankService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideRankService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        /// <summary>
        /// Get all dealers for search.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideDealerModel>> GetDealers(int itemPerPage, int pageIndex)
        {
            var dealers = await this.DB
                .Dealers
                .Select(Q => new UserSideDealerModel
                {
                    DealerId = Q.DealerId,
                    DealerName = Q.DealerName
                })
                .AsNoTracking()
                .Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();

            return dealers;
        }

        /// <summary>
        /// Get all positions for search.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSidePositionModel>> GetPositions(int itemPerPage, int pageIndex)
        {
            var positions = await this.DB
                .Positions.Where(Q => Q.PositionName != "*")
                .Select(Q => new UserSidePositionModel
                {
                    PositionId = Q.PositionId,
                    PositionName = Q.PositionName,
                    PositionDescription = Q.PositionDescription
                })
                .AsNoTracking()
                .Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();

            return positions;
        }

        /// <summary>
        /// Get all areas for search.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideAreaModel>> GetAreas(int itemPerPage, int pageIndex)
        {
            var areas = await this.DB
                .Areas
                .Select(Q => new UserSideAreaModel
                {
                    AreaId = Q.AreaId,
                    AreaName = Q.Name
                })
                .AsNoTracking()
                .Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();

            return areas;
        }

        public async Task<List<UserSideRankModel>> GetRankCoach()
        {
            var queryRankCoach = await (from c in DB.Coaches.AsNoTracking()
                                        join e in DB.Employees.AsNoTracking()
                                        on c.EmployeeId equals e.EmployeeId

                                        join b in DB.Blobs.AsNoTracking()
                                        on e.BlobId equals b.BlobId into ab
                                        from b in ab.DefaultIfEmpty()

                                        join epm in DB.EmployeePositionMappings.AsNoTracking()
                                        on e.EmployeeId equals epm.EmployeeId into aepm
                                        from epm in aepm.DefaultIfEmpty()

                                        join p in DB.Positions.AsNoTracking()
                                        on epm.PositionId equals p.PositionId into ap
                                        from p in ap.DefaultIfEmpty()

                                        join o in DB.Outlets.AsNoTracking()
                                        on e.OutletId equals o.OutletId into ao
                                        from o in ao.DefaultIfEmpty()
                                        where e.IsDeleted == false

                                        orderby e.EmployeeExperience descending

                                        select new
                                        {
                                            e.EmployeeId,
                                            e.EmployeeName,
                                            BlobId = e.BlobId == null ? Guid.Empty : e.BlobId,
                                            e.TeachingPoint,
                                            e.LearningPoint,
                                            e.EmployeeExperience,
                                            Mime = string.IsNullOrEmpty(b.Mime) ? "" : b.Mime,
                                            PositionName = string.IsNullOrEmpty(p.PositionName) ? "" : p.PositionName,
                                            OutletName = string.IsNullOrEmpty(o.Name) ? "" : o.Name
                                        })
                                        .ToListAsync();

            var queryLevel = await
                DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            var dataRankCoach = queryRankCoach
                .GroupBy(Q => Q.EmployeeId)
                .Select(async X => new UserSideRankModel
                {
                    EmployeeId = X.First().EmployeeId,
                    EmployeeName = X.First().EmployeeName,
                    PositionName = X.First().PositionName,
                    ImageUrl = X.First().BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.First().BlobId.ToString(), X.First().Mime),
                    Name = X.First().OutletName,
                    EmployeePoint = X.First().LearningPoint + X.First().TeachingPoint,
                    Level = GenerateLevel(queryLevel, X.First().EmployeeExperience)
                })
                .Select(Q => Q.Result)
                .ToList();

            return dataRankCoach;
        }

        /// <summary>
        /// Get rank for all user when user choose current year.
        /// This method works for generate rank all user based on total experience employees in this year.
        /// Total Experience used is obtained from Employees Table joined with employee point history and sum all point learning income user.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<UserSideRankModel>> GetRankAllUserCurrentYear(UserSideRankFilterModel filter, string employeeId)
        {
            // Get master EmployeeLevels for generate level of employees.
            var levels = await this.DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            // Query employee that joined with employee point history.
            // This Query get data employee that have point in this year, with point type is learning point and,
            // Point transaction type is incoming.
            var employeeQueryable = from e in DB.Employees.AsNoTracking()

                                    join epm in DB.EmployeePositionMappings.AsNoTracking()
                                    on e.EmployeeId equals epm.EmployeeId into aepm
                                    from epm in aepm.DefaultIfEmpty()

                                    join o in DB.Outlets.AsNoTracking()
                                    on e.OutletId equals o.OutletId into ao
                                    from o in ao.DefaultIfEmpty()

                                    join d in DB.Dealers.AsNoTracking()
                                    on o.DealerId equals d.DealerId into ad
                                    from d in ad.DefaultIfEmpty()

                                    join a in DB.Areas.AsNoTracking()
                                    on o.AreaId equals a.AreaId into aa
                                    from a in aa.DefaultIfEmpty()

                                    join eph in DB.EmployeePointHistories.AsNoTracking()
                                    on e.EmployeeId equals eph.EmployeeId into aeph
                                    from eph in aeph.DefaultIfEmpty()

                                    where eph.CreatedAt.Year == DateTime.Now.Year && eph.CreatedAt.DayOfYear == DateTime.Now.DayOfYear &&
                                    eph.RewardPointTypeId == RewardPointTypeEnum.LearningPointType &&
                                    eph.PointTransactionTypeId == (int)PointTransactionTypeEnum.Income && e.IsDeleted == false && e.IsDeleted == false
                                    select new
                                    {
                                        eph.EmployeePointHistoryId,
                                        e.EmployeeId,
                                        e.EmployeeExperience,
                                        o.OutletId,
                                        d.DealerId,
                                        a.AreaId,
                                        epm.PositionId,
                                        eph.Point
                                    };
            // Filter by area.
            if (filter.AreaIds != null)
            {
                filter.AreaIds = filter.AreaIds.Where(Q => Q != null).ToList();
                if (filter.AreaIds.Count > 0)
                {
                    employeeQueryable = employeeQueryable.Where(Q => filter.AreaIds.Contains(Q.AreaId));
                }
            }

            // Filter by dealer.
            if (filter.DealerIds != null)
            {
                filter.DealerIds = filter.DealerIds.Where(Q => Q != null).ToList();
                if (filter.DealerIds.Count > 0)
                {
                    employeeQueryable = employeeQueryable.Where(Q => filter.DealerIds.Contains(Q.DealerId));
                }
            }


            // Filter by position.
            if (filter.PositionIds != null)
            {
                filter.PositionIds = filter.PositionIds.Where(Q => Q != null).ToList();
                if (filter.PositionIds.Count > 0)
                {
                    employeeQueryable = employeeQueryable.Where(Q => filter.PositionIds.Contains(Q.PositionId));
                }
            }

            var tempemployeeQueryable = employeeQueryable.Select(Q => new
            {
                Q.EmployeePointHistoryId,
                Q.EmployeeId,
                Q.EmployeeExperience,
                Q.OutletId,
                Q.DealerId,
                Q.AreaId,
                Q.Point,
            }).Distinct().AsQueryable();


            var employeesShowData = new List<UserSideRankModel>();

            // Set current rank = 1, this value will increase when looping employees that ordered by EmployeeExperience.
            var currentRank = 1;

            // Query to select only EmployeeIds to get top 3 employee id, and next rank.
            // This query group by employee id to count point in this year to get Employee Experience.

            var employeeIdsQueryable = tempemployeeQueryable
                .GroupBy(Q => Q.EmployeeId)
                .Select(Q => new
                {
                    EmployeeId = Q.Key,
                    EmployeeExperience = Q.Sum(X => X.Point)
                })
                .OrderByDescending(Q => Q.EmployeeExperience)
                .Select(Q => Q.EmployeeId)
                .AsQueryable();

            // If Employee logged in filtered out
            if (tempemployeeQueryable.Where(Q => Q.EmployeeId == employeeId).Count() == 0)
            {
                var employeeIdsTop10 = await employeeIdsQueryable
                    .Take(10)
                    .ToListAsync();

                // Get Rank of employee detail top 10.
                var nextEmployeeDetailsTop10 = await GetRankQueryable(employeeIdsTop10).ToListAsync();

                // Get positions of employees top 10.
                var positionEmployeesTop10 = await (from epm in DB.EmployeePositionMappings
                                                    join p in DB.Positions on epm.PositionId equals p.PositionId
                                                    where employeeIdsTop10.Contains(epm.EmployeeId)
                                                    select new
                                                    {
                                                        epm.EmployeeId,
                                                        p.PositionName
                                                    }).ToListAsync();

                // Looping top 10 employee ids to insert top 10 employees to show data model.
                foreach (var id in employeeIdsTop10)
                {
                    var historyPointEmployees2 = this.DB
                .EmployeePointHistories
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType &&
                       Q.PointTransactionTypeId == (int)PointTransactionTypeEnum.Income &&
                       Q.CreatedAt.Year == DateTime.Now.Year && Q.CreatedAt.DayOfYear == DateTime.Now.DayOfYear)
                .AsQueryable();

                    var totalExperience = historyPointEmployees2
                    .Where(Q => Q.EmployeeId == id && Q.CreatedAt.Year == DateTime.Now.Year && Q.CreatedAt.DayOfYear == DateTime.Now.DayOfYear)
                    .Sum(Q => Q.Point);

                    // Get Spesific employee for get data detail.
                    var employeeDetail = nextEmployeeDetailsTop10
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    // Get position spesific employee and get first data.
                    var position = positionEmployeesTop10
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    var data = new UserSideRankModel
                    {
                        EmployeeId = employeeDetail.EmployeeId,
                        EmployeeName = employeeDetail.EmployeeName,
                        PositionName = position == null ? null : position.PositionName,
                        ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                        Name = employeeDetail.OutletName,
                        EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                        Level = GenerateLevel(levels, employeeDetail.EmployeeExperience),
                        TotalExperience = totalExperience,
                        AreaName = employeeDetail.AreaName,
                        DealerName = employeeDetail.DealerName,
                        Rank = currentRank,
                        IsLoggedInUser = false
                    };

                    // Update current Rank.
                    currentRank += 1;

                    employeesShowData.Add(data);
                }

                return employeesShowData;
            }

            // Query point history employees.
            var historyPointEmployees = this.DB
                .EmployeePointHistories
                .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType &&
                       Q.PointTransactionTypeId == (int)PointTransactionTypeEnum.Income &&
                       Q.CreatedAt.Year == DateTime.Now.Year && Q.CreatedAt.DayOfYear == DateTime.Now.DayOfYear )
                .AsQueryable();

            // Get detail employee top 3 rank of employee id.
            var top3EmployeeIds = await employeeIdsQueryable
                .Take(3)
                .ToListAsync();

            // Check this employee is top 3 or not.
            var isTop3 = top3EmployeeIds
                .Where(Q => Q.ToLower() == employeeId.ToLower())
                .Any();

            // TOP 3 Employee Process.
            // Get Rank of employee detail.
            var employeeDetails = await GetRankQueryable(top3EmployeeIds).ToListAsync();

            // Is logged in user. this value will change to True when Employee in params exist in employee rank.
            var isLoggedInUSer = false;

            // Get positions of employees.
            var positionEmployees = await (from epm in DB.EmployeePositionMappings
                                           join p in DB.Positions on epm.PositionId equals p.PositionId
                                           where top3EmployeeIds.Contains(epm.EmployeeId)
                                           select new
                                           {
                                               epm.EmployeeId,
                                               p.PositionName
                                           }).ToListAsync();

            // Looping top 3 employee ids to insert top 3 employees to show data model.
            foreach (var id in top3EmployeeIds)
            {
                // Get Total Experience for spesific employee.
                var totalExperience = historyPointEmployees
                    .Where(Q => Q.EmployeeId == id)
                    .Sum(Q => Q.Point);

                // Get Spesific employee for get data detail.
                var employeeDetail = employeeDetails
                    .Where(Q => Q.EmployeeId == id)
                    .FirstOrDefault();

                // Get position spesific employee and get first data.
                var position = positionEmployees
                    .Where(Q => Q.EmployeeId == id)
                    .FirstOrDefault();

                // Get logged in user.
                if (id == employeeId)
                {
                    isLoggedInUSer = true;
                }

                var data = new UserSideRankModel
                {
                    EmployeeId = employeeDetail.EmployeeId,
                    EmployeeName = employeeDetail.EmployeeName,
                    PositionName = position.PositionName,
                    ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                    Name = employeeDetail.OutletName,
                    EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                    Level = GenerateLevel(levels, totalExperience),
                    TotalExperience = totalExperience,
                    AreaName = employeeDetail.AreaName,
                    DealerName = employeeDetail.DealerName,
                    Rank = currentRank,
                    IsLoggedInUser = isLoggedInUSer
                };

                // Update current Rank.
                currentRank += 1;

                // Reset logged in user.
                isLoggedInUSer = false;

                employeesShowData.Add(data);
            }

            if (isTop3)
            {
                // Next Employee Rank Process.
                // Because employee in params are at the top 3 rank. So skip 3 data from employee ids that ordered by employee experience, 
                // And take 7 data employee id to get next rank
                var employeeIdsAfterTop3 = await employeeIdsQueryable
                    .Skip(3)
                    .Take(7)
                    .ToListAsync();

                // Get employee detail of next rank after top 3 employee.
                var nextEmployeeDetails = await GetRankQueryable(employeeIdsAfterTop3).ToListAsync();

                // Get positions of employees after top 3.
                var positionEmployeesAfterTop3 = await (from epm in DB.EmployeePositionMappings
                                                        join p in DB.Positions on epm.PositionId equals p.PositionId
                                                        where employeeIdsAfterTop3.Contains(epm.EmployeeId)
                                                        select new
                                                        {
                                                            epm.EmployeeId,
                                                            p.PositionName
                                                        }).ToListAsync();

                // Looping top 3 employee ids to insert after top 3 employees to show data model.
                foreach (var id in employeeIdsAfterTop3)
                {
                    // Total Experience in this year for spesific employee.
                    var totalExperience = historyPointEmployees
                        .Where(Q => Q.EmployeeId == id)
                        .Sum(Q => Q.Point);

                    // Get Spesific employee for get data detail.
                    var employeeDetail = nextEmployeeDetails
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    // Get position spesific employee and get first data.
                    var position = positionEmployeesAfterTop3
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    var data = new UserSideRankModel
                    {
                        EmployeeId = employeeDetail.EmployeeId,
                        EmployeeName = employeeDetail.EmployeeName,
                        PositionName = position == null ? null : position.PositionName,
                        ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                        Name = employeeDetail.OutletName,
                        EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                        Level = GenerateLevel(levels, totalExperience),
                        TotalExperience = totalExperience,
                        AreaName = employeeDetail.AreaName,
                        DealerName = employeeDetail.DealerName,
                        Rank = currentRank,
                        IsLoggedInUser = isLoggedInUSer
                    };

                    // Update current Rank.
                    currentRank += 1;

                    // Reset logged in user.
                    isLoggedInUSer = false;

                    employeesShowData.Add(data);
                }

                return employeesShowData;
            }

            // Next Employee Rank Process.
            // If employee not at Top 3.
            // Get EmployeeExperience of this employee. 
            var totalExperienceSpesificEmployee = historyPointEmployees
                .Where(Q => Q.EmployeeId == employeeId)
                .Sum(Q => Q.Point);

            // Query to select only EmployeeIds to get top 3 employee id, and next rank.
            // This query group by employee id to count point in this year to get Employee Experience, 
            // And get total employee that have employee experience greater than employee experience this employee.
            // This total employee determines where this employee's index/order/rank
            var totalEmployee = await tempemployeeQueryable
                .GroupBy(Q => Q.EmployeeId)
                .Select(Q => new
                {
                    EmployeeId = Q.Key,
                    EmployeeExperience = Q.Sum(X => X.Point)
                })
                .OrderByDescending(Q => Q.EmployeeExperience)
                .Where(Q => Q.EmployeeExperience > totalExperienceSpesificEmployee)
                .CountAsync();

            // Change current rank to rank this employee
            // Because, after top 3, next rank starts from this rank employee.
            currentRank = totalEmployee + 1;

            // Get employee ids after Top 3
            // Because employee in params are not at the top 3 rank. So skip data to this employee rank,
            // And take 7 data employee id to get next rank
            var employeeIdsNotTop3 = await employeeIdsQueryable
                .Skip(currentRank - 1)
                .Take(7)
                .ToListAsync();

            // Get Rank of employee detail after top 3.
            var nextEmployeeDetailsNotTop3 = await GetRankQueryable(employeeIdsNotTop3).ToListAsync();

            // Get positions of employees after top 3.
            var positionEmployeesNotTop3 = await (from epm in DB.EmployeePositionMappings
                                                  join p in DB.Positions on epm.PositionId equals p.PositionId
                                                  where employeeIdsNotTop3.Contains(epm.EmployeeId)
                                                  select new
                                                  {
                                                      epm.EmployeeId,
                                                      p.PositionName
                                                  }).ToListAsync();

            // Looping top 3 employee ids to insert after top 3 employees to show data model.
            foreach (var id in employeeIdsNotTop3)
            {
                // Get logged in user.
                if (id == employeeId)
                {
                    isLoggedInUSer = true;
                }

                // Total Experience in this year for spesific employee.
                var totalExperience = historyPointEmployees
                    .Where(Q => Q.EmployeeId == id)
                    .Sum(Q => Q.Point);

                // Get Spesific employee for get data detail.
                var employeeDetail = nextEmployeeDetailsNotTop3
                    .Where(Q => Q.EmployeeId == id)
                    .FirstOrDefault();

                // Get position spesific employee and get first data.
                var position = positionEmployeesNotTop3
                    .Where(Q => Q.EmployeeId == id)
                    .FirstOrDefault();

                var data = new UserSideRankModel
                {
                    EmployeeId = employeeDetail.EmployeeId,
                    EmployeeName = employeeDetail.EmployeeName,
                    PositionName = position == null ? null : position.PositionName,
                    ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                    Name = employeeDetail.OutletName,
                    EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                    Level = GenerateLevel(levels, totalExperience),
                    TotalExperience = totalExperience,
                    AreaName = employeeDetail.AreaName,
                    DealerName = employeeDetail.DealerName,
                    Rank = currentRank,
                    IsLoggedInUser = isLoggedInUSer
                };

                // Update current Rank.
                currentRank += 1;

                // Reset logged in user.
                isLoggedInUSer = false;

                employeesShowData.Add(data);
            }

            return employeesShowData;
        }

        /// <summary>
        /// Get rank for all user when user choose All Time.
        /// This method works for generate rank all user based on total experience employees.
        /// Total Experience used is obtained from Employees Table in TotalExperience field.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<UserSideRankModel>> GetRankAllUserAllTime(UserSideRankFilterModel filter, string employeeId)
        {
            //var currentEmployee = await this.DB.GetGenerateRank(employeeId,filter.AreaIds, filter.DealerIds, filter.PositionIds,filter.SortBy).ToListAsync();


            // Get master EmployeeLevels for generate level of employees.
            var levels = await this.DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            //  Query employee that order by employee experience to get rank of employees.
            var employeeQueryable = from e in DB.Employees.AsNoTracking()

                                    join epm in DB.EmployeePositionMappings.AsNoTracking()
                                    on e.EmployeeId equals epm.EmployeeId into aepm
                                    from epm in aepm.DefaultIfEmpty()

                                    join o in DB.Outlets.AsNoTracking()
                                    on e.OutletId equals o.OutletId into ao
                                    from o in ao.DefaultIfEmpty()

                                    join d in DB.Dealers.AsNoTracking()
                                    on o.DealerId equals d.DealerId into ad
                                    from d in ad.DefaultIfEmpty()

                                    join a in DB.Areas.AsNoTracking()
                                    on o.AreaId equals a.AreaId into aa
                                    from a in aa.DefaultIfEmpty()

                                    where e.IsDeleted == false

                                    orderby e.EmployeeExperience descending
                                    select new
                                    {
                                        e.EmployeeId,
                                        e.EmployeeExperience,
                                        o.OutletId,
                                        d.DealerId,
                                        a.AreaId,
                                        epm.PositionId
                                    };
            // Filter by area ids.
            if (filter.AreaIds != null)
            {
                filter.AreaIds = filter.AreaIds.Where(Q => Q != null).ToList();
                if (filter.AreaIds.Count > 0)
                {
                    employeeQueryable = employeeQueryable.Where(Q => filter.AreaIds.Contains(Q.AreaId));
                }
            }

            // Filter by dealer ids.
            if (filter.DealerIds != null)
            {
                filter.DealerIds = filter.DealerIds.Where(Q => Q != null).ToList();
                if (filter.DealerIds.Count > 0)
                {
                    employeeQueryable = employeeQueryable.Where(Q => filter.DealerIds.Contains(Q.DealerId));
                }
            }


            // Filter by position.
            if (filter.PositionIds != null)
            {
                filter.PositionIds = filter.PositionIds.Where(Q => Q != null).ToList();
                if (filter.PositionIds.Count > 0)
                {
                    employeeQueryable = employeeQueryable.Where(Q => filter.PositionIds.Contains(Q.PositionId));
                }
            }

            // Query to select only EmployeeIds to get top 3 employee id, and next rank.
            var employeeIdsQueryable = employeeQueryable
                .GroupBy(Q => new
                {
                    Q.EmployeeId,
                    Q.EmployeeExperience
                })
                .OrderByDescending(Q => Q.Key.EmployeeExperience)
                .Select(Q => Q.Key.EmployeeId)
                .AsQueryable();

            // Set current rank = 1, this value will increase when looping employees that ordered by EmployeeExperience.
            var currentRank = 1;

            var employeesShowData = new List<UserSideRankModel>();

            // If Employee logged in filtered out
            if (employeeQueryable.Where(Q => Q.EmployeeId == employeeId).Count() == 0)
            {
                var employeeIdsTop10 = await employeeIdsQueryable
                    .Take(10)
                    .ToListAsync();

                // Get Rank of employee detail top 10.
                var nextEmployeeDetailsTop10 = await GetRankQueryable(employeeIdsTop10).ToListAsync();

                // Get positions of employees top 10.
                var positionEmployeesTop10 = await (from epm in DB.EmployeePositionMappings
                                                    join p in DB.Positions on epm.PositionId equals p.PositionId
                                                    where employeeIdsTop10.Contains(epm.EmployeeId)
                                                    select new
                                                    {
                                                        epm.EmployeeId,
                                                        p.PositionName
                                                    }).ToListAsync();

                // Looping top 10 employee ids to insert top 10 employees to show data model.
                foreach (var id in employeeIdsTop10)
                {

                    // Get Spesific employee for get data detail.
                    var employeeDetail = nextEmployeeDetailsTop10
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    // Get position spesific employee and get first data.
                    var position = positionEmployeesTop10
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    var data = new UserSideRankModel
                    {
                        EmployeeId = employeeDetail.EmployeeId,
                        EmployeeName = employeeDetail.EmployeeName,
                        PositionName = position == null ? null : position.PositionName,
                        ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                        Name = employeeDetail.OutletName,
                        EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                        Level = GenerateLevel(levels, employeeDetail.EmployeeExperience),
                        TotalExperience = employeeDetail.EmployeeExperience,
                        AreaName = employeeDetail.AreaName,
                        DealerName = employeeDetail.DealerName,
                        Rank = currentRank,
                        IsLoggedInUser = false
                    };

                    // Update current Rank.
                    currentRank += 1;

                    employeesShowData.Add(data);
                }

                return employeesShowData;
            }

            // Get top 3 rank of employee ids.
            var top3EmployeeIds = await employeeIdsQueryable
                .Take(3)
                .ToListAsync();

            // Check this employee is top 3 or not.
            var isTop3 = top3EmployeeIds
                .Where(Q => Q.ToLower() == employeeId.ToLower())
                .Any();

            // TOP 3 Employee Process.
            // Get employee detail of top 3 employee.
            var employeeDetails = await GetRankQueryable(top3EmployeeIds).ToListAsync();

            // Is logged in user. this value will change to True when Employee in params exist in employee rank.
            var isLoggedInUSer = false;

            // Get positions of employees.
            var positionEmployees = await (from epm in DB.EmployeePositionMappings
                                           join p in DB.Positions on epm.PositionId equals p.PositionId
                                           where top3EmployeeIds.Contains(epm.EmployeeId)
                                           select new
                                           {
                                               epm.EmployeeId,
                                               p.PositionName
                                           }).ToListAsync();

            // Looping top 3 employee ids to insert top 3 employees to show data model.
            foreach (var id in top3EmployeeIds)
            {
                // Get Spesific employee for get data detail.
                var employeeDetail = employeeDetails
                    .Where(Q => Q.EmployeeId == id)
                    .FirstOrDefault();

                // Get position spesific employee and get first data.
                var position = positionEmployees
                    .Where(Q => Q.EmployeeId == id)
                    .FirstOrDefault();

                // Get logged in user.
                if (id == employeeId)
                {
                    isLoggedInUSer = true;
                }

                var data = new UserSideRankModel
                {
                    EmployeeId = employeeDetail.EmployeeId,
                    EmployeeName = employeeDetail.EmployeeName,
                    PositionName = position.PositionName,
                    ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                    Name = employeeDetail.OutletName,
                    EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                    Level = GenerateLevel(levels, employeeDetail.EmployeeExperience),
                    TotalExperience = employeeDetail.EmployeeExperience,
                    AreaName = employeeDetail.AreaName,
                    DealerName = employeeDetail.DealerName,
                    Rank = currentRank,
                    IsLoggedInUser = isLoggedInUSer
                };

                // Update current Rank.
                currentRank += 1;

                // Reset logged in user.
                isLoggedInUSer = false;

                employeesShowData.Add(data);
            }

            if (isTop3)
            {
                // Next Employee Rank Process.
                // Because employee in params are at the top 3 rank. So skip 3 data from employee ids that ordered by employee experience, 
                // And take 7 data employee id to get next rank
                var employeeIdsAfterTop3 = await employeeIdsQueryable
                    .Skip(3)
                    .Take(7)
                    .ToListAsync();

                // Get employee detail of next rank after top 3 employee.
                var nextEmployeeDetails = await GetRankQueryable(employeeIdsAfterTop3).ToListAsync();

                // Get positions of employees after top 3.
                var positionEmployeesAfterTop3 = await (from epm in DB.EmployeePositionMappings
                                                        join p in DB.Positions on epm.PositionId equals p.PositionId
                                                        where employeeIdsAfterTop3.Contains(epm.EmployeeId)
                                                        select new
                                                        {
                                                            epm.EmployeeId,
                                                            p.PositionName
                                                        }).ToListAsync();

                // Looping top 3 employee ids to insert after top 3 employees to show data model.
                foreach (var id in employeeIdsAfterTop3)
                {
                    // Get Spesific employee for get data detail.
                    var employeeDetail = nextEmployeeDetails
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    // Get position spesific employee and get first data.
                    var position = positionEmployeesAfterTop3
                        .Where(Q => Q.EmployeeId == id)
                        .FirstOrDefault();

                    var data = new UserSideRankModel
                    {
                        EmployeeId = employeeDetail.EmployeeId,
                        EmployeeName = employeeDetail.EmployeeName,
                        PositionName = position == null ? null : position.PositionName,
                        ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                        Name = employeeDetail.OutletName,
                        EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                        Level = GenerateLevel(levels, employeeDetail.EmployeeExperience),
                        TotalExperience = employeeDetail.EmployeeExperience,
                        AreaName = employeeDetail.AreaName,
                        DealerName = employeeDetail.DealerName,
                        Rank = currentRank,
                        IsLoggedInUser = isLoggedInUSer
                    };

                    // Update current Rank.
                    currentRank += 1;

                    // Reset logged in user.
                    isLoggedInUSer = false;

                    employeesShowData.Add(data);
                }

                return employeesShowData;
            }

            // If employee not at Top 3.
            // Get EmployeeExperience of this employee. 
            var totalExperienceSpesificEmployee = await employeeQueryable
                .Where(Q => Q.EmployeeId == employeeId)
                .Select(Q => Q.EmployeeExperience)
                .FirstOrDefaultAsync();

            // Get total employee that have employee experience greater than employee experience this employee.
            // This total employee determines where this employee's index/order/rank.
            var totalEmployee = await employeeQueryable
                .Where(Q => Q.EmployeeExperience > totalExperienceSpesificEmployee).Select(Q => Q.EmployeeId).Distinct()
                .CountAsync();

            // Change current rank to rank this employee.
            // Because, after top 3, next rank starts from this rank employee.
            currentRank = totalEmployee + 1;

            // Get employee ids after Top 3
            // Because employee in params are not at the top 3 rank. So skip data to this employee rank,
            // And take 7 data employee id to get next rank
            var employeeIdsNotTop3 = await employeeIdsQueryable
                .Skip(currentRank - 1)
                .Take(7)
                .ToListAsync();

            if(employeeIdsNotTop3.Exists(Q => Q.ToLower() == employeeId.ToLower()) == false)
            {
                var tempResult = employeeIdsNotTop3.Take(6).ToList();
                employeeIdsNotTop3.Clear();
                employeeIdsNotTop3.Add(employeeId);
                employeeIdsNotTop3.AddRange(tempResult);
            }

            // Get Rank of employee detail after top 3.
            var nextEmployeeDetailsNotTop3 = await GetRankQueryable(employeeIdsNotTop3).ToListAsync();

            // Get positions of employees after top 3.
            var positionEmployeesNotTop3 = await (from epm in DB.EmployeePositionMappings
                                                  join p in DB.Positions on epm.PositionId equals p.PositionId
                                                  where employeeIdsNotTop3.Contains(epm.EmployeeId)
                                                  select new
                                                  {
                                                      epm.EmployeeId,
                                                      p.PositionName
                                                  }).ToListAsync();

            // Looping top 3 employee ids to insert after top 3 employees to show data model.
            foreach (var id in employeeIdsNotTop3)
            {
                // Get logged in user.
                if (id.ToLower() == employeeId.ToLower())
                {
                    isLoggedInUSer = true;
                }

                // Get Spesific employee for get data detail.
                var employeeDetail = nextEmployeeDetailsNotTop3
                    .Where(Q => Q.EmployeeId.ToLower() == id.ToLower())
                    .FirstOrDefault();

                // Get position spesific employee and get first data.
                var position = positionEmployeesNotTop3
                    .Where(Q => Q.EmployeeId.ToLower() == id.ToLower())
                    .FirstOrDefault();
                
                var data = new UserSideRankModel
                {
                    EmployeeId = employeeDetail.EmployeeId,
                    EmployeeName = employeeDetail.EmployeeName,
                    PositionName = position == null ? null : position.PositionName,
                    ImageUrl = employeeDetail.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(employeeDetail.BlobId.ToString(), employeeDetail.Mime),
                    Name = employeeDetail.OutletName,
                    EmployeePoint = employeeDetail.LearningPoint + employeeDetail.TeachingPoint,
                    Level = GenerateLevel(levels, employeeDetail.EmployeeExperience),
                    TotalExperience = employeeDetail.EmployeeExperience,
                    AreaName = employeeDetail.AreaName,
                    DealerName = employeeDetail.DealerName,
                    Rank = currentRank,
                    IsLoggedInUser = isLoggedInUSer
                };

                //if (id.ToLower() == employeeId.ToLower())
                //{
                //    data.Rank = (int)currentEmployee[0].Rank;
                //}

                // Update current Rank.
                currentRank += 1;

                // Reset logged in user.
                isLoggedInUSer = false;

                employeesShowData.Add(data);
            }

            return employeesShowData;
        }

        /// <summary>
        /// Query to get Employee with blob, outlet, dealer, and area.
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        public IQueryable<UserSideQueryRankModel> GetRankQueryable(List<string> employeeIds)
        {
            var queryRankUser = from e in DB.Employees.AsNoTracking()

                                join b in DB.Blobs.AsNoTracking()
                                on e.BlobId equals b.BlobId into ab
                                from b in ab.DefaultIfEmpty()

                                join o in DB.Outlets.AsNoTracking()
                                on e.OutletId equals o.OutletId into ao
                                from o in ao.DefaultIfEmpty()

                                join d in DB.Dealers.AsNoTracking()
                                on o.DealerId equals d.DealerId into ad
                                from d in ad.DefaultIfEmpty()

                                join a in DB.Areas.AsNoTracking()
                                on o.AreaId equals a.AreaId into aa
                                from a in aa.DefaultIfEmpty()

                                where employeeIds.Contains(e.EmployeeId) && e.IsDeleted == false
                                select new UserSideQueryRankModel
                                {
                                    EmployeeId = e.EmployeeId,
                                    EmployeeName = e.EmployeeName,
                                    BlobId = e.BlobId == null ? Guid.Empty : e.BlobId,
                                    LearningPoint = e.LearningPoint,
                                    TeachingPoint = e.TeachingPoint,
                                    EmployeeExperience = e.EmployeeExperience,
                                    Mime = string.IsNullOrEmpty(b.Mime) ? "" : b.Mime,
                                    OutletName = string.IsNullOrEmpty(o.Name) ? "" : o.Name,
                                    OutletId = string.IsNullOrEmpty(o.OutletId) ? "" : o.OutletId,
                                    DealerName = string.IsNullOrEmpty(d.DealerName) ? "" : d.DealerName,
                                    DealerId = string.IsNullOrEmpty(d.DealerId) ? "" : d.DealerId,
                                    AreaName = string.IsNullOrEmpty(a.Name) ? "" : a.Name,
                                    AreaId = string.IsNullOrEmpty(a.AreaId) ? "" : a.AreaId
                                };

            return queryRankUser;
        }

        public async Task<List<UserSideRankModel>> GetRankAllUser(UserSideRankFilterModel filter, string employeeId)
        {
            //var testquery = this.DB.GetGenerateRank(employeeId).FirstOrDefaultAsync();

            var queryCoach = await this
                .DB
                .Coaches
                .AsNoTracking()
                .Select(Q => Q.EmployeeId)
                .ToListAsync();

            var queryRankUser = from e in DB.Employees.AsNoTracking()

                                join b in DB.Blobs.AsNoTracking()
                                on e.BlobId equals b.BlobId into ab
                                from b in ab.DefaultIfEmpty()

                                join epm in DB.EmployeePositionMappings.AsNoTracking()
                                on e.EmployeeId equals epm.EmployeeId into aepm
                                from epm in aepm.DefaultIfEmpty()

                                join p in DB.Positions.AsNoTracking()
                                on epm.PositionId equals p.PositionId into ap
                                from p in ap.DefaultIfEmpty()

                                join o in DB.Outlets.AsNoTracking()
                                on e.OutletId equals o.OutletId into ao
                                from o in ao.DefaultIfEmpty()

                                join d in DB.Dealers.AsNoTracking()
                                on o.DealerId equals d.DealerId into ad
                                from d in ad.DefaultIfEmpty()

                                join a in DB.Areas.AsNoTracking()
                                on o.AreaId equals a.AreaId into aa
                                from a in aa.DefaultIfEmpty()

                                where e.IsDeleted == false

                                orderby e.EmployeeExperience descending

                                select new
                                {
                                    e.EmployeeId,
                                    e.EmployeeName,
                                    BlobId = e.BlobId == null ? Guid.Empty : e.BlobId,
                                    e.LearningPoint,
                                    e.TeachingPoint,
                                    e.EmployeeExperience,
                                    Mime = string.IsNullOrEmpty(b.Mime) ? "" : b.Mime,
                                    PositionName = string.IsNullOrEmpty(p.PositionName) ? "" : p.PositionName,
                                    PositionId = (int?)p.PositionId,
                                    OutletName = string.IsNullOrEmpty(o.Name) ? "" : o.Name,
                                    OutletId = string.IsNullOrEmpty(o.OutletId) ? "" : o.OutletId,
                                    DealerName = string.IsNullOrEmpty(d.DealerName) ? "" : d.DealerName,
                                    DealerId = string.IsNullOrEmpty(d.DealerId) ? "" : d.DealerId,
                                    AreaName = string.IsNullOrEmpty(a.Name) ? "" : a.Name,
                                    AreaId = string.IsNullOrEmpty(a.AreaId) ? "" : a.AreaId
                                };

            // Filter by area.
            if (filter.AreaIds != null)
            {
                filter.AreaIds = filter.AreaIds.Where(Q => Q != null).ToList();
                if (filter.AreaIds.Count > 0)
                {
                    queryRankUser = queryRankUser.Where(Q => filter.AreaIds.Contains(Q.AreaId));
                }
            }

            // Filter by dealer.
            if (filter.DealerIds != null)
            {
                filter.DealerIds = filter.DealerIds.Where(Q => Q != null).ToList();
                if (filter.DealerIds.Count > 0)
                {
                    queryRankUser = queryRankUser.Where(Q => filter.DealerIds.Contains(Q.DealerId));
                }
            }


            // Filter by position.
            if (filter.PositionIds != null)
            {
                filter.PositionIds = filter.PositionIds.Where(Q => Q != null).ToList();
                if (filter.PositionIds.Count > 0)
                {
                    queryRankUser = queryRankUser.Where(Q => filter.PositionIds.Contains(Q.PositionId));
                }
            }


            var rankUsers = await queryRankUser
                .AsNoTracking()
                .ToListAsync();

            // Get all Levels.
            var levels = await this.DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            // If get rank NOW.
            if (filter.IsAllTime == false)
            {
                var dataAllUser = new List<UserSideRankModel>();

                // Get all learning point users.
                var learningPointUsers = await this.DB
                    .EmployeePointHistories
                    .Where(Q => Q.RewardPointTypeId == RewardPointTypeEnum.LearningPointType &&
                           Q.PointTransactionTypeId == (int)PointTransactionTypeEnum.Income &&
                           Q.CreatedAt.Year == DateTime.Now.Year)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var user in rankUsers)
                {
                    // Check user is exist in list or not.
                    var isUserExist = dataAllUser
                        .Where(Q => Q.EmployeeId.Contains(user.EmployeeId))
                        .Any();

                    if (isUserExist == true)
                    {
                        continue;
                    }

                    // Get experience for spesific employee.
                    var employeeExperience = learningPointUsers
                        .Where(Q => Q.EmployeeId.ToLower() == user.EmployeeId.ToLower())
                        .Sum(Q => Q.Point);

                    // Get Image Url for spesific employee
                    string imageUrl = null;
                    if (user.BlobId != Guid.Empty)
                    {
                        imageUrl = await this.FileService.GetFileAsync(user.BlobId.ToString(), user.Mime);
                    }

                    var dataUser = new UserSideRankModel
                    {
                        EmployeeId = user.EmployeeId,
                        EmployeeName = user.EmployeeName,
                        PositionName = user.PositionName,
                        ImageUrl = imageUrl,
                        Name = user.OutletName,
                        EmployeePoint = user.LearningPoint + user.TeachingPoint,
                        Level = GenerateLevel(levels, employeeExperience),
                        TotalExperience = employeeExperience,
                        AreaName = user.AreaName,
                        DealerName = user.DealerName
                    };

                    dataAllUser.Add(dataUser);
                }

                dataAllUser = dataAllUser
                    .OrderByDescending(Q => Q.TotalExperience)
                    .ToList();

                var rankUsersViewNow = GenerateRank(dataAllUser, employeeId);

                // Sorting
                rankUsersViewNow = RankSort(filter.SortBy, rankUsersViewNow);

                return rankUsersViewNow.Take(10).ToList();
            }

            var dataRankUser = rankUsers
                .GroupBy(Q => Q.EmployeeId)
                .Select(async X => new UserSideRankModel
                {
                    EmployeeId = X.First().EmployeeId,
                    EmployeeName = X.First().EmployeeName,
                    PositionName = X.First().PositionName,
                    ImageUrl = X.First().BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.First().BlobId.ToString(), X.First().Mime),
                    Name = X.First().OutletName,
                    EmployeePoint = X.First().LearningPoint + X.First().TeachingPoint,
                    Level = GenerateLevel(levels, X.First().EmployeeExperience),
                    TotalExperience = X.First().EmployeeExperience,
                    AreaName = X.First().AreaName,
                    DealerName = X.First().DealerName
                })
                .Select(Q => Q.Result)
                .OrderByDescending(Q => Q.TotalExperience)
                .ToList();

            var rankUsersView = GenerateRank(dataRankUser, employeeId);

            // Sorting
            rankUsersView = RankSort(filter.SortBy, rankUsersView);

            return rankUsersView.Take(10).ToList();
        }

        /// <summary>
        /// Sort Rank data employee.
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<UserSideRankModel> RankSort(string sortBy, List<UserSideRankModel> data)
        {
            switch (sortBy)
            {
                case "PositionAsc":
                    data = data.OrderBy(Q => Q.PositionName).ToList();
                    break;
                case "PositionDesc":
                    data = data.OrderByDescending(Q => Q.PositionName).ToList();
                    break;
                case "AreaAsc":
                    data = data.OrderBy(Q => Q.AreaName).ToList();
                    break;
                case "AreaDesc":
                    data = data.OrderByDescending(Q => Q.AreaName).ToList();
                    break;
                case "DealerAsc":
                    data = data.OrderBy(Q => Q.DealerName).ToList();
                    break;
                case "DealerDesc":
                    data = data.OrderByDescending(Q => Q.DealerName).ToList();
                    break;
            }

            return data;
        }


        public async Task<List<UserSideRankModel>> GetRankUser(string employeeId)
        {
            var queryCoach = await this
                .DB
                .Coaches
                .AsNoTracking()
                .Select(Q => Q.EmployeeId)
                .ToListAsync();

            var queryRankUser = await (from e in DB.Employees.AsNoTracking()

                                       join b in DB.Blobs.AsNoTracking()
                                       on e.BlobId equals b.BlobId into ab
                                       from b in ab.DefaultIfEmpty()

                                       join epm in DB.EmployeePositionMappings.AsNoTracking()
                                       on e.EmployeeId equals epm.EmployeeId into aepm
                                       from epm in aepm.DefaultIfEmpty()

                                       join p in DB.Positions.AsNoTracking()
                                       on epm.PositionId equals p.PositionId into ap
                                       from p in ap.DefaultIfEmpty()

                                       join o in DB.Outlets.AsNoTracking()
                                       on e.OutletId equals o.OutletId into ao
                                       from o in ao.DefaultIfEmpty()

                                       orderby e.EmployeeExperience descending

                                       where !queryCoach.Contains(e.EmployeeId) && e.IsDeleted == false

                                       select new
                                       {
                                           e.EmployeeId,
                                           e.EmployeeName,
                                           BlobId = e.BlobId == null ? Guid.Empty : e.BlobId,
                                           e.LearningPoint,
                                           e.TeachingPoint,
                                           e.EmployeeExperience,
                                           Mime = string.IsNullOrEmpty(b.Mime) ? "" : b.Mime,
                                           PositionName = string.IsNullOrEmpty(p.PositionName) ? "" : p.PositionName,
                                           OutletName = string.IsNullOrEmpty(o.Name) ? "" : o.Name
                                       })
                                        .ToListAsync();

            var queryLevel = await
                DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            var dataRankUser = queryRankUser
                .GroupBy(Q => Q.EmployeeId)
                .Select(async X => new UserSideRankModel
                {
                    EmployeeId = X.First().EmployeeId,
                    EmployeeName = X.First().EmployeeName,
                    PositionName = X.First().PositionName,
                    ImageUrl = X.First().BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.First().BlobId.ToString(), X.First().Mime),
                    Name = X.First().OutletName,
                    EmployeePoint = X.First().LearningPoint + X.First().TeachingPoint,
                    Level = GenerateLevel(queryLevel, X.First().EmployeeExperience),
                    TotalExperience = X.First().EmployeeExperience
                })
                .Select(Q => Q.Result)
                .OrderByDescending(Q => Q.TotalExperience)
                .ToList();

            return GenerateRank(dataRankUser, employeeId);
        }

        public int GenerateLevel(List<int> minLvl, int exp)
        {
            var resLvl = 0;
            foreach (var minlevel in minLvl)
            {
                if (exp > minlevel)
                {
                    resLvl++;
                }
                else
                {
                    break;
                }
            }

            return resLvl;
        }

//        public UserSideRankModel GenerateSingleRank(int employeeId)
//        {
//            with rankingEmployee as (
//SELECT ROW_NUMBER() over(order by EmployeeExperience) as [Rank] , *
//FROM[dbo].[Employees]
//  ) select TOP 10 * from rankingEmployee where Rank >= 9858 AND EmployeeId = 'OCAUT001152EC5666'
//        }

        /// <summary>
        /// Generate rank of employee and customize list of rank
        /// </summary>
        /// <param name="dataRank"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<UserSideRankModel> GenerateRank(List<UserSideRankModel> dataRank, string employeeId)
        {
            var rankUserLoggedIn = 0;

            var rank = 1;
            foreach (var value in dataRank)
            {
                value.Rank = rank;
                rank += 1;

                if (value.EmployeeId.ToLower() == employeeId.ToLower())
                {
                    value.IsLoggedInUser = true;
                    rankUserLoggedIn = value.Rank;
                }
                else
                {
                    value.IsLoggedInUser = false;
                }
            }

            // List Rank of user to show (Rank 1, 2, 3. and skip until spesific employee id)
            var rankUserToShow = new List<UserSideRankModel>();

            // Get Rank 1 2 3
            rankUserToShow.AddRange(dataRank.Take(3));

            if (rankUserLoggedIn > 3)
            {
                // Get rank after spesific emloyeeId
                rankUserToShow
                    .AddRange(dataRank.SkipWhile(Q => Q.EmployeeId.ToLower() != employeeId.ToLower()).ToList());
            }
            else
            {
                rankUserToShow.AddRange(dataRank.Skip(3).ToList());
            }

            return rankUserToShow;
        }
    }
}
