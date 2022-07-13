using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class EventsModel
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int TotalView { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string BlobId { get; set; }
        public bool? IsJoin { get; set; }
        public bool? IsSave { get; set; }
    }

    public class EventSearchModel
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HostName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Mime { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
        public string BlobId { get; set; }
        public bool IsJoin { get; set; }
        public bool IsSave { get; set; }
        public int? TotalSave { get; set; }
        public int? TotalJoin { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
