using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class LearningViewModel
    {
        public int? TrainingId { get; set; }
        public int? CourseId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? TrainingBatch { get; set; }
        public string ImageUrl { get; set; }
        public string TrainingName { get; set; }
        public double? Rating { get; set; }
        public string ProgramTypeName { get; set; }
        public bool IsPopular { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LearningFilter
    {
        public string LearningName { get; set; }
        public List<int?> ProgramTypeId { get; set; }
        public List<int?> LearningTypeId { get; set; }
        public List<int?> MaterialTypeId { get; set; }
        public List<int?> CoursePrice { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class TrainingProgramType
    {
        public int ProgramTypeid { get; set; }
        public string ProgramTypeName { get; set; }
    }

    public class TrainingTypeProgramTypeFilter
    {
        public string ProgramTypeName { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class TrainingLearningType
    {
        public int LearningTypeId { get; set; }
        public string LearningTypeName { get; set; }
    }

    public class TrainingLearningTypeFilter
    {
        public string LearningTypeName { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class TrainingPriceType
    {
        public int TrainingPriceId { get; set; }
        public string TrainingPrice { get; set; }
    }

    public class TrainingMaterialType
    {
        public int MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }
    }

    public class TrainingMaterialTypeFilter
    {
        public string MaterialTypeName { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class CoachGridModel
    {
        public List<CoachViewModel> CoachViewModel;
    }

    public class CoachViewModel
    {
        public int CoachId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
        public double? Rating { get; set; }
        public UserSideTotalBadgeModel TotalBadge { get; set; }
    }

    public class CoachFilter
    {
        public string CoachName { get; set; }
        public List<string> AreaId { get; set; }
        public List<string> CityId { get; set; }
        public List<string> DealerId { get; set; }
        public List<string> ProvinceId { get; set; }
        public List<string> OutletId { get; set; }
        public List<int> PositionId { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public List<int> TopicId { get; set; }
        public List<int> EBadgeId { get; set; }
    }

    public class AreaModel
    {
        public string AreaId { get; set; }
        public string AreaName { get; set; }
    }

    public class AreaFilter
    {
        public string AreaName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class CityModel
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
    }

    public class CityFilter
    {
        public string CityName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class DealerModel
    {
        public string DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class DealerFilter
    {
        public string DealerName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class EventCategoryModel
    {
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
    }

    public class EventCategoryFilter
    {
        public string EventCategoryName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class HostModel
    {
        public string HostName { get; set; }
    }

    public class HostFilter
    {
        public string HostName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class OutletModel
    {
        public string OutletId { get; set; }
        public string OutletName { get; set; }
    }

    public class OutletFilter
    {
        public string OutletName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class PositionModel
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
    }

    public class PositionFilter
    {
        public string PositionName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class ProvinceModel
    {
        public int ProvinceId { get; set; }
        public string Provincename { get; set; }
    }

    public class ProvinceFilter
    {
        public string Provincename { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class PeopleViewModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PeopleFilter
    {
        public List<string> AreaId { get; set; }
        public List<string> CityId { get; set; }
        public List<string> DealerId { get; set; }
        public List<string> ProvinceId { get; set; }
        public List<string> OutletId { get; set; }
        public List<int> PositionId { get; set; }
        public string Name { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class NewsViewModel
    {
        public int NewsId { get; set; }
        public string NewsName { get; set; }
        public string NewsUrl { get; set; }
        public string NewsImageUrl { get; set; }
        public string NewsFile { get; set; }
        public string NewsDescription { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class NewsFilter
    {
        public string Name { get; set; }
        public List<int> NewsCategoryIds { get; set; }
        public List<int> NewsInternalExternalTypeIds { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class NewsAuthor
    {
        public string AuthorName { get; set; } //mungkin nama author ini dari EmployeeName
    }

    public class NewsAuthorFilter
    {
        public string AuthorName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class EventModel
    {
        public int EventId { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class EventFilter
    {
        public string Name { get; set; }
        public List<string> AreaId { get; set; }
        public List<string> CityId { get; set; }
        public List<string> ProvinceId { get; set; }
        public List<string> DealerId { get; set; }
        public List<string> OutletId { get; set; }
        public List<string> PositionId { get; set; }
        public List<int?> EventCategoryId { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class NewsType
    {
        public int NewsTypeId { get; set; }
        public string NewsTypeName { get; set; }
    }

    public class NewsTypeFilter
    {
        public string NewsTitle { get; set; }
        public string NewsTypeName { get; set; }
        public string AuthorName { get; set; }
        public string NewsCategoryName { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class NewsCategory
    {
        public int NewsCategoryId { get; set; }
        public string NewsCategoryName { get; set; }
    }

    public class NewsCategoryFilter
    {
        public string NewsCategoryName { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class InsightModel
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public DateTime? SurveyDueDate { get; set; }
    }

    public class InsightFilter
    {
        public string SurveyName { get; set; }
        public List<string> AreaId { get; set; }
        public List<string> CityId { get; set; }
        public List<string> DealerId { get; set; }
        public List<string> ProvinceId { get; set; }
        public List<string> OutletId { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
