using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideSearch
    {
        private readonly TalentContext DB;
        private readonly FileService FileService;
        private readonly UserSideBadgesService BadgesService;

        public UserSideSearch(TalentContext db, FileService fileService, UserSideBadgesService badgesService)
        {
            this.DB = db;
            this.FileService = fileService;
            this.BadgesService = badgesService;
        }

        /// <summary>
        /// Get All Learnings Data
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        //public async Task<List<LearningViewModel>> GetAllLearnings(LearningFilter filter)
        //{

        //    var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

        //    var releaseTraining = await this.DB.Trainings
        //        .Where(Q => Q.IsDeleted == false && Q.ApprovedAt != null)
        //        .Select(Q => Q.CourseId).ToListAsync();

        //    var releaseLearning = await this.DB.SetupLearning
        //        .Where(Q => Q.ApprovalStatus.ToLower() == "approved")
        //        .Select(Q => Q.CourseId.GetValueOrDefault()).ToListAsync();

        //    var tempList = new List<int>();
        //    tempList.AddRange(releaseTraining);
        //    tempList.AddRange(releaseLearning);

        //    var query = (from sm in DB.SetupModules

        //                 join sl in DB.SetupLearning on sm.CourseId equals sl.CourseId

        //                 join t in DB.Trainings on sl.CourseId equals t.CourseId

        //                 join c in DB.Courses on sm.CourseId equals c.CourseId

        //                 join atc in DB.ApprovalToCourses on c.CourseId equals atc.CourseId into catc
        //                 from atc in catc.DefaultIfEmpty()

        //                 join a in DB.Approvals on atc.ApprovalId equals a.ApprovalId into atca
        //                 from a in atca.DefaultIfEmpty()

        //                 join pt in DB.ProgramTypes on c.ProgramTypeId equals pt.ProgramTypeId

        //                 join b in DB.Blobs on c.BlobId equals b.BlobId into cb
        //                 from b in cb.DefaultIfEmpty()

        //                 join m in this.DB.Modules on sm.ModuleId equals m.ModuleId

        //                 join lt in DB.LearningTypes on c.LearningTypeId equals lt.LearningTypeId

        //                 where a.ApprovalStatusId == 1 && tempList.Contains(c.CourseId) && c.IsDeleted == false && t.IsDeleted == false
        //                 select new
        //                 {
        //                     t.TrainingId,
        //                     CourseName = c.CourseName,
        //                     t.Batch,
        //                     t.StartDate,
        //                     t.EndDate,
        //                     c.CourseId,
        //                     c.ProgramTypeId,
        //                     c.LearningTypeId,
        //                     pt.ProgramTypeName,
        //                     c.BlobId,
        //                     b.Mime,
        //                     c.CreatedAt,
        //                     c.UpdatedAt,
        //                     c.IsPopular,
        //                     c.CoursePrice,
        //                     t.ApprovedAt,
        //                     m.MaterialTypeId
        //                 })
        //                 .Distinct()
        //                 .AsNoTracking();

        //    if (filter.LearningName != null)
        //    {
        //        query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.LearningName.ToLower()));
        //    }
        //    if (!(filter.ProgramTypeId == null))
        //    {
        //        foreach (var programType in filter.ProgramTypeId)
        //        {
        //            if (programType != null)
        //            {
        //                query = query.Where(Q => Q.ProgramTypeId == programType);
        //            }
        //        }
        //    }
        //    if (!(filter.LearningTypeId == null))
        //    {
        //        foreach (var learningType in filter.LearningTypeId)
        //        {
        //            if (learningType != null)
        //            {
        //                query = query.Where(Q => Q.LearningTypeId == learningType);
        //            }
        //        }
        //    }
        //    if (!(filter.MaterialTypeId == null))
        //    {
        //        foreach (var materialType in filter.MaterialTypeId)
        //        {
        //            if (materialType != null)
        //            {
        //                query = query.Where(Q => Q.MaterialTypeId == materialType);
        //            }
        //        }
        //    }
        //    var ratings = (from tr in DB.TrainingRatings
        //                   group tr by tr.TrainingId into RatingGroup
        //                   select new
        //                   {
        //                       TrainingId = RatingGroup.Key,
        //                       TrainingRatingScore = RatingGroup.Select(Q => Q.RatingScore).Sum() * 1.0 / RatingGroup.Count() * 1.0,
        //                   }).ToDictionary(Q => Q.TrainingId, Q => Q.TrainingRatingScore);

        //    var dataCourses = query
        //        .Select(x => new LearningViewModel
        //        {
        //            TrainingName = x.CourseName,
        //            ProgramTypeName = x.ProgramTypeName,
        //            TrainingId = x.TrainingId,
        //            CourseId = x.CourseId,
        //            TrainingBatch = x.Batch,
        //            ImageUrl = FileService.GetFileAsync(x.BlobId.ToString(), x.Mime).Result,
        //            Rating = ratings.ContainsKey(x.TrainingId) ? ratings[x.TrainingId] : 0,
        //            IsPopular = x.IsPopular == true ? true : false,
        //            CreatedAt = x.CreatedAt,
        //            UpdatedAt = x.UpdatedAt
        //        });

        //    switch (filter.SortBy)
        //    {
        //        case "date":
        //            dataCourses = dataCourses.OrderBy(Q => Q.CreatedAt);
        //            break;
        //        case "dateDesc":
        //            dataCourses = dataCourses.OrderByDescending(Q => Q.CreatedAt);
        //            break;
        //        case "name":
        //            dataCourses = dataCourses.OrderBy(Q => Q.TrainingName);
        //            break;
        //        case "nameDesc":
        //            dataCourses = dataCourses.OrderByDescending(Q => Q.TrainingName);
        //            break;
        //        case "popular":
        //            dataCourses = dataCourses.OrderBy(Q => Q.IsPopular);
        //            break;
        //    }

        //    var grids = await dataCourses
        //        .AsNoTracking()
        //        .ToListAsync();

        //    switch (filter.SortBy)
        //    {
        //        case "rating":
        //            grids = grids.OrderBy(Q => Q.Rating).ToList();
        //            break;
        //        case "ratingDesc":
        //            grids = grids.OrderByDescending(Q => Q.Rating).ToList();
        //            break;
        //    }
        //    grids = grids.Skip((int)skipCount).Take(filter.PageSize).ToList();

        //    foreach (var grid in grids)
        //    {
        //        grid.SetupModuleId = await this.DB.SetupModules.Where(Q => Q.CourseId == grid.CourseId).Select(Q => Q.SetupModuleId).FirstOrDefaultAsync();
        //    }

        //    return grids;
        //}

        public async Task<List<LearningViewModel>> GetAllLearnings(LearningFilter filter, String employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            //var query = this.DB.GetAllLearningModels().AsQueryable();
            var employeeOutletId = await DB.Employees
                                    .Where(Q => Q.EmployeeId == employeeId)
                                    .Select(Q => Q.OutletId)
                                    .FirstOrDefaultAsync();
            var positionIds = await DB.EmployeePositionMappings
                                    .Where(Q => Q.EmployeeId == employeeId)
                                    .Select(Q => Q.PositionId)
                                    .ToListAsync();
            IQueryable<Entities.DbQueryModels.GetAllLearningUpdateModel> query;
            //var query = this.DB.GetAllLearningModels().AsQueryable();
            if (employeeOutletId != null)
            {
                // Orang Dealer
                query = DB.GetAllLearningUpdateModels(employeeOutletId, positionIds).AsQueryable();
            }
            else
            {
                // Orang TAM
                query = DB.GetAllLearningUpdateModels(positionIds).AsQueryable();
            }

            if (filter.LearningName != null)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.LearningName.ToLower()) || Q.ModuleName.ToLower().Contains(filter.LearningName.ToLower()));
            }
            if (!(filter.ProgramTypeId == null))
            {
                query = query.Where(Q => filter.ProgramTypeId.Contains(Q.ProgramTypeId));
            }
            if (!(filter.CoursePrice == null))
            {
                query = query.Where(Q => filter.CoursePrice.Contains(Q.CoursePrice));
            }
            if (!(filter.LearningTypeId == null))
            {
                query = query.Where(Q => filter.LearningTypeId.Contains(Q.LearningTypeId));
            }
            if (!(filter.MaterialTypeId == null))
            {
                var satu = filter.MaterialTypeId[0].ToString();
                var dua = filter.MaterialTypeId.Count >= 2 ? filter.MaterialTypeId[1].ToString() : satu;
                var tiga = filter.MaterialTypeId.Count >= 3 ? filter.MaterialTypeId[2].ToString() : satu;

                query = query.Where(Q => Q.MaterialTypeId.Contains(satu) || Q.MaterialTypeId.Contains(dua) || Q.MaterialTypeId.Contains(tiga));
            }

            var dataLearning = query.Select(Q => new
            {
                TrainingName = Q.CourseName != null ? Q.CourseName : Q.ModuleName,
                SetupModuleId = Q.SetupModuleId,
                ProgramTypeName = Q.ProgramTypeName,
                TrainingId = Q.TrainingId != null ? Q.TrainingId : null,
                CourseId = Q.CourseId,
                TrainingBatch = Q.Batch != null ? Q.Batch : null,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.MIME).Result,
                Rating = Q.Ratings != null ? Q.Ratings : (double?)null,
                IsPopular = Q.IsPopular,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
                Top5Course = Q.Top5Course,
                WhoTookWhoLike = Q.WhoTookWhoLike
            });

            switch (filter.SortBy)
            {
                case "date":
                    dataLearning = dataLearning.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "dateDesc":
                    dataLearning = dataLearning.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case "name":
                    dataLearning = dataLearning.OrderBy(Q => Q.TrainingName);
                    break;
                case "nameDesc":
                    dataLearning = dataLearning.OrderByDescending(Q => Q.TrainingName);
                    break;
                case "popular":
                    dataLearning = dataLearning.OrderBy(Q => Q.Top5Course).ThenByDescending(Q => Q.WhoTookWhoLike).ThenByDescending(Q => Q.CreatedAt);
                    break;
                case "rating":
                    dataLearning = dataLearning.OrderBy(Q => Q.Rating);
                    break;
                case "ratingDesc":
                    dataLearning = dataLearning.OrderByDescending(Q => Q.Rating);
                    break;
                default:
                    dataLearning = dataLearning.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var queryResult = await dataLearning.Skip((int)skipCount).Take(filter.PageSize).ToListAsync();

            var result = queryResult.Select(Q => new LearningViewModel
            {
                TrainingName = Q.TrainingName,
                SetupModuleId = Q.SetupModuleId,
                ProgramTypeName = Q.ProgramTypeName,
                TrainingId = Q.TrainingId != null ? Q.TrainingId : null,
                CourseId = Q.CourseId,
                TrainingBatch = Q.TrainingBatch,
                ImageUrl = Q.ImageUrl,
                Rating = Q.Rating,
                IsPopular = Q.IsPopular,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
            }).ToList();

            return result;
        }

        /// <summary>
        /// Get Learning Program Type, i.e. entry, mandatory, etc 
        /// </summary>
        /// <returns></returns>
        public List<TrainingProgramType> GetLearningProgramType()
        {
            var listData = new List<TrainingProgramType>
            {
                new TrainingProgramType{ProgramTypeid = LearningProgramType.EntryId, ProgramTypeName = LearningProgramType.Entry },
                new TrainingProgramType{ProgramTypeid = LearningProgramType.MandatoryId, ProgramTypeName = LearningProgramType.Mandatory },
                new TrainingProgramType{ProgramTypeid = LearningProgramType.ThematicId, ProgramTypeName = LearningProgramType.Thematic },
                new TrainingProgramType{ProgramTypeid = LearningProgramType.InstructorId, ProgramTypeName = LearningProgramType.Instructor }
            };

            return listData;
        }

        /// <summary>
        /// Get Learning Type, i.e. online or offline or both(online and offline)
        /// </summary>
        /// <returns></returns>
        public List<TrainingLearningType> GetLearningType()
        {
            var listData = new List<TrainingLearningType>
            {
                new TrainingLearningType{LearningTypeId = LearningType.OnlineId, LearningTypeName = LearningType.Online },
                new TrainingLearningType{LearningTypeId = LearningType.OfflineId, LearningTypeName = LearningType.Offline },
                new TrainingLearningType{LearningTypeId = LearningType.BothId, LearningTypeName = LearningType.Both },
            };

            return listData;
        }

        /// <summary>
        /// Get Material Type, i.e.
        /// </summary>
        /// <returns></returns>
        public List<TrainingMaterialType> GetMaterialType()
        {
            var listData = new List<TrainingMaterialType>
            {
                new TrainingMaterialType{MaterialTypeId = LearningMaterialType.PodcastId, MaterialTypeName = LearningMaterialType.Podcast },
                new TrainingMaterialType{MaterialTypeId = LearningMaterialType.VideoId, MaterialTypeName = LearningMaterialType.Video },
                new TrainingMaterialType{MaterialTypeId = LearningMaterialType.JournalId, MaterialTypeName = LearningMaterialType.Journal },
                new TrainingMaterialType{MaterialTypeId = LearningMaterialType.GameId, MaterialTypeName = LearningMaterialType.Game }
            };

            return listData;
        }

        /// <summary>
        /// Get Learning Price, free or paid
        /// </summary>
        /// <returns></returns>
        public List<TrainingPriceType> GetLearningPriceType()
        {
            var listData = new List<TrainingPriceType>
            {
                new TrainingPriceType{TrainingPriceId = LearningPriceType.FreeId, TrainingPrice = LearningPriceType.Free },
                new TrainingPriceType{TrainingPriceId = LearningPriceType.PaidId, TrainingPrice = LearningPriceType.Paid }
            };

            return listData;
        }

        /// <summary>
        /// Get All Coach Data
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<CoachViewModel>> GetAllCoach(CoachFilter filter)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var query = this.DB.GetCoachSearchList().AsQueryable();

            if (!String.IsNullOrEmpty(filter.CoachName))
            {
                query = query.Where(Q => Q.EmployeeName.ToLower().Contains(filter.CoachName.ToLower()));
            }
            if (filter.AreaId != null)
            {
                query = query.Where(Q => filter.AreaId.Contains(Q.AreaId));
            }
            if (filter.CityId != null)
            {
                query = query.Where(Q => filter.CityId.Contains(Q.CityId));
            }
            if (filter.DealerId != null)
            {
                query = query.Where(Q => filter.DealerId.Contains(Q.DealerId));
            }
            if (filter.OutletId != null)
            {
                query = query.Where(Q => filter.OutletId.Contains(Q.OutletId));
            }
            if (filter.ProvinceId != null)
            {
                query = query.Where(Q => filter.ProvinceId.Contains(Q.ProvinceId));
            }
            if (filter.PositionId != null)
            {
                var listPositionCoach = await this.DB.GetPositionCoachSearchList(filter.PositionId).Select(Q => Q.CoachId).ToListAsync();
                query = query.Where(Q => listPositionCoach.Contains(Q.CoachId));
            }
            if (filter.TopicId != null)
            {
                var listTopicCoach = await this.DB.GetTopicCoachSearchList(filter.TopicId).Select(Q => Q.CoachId).ToListAsync();
                query = query.Where(Q => listTopicCoach.Contains(Q.CoachId));
            }
            if (filter.EBadgeId != null)
            {
                var listEbadgeCoach = await this.DB.GetEbadgeCoachSearchList(filter.EBadgeId).Select(Q => Q.CoachId).ToListAsync();
                query = query.Where(Q => listEbadgeCoach.Contains(Q.CoachId));
            }

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.EmployeeName);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.EmployeeName);
                    break;
            }

            var queryResult = await query.Select(Q => new
            {
                CoachId = Q.CoachId,
                EmployeeName = Q.EmployeeName,
                EmployeeId = Q.EmployeeId,
                PositionId = Q.PositionId,
                ImageUrl = Q.BlobId == null ? null : FileService.GetFileAsync(Q.BlobId.ToString(), Q.MIME).Result,
            }).Skip((int)skipCount).Take(filter.PageSize).ToListAsync();

            var listEmployeeId = queryResult.Select(Q => Q.EmployeeId).ToList();
            var listPosition = await this.DB.GetEmployeePositionAggregateList().Where(Q => listEmployeeId.Contains(Q.EmployeeId)).Select(Q => new
            {
                Q.PositionName,
                Q.EmployeeId
            }).ToListAsync();

            var data = queryResult.Select(Q => new CoachViewModel
            {
                CoachId = Q.CoachId,
                EmployeeName = Q.EmployeeName,
                EmployeeId = Q.EmployeeId,
                ImageUrl = Q.ImageUrl,
            }).ToList();

            foreach (var coach in data)
            {
                coach.TotalBadge = await this.BadgesService.GetTotalBadgeCoach(coach.EmployeeId);
                coach.Position = listPosition.Where(Q => Q.EmployeeId == coach.EmployeeId).Select(Q => Q.PositionName).FirstOrDefault();
            }

            return data;
        }

        /// <summary>
        /// Get All Area, i.e. Area 1 , Area 2 or Area 3
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<AreaModel>> GetArea(AreaFilter filter)
        {

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var query = (from a in DB.Areas
                         select new
                         {
                             a.AreaId,
                             a.Name
                         }).AsNoTracking();

            if (!String.IsNullOrEmpty(filter.AreaName) == false)
            {
                query = query.Where(Q => Q.Name.ToLower().Contains(filter.AreaName.ToLower()));
            }

            var data = await query.AsNoTracking().ToListAsync();

            var dataView = data
                .Select(Q => new AreaModel
                {
                    AreaId = Q.AreaId,
                    AreaName = Q.Name
                }).Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToList();

            return dataView;
        }

        /// <summary>
        /// Get All City, i.e. Jakarta, Tangerang
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<CityModel>> GetCity(CityFilter filter)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var query = (from c in DB.Cities
                         select new
                         {
                             c.CityId,
                             c.CityName
                         }).AsNoTracking();
            if (!String.IsNullOrEmpty(filter.CityName))
            {
                query = query.Where(Q => Q.CityName.ToLower() == filter.CityName.ToLower());
            }

            var data = await query
                .Select(Q => new CityModel
                {
                    CityId = Q.CityId,
                    CityName = Q.CityName
                }).Skip((int)skipCount)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return data;
        }

        /// <summary>
        /// Get All Dealer, i.e. Auto 2000, etc
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<DealerModel>> GetDealer(DealerFilter filter)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var query = (from d in DB.Dealers
                         select new
                         {
                             d.DealerId,
                             d.DealerName
                         }).AsNoTracking();

            if (!String.IsNullOrEmpty(filter.DealerName))
            {
                query = query.Where(Q => Q.DealerName.ToLower() == filter.DealerName.ToLower());
            }

            var data = await query
                .Select(Q => new DealerModel
                {
                    DealerId = Q.DealerId,
                    DealerName = Q.DealerName
                }).Skip((int)skipCount)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return data;
        }

        /// <summary>
        /// Get All People Data
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<PeopleViewModel>> GetAllPeople(PeopleFilter filter)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var query = this.DB.GetPeopleSearchList();

            if (!String.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(Q => Q.EmployeeName.ToLower().Contains(filter.Name.ToLower()));
            }
            if (filter.AreaId != null)
            {
                query = query.Where(Q => filter.AreaId.Contains(Q.AreaId));
            }
            if (filter.CityId != null)
            {
                query = query.Where(Q => filter.CityId.Contains(Q.CityId));
            }
            if (filter.DealerId != null)
            {
                query = query.Where(Q => filter.DealerId.Contains(Q.DealerId));
            }
            if (filter.OutletId != null)
            {
                query = query.Where(Q => filter.OutletId.Contains(Q.OutletId));
            }
            if (filter.ProvinceId != null)
            {
                query = query.Where(Q => filter.ProvinceId.Contains(Q.ProvinceId));
            }
            if (filter.PositionId != null)
            {
                var listFilterPositionId = filter.PositionId.Select(Q => "*" + Q + "*").ToList();
                query = query.Where(Q => listFilterPositionId.Contains(Q.PositionId));
            }

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.EmployeeName.Trim());
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.EmployeeName.Trim());
                    break;
                default:
                    query = query.OrderBy(Q => Q.EmployeeName.Trim());
                    break;
            }

            var data = await query.Select(Q => new PeopleViewModel
            {
                EmployeeId = Q.EmployeeId,
                EmployeeName = Q.EmployeeName.Trim(),
                ImageUrl = Q.BlobId == null ? null : FileService.GetFileAsync(Q.BlobId.ToString(), Q.MIME).Result
            }).Skip((int)skipCount).Take(filter.PageSize).ToListAsync();

            var listEmployeeId = data.Select(Q => Q.EmployeeId).ToList();
            var listPosition = await this.DB.GetEmployeePositionAggregateList().Where(Q => listEmployeeId.Contains(Q.EmployeeId)).Select(Q => new
            {
                Q.PositionName,
                Q.EmployeeId
            }).ToListAsync();

            foreach (var people in data)
            {
                people.Position = listPosition.Where(Q => Q.EmployeeId == people.EmployeeId).Select(Q => Q.PositionName).FirstOrDefault();
            }

            return data;
        }

        /// <summary>
        /// Get All Event Data
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<SingleViewEventModel>> GetAllEvent(EventFilter filter, string employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var employeeInformation = await (from e in this.DB.Employees
                                             join epm in this.DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                             where e.EmployeeId == employeeId
                                             select new
                                             {
                                                 epm.PositionId,
                                                 e.OutletId
                                             }).ToListAsync();

            var getOutletId = employeeInformation.Select(Q => Q.OutletId).FirstOrDefault();
            var getPositionId = employeeInformation.Select(Q => Q.PositionId.ToString()).ToList();

            if (filter.PositionId != null)
            {
                var existInFilter = filter.PositionId.Where(Q => getPositionId.Contains(Q)).ToList();
                if (existInFilter.Count > 0)
                {
                    getPositionId = existInFilter.Select(Q => Q).ToList();
                }
                else
                {
                    return new List<SingleViewEventModel>();
                }
            }

            var query = DB.GetEventSearch(employeeId, getOutletId, getPositionId).AsQueryable();

            if (filter.AreaId != null || filter.CityId != null || filter.DealerId != null || filter.OutletId != null || filter.ProvinceId != null || filter.EventCategoryId != null)
            {
                var newQuery = from eom in DB.EventOutletMappings
                               join o in DB.Outlets on eom.OutletId equals o.OutletId
                               join e in DB.Events on eom.EventId equals e.EventId
                               select new
                               {
                                   eom.EventId,
                                   o.AreaId,
                                   o.CityId,
                                   o.DealerId,
                                   o.OutletId,
                                   o.ProvinceId,
                                   e.EventCategoryId
                               };

                if (filter.AreaId != null)
                {
                    newQuery = newQuery.Where(Q => filter.AreaId.Contains(Q.AreaId));
                }
                if (filter.CityId != null)
                {
                    newQuery = newQuery.Where(Q => filter.CityId.Contains(Q.CityId));
                }
                if (filter.DealerId != null)
                {
                    newQuery = newQuery.Where(Q => filter.DealerId.Contains(Q.DealerId));
                }
                if (filter.OutletId != null)
                {
                    newQuery = newQuery.Where(Q => filter.OutletId.Contains(Q.OutletId));
                }
                if (filter.ProvinceId != null)
                {
                    newQuery = newQuery.Where(Q => filter.ProvinceId.Contains(Q.ProvinceId));
                }
                if (filter.EventCategoryId != null)
                {
                    newQuery = newQuery.Where(Q => filter.EventCategoryId.Contains(Q.EventCategoryId));
                }
                var listEventIds = await newQuery.Select(Q => Q.EventId).Distinct().ToListAsync();

                query = query.Where(Q => listEventIds.Contains(Q.EventId));
            }

            if (string.IsNullOrEmpty(filter.Name) == false)
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Name.ToLower()));
            }

            switch (filter.SortBy)
            {
                case "date":
                    query = query.OrderBy(Q => Q.StartDateTime);
                    break;
                case "dateDesc":
                    query = query.OrderByDescending(Q => Q.StartDateTime);
                    break;
                case "name":
                    query = query.OrderBy(Q => Q.Title);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.Title);
                    break;
                case "rating":
                    // ini order by count isSaved
                    query = query.OrderBy(Q => Q.TotalSave);
                    break;
                case "ratingDesc":
                    // ini order desc by count isSaved
                    query = query.OrderByDescending(Q => Q.TotalSave);
                    break;
                case "popular":
                    // ini order by by count isJoined
                    query = query.OrderBy(Q => Q.TotalJoin);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var getList = await query.Select(Q => new SingleViewEventModel
            {
                EventId = Q.EventId,
                ImageUrl = Q.BlobId == null ? null : FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result,
                Title = Q.Title,
                Description = Q.Description,
                Location = Q.Location,
                StartDate = Q.StartDateTime,
                EndDate = Q.EndDateTime,
                StartTime = Q.Start.ToString("HH:mm"),
                EndTime = Q.End.ToString("HH:mm"),
                IsJoin = Q.IsJoin,
                IsSave = Q.IsSave,
            }).Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToListAsync();

            //getList = getList.Distinct().ToList();

            //getList = getList.Skip((int)skipCount)
            //.Take(filter.PageSize).ToList();

            //var newList = getList.GroupBy(Q => Q.EventId).Select(Q => Q.First()).Skip((int)skipCount)
            //    .Take(filter.PageSize).ToList();

            return getList;
        }

        /// <summary>
        /// Get All News Data
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<UserSideNewsModel>> GetNews(NewsFilter filter)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var listNews = await NewsQueryable(filter).ToListAsync();

            var news = listNews
                 .Select(async Q => new UserSideNewsModel
                 {
                     Author = Q.Author,
                     NewsCategoryId = Q.NewsCategoryId,
                     Description = Q.Description,
                     FileUrl = await FileService.GetFileAsync(Q.FileBlobId, Q.FileMime),
                     IsDownloadable = Q.IsDownloadable,
                     ApprovedAt = Q.ApprovedAt,
                     IsDeleted = Q.IsDeleted,
                     Link = Q.Link,
                     NewsCategoryName = Q.NewsCategoryName,
                     NewsId = Q.NewsId,
                     ThumbnailUrl = await FileService.GetFileAsync(Q.ThumbnailBlobId, Q.ThumbnailMime),
                     Title = Q.Title,
                     TotalView = Q.TotalView,
                     CreatedAt = Q.CreatedAt,
                     UpdatedAt = Q.UpdateAt
                 })
                 .Select(Q => Q.Result)
                 .Skip((int)skipCount)
                 .Take(filter.PageSize)
                 .ToList();

            return news;
        }

        /// <summary>
        /// Get All Insight
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<InsightModel>> GetInsight(InsightFilter filter, string employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var getTodayDate = DateTime.UtcNow;

            var getEmployeeInformation = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).FirstOrDefaultAsync();

            var query = (from s in DB.Surveys
                         join spm in DB.SurveyPositionMappings on s.SurveyId equals spm.SurveyId
                         join p in DB.Positions on spm.PositionId equals p.PositionId
                         join epm in DB.EmployeePositionMappings on
                         new { p.PositionId, EmployeeId = employeeId }
                         equals
                         new { epm.PositionId, epm.EmployeeId }
                         join som in DB.SurveyOutletMappings on s.SurveyId equals som.SurveyId

                         join sa in DB.SurveyAnswers on new { s.SurveyId, EmployeeId = employeeId } 
                         equals 
                         new { sa.SurveyId, sa.EmployeeId } into sas
                         from sa in sas.DefaultIfEmpty()

                         where s.IsDeleted == false
                            && s.ApprovedAt != null
                            && (getTodayDate >= s.StartDate && getTodayDate <= s.EndDate)
                            && (string.IsNullOrEmpty(getEmployeeInformation.OutletId) == false ? som.OutletId == getEmployeeInformation.OutletId : s.ApprovedAt != null)
                            && sa.SurveyId == null
                         select new
                         {
                             s.SurveyId,
                             s.Title,
                             s.EndDate,
                             s.ApprovedAt,
                             s.UpdatedAt
                         }).Distinct().AsNoTracking();

            if (!String.IsNullOrEmpty(filter.SurveyName))
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.SurveyName.ToLower()));
            }

            if (filter.AreaId != null || filter.CityId != null || filter.DealerId != null || filter.OutletId != null || filter.ProvinceId != null)
            {
                var newQuery = from som in DB.SurveyOutletMappings
                               join o in DB.Outlets on som.OutletId equals o.OutletId
                               select new
                               {
                                   som.SurveyId,
                                   o.AreaId,
                                   o.CityId,
                                   o.DealerId,
                                   o.OutletId,
                                   o.ProvinceId
                               };

                if (filter.AreaId != null)
                {
                    newQuery = newQuery.Where(Q => filter.AreaId.Contains(Q.AreaId));
                }
                if (filter.CityId != null)
                {
                    newQuery = newQuery.Where(Q => filter.CityId.Contains(Q.CityId));
                }
                if (filter.DealerId != null)
                {
                    newQuery = newQuery.Where(Q => filter.DealerId.Contains(Q.DealerId));
                }
                if (filter.OutletId != null)
                {
                    newQuery = newQuery.Where(Q => filter.OutletId.Contains(Q.OutletId));
                }
                if (filter.ProvinceId != null)
                {
                    newQuery = newQuery.Where(Q => filter.ProvinceId.Contains(Q.ProvinceId));
                }

                var listSurveyIds = await newQuery.Select(Q => Q.SurveyId).Distinct().ToListAsync();

                query = query.Where(Q => listSurveyIds.Contains(Q.SurveyId));
            }

            switch (filter.SortBy)
            {
                case "date":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "dateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                case "name":
                    query = query.OrderBy(Q => Q.Title);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.Title);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var data = await query
                .Select(Q => new InsightModel
                {
                    SurveyId = Q.SurveyId,
                    SurveyTitle = Q.Title,
                    SurveyDueDate = Q.EndDate,
                }).AsNoTracking()
                .Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToListAsync();

            return data;
        }

        /// <summary>
        /// Query for get news data
        /// </summary>
        /// <returns></returns>
        public IQueryable<UserSideNewsQueryModel> NewsQueryable(NewsFilter filter)
        {
            var query = from n in this.DB.News.AsNoTracking()
                        join nc in this.DB.NewsCategories.AsNoTracking() on n.NewsCategoryId equals nc.NewsCategoryId
                        join bf in this.DB.Blobs.AsNoTracking() on n.FileBlobId equals bf.BlobId
                        join bt in this.DB.Blobs.AsNoTracking() on n.ThumbnailBlobId equals bt.BlobId
                        where n.IsDeleted == false && n.ApprovedAt != null
                        select new UserSideNewsQueryModel
                        {
                            Author = n.Author,
                            NewsCategoryId = n.NewsCategoryId,
                            Description = n.Description,
                            FileBlobId = n.FileBlobId.ToString(),
                            FileMime = bf.Mime,
                            IsDownloadable = n.IsDownloadable,
                            ApprovedAt = n.ApprovedAt,
                            IsDeleted = n.IsDeleted,
                            Link = n.Link,
                            NewsCategoryName = nc.Name,
                            NewsId = n.NewsId,
                            ThumbnailBlobId = n.ThumbnailBlobId.ToString(),
                            ThumbnailMime = bt.Mime,
                            Title = n.Title,
                            TotalView = n.TotalView,
                            CreatedAt = n.CreatedAt,
                            IsInternal = n.IsInternal,
                            UpdateAt = n.UpdatedAt
                        };
            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.Title);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.Title);
                    break;
                case "dateDesc":
                    query = query.OrderByDescending(Q => Q.UpdateAt);
                    break;
                case "date":
                    query = query.OrderBy(Q => Q.UpdateAt);
                    break;
                case "popular":
                    query = query.OrderByDescending(Q => Q.TotalView);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdateAt);
                    break;
            }

            if (!String.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Name.ToLower()) || Q.Author.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.NewsCategoryIds != null)
            {
                query = query.Where(Q => filter.NewsCategoryIds.Contains(Q.NewsCategoryId));
            }
            if (filter.NewsInternalExternalTypeIds != null)
            {
                query = query.Where(Q => filter.NewsInternalExternalTypeIds.Contains(Q.IsInternal ? 1 : 0));
            }

            return query;
        }
    }
}
