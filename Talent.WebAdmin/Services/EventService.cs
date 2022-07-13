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
    public class EventService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private readonly FileService FileServiceMan;
        private readonly ApprovalService ApprovalMan;
        public EventService(TalentContext db, ClaimHelper claim, FileService fs, ApprovalService approvalService)
        {
            this.DB = db;
            this.ClaimMan = claim;
            this.FileServiceMan = fs;
            this.ApprovalMan = approvalService;
        }

        public async Task<EventViewModel> GetAllEvents()
        {

            var allEvents = await this.DB.Events.Select(Q => new EventModel
            {
                EventId = Q.EventId,
                EventCategoryId = Q.EventCategoryId.GetValueOrDefault(),
                BlobId = Q.BlobId,
                EventName = Q.Name,
                StartDateTime = Q.StartDateTime,
                EndDateTime = Q.EndDateTime,
                EventHostName = Q.HostName,
                EventDescription = Q.Description,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
                ApprovedAt = Q.ApprovedAt,
                IsDeleted = Q.IsDeleted,
            }).Where(Q => Q.IsDeleted == false).ToListAsync();

            var totalItem = await this.DB.Events.CountAsync();

            var result = new EventViewModel
            {
                ListEventModel = allEvents,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<EventModel> GetEventById(int id)
        {
            var result = await DB.Events.AsNoTracking().Select(Q => new EventModel
            {
                EventId = Q.EventId,
                EventCategoryId = Q.EventCategoryId.GetValueOrDefault(),
                BlobId = Q.BlobId,
                EventName = Q.Name,
                StartDateTime = Q.StartDateTime,
                EndDateTime = Q.EndDateTime,
                EventHostName = Q.HostName,
                EventDescription = Q.Description,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
                ApprovedAt = Q.ApprovedAt,
                IsDeleted = Q.IsDeleted,
            }).Where(Q => Q.IsDeleted == false && Q.EventId == id).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new EventModel();
            }

            return result;
        }

        public async Task<int> CreateEvent(EventFormModel model)
        {

            Events eventModel = new Events
            {
                EventCategoryId = model.EventCategoryId,
                //BlobId = model.BlobId,
                Name = model.EventName,
                StartDateTime = model.StartDateTime.ToIndonesiaTimeZone(),
                EndDateTime = model.EndDateTime.ToIndonesiaTimeZone(),
                HostName = model.EventHostName,
                Description = model.EventDescription,
                Location = model.Location,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
                ApprovedAt = null,
                IsDeleted = false,
                Source = EventSourceEnum.WebAdmin
            };

            if (model.ImageUpload != null)
            {
                if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);
                    eventModel.BlobId = blobId;
                }
            }
            this.DB.Events.Add(eventModel);

            var newApproval = new ApprovalCreateFormModel
            {
                ContentName = model.EventName,
                ContentCategory = ContentCategoryEnums.Event,
                ApprovalStatusId = model.ApprovalId,
                ContentId = eventModel.EventId,
                PageEnum = PageEnums.Event,
            };

            var approval = await ApprovalMan.CreateNewApproval(newApproval);

            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                eventModel.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }

            var eventPosition = new List<EventPositionMappings>();
            foreach (var position in model.ListEventPositions)
            {
                eventPosition.Add(new EventPositionMappings
                {
                    EventId = eventModel.EventId,
                    PositionId = position.PositionId
                });
            }
            this.DB.EventPositionMappings.AddRange(eventPosition);

            var eventOutlet = new List<EventOutletMappings>();
            foreach (var outlet in model.ListEventOutlets)
            {
                eventOutlet.Add(new EventOutletMappings
                {
                    EventId = eventModel.EventId,
                    OutletId = outlet.OutletId
                });
            }
            this.DB.EventOutletMappings.AddRange(eventOutlet);
            await this.DB.SaveChangesAsync();
            return eventModel.EventId;
        }

        public async Task<bool> UpdateEvent(EventJoinModel model)
        {

            var eventToUpdateModel = await this.DB.Events.FindAsync(model.EventId);

            if (eventToUpdateModel == null)
            {
                return false;
            }

            Blobs blobs = null;

            if (eventToUpdateModel.BlobId != null)
            {
                blobs = await this.DB.Blobs.Where(Q => Q.BlobId == eventToUpdateModel.BlobId).FirstOrDefaultAsync();
            }

            if (model.ImageUpload != null)
            {
                if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);

                    if (blobs != null)
                    {
                        await FileServiceMan.DeleteFileAsync(blobs.BlobId, blobs.Mime);
                    }

                    eventToUpdateModel.BlobId = blobId;
                }
            }

            eventToUpdateModel.EventCategoryId = model.EventCategoryId;
            //updateModel.BlobId = model.BlobId;
            eventToUpdateModel.Name = model.EventName;
            eventToUpdateModel.StartDateTime = model.StartDateTime.ToIndonesiaTimeZone();
            eventToUpdateModel.EndDateTime = model.EndDateTime.ToIndonesiaTimeZone();
            eventToUpdateModel.HostName = model.EventHostName;
            eventToUpdateModel.Description = model.EventDescription;
            eventToUpdateModel.Location = model.Location;
            eventToUpdateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            eventToUpdateModel.UpdatedBy = ClaimMan.GetLoginUserId();


            var positionToDelete = await DB.EventPositionMappings.Where(Q => Q.EventId == model.EventId).ToListAsync();
            this.DB.EventPositionMappings.RemoveRange(positionToDelete);
            var eventPosition = new List<EventPositionMappings>();
            foreach (var position in model.ListEventPositions)
            {
                eventPosition.Add(new EventPositionMappings
                {
                    EventId = model.EventId,
                    PositionId = position.PositionId
                });
            }
            this.DB.EventPositionMappings.AddRange(eventPosition);

            var outletToDelete = await DB.EventOutletMappings.Where(Q => Q.EventId == model.EventId).ToListAsync();
            this.DB.EventOutletMappings.RemoveRange(outletToDelete);
            var eventOutlet = new List<EventOutletMappings>();
            foreach (var outlet in model.ListEventOutlets)
            {
                eventOutlet.Add(new EventOutletMappings
                {
                    EventId = model.EventId,
                    OutletId = outlet.OutletId
                });
            }
            this.DB.EventOutletMappings.AddRange(eventOutlet);
            var currentApproval = await DB.ApprovalToEvents.Where(Q => Q.EventId == model.EventId).FirstOrDefaultAsync();

            var newApproval = new ApprovalUpdateFormModel
            {
                ApprovalId = currentApproval.ApprovalId,
                PageEnum = PageEnums.Event,
                ApprovalStatusId = model.ApprovalId,
                ContentName = model.EventName,
                ContentCategory = ContentCategoryEnums.Event
            };
            var approval = await ApprovalMan.UpdateNewApproval(newApproval);
            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId) // submit
            {
                eventToUpdateModel.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else
            {
                eventToUpdateModel.ApprovedAt = null;
            }
            await this.DB.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteEvent(int id)
        {

            var deleteModel = await this.DB.Events.FindAsync(id);

            if (deleteModel == null) // deleteModel.IsDeleted == true
            {
                return false;
            }

            deleteModel.IsDeleted = true;
            deleteModel.EventCategoryId = null;

            var approvalToEvents = await this.DB.ApprovalToEvents.Where(Q => Q.EventId == id).FirstOrDefaultAsync();

            if (approvalToEvents == null)
            {
                return false;
            }

            this.DB.ApprovalToEvents.Remove(approvalToEvents);
            var isDeleted = await this.ApprovalMan.DeleteApproval(approvalToEvents.ApprovalId);

            if(isDeleted == false)
            {
                return false;
            }

            await DB.SaveChangesAsync();

            return true;

        }
    }

    public class EventJoinService
    {
        private readonly TalentContext DB;

        public EventJoinService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<EventJoinViewModel> GetAllEvents(EventFilter filter)
        {
            var query = (
                from e in DB.Events.AsNoTracking()
                join ec in DB.EventCategories.AsNoTracking() on e.EventCategoryId equals ec.EventCategoryId
                join b in DB.Blobs.AsNoTracking() on e.BlobId equals b.BlobId into lb
                from b in lb.DefaultIfEmpty()
                join ate in DB.ApprovalToEvents on e.EventId equals ate.EventId into late
                from ate in late.DefaultIfEmpty()
                join a in DB.Approvals on ate.ApprovalId equals a.ApprovalId into la
                from a in la.DefaultIfEmpty()
                join s in DB.ApprovalStatus on a.ApprovalStatusId equals s.ApprovalStatusId into ls
                from s in ls.DefaultIfEmpty()
                where e.IsDeleted == false && e.Source == EventSourceEnum.WebAdmin
                select new EventJoinModel
                {
                    EventId = e.EventId,
                    EventCategoryId = e.EventCategoryId.GetValueOrDefault(),
                    EventCategoryName = ec.Name,
                    BlobId = e.BlobId,
                    FileName = b.Name,
                    Mime = b.Mime,
                    EventName = e.Name,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    EventHostName = e.HostName,
                    EventDescription = e.Description,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    ApprovedAt = e.ApprovedAt,
                    ApprovalStatus = e.ApprovedAt != null ? ApprovalStatusesEnum.Approve : (s.ApprovalName ?? ApprovalStatusesEnum.Approve),
                    ListEventOutlets = new List<EventOutletModel>(),
                    ListEventPositions = new List<EventPositionModel>()
                });

            if (filter.StartDate != null && filter.EndDate != null)
            {

                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (string.IsNullOrEmpty(filter.EventName) == false)
            {
                query = query.Where(Q => Q.EventName.ToLower().Contains(filter.EventName.ToLower()));
            }

            if (string.IsNullOrEmpty(filter.EventHostName) == false)
            {
                query = query.Where(Q => Q.EventHostName.Contains(filter.EventHostName));
            }

            if (string.IsNullOrEmpty(filter.EventCategoryName) == false)
            {
                query = query.Where(Q => Q.EventCategoryName.Equals(filter.EventCategoryName));
            }

            if (filter.EventFrom != null)
            {

                var newFromDate = filter.EventFrom.Value.UtcDateTime.ToIndonesiaTimeZone();
                var fromDate = new DateTime(newFromDate.Year, newFromDate.Month, newFromDate.Day, 0, 0, 0);

                var newFromDateEnd = filter.EventFrom.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newFromDateEnd.Year, newFromDateEnd.Month, newFromDateEnd.Day, 23, 59, 59);

                query = query.Where(Q => Q.StartDateTime >= fromDate && Q.StartDateTime < endDate);
            }

            if (filter.EventTo != null)
            {
                var newFromDate = filter.EventTo.Value.UtcDateTime.ToIndonesiaTimeZone();
                var fromDate = new DateTime(newFromDate.Year, newFromDate.Month, newFromDate.Day, 0, 0, 0);

                var newFromDateEnd = filter.EventTo.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newFromDateEnd.Year, newFromDateEnd.Month, newFromDateEnd.Day, 23, 59, 59);

                query = query.Where(Q => Q.EndDateTime >= fromDate && Q.EndDateTime <= endDate);
            }

            if (string.IsNullOrEmpty(filter.ApprovalStatus) == false)
            {
                query = query.Where(Q => Q.ApprovalStatus.Equals(filter.ApprovalStatus));
            }

            query = query.OrderByDescending(Q => Q.UpdatedAt);

            switch (filter.SortBy)
            {
                case "eventName":
                    query = query.OrderBy(Q => Q.EventName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "eventNameDesc":
                    query = query.OrderByDescending(Q => Q.EventName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "eventCategory":
                    query = query.OrderBy(Q => Q.EventCategoryName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "eventCategoryDesc":
                    query = query.OrderByDescending(Q => Q.EventCategoryName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "startDate":
                    query = query.OrderBy(Q => Q.StartDateTime).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDateTime).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "endDate":
                    query = query.OrderBy(Q => Q.EndDateTime).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDateTime).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "host":
                    query = query.OrderBy(Q => Q.EventHostName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "hostDesc":
                    query = query.OrderByDescending(Q => Q.EventHostName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "approvalStatus":
                    query = query.OrderBy(Q => Q.ApprovalStatus).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "approvalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var resultQuery = await query.Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();
            var totalData = await query.CountAsync();

            var result = new EventJoinViewModel
            {
                ListEventJoinModel = resultQuery,
                TotalItem = totalData
            };

            return result;
        }

        public async Task<EventJoinModel> GetEventJoinById(int id)
        {
            var result = await (
                from e in DB.Events.AsNoTracking()
                join ec in DB.EventCategories.AsNoTracking() on e.EventCategoryId equals ec.EventCategoryId
                join b in DB.Blobs.AsNoTracking() on e.BlobId equals b.BlobId
                where e.EventId == id
                select new EventJoinModel
                {
                    EventId = e.EventId,
                    EventCategoryId = e.EventCategoryId.GetValueOrDefault(),
                    EventCategoryName = ec.Name,
                    BlobId = e.BlobId,
                    FileName = b.Name,
                    Mime = b.Mime,
                    EventName = e.Name,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    EventHostName = e.HostName,
                    Location = e.Location,
                    EventDescription = e.Description,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    ApprovedAt = e.ApprovedAt,
                }).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new EventJoinModel();
            }

            result.ListEventOutlets = await DB.EventOutletMappings.Where(Q => Q.EventId == id).Select(Q => new EventOutletModel
            {
                EventId = id,
                OutletId = Q.OutletId
            }).ToListAsync();

            result.ListEventPositions = await DB.EventPositionMappings.Where(Q => Q.EventId == id).Select(Q => new EventPositionModel
            {
                EventId = id,
                PositionId = Q.PositionId
            }).ToListAsync();

            result.ListEventAreaIds = await (
                from eo in this.DB.EventOutletMappings
                join o in this.DB.Outlets on eo.OutletId equals o.OutletId
                join a in this.DB.Areas on o.AreaId equals a.AreaId
                where eo.EventId == id
                select a.AreaId
                ).Distinct().AsNoTracking().ToListAsync();

            result.ListEventCityIds = await (
                from eo in this.DB.EventOutletMappings
                join o in this.DB.Outlets on eo.OutletId equals o.OutletId
                join c in this.DB.Cities on o.CityId equals c.CityId
                where eo.EventId == id
                select c.CityId
                ).Distinct().AsNoTracking().ToListAsync();

            result.ListEventProvinceIds = await (
                from eo in this.DB.EventOutletMappings
                join o in this.DB.Outlets on eo.OutletId equals o.OutletId
                join p in this.DB.Provinces on o.ProvinceId equals p.ProvinceId
                where eo.EventId == id
                select p.ProvinceId
                ).Distinct().AsNoTracking().ToListAsync();

            result.ListEventDealerIds = await (
                from eo in this.DB.EventOutletMappings
                join o in this.DB.Outlets on eo.OutletId equals o.OutletId
                join d in this.DB.Dealers on o.DealerId equals d.DealerId
                where eo.EventId == id
                select d.DealerId
                ).Distinct().AsNoTracking().ToListAsync();

            result.ListEventRegionIds = await (
                from eo in DB.EventOutletMappings
                join o in DB.Outlets on eo.OutletId equals o.OutletId
                join r in DB.Regions on o.RegionId equals r.RegionId
                where eo.EventId == id
                select new EventRegionModel
                {
                    RegionId = r.RegionId
                }).Distinct().AsNoTracking().ToListAsync();

            return result;
        }
    }
}
