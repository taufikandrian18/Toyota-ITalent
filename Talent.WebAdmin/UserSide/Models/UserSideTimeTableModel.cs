using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTimeTableModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int ProgramTypeId { get; set; }
        public string ProgramTypeName { get; set; }
        public int TrainingId { get; set; }
        public int Batch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Schedule Status (Reschedule / Cancelled)
        /// </summary>
        public string ScheduleStatus { get; set; }

        public int IsDeleted { get; set; }
        public int? Quota { get; set; }
        public int? CoursePrice { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<UserSidePositionModel> Positions { get; set; }
        public List<UserSideTimeTableOutletModel> Outlets { get; set; }
    }

    public class UserSideTimeTableOutletModel
    {
        public string OutletId { get; set; }
        public string OutletName { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public string DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class UserSideTimeTableFilterModel
    {
        /// <summary>
        /// Page Index (current page).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size (data per-page).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// SorBy Value :
        /// name => sort CourseName ASC.
        /// nameDesc => sort CourseName DESC.
        /// date => sort StartDate ASC.
        /// dateDesc => sort StartDate DESC.
        /// </summary>
        public string SortBy { get; set; }

        public List<string> AreaId { get; set; }
        public List<string> DealerId { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public List<int> PositionId { get; set; }
        public bool IsFree { get; set; }
        public bool IsNotFree { get; set; }
    }

    public class UserSideTimeTableResponseModel
    {
        public List<UserSideTimeTableModel> TimeTables { get; set; }
        public int TotalData { get; set; }
    }

    public class UserSideTimeTableCoursePriceModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? CoursePrice { get; set; }
    }

    public class UserSideTimeTableFilterValueModel
    {
        public List<UserSideAreaModel> Areas { get; set; }
        public List<UserSideDealerModel> Dealers { get; set; }
        public List<UserSidePositionModel> Positions { get; set; }
        public List<int> PriceList { get; set; }
    }
}