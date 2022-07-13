using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class CoachesService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public CoachesService(TalentContext talentContext, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this.ClaimMan = claimHelper;
        }

        public async Task<CoachesViewModel> GetAllCoachesViewFiltered(CoachesFilter filter)
        {
            var query = (from c in DB.Coaches
                         join cs in DB.CoachSchedules on c.CoachId equals cs.CoachId into css
                         from cs in css.DefaultIfEmpty()
                         join ctm in DB.CoachTopicMappings on c.CoachId equals ctm.CoachId
                         join t in DB.Topics on ctm.TopicId equals t.TopicId
                         join e in DB.Ebadges on t.EbadgeId equals e.EbadgeId
                         join emp in DB.Employees on c.EmployeeId equals emp.EmployeeId
                         select new
                         {
                             CoachId = c.CoachId,
                             TopicId = ctm.TopicId,
                             CoachIsActive = c.CoachIsActive,
                             Category = c.Category,
                             CoachName = emp.EmployeeName,
                             CoachScheduleId = cs == null ? int.MinValue : cs.CoachScheduleId,
                             EbadgeId = t.EbadgeId,
                             EmployeeId = c.EmployeeId,
                             EndDateTime = cs == null ? "" : cs.EndDateTime.ToString(),
                             StartDateTime = cs == null ? "" : cs.StartDateTime.ToString(),
                             EndDateTimeDate = cs == null ? DateTime.MinValue : cs.EndDateTime,
                             StartDateTimeDate = cs == null ? DateTime.MinValue : cs.StartDateTime,
                             TopicName = t.TopicName,
                             EbadgeName = e.EbadgeName,
                             CreatedAt = c.CreatedAt == null ? "" : c.CreatedAt.ToString(),
                             UpdatedAt = c.UpdatedAt == null ? "" : c.UpdatedAt.ToString(),
                             CreatedAtDate = c.CreatedAt,
                             UpdatedAtDate = c.UpdatedAt,
                         }).AsNoTracking();

            if (string.IsNullOrEmpty(filter.CoachName) == false)
            {
                query = query.Where(Q => Q.CoachName.Contains(filter.CoachName));
            }

            if (string.IsNullOrEmpty(filter.TopicName) == false)
            {
                query = query.Where(Q => Q.TopicName.Contains(filter.TopicName));
            }

            if (string.IsNullOrEmpty(filter.EbadgeName) == false)
            {
                query = query.Where(Q => Q.EbadgeName.StartsWith(filter.EbadgeName));
            }

            if (filter.CoachIsActive != null)
            {
                query = query.Where(Q => Q.CoachIsActive == filter.CoachIsActive);
            }

            if (filter.Category != null)
            {
                query = query.Where(Q => Q.Category == filter.Category);
            }

            if (filter.DateFilterStart != null && filter.DateFilterEnd != null)
            {
                var newStartDate = filter.DateFilterStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateFilterEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAtDate >= startDate && Q.CreatedAtDate < endDate) ||
                                         (Q.UpdatedAtDate >= startDate && Q.UpdatedAtDate < endDate));
            }

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.StartDateTimeDate >= startDate && Q.StartDateTimeDate < endDate) ||
                                         (Q.EndDateTimeDate >= startDate && Q.EndDateTimeDate < endDate));
            }

            switch (filter.SortBy)
            {
                case "coachName":
                    query = query.OrderBy(Q => Q.CoachName);
                    break;
                case "coachNameDesc":
                    query = query.OrderByDescending(Q => Q.CoachName);
                    break;
                case "category":
                    query = query.OrderBy(Q => Q.Category);
                    break;
                case "topicName":
                    query = query.OrderBy(Q => Q.TopicName);
                    break;
                case "topicNameDesc":
                    query = query.OrderByDescending(Q => Q.TopicName);
                    break;
                case "coachIsActive":
                    query = query.OrderBy(Q => Q.CoachIsActive);
                    break;
                case "coachIsActiveDesc":
                    query = query.OrderByDescending(Q => Q.CoachIsActive);
                    break;
                case "ebadgeName":
                    query = query.OrderBy(Q => Q.EbadgeName);
                    break;
                case "ebadgeNameDesc":
                    query = query.OrderByDescending(Q => Q.EbadgeName);
                    break;
                case "createDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case "coachSchedule":
                    query = query.OrderBy(Q => Q.StartDateTime);
                    break;
                case "coachScheduleDesc":
                    query = query.OrderByDescending(Q => Q.StartDateTime);
                    break;
                case "":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case null:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.CountAsync();
            var queryResult = await query.Select(
                Q => new CoachesModel
                {
                    CoachId = Q.CoachId,
                    TopicId = Q.TopicId,
                    CoachIsActive = Q.CoachIsActive,
                    Category = Q.Category,
                    CoachName = Q.CoachName,
                    CoachScheduleId = Q == null ? int.MinValue : Q.CoachScheduleId,
                    EbadgeId = Q.EbadgeId,
                    EmployeeId = Q.EmployeeId,
                    EndDateTime = Q.EndDateTimeDate == null || Q.EndDateTimeDate == DateTime.MinValue ? "" : Q.EndDateTimeDate.ToString("HH:mm"),
                    StartDateTime = Q.StartDateTimeDate == null || Q.StartDateTimeDate == DateTime.MinValue ? "" : Q.StartDateTimeDate.ToString("dd/MM/yyyy HH:mm"),
                    TopicName = Q.TopicName,
                    EbadgeName = Q.EbadgeName,
                    CreatedAt = Q.CreatedAtDate == null ? "" : Q.CreatedAtDate.ToString("dd/MM/yyyy"),
                    UpdatedAt = Q.UpdatedAtDate == null ? "" : Q.UpdatedAtDate.ToString("dd/MM/yyyy"),
                }).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var result = new CoachesViewModel
            {
                ListCoaches = queryResult,
                TotalData = totalData
            };

            return result;
        }

        public async Task<ListEmployeeForCoach> GetAllEmployeeForAddCoach(string employeeName,string IsTam, string IsDealer)
        {
            var getAllEmployee = from e in DB.Employees
                                 join outs in this.DB.Outlets on e.OutletId equals outs.OutletId into grpjoin_e_outs
                                 from outs in grpjoin_e_outs.DefaultIfEmpty()
                                 where !DB.Coaches.Any(Q => Q.EmployeeId == e.EmployeeId) && e.IsDeleted == false && e.ManpowerStatusType != "GuestAccount"
                                 select new EmployeeForCoachModel
                                 {
                                     EmployeeName = e.EmployeeName,
                                     EmployeeId = e.EmployeeId,
                                 };

            if (!String.IsNullOrWhiteSpace(IsTam))
            {
                getAllEmployee = from e in DB.Employees
                                 join outs in this.DB.Outlets on e.OutletId equals outs.OutletId into grpjoin_e_outs
                                 from outs in grpjoin_e_outs.DefaultIfEmpty()
                                 where !DB.Coaches.Any(Q => Q.EmployeeId == e.EmployeeId) && e.OutletId == null && e.IsDeleted == false && e.ManpowerStatusType != "GuestAccount"
                                 select new EmployeeForCoachModel
                                 {
                                     EmployeeName = e.EmployeeName,
                                     EmployeeId = e.EmployeeId,
                                 };
            }

            if (!String.IsNullOrWhiteSpace(IsDealer))
            {
                getAllEmployee = from e in DB.Employees
                                 join outs in this.DB.Outlets on e.OutletId equals outs.OutletId into grpjoin_e_outs
                                 from outs in grpjoin_e_outs.DefaultIfEmpty()
                                 where !DB.Coaches.Any(Q => Q.EmployeeId == e.EmployeeId) && e.OutletId != null && e.IsDeleted == false && e.ManpowerStatusType != "GuestAccount"
                                 select new EmployeeForCoachModel
                                 {
                                     EmployeeName = e.EmployeeName,
                                     EmployeeId = e.EmployeeId,
                                 };
            }

            var result = await getAllEmployee.Where(Q => Q.EmployeeName.StartsWith(employeeName)).ToListAsync();

            if (!String.IsNullOrWhiteSpace(IsDealer))
            {
                var resultFormatting = await getAllEmployee.Where(Q => Q.EmployeeName.StartsWith(employeeName)).Select(Q => new EmployeeForCoachModel
                {
                    EmployeeName = Q.EmployeeName + " (" + DB.Employees.Where(x => x.EmployeeId == Q.EmployeeId).Include(x => x.Outlet).Select(x => x.Outlet.Name).FirstOrDefault() + ") - " + Q.EmployeeId,
                    EmployeeId = Q.EmployeeId
                }).ToListAsync();

                if (resultFormatting == null)
                {
                    return new ListEmployeeForCoach();
                }

                var searchResultFormatting = new ListEmployeeForCoach
                {
                    ListEmployee = resultFormatting
                };

                return searchResultFormatting;

            }

            if (!String.IsNullOrWhiteSpace(IsTam))
            {
                var resultFormatting = await getAllEmployee.Where(Q => Q.EmployeeName.StartsWith(employeeName)).Select(Q => new EmployeeForCoachModel
                {
                    EmployeeName = Q.EmployeeName + " (TAM) - " + Q.EmployeeId,
                    EmployeeId = Q.EmployeeId
                }).ToListAsync();

                if (resultFormatting == null)
                {
                    return new ListEmployeeForCoach();
                }

                var searchResultFormatting = new ListEmployeeForCoach
                {
                    ListEmployee = resultFormatting
                };

                return searchResultFormatting;
            }

            if (result == null)
            {
                return new ListEmployeeForCoach();
            }

            var searchResult = new ListEmployeeForCoach
            {
                ListEmployee = result
            };

            return searchResult;
        }

        public async Task<ListEmployeeForCoach> GetAllEmployee(string name, string IsTam, string IsDealer)
        {
            var searchEmployee = await this.DB.Employees.Select(Q => new EmployeeForCoachModel
            {
                EmployeeName = Q.EmployeeName,
                EmployeeId = Q.EmployeeId,
            }).Where(Q => Q.EmployeeName.StartsWith(name)).ToListAsync();

            if (!String.IsNullOrWhiteSpace(IsTam))
            {
                searchEmployee = await this.DB.Employees.Where(Q => Q.OutletId == null).Select(Q => new EmployeeForCoachModel
                {
                    EmployeeName = Q.EmployeeName,
                    EmployeeId = Q.EmployeeId,
                }).Where(Q => Q.EmployeeName.StartsWith(name)).ToListAsync();
            }

            if (!String.IsNullOrWhiteSpace(IsDealer))
            {
                searchEmployee = await this.DB.Employees.Where(Q => Q.OutletId != null).Select(Q => new EmployeeForCoachModel
                {
                    EmployeeName = Q.EmployeeName,
                    EmployeeId = Q.EmployeeId,
                }).Where(Q => Q.EmployeeName.StartsWith(name)).ToListAsync();
            }

            if (searchEmployee == null)
            {
                searchEmployee = new List<EmployeeForCoachModel>();
            }

            if (!String.IsNullOrWhiteSpace(IsTam))
            {
                searchEmployee = searchEmployee.Select(Q => new EmployeeForCoachModel
                {
                    EmployeeName = Q.EmployeeName + " (TAM) - " + Q.EmployeeId,
                    EmployeeId = Q.EmployeeId
                }).ToList();
            }
            if (!String.IsNullOrWhiteSpace(IsDealer))
            {
                searchEmployee = searchEmployee.Select(Q => new EmployeeForCoachModel
                {
                    EmployeeName = Q.EmployeeName + " (" + DB.Employees.Where(x => x.EmployeeId == Q.EmployeeId).Include(x => x.Outlet).Select(x => x.Outlet.Name).FirstOrDefault() + ") - " + Q.EmployeeId,
                    EmployeeId = Q.EmployeeId
                }).ToList();
            }

            var searchResult = new ListEmployeeForCoach
            {
                ListEmployee = searchEmployee
            };

            return searchResult;
        }

        public async Task<List<TopicsEbadgeJoinModelForCoach>> GetTopicsAndEbadge()
        {
            var query = from t in DB.Topics
                        join e in DB.Ebadges on t.EbadgeId equals e.EbadgeId
                        select new TopicsEbadgeJoinModelForCoach
                        {
                            EbadgeId = e.EbadgeId,
                            TopicId = t.TopicId,
                            EbadgeName = e.EbadgeName,
                            TopicName = t.TopicName
                        };

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<CoachViewDetail> GetCoachViewDetail(int coachId)
        {
            var topicEbadgeMapping = from ctm in DB.CoachTopicMappings
                                     join t in DB.Topics on ctm.TopicId equals t.TopicId
                                     join e in DB.Ebadges on t.EbadgeId equals e.EbadgeId
                                     where ctm.CoachId == coachId
                                     select new TopicsEbadgeJoinModelForCoach
                                     {
                                         TopicId = t.TopicId,
                                         TopicName = t.TopicName,
                                         EbadgeId = e.EbadgeId,
                                         EbadgeName = e.EbadgeName
                                     };
            var result = await topicEbadgeMapping.ToListAsync();
            var schedule = this.DB.CoachSchedules.Where(Q => Q.CoachId == coachId).Select(Q => new
            {
                CoachScheduleId = Q.CoachScheduleId,
                StartDateTimeDate = Q.StartDateTime,
                EndDateTimeDate = Q.EndDateTime
            }).AsNoTracking();

            var resultSchedule = await schedule.Select(
                Q => new CoachSchedulesFormModel
                {
                    CoachScheduleId = Q.CoachScheduleId,
                    EndDateTime = Q.EndDateTimeDate.ToString("HH:mm"),
                    StartDateTime = Q.StartDateTimeDate.ToString("HH:mm"),
                    StartDate = Q.StartDateTimeDate.ToString("dd/MM/yyyy"),
                    StartDateTimeDate = Q.StartDateTimeDate,
                    EndDateTimeDate = Q.EndDateTimeDate
                }).ToListAsync();

            var resultView = new CoachViewDetail
            {
                ListCoachSchedule = resultSchedule,
                ListTopicEbadge = result
            };

            return resultView;
        }

        /// <summary>
        /// Insert Coach
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertCoaches(CoachFormModel model)
        {
            try
            {
                //Validation if coach exist
                var findCoach = await this.DB.Coaches.Where(Q => Q.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();

                if (findCoach != null)
                {
                    return false;
                }

                //Validation of empty topic
                if (model.TopicId.Count == 0)
                {
                    return false;
                }

                //Validate if topics are duplicate
                var duplicates = model.TopicId.GroupBy(Q => Q).Where(c => c.Count() > 1).Select(y => y.Key).ToList();
                if (duplicates.Count != 0)
                {
                    return false;
                }

                var userLogin = this.ClaimMan.GetLoginUserId();

                var insertCoach = new Coaches
                {
                    CoachIsActive = model.CoachIsActive,
                    EmployeeId = model.EmployeeId,
                    Category = model.Category,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = userLogin,
                    UpdatedBy = userLogin,
                };

                this.DB.Coaches.Add(insertCoach);

                var coachId = insertCoach.CoachId;

                foreach (var schedule in model.CoachSchedule)
                {
                    var insertSchedule = new CoachSchedules
                    {
                        CoachId = coachId,
                        EndDateTime = schedule.EndDateTimeDate.GetValueOrDefault().ToIndonesiaTimeZone(),
                        StartDateTime = schedule.StartDateTimeDate.GetValueOrDefault().ToIndonesiaTimeZone()
                    };

                    this.DB.CoachSchedules.Add(insertSchedule);
                }

                foreach (var topic in model.TopicId)
                {
                    var insertTopic = new CoachTopicMappings
                    {
                        TopicId = topic,
                        CoachId = coachId
                    };

                    this.DB.CoachTopicMappings.Add(insertTopic);
                }

                var findEmployee = await this.DB.Employees.Where(Q => Q.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();
                findEmployee.IsCoach = true;

                await this.DB.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCoach(CoachFormModel model)
        {
            var duplicates = model.TopicId.GroupBy(Q => Q).Where(c => c.Count() > 1).Select(y => y.Key).ToList();
            if (duplicates.Count != 0)
            {
                return false;
            }

            var findCoach = await this.DB.Coaches.Where(Q => Q.CoachId == model.CoachId).FirstOrDefaultAsync();
            if (findCoach == null)
            {
                return false;
            }

            var duplicateQuery = this.DB.Coaches.Where(Q => Q.EmployeeId == model.EmployeeId);
            var resultQuery = await duplicateQuery.FirstOrDefaultAsync();

            if (resultQuery != null)
            {
                if (resultQuery != findCoach)
                {
                    return false;
                }
            }

            var userLogin = this.ClaimMan.GetLoginUserId();

            findCoach.EmployeeId = model.EmployeeId;
            findCoach.CoachIsActive = model.CoachIsActive;
            findCoach.Category = model.Category;
            findCoach.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            findCoach.UpdatedBy = userLogin;

            var queryGetCurrentTopicMapping = await this.DB.CoachTopicMappings.Where(Q => Q.CoachId == model.CoachId).ToListAsync();

            foreach (var a in queryGetCurrentTopicMapping)
            {
                this.DB.CoachTopicMappings.Remove(a);
            }

            foreach (var topicId in model.TopicId)
            {
                var insert = new CoachTopicMappings
                {
                    CoachId = findCoach.CoachId,
                    TopicId = topicId
                };

                this.DB.CoachTopicMappings.Add(insert);
            }

            // 1 2 3 4
            var getAllCoachSchedules = await this.DB.CoachSchedules.Where(Q => Q.CoachId == model.CoachId).ToListAsync();

            // Yang sudah ada sebelumnya 2 4
            var currentExisitingCoachSchedulesIds = model.CoachSchedule.Where(Q => Q.CoachScheduleId != null).Select(Q => Q.CoachScheduleId).ToList();

            var currentCoachStillExist = getAllCoachSchedules.Where(Q => currentExisitingCoachSchedulesIds.Contains(Q.CoachScheduleId)).ToList();

            foreach (var currenctSchedule in currentCoachStillExist)
            {
                var findSchedules = model.CoachSchedule.Where(Q => Q.CoachScheduleId == currenctSchedule.CoachScheduleId).FirstOrDefault();

                currenctSchedule.EndDateTime = findSchedules.EndDateTimeDate.GetValueOrDefault().ToIndonesiaTimeZone();
                currenctSchedule.StartDateTime = findSchedules.StartDateTimeDate.GetValueOrDefault().ToIndonesiaTimeZone();
            }

            // Data yang di remove 1 3
            var nonExistedSchedules = getAllCoachSchedules.Where(Q => !currentExisitingCoachSchedulesIds.Contains(Q.CoachScheduleId)).ToList();

            foreach (var a in nonExistedSchedules)
            {
                this.DB.CoachSchedules.Remove(a);
            }

            //Get data schedules yang baru
            var newCoachSchedules = model.CoachSchedule.Where(Q => Q.CoachScheduleId == null).ToList();

            foreach (var schedule in newCoachSchedules)
            {
                var insertSchedule = new CoachSchedules
                {
                    CoachId = model.CoachId.GetValueOrDefault(),
                    EndDateTime = schedule.EndDateTimeDate.GetValueOrDefault().ToIndonesiaTimeZone(),
                    StartDateTime = schedule.StartDateTimeDate.GetValueOrDefault().ToIndonesiaTimeZone()
                };

                this.DB.CoachSchedules.Add(insertSchedule);
            }

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCoachMapping(CoachDeleteFormModel model)
        {
            var searchCoach = await this.DB.Coaches.Where(Q => Q.CoachId == model.CoachId).FirstOrDefaultAsync();

            if (model.DeleteAll == true)
            {
                if (searchCoach == null)
                {
                    return false;
                }

                this.DB.Coaches.Remove(searchCoach);

                var findEmployee = await this.DB.Employees.Where(Q => Q.EmployeeId == searchCoach.EmployeeId).FirstOrDefaultAsync();
                findEmployee.IsCoach = false;

                await this.DB.SaveChangesAsync();
                return true;
            }

            if (model.ScheduleId.Count > 0)
            {
                var listRemoveSchedule = await this.DB.CoachSchedules.Where(Q => model.ScheduleId.Contains(Q.CoachScheduleId)).ToListAsync();
                this.DB.CoachSchedules.RemoveRange(listRemoveSchedule);
            }

            if (model.TopicId.Count > 0)
            {
                var listRemoveTopic = await this.DB.CoachTopicMappings.Where(Q => model.TopicId.Contains(Q.TopicId)).ToListAsync();
                this.DB.CoachTopicMappings.RemoveRange(listRemoveTopic);
            }

            searchCoach.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            searchCoach.UpdatedBy = this.ClaimMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateUpdate(int id, string employeeId)
        {
            var findCoach = await this.DB.Coaches.Where(Q => Q.CoachId == id).FirstOrDefaultAsync();
            if (findCoach == null)
            {
                return false;
            }

            var duplicateQuery = this.DB.Coaches.Where(Q => Q.EmployeeId == employeeId);
            var resultQuery = await duplicateQuery.FirstOrDefaultAsync();

            if (resultQuery != null)
            {
                if (resultQuery != findCoach)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
