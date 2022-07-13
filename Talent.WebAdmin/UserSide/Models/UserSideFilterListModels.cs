using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Talent.WebAdmin.UserSide.Models.UserSideProductModel;

namespace Talent.WebAdmin.UserSide.Models
{

    public class UserSideOutletFilterListModel
    {
        public string FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSidePositionFilterListModel
    {
        public string FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideAreaFilterListModel
    {
        public string FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideDealerFilterListModel
    {
        public string FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideRegionFilterListModel
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideProvinceFilterListModel
    {
        public string FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideCityFilterListModel
    {
        public string FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideProgramTypeFilterListModel
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideLearningTypeFilterListModel
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideMaterialTypeFilterListModel
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSidePricingTypeFilterListModel
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideNewsInternalExternalFilterListModel
    {
        public int FilterId { get; set; }

        public string FilterName { get; set; }
    }

    public class UserSideEventCategoryFilterListModel
    {
        public int FilterId { get; set; }

        public string FilterName { get; set; }
    }

    public class UserSideEBadgesFilterListModel
    {
        public int FilterId { get; set; }

        public string FilterName { get; set; }
    }

    public class UserSideTopicFilterListModel
    {
        public int FilterId { get; set; }

        public string FilterName { get; set; }
    }

    public class UserSideProductCatListModel
    {
        public Guid FilterId { get; set; }

        public string FilterName { get; set; }
    }

    public class UserSideNewsCategoriesFilterListModel
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
    }

    public class UserSideMyTeamFilterListModel
    {
        public List<UserSideOutletFilterListModel> Outlets { get; set; }
        public List<UserSidePositionFilterListModel> Positions { get; set; }
    }

    public class UserSideGeneralFilterListModel
    {
        public List<UserSideOutletFilterListModel> Outlets { get; set; }
        public List<UserSideAreaFilterListModel> Areas { get; set; }
        public List<UserSideDealerFilterListModel> Dealers { get; set; }
        public List<UserSideProvinceFilterListModel> Provinces { get; set; }
        public List<UserSideCityFilterListModel> Cities { get; set; }
        public List<UserSidePositionFilterListModel> Position { get; set; }
    }

    public class UserSideMyEventFilterListModel
    {
        public List<UserSideEventCategoryFilterListModel> EventCategory { get; set; }
    }

    public class UserSideMyCoachFilterListModel
    {
        public List<UserSideEBadgesFilterListModel> Ebadges { get; set; }
        public List<UserSideTopicFilterListModel> Topics { get; set; }
    }

    public class UserSideMyNewsFilterListModel
    {
        public List<UserSideNewsInternalExternalFilterListModel> InternalExternal { get; set; }
        public List<UserSideNewsCategoriesFilterListModel> NewsCategories { get; set; }
    }

    public class UserSideMyLearningFilterListModel
    {
        public List<UserSideProgramTypeFilterListModel> ProgramTypes { get; set; }
        public List<UserSideLearningTypeFilterListModel> LearningTypes { get; set; }
        public List<UserSideMaterialTypeFilterListModel> MaterialTypes { get; set; }
        public List<UserSidePricingTypeFilterListModel> Pricing { get; set; }
    }

    public class UserSideGeneralFilterPageModel
    {
        public int OutletPageSize { get; set; }
        public int AreaPageSize { get; set; }
        public int DealerPageSize { get; set; }
        public int ProvincePageSize { get; set; }
        public int CityPageSize { get; set; }
        public int PositionPageSize { get; set; }
    }

    public class UserSideMyCoachFilterPageModel
    {
        public int TopicPageSize { get; set; }
        public int TopicPageIndex { get; set; }
    }

    public class UserSideMyRankFilterListModel
    {
        public List<UserSideAreaFilterListModel> Area { get; set; }
        public List<UserSidePositionFilterListModel> Position { get; set; }
        public List<UserSideDealerFilterListModel> Dealer { get; set; }
    }

    public class UserSideMyRankFilterModel
    {
        public int? AreaPageSize { get; set; }
        public int? PositionPageSize { get; set; }
        public int? DealerPageSize { get; set; }
        public string AreaKeyword { get; set; }
        public string PostionKeyword { get; set; }
        public string DealerKeyword { get; set; }
    }
}
