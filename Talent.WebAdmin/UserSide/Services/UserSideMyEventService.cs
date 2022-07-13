using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideMyEventService
    {

        private readonly TalentContext DB;
        private readonly IFileStorageService FileServiceMan;

        public UserSideMyEventService(TalentContext db, IFileStorageService fileService)
        {
            this.DB = db;
            this.FileServiceMan = fileService;
        }

        public void GetUpcomingEvent()
        {

        }

        /// <summary>
        /// Function buat get all event for main page.
        /// Gak dipake jadinya pake 1-1 aja (kata orang front end).
        /// </summary>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<MainPageModel> GetMainPageAsync(string user, EventFilterModel filter)
        {
            var getPopular = await GetPopularEventAsync(user, filter);
            var getSaved = await GetSavedEventAsync(user, filter);
            var getRecomend = await GetRecomendEvent(user, filter);

            var model = new MainPageModel
            {
                PopularEventList = getPopular,
                RecomendEventList = getRecomend,
                SavedEventList = getSaved,
            };

            return model;
        }

        /// <summary>
        /// Get event yang rekomen buat user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<SingleViewEventModel>> GetRecomendEvent(string user, EventFilterModel filter)
        {
            if (filter.Page <= 0)
            {
                filter.Page = 1;
            }

            if (filter.ItemSize <= 0)
            {
                filter.ItemSize = 10;
            }

            if (string.IsNullOrEmpty(filter.Name))
            {
                filter.Name = "";
            }
            else
            {
                filter.Name = filter.Name.ToLower();
            }

            var employeeId = "";
            try
            {
                employeeId = user;
            }
            catch
            {
                // return null;
            }

            var employeeInformation = await (from e in this.DB.Employees
                                             join epm in this.DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                             where e.EmployeeId == employeeId
                                             select new
                                             {
                                                 epm.PositionId,
                                                 e.OutletId,
                                             }).ToListAsync();

            var getOutlet = employeeInformation.Select(Q => Q.OutletId).FirstOrDefault();
            var getPositionId = employeeInformation.Select(Q => Q.PositionId).ToList();

            //var getOutlet = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.OutletId).FirstOrDefaultAsync();
            var getArea = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.Outlet.AreaId).FirstOrDefaultAsync();

            Random numbers = new Random();

            //var query = (from eom in DB.EventOutletMappings
            //             join a in DB.ApprovalToEvents
            //             on eom.EventId equals a.EventId into at
            //             from ate in at.DefaultIfEmpty()
            //             join e in DB.EmployeeEventMappings
            //             on eom.EventId equals e.EventId into em
            //             from eem in em.DefaultIfEmpty()
            //             where
            //             eom.Event.IsDeleted == false &&
            //             ate.Approval.ApprovalStatus.ApprovalName == ApprovalStatusesEnum.Approve
            //             && eom.OutletId == getOutlet
            //             //&& eom.Outlet.AreaId == getArea
            //             select new
            //             {
            //                 EventId = eom.EventId,
            //                 Title = eom.Event.Name,
            //                 Description = eom.Event.Description,
            //                 EventDate = eom.Event.StartDateTime,
            //                 location = eom.Event.Location,
            //                 startDate = eom.Event.StartDateTime,
            //                 endDate = eom.Event.EndDateTime,
            //                 // start = eom.Event.StartDateTime.TimeOfDay,
            //                 start = eom.Event.StartDateTime.ToString("HH:mm"),
            //                 // end = eom.Event.EndDateTime.TimeOfDay,
            //                 end = eom.Event.EndDateTime.ToString("HH:mm"),
            //                 ApprovalDate = eom.Event.ApprovedAt,
            //                 BlobId = eom.Event.BlobId.HasValue ? eom.Event.BlobId.ToString() : "",
            //                 isJoin = eem.IsJoined.HasValue ? eem.IsJoined.Value : false,
            //                 isSave = eem.IsSaved.HasValue ? eem.IsSaved.Value : false
            //             });

            var query = this.DB.GetEvents(employeeId, getOutlet, getPositionId);

            //var total = await query.CountAsync();
            //var list = await query.ToListAsync();
            if (string.IsNullOrEmpty(filter.Name) == false)
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Name.ToLower()));
            }
            //var total2 = await query.CountAsync();

            // query = query.OrderBy(Q => numbers.Next());
            query = query.OrderByDescending(Q => Q.ApprovedAt);

            //v.1.0
            var getQueryList = await query
                .AsNoTracking()
                .Skip((filter.Page - 1) * filter.ItemSize)
                .Take(filter.ItemSize)
                .ToListAsync();

            var getList = new List<SingleViewEventModel>();

            foreach (var data in getQueryList)
            {

                var getUrl = new FileModel();

                if (data.BlobId != Guid.Empty.ToString() && data.BlobId != null)
                {
                    getUrl = await FileServiceMan.GetFileDetailAsync(data.BlobId);
                }
                else
                {
                    getUrl = new FileModel
                    {
                        FileUrl = "",
                        Mime = "",
                        Name = ""
                    };
                }

                // var getUrl = await FileServiceMan.GetFileDetailAsync(data.BlobId);

                var temp = new SingleViewEventModel
                {
                    EventId = data.EventId,
                    ImageUrl = getUrl.FileUrl,
                    Title = data.Title,
                    Description = data.Description,
                    Location = data.Location,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    StartTime = data.StartTime.ToString("HH:mm"),
                    EndTime = data.EndTime.ToString("HH:mm"),
                    IsJoin = data.IsJoin.HasValue ? data.IsJoin.Value : false,
                    IsSave = data.IsSave.HasValue ? data.IsSave.Value : false,
                };

                getList.Add(temp);
            }

            //v 2.0
            //var getList = await query
            //    .Select(Q => new SingleViewEventModel
            //    {
            //        EventId = Q.EventId,
            //        ImageUrl = FileServiceMan.GetFileDetailAsync(Q.BlobId).Result.FileUrl,
            //        Title = Q.Title,
            //        Description = Q.Description,
            //        Location = Q.location,
            //        StartDate = Q.startDate,
            //        EndDate = Q.endDate,
            //        StartTime = Q.start,
            //        EndTime = Q.end,
            //        IsJoin = Q.isJoin,
            //        IsSave = Q.isSave,
            //    })
            //    .AsNoTracking()
            //    .Skip((filter.Page - 1) * filter.ItemSize)
            //    .Take(filter.ItemSize)
            //    .ToListAsync();

            return getList;
        }

        public async Task<List<SingleViewEventModel>> GetPopularEventAsync(string user, EventFilterModel filter)
        {
            if (filter.Page <= 0)
            {
                filter.Page = 1;
            }

            if (filter.ItemSize <= 0)
            {
                filter.ItemSize = 10;
            }

            if (string.IsNullOrEmpty(filter.Name))
            {
                filter.Name = "";
            }
            else
            {
                filter.Name = filter.Name.ToLower();
            }

            var employeeId = "";
            try
            {
                employeeId = user;
            }
            catch
            {
                // return null;
            }

            var employeeInformation = await (from e in this.DB.Employees
                                             join epm in this.DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                             where e.EmployeeId == employeeId
                                             select new
                                             {
                                                 epm.PositionId,
                                                 e.OutletId,
                                             }).ToListAsync();

            var getOutlet = employeeInformation.Select(Q => Q.OutletId).FirstOrDefault();
            var getPositionId = employeeInformation.Select(Q => Q.PositionId).ToList();

            //var getOutlet = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.OutletId).FirstOrDefaultAsync();
            var getArea = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.Outlet.AreaId).FirstOrDefaultAsync();

            //var query = (from eom in DB.EventOutletMappings
            //             join a in DB.ApprovalToEvents
            //             on eom.EventId equals a.EventId into at
            //             from ate in at.DefaultIfEmpty()
            //             join e in DB.EmployeeEventMappings
            //            on eom.EventId equals e.EventId into em
            //            from eem in em.DefaultIfEmpty().Where(Q => Q.EmployeeId == employeeId || Q.EmployeeId == null)
            //        where 
            //        eom.Event.IsDeleted == false && 
            //        ate.Approval.ApprovalStatus.ApprovalName == ApprovalStatusesEnum.Approve
            //        && eom.OutletId == getOutlet
            //        //&& eom.Outlet.AreaId == getArea
            //             select new {
            //            EventId = eom.EventId,
            //            Title = eom.Event.Name,
            //            Description = eom.Event.Description,
            //            EventDate = eom.Event.StartDateTime,
            //            TotalView = eom.Event.TotalView,
            //            location = eom.Event.Location,
            //            startDate = eom.Event.StartDateTime,
            //            endDate = eom.Event.EndDateTime,
            //            // start = eom.Event.StartDateTime.TimeOfDay,
            //            start = eom.Event.StartDateTime.ToString("HH:mm"),
            //            // end = eom.Event.EndDateTime.TimeOfDay,
            //            end = eom.Event.EndDateTime.ToString("HH:mm"),
            //            BlobId = eom.Event.BlobId.HasValue? eom.Event.BlobId.ToString() : "",
            //            isJoin = eem.IsJoined.HasValue? eem.IsJoined.Value : false,
            //            isSave = eem.IsSaved.HasValue? eem.IsSaved.Value : false
            //        });

            var query = this.DB.GetEvents(employeeId, getOutlet, getPositionId);

            if (string.IsNullOrEmpty(filter.Name) == false)
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Name.ToLower()));
            }

            query = query.OrderByDescending(Q => Q.IsJoin);

            //v 1.0
            var getQueryList = await query
                .AsNoTracking()
                .Skip((filter.Page - 1) * filter.ItemSize)
                .Take(filter.ItemSize)
                .ToListAsync();

            var getList = new List<SingleViewEventModel>();

            foreach (var data in getQueryList)
            {

                var getUrl = new FileModel();

                if (data.BlobId != Guid.Empty.ToString() && data.BlobId != null)
                {
                    getUrl = await FileServiceMan.GetFileDetailAsync(data.BlobId);
                }
                else
                {
                    getUrl = new FileModel
                    {
                        FileUrl = "",
                        Mime = "",
                        Name = ""
                    };
                }

                // var getUrl = await FileServiceMan.GetFileDetailAsync(data.BlobId);

                var temp = new SingleViewEventModel
                {
                    EventId = data.EventId,
                    ImageUrl = getUrl.FileUrl,
                    Title = data.Title,
                    Description = data.Description,
                    Location = data.Location,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    StartTime = data.StartTime.ToString("HH:mm"),
                    EndTime = data.EndTime.ToString("HH:mm"),
                    IsJoin = data.IsJoin.HasValue ? data.IsJoin.Value : false,
                    IsSave = data.IsSave.HasValue ? data.IsSave.Value : false,
                };

                getList.Add(temp);
            }

            //v 2.0
            //var getList = await query
            //    .Select(Q => new SingleViewEventModel
            //    {
            //        EventId = Q.EventId,
            //        ImageUrl = FileServiceMan.GetFileDetailAsync(Q.BlobId).Result.FileUrl,
            //        Title = Q.Title,
            //        Description = Q.Description,
            //        Location = Q.location,
            //        StartDate = Q.startDate,
            //        EndDate = Q.endDate,
            //        StartTime = Q.start,
            //        EndTime = Q.end,
            //        IsJoin = Q.isJoin,
            //        IsSave = Q.isSave,
            //    })
            //    .AsNoTracking()
            //    .Skip((filter.Page - 1) * filter.ItemSize)
            //    .Take(filter.ItemSize)
            //    .ToListAsync();

            return getList;
        }

        /// <summary>
        /// Get saved Event by employee who login.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<SingleViewEventModel>> GetSavedEventAsync(string user, EventFilterModel filter)
        {
            if (filter.Page <= 0)
            {
                filter.Page = 1;
            }

            if (filter.ItemSize <= 0)
            {
                filter.ItemSize = 10;
            }

            if (string.IsNullOrEmpty(filter.Name))
            {
                filter.Name = "";
            }
            else
            {
                filter.Name = filter.Name.ToLower();
            }

            var employeeId = "";
            try
            {
                employeeId = user;
            }
            catch
            {
                // return null;
            }

            var getOutlet = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.OutletId).FirstOrDefaultAsync();
            var getArea = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.Outlet.AreaId).FirstOrDefaultAsync();

            var query = (from eom in DB.EventOutletMappings
                         join a in DB.ApprovalToEvents
                         on eom.EventId equals a.EventId into at
                         from ate in at.DefaultIfEmpty()
                         join map in DB.EmployeeEventMappings
                         on ate.EventId equals map.EventId
                         where
                         eom.Event.IsDeleted == false
                         && ate.Approval.ApprovalStatus.ApprovalName == ApprovalStatusesEnum.Approve
                         && map.EmployeeId == employeeId
                         && eom.OutletId == getOutlet
                         && map.IsSaved == true
                         && eom.Event.ApprovedAt != null
                         //&& eom.Outlet.AreaId == getArea
                         select new
                         {
                             EventId = eom.EventId,
                             Title = eom.Event.Name,
                             Description = eom.Event.Description,
                             startDate = eom.Event.StartDateTime,
                             endDate = eom.Event.EndDateTime,
                             // start = eom.Event.StartDateTime.TimeOfDay,
                             start = eom.Event.StartDateTime.ToString("HH:mm"),
                             // end = eom.Event.EndDateTime.TimeOfDay,
                             end = eom.Event.EndDateTime.ToString("HH:mm"),
                             location = eom.Event.Location,
                             BlobId = eom.Event.BlobId.HasValue ? eom.Event.BlobId.ToString() : "",
                             isJoin = map.IsJoined.HasValue ? map.IsJoined.Value : false,
                             isSave = map.IsSaved.HasValue ? map.IsSaved.Value : false
                         });
            if (string.IsNullOrEmpty(filter.Name) == false)
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Name.ToLower()));
            }

            query = query.OrderByDescending(Q => Q.EventId);

            //v1.0
            var getQueryList = await query
                .AsNoTracking()
                .Skip((filter.Page - 1) * filter.ItemSize)
                .Take(filter.ItemSize)
                .ToListAsync();

            var getList = new List<SingleViewEventModel>();

            foreach (var data in getQueryList)
            {

                var getUrl = new FileModel();

                if (data.BlobId != Guid.Empty.ToString() && data.BlobId != null)
                {
                    getUrl = await FileServiceMan.GetFileDetailAsync(data.BlobId);
                }
                else
                {
                    getUrl = new FileModel
                    {
                        FileUrl = "",
                        Mime = "",
                        Name = ""
                    };
                }

                // var getUrl = await FileServiceMan.GetFileDetailAsync(data.BlobId);

                var temp = new SingleViewEventModel
                {
                    EventId = data.EventId,
                    ImageUrl = getUrl.FileUrl,
                    Title = data.Title,
                    Description = data.Description,
                    Location = data.location,
                    StartDate = data.startDate,
                    EndDate = data.endDate,
                    StartTime = data.start,
                    EndTime = data.end,
                    IsJoin = data.isJoin,
                    IsSave = data.isSave,
                };

                getList.Add(temp);
            }

            //v 2.0
            //var getList = await query
            //    .Select(Q => new SingleViewEventModel
            //    {
            //        EventId = Q.EventId,
            //        ImageUrl = FileServiceMan.GetFileDetailAsync(Q.BlobId).Result.FileUrl,
            //        Title = Q.Title,
            //        Description = Q.Description,
            //        Location = Q.location,
            //        StartDate = Q.startDate,
            //        EndDate = Q.endDate,
            //        StartTime = Q.start,
            //        EndTime = Q.end,
            //        IsJoin = Q.isJoin,
            //        IsSave = Q.isSave,
            //    })
            //    .AsNoTracking()
            //    .Skip((filter.Page - 1) * filter.ItemSize)
            //    .Take(filter.ItemSize)
            //    .ToListAsync();

            return getList;
        }

        /// <summary>
        /// Get List User Who can join to the selected event.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<DropdownWithImage>> GetInviteUserListAsync(string user, EventFilterModel filter, int eventId)
        {
            var employeeId = "";
            try
            {
                employeeId = user;
            }
            catch
            {
                // return null;
            }

            if (filter.Page <= 0)
            {
                filter.Page = 1;
            }

            if (filter.ItemSize <= 0)
            {
                filter.ItemSize = 10;
            }

            if (string.IsNullOrEmpty(filter.Name))
            {
                filter.Name = "";
            }
            else
            {
                filter.Name = filter.Name.ToLower();
            }

            var getOutlet = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.OutletId).FirstOrDefaultAsync();
            var getArea = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.Outlet.AreaId).FirstOrDefaultAsync();

            //string filter.Name buat filter.Name nama
            var query = DB.Employees
                        .Where(Q => Q.LastSeenAt != null
                        //&& Q.Outlet.AreaId == getArea
                         && Q.OutletId == getOutlet
                         && Q.EmployeeId.ToLower() != employeeId.ToLower()
                        )
                        .Select(Q => new
                        {
                            imageId = Q.BlobId.ToString(),
                            Name = Q.EmployeeName,
                            employeeId = Q.EmployeeId,
                        });
            // var total = await query.CountAsync();

            var alreadyJoin = await DB.EmployeeEventMappings
                           .Where(Q => Q.EventId == eventId && Q.IsJoined == true)
                           .Select(Q => new
                           {
                               employeeId = Q.EmployeeId,
                               isJoin = Q.IsJoined
                           })
                           .AsNoTracking()
                           .ToListAsync();

            query = query.Where(Q => Q.Name.ToLower().Contains(filter.Name.ToLower()));

            query = query.OrderBy(Q => Q.Name);

            var queryList = await query
                .AsNoTracking()
                .Distinct()
                .Skip((filter.Page - 1) * filter.ItemSize)
                .Take(filter.ItemSize)
                .ToListAsync();

            var getUserList = new List<DropdownWithImage>();

            foreach (var data in queryList)
            {
                var url = new FileModel();

                var exist = alreadyJoin.Any(Q => Q.employeeId.ToLower() == data.employeeId.ToLower());

                if (exist == true)
                {
                    continue;
                }

                if (data.imageId != Guid.Empty.ToString() && data.imageId != null)
                {
                    url = await FileServiceMan.GetFileDetailAsync(data.imageId);
                }
                else
                {
                    url = new FileModel
                    {
                        FileUrl = "",
                        Mime = "",
                        Name = ""
                    };
                }

                var temp = new DropdownWithImage
                {
                    Id = data.employeeId,
                    ImageUrl = url.FileUrl,
                    Name = data.Name,
                };

                getUserList.Add(temp);
            }

            return getUserList;
        }

        public async Task<SingleViewEventModel> GetDetailMyEventAsync(int id)
        {
            var query = await DB.EventOutletMappings
                .Where(Q => Q.EventId == id && Q.Event.IsDeleted == false)
                .Select(Q => new
                {
                    dataEvent = Q.Event,
                    location = Q.Outlet.City.CityName,
                    outletId = Q.OutletId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var getData = new SingleViewEventModel
            {
                Title = query.dataEvent.Name,
                Description = query.dataEvent.Description,
                StartDate = query.dataEvent.StartDateTime.Date,
                EndDate = query.dataEvent.EndDateTime.Date,
                StartTime = query.dataEvent.StartDateTime.ToString("HH:mm"),
                EndTime = query.dataEvent.EndDateTime.ToString("HH:mm"),
                Location = query.location
            };

            var getBlobId = query.dataEvent.BlobId.Value.ToString();

            var getUrl = new FileModel();

            if (getBlobId != Guid.Empty.ToString() && getBlobId != null)
            {
                getUrl = await FileServiceMan.GetFileDetailAsync(getBlobId);
            }
            else
            {
                getUrl = new FileModel
                {
                    FileUrl = "",
                    Mime = "",
                    Name = ""
                };
            }

            // var getUrl = await FileServiceMan.GetFileDetailAsync(getBlobId);

            getData.ImageUrl = getUrl.FileUrl;

            return getData;
        }

        public async Task<MyEventFormModel> GetMyEventByIdAsync(int id)
        {

            var query = await DB.EventOutletMappings
                .Where(Q => Q.EventId == id && Q.Event.IsDeleted == false)
                .Select(Q => new
                {
                    dataEvent = Q.Event,
                    location = Q.Outlet.City.CityName,
                    outletId = Q.OutletId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var getData = new MyEventFormModel
            {
                Name = query.dataEvent.Name,
                Description = query.dataEvent.Description,
                StartDate = query.dataEvent.StartDateTime.Date,
                EndDate = query.dataEvent.EndDateTime.Date,
                StartTimeString = query.dataEvent.StartDateTime.ToString("HH:mm"),
                EndTimeString = query.dataEvent.EndDateTime.ToString("HH:mm"),
                Location = query.location,
                OutletId = query.outletId,
                EventId = query.dataEvent.EventId,
                Host = query.dataEvent.HostName,
                CategoryId = query.dataEvent.EventCategoryId.GetValueOrDefault(),
            };

            return getData;

        }

        /// <summary>
        /// Insertevent from mobile app.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> InsertMyEventAsync(string user, MyEventFormModel model)
        {
            TimeSpan startTime = TimeSpan.Parse(model.StartTimeString + ":00");

            TimeSpan endTime = TimeSpan.Parse(model.EndTimeString + ":00");

            var tempStartDate = model.StartDate.UtcDateTime.ToIndonesiaTimeZone();
            var tempEndDate = model.EndDate.UtcDateTime.ToIndonesiaTimeZone();

            var startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day, 0, 0, 0).Add(startTime);
            var endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day, 0, 0, 0).Add(endTime);

            var data = new Events
            {
                Name = model.Name,
                Description = model.Description,
                EventCategoryId = model.CategoryId,
                StartDateTime = startDate,
                EndDateTime = endDate,
                Location = model.Location,
                HostName = model.Host,

                IsDeleted = false,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = user,
                UpdatedBy = user,
                TotalView = 0,
                Source = EventSourceEnum.Mobile
            };

            DB.Events.Add(data);

            var getEmployeeInformation = await (from e in DB.Employees
                                                join o in DB.Outlets on e.OutletId equals o.OutletId
                                                where e.EmployeeId == user
                                                select new
                                                {
                                                    outletId = e.OutletId,
                                                    dealerPeopleCategoryCode = e.DealerPeopleCategoryCode,
                                                    dealerId = o.DealerId
                                                }).FirstOrDefaultAsync();

            if (getEmployeeInformation.dealerPeopleCategoryCode == "HO")
            {
                var eventOutletMapping = await DB.Outlets.Where(Q => Q.DealerId == getEmployeeInformation.dealerId).Select(Q => new EventOutletMappings()
                {
                    EventId = data.EventId,
                    OutletId = Q.OutletId
                }).ToListAsync();

                DB.EventOutletMappings.AddRange(eventOutletMapping);
            }
            else
            {
                var outletMap = new EventOutletMappings
                {
                    EventId = data.EventId,
                    OutletId = getEmployeeInformation.outletId,
                };

                DB.EventOutletMappings.Add(outletMap);
            }

            //get approval mapping
            var approvalMap = await DB.ApprovalMappings
                .Where(Q => Q.Page.Name == PageEnums.Event)
                .Select(Q => new { Q.ApprovalLevel, Q.ApprovalMappingid })
                .FirstOrDefaultAsync();

            // get approve status id
            var approveStatusId = await DB.ApprovalStatus
                .Where(Q => Q.ApprovalName == ApprovalStatusesEnum.Approve)
                .Select(Q => Q.ApprovalStatusId)
                .FirstOrDefaultAsync();

            var approval = new Approvals();

            if (approvalMap == null)
            {
                approval = new Approvals
                {
                    ApprovalLevel = 0,
                    ApprovalStatusId = approveStatusId,
                    ContentName = model.Name,
                    ContentCategory = ContentCategoryEnums.Event,

                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = user,
                    ActionBy = user,
                };
            }
            else
            {
                approval = new Approvals
                {
                    ApprovalLevel = 0,
                    ApprovalMappingId = approvalMap.ApprovalMappingid,
                    ApprovalStatusId = approveStatusId,
                    ContentName = model.Name,
                    ContentCategory = ContentCategoryEnums.Event,

                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = user,
                    ActionBy = user,
                };
            }

            DB.Approvals.Add(approval);

            var approvalEvent = new ApprovalToEvents
            {
                ApprovalId = approval.ApprovalId,
                EventId = data.EventId,
            };

            DB.ApprovalToEvents.Add(approvalEvent);

            await DB.SaveChangesAsync();

            return ResponseMessageEnum.SuccessAddSave;
        }

        public async Task<string> MyEventAttendAsync(string user, EventAttendModel model)
        {

            var employeeId = "";
            var returnMessage = "";

            try
            {
                employeeId = user;
            }
            catch
            {
                return ResponseMessageEnum.FailedBaseString + "can't get login data";
            }

            var getData = await DB.EmployeeEventMappings.Where(Q => Q.EventId == model.EventId && Q.EmployeeId == employeeId).FirstOrDefaultAsync();

            if (getData == null)
            {
                var newMap = new EmployeeEventMappings();
                if (model.Type.ToLower() == "join")
                {
                    newMap = new EmployeeEventMappings
                    {
                        EmployeeId = employeeId,
                        EventId = model.EventId,
                        IsSaved = false,
                        IsJoined = model.Value,
                        JoinedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        SavedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                    };
                    if (model.Value == true)
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "join event";
                    }
                    else
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "left event";
                    }
                }
                else
                if (model.Type.ToLower() == "save")
                {
                    newMap = new EmployeeEventMappings
                    {
                        EmployeeId = employeeId,
                        EventId = model.EventId,
                        IsSaved = model.Value,
                        IsJoined = false,
                        SavedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                    };
                    if (model.Value == true)
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "save event";
                    }
                    else
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "unsave event";
                    }
                }
                DB.EmployeeEventMappings.Add(newMap);
            }
            else
            {
                if (model.Type.ToLower() == "join")
                {
                    getData.IsJoined = model.Value;
                    getData.JoinedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    getData.SavedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                    if (model.Value == true)
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "join event";
                    }
                    else
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "left event";
                    }
                }
                else
                if (model.Type.ToLower() == "save")
                {
                    getData.IsSaved = model.Value;
                    getData.SavedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                    if (model.Value == true)
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "save event";
                    }
                    else
                    {
                        returnMessage = ResponseMessageEnum.SuccessBaseString + "unsave event";
                    }
                }
                DB.EmployeeEventMappings.Update(getData);
            }

            await DB.SaveChangesAsync();
            try
            {

            }
            catch
            {

            }
            return returnMessage;
        }

        public async Task<string> JoinInvitedEventAsync(int eventId, List<string> employeeIdList)
        {
            var getEmployee = await DB.EmployeeEventMappings
                        .Where(Q => Q.EventId == eventId
                            && employeeIdList.Contains(Q.EmployeeId)
                        )
                        .Select(Q => new
                        {
                            id = Q.EmployeeId,
                            isJoin = Q.IsJoined,
                            isSave = Q.IsSaved,
                            data = Q
                        })
                        .AsNoTracking()
                        .ToListAsync();

            var eemList = new List<EmployeeEventMappings>();

            foreach (var data in employeeIdList)
            {
                var exist = getEmployee.Where(Q => Q.id.ToLower() == data.ToLower()).FirstOrDefault();

                var newMap = new EmployeeEventMappings();
                if (exist == null)
                {
                    newMap = new EmployeeEventMappings
                    {
                        EmployeeId = data,
                        EventId = eventId,
                        IsSaved = false,
                        IsJoined = true,
                        SavedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                    };
                    eemList.Add(newMap);

                }
                else if (exist.isJoin != true)
                {
                    var temp = exist.data;
                    temp.IsJoined = true;
                    temp.SavedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                    DB.EmployeeEventMappings.Update(temp);
                }
                //already join the event
                else
                {
                    continue;
                }
            }

            DB.AddRange(eemList);

            //save DB
            await DB.SaveChangesAsync();

            return ResponseMessageEnum.SuccessBaseString + "add invited employee into event";
        }

        public async Task<List<DropDownModel>> GetEventCategoryAsync()
        {
            var getList = await DB.EventCategories
                        .Select(Q => new DropDownModel
                        {
                            Id = Q.EventCategoryId,
                            Name = Q.Name
                        })
                        .AsNoTracking()
                        .ToListAsync();

            return getList;
        }

        public async Task<List<DropDownStringModel>> GetOutletListAsync(string user, EventFilterModel filter)
        {
            var employeeId = "";

            try
            {
                employeeId = user;
            }
            catch
            {
                // return null;
            }

            if (filter.Page <= 0)
            {
                filter.Page = 1;
            }

            if (filter.ItemSize <= 0)
            {
                filter.ItemSize = 10;
            }

            if (string.IsNullOrEmpty(filter.Name))
            {
                filter.Name = "";
            }
            else
            {
                filter.Name = filter.Name.ToLower();
            }

            //var getOutlet = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.OutletId).FirstOrDefaultAsync();
            //var getArea = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.Outlet.AreaId).FirstOrDefaultAsync();

            var getList = await DB.Outlets
                    .Where(Q => Q.Name.ToLower().Contains(filter.Name)
                    // && Q.Area.AreaId == getArea
                    )
                    .Select(Q => new DropDownStringModel
                    {
                        Id = Q.OutletId,
                        Name = Q.Name
                    })
                    .OrderBy(Q => Q.Id)
                    .AsNoTracking()
                    .Skip((filter.Page - 1) * filter.ItemSize)
                    .Take(filter.ItemSize)
                    .ToListAsync();

            return getList;

        }

    }
}
