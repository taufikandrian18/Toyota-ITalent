using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using static Talent.WebAdmin.Enums.ProductFeatureCategoryEnum;
using static Talent.WebAdmin.Enums.ProductGalleryApprovalStatusesEnum;

namespace Talent.WebAdmin.Services
{
    public class DropdownService
    {
        private readonly TalentContext DB;

        public DropdownService(TalentContext context)
        {
            this.DB = context;
        }

        public async Task<List<DealerDropdownModel>> GetDealerForAssessment()
        {
            var data = await this.DB.Dealers.Select(Q => new DealerDropdownModel
            {
                DealerId = Q.DealerId,
                DealerName = Q.DealerName,
                IsOther = Q.OtherCompany,
            }).ToListAsync();

            return data;
        }

        public async Task<List<OutletDropdownModel>> GetOutletForAssessment(string dealerId)
        {
            var data = await this.DB.Outlets.Where(Q => Q.DealerId == dealerId).Select(Q => new OutletDropdownModel
            {
                OutletId = Q.OutletId,
                OutletName = Q.Name
            }).ToListAsync();

            return data;
        }

        public async Task<List<PositionDropdownModel>> GetPositionForAssessment(string outletId)
        {
            var data = await (from e in DB.Employees
                              join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                              join p in DB.Positions on epm.PositionId equals p.PositionId
                              where e.OutletId == outletId
                              select new PositionDropdownModel
                              {
                                  PositionId = p.PositionId,
                                  PositionName = p.PositionName,
                                  IsTamPeople = p.IsTamPeople,
                                  IsOther = p.IsOtherDealer
                              }).Distinct().ToListAsync();

            return data;
        }

        public async Task<List<EmployeeDropdownModel>> GetEmployeeForAssessment(int positionId, string outletId)
        {
            var data = await (from e in DB.Employees
                              join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                              where epm.PositionId == positionId && e.OutletId == outletId
                              select new EmployeeDropdownModel
                              {
                                  EmployeeId = e.EmployeeId,
                                  EmployeeName = e.EmployeeName
                              }).Distinct().ToListAsync();

            return data;
        }

        public async Task<List<RewardTypeDropdownModel>> GetRewardTypesForRewardAsync()
        {
            var data = await this.DB.RewardTypes.Select(Q => new RewardTypeDropdownModel
            {
                RewardTypeId = Q.RewardTypeId,
                RewardTypeName = Q.Name
            }).ToListAsync();

            return data;
        }

        public async Task<List<ModuleDropdownModel>> GetModulesForRewardAsync()
        {
            var data = await (from sm in DB.SetupModules
                              join m in DB.Modules on sm.ModuleId equals m.ModuleId
                              where sm.CourseId == null
                              select new ModuleDropdownModel
                              {
                                  ModuleId = sm.ModuleId,
                                  ModuleName = m.ModuleName
                              }).ToListAsync();

            return data;
        }

        public async Task<List<CoachDropdownModel>> GetCoachesForRewardAsync()
        {
            var data = await (from c in DB.Coaches
                              join e in DB.Employees on c.EmployeeId equals e.EmployeeId
                              select new CoachDropdownModel
                              {
                                  CoachId = c.CoachId,
                                  CoachName = e.EmployeeName
                              }).ToListAsync();

            return data;
        }

        public async Task<List<EventDropdownModel>> GetEventsForRewardAsync()
        {
            var data = await this.DB.Events.Select(Q => new EventDropdownModel
            {
                EventId = Q.EventId,
                EventName = Q.Name
            }).ToListAsync();

            return data;
        }

        public async Task<List<RewardPointTypeDropdown>> GetRewardPointTypeForRewardAsync()
        {
            var data = await this.DB.RewardPointTypes.Select(Q => new RewardPointTypeDropdown
            {
                RewardPointTypeId = Q.RewardPointTypeId,
                RewardPointTypeName = Q.Name
            }).ToListAsync();

            return data;
        }

        //approval status dropdown
        public List<string> GetApprovalStatusList(){

            var getList = new List<string>{
                ApprovalStatusesEnum.Approve,
                ApprovalStatusesEnum.Rejected,
                ApprovalStatusesEnum.Waiting,
                ApprovalStatusesEnum.Draft
            };

            return getList;
        }

        public List<string> GetContentCategoryList(){

            var getList = new List<string>{
                ContentCategoryEnums.Banner,
                ContentCategoryEnums.Course,
                ContentCategoryEnums.Event,
                ContentCategoryEnums.Guide,
                ContentCategoryEnums.Module,
                ContentCategoryEnums.SetupCourse,
                ContentCategoryEnums.SetupModule,
                ContentCategoryEnums.SetupPopQuiz,
                ContentCategoryEnums.Task,
                // ContentCategoryEnums.Training,
                ContentCategoryEnums.News,
                ContentCategoryEnums.ReleaseTraining,
                ContentCategoryEnums.Survey,
            }
            .OrderBy(Q => Q)
            .ToList();

            // var getList = DB.Pages.wh

            return getList;
        }

        /// <summary>
        /// Position Dropdown for TAM people
        /// </summary>
        /// <returns></returns>
        public async Task<List<PositionDropdownModel>> GetTamPosition()
        {
            var data = await this
                .DB
                .Positions
                .Where(Q => Q.IsTamPeople == true && Q.PositionName != "*")
                .Select(Q => new PositionDropdownModel
            {
                PositionId = Q.PositionId,
                PositionName = Q.PositionName
            }).OrderBy(Q => Q.PositionName).ToListAsync();

            return data;
        }

        /// <summary>
        /// Position Dropdown for Dealer people
        /// </summary>
        /// <returns></returns>
        public async Task<List<PositionDropdownModel>> GetDealerPosition()
        {
            var data = await this
                .DB
                .Positions
                .Where(Q => Q.IsTamPeople == false && Q.PositionName != "*")
                .Select(Q => new PositionDropdownModel
                {
                    PositionId = Q.PositionId,
                    PositionName = Q.PositionName
                }).OrderBy(Q => Q.PositionName).ToListAsync();

            return data;
        }

        /// <summary>
        /// User Role Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserRoleDropdownModel>> GetUserRole()
        {
            var data = await this
                .DB
                .Roles
                .Select(Q => new UserRoleDropdownModel
                {
                UserRoleId = Q.RoleId,
                UserRoleName = Q.Name
            }).ToListAsync();

            return data;
        }

        /// <summary>
        /// User Role Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDropdownModel>> GetCategory()
        {
            var data = await this
                .DB
                .DealerPeopleCategories
                .Select(Q => new CategoryDropdownModel
                {
                    CategoryId = Q.DealerPeopleCategoryCode,
                    CategoryName = Q.Name
                }).OrderBy(Q => Q.CategoryName).ToListAsync();

            return data;
        }

        public async Task<List<MaterialTypeDropdownModel>> GetMaterialTypeForModuleAsync()
        {
            var data = await this.DB.MaterialTypes.Select(Q => new MaterialTypeDropdownModel
            {
                MaterialTypeId = Q.MaterialTypeId,
                MaterialTypeName = Q.MaterialTypeName
            }).ToListAsync();

            return data;
        }

        public async Task<List<TopicDropdownModel>> GetTopicForModuleAsync()
        {
            var data = await this.DB.Topics.Select(Q => new TopicDropdownModel
            {
                TopicId = Q.TopicId,
                TopicName = Q.TopicName
            }).ToListAsync();

            return data;
        }

        public async Task<List<ApprovalStatusDropdownModel>> GetApprovalStatusForModuleAsync()
        {
            var data = await this.DB.ApprovalStatus.Select(Q => new ApprovalStatusDropdownModel
            {
                StatusId = Q.ApprovalStatusId,
                StatusName = Q.ApprovalName
            })
            .AsNoTracking()
            .ToListAsync();

            return data;
        }
        public async Task<List<DropDownModel>> GetMasterNewsList()
        {
            var data = await this.DB.NewsCategories.Select(Q => new DropDownModel
            {
                Id = Q.NewsCategoryId,
                Name = Q.Name
            })
            .AsNoTracking()
            .Distinct()
            .ToListAsync();

            return data;
        }

        public List<string> GetCompetencyPriorityList()
        {

            var data = new List<string>{
                CompetencyPriorityEnum.MustHave,
                CompetencyPriorityEnum.ShouldHave,
                CompetencyPriorityEnum.NiceToHave,
            };

            return data;
        }

        public List<string> GetProductCategoryList()
        {
            var data = new List<string>
            {
                ProductCategoryNameEnum.AllCategories,
                ProductCategoryNameEnum.SuvCategories,
                ProductCategoryNameEnum.SedanCategories,
                ProductCategoryNameEnum.MpvCategories,
                ProductCategoryNameEnum.HatchbackCategories,
                ProductCategoryNameEnum.CommercialCategories,
            };

            return data;
        }
        public List<DropDownModel> GetProductGalleryApprovalList()
        {
            var enumData = (from ProductGalleryApprovals e in Enum.GetValues(typeof(ProductGalleryApprovals))
                            select new DropDownModel
                            {
                                Id = (int)e,
                                Name = e.ToString()
                            }).ToList();
            return enumData;
        }
        public List<DropDownModel> GetProductSegmentList()
        {
            var enumData = (from ProductSegments e in Enum.GetValues(typeof(ProductSegments))
                           select new DropDownModel
                           {
                               Id = (int)e,
                               Name = e.ToString()
                           }).ToList();

            return enumData;
        }

        public List<decimal> GetCompetencyProficiencyList()
        {

            var data = new List<decimal>{
                1,2,3,4,5
            };

            return data;
        }

        public async Task<List<DropDownModel>> GetCompetencyListAsync()
        {
            var data = await this.DB.Competencies
            .Select(Q =>new DropDownModel{
                Id = Q.CompetencyId,
                Name = Q.CompetencyName
            })
            .AsNoTracking()
            .ToListAsync();

            return data;
        }

        public async Task<List<DropDownModel>> GetPositionListAsync()
        {
            var data = await this.DB.Positions
            .Where(Q => Q.PositionName != "*")
            .Select(Q =>new DropDownModel{
                Id = Q.PositionId,
                Name = Q.PositionName
            })
            .AsNoTracking()
            .ToListAsync();

            return data;
        }
    }
}