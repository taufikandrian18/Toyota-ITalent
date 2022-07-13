using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideCoachService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly UserSideBadgesService BadgesService;

        public UserSideCoachService(TalentContext talentContext, IFileStorageService fileService, UserSideBadgesService badgesService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
            this.BadgesService = badgesService;
        }

        public async Task<UserSideCoachResponseModel> GetCoachList(UserSideCoachFilterModel filter)
        {
            var queryCoachList = from c in this.DB.Coaches.AsNoTracking()
                                 join e in this.DB.Employees.AsNoTracking()
                                 on c.EmployeeId equals e.EmployeeId

                                 join b in this.DB.Blobs.AsNoTracking()
                                 on e.BlobId equals b.BlobId into ab
                                 from abn in ab.DefaultIfEmpty()
                                 where c.CoachIsActive == true
                                 select new
                                 {
                                     e.EmployeeId,
                                     e.EmployeeName,
                                     c.CoachIsActive,
                                     BlobId = e.BlobId.HasValue ? e.BlobId.Value : Guid.Empty,
                                     abn.Mime,
                                     c.CoachId
                                 };
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                queryCoachList = queryCoachList.Where(Q => Q.EmployeeName.ToLower().Contains(filter.Keyword.ToLower()) == true);
            }

            var dataCoachList = await queryCoachList.ToListAsync();
            var totalData = dataCoachList.Count();

            if (totalData <= filter.PageSize)
            {
                filter.PageIndex = 1;
            };

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var coaches = dataCoachList
                .Select(async X => new UserSideCoachModel
                {
                    EmployeeName = X.EmployeeName,
                    CoachIsActive = Convert.ToInt32(X.CoachIsActive),
                    ImageUrl = X.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.BlobId.ToString(), X.Mime),
                    TotalBadge = await this.BadgesService.GetTotalBadgeCoach(X.EmployeeId),
                    EmployeeId = X.EmployeeId,
                    CoachId = X.CoachId,

                    CoachTopics =
                    (from tm in this.DB.CoachTopicMappings.AsNoTracking()
                     join t in this.DB.Topics.AsNoTracking()
                     on tm.TopicId equals t.TopicId

                     where tm.CoachId == X.CoachId
                     select new UserSideTopicModel
                     {
                         TopicId = t.TopicId,
                         TopicName = t.TopicName,
                         EbadgesId = t.EbadgeId,
                         BlobId = t.BlobId
                     }).ToList()
                })
                .Select(Q => Q.Result)
                .Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToList();

            var responseCoaches = new UserSideCoachResponseModel
            {
                CoachList = coaches,
                TotalData = totalData
            };

            return responseCoaches;
        }

        public async Task<UserSideCoachScheduleBookingModel> GetCoachScheduleBooking(int coachId, string employeeId)
        {
            var initCoach = await (from cc in this.DB.Coaches.AsNoTracking()

                                   join e in this.DB.Employees.AsNoTracking()
                                   on cc.EmployeeId equals e.EmployeeId

                                   join bl in this.DB.Blobs.AsNoTracking()
                                   on e.BlobId equals bl.BlobId into abl
                                   from abln in abl.DefaultIfEmpty()

                                   where cc.CoachId == coachId
                                   select new
                                   {
                                       cc.CoachId,
                                       Employeeid = cc.EmployeeId,
                                       e.EmployeeName,
                                       BlobId = e.BlobId.HasValue ? abln.BlobId : Guid.Empty,
                                       abln.Mime,
                                       ListSchedule =
                                       (from cs in this.DB.CoachSchedules.AsNoTracking()
                                       join cbs in this.DB.CoachBookingSchedules.AsNoTracking() on cs.CoachScheduleId equals cbs.CoachScheduleId into lcbs
                                       from cbs in lcbs.DefaultIfEmpty()
                                        where cs.CoachId == cc.CoachId && (cbs.EmployeeId == employeeId || cbs.EmployeeId == null)
                                        select new UserSideCoachScheduleViewModel
                                        {
                                            CoachId = cs.CoachId,
                                            EndDateTime = cs.EndDateTime,
                                            StartDateTime = cs.StartDateTime,
                                            ScheduleName = cs.ScheduleName,
                                            CoachScheduleId = cs.CoachScheduleId,
                                            IsBooked =  cbs.EmployeeId != null ? true : false
                                        }).ToList()
                                   }).FirstOrDefaultAsync();

            // For data total badge.
            var dataTotalBadge = await this.BadgesService.GetTotalBadgeCoach(initCoach.Employeeid);

            // Get image if blobId not null.
            string imageUrlEmployee = null;
            if (initCoach.BlobId != Guid.Empty)
            {
                imageUrlEmployee = await this.FileService.GetFileAsync(initCoach.BlobId.ToString(), initCoach.Mime);
            }

            // Store all data to model.
            var dataCoach = new UserSideCoachScheduleBookingModel
            {
                CoachId = initCoach.CoachId,
                Employeeid = initCoach.Employeeid,
                EmployeeName = initCoach.EmployeeName,
                ImageUrl = imageUrlEmployee,
                ScheduleList = initCoach.ListSchedule,
                TotalBadge = dataTotalBadge
            };

            return dataCoach;
        }
    }
}