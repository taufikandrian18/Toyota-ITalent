using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Enums;

namespace Talent.WebAdmin.Services
{
    public class DashboardOperationService
    {
        private readonly TalentContext _Db;

        public DashboardOperationService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public async Task<int> GetTotalUsers()
        {
            var getAllTotalUsers = await this._Db.Employees.Where(Q => Q.LastSeenAt != null && Q.IsDeleted == false).CountAsync();

            return getAllTotalUsers;
        }

        public async Task<int> GetTotalActiveUsers()
        {
            //var getCurrentDate = DateTime.UtcNow.ToIndonesiaTimeZone().Date;

            //var lastSeenEmployeeDates = await this._Db.Employees.Where(Q => Q.LastSeenAt != null)
            //    .Select(Q => Q.LastSeenAt.GetValueOrDefault().Date)
            //    .ToListAsync();

            //var getAllTotalActiveUsers = lastSeenEmployeeDates
            //    .Where(Q => Q.Date == getCurrentDate)
            //    .Count();

            var getAllTotalActiveUsers = await this._Db.GetTotalActiveUser()
                .Select(Q => Q.TotalActiveUserCount)
                .FirstOrDefaultAsync();

            return getAllTotalActiveUsers;
        }

        public async Task<int> GetTotalAccessRate()
        {
            var totalUsers = await GetTotalUsers();
            var activeUsers = await GetTotalActiveUsers();
            var totalAccessRate = (activeUsers * 100 / totalUsers);

            return totalAccessRate;
        }

        public async Task<int> GetAverageAccessTime()
        {
            var getLast30Days = DateTime.UtcNow.ToIndonesiaTimeZone().AddDays(-30);
            var getAllUsersLast30Days = await this._Db.EmployeeAccessTimes.Where(Q => Q.EndTime.Value.Date >= getLast30Days.Date).ToListAsync();

            var totalUser = getAllUsersLast30Days.GroupBy(Q => Q.EmployeeId).Count();

            var totalTime = await this._Db.GetAverageAccessTimes()
                .Select(Q => Q.AverageAccessCount)
                .FirstOrDefaultAsync();

            var getAverageAccessTime = 0;
            if (totalUser != 0)
            {
                getAverageAccessTime = (int)totalTime / totalUser;
            }

            return getAverageAccessTime;
        }

        public async Task<int> GetTotalLearningEnrollment()
        {
            var getAllTotalLearningEnrollment = await this._Db.EnrollLearnings.Where(Q => Q.IsEnrolled == true && Q.Employee.IsDeleted == false).CountAsync();

            return getAllTotalLearningEnrollment;
        }

        public async Task<int> GetTotalCertificationRate()
        {
            var totalUser = await GetTotalUsers();
            var completedCertificationRate = await this._Db.EmployeeCertificates.ToListAsync();

            var getRate = completedCertificationRate.Select(Q => Q.EmployeeId).Distinct().Count();

            var certificationRate = (getRate * 100 / totalUser);

            return certificationRate;
        }

        public async Task<DashboardApprovalListViewModel> GetApprovalData()
        {
            DateTime goLive = new DateTime(2020, 06, 30).ToIndonesiaTimeZone();

            var getAllAprovalDatas = await this._Db.Approvals.Where(Q => Q.CreatedAt >= goLive).ToListAsync();

            var getAprovalDatas = getAllAprovalDatas
                .GroupBy(Q => Q.ContentCategory)
                .Select(Q => new DashboardApprovalData
                {
                    ApprovalContent = Q.Key,
                    Completed = Q.Where(X => X.ApprovalStatusId == 1).Count(),
                    Remaining = Q.Where(X => X.ApprovalStatusId == 2).Count()
                }).ToList();

            var approvalDataResult = new DashboardApprovalListViewModel
            {
                ListApproval = getAprovalDatas
            };

            return approvalDataResult;
        }

        private int GetMonthDataTotal(List<DashboardUsersThisYearData> storeDataToList, int month)
        {
            return storeDataToList.Where(Q => Q.Month == month).Distinct().Count();
        }

        public async Task<DashboardUsersThisYearListViewModel> GetTotalUsersThisYear()
        {
            var getCurrentYear = DateTime.UtcNow.ToIndonesiaTimeZone().Year;
            var getAllUsersThisYear = await this._Db.EmployeeAccessTimes.Where(Q => Q.EndTime.Value.Year == getCurrentYear).ToListAsync();

            var storeDataToList = getAllUsersThisYear
                .GroupBy(Q => new { Q.EndTime.Value.Month, Q.EmployeeId })
                .Select(Q => new DashboardUsersThisYearData
                {
                    EmployeeId = Q.Key.EmployeeId,
                    Month = Q.Key.Month
                }).ToList();

            var getTotalUsersResult = new DashboardUsersThisYearListViewModel
            {
                JanuaryUsersData = GetMonthDataTotal(storeDataToList, 1),
                FebruaryUsersData = GetMonthDataTotal(storeDataToList, 2),
                MarchUsersData = GetMonthDataTotal(storeDataToList, 3),
                AprilUsersData = GetMonthDataTotal(storeDataToList, 4),
                MayUsersData = GetMonthDataTotal(storeDataToList, 5),
                JuneUsersData = GetMonthDataTotal(storeDataToList, 6),
                JulyUsersData = GetMonthDataTotal(storeDataToList, 7),
                AugustUsersData = GetMonthDataTotal(storeDataToList, 8),
                SeptemberUsersData = GetMonthDataTotal(storeDataToList, 9),
                OctoberUsersData = GetMonthDataTotal(storeDataToList, 10),
                NovemberUsersData = GetMonthDataTotal(storeDataToList, 11),
                DecemberUsersData = GetMonthDataTotal(storeDataToList, 12),
            };

            return getTotalUsersResult;
        }

        public async Task<DashboardClassListViewModel> GetClassScheduleList()
        {
            var ThisWeekSunday = DateTime.UtcNow.ToIndonesiaTimeZone().AddDays(-(int)DateTime.UtcNow.ToIndonesiaTimeZone().DayOfWeek);
            var ThisWeekSaturday = DateTime.UtcNow.ToIndonesiaTimeZone().AddDays(-(int)DateTime.UtcNow.ToIndonesiaTimeZone().DayOfWeek + (int)DayOfWeek.Saturday);
            var NextWeekSunday = DateTime.UtcNow.ToIndonesiaTimeZone().AddDays(-(int)DateTime.UtcNow.ToIndonesiaTimeZone().DayOfWeek + 7);
            var NextWeekSaturday = DateTime.UtcNow.ToIndonesiaTimeZone().AddDays(-(int)DateTime.UtcNow.ToIndonesiaTimeZone().DayOfWeek + (int)DayOfWeek.Saturday + 7);


            var query = (from t in this._Db.Trainings
                         join c in this._Db.Courses on t.CourseId equals c.CourseId
                         where t.IsDeleted == false
                         select new DashboardClassData
                         {
                             CourseName = c.CourseName,
                             StartDate = t.StartDate,
                             EndDate = t.EndDate
                         }).AsNoTracking().AsQueryable();

            var getThisWeekDatas = await query.Where(
                Q => (ThisWeekSunday.Date < Q.StartDate.Value.Date
                || ThisWeekSunday.Date < Q.EndDate.Value.Date)
                && (ThisWeekSaturday.Date > Q.StartDate.Value.Date
                || ThisWeekSaturday.Date > Q.EndDate.Value.Date))
                .ToListAsync();

            var thisWeekTotalData = getThisWeekDatas.Count();

            var getNextWeekDatas = await query.Where(
               Q => (NextWeekSunday.Date < Q.StartDate.Value.Date
               || NextWeekSunday.Date < Q.EndDate.Value.Date)
               && (NextWeekSaturday.Date > Q.StartDate.Value.Date
               || NextWeekSaturday.Date > Q.EndDate.Value.Date))
               .ToListAsync();

            var nextWeekTotalData = getNextWeekDatas.Count();

            var classScheduleResult = new DashboardClassListViewModel
            {
                ThisWeekClassList = getThisWeekDatas.Take(5).ToList(),
                TotalThisWeekClass = thisWeekTotalData,
                NextWeekClassList = getNextWeekDatas.Take(5).ToList(),
                TotalNextWeekClass = nextWeekTotalData
            };

            return classScheduleResult;
        }

        public async Task<List<PositionNameModel>> GetAllPositionName()
        {
            var getPositionNameList = await this._Db.Positions
                .Where(Q => Q.PositionName != "*")
                .Select(Q => new PositionNameModel
                {
                    PositionName = Q.PositionName
                }).OrderBy(Q => Q.PositionName)
                .ToListAsync();

            return getPositionNameList;
        }

        public async Task<DashboardCompetencyMappingModel> GetPositionCompetencyMapping(string positionName)
        {
            //demanded
            var getDemandedDatas = (from pcm in this._Db.PositionCompentencyMappings
                                    join p in this._Db.Positions on pcm.PositionId equals p.PositionId
                                    join c in this._Db.Competencies on pcm.CompetencyId equals c.CompetencyId
                                    join ct in this._Db.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                                    where p.PositionName == positionName
                                    select new
                                    {
                                        prefixCode = c.PrefixCode,
                                        proficiencyLevel = pcm.ProficiencyLevel,
                                        competencyTypeId = ct.CompetencyTypeId
                                    });

            var demandedHardCompetencyDatas = await getDemandedDatas
                .Where(Q => Q.competencyTypeId == (int)CompetencyType.Hard)
                .Select(Q => new CompetencyMappingModel
                {
                    PrefixCode = Q.prefixCode,
                    ProficiencyLevel = Q.proficiencyLevel
                }).OrderBy(Q => Q.PrefixCode)
                .ToListAsync();

            var demandedSoftCompetencyDatas = await getDemandedDatas
                .Where(Q => Q.competencyTypeId == (int)CompetencyType.Soft)
                .Select(Q => new CompetencyMappingModel
                {
                    PrefixCode = Q.prefixCode,
                    ProficiencyLevel = Q.proficiencyLevel
                }).OrderBy(Q => Q.PrefixCode)
                .ToListAsync();


            //evaluated
            var getEvaluatedDatas = await _Db.GetDashboardEvaluatedCompetencyMapping(positionName).ToListAsync();

            var getCompetencyList = getEvaluatedDatas
                .GroupBy(Q => Q.PrefixCode)
                .Select(Q => new
                {
                    PrefixCode = Q.Key,
                    CompetencyTypeId = Q.Select(X => X.CompetencyTypeId).FirstOrDefault(),
                    proficiencyLevel = Q.Select(X => X.ProficiencyLevel).FirstOrDefault(),
                    ListScore = Q.Where(X => X.PrefixCode == Q.Key)
                        .Select(X => new
                        {
                            score = X.EvaluationTypeId == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.Score * 0.1
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.SkillId
                                    ? X.Score * 0.2
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.Score * 0.7
                                    : X.Score,
                            maxScore = X.EvaluationTypeId == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.MaxScore * 0.1
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.SkillId
                                    ? X.MaxScore * 0.2
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.MaxScore * 0.7
                                    : X.MaxScore
                        }).ToList()
                }).ToList();

            var dataEvaluated = getCompetencyList
                .Select(Q => new DashboardCompetencyMappingModel
                {
                    HardCompetency = getCompetencyList
                    .Where(X => X.CompetencyTypeId == (int)CompetencyType.Hard)
                    .Select(X => new CompetencyMappingModel
                    {
                        PrefixCode = X.PrefixCode,
                        ProficiencyLevel = X.proficiencyLevel,
                        ScoreTotal = X.ListScore.Select(Y => Y.score).Sum(),
                        MaxScoreTotal = X.ListScore.Select(Y => Y.maxScore).Sum()
                    }).ToList(),

                    SoftCompetency = getCompetencyList
                    .Where(X => X.CompetencyTypeId == (int)CompetencyType.Soft)
                    .Select(X => new CompetencyMappingModel
                    {
                        PrefixCode = X.PrefixCode,
                        ProficiencyLevel = X.proficiencyLevel,
                        ScoreTotal = X.ListScore.Select(Y => Y.score).Sum(),
                        MaxScoreTotal = X.ListScore.Select(Y => Y.maxScore).Sum(),
                    }).ToList(),
                }).FirstOrDefault();

            var positionCompetencyDatas = new DashboardCompetencyMappingModel
            {
                SoftCompetency = demandedSoftCompetencyDatas,
                HardCompetency = demandedHardCompetencyDatas
            };

            if (dataEvaluated != null)
            {
                if (dataEvaluated.HardCompetency.Count() != 0)
                {
                    foreach (var item in dataEvaluated.HardCompetency)
                    {
                        var getDataIndex = positionCompetencyDatas.HardCompetency.FindIndex(Q => Q.PrefixCode == item.PrefixCode);
                        positionCompetencyDatas.HardCompetency[getDataIndex] = item;
                    }
                }

                if (dataEvaluated.SoftCompetency.Count() != 0)
                {
                    foreach (var item in dataEvaluated.SoftCompetency)
                    {
                        var getDataIndex = positionCompetencyDatas.SoftCompetency.FindIndex(Q => Q.PrefixCode == item.PrefixCode);
                        positionCompetencyDatas.SoftCompetency[getDataIndex] = item;
                    }
                }
            }

            return positionCompetencyDatas;
        }

        public async Task<SetupTSLViewModel> GetAllTSLReportData()
        {
            var getAllTSLData = await this._Db.TrainingServiceLevels.ToListAsync();

            var getSalesTSLData = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 1).FirstOrDefault();
            var salesTSLDataResult = new Sales
            {
                BasicLevel = getSalesTSLData.BasicLevel,
                FundamentalLevel = getSalesTSLData.FundamentalLevel,
                AdvanceLevel = getSalesTSLData.AdvanceLevel
            };

            var getAfterSalesTSLData = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 2).FirstOrDefault();
            var afterSalesTSLDataResult = new AfterSales
            {
                BasicLevel = getAfterSalesTSLData.BasicLevel,
                FundamentalLevel = getAfterSalesTSLData.FundamentalLevel,
                AdvanceLevel = getAfterSalesTSLData.AdvanceLevel
            };

            var getTotalTslData = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 3).FirstOrDefault();
            var totalTSLDataResult = new TotalSales
            {
                BasicLevel = getTotalTslData.BasicLevel,
                FundamentalLevel = getTotalTslData.FundamentalLevel,
                AdvanceLevel = getTotalTslData.AdvanceLevel
            };

            var tslDataResult = new SetupTSLViewModel
            {
                Sales = salesTSLDataResult,
                AfterSales = afterSalesTSLDataResult,
                Total = totalTSLDataResult
            };

            return tslDataResult;
        }

        public DashboardTop5TopicViewModel GetTop5Topic()
        {

            var query = _Db.GetTop5Topic().ToList();

            var resultQuery = query.Select(Q => new DashboardTop5TopicData
            {
                TopicName = Q.TopicName,
                TopicTotal = Q.Total,
            }).ToList();

            var getTop5Topics = new DashboardTop5TopicViewModel
            {
                TopicsList = resultQuery
            };

            if (getTop5Topics == null)
            {
                getTop5Topics = new DashboardTop5TopicViewModel();
            }

            return getTop5Topics;
        }

        public DashboardTop5LearningViewModel GetTop5Learning()
        {
            var query = _Db.GetTop5Learning().ToList();

            var resultQuery = query.Select(Q => new DashboardTop5LearningData
            {
                CourseName = Q.CourseName,
                Rating = (float)Q.Rating
            }).ToList();

            var getTop5Learning = new DashboardTop5LearningViewModel
            {
                LearningsList = resultQuery
            };

            if (getTop5Learning == null)
            {
                getTop5Learning = new DashboardTop5LearningViewModel();
            }

            return getTop5Learning;
        }

        private int GetLearningLibraryDataCount(List<DashboardTotalLearningLibraryData> getAllSetupLearningData, string category)
        {
            return getAllSetupLearningData.Where(Q => Q.CategoryName == category).Select(Q => Q.Total).FirstOrDefault();
        }

        public async Task<DashboardTotalLearningLibraryViewModel> GetTotalLearningLibrary()
        {
            var getAllSetupLearningData = await this._Db.SetupLearning.GroupBy(Q => Q.LearningCategoryName).Select(Q => new DashboardTotalLearningLibraryData
            {
                CategoryName = Q.Key,
                Total = Q.Count()
            }).ToListAsync();

            var totalTask = await this._Db.Tasks.Where(Q => Q.IsDeleted == false).CountAsync();

            var total = getAllSetupLearningData.Sum(Q => Q.Total) + totalTask;

            var getTotalLearningLibrary = new DashboardTotalLearningLibraryViewModel
            {
                Total = total,
                TotalCourses = GetLearningLibraryDataCount(getAllSetupLearningData, LearningCategoryNameEnum.Course),
                TotalModules = GetLearningLibraryDataCount(getAllSetupLearningData, LearningCategoryNameEnum.Module),
                TotalPopQuiz = GetLearningLibraryDataCount(getAllSetupLearningData, LearningCategoryNameEnum.PopQuiz),
                TotalTask = totalTask,
            };

            return getTotalLearningLibrary;
        }

        public async Task<DashboardTop5NewsViewModel> GetTop5News()
        {
            var getTop5NewsList = await this._Db.News.Where(Q => Q.IsDeleted == false).OrderByDescending(Q => Q.TotalView).Take(5).ToListAsync();

            var getTop5NewsTitle = getTop5NewsList.Select(Q => new DashboardTop5NewsData
            {
                Title = Q.Title
            }).ToList();

            var getTop5NewsTitles = new DashboardTop5NewsViewModel
            {
                TopNewsList = getTop5NewsTitle
            };

            return getTop5NewsTitles;
        }

        public async Task<DashboardTop5EventsViewModel> GetTop5Events()
        {
            var query = await this._Db.GetTop5Events().ToListAsync();

            var getTop5EventsTitle = query.Select(Q => new DashboardTop5EventsData
            {
                Title = Q.Name
            }).ToList();

            var getTop5EventsTitles = new DashboardTop5EventsViewModel
            {
                TopEventsList = getTop5EventsTitle
            };

            return getTop5EventsTitles;
        }

        public DashboardTop5CoachViewModel GetTop5Coach()
        {
            var query = _Db.GetTop5Coach().ToList();

            var resultQuery = query.Select(Q => new DashboardTop5CoachData
            {
                CoachName = Q.CoachName,
                Rating = (float)Q.Rating
            }).ToList();

            var getTop5Coach = new DashboardTop5CoachViewModel
            {
                CoachList = resultQuery
            };

            if (getTop5Coach == null)
            {
                getTop5Coach = new DashboardTop5CoachViewModel();
            }

            return getTop5Coach;
        }

        public DashboardTop5RewardTypeViewModel GetTop5RewardType()
        {
            var query = _Db.GetTop5RewardType().ToList();

            var resultQuery = query.Select(Q => new DashboardTop5RewardTypeData
            {
                RewardType = Q.RewardType,
                TotalReward = Q.TotalReward
            }).ToList();

            var getTop5RewardType = new DashboardTop5RewardTypeViewModel
            {
                TopRewardTypeList = resultQuery
            };

            if (getTop5RewardType == null)
            {
                getTop5RewardType = new DashboardTop5RewardTypeViewModel();
            }

            return getTop5RewardType;
        }

        public DashboardNPSReportViewModel GetNPSReport()
        {
            //var getAllNPSReport = await this._Db.CoachRatings.ToListAsync();

            //var promotor = (float)getAllNPSReport.Where(Q => Q.RatingScore < 6).ToList().Sum(Q => Q.RatingScore);
            //var passive = (float)getAllNPSReport.Where(Q => Q.RatingScore >= 6 && Q.RatingScore < 8).Sum(Q => Q.RatingScore);
            //var detractors = (float)getAllNPSReport.Where(Q => Q.RatingScore >= 8).Sum(Q => Q.RatingScore);

            var getNPSReport = _Db.GetNPSReport().ToList();

            var getNPSReportResult = getNPSReport.Select(Q => new DashboardNPSReportViewModel
            {
                Promotor = Q.Promotor,
                Passive = Q.Passive,
                Detractors = Q.Detractors
            }).FirstOrDefault();

            if (getNPSReportResult == null)
            {
                getNPSReportResult = new DashboardNPSReportViewModel();
            }

            return getNPSReportResult;
        }

        public async Task<DashboardMyInsightViewModel> GetMyInsight()
        {
            var getCurrentDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var getPreviousMonthDate = new DateTime(getCurrentDate.Year, getCurrentDate.Month, 1).AddMonths(-1);
            var getPrevious2MonthDate = new DateTime(getCurrentDate.Year, getCurrentDate.Month, 1).AddMonths(-2);
            var getNextMonthDate = new DateTime(getCurrentDate.Year, getCurrentDate.Month, 1).AddMonths(1);

            var getAllMyInsightIn3Month = await this._Db.Surveys
                .Where(Q => Q.CreatedAt >= getPrevious2MonthDate && Q.CreatedAt < getNextMonthDate && Q.IsDeleted == false)
                .ToListAsync();

            var getAllMyInsight = getAllMyInsightIn3Month
                .GroupBy(Q => Q.CreatedAt.Month)
                .Select(Q => new DashboardMyInsightData
                {
                    GetMonth = Q.Select(X => X.CreatedAt.ToString("MMM", CultureInfo.InvariantCulture)).FirstOrDefault(),
                    TotalInsight = Q.Count()
                }).ToList();

            var getResultMyInsight = new DashboardMyInsightViewModel
            {
                ThisMonth = getAllMyInsight.Where(Q => Q.GetMonth == getCurrentDate.ToString("MMM", CultureInfo.InvariantCulture)).FirstOrDefault(),
                LastMonth = getAllMyInsight.Where(Q => Q.GetMonth == getPreviousMonthDate.ToString("MMM", CultureInfo.InvariantCulture)).FirstOrDefault(),
                Last2Month = getAllMyInsight.Where(Q => Q.GetMonth == getPrevious2MonthDate.ToString("MMM", CultureInfo.InvariantCulture)).FirstOrDefault(),
            };

            if (getResultMyInsight.ThisMonth == null)
            {
                getResultMyInsight.ThisMonth = new DashboardMyInsightData
                {
                    GetMonth = getCurrentDate.ToString("MMM", CultureInfo.InvariantCulture),
                    TotalInsight = 0
                };
            }

            if (getResultMyInsight.LastMonth == null)
            {
                getResultMyInsight.LastMonth = new DashboardMyInsightData
                {
                    GetMonth = getPreviousMonthDate.ToString("MMM", CultureInfo.InvariantCulture),
                    TotalInsight = 0
                };
            }

            if (getResultMyInsight.Last2Month == null)
            {
                getResultMyInsight.Last2Month = new DashboardMyInsightData
                {
                    GetMonth = getPrevious2MonthDate.ToString("MMM", CultureInfo.InvariantCulture),
                    TotalInsight = 0
                };
            }

            return getResultMyInsight;
        }

    }
}
