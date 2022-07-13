using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model for store employee data query with rank and position mapping
    /// </summary>
    public class UserSideQueryRankModel
    {
        public string EmployeeId { set; get; }
        public string EmployeeName { set; get; }
        public Guid? BlobId { set; get; }
        public int LearningPoint { set; get; }
        public int TeachingPoint { set; get; }
        public int EmployeeExperience { set; get; }
        public string Mime { set; get; }
        public string OutletName { set; get; }
        public string OutletId { set; get; }
        public string DealerName { set; get; }
        public string DealerId { set; get; }
        public string AreaName { set; get; }
        public string AreaId { set; get; }
    }
}
