using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideMyInsightService
    {
        private readonly TalentContext DB;

        public UserSideMyInsightService(TalentContext context)
        {
            this.DB = context;
        }

        public async Task<List<UserSideMyInsightModel>> GetAllSurveys(UserSideMyInsightFilterModel filter, List<int> userPositionId)
        {
            var query = (from s in DB.Surveys
                         join spm in DB.SurveyPositionMappings on s.SurveyId equals spm.SurveyId
                         join som in DB.SurveyOutletMappings on s.SurveyId equals som.SurveyId
                         join o in DB.Outlets on som.OutletId equals o.OutletId
                         where userPositionId.Contains(spm.PositionId) && s.IsDeleted == false
                         select new
                         {
                             s.SurveyId,
                             s.Title,
                             s.EndDate,
                             s.UpdatedAt,
                             o.AreaId,
                             o.DealerId,
                             o.ProvinceId,
                             o.CityId,
                             o.OutletId
                         });

            if (filter.AreaIds.FirstOrDefault() != null)
            {
                query = query.Where(Q => filter.AreaIds.Contains(Q.AreaId));
            }

            if (filter.DealerIds.FirstOrDefault() != null)
            {
                query = query.Where(Q => filter.DealerIds.Contains(Q.DealerId));
            }

            if (filter.ProvinceIds.FirstOrDefault() != null)
            {
                query = query.Where(Q => filter.ProvinceIds.Contains(Q.ProvinceId));
            }

            if (filter.CityIds.FirstOrDefault() != null)
            {
                query = query.Where(Q => filter.CityIds.Contains(Q.CityId));
            }

            if (filter.OutletIds.FirstOrDefault() != null)
            {
                query = query.Where(Q => filter.OutletIds.Contains(Q.OutletId));
            }

            if (!String.IsNullOrEmpty(filter.SurveyTitle))
            {
                query = query.Where(Q => Q.Title.Contains(filter.SurveyTitle));
            }

            if (filter.StartDueDate != null && filter.EndDueDate != null)
            {
                query = query.Where(Q => Q.EndDate >= filter.StartDueDate && Q.EndDate <= filter.EndDueDate);
            }

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.Title);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.Title);
                    break;
                case "date":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "dateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;
                default:
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
            }

            var data = await query.Select(Q => new UserSideMyInsightModel
            {
                SurveyId = Q.SurveyId,
                SurveyTitle = Q.Title,
                SurveyDueDate = Q.EndDate.GetValueOrDefault()
            }).ToListAsync();

            data = data.GroupBy(Q => Q.SurveyId).Select(Q => Q.First()).ToList();

            if (filter.PageIndex != null && filter.PageSize != null)
            {
                var skipCount = Math.Ceiling((decimal)(filter.PageIndex.GetValueOrDefault() - 1) * filter.PageSize.GetValueOrDefault());

                data = data.Skip((int)skipCount).Take(filter.PageSize.GetValueOrDefault()).ToList();
            }

            return data;
        }

        public async Task<List<int>> GetAllSurveyQuestionAsync(int surveyId)
        {
            var data = await (from s in DB.Surveys
                              join sq in DB.SurveyQuestions on s.SurveyId equals sq.SurveyId
                              where s.SurveyId == surveyId && s.IsDeleted == false
                              orderby sq.QuestionNumber ascending
                              select sq.SurveyQuestionId
                              ).ToListAsync();

            return data;
        }
    }
}
