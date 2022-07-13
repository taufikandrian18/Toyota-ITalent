using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTimeTableService
    {
        private readonly TalentContext DB;

        public UserSideTimeTableService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<UserSideTimeTableResponseModel> GetTimeTable(UserSideTimeTableFilterModel filter)
        {
            var query = (from t in DB.Trainings
                         join tpm in DB.TrainingPositionMappings on t.TrainingId equals tpm.TrainingId
                         join tom in DB.TrainingOutletMappings on t.TrainingId equals tom.TrainingId
                         join o in DB.Outlets on tom.OutletId equals o.OutletId
                         join a in DB.Areas on o.AreaId equals a.AreaId
                         join d in DB.Dealers on o.DealerId equals d.DealerId
                         where t.ApprovedAt.HasValue
                         select new
                         {
                             t.TrainingId,
                             tpm.PositionId,
                             o.OutletId,
                             a.AreaId,
                             d.DealerId
                         }).AsQueryable();

            if (filter.PositionId.Count > 0)
            {
                query = query.Where(Q => filter.PositionId.Contains(Q.PositionId)).AsQueryable();
            }
            if (filter.DealerId.Count > 0)
            {
                query = query.Where(Q => filter.DealerId.Contains(Q.DealerId)).AsQueryable();
            }
            if (filter.AreaId.Count > 0)
            {
                query = query.Where(Q => filter.AreaId.Contains(Q.AreaId)).AsQueryable();
            }

            var trainingIds = await query.Select(Q => Q.TrainingId).Distinct().ToListAsync();

            var initQuery = (from tr in this.DB.Trainings.AsNoTracking()

                             join c in this.DB.Courses.AsNoTracking()
                             on tr.CourseId equals c.CourseId

                             join pt in this.DB.ProgramTypes.AsNoTracking()
                             on c.ProgramTypeId equals pt.ProgramTypeId

                             where tr.StartDate.HasValue && tr.EndDate.HasValue && trainingIds.Contains(tr.TrainingId) &&
                             tr.ApprovedAt.HasValue

                             select new UserSideTimeTableModel
                             {
                                 TrainingId = tr.TrainingId,
                                 Batch = tr.Batch,
                                 StartDate = tr.StartDate,
                                 EndDate = tr.EndDate,
                                 UpdatedAt = tr.UpdatedAt,
                                 IsDeleted = Convert.ToInt32(tr.IsDeleted),
                                 Quota = tr.Quota,
                                 CourseId = c.CourseId,
                                 CourseName = c.CourseName,
                                 CoursePrice = c.CoursePrice,
                                 ProgramTypeId = pt.ProgramTypeId,
                                 ProgramTypeName = pt.ProgramTypeName,
                                 ScheduleStatus =
                                 (tr.IsDeleted ? Constant.ScheduleStatus.Cancelled :
                                 tr.RescheduledAt != null ? Constant.ScheduleStatus.Reschedule : ""),

                                 Positions =
                                 (from tpm in this.DB.TrainingPositionMappings.AsNoTracking()
                                  join p in this.DB.Positions.AsNoTracking()
                                  on tpm.PositionId equals p.PositionId into ap
                                  from apn in ap.DefaultIfEmpty()

                                  where tpm.TrainingId == tr.TrainingId
                                  && filter.PositionId.Count > 0 ? filter.PositionId.Contains(tpm.PositionId) : tpm.TrainingId == tr.TrainingId
                                  select new UserSidePositionModel
                                  {
                                      PositionId = apn.PositionId,
                                      PositionName = apn.PositionName,
                                      PositionDescription = apn.PositionDescription
                                  }).ToList(),

                                 Outlets =
                                  (from tom in this.DB.TrainingOutletMappings.AsNoTracking()

                                   join o in this.DB.Outlets.AsNoTracking()
                                   on tom.OutletId equals o.OutletId into ao
                                   from aon in ao.DefaultIfEmpty()

                                   join a in this.DB.Areas.AsNoTracking()
                                   on aon.AreaId equals a.AreaId into aa
                                   from aan in aa.DefaultIfEmpty()

                                   join d in this.DB.Dealers.AsNoTracking()
                                   on aon.DealerId equals d.DealerId into ad
                                   from adn in ad.DefaultIfEmpty()

                                   where (tom.TrainingId == tr.TrainingId
                                   &&
                                   filter.AreaId.Count > 0 ? filter.AreaId.Contains(aan.AreaId) : tom.TrainingId == tr.TrainingId)
                                   &&
                                   (tom.TrainingId == tr.TrainingId
                                   &&
                                   filter.DealerId.Count > 0 ? filter.DealerId.Contains(adn.DealerId) : tom.TrainingId == tr.TrainingId)
                                   select new UserSideTimeTableOutletModel
                                   {
                                       OutletId = aon.OutletId,
                                       OutletName = aon.Name,
                                       AreaId = aan.AreaId,
                                       AreaName = aan.Name,
                                       DealerId = adn.DealerId,
                                       DealerName = adn.DealerName
                                   }).ToList()
                             }).AsQueryable();

            if (filter.PeriodStart != null)
            {
                initQuery = initQuery.Where(Q => Q.StartDate >= filter.PeriodStart.Value);
            }

            if (filter.PeriodEnd != null)
            {
                initQuery = initQuery.Where(Q => Q.StartDate <= filter.PeriodEnd.Value);
            }

            if (filter.IsFree || filter.IsNotFree)
            {
                if (filter.IsFree && !filter.IsNotFree)
                {
                    initQuery = initQuery.Where(Q => Q.CoursePrice == null || Q.CoursePrice.Value == 0);
                }
                else if (!filter.IsFree && filter.IsNotFree)
                {
                    initQuery = initQuery.Where(Q => Q.CoursePrice != null && Q.CoursePrice.Value != 0);
                }
            }

            switch (filter.SortBy)
            {
                case "name":
                    initQuery = initQuery.OrderBy(Q => Q.CourseName);
                    break;

                case "nameDesc":
                    initQuery = initQuery.OrderByDescending(Q => Q.CourseName);
                    break;

                case "date":
                    initQuery = initQuery.OrderBy(Q => Q.StartDate);
                    break;

                case "dateDesc":
                    initQuery = initQuery.OrderByDescending(Q => Q.StartDate);
                    break;

                default:
                    initQuery = initQuery.OrderByDescending(Q => Q.StartDate);
                    break;
            }

            var totalData = initQuery.Count();

            if (totalData <= filter.PageSize)
            {
                filter.PageIndex = 1;
            };

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var data = await initQuery
                .Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToListAsync();

            //var dataOutlet = data.Select(Q => Q.Outlets.Select(X => X.DealerId)).ToList();

            //if (filter.DealerId != null)
            //{
            //    foreach(var dataOutlet in data)
            //    {
            //        dataOutlet.Outlets.Where(Q => filter.DealerId.Contains(Q.DealerId)).ToList();
            //    }
            //}

            var response = new UserSideTimeTableResponseModel
            {
                TimeTables = data,
                TotalData = totalData
            };

            return response;
        }

        public async Task<UserSideTimeTableFilterValueModel> GetFilterValue()
        {
            var outletIds = await this
                .DB
                .TrainingOutletMappings
                .AsNoTracking()
                .Select(Q => Q.OutletId)
                .Distinct()
                .ToListAsync();

            var dataOutlets = await this
                .DB
                .Outlets
                .AsNoTracking()
                .Where(Q => outletIds.Contains(Q.OutletId))
                .Select(Q => new
                {
                    Q.AreaId,
                    Q.DealerId
                })
                .ToListAsync();

            // For data Areas.
            var areaIds = dataOutlets.Select(Q => Q.AreaId).Distinct().ToList();

            var dataAreas = await this
                .DB
                .Areas
                .AsNoTracking()
                .Where(Q => areaIds.Contains(Q.AreaId))
                .Select(Q => new UserSideAreaModel
                {
                    AreaId = Q.AreaId,
                    AreaName = Q.Name
                })
                .ToListAsync();

            // For data Dealer.
            var dealerIds = dataOutlets.Select(Q => Q.DealerId).Distinct().ToList();

            var dataDealers = await this
                .DB
                .Dealers
                .AsNoTracking()
                .Where(Q => dealerIds.Contains(Q.DealerId))
                .Select(Q => new UserSideDealerModel
                {
                    DealerId = Q.DealerId,
                    DealerName = Q.DealerName
                })
                .ToListAsync();

            // For data position
            var positionIds = await this
                .DB
                .TrainingPositionMappings
                .AsNoTracking()
                .Select(Q => Q.PositionId)
                .Distinct()
                .ToListAsync();

            var dataPositions = await this
                .DB
                .Positions
                .AsNoTracking()
                .Where(Q => positionIds.Contains(Q.PositionId))
                .Select(Q => new UserSidePositionModel
                {
                    PositionId = Q.PositionId,
                    PositionName = Q.PositionName,
                    PositionDescription = Q.PositionDescription
                })
                .ToListAsync();

            // For data price.
            var courseIds = await this
                .DB
                .Trainings
                .AsNoTracking()
                .Select(Q => Q.CourseId)
                .Distinct()
                .ToListAsync();

            var dataPrice = await this
                .DB
                .Courses
                .AsNoTracking()
                .Where(Q => courseIds.Contains(Q.CourseId) && Q.IsDeleted == false)
                .Select(Q => Q.CoursePrice.Value)
                .Distinct()
                .ToListAsync();

            var response = new UserSideTimeTableFilterValueModel
            {
                Areas = dataAreas,
                Dealers = dataDealers,
                Positions = dataPositions,
                PriceList = dataPrice
            };

            return response;
        }
    }
}