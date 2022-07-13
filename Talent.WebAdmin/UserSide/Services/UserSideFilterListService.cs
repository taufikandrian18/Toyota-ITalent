using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideFilterListService
    {
        private readonly TalentContext DB;

        public UserSideFilterListService(TalentContext context)
        {
            this.DB = context;
        }

        public async Task<List<UserSideOutletFilterListModel>> GetUserSideOutletFilterByDealerListAsync(int? pageSize, string keyword, string employeeId)
        {
            var getEmployeeDealerId = await (from e in DB.Employees
                                             join o in DB.Outlets on e.OutletId equals o.OutletId
                                             where e.EmployeeId == employeeId
                                             select new
                                             {
                                                 dealerId = o.DealerId
                                             }).FirstOrDefaultAsync();

            var query = this.DB.Outlets.AsQueryable();

            if (getEmployeeDealerId != null)
            {
                query.Where(Q => Q.DealerId == getEmployeeDealerId.dealerId);
            }

            if (query != null)
            {
                var data = query.Select(Q => new UserSideOutletFilterListModel
                {
                    FilterId = Q.OutletId,
                    FilterName = Q.Name
                });

                if (!String.IsNullOrEmpty(keyword))
                {
                    data = data.Where(Q => Q.FilterName.Contains(keyword));
                }

                if (pageSize != null)
                {
                    data = data.Take(pageSize.GetValueOrDefault());
                }

                var result = await data.ToListAsync();

                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<UserSideOutletFilterListModel>> GetUserSideOutletFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Outlets
                .Select(Q => new UserSideOutletFilterListModel
                {
                    FilterId = Q.OutletId,
                    FilterName = Q.Name
                });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSidePositionFilterListModel>> GetUserSidePositionFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Positions.Where(Q => Q.PositionName != "*").Select(Q => new UserSidePositionFilterListModel
            {
                FilterId = Q.PositionId.ToString(),
                FilterName = Q.PositionName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideAreaFilterListModel>> GetUserSideAreaFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Areas.Select(Q => new UserSideAreaFilterListModel
            {
                FilterId = Q.AreaId,
                FilterName = Q.Name
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideDealerFilterListModel>> GetUserSideDealerFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Dealers.Select(Q => new UserSideDealerFilterListModel
            {
                FilterId = Q.DealerId,
                FilterName = Q.DealerName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideRegionFilterListModel>> GetUserSideRegionFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Regions.Select(Q => new UserSideRegionFilterListModel
            {
                FilterId = Q.RegionId,
                FilterName = Q.RegionName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideProvinceFilterListModel>> GetUserSideProvinceFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Provinces.Select(Q => new UserSideProvinceFilterListModel
            {
                FilterId = Q.ProvinceId,
                FilterName = Q.ProvinceName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideCityFilterListModel>> GetUserSideCityFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.Cities.Select(Q => new UserSideCityFilterListModel
            {
                FilterId = Q.CityId,
                FilterName = Q.CityName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideProgramTypeFilterListModel>> GetUserSideProgramTypesFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.ProgramTypes.Select(Q => new UserSideProgramTypeFilterListModel
            {
                FilterId = Q.ProgramTypeId,
                FilterName = Q.ProgramTypeName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideLearningTypeFilterListModel>> GetUserSideLearningTypeFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.LearningTypes.Select(Q => new UserSideLearningTypeFilterListModel
            {
                FilterId = Q.LearningTypeId,
                FilterName = Q.LearningName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideMaterialTypeFilterListModel>> GetUserSideMaterialTypeFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.MaterialTypes.Select(Q => new UserSideMaterialTypeFilterListModel
            {
                FilterId = Q.MaterialTypeId,
                FilterName = Q.MaterialTypeName
            });

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.FilterName.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<UserSideNewsCategoriesFilterListModel>> GetUserSideNewsCategoriesFilterListAsync()
        {
            var query = this.DB.NewsCategories.Select(Q => new UserSideNewsCategoriesFilterListModel
            {
                FilterId = Q.NewsCategoryId,
                FilterName = Q.Name
            });

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<List<string>> GetUserSideNewsAuthorsFilterListAsync(int? pageSize, string keyword)
        {
            var query = this.DB.News.OrderBy(Q => Q.Author).Select(Q => Q.Author);

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(Q => Q.Contains(keyword));
            }

            if (pageSize != null)
            {
                query = query.Take(pageSize.GetValueOrDefault());
            }

            var data = await query.ToListAsync();

            return data;
        }

        public List<UserSidePricingTypeFilterListModel> GetUserSidePriceFilterList()
        {
            var data = new List<UserSidePricingTypeFilterListModel>();

            data.Add(new UserSidePricingTypeFilterListModel
            {
                FilterId = 0,
                FilterName = "Free"
            });

            data.Add(new UserSidePricingTypeFilterListModel
            {
                FilterId = 1,
                FilterName = "Pay"
            });

            return data;
        }

        public List<UserSideNewsInternalExternalFilterListModel> GetUserSideNewsInternalExternalList()
        {
            var data = new List<UserSideNewsInternalExternalFilterListModel>();

            data.Add(new UserSideNewsInternalExternalFilterListModel
            {
                FilterId = 1,
                FilterName = "Internal"
            });

            data.Add(new UserSideNewsInternalExternalFilterListModel
            {
                FilterId = 0,
                FilterName = "External"
            });

            return data;
        }

        public async Task<List<UserSideEventCategoryFilterListModel>> GetUserSideEventCategoriesList()
        {
            var data = await this.DB.EventCategories.Select(Q => new UserSideEventCategoryFilterListModel
            {
                FilterId = Q.EventCategoryId,
                FilterName = Q.Name
            }).ToListAsync();

            return data;
        }

        public async Task<List<UserSideEBadgesFilterListModel>> GetUserSideEBadgesFilterList()
        {
            var data = await this.DB.Ebadges.Select(Q => new UserSideEBadgesFilterListModel
            {
                FilterId = Q.EbadgeId,
                FilterName = Q.EbadgeName
            }).ToListAsync();

            return data;
        }

        public async Task<List<UserSideTopicFilterListModel>> GetUserSideTopicFilterListModels(int? pageSize, int? pageIndex)
        {
            if (pageSize == null || pageSize == 0)
            {
                pageSize = 50;
            }

            if (pageIndex == null || pageIndex == 0)
            {
                pageIndex = 1;
            }

            var data = await this.DB.Topics.Select(Q => new UserSideTopicFilterListModel
            {
                FilterName = Q.TopicName,
                FilterId = Q.TopicId
            }).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();

            return data;
        }

    }
}
