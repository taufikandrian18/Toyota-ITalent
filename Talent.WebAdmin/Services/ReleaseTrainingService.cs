using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.Services
{
    public class ReleaseTrainingService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private readonly InboxService InboxMan;
        private readonly UserSideInboxService mobileInboxesMan;
        private readonly SetupCourseService SetupCourseMan;
        private readonly ApprovalService ApprovalMan;

        public ReleaseTrainingService(TalentContext talentContext, ClaimHelper claimHelper, InboxService inboxService, SetupCourseService setupCourseService, ApprovalService approvalService, UserSideInboxService mobileInboxes)
        {
            this.ApprovalMan = approvalService;
            this.DB = talentContext;
            this.ClaimMan = claimHelper;
            this.InboxMan = inboxService;
            this.mobileInboxesMan = mobileInboxes;
            this.SetupCourseMan = setupCourseService;
        }

        public async Task<List<CourseReleaseTrainingModel>> GetCourseReleaseTraining(string courseName)
        {

            if (!String.IsNullOrWhiteSpace(courseName))
            {

                var resultCourse = await (from sl in this.DB.SetupLearning
                                          join c in this.DB.Courses on sl.CourseId equals c.CourseId

                                          where c.CourseName.ToLower().Contains(courseName.ToLower()) && c.SetupCourseApprovedAt != null
                                          select new CourseReleaseTrainingModel
                                          {
                                              CourseId = sl.CourseId.GetValueOrDefault(),
                                              CourseName = sl.LearningName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningTypeId = c.LearningTypeId
                                          }).AsNoTracking().ToListAsync();

                return resultCourse;
            }
            else
            {

                var resultCourse = await (from sl in this.DB.SetupLearning
                                          join c in this.DB.Courses on sl.CourseId equals c.CourseId

                                          where c.SetupCourseApprovedAt != null
                                          select new CourseReleaseTrainingModel
                                          {
                                              CourseId = sl.CourseId.GetValueOrDefault(),
                                              CourseName = sl.LearningName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningTypeId = c.LearningTypeId,

                                          }).AsNoTracking().ToListAsync();

                return resultCourse;
            }
        }

        public async Task<List<CourseReleaseTrainingModel>> GetAllCourseReleaseTraining(string courseName)
        {
            if (String.IsNullOrWhiteSpace(courseName))
            {
                var resultCourse = await (from sl in this.DB.SetupLearning
                                          join c in this.DB.Courses on sl.CourseId equals c.CourseId

                                          where c.SetupCourseApprovedAt != null
                                          select new CourseReleaseTrainingModel
                                          {
                                              CourseId = sl.CourseId.GetValueOrDefault(),
                                              CourseName = sl.LearningName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningTypeId = c.LearningTypeId
                                          }).AsNoTracking().ToListAsync();

                return resultCourse;
            }
            else
            {
                var resultCourse = await (from sl in this.DB.SetupLearning
                                          join c in this.DB.Courses on sl.CourseId equals c.CourseId

                                          where sl.LearningName.ToLower().Contains(courseName.ToLower()) && c.SetupCourseApprovedAt != null
                                          select new CourseReleaseTrainingModel
                                          {
                                              CourseId = sl.CourseId.GetValueOrDefault(),
                                              CourseName = sl.LearningName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningTypeId = c.LearningTypeId
                                          }).AsNoTracking().ToListAsync();

                return resultCourse;
            }
        }

        public async Task<List<ApprovalStatusViewModels>> GetAllApprovalStatus()
        {
            var result = await this.DB.ApprovalStatus.Select(Q => new ApprovalStatusViewModels
            {
                ApprovalId = Q.ApprovalStatusId,
                ApprovalName = Q.ApprovalName
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<List<TeachingTimepPointsModel>> GetAllTeachingTimePoints()
        {
            var result = await this.DB.TimePoints.Where(Q => Q.PointTypeId == 2).Select(Q => new TeachingTimepPointsModel
            {
                Points = Q.Points,
                Time = Q.Time,
                TimePointId = Q.TimePointId
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<int> GetBatchReleaseTraining(int courseId)
        {
            var countBacth = await (from t in this.DB.Trainings
                                    join att in this.DB.ApprovalToTrainings on t.TrainingId equals att.TrainingId
                                    join a in this.DB.Approvals on att.ApprovalId equals a.ApprovalId

                                    where a.ApprovalStatusId != ApprovalStatusesEnum.RejectedId && t.CourseId == courseId

                                    select new
                                    {
                                        t.TrainingId
                                    }).CountAsync();
            //var count = await this.DB.Trainings.Where(Q => Q.CourseId == courseId).CountAsync();
            return countBacth + 1;
        }

        public async Task<List<ReleaseTrainingSetupModuleModel>> GetAllSetupModuleReleaseResult(int courseId)
        {
            var response = new List<ReleaseTrainingSetupModuleModel>();
            var result = await (
                from sm in this.DB.SetupModules

                join tt in this.DB.TrainingTypes on sm.TrainingTypeId equals tt.TrainingTypeId

                join m in this.DB.Modules on sm.ModuleId equals m.ModuleId

                where sm.CourseId == courseId && sm.IsDeleted == false
                select new ReleaseTrainingSetupModuleModel
                {
                    TrainingTypesName = tt.TrainingTypeName,
                    ModuleName = m.ModuleName,
                    SetupModuleId = sm.SetupModuleId,
                    Type = "Module",
                }).AsNoTracking().ToListAsync();

            var resultAssesment = await (
             from sm in this.DB.SetupModules

             join tt in this.DB.TrainingTypes on sm.TrainingTypeId equals tt.TrainingTypeId

             join m in this.DB.Assesments on sm.AssesmentId equals m.GUID

             where sm.CourseId == courseId && sm.IsDeleted == false
             select new ReleaseTrainingSetupModuleModel
             {
                 TrainingTypesName = tt.TrainingTypeName,
                 ModuleName = m.Name,
                 SetupModuleId = sm.SetupModuleId,
                 Type = "Assesment",
             }).AsNoTracking().ToListAsync();

            response.AddRange(result);
            response.AddRange(resultAssesment);
            return response;
        }

        public async Task<List<CoachForReleaseTraining>> GetCoachForReleaseTraining(string coachName)
        {
            var result = await (from c in this.DB.Coaches
                                join e in this.DB.Employees on c.EmployeeId equals e.EmployeeId
                                where c.CoachIsActive == true && e.EmployeeName.ToLower().Contains(coachName)
                                select new CoachForReleaseTraining
                                {
                                    CoachId = c.CoachId,
                                    EmployeeName = e.EmployeeName,
                                    EmployeeId = e.EmployeeId
                                }).AsNoTracking().ToListAsync();
            return result;

        }




        public async Task<bool> SetupRelaseTraining(ReleaseTrainingFormModel model)
        {
            try
            {
                var batch = await this.GetBatchReleaseTraining(model.Course.CourseId);
                var userLogin = this.ClaimMan.GetLoginUserId();

                var newReleaseTraining = new Trainings
                {
                    ApprovedAt = null,
                    Batch = batch,
                    CourseId = model.Course.CourseId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    EndDate = model.EndDate?.ToIndonesiaTimeZone(),
                    IsAccommodate = model.IsAccommodate,
                    IsParticipantTrainee = model.IsParticipantTrainee,
                    //IsPublished = false,
                    StartDate = model.StartDate?.ToIndonesiaTimeZone(),
                    CreatedBy = userLogin,
                    UpdatedBy = userLogin,
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    Quota = model.Quota,
                    Location = model.Location,
                    IsParticipantPermanent = model.IsParticipantPermanent,
                    EnumCeritificationTrigger = model.EnumCertificationTtrigger,
                    EnumCertificate = model.EnumCertificate,
                    IsCertificate = model.IsCertificate,
                };

                if (newReleaseTraining.IsCertificate == false)
                {
                    newReleaseTraining.EnumCeritificationTrigger = null;
                    newReleaseTraining.EnumCertificate = null;
                }

                this.DB.Trainings.Add(newReleaseTraining);

                var listModuleTrainingMapping = new List<TrainingModuleMappings>();

                foreach (var module in model.ListSetupModule)
                {
                    var newModuleTrainingMapping = new TrainingModuleMappings
                    {
                        CoachId = module.Coach != null ? module.Coach.CoachId : null,
                        SetupModuleId = module.SetupModuleId.GetValueOrDefault(),
                        TimePointId = module.TeachingTimePoint != null ? module.TeachingTimePoint.TimePointId : null,
                        TrainingEnd = module.TrainingEnd != null ? module.TrainingEnd.GetValueOrDefault().ToIndonesiaTimeZone() : (DateTime?)null,
                        TrainingStart = module.TrainingStart != null ? module.TrainingStart.GetValueOrDefault().ToIndonesiaTimeZone() : (DateTime?)null,
                        TrainingId = newReleaseTraining.TrainingId,
                    };

                    listModuleTrainingMapping.Add(newModuleTrainingMapping);
                }

                var listPositionTrainingMapping = new List<TrainingPositionMappings>();
                var listPositionOnlyViewMapping = new List<TrainingPositionOnlyViewMappings>();

                await this.DB.TrainingModuleMappings.AddRangeAsync(listModuleTrainingMapping);

                foreach (var position in model.ListPosition)
                {
                    var newPostionTrainingMapping = new TrainingPositionMappings
                    {
                        PositionId = position.PositionId,
                        TrainingId = newReleaseTraining.TrainingId
                    };

                    listPositionTrainingMapping.Add(newPostionTrainingMapping);
                }

                await this.DB.TrainingPositionMappings.AddRangeAsync(listPositionTrainingMapping);

                foreach (var item in model.ListPositionOnlyView)
                {
                    var newPositionOnlyView = new TrainingPositionOnlyViewMappings
                    {
                        PositionId = item.PositionId,
                        TrainingId = newReleaseTraining.TrainingId
                    };
                    listPositionOnlyViewMapping.Add(newPositionOnlyView);
                }

                await this.DB.TrainingPositionOnlyViewMappings.AddRangeAsync(listPositionOnlyViewMapping);

                var listCertifications = new List<TrainingCertifications>();
                if (model.ListCertifications != null)
                {
                    foreach (var item in model.ListCertifications)
                    {
                        var newCertifications = new TrainingCertifications
                        {
                            GUID = Guid.NewGuid().ToString(),
                            RelatedTrainingId = item.CourseId,
                            TrainingId = newReleaseTraining.TrainingId,
                        };
                        listCertifications.Add(newCertifications);
                    }
                    await this.DB.TrainingCertifications.AddRangeAsync(listCertifications);
                }


                var listOutletTrainingMapping = new List<TrainingOutletMappings>();

                foreach (var outlet in model.ListOutlet)
                {
                    var newOutletMapping = new TrainingOutletMappings
                    {
                        OutletId = outlet.OutletId,
                        TrainingId = newReleaseTraining.TrainingId
                    };

                    listOutletTrainingMapping.Add(newOutletMapping);
                }

                await this.DB.AddRangeAsync(listOutletTrainingMapping);
                await this.DB.SaveChangesAsync();

                var newApproval = new ApprovalCreateFormModel
                {
                    ApprovalStatusId = model.InputType == 1 ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                    ContentCategory = ContentCategoryEnums.ReleaseTraining,
                    ContentId = newReleaseTraining.TrainingId,
                    ContentName = model.Course.CourseName,
                    PageEnum = PageEnums.ReleaseTraining
                };

                var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

                if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                {
                    newReleaseTraining.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                }

                await this.DB.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var result = e.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// To get positions by OutletId
        /// </summary>
        /// <param name="outletJson"></param>
        /// <returns></returns>dotnet ef --startup-project ../Talent.WebAdmin/ database update 0
        public async Task<List<PositionViewModel>> GetAllPositionByOutletIds(string outletJson)
        {
            var outletIds = JsonConvert.DeserializeObject<List<string>>(outletJson);

            var result = (from e in DB.Employees
                          join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                          join p in DB.Positions on epm.PositionId equals p.PositionId
                          where p.PositionName != "*"
                          select new
                          {
                              e.OutletId,
                              p.PositionId,
                              p.PositionName
                          }).AsQueryable();

            var listPosition = new List<PositionViewModel>();

            if (outletIds.Count > 0)
            {
                result = result.Where(Q => outletIds.Contains(Q.OutletId));
            }

            listPosition = await result.Select(Q => new PositionViewModel
            {
                PositionId = Q.PositionId,
                PositionName = Q.PositionName
            }).Distinct().ToListAsync();

            return listPosition;
        }

        public async Task<List<PositionViewModel>> GetAllPosition()
        {

            var result = await this.DB.Positions.Where(Q => Q.PositionName != "*").Select(Q => new PositionViewModel
            {
                PositionId = Q.PositionId,
                PositionName = Q.PositionName
            }).ToListAsync();

            return result;
        }

        public async Task<List<AreaViewModel>> GetAllArea()
        {
            var result = await this.DB.Areas.Select(Q => new AreaViewModel
            {
                AreaId = Q.AreaId,
                Name = Q.Name
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<List<DealerViewModel>> GetAllDealer()
        {
            var result = await this.DB.Dealers.Select(Q => new DealerViewModel
            {
                DealerId = Q.DealerId,
                DealerName = Q.DealerName
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<List<CityViewModel>> GetAllCity()
        {
            var result = await this.DB.Cities.Select(Q => new CityViewModel
            {
                CityId = Q.CityId,
                CityName = Q.CityName
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<List<ProvinceViewModel>> GetAllProvince()
        {
            var result = await this.DB.Provinces.Select(Q => new ProvinceViewModel
            {
                ProvinceId = Q.ProvinceId,
                ProvinceName = Q.ProvinceName
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<List<OutletCompleteViewModel>> GetAllOutlet()
        {
            var result = await this.DB.Outlets.Select(Q => new OutletCompleteViewModel
            {
                Name = Q.Name,
                OutletId = Q.OutletId,
                ProvinceId = Q.ProvinceId,
                AreaId = Q.AreaId,
                CityId = Q.CityId,
                DealerId = Q.DealerId,
                AreaSpecialistId = Q.AreaSpecialistId
            }).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<List<OutletViewModel>> GetOutletFilteredIds(string filterJson)
        {
            var model = JsonConvert.DeserializeObject<FilterIdForOutletModel>(filterJson);

            var query = this.DB.Outlets.AsNoTracking();

            if (model.Area.Count > 0)
            {
                query = query.Where(Q => model.Area.Contains(Q.AreaId));
            }

            if (model.Province.Count > 0)
            {
                query = query.Where(Q => model.Province.Contains(Q.ProvinceId));
            }

            if (model.Dealer.Count > 0)
            {
                query = query.Where(Q => model.Dealer.Contains(Q.DealerId));
            }

            if (model.City.Count > 0)
            {
                query = query.Where(Q => model.City.Contains(Q.CityId));
            }

            var result = await query.Select(Q => new OutletViewModel
            {
                Name = Q.Name,
                OutletId = Q.OutletId
            }).ToListAsync();

            return result;
        }

        public async Task<List<OutletViewModel>> GetOutletFiltered(string filterJson)
        {
            var model = JsonConvert.DeserializeObject<FilterForOutletModel>(filterJson);

            var areaIds = model.Area.Select(Q => Q.AreaId).ToList();
            var provinceIds = model.Province.Select(Q => Q.ProvinceId).ToList();
            var dealerIds = model.Dealer.Select(Q => Q.DealerId).ToList();
            var cityIds = model.City.Select(Q => Q.CityId).ToList();

            var query = this.DB.Outlets.AsNoTracking();
            query = query.Where(Q => areaIds.Contains(Q.AreaId));
            query = query.Where(Q => provinceIds.Contains(Q.ProvinceId));
            query = query.Where(Q => dealerIds.Contains(Q.DealerId));
            query = query.Where(Q => cityIds.Contains(Q.CityId));

            var result = await query.Select(Q => new OutletViewModel
            {
                Name = Q.Name,
                OutletId = Q.OutletId
            }).ToListAsync();

            return result;
        }

        public async Task<ReleaseTrainingViewModel> GetReleaseTrainingFiltered(ReleaseTrainingFilter filter)
        {
            var query = (from t in this.DB.Trainings
                         join c in this.DB.Courses on t.CourseId equals c.CourseId

                         join att in this.DB.ApprovalToTrainings on t.TrainingId equals att.TrainingId into latt
                         from att in latt.DefaultIfEmpty()

                         join a in this.DB.Approvals on att.ApprovalId equals a.ApprovalId into la
                         from a in la.DefaultIfEmpty()

                         join ass in this.DB.ApprovalStatus on a.ApprovalStatusId equals ass.ApprovalStatusId into lass
                         from ass in lass.DefaultIfEmpty()

                         where t.IsDeleted == false
                         select new
                         {
                             ApprovalStatus = ass.ApprovalName == null ? "" : ass.ApprovalName,
                             Batch = t.Batch,
                             CourseName = c.CourseName,
                             ProgramTypeId = c.ProgramTypeId,
                             ApprovalStatusId = (int?)a.ApprovalStatusId,
                             CreatedAt = t.CreatedAt,
                             EndDate = t.EndDate,
                             //IsPublished = t.IsPublished,
                             StartDate = t.StartDate,
                             TrainingId = t.TrainingId,
                             UpdatedAt = t.UpdatedAt
                         }).AsNoTracking();

            if (filter.ApprovalStatusId != null)
            {
                query = query.Where(Q => Q.ApprovalStatusId == filter.ApprovalStatusId);
            }

            if (filter.Batch != null)
            {
                query = query.Where(Q => Q.Batch == filter.Batch);
            }

            if (filter.ProgramTypeId != 0 && filter.ProgramTypeId != null)
            {
                query = query.Where(Q => Q.ProgramTypeId == filter.ProgramTypeId);
            }

            if (string.IsNullOrEmpty(filter.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.CourseName.ToLower()));
            }

            if (filter.EnrollmentEndDate != null)
            {
                var newEndDate = filter.EnrollmentEndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => Q.EndDate.Value.Date == endDate);
            }

            if (filter.EnrollmentStartDate != null)
            {
                var newStartDate = filter.EnrollmentStartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                query = query.Where(Q => Q.StartDate.Value.Date == startDate);
            }

            if (filter.DateFilterStart != null && filter.DateFilterEnd != null)
            {
                var newStartDate = filter.DateFilterStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateFilterEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            //if (filter.DateFilterStart != null)
            //{
            //    query = query.Where(Q => Q.CreatedAt >= filter.DateFilterStart || Q.UpdatedAt >= filter.DateFilterStart);
            //}

            //if (filter.DateFilterEnd != null)
            //{
            //    query = query.Where(Q => Q.CreatedAt <= filter.DateFilterEnd.EndDayTime() || Q.UpdatedAt <= filter.DateFilterEnd.EndDayTime());
            //}

            switch (filter.SortBy)
            {
                case "courseName":
                    query = query.OrderBy(Q => Q.CourseName);
                    break;
                case "courseNameDesc":
                    query = query.OrderByDescending(Q => Q.CourseName);
                    break;

                case "batch":
                    query = query.OrderBy(Q => Q.Batch);
                    break;
                case "batchDesc":
                    query = query.OrderByDescending(Q => Q.Batch);
                    break;

                case "startDate":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;

                case "endDate":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;

                case "approvalStatus":
                    query = query.OrderBy(Q => Q.ApprovalStatus);
                    break;
                case "approvalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus);
                    break;

                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;

                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;

                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.CountAsync();

            var result = await query.Select(Q => new ReleaseTrainingModel
            {
                UpdatedAt = Q.UpdatedAt,
                ApprovalStatus = Q.ApprovalStatus,
                Batch = Q.Batch,
                CourseName = Q.CourseName,
                CreatedAt = Q.CreatedAt,
                EndDate = Q.EndDate.GetValueOrDefault(),
                // IsPublished = Q.IsPublished,
                StartDate = Q.StartDate.GetValueOrDefault(),
                TrainingId = Q.TrainingId,

            }).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var viewRelease = new ReleaseTrainingViewModel
            {
                ListTraining = result,
                TotalData = totalData
            };

            return viewRelease;
        }

        public async Task<ReleaseTrainingViewModel> GetReleaseTrainingByDealerFiltered(ReleaseTrainingByDealerFilter filter)
        {
            var query = (from t in this.DB.Trainings
                         join c in this.DB.Courses on t.CourseId equals c.CourseId

                         join att in this.DB.ApprovalToTrainings on t.TrainingId equals att.TrainingId into latt
                         from att in latt.DefaultIfEmpty()

                         join a in this.DB.Approvals on att.ApprovalId equals a.ApprovalId into la
                         from a in la.DefaultIfEmpty()

                         join ass in this.DB.ApprovalStatus on a.ApprovalStatusId equals ass.ApprovalStatusId into lass
                         from ass in lass.DefaultIfEmpty()

                         where t.IsDeleted == false
                         select new
                         {
                             ApprovalStatus = ass.ApprovalName == null ? "" : ass.ApprovalName,
                             Batch = t.Batch,
                             CourseName = c.CourseName,
                             ProgramTypeId = c.ProgramTypeId,
                             ApprovalStatusId = (int?)a.ApprovalStatusId,
                             CreatedAt = t.CreatedAt,
                             EndDate = t.EndDate,
                             //IsPublished = t.IsPublished,
                             StartDate = t.StartDate,
                             TrainingId = t.TrainingId,
                             UpdatedAt = t.UpdatedAt
                         }).AsNoTracking();

            if (filter.ApprovalStatusId != null)
            {
                query = query.Where(Q => Q.ApprovalStatusId == filter.ApprovalStatusId);
            }

            if (filter.Batch != null)
            {
                query = query.Where(Q => Q.Batch == filter.Batch);
            }

            if (filter.ProgramTypeId != 0 && filter.ProgramTypeId != null)
            {
                query = query.Where(Q => Q.ProgramTypeId == filter.ProgramTypeId);
            }

            if (string.IsNullOrEmpty(filter.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.CourseName.ToLower()));
            }

            if(!string.IsNullOrEmpty(filter.DealerId))
            {
                var trainingIdList = await (from tom in this.DB.TrainingOutletMappings
                                            join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                            where o.DealerId == filter.DealerId.Trim()
                                            select tom.TrainingId).Distinct().ToListAsync();

                query = query.Where(Q => trainingIdList.Contains(Q.TrainingId));
            }

            if (filter.EnrollmentEndDate != null)
            {
                var newEndDate = filter.EnrollmentEndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => Q.EndDate.Value.Date == endDate);
            }

            if (filter.EnrollmentStartDate != null)
            {
                var newStartDate = filter.EnrollmentStartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                query = query.Where(Q => Q.StartDate.Value.Date == startDate);
            }

            if (filter.DateFilterStart != null && filter.DateFilterEnd != null)
            {
                var newStartDate = filter.DateFilterStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateFilterEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            //if (filter.DateFilterStart != null)
            //{
            //    query = query.Where(Q => Q.CreatedAt >= filter.DateFilterStart || Q.UpdatedAt >= filter.DateFilterStart);
            //}

            //if (filter.DateFilterEnd != null)
            //{
            //    query = query.Where(Q => Q.CreatedAt <= filter.DateFilterEnd.EndDayTime() || Q.UpdatedAt <= filter.DateFilterEnd.EndDayTime());
            //}

            switch (filter.SortBy)
            {
                case "courseName":
                    query = query.OrderBy(Q => Q.CourseName);
                    break;
                case "courseNameDesc":
                    query = query.OrderByDescending(Q => Q.CourseName);
                    break;

                case "batch":
                    query = query.OrderBy(Q => Q.Batch);
                    break;
                case "batchDesc":
                    query = query.OrderByDescending(Q => Q.Batch);
                    break;

                case "startDate":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;

                case "endDate":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;

                case "approvalStatus":
                    query = query.OrderBy(Q => Q.ApprovalStatus);
                    break;
                case "approvalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus);
                    break;

                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;

                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;

                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.CountAsync();

            var result = await query.Select(Q => new ReleaseTrainingModel
            {
                UpdatedAt = Q.UpdatedAt,
                ApprovalStatus = Q.ApprovalStatus,
                Batch = Q.Batch,
                CourseName = Q.CourseName,
                CreatedAt = Q.CreatedAt,
                EndDate = Q.EndDate.GetValueOrDefault(),
                // IsPublished = Q.IsPublished,
                StartDate = Q.StartDate.GetValueOrDefault(),
                TrainingId = Q.TrainingId,

            }).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var viewRelease = new ReleaseTrainingViewModel
            {
                ListTraining = result,
                TotalData = totalData
            };

            return viewRelease;
        }


        public async Task<ReleaseTrainingViewModel> GetRelatedReleaseTrainingFiltered(ReleaseTrainingFilter filter)
        {
            var query = (from t in this.DB.Trainings
                         join c in this.DB.Courses on t.CourseId equals c.CourseId

                         join att in this.DB.ApprovalToTrainings on t.TrainingId equals att.TrainingId into latt
                         from att in latt.DefaultIfEmpty()

                         join a in this.DB.Approvals on att.ApprovalId equals a.ApprovalId into la
                         from a in la.DefaultIfEmpty()

                         join ass in this.DB.ApprovalStatus on a.ApprovalStatusId equals ass.ApprovalStatusId into lass
                         from ass in lass.DefaultIfEmpty()

                         where t.IsDeleted == false && t.IsCertificate
                         select new
                         {
                             ApprovalStatus = ass.ApprovalName == null ? "" : ass.ApprovalName,
                             Batch = t.Batch,
                             CourseName = c.CourseName,
                             ProgramTypeId = c.ProgramTypeId,
                             ApprovalStatusId = (int?)a.ApprovalStatusId,
                             CreatedAt = t.CreatedAt,
                             EndDate = t.EndDate,
                             //IsPublished = t.IsPublished,
                             StartDate = t.StartDate,
                             TrainingId = t.TrainingId,
                             UpdatedAt = t.UpdatedAt
                         }).AsNoTracking();

            if (filter.ApprovalStatusId != null)
            {
                query = query.Where(Q => Q.ApprovalStatusId == filter.ApprovalStatusId);
            }

            if (filter.Batch != null)
            {
                query = query.Where(Q => Q.Batch == filter.Batch);
            }

            if (filter.ProgramTypeId != 0 && filter.ProgramTypeId != null)
            {
                query = query.Where(Q => Q.ProgramTypeId == filter.ProgramTypeId);
            }

            if (string.IsNullOrEmpty(filter.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.CourseName.ToLower()));
            }

            if (filter.EnrollmentEndDate != null)
            {
                var newEndDate = filter.EnrollmentEndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => Q.EndDate.Value.Date == endDate);
            }

            if (filter.EnrollmentStartDate != null)
            {
                var newStartDate = filter.EnrollmentStartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                query = query.Where(Q => Q.StartDate.Value.Date == startDate);
            }

            if (filter.DateFilterStart != null && filter.DateFilterEnd != null)
            {
                var newStartDate = filter.DateFilterStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateFilterEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            var relatedTraining = await DB.TrainingCertifications.Select(x => x.RelatedTrainingId).ToListAsync();
            query = query.Where(x => !relatedTraining.Contains(x.TrainingId));

            //if (filter.DateFilterStart != null)
            //{
            //    query = query.Where(Q => Q.CreatedAt >= filter.DateFilterStart || Q.UpdatedAt >= filter.DateFilterStart);
            //}

            //if (filter.DateFilterEnd != null)
            //{
            //    query = query.Where(Q => Q.CreatedAt <= filter.DateFilterEnd.EndDayTime() || Q.UpdatedAt <= filter.DateFilterEnd.EndDayTime());
            //}

            switch (filter.SortBy)
            {
                case "courseName":
                    query = query.OrderBy(Q => Q.CourseName);
                    break;
                case "courseNameDesc":
                    query = query.OrderByDescending(Q => Q.CourseName);
                    break;

                case "batch":
                    query = query.OrderBy(Q => Q.Batch);
                    break;
                case "batchDesc":
                    query = query.OrderByDescending(Q => Q.Batch);
                    break;

                case "startDate":
                    query = query.OrderBy(Q => Q.StartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.StartDate);
                    break;

                case "endDate":
                    query = query.OrderBy(Q => Q.EndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.EndDate);
                    break;

                case "approvalStatus":
                    query = query.OrderBy(Q => Q.ApprovalStatus);
                    break;
                case "approvalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus);
                    break;

                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;

                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;

                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.CountAsync();

            var result = await query.Select(Q => new ReleaseTrainingModel
            {
                UpdatedAt = Q.UpdatedAt,
                ApprovalStatus = Q.ApprovalStatus,
                Batch = Q.Batch,
                CourseName = Q.CourseName,
                CreatedAt = Q.CreatedAt,
                EndDate = Q.EndDate.GetValueOrDefault(),
                // IsPublished = Q.IsPublished,
                StartDate = Q.StartDate.GetValueOrDefault(),
                TrainingId = Q.TrainingId,

            }).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var viewRelease = new ReleaseTrainingViewModel
            {
                ListTraining = result,
                TotalData = totalData
            };

            return viewRelease;
        }


        public async Task<RelaseTrainingDetailModel> GetTrainingDetailById(int trainingId)
        {
            var result = new RelaseTrainingDetailModel();
            var courseResult = await (from t in this.DB.Trainings
                                      join sl in this.DB.SetupLearning on t.CourseId equals sl.CourseId
                                      join c in this.DB.Courses on sl.CourseId equals c.CourseId
                                      where t.TrainingId == trainingId && t.IsDeleted == false
                                      select new RelaseTrainingDetailModel
                                      {
                                          Batch = t.Batch,
                                          Course = new CourseReleaseTrainingModel
                                          {
                                              CourseId = c.CourseId,
                                              CourseName = c.CourseName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningTypeId = c.LearningTypeId
                                          },
                                          EndDate = t.EndDate.GetValueOrDefault(),
                                          IsAccommodate = t.IsAccommodate,
                                          IsParticipantTrainee = t.IsParticipantTrainee,
                                          IsParticipantPermanent = t.IsParticipantPermanent,
                                          ListArea = new List<AreaViewModel>(),
                                          ListCity = new List<CityViewModel>(),
                                          ListOutlet = new List<OutletViewModel>(),
                                          ListPosition = new List<PositionViewModel>(),
                                          ListPositionOnlyView = new List<PositionOnlyViewModel>(),
                                          ListProvince = new List<ProvinceViewModel>(),
                                          ListRegion = new List<RegionViewModel>(),
                                          ListSetupModule = new List<TrainingModuleFormModel>(),
                                          ListDealer = new List<DealerViewModel>(),
                                          Location = t.Location,
                                          Quota = t.Quota.GetValueOrDefault(),
                                          StartDate = t.StartDate.GetValueOrDefault(),
                                          TrainingId = t.TrainingId,
                                          EnumCertificationTrigger = t.EnumCeritificationTrigger,
                                          EnumCertificate = t.EnumCertificate,
                                          IsCertificate = t.IsCertificate
                                      }).AsNoTracking().FirstOrDefaultAsync();


            if (courseResult != null)
            {
                result = courseResult;
            }
            else
            {
                var assesmentResult = await (from t in this.DB.Trainings
                                             join sl in this.DB.SetupLearning on t.CourseId equals sl.CourseId
                                             join c in this.DB.Assesments on sl.AssesmentId equals c.GUID
                                             join sm in this.DB.SetupModules on sl.SetupModuleId equals sm.SetupModuleId
                                             where t.TrainingId == trainingId && t.IsDeleted == false && sl.SetupModuleId != null && sm.ModuleId == null
                                             select new RelaseTrainingDetailModel
                                             {
                                                 Batch = t.Batch,
                                                 Course = new CourseReleaseTrainingModel
                                                 {
                                                     AssesmentId = c.GUID,
                                                     CourseName = c.Name,
                                                     ProgramTypeName = sl.ProgramTypeName,

                                                 },
                                                 EndDate = t.EndDate.GetValueOrDefault(),
                                                 IsAccommodate = t.IsAccommodate,
                                                 IsParticipantTrainee = t.IsParticipantTrainee,
                                                 IsParticipantPermanent = t.IsParticipantPermanent,
                                                 ListArea = new List<AreaViewModel>(),
                                                 ListCity = new List<CityViewModel>(),
                                                 ListOutlet = new List<OutletViewModel>(),
                                                 ListPosition = new List<PositionViewModel>(),
                                                 ListPositionOnlyView = new List<PositionOnlyViewModel>(),
                                                 ListProvince = new List<ProvinceViewModel>(),
                                                 ListRegion = new List<RegionViewModel>(),
                                                 ListSetupModule = new List<TrainingModuleFormModel>(),
                                                 ListDealer = new List<DealerViewModel>(),
                                                 Location = t.Location,
                                                 Quota = t.Quota.GetValueOrDefault(),
                                                 StartDate = t.StartDate.GetValueOrDefault(),
                                                 TrainingId = t.TrainingId,
                                                 EnumCertificationTrigger = t.EnumCeritificationTrigger,
                                             }).AsNoTracking().FirstOrDefaultAsync();
                if (assesmentResult != null)
                {
                    result = assesmentResult;
                }

            }

            if (result != null)
            {
                result.ListArea = await (from tom in this.DB.TrainingOutletMappings
                                         join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                         join a in this.DB.Areas on o.AreaId equals a.AreaId
                                         where tom.TrainingId == trainingId
                                         select new AreaViewModel
                                         {
                                             AreaId = a.AreaId,
                                             Name = a.Name
                                         }).AsNoTracking().ToListAsync();

                result.ListCity = await (from tom in this.DB.TrainingOutletMappings
                                         join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                         join c in this.DB.Cities on o.CityId equals c.CityId
                                         where tom.TrainingId == trainingId
                                         select new CityViewModel
                                         {
                                             CityId = c.CityId,
                                             CityName = c.CityName
                                         }).Distinct().AsNoTracking().ToListAsync();

                result.ListOutlet = await (from tom in this.DB.TrainingOutletMappings
                                           join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                           where tom.TrainingId == trainingId
                                           select new OutletViewModel
                                           {
                                               OutletId = o.OutletId,
                                               Name = o.Name
                                           }).Distinct().AsNoTracking().ToListAsync();

                result.ListProvince = await (from tom in this.DB.TrainingOutletMappings
                                             join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                             join p in this.DB.Provinces on o.ProvinceId equals p.ProvinceId
                                             where tom.TrainingId == trainingId
                                             select new ProvinceViewModel
                                             {
                                                 ProvinceId = p.ProvinceId,
                                                 ProvinceName = p.ProvinceName
                                             }).Distinct().AsNoTracking().ToListAsync();

                result.ListDealer = await (from tom in this.DB.TrainingOutletMappings
                                           join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                           join d in this.DB.Dealers on o.DealerId equals d.DealerId
                                           where tom.TrainingId == trainingId
                                           select new DealerViewModel
                                           {
                                               DealerId = d.DealerId,
                                               DealerName = d.DealerName
                                           }).Distinct().AsNoTracking().ToListAsync();

                //No Region?
                result.ListRegion = await (from tom in this.DB.TrainingOutletMappings
                                           join o in this.DB.Outlets on tom.OutletId equals o.OutletId
                                           join r in this.DB.Regions on o.RegionId equals r.RegionId
                                           where tom.TrainingId == trainingId
                                           select new RegionViewModel
                                           {
                                               RegionId = r.RegionId,
                                               RegionName = r.RegionName
                                           }).Distinct().AsNoTracking().ToListAsync();

                var listSetupModule = await (from tmm in this.DB.TrainingModuleMappings
                                             join sm in this.DB.SetupModules on tmm.SetupModuleId equals sm.SetupModuleId
                                             join tt in this.DB.TrainingTypes on sm.TrainingTypeId equals tt.TrainingTypeId

                                             join c in this.DB.Coaches on tmm.CoachId equals c.CoachId into lc
                                             from c in lc.DefaultIfEmpty()

                                             join e in this.DB.Employees on c.EmployeeId equals e.EmployeeId into le
                                             from e in le.DefaultIfEmpty()

                                             join m in this.DB.Modules on sm.ModuleId equals m.ModuleId
                                             join tp in this.DB.TimePoints on tmm.TimePointId equals tp.TimePointId into llp
                                             from tp in llp.DefaultIfEmpty()

                                             where tmm.TrainingId == trainingId && sm.ModuleId != null
                                             select new TrainingModuleFormModel
                                             {
                                                 SetupModuleId = sm.SetupModuleId,
                                                 TrainingId = trainingId,
                                                 Coach = new CoachForReleaseTraining
                                                 {
                                                     CoachId = c.CoachId,
                                                     EmployeeId = c.EmployeeId,
                                                     EmployeeName = e.EmployeeName
                                                 },
                                                 ModuleName = m.ModuleName,
                                                 TeachingTimePoint = new TeachingTimepPointsModel
                                                 {
                                                     TimePointId = tp.TimePointId,
                                                     Points = tp.Points,
                                                     Time = tp.Time
                                                 },
                                                 TrainingEnd = tmm.TrainingEnd,
                                                 TrainingModuleMappingId = tmm.TrainingModuleMappingId,
                                                 TrainingStart = tmm.TrainingStart,
                                                 TrainingTypesName = tt.TrainingTypeName
                                             }).AsNoTracking().OrderBy(Q => Q.SetupModuleId).ToListAsync();

                var listAssesmentModule = await (from tmm in this.DB.TrainingModuleMappings
                                                 join sm in this.DB.SetupModules on tmm.SetupModuleId equals sm.SetupModuleId
                                                 join tt in this.DB.TrainingTypes on sm.TrainingTypeId equals tt.TrainingTypeId

                                                 join c in this.DB.Coaches on tmm.CoachId equals c.CoachId into lc
                                                 from c in lc.DefaultIfEmpty()

                                                 join e in this.DB.Employees on c.EmployeeId equals e.EmployeeId into le
                                                 from e in le.DefaultIfEmpty()

                                                 join m in this.DB.Assesments on sm.AssesmentId equals m.GUID
                                                 join tp in this.DB.TimePoints on tmm.TimePointId equals tp.TimePointId into llp
                                                 from tp in llp.DefaultIfEmpty()

                                                 where tmm.TrainingId == trainingId && sm.AssesmentId != null
                                                 select new TrainingModuleFormModel
                                                 {
                                                     SetupModuleId = sm.SetupModuleId,
                                                     TrainingId = trainingId,
                                                     Coach = new CoachForReleaseTraining
                                                     {
                                                         CoachId = c.CoachId,
                                                         EmployeeId = c.EmployeeId,
                                                         EmployeeName = e.EmployeeName
                                                     },
                                                     ModuleName = m.Name,
                                                     TeachingTimePoint = new TeachingTimepPointsModel
                                                     {
                                                         TimePointId = tp.TimePointId,
                                                         Points = tp.Points,
                                                         Time = tp.Time
                                                     },
                                                     TrainingEnd = tmm.TrainingEnd,
                                                     TrainingModuleMappingId = tmm.TrainingModuleMappingId,
                                                     TrainingStart = tmm.TrainingStart,
                                                     TrainingTypesName = tt.TrainingTypeName
                                                 }).AsNoTracking().OrderBy(Q => Q.SetupModuleId).ToListAsync();


                result.ListSetupModule.AddRange(listSetupModule);
                result.ListSetupModule.AddRange(listAssesmentModule);

                result.ListPosition = await (from tpm in this.DB.TrainingPositionMappings
                                             join p in this.DB.Positions on tpm.PositionId equals p.PositionId
                                             where tpm.TrainingId == trainingId
                                             select new PositionViewModel
                                             {
                                                 PositionId = p.PositionId,
                                                 PositionName = p.PositionName
                                             }).AsNoTracking().ToListAsync();

                result.ListPositionOnlyView = await (from tpm in this.DB.TrainingPositionOnlyViewMappings
                                                     join p in this.DB.Positions on tpm.PositionId equals p.PositionId
                                                     where tpm.TrainingId == trainingId
                                                     select new PositionOnlyViewModel
                                                     {
                                                         PositionId = p.PositionId,
                                                         PositionName = p.PositionName
                                                     }).AsNoTracking().ToListAsync();

                result.TrainingCertificateView = await this.DB.TrainingCertifications.Include(xz => xz.RelatedTraining).ThenInclude(xz => xz.Course).Where(xz => xz.TrainingId == result.TrainingId).Select(xz => new TrainingCertificateView
                {
                    RelatedTrainingId = xz.RelatedTrainingId,
                    RelatedTrainingName = xz.RelatedTraining.Course.CourseName,
                }).ToListAsync();
            }

            var existingSetupModuleIds = new List<TrainingModuleFormModel>();

            foreach (var datum in result.ListSetupModule)
            {
                if (!existingSetupModuleIds.Select(x => x.SetupModuleId).ToList().Contains(datum.SetupModuleId))
                {
                    existingSetupModuleIds.Add(datum);
                }
            }

            result.ListSetupModule = existingSetupModuleIds.Where(x => x.TrainingId == trainingId).ToList();
            return result;
        }

        public async Task<bool> UpdateReleaseTraining(ReleaseTrainingFormModel model)
        {
            try
            {
                var userLogin = this.ClaimMan.GetLoginUserId();
                var findTraining = await this.DB.Trainings.Where(Q => Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();

                if (findTraining == null)
                {
                    return false;
                }

                var approvalToTrainings = await this.DB.ApprovalToTrainings.Where(Q => Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();

                if (approvalToTrainings == null)
                {
                    return false;
                }

                var approval = await this.DB.Approvals.Where(Q => Q.ApprovalId == approvalToTrainings.ApprovalId).FirstOrDefaultAsync();

                if (approval.ApprovalStatusId == 2)
                {
                    return false;
                }

                if ((model.StartDate != null || model.EndDate != null) &&
                    (findTraining.StartDate != model.StartDate || findTraining.EndDate != model.EndDate))
                {
                    findTraining.RescheduledAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                }

                findTraining.Location = model.Location;
                findTraining.IsAccommodate = model.IsAccommodate;
                findTraining.IsParticipantTrainee = model.IsParticipantTrainee;
                findTraining.StartDate = model.StartDate == null ? (DateTime?)null : model.StartDate.GetValueOrDefault().ToIndonesiaTimeZone();
                findTraining.EndDate = model.EndDate == null ? (DateTime?)null : model.EndDate.GetValueOrDefault().ToIndonesiaTimeZone();
                findTraining.CourseId = model.Course.CourseId;
                findTraining.IsParticipantPermanent = model.IsParticipantPermanent;
                findTraining.Quota = model.Quota;
                findTraining.IsCertificate = model.IsCertificate;
                findTraining.EnumCertificate = model.EnumCertificate;
                findTraining.EnumCeritificationTrigger = model.EnumCertificationTtrigger;
                findTraining.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                findTraining.UpdatedBy = userLogin;

                if (findTraining.CourseId != model.Course.CourseId)
                {
                    findTraining.Batch = await this.GetBatchReleaseTraining(model.Course.CourseId);
                }

                findTraining.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                var findPreviousOutlet = await this.DB.TrainingOutletMappings.Where(Q => Q.TrainingId == findTraining.TrainingId).ToListAsync();
                this.DB.TrainingOutletMappings.RemoveRange(findPreviousOutlet);

                var newListOutlet = new List<TrainingOutletMappings>();

                foreach (var outlet in model.ListOutlet)
                {
                    newListOutlet.Add(new TrainingOutletMappings
                    {
                        OutletId = outlet.OutletId,
                        TrainingId = findTraining.TrainingId
                    });
                }

                await this.DB.TrainingOutletMappings.AddRangeAsync(newListOutlet);

                var findPreviousPosition = await this.DB.TrainingPositionMappings.Where(Q => Q.TrainingId == findTraining.TrainingId).ToListAsync();
                this.DB.TrainingPositionMappings.RemoveRange(findPreviousPosition);

                var newListPosition = new List<TrainingPositionMappings>();

                foreach (var position in model.ListPosition)
                {
                    newListPosition.Add(new TrainingPositionMappings
                    {
                        PositionId = position.PositionId,
                        TrainingId = findTraining.TrainingId
                    });
                }

                await this.DB.AddRangeAsync(newListPosition);

                var findModuleTrainings = await this.DB.TrainingModuleMappings.Where(Q => Q.TrainingId == findTraining.TrainingId).ToListAsync();
                this.DB.TrainingModuleMappings.RemoveRange(findModuleTrainings);

                var newListModule = new List<TrainingModuleMappings>();

                foreach (var module in model.ListSetupModule)
                {
                    newListModule.Add(new TrainingModuleMappings
                    {
                        CoachId = module.Coach == null ? null : module.Coach.CoachId,
                        SetupModuleId = module.SetupModuleId.GetValueOrDefault(),
                        TimePointId = module.TeachingTimePoint == null ? null : module.TeachingTimePoint.TimePointId,
                        TrainingEnd = module.TrainingEnd != null ? module.TrainingEnd.GetValueOrDefault().ToIndonesiaTimeZone() : (DateTime?)null,
                        TrainingStart = module.TrainingStart != null ? module.TrainingStart.GetValueOrDefault().ToIndonesiaTimeZone() : (DateTime?)null,
                        TrainingId = findTraining.TrainingId,
                    });
                }

                await this.DB.TrainingModuleMappings.AddRangeAsync(newListModule);

                var updateApprovals = new ApprovalUpdateFormModel
                {
                    ApprovalId = approval.ApprovalId,
                    ApprovalStatusId = model.InputType == 1 ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                    ContentName = model.Course.CourseName,
                    PageEnum = PageEnums.ReleaseTraining,
                    ContentId = findTraining.TrainingId,
                    ContentCategory = ContentCategoryEnums.ReleaseTraining
                };

                var approvals = await this.ApprovalMan.UpdateNewApproval(updateApprovals);

                if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                {
                    findTraining.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                    if (findTraining.RescheduledAt != null)
                    {
                        await mobileInboxesMan.InsertInboxForTrainingEdited(model.TrainingId.Value);
                    }
                }
                else
                {
                    findTraining.ApprovedAt = null;
                }

                var findPreviousCertification = await this.DB.TrainingCertifications.Where(Q => Q.TrainingId == findTraining.TrainingId).ToListAsync();
                this.DB.TrainingCertifications.RemoveRange(findPreviousCertification);

                var newListCertifications = new List<TrainingCertifications>();

                foreach (var item in model.ListCertifications)
                {
                    newListCertifications.Add(new TrainingCertifications
                    {
                        RelatedTrainingId = item.CourseId,
                        TrainingId = findTraining.TrainingId
                    });
                }

                await this.DB.TrainingCertifications.AddRangeAsync(newListCertifications);



                var findPreviousPositionOnlyView = await this.DB.TrainingPositionOnlyViewMappings.Where(Q => Q.TrainingId == findTraining.TrainingId).ToListAsync();
                this.DB.TrainingPositionOnlyViewMappings.RemoveRange(findPreviousPositionOnlyView);

                var newListPositionOnlyView = new List<TrainingPositionOnlyViewMappings>();

                foreach (var position in model.ListPositionOnlyView)
                {
                    newListPositionOnlyView.Add(new TrainingPositionOnlyViewMappings
                    {
                        PositionId = position.PositionId,
                        TrainingId = findTraining.TrainingId
                    });
                }

                await this.DB.TrainingPositionOnlyViewMappings.AddRangeAsync(newListPositionOnlyView);
                await this.DB.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return false;
            }
        }


        public async Task<bool> RemoveReleaseTraining(int trainingId)
        {
            var findReleaseTraining = await this.DB.Trainings.Where(Q => Q.TrainingId == trainingId).FirstOrDefaultAsync();

            if (findReleaseTraining == null)
            {
                return false;
            }

            var approvalToTrainings = await this.DB.ApprovalToTrainings.Where(Q => Q.TrainingId == trainingId).FirstOrDefaultAsync();

            if (approvalToTrainings == null)
            {
                return false;
            }
            this.DB.ApprovalToTrainings.Remove(approvalToTrainings);

            //send inbox notification
            var sendNotifitication = await mobileInboxesMan.InsertInboxForTrainingRemoved(trainingId);
            if (sendNotifitication == false) return false;

            var isDeleted = await this.ApprovalMan.DeleteApproval(approvalToTrainings.ApprovalId);

            if (isDeleted == false)
            {
                return false;
            }

            var userLogin = this.ClaimMan.GetLoginUserId();

            findReleaseTraining.IsDeleted = true;
            findReleaseTraining.UpdatedBy = userLogin;
            findReleaseTraining.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();


            if (findReleaseTraining.Top5Course != 0)
            {
                findReleaseTraining.Top5Course = 0;
                var findTop5 = await this.DB.Trainings.Where(Q => Q.IsDeleted == false && Q.Top5Course != 0).Take(5).OrderBy(Q => Q.Top5Course).ToListAsync();
                findTop5.Remove(findReleaseTraining);
                var currentPost = 5;
                for (int a = 0; a < findTop5.Count; a++)
                {
                    findTop5[a].Top5Course = currentPost;
                    currentPost--;
                }
            }


            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<TotalCourseDetail> GetTotalCourseDetail(int courseId)
        {
            var listSetupModule = await (
                from sm in this.DB.SetupModules
                join tt in this.DB.TrainingTypes on sm.TrainingTypeId equals tt.TrainingTypeId
                join m in this.DB.Modules on sm.ModuleId equals m.ModuleId

                where sm.CourseId == courseId && sm.IsDeleted == false
                select new
                {
                    sm.SetupModuleId,
                    sm.TimePointId
                }).AsNoTracking().ToListAsync();

            var totalModule = listSetupModule.Count();

            var totalPoints = 0;
            var totalTime = 0;
            var totalScore = 0;
            var timePoints = listSetupModule.Select(Q => Q.TimePointId).ToList();
            var setupModule = listSetupModule.Select(Q => Q.SetupModuleId).ToList();

            var timePoint = (from t in timePoints
                             join tp in this.DB.TimePoints on t equals tp.TimePointId
                             select tp).ToList();
            totalTime += timePoint.Sum(Q => Q.Time.GetValueOrDefault());
            totalPoints += timePoint.Sum(Q => Q.Points);

            var setupTask = await this.DB.SetupTasks.Where(Q => setupModule.Contains(Q.SetupModuleId.GetValueOrDefault())).ToListAsync();

            var setupTaskIds = setupTask.Select(Q => Q.SetupTaskId).ToList();
            var setupTaskTestTime = setupTask.Select(Q => Q.TestTime).ToList();

            totalTime += setupTaskTestTime.Sum();

            var setupTaskCode = await this.DB.SetupTaskCodes.Where(Q => setupTaskIds.Contains(Q.SetupTaskId)).ToListAsync();
            var taskIds = setupTaskCode.Select(Q => Q.TaskId).ToList();
            var setupTasksIds = setupTaskCode.Select(Q => Q.SetupTaskId).ToList();

            var tasks = await (from stc in this.DB.SetupTaskCodes
                               join t in this.DB.Tasks on stc.TaskId equals t.TaskId
                               where setupTaskIds.Contains(stc.SetupTaskId) && t.IsDeleted == false
                               select new
                               {
                                   t.QuestionTypeId,
                                   t.TaskId
                               }).ToListAsync();

            foreach (var taskResult in tasks)
            {
                var taskPointScore = await this.SetupCourseMan.GetScorePoints(taskResult.QuestionTypeId, taskResult.TaskId);
                totalScore += taskPointScore.Score;
                totalPoints += taskPointScore.Points;
            }

            var result = new TotalCourseDetail
            {
                TotalModule = totalModule,
                TotalPoints = totalPoints,
                TotalScore = totalScore,
                TotalTime = totalTime
            };

            return result;
        }

    }
}
