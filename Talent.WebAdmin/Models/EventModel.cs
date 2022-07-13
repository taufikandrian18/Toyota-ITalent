using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class EventModel
    {
        public int EventId { get; set; }
        public int EventCategoryId { get; set; }
        public Guid? BlobId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string EventHostName { get; set; }
        public string Location { get; set; }
        public string EventDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class EventViewModel
    {
        public List<EventModel> ListEventModel { get; set; }
        public int TotalItem { get; set; }
    }

    public class EventFormModel
    {
        public int EventId { get; set; }
        public int EventCategoryId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(64)]
        public string EventName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [Required]
        [StringLength(64)]
        public string EventHostName { get; set; }
        [Required]
        [StringLength(255)]
        public string Location { get; set; }
        [StringLength(1024)]
        public string EventDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int ApprovalId { get; set; }

        public List<EventOutletModel> ListEventOutlets { get; set; }
        public List<EventPositionModel> ListEventPositions { get; set; }
        public FileContent ImageUpload { get; set; }

    }

    public class EventPositionModel
    {
        public int EventId { get; set; }
        public int PositionId { get; set; }
    }

    public class EventOutletModel
    {
        public int EventId { get; set; }
        public string OutletId { get; set; }
    }

    public class EventFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string EventName { get; set; }
        public string EventCategoryName { get; set; }
        public string EventHostName { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTimeOffset? EventFrom { get; set; }
        public DateTimeOffset? EventTo { get; set; }

    }

    public class EventJoinModel
    {
        public int EventId { get; set; }
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
        public Guid? BlobId { get; set; }
        public string FileName { get; set; }
        public string Mime { get; set; }
        public string EventName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string EventHostName { get; set; }
        public string Location { get; set; }
        public string EventDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovalId { get; set; }

        public List<EventOutletModel> ListEventOutlets { get; set; }
        public List<EventPositionModel> ListEventPositions { get; set; }
        public List<string> ListEventAreaIds { get; set; }
        public List<string> ListEventCityIds { get; set; }
        public List<string> ListEventDealerIds { get; set; }
        public List<string> ListEventProvinceIds { get; set; }
        public List<EventRegionModel> ListEventRegionIds { get; set; }
        public FileContent ImageUpload { get; set; }

    }

    public class EventRegionModel
    {
        public int RegionId { get; set; }
    }

    public class EventJoinViewModel
    {
        public List<EventJoinModel> ListEventJoinModel { get; set; }
        public int TotalItem { get; set; }
    }
}
