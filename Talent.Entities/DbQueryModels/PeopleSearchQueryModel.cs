using System;

namespace Talent.Entities.DbQueryModels
{
    public class PeopleSearchQueryModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string AreaId { get; set; }
        public string OutletId { get; set; }
        public string CityId { get; set; }
        public string DealerId { get; set; }
        public string ProvinceId { get; set; }
        public Guid? BlobId { get; set; }
        public string MIME { get; set; }
        public string PositionId { get; set; }
    }
}