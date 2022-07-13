using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class CoachSearchQueryModel
    {
        public int CoachId { set; get; }
        public string EmployeeName { set; get; }
        public string EmployeeId { set; get; }
        public string AreaId { set; get; }
        public string OutletId { set; get; }
        public string CityId { set; get; }
        public string DealerId { set; get; }
        public string ProvinceId { set; get; }
        public Guid? BlobId { get; set; }
        public string MIME { set; get; }
        public string TopicId { set; get; }
        public string EbadgeId { get; set; }
        public string PositionId { get; set; }
    }

    public class PositionCoachSearchQueryModel
    {
        public int CoachId { set; get; }
        public string PositionId { get; set; }
    }

    public class TopicCoachSearchQueryModel
    {
        public int CoachId { set; get; }
    }

    public class EbadgeCoachSearchQueryModel
    {
        public int CoachId { set; get; }
    }

    public class EmployeeByCoach{
        public int TrainingId {get;set;}
        public int PositionId {get;set;}
        public string PositionName {get;set;}
        public string EmployeeName {get;set;}
        public string EmployeeId {get;set;}
        public string OutletId {get;set;}
        public string DealerId {get;set;}
        public string OutletName { get; set; }
    }

}