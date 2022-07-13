using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideMyEventModel
    {
    }


    //event detail dan single view dibikin sama.....
    public class MyEventDetailModel
    {
        //image url
        public string ImageUrl { get; set; }
        
        //info
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        //tanggal dan jam
        public DateTime StartDate {get; set;}
        public DateTime EndDate {get; set;}
        public string StartTime { get; set; }
        public string EndTime { get; set; }

    }

    public class SingleViewEventModel
    {
        public int EventId { get; set; }
        public string ImageUrl { get; set; }
        //info
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        //date
        public DateTime StartDate {get; set;}
        public DateTime EndDate {get; set;}
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsJoin {get; set;}
        public bool IsSave {get; set;}
    }

    public class MainPageModel{

        public List<SingleViewEventModel> SavedEventList { get; set; }
        public List<SingleViewEventModel> RecomendEventList { get; set; }
        public List<SingleViewEventModel> PopularEventList { get; set; }

    }

    public class DropdownWithImage{

        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

    }

    public class MyEventFormModel
    {
        public int EventId {get; set;}
        public string Name {get; set;}
        public int CategoryId {get; set;}
        public string Location {get; set;}
        public string OutletId {get; set;}
        public string Description {get; set;}
        public DateTimeOffset StartDate {get; set;}
        public DateTimeOffset EndDate {get; set;}
        public string StartTimeString {get; set;}
        public string EndTimeString {get; set;}
        public string Host {get; set;}
        
    }

    public class EventFilterModel
    {
        public int Page {get; set;}
        public int ItemSize {get; set;}
        public string Name {get; set;}
    }
    public class EventAttendModel
    {
        public int EventId {get; set;}
        public string Type {get; set;}
        public bool Value {get; set;}
    }
}
