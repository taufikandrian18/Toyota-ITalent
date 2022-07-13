using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideMyLearningService : Controller
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly UserSideCertificateService CertificateMan;
        private readonly CourseService CourseMan;

        public UserSideMyLearningService(TalentContext context, IFileStorageService fileService, UserSideCertificateService userSideCertificateService, CourseService courseService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.CertificateMan = userSideCertificateService;
            this.CourseMan = courseService;
        }

        /// <summary>
        /// GEt all Course + pagination
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetAllUserSideTrainings(int pageSize, int pageIndex)
        {

            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var releaseTraining = await this.DB.Trainings
                .Where(Q => Q.IsDeleted == false && Q.ApprovedAt != null)
                .Select(Q => Q.CourseId).ToListAsync();

            var releaseLearning = await this.DB.SetupLearning
                .Where(Q => Q.ApprovalStatus.ToLower() == "approved")
                .Select(Q => Q.CourseId.GetValueOrDefault()).ToListAsync();

            var tempList = new List<int>();
            tempList.AddRange(releaseTraining);
            tempList.AddRange(releaseLearning);

            var query2 = await (from c in DB.Courses

                                join pt in DB.ProgramTypes on c.ProgramTypeId equals pt.ProgramTypeId
                                join b in DB.Blobs on c.BlobId equals b.BlobId

                                join t in DB.Trainings on c.CourseId equals t.CourseId

                                join atc in DB.ApprovalToCourses on c.CourseId equals atc.CourseId

                                join a in DB.Approvals on atc.ApprovalId equals a.ApprovalId

                                where a.ApprovalStatusId == 1 && tempList.Contains(c.CourseId) && t.IsDeleted == false

                                orderby t.ApprovedAt descending
                                select new
                                {
                                    t.TrainingId,
                                    t.Batch,
                                    t.StartDate,
                                    t.EndDate,
                                    c.CourseId,
                                    c.CourseName,
                                    pt.ProgramTypeName,
                                    c.BlobId,
                                    b.Mime,
                                    c.CreatedAt,
                                    c.UpdatedAt
                                }).AsNoTracking()
                                  .ToListAsync();

            var totalData = query2.Count();

            var dataCourses = query2
            .Select(async x => new UserSideMyLearningHomepageCourseModel
            {
                TrainingName = x.CourseName,
                ProgramTypeName = x.ProgramTypeName,
                TrainingId = x.TrainingId,
                TrainingBatch = x.Batch,
                ImageUrl = await FileService.GetFileAsync(x.BlobId.ToString(), x.Mime),
                Rating = await GenerateRatingTraining(x.TrainingId),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .Select(Q => Q.Result)
            .Skip((int)skipCount)
            .Take(pageSize)
            .ToList();

            return dataCourses;
        }

        public async Task<bool> GenerateIsAllowEnrolled(int trainingId, int courseId)
        {
            var training = await this.DB.Trainings
                .Where(Q => Q.IsDeleted == false && Q.ApprovedAt != null && Q.TrainingId == trainingId)
                .Select(Q => new TrainingProcessModel
                {
                    TrainingStartDate = Q.StartDate,
                    TrainingEndDate = Q.EndDate
                }).FirstOrDefaultAsync();

            var course = await CourseMan.GetCourseById(courseId);

            if (training != null && course != null)
            {
                if (training.TrainingStartDate >= DateTime.Now || training.TrainingEndDate <= DateTime.Now)
                {
                    //untuk Training Process = no
                    if ((course.CoursePrice <= 0 && course.LearningTypeId == 1) || (course.CoursePrice <= 0 && course.LearningTypeId == 3))
                    {
                        return true;
                    }
                    //untuk Training Process = yes
                    else
                    {
                        if (training.TrainingStartDate >= DateTime.Now || training.TrainingEndDate <= DateTime.Now)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetPaginatedResultUserSidePopularTrainings(int pageSize, int pageIndex, string employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var employeeInformation = await (from e in this.DB.Employees
                                             join epm in this.DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                             where e.EmployeeId == employeeId
                                             select new
                                             {
                                                 epm.PositionId,
                                                 e.OutletId
                                             }).ToListAsync();

            var getOutletId = employeeInformation.Select(Q => Q.OutletId).FirstOrDefault();
            var getPositionId = employeeInformation.Select(Q => Q.PositionId).ToList();
            List<GetPopularLearningModel> paginatedResult;

            //orang dealer
            if (getOutletId != null)
            {
                //paginatedResult = await DB.GetPopularLearningModel((int)skipCount, pageSize, getOutletId, getPositionId).ToListAsync();
                paginatedResult = await DB.GetPopularLearningDealer((int)skipCount, pageSize, getOutletId, getPositionId).ToListAsync();
                //paginatedResult = new List<GetPopularLearningModel>();

            }
            //orang TAM
            else
            {
                //paginatedResult = await DB.GetPopularLearningModel((int)skipCount, pageSize, getPositionId).ToListAsync();
                paginatedResult = await DB.GetPopularLearningTAM((int)skipCount, pageSize, getPositionId).ToListAsync();
                //paginatedResult = new List<GetPopularLearningModel>();
            }

            var dataCourses = new List<UserSideMyLearningHomepageCourseModel>();

            foreach (var data in paginatedResult)
            {
                double preDataPercentage = 0;
                double duringDataPercentage = 0;
                double postDataPercentage = 0;
                double totalDataPercentage = 0;

                bool preNA = false;
                bool duringNA = false;
                bool postNA = false;

                var elData = new List<EnrollLearningTimes>();
                var tempElData = await this.DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).ThenInclude(x => x.Assesment).Include(x => x.SetupModule).ThenInclude(x => x.Module).Where(x => x.EnrollLearning.EmployeeId == employeeId).Where(x => x.EnrollLearning.TrainingId == data.TrainingId).OrderBy(x => x.SetupModule.SetupModuleId).ToListAsync();
                if(tempElData.Count() > 0)
                {
                    foreach (var datum in tempElData)
                    {
                        if (!elData.Select(x => x.SetupModuleId).ToList().Contains(datum.SetupModuleId))
                        {
                            elData.Add(datum);
                        }
                    }
                    var preData = elData.Where(x => x.SetupModule.TrainingTypeId == 1).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var duringData = elData.Where(x => x.SetupModule.TrainingTypeId == 2).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var postData = elData.Where(x => x.SetupModule.TrainingTypeId == 3).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    if (preData.Count() > 0)
                    {
                        preDataPercentage = ((double)preData.Where(x => x.Status == 3).Count() / (double)preData.Count()) * 100;
                    }
                    else
                    {
                        preNA = true;
                    }
                    if (duringData.Count() > 0)
                    {
                        duringDataPercentage = ((double)duringData.Where(x => x.Status == 3).Count() / (double)duringData.Count()) * 100;
                    }
                    else
                    {
                        duringNA = true;
                    }
                    if (postData.Count() > 0)
                    {
                        postDataPercentage = ((double)postData.Where(x => x.Status == 3).Count() / (double)postData.Count()) * 100;
                    }
                    else
                    {
                        postNA = true;
                    }

                    totalDataPercentage = (((double)postData.Where(x => x.Status == 3).Count() + (double)duringData.Where(x => x.Status == 3).Count() + (double)preData.Where(x => x.Status == 3).Count()) / (preData.Count() + duringData.Count() + postData.Count())) * 100;
                }

                var dataCourse = new UserSideMyLearningHomepageCourseModel
                {
                    TrainingName = data.CourseName == null ? data.ModuleName : data.CourseName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    CourseId = data.CourseId,
                    MaterialTypeName = data.MaterialTypeName,
                    SetupModuleId = data.SetupModuleId,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    ApprovedAt = data.ApprovedAt,
                    Percentage = Math.Round(totalDataPercentage, 2),
                    Price = DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId).CoursePrice,
                    Points = DB.SetupModules.Include(X => X.TimePoint).Where(X => X.CourseId == data.CourseId) == null ? 0 : DB.SetupModules.Include(X => X.TimePoint).Where(X => X.CourseId == data.CourseId).Sum(Y => Y.TimePoint.Points),
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }


        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetPaginatedResultUserSideLatestTrainings(int pageSize, int pageIndex, string employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var employeeInformation = await (from e in this.DB.Employees
                                             join epm in this.DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                             where e.EmployeeId == employeeId
                                             select new
                                             {
                                                 epm.PositionId,
                                                 e.OutletId
                                             }).ToListAsync();

            var getOutletId = employeeInformation.Select(Q => Q.OutletId).FirstOrDefault();
            var getPositionId = employeeInformation.Select(Q => Q.PositionId).ToList();
            List<GetPopularLearningModel> paginatedResult;

            //orang dealer
            if (getOutletId != null)
            {
                //paginatedResult = await DB.GetLatestLearningModel((int)skipCount, pageSize, getOutletId, getPositionId).ToListAsync();
                paginatedResult = await DB.GetLatestLearningDealer((int)skipCount, pageSize, getOutletId, getPositionId).ToListAsync();
                //paginatedResult = new List<GetPopularLearningModel>();

            }
            //orang TAM
            else
            {
                //paginatedResult = await DB.GetLatestLearningModel((int)skipCount, pageSize, getPositionId).ToListAsync();
                paginatedResult = await DB.GetLatestLearningTAM((int)skipCount, pageSize, getPositionId).ToListAsync();
                //paginatedResult = new List<GetPopularLearningModel>();

            }

            var dataCourses = new List<UserSideMyLearningHomepageCourseModel>();

            foreach (var data in paginatedResult)
            {
                double preDataPercentage = 0;
                double duringDataPercentage = 0;
                double postDataPercentage = 0;
                double totalDataPercentage = 0;

                bool preNA = false;
                bool duringNA = false;
                bool postNA = false;

                var elData = new List<EnrollLearningTimes>();
                var tempElData = await this.DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).ThenInclude(x => x.Assesment).Include(x => x.SetupModule).ThenInclude(x => x.Module).Where(x => x.EnrollLearning.EmployeeId == employeeId).Where(x => x.EnrollLearning.TrainingId == data.TrainingId).OrderBy(x => x.SetupModule.SetupModuleId).ToListAsync();
                if(tempElData.Count() > 0)
                {
                    foreach (var datum in tempElData)
                    {
                        if (!elData.Select(x => x.SetupModuleId).ToList().Contains(datum.SetupModuleId))
                        {
                            elData.Add(datum);
                        }
                    }
                    var preData = elData.Where(x => x.SetupModule.TrainingTypeId == 1).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var duringData = elData.Where(x => x.SetupModule.TrainingTypeId == 2).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var postData = elData.Where(x => x.SetupModule.TrainingTypeId == 3).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    if (preData.Count() > 0)
                    {
                        preDataPercentage = ((double)preData.Where(x => x.Status == 3).Count() / (double)preData.Count()) * 100;
                    }
                    else
                    {
                        preNA = true;
                    }
                    if (duringData.Count() > 0)
                    {
                        duringDataPercentage = ((double)duringData.Where(x => x.Status == 3).Count() / (double)duringData.Count()) * 100;
                    }
                    else
                    {
                        duringNA = true;
                    }
                    if (postData.Count() > 0)
                    {
                        postDataPercentage = ((double)postData.Where(x => x.Status == 3).Count() / (double)postData.Count()) * 100;
                    }
                    else
                    {
                        postNA = true;
                    }

                    totalDataPercentage = (((double)postData.Where(x => x.Status == 3).Count() + (double)duringData.Where(x => x.Status == 3).Count() + (double)preData.Where(x => x.Status == 3).Count()) / (preData.Count() + duringData.Count() + postData.Count())) * 100;
                }

                var dataCourse = new UserSideMyLearningHomepageCourseModel
                {
                    TrainingName = data.CourseName == null ? data.ModuleName : data.CourseName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    CourseId = data.CourseId,
                    MaterialTypeName = data.MaterialTypeName,
                    SetupModuleId = data.SetupModuleId,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    ApprovedAt = data.ApprovedAt,
                    Percentage = Math.Round(totalDataPercentage, 2),
                    Price = DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId).CoursePrice,
                    Points = DB.SetupModules.Include(X => X.TimePoint).Where(X => X.CourseId == data.CourseId) == null ? 0 : DB.SetupModules.Include(X => X.TimePoint).Where(X => X.CourseId == data.CourseId).Sum(Y => Y.TimePoint.Points),
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }


        private async Task<double?> GenerateRatingTraining(double? trainingId)
        {

            if (trainingId == null)
            {
                return null;
            }

            var queryTrainingRatings = await this.DB.TrainingRatings.Where(Q => Q.TrainingId == trainingId).Select(Q => Q.RatingScore).ToListAsync();

            double? trainingRatings = 0.0;

            if (queryTrainingRatings.Count() > 0)
            {
                trainingRatings = Convert.ToDouble(queryTrainingRatings.Sum() / queryTrainingRatings.Count());
            }

            var temp = Math.Round((decimal)trainingRatings, 1, MidpointRounding.AwayFromZero);
            trainingRatings = (double?)temp;
            return trainingRatings;

        }

        //public async Task<List<UserSideFreePopularCoursesModel>> GetAllUserSidePopularCourses(int pageSize, int pageIndex)
        //{
        //    var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

        //    var releaseLearning = await this.DB.SetupLearning
        //        .Where(Q => Q.ApprovalStatus.ToLower() == "approved")
        //        .Select(Q => Q.CourseId.GetValueOrDefault()).ToListAsync();

        //    var tempList = new List<int>();
        //    tempList.AddRange(releaseLearning);

        //    var query2 = await (from c in DB.Courses

        //                        join pt in DB.ProgramTypes on c.ProgramTypeId equals pt.ProgramTypeId
        //                        join b in DB.Blobs on c.BlobId equals b.BlobId

        //                        join atc in DB.ApprovalToCourses on c.CourseId equals atc.CourseId

        //                        join a in DB.Approvals on atc.ApprovalId equals a.ApprovalId

        //                        where c.SetupCourseApprovedAt != null && tempList.Contains(c.CourseId) && c.IsDeleted == false && c.IsPopular == true
        //                        orderby c.SetupCourseApprovedAt descending
        //                        select new
        //                        {
        //                            c.CourseId,
        //                            c.CourseName,
        //                            pt.ProgramTypeName,
        //                            c.BlobId,
        //                            b.Mime,
        //                            c.CreatedAt,
        //                            c.UpdatedAt
        //                        }).AsNoTracking()
        //                          .ToListAsync();

        //    var totalData = query2.Count();

        //    var dataCourses = query2
        //        .GroupBy(Q => Q.CourseId)
        //    .Select(async x => new UserSideFreePopularCoursesModel
        //    {
        //        CourseID = x.First().CourseId,
        //        CoursesName = x.First().CourseName,
        //        ImageUrl = await FileService.GetFileAsync(x.First().BlobId.ToString(), x.First().Mime),
        //        ProgramTypeName = x.First().ProgramTypeName
        //    })
        //    .Select(Q => Q.Result)
        //    .Skip((int)skipCount)
        //    .Take(pageSize)
        //    .ToList();

        //    return dataCourses;
        //}

        public async Task<List<UserSidePopQuizModel>> GetAllPopQuizFromSetupLearning(int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            //var initQuery = await this
            //    .DB
            //    .SetupLearning
            //    .Where(Q => Q.PopQuiz.IsDeleted == false && Q.PopQuiz.ApprovedAt != null)
            //    .Select(Q => Q.PopQuizId == null? 0 : Q.PopQuizId)
            //    .ToListAsync();

            var query = (from sl in DB.SetupLearning
                         join pq in DB.PopQuizzes on sl.PopQuizId equals pq.PopQuizId into slpq
                         from pq in slpq.DefaultIfEmpty()

                         join st in DB.SetupTasks on pq.PopQuizId equals st.PopQuizId
                         where pq.IsDeleted == false && pq.ApprovedAt != null
                         select new UserSidePopQuizModel
                         {
                             PopQuizId = pq.PopQuizId,
                             PopQuizName = pq.PopQuizName,
                             CreatedAt = pq.CreatedAt,
                             TestTime = st.TestTime,
                         })
                         .Skip((int)skipCount)
                         .Take(pageSize)
                         .AsNoTracking();

            var popQuizes = await query.Select(Q => new UserSidePopQuizModel
            {
                PopQuizId = Q.PopQuizId,
                PopQuizName = Q.PopQuizName,
                CreatedAt = Q.CreatedAt,
                TestTime = Q.TestTime,
            }).ToListAsync();

            return popQuizes;
        }

        public async Task<bool> InsertTaskAnswer(TaskAnswerInsertModel TaskAnswers, string employeeId)
        {
            var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();
            //TaskAnswer
            //SIMPEN JAWABAN DI DAN ANSWER
            var insertTaskAnswer = new TaskAnswers
            {
                EmployeeId = employeeId,
                SetupModuleId = null,
                PopQuizId = TaskAnswers.PopQuizId,
                TrainingId = TaskAnswers.TrainingId,
                CreatedAt = dateNow,
            };
            this.DB.TaskAnswers.Add(insertTaskAnswer);


            //TaskAnswerDetail
            string blobId = null;
            var insertAnswerDetail = new List<TaskAnswerDetails>();

            var totalPoint = 0;
            //SIMPEN JAWABAN DI DAN ANSWER DETAIL
            foreach (var Insert in TaskAnswers.Answer)
            {
                totalPoint = totalPoint + Insert.Point;

                switch (Insert.QuestionTypeId)
                {
                    case 11:
                        if (Insert.File.Base64String == "" || Insert.File.Base64String == null)
                        {
                            this.DB.Add(new TaskAnswerDetails
                            {
                                TaskAnswerId = insertTaskAnswer.TaskAnswerId,
                                TaskId = Insert.TaskId,
                                Answer = Insert.Answer,
                                AnswerBlobId = null,
                                Score = Insert.Score,
                                Point = Insert.Point,
                                IsTrue = Insert.IsTrue.GetValueOrDefault(),
                                IsChecked = Insert.IsCheck.GetValueOrDefault(),
                            });
                            break;
                        }
                        string fileName, fileExtension;
                        fileName = Insert.File.FileName;
                        fileExtension = Insert.File.ContextType;
                        var newGuid = this.FileService.InsertBlob(fileName, fileExtension);
                        fileName = newGuid.ToString();

                        Byte[] bytes = Convert.FromBase64String(Insert.File.Base64String);
                        using (MemoryStream fileStream = new MemoryStream(bytes))
                        //using (var fileStream = Insert.File.Files[0].OpenReadStream())
                        {
                            await this.FileService.UploadFile(fileName, fileExtension, fileStream);
                        }
                        blobId = fileName;

                        this.DB.Add(new TaskAnswerDetails
                        {
                            TaskAnswerId = insertTaskAnswer.TaskAnswerId,
                            TaskId = Insert.TaskId,
                            Answer = Insert.Answer,
                            AnswerBlobId = Guid.Parse(blobId),
                            Score = Insert.Score,
                            Point = Insert.Point,
                            IsTrue = Insert.IsTrue.GetValueOrDefault(),
                            IsChecked = Insert.IsCheck.GetValueOrDefault(),
                        }
                        );
                        break;

                    case 12:
                    case 9:
                    case 8:
                    case 2:
                        //Detail
                        var detail = new TaskAnswerDetails
                        {
                            TaskAnswerId = insertTaskAnswer.TaskAnswerId,
                            TaskId = Insert.TaskId,
                            Answer = Insert.Answer,
                            Score = Insert.Score,
                            Point = Insert.Point,
                            IsTrue = Insert.IsTrue.GetValueOrDefault(),
                            IsChecked = Insert.IsCheck.GetValueOrDefault(),
                        };
                        this.DB.Add(detail);


                        //Special
                        var TaskSpecialAnswers = new List<TaskSpecialAnswers>();
                        foreach (var specialTaskAnswer in Insert.Special)
                        {
                            var specialAnswers = new TaskSpecialAnswers
                            {
                                TaskAnswerDetailId = detail.TaskAnswerDetailId,
                                Number = specialTaskAnswer.Number,
                                Answer = specialTaskAnswer.Answer,
                                Score = specialTaskAnswer.Score,
                                Point = specialTaskAnswer.Point,
                                IsTrue = specialTaskAnswer.IsTrue,
                            };
                            TaskSpecialAnswers.Add(specialAnswers);
                        }
                        this.DB.AddRange(TaskSpecialAnswers);
                        break;

                    default:
                        this.DB.Add(new TaskAnswerDetails
                        {
                            TaskAnswerId = insertTaskAnswer.TaskAnswerId,
                            TaskId = Insert.TaskId,
                            Answer = Insert.Answer,
                            Score = Insert.Score,
                            Point = Insert.Point,
                            IsTrue = Insert.IsTrue.GetValueOrDefault(),
                            IsChecked = Insert.IsCheck.GetValueOrDefault(),
                        }
                        );
                        break;
                }
            }

            var enrollLearnings = await this.DB.EnrollLearnings.Where(Q => Q.EmployeeId == employeeId && Q.TrainingId == TaskAnswers.TrainingId).FirstOrDefaultAsync();

            var eltimes = await this.DB.EnrollLearningTimes.Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.EnrollLearningId == enrollLearnings.EnrollLearningId).FirstOrDefaultAsync();

            eltimes.EndTime = DateTime.UtcNow.ToIndonesiaTimeZone();
            var employeeTime = Convert.ToInt16((eltimes.EndTime?.Subtract(eltimes.StartTime.Value).TotalMinutes));

            var maximalTime = await (from sm in this.DB.SetupModules
                                     join tp in this.DB.TimePoints on sm.TimePointId equals tp.TimePointId
                                     select new UserSideTimeAndPointModel
                                     {
                                         Point = tp.Points,
                                         Time = tp.Time
                                     }).FirstOrDefaultAsync();
            //Check dapet bonus atau tidak
            if (employeeTime <= maximalTime.Time)
            {
                totalPoint = totalPoint + maximalTime.Point.GetValueOrDefault();
            }

            //Nambahin point history
            var data = new EmployeePointHistories
            {
                EmployeeId = employeeId,
                RewardPointTypeId = 1,
                PointTransactionTypeId = 1,
                Point = totalPoint,
                CreatedAt = DateTime.UtcNow.AddHours(7),
            };
            this.DB.Add(data);

            //Nambahin exp dan point
            var employee = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).FirstOrDefaultAsync();
            employee.EmployeeExperience = employee.EmployeeExperience + totalPoint;
            employee.LearningPoint = employee.LearningPoint + totalPoint;

            var checkCourse = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId).FirstOrDefaultAsync();

            //KALAU BUKAN COURSE MAKA GAK ADA REMED
            if (checkCourse.CourseId == null || TaskAnswers.PopQuizId != null)
            {
                await DB.SaveChangesAsync();
                return true;
            }

            //CHECK TASK REMED ATAU TASK BIASA
            var remedLevel = await this.DB.EnrollLearnings.Where(Q => Q.TrainingId == TaskAnswers.TrainingId && Q.EmployeeId == employeeId).Select(Q => Q.RemedialLevel).FirstOrDefaultAsync();
            if (remedLevel == 0)
            {
                //CHECK REMED APA ENGGA
                var allSetupModule = await (from t in this.DB.Trainings
                                            join sm in this.DB.SetupModules on t.CourseId equals sm.CourseId
                                            where t.TrainingId == TaskAnswers.TrainingId
                                            select sm.SetupModuleId
                                            ).ToListAsync();

                var numberofModuleFinish = await (from t in this.DB.Trainings
                                                  join el in this.DB.EnrollLearnings on t.TrainingId equals el.TrainingId
                                                  join elt in this.DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId
                                                  join sm in this.DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId
                                                  where t.TrainingId == TaskAnswers.TrainingId && el.EmployeeId == employeeId && elt.EndTime != null && sm.IsForRemedial == false
                                                  select sm.MinimumScore.GetValueOrDefault()).ToListAsync();

                if (numberofModuleFinish.Count() != allSetupModule.Count())
                {
                    await DB.SaveChangesAsync();

                    return true;
                }

                var minimumPassingGrade = 0;

                foreach (var module in numberofModuleFinish)
                {
                    minimumPassingGrade = minimumPassingGrade + module;
                }

                //ambil semua score dari answer yang telah di jawab employee mulai dari module 1 sampai terakhir yang tidak remed
                var employeeScores = await (
                        from ta in this.DB.TaskAnswers
                        join tad in this.DB.TaskAnswerDetails on ta.TaskAnswerId equals tad.TaskAnswerId
                        where (allSetupModule.Contains(ta.SetupModuleId.GetValueOrDefault()) && ta.EmployeeId == employeeId)
                        select tad.Score.GetValueOrDefault()
                    ).ToListAsync();

                var employeeTotalScore = 0;

                foreach (var score in employeeScores)
                {
                    employeeTotalScore = employeeTotalScore + score;
                }

                if (employeeTotalScore < minimumPassingGrade)
                {
                    enrollLearnings.RemedialLevel = 1;
                }
            }
            else
            {
                var minimumPassingGrade = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId).Select(Q => Q.MinimumScore.GetValueOrDefault()).FirstOrDefaultAsync();

                //ambil semua score dari answer yang telah di jawab employee mulai dari module 1 sampai terakhir yang tidak remed
                var employeeScores = await (
                        from ta in this.DB.TaskAnswers
                        join tad in this.DB.TaskAnswerDetails on ta.TaskAnswerId equals tad.TaskAnswerId
                        where (ta.SetupModuleId.GetValueOrDefault() == TaskAnswers.SetupModuleId && ta.EmployeeId == employeeId)
                        select tad.Score.GetValueOrDefault()
                    ).ToListAsync();

                var employeeTotalScore = 0;

                foreach (var score in employeeScores)
                {
                    employeeTotalScore = employeeTotalScore + score;
                }
                if (employeeTotalScore < minimumPassingGrade)
                {
                    if (enrollLearnings.RemedialLevel < 3)
                    {
                        enrollLearnings.RemedialLevel++;
                    }
                }
            }
            //CHECK REMED APA TIDAK
            await DB.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetAllUserSideRecommendedTrainings(int pageSize, int pageIndex, string employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getTopicId = await DB.EmployeeInterests.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.TopicId).ToListAsync();

            var employeeOutletId = await DB.Employees
                                    .Where(Q => Q.EmployeeId == employeeId)
                                    .Select(Q => Q.OutletId)
                                    .FirstOrDefaultAsync();
            var positionIds = await DB.EmployeePositionMappings.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.PositionId).ToListAsync();
            
            
            IQueryable<GetRecommendedLearningModel> query;
            //List<GetRecommendedLearningModel> query;

            //--------------------------------------------------------
            //DITAMBAHKAN
            DB.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            //-------------------------------------------------------

            if (employeeOutletId != null)
            {
                // Orang Dealer
                //query = DB.GetRecommendedLearningModel(employeeOutletId, positionIds, getTopicId).AsQueryable();
                query = DB.GetRecommendedLearningDealer(employeeOutletId, positionIds, getTopicId).AsQueryable();
                //query = new List<GetRecommendedLearningModel>();
            }
            else
            {
                // Orang TAM
                //query = DB.GetRecommendedLearningModel(positionIds, getTopicId).AsQueryable();
                query = DB.GetRecommendedLearningTAM(positionIds, getTopicId).AsQueryable();
                //query = new List<GetRecommendedLearningModel>();
            }

            var totalData = await query.CountAsync();
            //var totalData = query.Count();

            var paginatedResult = await query.OrderByDescending(Q => Q.UpdatedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToListAsync();
            //var paginatedResult = query.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            //.Take(pageSize);

            var dataCourses = new List<UserSideMyLearningHomepageCourseModel>();

            foreach (var data in paginatedResult)
            {
                double preDataPercentage = 0;
                double duringDataPercentage = 0;
                double postDataPercentage = 0;
                double totalDataPercentage = 0;

                bool preNA = false;
                bool duringNA = false;
                bool postNA = false;

                var elData = new List<EnrollLearningTimes>();
                var tempElData = await this.DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).ThenInclude(x => x.Assesment).Include(x => x.SetupModule).ThenInclude(x => x.Module).Where(x => x.EnrollLearning.EmployeeId == employeeId).Where(x => x.EnrollLearning.TrainingId == data.TrainingId).OrderBy(x => x.SetupModule.SetupModuleId).ToListAsync();
                if(tempElData.Count() > 0)
                {
                    foreach (var datum in tempElData)
                    {
                        if (!elData.Select(x => x.SetupModuleId).ToList().Contains(datum.SetupModuleId))
                        {
                            elData.Add(datum);
                        }
                    }
                    var preData = elData.Where(x => x.SetupModule.TrainingTypeId == 1).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var duringData = elData.Where(x => x.SetupModule.TrainingTypeId == 2).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var postData = elData.Where(x => x.SetupModule.TrainingTypeId == 3).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(employeeId, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(employeeId, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    if (preData.Count() > 0)
                    {
                        preDataPercentage = ((double)preData.Where(x => x.Status == 3).Count() / (double)preData.Count()) * 100;
                    }
                    else
                    {
                        preNA = true;
                    }
                    if (duringData.Count() > 0)
                    {
                        duringDataPercentage = ((double)duringData.Where(x => x.Status == 3).Count() / (double)duringData.Count()) * 100;
                    }
                    else
                    {
                        duringNA = true;
                    }
                    if (postData.Count() > 0)
                    {
                        postDataPercentage = ((double)postData.Where(x => x.Status == 3).Count() / (double)postData.Count()) * 100;
                    }
                    else
                    {
                        postNA = true;
                    }

                    totalDataPercentage = (((double)postData.Where(x => x.Status == 3).Count() + (double)duringData.Where(x => x.Status == 3).Count() + (double)preData.Where(x => x.Status == 3).Count()) / (preData.Count() + duringData.Count() + postData.Count())) * 100;
                }

                var dataCourse = new UserSideMyLearningHomepageCourseModel
                {
                    TrainingName = data.CourseName == null ? data.ModuleName : data.CourseName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    CourseId = data.CourseId,
                    MaterialTypeName = data.MaterialTypeName,
                    SetupModuleId = data.SetupModuleId,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    Percentage = Math.Round(totalDataPercentage, 2),
                    Price = DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId).CoursePrice,
                    Points = DB.SetupModules.Include(X => X.TimePoint).FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.SetupModules.Include(X => X.TimePoint).FirstOrDefault(X => X.CourseId == data.CourseId).TimePoint.Points,
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        public int GetTrackingProgressStatusModule(string EmployeeId, int? ModuleId, int? SetupModuleId, int trainingId)
        {
            int returnValue = 1;
            var param = DB.FinalScores.OrderByDescending(x => x.CreatedAt).Where(x => x.TrainingId == trainingId).Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.ModuleId == ModuleId).Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            if (param != null)
            {
                if (param.PassedStatus)
                {
                    returnValue = 3;
                }
                else
                {
                    returnValue = 4;
                }
            }
            else
            {
                var containTask = DB.TaskAnswers.Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.TrainingId == trainingId).Count();
                if (containTask > 0)
                {
                    var answerData = DB.TaskAnswerDetails.Include(x => x.TaskAnswer).ThenInclude(x => x.SetupModule).ThenInclude(x => x.EnrollLearningTimes).Where(x => x.TaskAnswer.SetupModule.ModuleId == ModuleId).Where(x => x.TaskAnswer.EmployeeId == EmployeeId).Where(x => x.TaskAnswer.TrainingId == trainingId).FirstOrDefault();
                    if (answerData != null)
                    {
                        returnValue = 2;
                    }
                    else
                    {
                        returnValue = 1;
                    }
                }
                else
                {
                    var alreadyTaken = DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.EnrollLearning.TrainingId == trainingId).Where(x => x.EnrollLearning.EmployeeId == EmployeeId).FirstOrDefault();
                    if (alreadyTaken.EndTime != null)
                    {
                        returnValue = 3;
                    }
                    else
                    {
                        returnValue = 1;
                    }
                }
            }
            return returnValue;
        }

        public int GetTrackingProgressStatus(string EmployeeId, string AssesmentId, int? SetupModuleId, int trainigId)
        {
            int returnValue = 1;
            var enumByOption = DB.Assesments.Where(x => x.GUID == AssesmentId).FirstOrDefault().IsByOption;
            var skillCheckList = DB.LiveAssesmentSkillChecks.Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.TrainingId == trainigId).Select(x => x.SkillCheckGUID).ToList();
            if (enumByOption)
            {
                foreach (var skillCheckGUID in skillCheckList)
                {
                    var flag = DB.FinalScores.Include(x => x.Assesment).Where(x => x.SkillCheckGuid == skillCheckGUID).Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentId == AssesmentId).Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.EmployeeId == EmployeeId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    if (flag != null)
                    {
                        if (flag.Assesment.EnumScoringMethod.ToLower() == "latest")
                        {
                            var param = DB.FinalScores.Include(x => x.Assesment).Where(x => x.SkillCheckGuid == skillCheckGUID).Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentId == AssesmentId).Where(x => x.EmployeeId == EmployeeId).Where(x => x.SetupModuleId == SetupModuleId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                            if (param != null)
                            {
                                if (!param.PassedStatus)
                                {
                                    returnValue = 4;
                                    break;
                                }
                                else
                                {
                                    returnValue = 3;
                                }
                            }
                            else
                            {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (liveAssesment != null)
                                {
                                    returnValue = 2;
                                }
                                else
                                {
                                    returnValue = 1;
                                }
                            }
                        }
                        else if (flag.Assesment.EnumScoringMethod.ToLower() == "highest")
                        {
                            var param = DB.FinalScores.Include(x => x.Assesment).Where(x => x.SkillCheckGuid == skillCheckGUID).Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentId == AssesmentId).Where(x => x.EmployeeId == EmployeeId).Where(x => x.SetupModuleId == SetupModuleId).OrderByDescending(x => x.FinalScore).FirstOrDefault();
                            if (param != null)
                            {
                                if (!param.PassedStatus)
                                {
                                    returnValue = 4;
                                    break;

                                }
                                else
                                {
                                    returnValue = 3;
                                }
                            }
                            else
                            {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (liveAssesment != null)
                                {
                                    returnValue = 2;
                                }
                                else
                                {
                                    returnValue = 1;
                                }
                            }
                        }
                        else if (flag.Assesment.EnumScoringMethod.ToLower() == "average")
                        {
                            var param = DB.FinalScores.Include(x => x.Assesment).Where(x => x.SkillCheckGuid == skillCheckGUID).Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentId == AssesmentId).Where(x => x.EmployeeId == EmployeeId).Where(x => x.SetupModuleId == SetupModuleId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                            if (param != null)
                            {
                                if (!param.PassedStatus)
                                {
                                    returnValue = 4;
                                    break;
                                }
                                else
                                {
                                    returnValue = 3;
                                }
                            }
                            else
                            {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (liveAssesment != null)
                                {
                                    returnValue = 2;
                                }
                                else
                                {
                                    returnValue = 1;
                                }
                            }
                        }
                        else
                        {
                            var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                            if (liveAssesment != null)
                            {
                                returnValue = 2;
                            }
                            else
                            {
                                returnValue = 1;
                            }
                        }
                    }
                    else
                    {
                        var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        if (liveAssesment != null)
                        {
                            returnValue = 2;
                        }
                        else
                        {
                            returnValue = 1;
                        }
                    }
                }
            }
            else
            {
                var param = DB.FinalScores.OrderByDescending(x => x.CreatedAt).Where(x => x.TrainingId == trainigId).Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.AssesmentId == AssesmentId).Where(x => x.EmployeeId == EmployeeId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                if (param != null)
                {
                    if (param.PassedStatus)
                    {
                        returnValue = 3;
                    }
                    else
                    {
                        returnValue = 4;
                    }
                }
                else
                {
                    var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    if (liveAssesment != null)
                    {
                        returnValue = 2;
                    }
                    else
                    {
                        returnValue = 1;
                    }
                }
            }
            return returnValue;
        }

        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetAllUserSideQueuedTrainings(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getAllData = await DB.GetQueuedLearningModel(user).ToListAsync();
            var totalData = getAllData.Count();

            var paginatedResult = getAllData.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToList();

            var dataCourses = new List<UserSideMyLearningHomepageCourseModel>();

            foreach (var data in paginatedResult)
            {
                var dataCourse = new UserSideMyLearningHomepageCourseModel
                {
                    TrainingName = data.CourseName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    SetupModuleId = data.SetupModuleId,
                    CourseId = data.CourseId,
                    Price = DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId).CoursePrice,
                    Points = DB.SetupModules.Include(X => X.TimePoint).FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.SetupModules.Include(X => X.TimePoint).FirstOrDefault(X => X.CourseId == data.CourseId).TimePoint.Points,
                    MaterialTypeName = data.MaterialTypeName,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ApprovedAt = data.ApprovedAt,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    Percentage = GenerateTrainingPercentageAsync(user, data.TrainingId, data.SetupModuleId).Result,
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,

                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetAllUserSideContinueTrainings(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getAllData = DB.GetContinuedLearningModel(user).AsQueryable();
            var totalData = await getAllData.CountAsync();

            var paginatedResult = await getAllData.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToListAsync();

            var dataCourses = new List<UserSideMyLearningHomepageCourseModel>();

            foreach (var data in paginatedResult)
            {
                double preDataPercentage = 0;
                double duringDataPercentage = 0;
                double postDataPercentage = 0;
                double totalDataPercentage = 0;

                bool preNA = false;
                bool duringNA = false;
                bool postNA = false;

                var elData = new List<EnrollLearningTimes>();
                var tempElData = await this.DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).ThenInclude(x => x.Assesment).Include(x => x.SetupModule).ThenInclude(x => x.Module).Where(x => x.EnrollLearning.EmployeeId == user).Where(x => x.EnrollLearning.TrainingId == data.TrainingId).OrderBy(x => x.SetupModule.SetupModuleId).ToListAsync();
                if (tempElData.Count() > 0)
                {
                    foreach (var datum in tempElData)
                    {
                        if (!elData.Select(x => x.SetupModuleId).ToList().Contains(datum.SetupModuleId))
                        {
                            elData.Add(datum);
                        }
                    }
                    var preData = elData.Where(x => x.SetupModule.TrainingTypeId == 1).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(user, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(user, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var duringData = elData.Where(x => x.SetupModule.TrainingTypeId == 2).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(user, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(user, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    var postData = elData.Where(x => x.SetupModule.TrainingTypeId == 3).OrderBy(x => x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                    {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                        Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(user, x.SetupModule.ModuleId, x.SetupModuleId, data.TrainingId.Value) : GetTrackingProgressStatus(user, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, data.TrainingId.Value),
                    }).ToList();

                    if (preData.Count() > 0)
                    {
                        preDataPercentage = ((double)preData.Where(x => x.Status == 3).Count() / (double)preData.Count()) * 100;
                    }
                    else
                    {
                        preNA = true;
                    }
                    if (duringData.Count() > 0)
                    {
                        duringDataPercentage = ((double)duringData.Where(x => x.Status == 3).Count() / (double)duringData.Count()) * 100;
                    }
                    else
                    {
                        duringNA = true;
                    }
                    if (postData.Count() > 0)
                    {
                        postDataPercentage = ((double)postData.Where(x => x.Status == 3).Count() / (double)postData.Count()) * 100;
                    }
                    else
                    {
                        postNA = true;
                    }

                    totalDataPercentage = (((double)postData.Where(x => x.Status == 3).Count() + (double)duringData.Where(x => x.Status == 3).Count() + (double)preData.Where(x => x.Status == 3).Count()) / (preData.Count() + duringData.Count() + postData.Count())) * 100;
                }


                var dataCourse = new UserSideMyLearningHomepageCourseModel
                {
                    TrainingName = data.CourseName == null ? data.ModuleName : data.CourseName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    SetupModuleId = data.SetupModuleId,
                    CourseId = data.CourseId,
                    MaterialTypeName = data.MaterialTypeName,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ApprovedAt = data.ApprovedAt,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    //Percentage = GenerateTrainingPercentageAsync(user, data.TrainingId, data.SetupModuleId).Result,
                    Percentage = Math.Round(totalDataPercentage, 2),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    Price = DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId).CoursePrice,
                    Points = DB.SetupModules.Include(X => X.TimePoint).Where(X => X.CourseId == data.CourseId) == null ? 0 : DB.SetupModules.Include(X => X.TimePoint).Where(X => X.CourseId == data.CourseId).Sum(Y => Y.TimePoint.Points),
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        private async Task<double> GenerateTrainingPercentageAsync(string user, int? trainingId, int? setupModuleId)
        {
            //var query = await (from el in DB.EnrollLearnings
            //                   join elt in DB.EnrollLearningTimes
            //                   on el.EnrollLearningId equals elt.EnrollLearningId
            //                   where
            //                   el.EmployeeId == user &&
            //                   el.TrainingId == trainingId
            //                   select new
            //                   {
            //                       ModuleId = elt.SetupModule.ModuleId,
            //                       EndTime = elt.EndTime,
            //                   })
            //           .AsNoTracking()
            //           .ToListAsync();
            var query = from el in DB.EnrollLearnings
                        join elt in DB.EnrollLearningTimes
                        on el.EnrollLearningId equals elt.EnrollLearningId
                        where
                        el.EmployeeId == user
                        select new { el, elt };

            if (trainingId != null)
            {
                query = query.Where(Q => Q.el.TrainingId == trainingId);
            }
            else
            {
                query = query.Where(Q => Q.el.SetupModuleId == setupModuleId && Q.el.TrainingId == null);
            }

            var data = await query.Select(Q => new
            {
                Q.elt.SetupModule.ModuleId,
                Q.elt.EndTime
            }).AsNoTracking().ToListAsync();


            var persen = 0.0d;

            if (data.Count > 0)
            {
                var progress = data.Where(Q => Q.EndTime.HasValue == true).Count();
                var total = query.Count();

                persen = Math.Round(((double)progress / total) * 100, 1);
            }

            return persen;
        }

        public async Task<List<UserSideMyLearningHomepageCourseModel>> GetAllUserSideCompleteTrainings(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getAllData = DB.GetCompletedLearningModel(user).AsQueryable();
            var totalData = await getAllData.CountAsync();

            var paginatedResult = await getAllData.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToListAsync();

            var dataCourses = new List<UserSideMyLearningHomepageCourseModel>();

            foreach (var data in paginatedResult)
            {
                var dataCourse = new UserSideMyLearningHomepageCourseModel
                {
                    TrainingName = data.TrainingId != null ? data.CourseName : data.ModuleName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    SetupModuleId = data.SetupModuleId,
                    CourseId = data.CourseId,
                    MaterialTypeName = data.MaterialTypeName,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ApprovedAt = data.ApprovedAt,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    Percentage = 100,
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,

                    Price = DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.Courses.FirstOrDefault(X => X.CourseId == data.CourseId).CoursePrice,
                    Points = DB.SetupModules.Include(X => X.TimePoint).FirstOrDefault(X => X.CourseId == data.CourseId) == null ? 0 : DB.SetupModules.Include(X => X.TimePoint).FirstOrDefault(X => X.CourseId == data.CourseId).TimePoint.Points,
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        /// <summary>
        /// get all popular module
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideMyLearningHomepageModuleModel>> GetAllUserSidePopularModules(int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var query = await (from m in DB.Modules

                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join b in DB.Blobs on m.BlobId equals b.BlobId
                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join atm in DB.ApprovalToSetupModules on sm.SetupModuleId equals atm.SetupModuleId

                               join a in DB.Approvals on atm.ApprovalId equals a.ApprovalId

                               where sm.CourseId == null && sm.IsDeleted == false && m.IsDeleted == false &&
                               a.ApprovalStatusId == ApprovalStatusesEnum.ApproveId && sm.IsPopular == true
                               orderby m.UpdatedAt descending
                               select new
                               {
                                   sm.SetupModuleId,
                                   m.ModuleName,
                                   b.BlobId,
                                   b.Mime,
                                   mt.MaterialTypeName
                               }).AsNoTracking().ToListAsync();

            var dataModule = query.Select(Q => new UserSideMyLearningHomepageModuleModel
            {
                SetupModuleId = Q.SetupModuleId,
                ModuleName = Q.ModuleName,
                MaterialTypeName = Q.MaterialTypeName,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).Skip((int)skipCount).Take(pageSize).ToList();

            return dataModule;
        }

        /// <summary>
        /// get all recommended module
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideMyLearningHomepageModuleModel>> GetAllUserSideRecommendedModules(int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var query = await (from m in DB.Modules

                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join b in DB.Blobs on m.BlobId equals b.BlobId
                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join atm in DB.ApprovalToSetupModules on sm.SetupModuleId equals atm.SetupModuleId

                               join a in DB.Approvals on atm.ApprovalId equals a.ApprovalId

                               where sm.CourseId == null && sm.IsDeleted == false && m.IsDeleted == false &&
                               a.ApprovalStatusId == ApprovalStatusesEnum.ApproveId && sm.IsRecommendedForYou == true
                               orderby m.UpdatedAt descending
                               select new
                               {
                                   sm.SetupModuleId,
                                   m.ModuleName,
                                   b.BlobId,
                                   b.Mime,
                                   mt.MaterialTypeName
                               }).AsNoTracking().ToListAsync();

            var dataModule = query.Select(Q => new UserSideMyLearningHomepageModuleModel
            {
                SetupModuleId = Q.SetupModuleId,
                ModuleName = Q.ModuleName,
                MaterialTypeName = Q.MaterialTypeName,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).Skip((int)skipCount).Take(pageSize).ToList();

            return dataModule;
        }

        /// <summary>
        /// get all queued module
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideMyLearningHomepageModuleModel>> GetAllUserSideQueuedModules(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var query = await (from m in DB.Modules

                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join b in DB.Blobs on m.BlobId equals b.BlobId
                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join el in DB.EnrollLearnings on sm.SetupModuleId equals el.SetupModuleId

                               join e in DB.Employees on el.EmployeeId equals e.EmployeeId

                               join atm in DB.ApprovalToSetupModules on sm.SetupModuleId equals atm.SetupModuleId

                               join a in DB.Approvals on atm.ApprovalId equals a.ApprovalId

                               where sm.CourseId == null && sm.IsDeleted == false && m.IsDeleted == false &&
                               a.ApprovalStatusId == ApprovalStatusesEnum.ApproveId && el.IsQueued == true &&
                               e.EmployeeId.ToLower() == user.ToLower()
                               orderby m.UpdatedAt descending
                               select new
                               {
                                   sm.SetupModuleId,
                                   m.ModuleName,
                                   b.BlobId,
                                   b.Mime,
                                   mt.MaterialTypeName
                               }).AsNoTracking().ToListAsync();

            var dataModule = query.Select(Q => new UserSideMyLearningHomepageModuleModel
            {
                SetupModuleId = Q.SetupModuleId,
                ModuleName = Q.ModuleName,
                MaterialTypeName = Q.MaterialTypeName,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result,

            }).Skip((int)skipCount).Take(pageSize).ToList();

            return dataModule;
        }

        /// <summary>
        /// get all continue module
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideMyLearningHomepageModuleModel>> GetAllUserSideContinueModules(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var query = await (from m in DB.Modules

                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join b in DB.Blobs on m.BlobId equals b.BlobId
                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join el in DB.EnrollLearnings on sm.SetupModuleId equals el.SetupModuleId

                               join e in DB.Employees on el.EmployeeId equals e.EmployeeId

                               join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                               join atm in DB.ApprovalToSetupModules on sm.SetupModuleId equals atm.SetupModuleId

                               join a in DB.Approvals on atm.ApprovalId equals a.ApprovalId

                               where sm.CourseId == null && sm.IsDeleted == false && m.IsDeleted == false &&
                               a.ApprovalStatusId == ApprovalStatusesEnum.ApproveId && el.IsEnrolled == true && e.EmployeeId == user && elt.EndTime == null

                               orderby m.UpdatedAt descending
                               select new
                               {
                                   sm.SetupModuleId,
                                   m.ModuleName,
                                   b.BlobId,
                                   b.Mime,
                                   mt.MaterialTypeName
                               }).AsNoTracking().ToListAsync();

            var dataModule = query.Select(Q => new UserSideMyLearningHomepageModuleModel
            {
                SetupModuleId = Q.SetupModuleId,
                ModuleName = Q.ModuleName,
                MaterialTypeName = Q.MaterialTypeName,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).Skip((int)skipCount).Take(pageSize).ToList();

            return dataModule;
        }

        /// <summary>
        /// get all completed module
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideMyLearningHomepageModuleModel>> GetAllUserSideCompleteModules(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var query = await (from m in DB.Modules

                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join b in DB.Blobs on m.BlobId equals b.BlobId
                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join el in DB.EnrollLearnings on sm.SetupModuleId equals el.SetupModuleId

                               join e in DB.Employees on el.EmployeeId equals e.EmployeeId

                               join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                               join atm in DB.ApprovalToSetupModules on sm.SetupModuleId equals atm.SetupModuleId

                               join a in DB.Approvals on atm.ApprovalId equals a.ApprovalId

                               where sm.CourseId == null && sm.IsDeleted == false && m.IsDeleted == false &&
                               a.ApprovalStatusId == ApprovalStatusesEnum.ApproveId && el.IsEnrolled == true && e.EmployeeId == user && elt.EndTime != null

                               orderby m.UpdatedAt descending
                               select new
                               {
                                   sm.SetupModuleId,
                                   m.ModuleName,
                                   b.BlobId,
                                   b.Mime,
                                   mt.MaterialTypeName,
                                   sm.ApprovedAt
                               }).AsNoTracking().ToListAsync();

            var dataModule = query.Select(Q => new UserSideMyLearningHomepageModuleModel
            {
                SetupModuleId = Q.SetupModuleId,
                ModuleName = Q.ModuleName,
                MaterialTypeName = Q.MaterialTypeName,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result,
                ApprovedAt = Q.ApprovedAt
            }).Skip((int)skipCount).Take(pageSize).ToList();

            return dataModule;
        }

        public async Task<List<UserSideMyLearningHomepageCourseBadgeModelNew>> GetAllUserSideQueuedBadges(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getAllData = await DB.GetQueuedBadgesModel(user).ToListAsync();
            var totalData = getAllData.Count();

            var paginatedResult = getAllData.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToList();

            var dataCourses = new List<UserSideMyLearningHomepageCourseBadgeModelNew>();

            foreach (var data in paginatedResult)
            {
                var dataCourse = new UserSideMyLearningHomepageCourseBadgeModelNew
                {
                    TrainingName = data.TopicName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    CourseId = data.CourseId,
                    TrainingBatch = data.Batch,
                    SetupModuleId = data.SetupModuleId,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    BronzeBadge = data.BadgeId == 1 ? 1 : 0,
                    SilverBadge = data.BadgeId == 2 ? 1 : 0,
                    GoldBadge = data.BadgeId == 3 ? 1 : 0,
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        public async Task<List<UserSideMyLearningHomepageCourseBadgeModelNew>> GetAllUserSideContinueBadges(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getAllData = DB.GetContinuedBadgesModel(user).AsQueryable();
            var totalData = await getAllData.CountAsync();

            var paginatedResult = await getAllData.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToListAsync();

            var dataCourses = new List<UserSideMyLearningHomepageCourseBadgeModelNew>();


            foreach (var data in paginatedResult)
            {
                var dataCourse = new UserSideMyLearningHomepageCourseBadgeModelNew
                {
                    TrainingName = data.TopicName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    SetupModuleId = data.SetupModuleId,
                    CourseId = data.CourseId,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    BronzeBadge = data.BadgeId == 1 ? 1 : 0,
                    SilverBadge = data.BadgeId == 2 ? 1 : 0,
                    GoldBadge = data.BadgeId == 3 ? 1 : 0,
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        public async Task<List<UserSideMyLearningHomepageCourseBadgeModelNew>> GetAllUserSideCompleteBadges(string user, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var getAllData = DB.GetCompletedBadgesModel(user).AsQueryable();
            var totalData = await getAllData.CountAsync();

            var paginatedResult = await getAllData.OrderByDescending(Q => Q.ApprovedAt).Skip((int)skipCount)
            .Take(pageSize)
            .ToListAsync();

            var dataCourses = new List<UserSideMyLearningHomepageCourseBadgeModelNew>();

            foreach (var data in paginatedResult)
            {
                var dataCourse = new UserSideMyLearningHomepageCourseBadgeModelNew
                {
                    TrainingName = data.TopicName,
                    ProgramTypeName = data.ProgramTypeName,
                    TrainingId = data.TrainingId,
                    TrainingBatch = data.Batch,
                    SetupModuleId = data.SetupModuleId,
                    CourseId = data.CourseId,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ImageUrl = await FileService.GetFileAsync(data.BlobId.ToString(), data.MIME),
                    Rating = await GenerateRatingTraining(data.TrainingId),
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt,
                    BronzeBadge = data.BadgeId == 1 ? 1 : 0,
                    SilverBadge = data.BadgeId == 2 ? 1 : 0,
                    GoldBadge = data.BadgeId == 3 ? 1 : 0,
                };
                dataCourses.Add(dataCourse);
            }

            return dataCourses;
        }

        public async Task<UserSideCourseViewModel> GetUserSideCourseAsync(string user, int trainingId, int courseId)
        {
            //ini gw tambahin null karena apinya sekarang ada 3 parameter. user, trainingid, setupmoduleid.. gw kasih null supaya gak nyenggol aja
            var getData = await this.GetCourseContent(user, trainingId, courseId, null);
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            int totalTime = (getData.Item1.Sum(Q => Q.ModuleTimeLength)).Value;

            /* if (getData.IsEnrolled == false)
             {
                 getData.Item1 = null;
             }*/

            var query = new List<SetupLearningContentQueryModel>();

            if (trainingId != 0)
            {
                query = await (from sl in DB.SetupLearning

                               join t in DB.Trainings on sl.CourseId equals t.CourseId into tc
                               from t in tc.DefaultIfEmpty()

                               join c in DB.Courses on sl.CourseId equals c.CourseId into csl
                               from c in csl.DefaultIfEmpty()

                               join pt in DB.ProgramTypes on sl.Course.ProgramTypeId equals pt.ProgramTypeId
                               join b in DB.Blobs on sl.Course.BlobId equals b.BlobId into cb
                               from b in cb.DefaultIfEmpty()

                               join sm in DB.SetupModules on sl.Course.CourseId equals sm.CourseId

                               join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId into smtp
                               from tp in smtp.DefaultIfEmpty()

                               join mo in DB.Modules on sm.ModuleId equals mo.ModuleId
                               join mtm in DB.ModuleTopicMappings on mo.ModuleId equals mtm.ModuleId

                               join to in DB.Topics on mtm.TopicId equals to.TopicId

                               //join tr in DB.TrainingRatings on t.TrainingId equals tr.TrainingId into ctr
                               //from tr in ctr.DefaultIfEmpty()

                               where sl.Course.CourseApprovedAt != null
                               && (t.TrainingId == trainingId)

                               select new SetupLearningContentQueryModel
                               {
                                   Batch = t == null ? 0 : t.Batch,
                                   StartDate = t.StartDate == null ? null : t.StartDate,
                                   EndDate = t.EndDate == null ? DateTime.MinValue : t.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59),
                                   //EndDate = t.EndDate == null ? DateTime.MinValue : t.EndDate.Value,
                                   //RatingScore = tr == null ? 0 : tr.RatingScore,
                                   CourseName = sl.Course.CourseName,
                                   ProgramTypeName = pt.ProgramTypeName,
                                   Price = sl.Course.CoursePrice == null ? 0 : sl.Course.CoursePrice,
                                   Time = tp.Time == null ? 0 : tp.Time,
                                   Points = tp.Points,
                                   TopicName = to.TopicName,
                                   BlobId = sl.Course.BlobId,
                                   Mime = b.Mime,
                                   CreatedAt = sl.Course.CreatedAt,
                                   UpdatedAt = sl.Course.UpdatedAt,
                                   TrainingId = t.TrainingId,
                                   CourseId = sl.CourseId,
                                   ModuleId = mo.ModuleId,
                                   IsOffline = c.LearningTypeId != 1 ? true : false,
                                   IsPaid = c.CoursePrice > 0 ? true : false,
                               }).AsNoTracking()
                                  .ToListAsync();


                var assesmentQuery = await (from sl in DB.SetupLearning

                                            join c in DB.Courses on sl.CourseId equals c.CourseId into lc
                                            from c in lc.DefaultIfEmpty()

                                            join pt in DB.ProgramTypes on sl.Course.ProgramTypeId equals pt.ProgramTypeId
                                            join b in DB.Blobs on sl.Course.BlobId equals b.BlobId into cb
                                            from b in cb.DefaultIfEmpty()

                                            join sm in DB.SetupModules on sl.Course.CourseId equals sm.CourseId

                                            join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId into smtp
                                            from tp in smtp.DefaultIfEmpty()

                                            join mo in DB.Assesments on sm.AssesmentId equals mo.GUID


                                            where sl.Course.CourseApprovedAt != null && c.CourseId == courseId
                                            select new SetupLearningContentQueryModel
                                            {
                                                Batch = 0,
                                                StartDate = null,
                                                EndDate = null,
                                                RatingScore = 0,
                                                CourseName = sl.Course.CourseName,
                                                ProgramTypeName = pt.ProgramTypeName,
                                                Price = sl.Course.CoursePrice == null ? 0 : sl.Course.CoursePrice,
                                                Time = tp.Time == null ? 0 : tp.Time,
                                                Points = tp.Points,
                                                RemedialLevel = 0,
                                                BlobId = sl.Course.BlobId,
                                                Mime = b.Mime,
                                                CreatedAt = sl.Course.CreatedAt,
                                                UpdatedAt = sl.Course.UpdatedAt,
                                                CourseId = sl.CourseId,
                                                AssesmentGUID = mo.GUID,
                                                AssesmentName = mo.Name
                                            }).AsNoTracking()
                                 .ToListAsync();

                query.AddRange(assesmentQuery);
            }
            else
            {
                query = await (from sl in DB.SetupLearning

                               join c in DB.Courses on sl.CourseId equals c.CourseId into lc
                               from c in lc.DefaultIfEmpty()

                               join pt in DB.ProgramTypes on sl.Course.ProgramTypeId equals pt.ProgramTypeId
                               join b in DB.Blobs on sl.Course.BlobId equals b.BlobId into cb
                               from b in cb.DefaultIfEmpty()

                               join sm in DB.SetupModules on sl.Course.CourseId equals sm.CourseId

                               join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId into smtp
                               from tp in smtp.DefaultIfEmpty()

                               join mo in DB.Modules on sm.ModuleId equals mo.ModuleId
                               join mtm in DB.ModuleTopicMappings on mo.ModuleId equals mtm.ModuleId

                               join to in DB.Topics on mtm.TopicId equals to.TopicId

                               where sl.Course.CourseApprovedAt != null && c.CourseId == courseId
                               select new SetupLearningContentQueryModel
                               {
                                   Batch = 0,
                                   StartDate = null,
                                   EndDate = null,
                                   RatingScore = 0,
                                   CourseName = sl.Course.CourseName,
                                   ProgramTypeName = pt.ProgramTypeName,
                                   Price = sl.Course.CoursePrice == null ? 0 : sl.Course.CoursePrice,
                                   Time = tp.Time == null ? 0 : tp.Time,
                                   Points = tp.Points,
                                   TopicName = to.TopicName,
                                   RemedialLevel = 0,
                                   BlobId = sl.Course.BlobId,
                                   Mime = b.Mime,
                                   CreatedAt = sl.Course.CreatedAt,
                                   UpdatedAt = sl.Course.UpdatedAt,
                                   CourseId = sl.CourseId,
                                   ModuleId = mo.ModuleId
                               }).AsNoTracking()
                                  .ToListAsync();



                var assesmentQuery = await (from sl in DB.SetupLearning

                                            join c in DB.Courses on sl.CourseId equals c.CourseId into lc
                                            from c in lc.DefaultIfEmpty()

                                            join pt in DB.ProgramTypes on sl.Course.ProgramTypeId equals pt.ProgramTypeId
                                            join b in DB.Blobs on sl.Course.BlobId equals b.BlobId into cb
                                            from b in cb.DefaultIfEmpty()

                                            join sm in DB.SetupModules on sl.Course.CourseId equals sm.CourseId

                                            join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId into smtp
                                            from tp in smtp.DefaultIfEmpty()

                                            join mo in DB.Assesments on sm.AssesmentId equals mo.GUID


                                            where sl.Course.CourseApprovedAt != null && c.CourseId == courseId
                                            select new SetupLearningContentQueryModel
                                            {
                                                Batch = 0,
                                                StartDate = null,
                                                EndDate = null,
                                                RatingScore = 0,
                                                CourseName = sl.Course.CourseName,
                                                ProgramTypeName = pt.ProgramTypeName,
                                                Price = sl.Course.CoursePrice == null ? 0 : sl.Course.CoursePrice,
                                                Time = tp.Time == null ? 0 : tp.Time,
                                                Points = tp.Points,
                                                RemedialLevel = 0,
                                                BlobId = sl.Course.BlobId,
                                                Mime = b.Mime,
                                                CreatedAt = sl.Course.CreatedAt,
                                                UpdatedAt = sl.Course.UpdatedAt,
                                                CourseId = sl.CourseId,
                                                AssesmentGUID = mo.GUID,
                                                AssesmentName = mo.Name
                                            }).AsNoTracking()
                                 .ToListAsync();

                query.AddRange(assesmentQuery);

            }

            var MainModulePoints = query.Select(Q => Q.Points).FirstOrDefault();

            var dataRating = await GenerateRatingTraining(trainingId);

            //var dataRating = await (from c in DB.Courses
            //                        join tr in DB.Trainings on c.CourseId equals tr.CourseId
            //                        join cr in DB.TrainingRatings on c.CourseId equals cr.CourseId
            //                        into leftjoin
            //                        from cr1 in leftjoin.DefaultIfEmpty()
            //                        where tr.TrainingId == trainingId
            //                        select new
            //                        {
            //                            RatingScore = cr1 == null ? 0 : cr1.RatingScore,
            //                        }).ToListAsync();

            var moduleids = query.Select(Q => Q.ModuleId).ToList();

            var dataCourseDetail = query
                .Select(async Q => new UserSideCourseViewModel
                {
                    TrainingId = Q.TrainingId.ToString(),
                    TrainingName = Q.CourseName,
                    ProgramTypeName = Q.ProgramTypeName,
                    Duration = totalTime,
                    Price = Q.Price,
                    Batch = Q.Batch,
                    RemedialLevel = Q.RemedialLevel,
                    IsPassed = Q.IsPassed,
                    StartDate = Q.StartDate,
                    EndDate = Q.EndDate,
                    CreatedAt = Q.CreatedAt,
                    UpdatedAt = Q.UpdatedAt,
                    ImageUrl = await FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime),
                    IsAllowEnroll = GenerateIsAllowEnrolled(Q.TrainingId ?? 0, Q.CourseId ?? 0).Result
                })
                .Select(Q => Q.Result)
                .FirstOrDefault();

            if (dataCourseDetail == null)
            {
                return new UserSideCourseViewModel();
            }
            dataCourseDetail.Points = 0;
            //test roberto
            if (moduleids != null)
            {
                var ListSetupModules = await (
                from SM in this.DB.SetupModules
                join TP in this.DB.TimePoints on SM.TimePointId equals TP.TimePointId
                join M in this.DB.Modules on SM.ModuleId equals M.ModuleId
                where SM.CourseId == courseId && SM.IsDeleted == false
                select new SetupModuleFormModel
                {
                    SetupModuleId = SM.SetupModuleId,
                    TrainingTypeId = SM.TrainingTypeId ?? default(int),
                    MinimumScore = SM.MinimumScore ?? default(int),
                    TimePointId = SM.TimePointId,
                    Score = TP.Time,
                    Points = TP.Points,
                    ModuleId = M.ModuleId,
                    ModuleName = M.ModuleName,
                    IsForRemedial = SM.IsForRemedial ?? default(bool),
                    SetupTaskForm = new SetupTaskFormModel()
                }).AsNoTracking().ToListAsync();

                foreach (var setupModule in ListSetupModules)
                {
                    dataCourseDetail.Points += setupModule.Points;
                    setupModule.SetupTaskForm = await (from ST in this.DB.SetupTasks

                                                       join M in this.DB.Modules on ST.ModuleId equals M.ModuleId into LM
                                                       from M in LM.DefaultIfEmpty()

                                                       join C in this.DB.Competencies on ST.CompetencyId equals C.CompetencyId into LC
                                                       from C in LC.DefaultIfEmpty()

                                                       join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId into LCT
                                                       from CT in LCT.DefaultIfEmpty()

                                                       where ST.SetupModuleId == setupModule.SetupModuleId

                                                       select new SetupTaskFormModel
                                                       {
                                                           SetupTaskId = ST.SetupTaskId,
                                                           TestTime = ST.TestTime,
                                                           IsGrouping = ST.IsGrouping,
                                                           CompetencyId = C.CompetencyId,
                                                           CompetencyTypeId = C.CompetencyTypeId,
                                                           CompetencyTypeName = CT.CompetencyTypeName,
                                                           PrefixCode = C.PrefixCode,
                                                           CompetencyNameMapping = ST.CompetencyId != null ? CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode : "",
                                                           ModuleId = M.ModuleId,
                                                           ModuleName = ST.ModuleId != null ? M.ModuleName : "",
                                                           ModuleDescription = M.ModuleDescription,
                                                           TotalParticipant = ST.TotalParticipant,
                                                           TotalQuestion = ST.TotalQuestion,
                                                           QuestionPerParticipant = ST.QuestionPerParticipant,
                                                           TaskList = new List<SetupTaskCodesFormModel>(),
                                                       }).AsNoTracking().FirstOrDefaultAsync();

                    if (setupModule.SetupTaskForm == null)
                    {
                        setupModule.SetupTaskForm = new SetupTaskFormModel();
                    }

                    setupModule.SetupTaskForm.TaskList = await (from STC in this.DB.SetupTaskCodes

                                                                join T in this.DB.Tasks on STC.TaskId equals T.TaskId into lt
                                                                from T in lt.DefaultIfEmpty()
                                                                join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId into lq
                                                                from Q in lq.DefaultIfEmpty()
                                                                join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                                                                join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId into lc
                                                                from C in lc.DefaultIfEmpty()
                                                                join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId into lct
                                                                from CT in lct.DefaultIfEmpty()
                                                                join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId into ljet
                                                                from ET in ljet.DefaultIfEmpty()

                                                                where STC.SetupTaskId == setupModule.SetupTaskForm.SetupTaskId

                                                                select new SetupTaskCodesFormModel
                                                                {
                                                                    SetupTaskId = STC.SetupTaskId,
                                                                    TaskId = STC.TaskId,
                                                                    QuestionNumber = STC.QuestionNumber,
                                                                    TaskName = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                                                                    TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                                                                    TaskTypeId = T.QuestionTypeId
                                                                }).AsNoTracking().ToListAsync();

                    for (var i = 0; i < setupModule.SetupTaskForm.TaskList.Count(); i++)
                    {
                        if (setupModule.SetupTaskForm.TaskList[i].TaskId == 0) continue;
                        var scorePoints = await GetScorePoints(setupModule.SetupTaskForm.TaskList[i].TaskTypeId ?? default(int), setupModule.SetupTaskForm.TaskList[i].TaskId ?? default(int));
                        setupModule.SetupTaskForm.TaskList[i].Score = scorePoints.Score;
                        dataCourseDetail.Points += scorePoints.Points;
                    }
                }
            }
            //dataCourseDetail.Points = await GetAllPointsAsync(moduleids);
            dataCourseDetail.TopicName = query.Select(Q => Q.TopicName).Distinct().ToList();
            dataCourseDetail.Rating = dataRating;
            //dataCourseDetail.Rating = Convert.ToDouble(dataRating.Sum(Q => Q.RatingScore)) / Convert.ToDouble(dataRating.Count());

            var userDataEnroll = await this.DB.EnrollLearnings.Where(Q => Q.TrainingId == trainingId && Q.EmployeeId.ToLower() == user.ToLower()).FirstOrDefaultAsync();

            if (userDataEnroll != null)
            {
                dataCourseDetail.IsQueue = userDataEnroll.IsQueued;
                dataCourseDetail.IsEnroll = userDataEnroll.IsEnrolled;
                dataCourseDetail.IsPassed = userDataEnroll.IsPassed;
                dataCourseDetail.RemedialLevel = userDataEnroll.RemedialLevel;
                dataCourseDetail.IsJoined = userDataEnroll.IsJoined;
            }
            else
            {
                dataCourseDetail.IsQueue = false;
                dataCourseDetail.IsPassed = false;
                dataCourseDetail.RemedialLevel = 0;
                dataCourseDetail.IsEnroll = false;
                dataCourseDetail.IsJoined = false;
            }

            var canEnroll = false;
            var getTrainingEndDate = await this
                .DB
                .Trainings
                .Where(Q => Q.TrainingId == trainingId && Q.IsDeleted == false)
                .Select(Q => Q.EndDate).FirstOrDefaultAsync();

            if (thisDate.Date <= getTrainingEndDate)
            {
                canEnroll = true;
            }

            dataCourseDetail.CanEnroll = canEnroll;

            var relatedTraining = DB.TrainingCertifications.Include(x=> x.RelatedTraining).Where(x=> x.TrainingId.Value.ToString() == dataCourseDetail.TrainingId).FirstOrDefault();
            if (relatedTraining != null){
                dataCourseDetail.RelatedTrainingId = relatedTraining.RelatedTrainingId.ToString();
                dataCourseDetail.RelatedTrainingId = relatedTraining.RelatedTraining.TrainingId.ToString();
                var setup = DB.SetupModules.Where(x=> x.CourseId == relatedTraining.RelatedTraining.CourseId);
                if (setup != null){
                    dataCourseDetail.RelatedTrainingAssesment = setup.Where(x=> x.AssesmentId != null).Any();
                }

            }

            return dataCourseDetail;
        }

        public async Task<int> GetAllPointsAsync(List<int> moduleIds)
        {

            var tasks = await DB.Tasks
                .Where(Q => moduleIds.Contains(Q.ModuleId))
                .Select(Q => new
                {
                    Q.TaskId,
                    Q.QuestionTypeId
                }).ToListAsync();

            var groupedTask = tasks.GroupBy(Q => Q.QuestionTypeId).Select(t => new GroupedTaskModel
            {
                QuestionTypeId = t.First().QuestionTypeId,
                TaskIds = t.Select(Q => Q.TaskId).ToList()
            })
            .ToList();

            var totalPoint = 0;
            foreach (var task in groupedTask)
            {
                var point = 0;
                if (task.QuestionTypeId == (int)QuestionTypeEnum.TrueOrFalse)
                {
                    point = await DB.TaskTrueFalseTypes
                        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                        .SumAsync(Q => Q.Score);
                }
                else if (task.QuestionTypeId == (int)QuestionTypeEnum.Matching)
                {
                    point = await DB.TaskMatchingTypes
                        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                        .SumAsync(Q => Q.Score);
                }
                else if (task.QuestionTypeId == (int)QuestionTypeEnum.Sequence)
                {
                    point = await DB.TaskSequenceChoices
                        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                        .SumAsync(Q => Q.Score);
                }
                else if (task.QuestionTypeId == (int)QuestionTypeEnum.TebakGambar)
                {
                    point = await DB.TaskTebakGambarTypes
                        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                        .SumAsync(Q => Q.Score);
                }
                else if (task.QuestionTypeId == (int)QuestionTypeEnum.HotSpot)
                {
                    point = await DB.TaskHotSpotAnswers
                        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                        .SumAsync(Q => Q.Score);
                }
                //else if (task.QuestionTypeId == (int)QuestionTypeEnum.ShortAnswer)
                //{
                //    point = await DB.TaskShortAnswerTypes
                //        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                //        .SumAsync(Q => Q.Score);
                //}
                //else if (task.QuestionTypeId == (int)QuestionTypeEnum.OpenQuestionOrEssay)
                //{
                //    point = await DB.TaskEssayTypes
                //        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                //        .SumAsync(Q => Q.Score);
                //}
                else if (task.QuestionTypeId == (int)QuestionTypeEnum.Checklist)
                {
                    point = await DB.TaskChecklistChoices
                        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                        .SumAsync(Q => Q.Score);
                }
                //else if (task.QuestionTypeId == (int)QuestionTypeEnum.Rating)
                //{
                //    point = await DB.TaskRatingChoices
                //        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                //        .SumAsync(Q => Q.Score);
                //}
                //else if (task.QuestionTypeId == (int)QuestionTypeEnum.MultipleChoice)
                //{
                //    point = await DB.TaskMultipleChoiceChoices
                //        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                //        .SumAsync(Q => Q.Score);
                //}
                //else if (task.QuestionTypeId == (int)QuestionTypeEnum.FileUpload)
                //{
                //    point = await DB.TaskFileUploadTypes
                //        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                //        .SumAsync(Q => Q.Score);
                //}
                //else if (task.QuestionTypeId == (int)QuestionTypeEnum.Matrix)
                //{
                //    point = await DB.TaskMatrixChoices
                //        .Where(Q => task.TaskIds.Contains(Q.TaskId))
                //        .SumAsync(Q => Q.Score);
                //}
                totalPoint += point;

            }

            return totalPoint;
        }

        /// <summary>
        /// Untuk ambil data module dari setup module
        /// </summary>
        /// <param name="user"></param>
        /// <param name="setupModuleId"></param>
        /// <returns></returns>
        public async Task<UserSideModuleViewModel> GetUserSideModuleAsync(string user, int setupModuleId)
        {
            var query = await (from sm in DB.SetupModules
                               join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId

                               join m in DB.Modules on sm.ModuleId equals m.ModuleId
                               join b in DB.Blobs on m.BlobId equals b.BlobId
                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join mtm in DB.ModuleTopicMappings on m.ModuleId equals mtm.ModuleId
                               join t in DB.Topics on mtm.TopicId equals t.TopicId

                               where sm.SetupModuleId == setupModuleId
                               select new
                               {
                                   b.BlobId,
                                   b.Mime,
                                   tp.Points,
                                   m.ModuleName,
                                   mt.MaterialTypeName,
                                   m.ModuleDescription,
                                   t.TopicName,
                                   tp.Time
                               }).AsNoTracking().ToListAsync();

            var dataModuleDetail = query.Select(Q => new UserSideModuleViewModel
            {
                ModuleName = Q.ModuleName,
                ModuleDescription = Q.ModuleDescription,
                MaterialTypeName = Q.MaterialTypeName,
                Duration = Q.Time.GetValueOrDefault(),
                Points = Q.Points,
                TopicNames = new List<string>(),
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).FirstOrDefault();

            if (query != null)
            {
                dataModuleDetail.TopicNames = query.Select(Q => Q.TopicName).Distinct().ToList();
            }

            var queue = await DB.EnrollLearnings.Where(X => X.EmployeeId.ToLower() == user.ToLower() && X.SetupModuleId == setupModuleId && X.IsQueued == true).AnyAsync();
            dataModuleDetail.IsQueue = queue;


            var enroll = await DB.EnrollLearnings.Where(X => X.EmployeeId.ToLower() == user.ToLower() && X.SetupModuleId == setupModuleId && X.IsEnrolled == true).AnyAsync();
            dataModuleDetail.IsEnroll = enroll;

            if(enroll == false)
            {
                dataModuleDetail.IsJoined = false;
            }
            else
            {
                dataModuleDetail.IsJoined = true;
            }

            return dataModuleDetail;
        }

        /// <summary>
        /// Obtain Course Overview based on the course ID.
        /// </summary>
        /// <param name="courseId">The requested course ID.</param>
        /// <returns>Return <seealso cref="UserSideCourseOverviewModel"/> object.</returns>
        public async Task<UserSideCourseOverviewModel> GetCourseOverview(int courseId, string userId)
        {
            var overview = new UserSideCourseOverviewModel();

            // Check if course exists or not.
            var courseDescription = await this
                .DB
                .Courses
                .AsNoTracking()
                .Where(Q => Q.CourseId == courseId)
                .Select(Q => Q.CourseDescription)
                .FirstOrDefaultAsync();

            overview.CourseDesc = courseDescription;

            var prerequisitesQuery = (from cpm in DB.CoursePrerequisiteMappings

                                          // LEFT JOIN.
                                      join sl in DB.SetupLearning on cpm.NextCourseId equals sl.CourseId into cpmsl
                                      from sl in cpmsl.DefaultIfEmpty()

                                          // LEFT JOIN.
                                      join cpm2 in DB.CoursePrerequisiteMappings on cpm.PrevCourseId equals cpm2.NextCourseId into cpmcpm2
                                      from cpm2 in cpmcpm2.DefaultIfEmpty()

                                          // LEFT JOIN.
                                      join t in DB.Trainings on sl.CourseId equals t.CourseId into slt
                                      from t in slt.DefaultIfEmpty()


                                          // LEFT JOIN.
                                      join sm in DB.SetupModules on cpm.NextSetupModuleId equals sm.SetupModuleId into cpmsm
                                      from sm in cpmsm.DefaultIfEmpty()

                                          //uncomment this
                                      join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId into smtp
                                      from tp in smtp.DefaultIfEmpty()

                                      join cr in DB.TrainingRatings on sl.CourseId equals cr.CourseId into ccr
                                      from cr in ccr.DefaultIfEmpty()

                                          // LEFT JOIN.
                                      join m in DB.Modules on sm.ModuleId equals m.ModuleId into lm
                                      from m in lm.DefaultIfEmpty()

                                      join blobs in DB.Blobs on sl.Course.BlobId equals blobs.BlobId into mblobs
                                      from blobs in mblobs.DefaultIfEmpty()

                                      where (cpm.PrevCourseId == courseId)
                                      select new
                                      {
                                          TrainingId = (t == null || t.IsDeleted == true) ? 0 : t.TrainingId,
                                          CourseId = cpm.NextCourseId,
                                          CourseName = sl.Course.CourseName,
                                          SetupModuleId = cpm2.NextSetupModuleId,
                                          ModuleName = m.ModuleName,
                                          Batch = (int?)t.Batch,
                                          Image = blobs.BlobId,
                                          Mime = blobs.Mime,
                                          CreatedAt = sl.Course.CreatedAt,
                                          Price = sl.Course.CoursePrice,
                                          Point = tp == null ? 0 : tp.Points,
                                          Rating = cr == null ? 0 : cr.RatingScore,
                                          Percentage = 100,
                                      })
                                      .AsNoTracking();

            var query = await prerequisitesQuery.ToListAsync();

            //here 
            var module = query.Where(Q => Q.CourseId == null && Q.SetupModuleId != null).Select(Q => new UserSidePrerequisiteModel
            {
                CourseId = Q.CourseId,
                CourseName = Q.CourseName,
                ModuleName = Q.ModuleName,
                SetupModuleId = Q.SetupModuleId,
                TrainingId = Q.TrainingId,
                Price = Q.Price,
                Point = Q.Point,
                CreatedAt = Q.CreatedAt,
                Rating = Q.Rating,
                Image = Q.Image.ToString() == "" ? "" : FileService.GetFileAsync(Q.Image.ToString(), Q.Mime).Result,
                Percentage = (int)Q.Percentage,

            }).ToList();

            var course = query.Where(Q => Q.SetupModuleId == null && Q.CourseId != null).ToList();
            var outputList = new List<UserSidePrerequisiteModel>();
            if (course.Count != 0)
            {
                var groupingTraining = (from listCourse in course
                                        group listCourse by listCourse.CourseId into resultList
                                        select new
                                        {
                                            CourseId = resultList.Key,
                                            CourseName = resultList.First().CourseName,
                                            ModuleName = resultList.First().ModuleName,
                                            SetupModuleId = resultList.First().SetupModuleId,
                                            Batch = resultList.Max(Q => Q.Batch),
                                            TrainingId = resultList.Max(Q => Q.TrainingId),
                                            Price = resultList.First().Price,
                                            Point = resultList.First().Point,
                                            CreatedAt = resultList.First().CreatedAt,
                                            Rating = resultList.First().Rating,
                                            Image = resultList.First().Image,
                                            Mime = resultList.First().Mime,
                                            Percentage = 100,
                                        }).ToList();

                var sortTrainingBatch = groupingTraining.Select(Q => new UserSidePrerequisiteModel
                {
                    CourseId = Q.CourseId,
                    CourseName = Q.CourseName,
                    ModuleName = Q.ModuleName,
                    SetupModuleId = Q.SetupModuleId,
                    TrainingId = Q.TrainingId,
                    Price = Q.Price,
                    Point = Q.Point,
                    CreatedAt = Q.CreatedAt,
                    Rating = Q.Rating,
                    Image = Q.Image.ToString() == "" ? "" : FileService.GetFileAsync(Q.Image.ToString(), Q.Mime).Result,
                    Percentage = (int)Q.Percentage,
                }).ToList();

                outputList = sortTrainingBatch;
            }

            outputList.AddRange(module);

            overview.Prerequisites = outputList;

            var learningObjectivesQuery = (from clo in DB.CourseLearningObjectives

                                               // LEFT JOIN.
                                           join sl in DB.SetupLearning on clo.CourseId equals sl.Course.CourseId into closl
                                           from sl in closl.DefaultIfEmpty()

                                           where sl.CourseId == courseId
                                           select clo.LearningObjectiveName)
                                           .AsNoTracking()
                                           .Distinct();

            overview.LearningObjs = await learningObjectivesQuery.ToListAsync();

            return overview;
        }

        /// <summary>
        /// To get Training Scheme for Course Overview
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<List<UserSideCourseTrainingViewModel>> GetTrainingScheme(int courseId)
        {

            var query = await (from c in DB.Courses

                               join cttm in DB.CourseTrainingTypeMappings on c.CourseId equals cttm.CourseId into ccttm
                               from cttm in ccttm.DefaultIfEmpty()

                               join tt in DB.TrainingTypes on cttm.TrainingTypeId equals tt.TrainingTypeId into cttmtt
                               from tt in cttmtt.DefaultIfEmpty()

                               where c.CourseId == courseId
                               select new UserSideCourseTrainingViewModel
                               {
                                   TrainingTypeName = tt.TrainingTypeName,
                                   MinScore = cttm.MinimumScore.GetValueOrDefault(),
                                   WorkloadReq = cttm.WorkloadRequirement,
                                   Percentage = cttm.Percentage.GetValueOrDefault()
                               }
                               ).AsNoTracking()
                               .ToListAsync();

            return query;

        }

        /// <summary>
        /// To get Coaches Data for Course Overview
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<List<UserSideCourseCoachViewModel>> GetCoach(int trainingId)
        {

            var query = (from tmm in DB.TrainingModuleMappings

                         join c in DB.Coaches on tmm.CoachId equals c.CoachId into tmmc
                         from c in tmmc.DefaultIfEmpty()

                         join t in DB.Trainings on tmm.TrainingId equals t.TrainingId

                         join e in DB.Employees on c.EmployeeId equals e.EmployeeId

                         join b in DB.Blobs on e.BlobId equals b.BlobId into eb
                         from b in eb.DefaultIfEmpty()

                         where t.TrainingId == trainingId
                         select new
                         {
                             c.CoachId,
                             e.EmployeeName,
                             e.BlobId,
                             b.Mime
                         }
                               ).AsNoTracking().Distinct();

            var data = await query.Select(Q => new UserSideCourseCoachViewModel
            {
                CoachId = Q.CoachId,
                CoachName = Q.EmployeeName,
                ImageUrl = FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).ToListAsync();

            return data;
        }

        /// <summary>
        /// To get Course liked by user
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<int> GetLike(int trainingId)
        {
            var data = await this.DB.GetUserSideCourseLikePeopleList(trainingId).CountAsync();
            return data;
        }

        public async Task<UserSideCourseLikePeopleSampleImage> GetPeopleWhoLikeTheCourseSampleImage(int trainingId)
        {

            var query = await (from c in DB.Courses

                               join t in DB.Trainings on c.CourseId equals t.CourseId

                               join cr in DB.TrainingRatings on c.CourseId equals cr.CourseId into ccr
                               from cr in ccr.DefaultIfEmpty()

                               join e in DB.Employees on cr.EmployeeId equals e.EmployeeId

                               join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId

                               join p in DB.Positions on epm.PositionId equals p.PositionId

                               join b in DB.Blobs on e.BlobId equals b.BlobId into eb
                               from b in eb.DefaultIfEmpty()

                               where t.TrainingId == trainingId && cr.RatingScore > 0
                               select new
                               {
                                   e.EmployeeId,
                                   e.EmployeeName,
                                   p.PositionName,
                                   e.BlobId,
                                   b.Mime,
                                   cr.RatingScore
                               }).AsNoTracking()
                                 .Take(3)
                                 .ToListAsync();

            var data = query.GroupBy(Q => Q.EmployeeId).Select(async Q =>
            await FileService.GetFileAsync(Q.First().BlobId.ToString(), Q.First().Mime))
            .Select(Q => Q.Result)
            .ToList();

            var dataCount = query.Count();

            var response = new UserSideCourseLikePeopleSampleImage
            {
                ImageUrl = data,
                Count = dataCount
            };

            return response;
        }

        public async Task<List<UserSideCourseLikePeopleListModel>> GetPeopleWhoLikeTheCourseList(int trainingId, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            var query = await this.DB.GetUserSideCourseLikePeopleList(trainingId).Skip((int)skipCount).Take(pageSize).ToListAsync();

            var data = query.Select(async Q => new UserSideCourseLikePeopleListModel
            {
                EmployeeId = Q.EmployeeId,
                EmployeeName = Q.EmployeeName,
                Position = Q.PositionName,
                ImageUrl = await FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime)
            }).Select(Q => Q.Result)
            .ToList();

            return data;
        }

        public async Task<UserSideWhoTookTheCourseSampleImage> GetPeopleWHoTookTheCourseSampleImage(int trainingId)
        {
            var query = await (from el in DB.EnrollLearnings

                               join epm in DB.EmployeePositionMappings on el.EmployeeId equals epm.EmployeeId

                               join b in DB.Blobs on el.Employee.BlobId equals b.BlobId into eb
                               from b in eb.DefaultIfEmpty()

                               where el.TrainingId == trainingId
                               select new
                               {
                                   el.Employee.EmployeeId,
                                   el.Employee.BlobId,
                                   b.Mime
                               }).AsNoTracking()
                        .Take(10)
                        .ToListAsync();

            var dataUrl = query.GroupBy(Q => Q.EmployeeId).Select(async Q =>
                 await FileService.GetFileAsync(Q.First().BlobId.ToString(), Q.First().Mime)
             ).Select(Q => Q.Result)
             .ToList();

            var dataCount = query.Count();

            var response = new UserSideWhoTookTheCourseSampleImage
            {
                ImageUrl = dataUrl,
                Count = dataCount
            };

            return response;

        }

        public async Task<List<UserSidePeopleWhoTookTheCourseListModel>> GetPeopleWhoTookTheCourseList(int trainingId, int pageSize, int pageIndex)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);
            var query = await this.DB.GetUserSidePeopleWhoTookTheCourseList(trainingId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var data = query.Select(async Q => new UserSidePeopleWhoTookTheCourseListModel
            {
                EmployeeId = Q.EmployeeId,
                EmployeeName = Q.EmployeeName,
                Position = Q.Position,
                ImageUrl = await FileService.GetFileAsync(Q.BlobId.ToString(), Q.MIME)
            }).Select(Q => Q.Result)
           .ToList();

            return data;

        }

        /// <summary>
        /// To get Course taken by user
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<int> GetTotalAssignee(int trainingId)
        {
            var data = await this.DB.GetUserSidePeopleWhoTookTheCourseList(trainingId).CountAsync();

            return data;
        }

        public async Task<(ModuleContentViewPaginationModel, bool IsEnrolled)> GetCourseContentPagination(string user, int? trainingId, int? courseId, int? setupModuleId, int? index)
        {
            var data = await GetCourseContent(user, trainingId, courseId, setupModuleId);
            var totalData = data.Item1.Count();
            var itemPerPages = 5;
            var newIndex = index.GetValueOrDefault();
            var datas = data.Item1.Skip(newIndex * itemPerPages).Take(5).ToList();
            var totalPages = 0;

            try
            {
                totalPages = (int)Math.Round((double)totalData / (double)itemPerPages);
            }
            catch (Exception)
            {
                totalPages = 0;
            }

            var returnData = new ModuleContentViewPaginationModel
            {
                Modules = datas,
                TotalData = totalData,
                Index = newIndex,
                ItemPerPage = itemPerPages,
                TotalPages = totalPages
            };

            return (returnData, data.IsEnrolled);
        }

        /// <summary>
        /// untuk mendapatkan isi konten dari course
        /// </summary>
        /// <param name="user"></param>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public async Task<(List<ModuleContentViewModel>, bool IsEnrolled)> GetCourseContent(string user, int? trainingId, int? courseId, int? setupModuleId)
        {
            if (trainingId != null && trainingId != 0)
            {
                var isEnrolled = false;
                var enrollLearnings = await this
                    .DB
                    .EnrollLearnings
                    .Where(Q => Q.TrainingId == trainingId && Q.EmployeeId.ToLower() == user.ToLower())
                    .Select(Q => new
                    {
                        Q.IsEnrolled,
                        Q.RemedialLevel,
                        Q.IsPassed,
                        Q.IsJoined
                    })
                    .FirstOrDefaultAsync();

                if (enrollLearnings?.IsEnrolled == true && enrollLearnings?.IsJoined == true)
                {
                    isEnrolled = enrollLearnings.IsEnrolled;
                }
                else
                {
                    var allSetupModule = new List<SetupModules>();

                    if (setupModuleId != null && setupModuleId != 0)
                    {

                        allSetupModule = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).ToListAsync();
                    }
                    else if (courseId != null && courseId != 0)
                    {
                        allSetupModule = await this.DB.SetupModules.Where(Q => Q.CourseId == courseId && Q.IsDeleted == false).ToListAsync();
                    }
                    else
                    {
                        var q = await this.DB.TrainingModuleMappings.Where(x => x.TrainingId == trainingId).Select(x => x.SetupModuleId).ToListAsync();
                        if (q != null)
                        {

                            allSetupModule = await this.DB.SetupModules.Where(Q => q.Contains(Q.SetupModuleId) && Q.IsDeleted == false).ToListAsync();
                        }
                    }

                    var response = new List<ModuleContentViewModel>();

                    foreach (var subData in allSetupModule)
                    {
                        if (subData.ModuleId != null)
                        {

                            var queryWithoutEnrollLearnining = await this.DB.GetCourseOverviewQueryWithoutEnrollLearningsBySetupModuleId(subData.SetupModuleId).ToListAsync();

                            var datas = queryWithoutEnrollLearnining.GroupBy(Q => Q.ModuleId).Select(async Q => new ModuleContentViewModel
                            {
                                Orders = Q.FirstOrDefault().Orders,
                                SetupModuleId = Q.FirstOrDefault().SetupModuleId,
                                ModuleId = Q.FirstOrDefault().ModuleId,
                                ImageUrl = Q.FirstOrDefault().BlobId == null ? "" : await FileService.GetFileAsync(Q.FirstOrDefault().BlobId.ToString(), Q.First().Mime),
                                ContentName = Q.FirstOrDefault().ContentName,
                                ModuleType = Q.FirstOrDefault().ModuleType,
                                ModuleTimeLength = Q.FirstOrDefault().ModuleTimeLength,
                                ModuleTotalPoints = Q.FirstOrDefault().ModuleTotalPoints,
                                IsDownloaded = await GetStatusDownload(Q.FirstOrDefault().ModuleId.ToString(), user) || await GetStatusDownload(Q.FirstOrDefault().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user),
                                IsAvailable = true,
                            }).Select(Q => Q.Result).ToList();


                            response.AddRange(datas);
                        }
                        else
                        {
                            var datas = DB.SetupModules.Where(x => x.AssesmentId != null).Where(x => x.SetupModuleId == subData.SetupModuleId).Include(x => x.Assesment).Select(Q => new ModuleContentViewModel
                            {
                                Orders = (int)Q.Order,
                                SetupModuleId = Q.SetupModuleId,
                                AssesmentId = Q.AssesmentId,
                                // ImageUrl = await FileService.GetFileAsync(Q.Assesment..BlobId.ToString(), Q.First().Mime),
                                ContentName = Q.Assesment.Name,
                                ModuleType = Q.EnumCategoryPreDuringPost,
                                IsAvailable = true,

                                //    IsDownloaded = await GetStatusDownload(Q.FirstOrDefault().ModuleId.ToString(), user) || await GetStatusDownload(Q.FirstOrDefault().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user)
                            }).ToList();

                            response.AddRange(datas);
                        }
                    }


                    return (response.OrderBy(x => x.Orders).ToList(), false);
                }

                //if (enrollLearnings.RemedialLevel > 0 && enrollLearnings.IsPassed == null)
                //{
                //    //var querys = from t in DB.Trainings
                //    //            join sm in DB.SetupModules
                //    //            on t.CourseId equals sm.CourseId into tsmJoin
                //    //            from tsm in tsmJoin
                //    //            join elt in DB.EnrollLearningTimes
                //    //            on t.TrainingId equals elt.EnrollLearning.TrainingId into eltJoin
                //    //            from elt in eltJoin.DefaultIfEmpty()
                //    //            join mt in DB.MaterialTypes 
                //    //            on tsm.Module.MaterialTypeId equals mt.MaterialTypeId into mtJoin
                //    //            from mt in mtJoin.DefaultIfEmpty()
                //    //            join tp.timepoin
                //}

                var remedialLevel = enrollLearnings.RemedialLevel;
                var isPassed = enrollLearnings.IsPassed;
                var responses = new List<ModuleContentViewModel>();

                if (isPassed == null)
                {

                    var query = await DB.GetCourseOverviewQuery(user, trainingId.GetValueOrDefault()).AsNoTracking().ToListAsync();

                    /* if (remedialLevel > 0)
                     {
                         query = query.Where(Q => Q.IsForRemedial == true).ToList();
                         //if (isPassed == null)
                         //{
                         //    query = query.Where(Q => Q.IsForRemedial.Value == true).ToList();
                         //}
                     }
                     else if (remedialLevel == 0)
                     {
                         query = query.Where(Q => Q.IsForRemedial == false).ToList();
                         //if (isPassed == null)
                         //{
                         //    query = query.Where(Q => Q.IsForRemedial.Value == false).ToList();
                         //}
                     }*/

                    query = query.OrderBy(Q => Q.IsForRemedial).ThenBy(Q => Q.SetupModuleId).ToList();



                    var data = query.Where(x => x.ModuleId != null).GroupBy(Q => Q.ModuleId).Select(async Q => new ModuleContentViewModel
                    {
                        Orders = Q.First().Orders,
                        SetupModuleId = Q.First().SetupModuleId,
                        ModuleId = Q.First().ModuleId,
                        ImageUrl = Q.First().BlobId == null ? "" : (await FileService.GetFileAsync(Q.First().BlobId.ToString(), Q.First().Mime)),
                        FileSize = Q.First().MaterialBlobId == null ? 0 : (await FileService.GetFileSize(Q.First().MaterialBlobId.ToString(), Q.First().MaterialMime)),
                        ContentName = Q.First().ContentName,
                        ModuleType = Q.First().ModuleType,
                        ModuleTimeLength = Q.First().ModuleTimeLength,
                        ModuleTotalPoints = Q.First().ModuleTotalPoints,
                        StartTime = Q.First().StartTime,
                        EndTime = Q.First().EndTime,
                        IsPublished = Q.First().IsPublished,
                        ModuleStartTime = Q.First().ModuleStartTime,
                        ModuleEndTime = Q.First().ModuleEndTime,
                        CourseId = Q.First().CourseId,
                        IsAvailable = true,
                        IsDownloaded = Q.First().ModuleId == null ? false : (await GetStatusDownload(Q.First().ModuleId.ToString(), user) || await GetStatusDownload(Q.First().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user))
                    }).Select(Q => Q.Result)
                    .ToList();

                    var coursesId = query.Select(x => x.CourseId).ToList();
                    var listExisting = DB.SetupModules.Where(x => coursesId.Contains(x.CourseId)).Where(x => x.AssesmentId != null).Select(x => x.SetupModuleId).ToList();

                    if (listExisting == null)
                    {
                        listExisting = new List<int>();
                    }
                    var assesmentData = DB.SetupModules.Where(x => listExisting.Contains(x.SetupModuleId)).Where(x => x.AssesmentId != null).Where(x => !x.IsDeleted).Select(x => new ModuleContentViewModel
                    {
                        SetupModuleId = x.SetupModuleId,
                        ContentName = x.Assesment.Name,
                        AssesmentId = x.AssesmentId,
                        IsAvailable = true,
                        ModuleType = x.EnumCategoryPreDuringPost,
                        Orders = (int)x.Order,
                        IsPublished = x.IsPublished,
                        //ModuleStartTime = DB.EnrollLearningTimes.Include(el=>el.EnrollLearning).ThenInclude(tr=> tr.Training).Where(q=> q.SetupModuleId == x.SetupModuleId).FirstOrDefault().EnrollLearning == null ? null : DB.EnrollLearningTimes.Include(el=>el.EnrollLearning).ThenInclude(tr=> tr.Training).Where(q=> q.SetupModuleId == x.SetupModuleId).FirstOrDefault().EnrollLearning.Training.StartDate,
                        //ModuleEndTime = DB.EnrollLearningTimes.Include(el=>el.EnrollLearning).ThenInclude(tr=> tr.Training).Where(q=> q.SetupModuleId == x.SetupModuleId).FirstOrDefault().EnrollLearning == null ? null : DB.EnrollLearningTimes.Include(el=>el.EnrollLearning).ThenInclude(tr=> tr.Training).Where(q=> q.SetupModuleId == x.SetupModuleId).FirstOrDefault().EnrollLearning.Training.EndDate,
                        StartTime = DB.EnrollLearningTimes.Include(el => el.EnrollLearning).Where(el => el.EnrollLearning.TrainingId == trainingId).Where(el => el.EnrollLearning.EmployeeId == user).Where(pz => pz.SetupModuleId == x.SetupModuleId).FirstOrDefault().StartTime,
                        EndTime = DB.EnrollLearningTimes.Include(el => el.EnrollLearning).Where(el => el.EnrollLearning.TrainingId == trainingId).Where(el => el.EnrollLearning.EmployeeId == user).Where(pz => pz.SetupModuleId == x.SetupModuleId).FirstOrDefault().EndTime,

                    }).ToList();


                    responses.AddRange(assesmentData);

                    responses.AddRange(data);

                    responses = responses.OrderBy(x => x.Orders).ToList();


                    //logic lock unlock 
                    var todayDate = DateTime.UtcNow.ToIndonesiaTimeZone();
                    foreach (var item in responses.Select((value, i) => new { i, value }))
                    {

                        //semua modul yang tanggalnya belum lewat akan disabled
                        if (item.value.ModuleStartTime != null && ((todayDate.Date >= item.value.ModuleStartTime.Value.Date) == false))
                        {
                            item.value.IsAvailable = false;
                        }


                        //kalau belum lulus dan belum gagal dan tanggalnya udah lewat
                        else if (isPassed == null)
                        {
                            //kalau remedial
                            if (remedialLevel > 0 && remedialLevel < 3)
                            {
                                item.value.IsAvailable = true;
                            }
                            //kalau ga remedial
                            else if (remedialLevel == 0)
                            {
                                //kalau modul locked
                                if (item.value.IsPublished == false)
                                {
                                    item.value.IsAvailable = false;

                                    //looping cek modul sebelumnya udah dikerjakan apa belum
                                    //for (int j = 0; j < item.i; j++)
                                    //{
                                    if (item.i > 0)
                                    {
                                        var j = item.i - 1;
                                        if (responses[j] != null)
                                        {
                                            //modul dicek berdasarkan endTime 
                                            if (responses[j].EndTime == null)
                                            {
                                                //overwrite isAvailable jika ada modul sebelumnya yang belum dikerjakan
                                                item.value.IsAvailable = false;
                                                // break;
                                            }
                                            else
                                            {
                                                item.value.IsAvailable = true;
                                            }
                                        }
                                    }
                                    // }
                                }
                                //kalau modul unlocked, 
                                else item.value.IsAvailable = true;
                            }
                            if (item.i == 0)
                            {
                                item.value.IsAvailable = true;
                            }
                        }
                        //kalau gagal atau lulus semua modul terbuka dan tanggalnya udah lewat
                        else item.value.IsAvailable = true;
                    }

                    //untuk melakukan pengecekan modul yang terakhir dikerjakan 
                    if (remedialLevel == 0)
                    {
                        //menghitung apakah masih ada endTime yang null, 
                        var countModulLeft = data.Where(Q => Q.EndTime == null).Count();

                        //jika ada 1 modul yang terakhir yang belum dikerjakan, digunakan logic dibawah
                        if (countModulLeft == 1)
                        {
                            var getIndex = data.FindIndex(Q => Q.EndTime == null);
                            data[getIndex].IsLast = true;
                        }

                        //jika smua modul udah dikerjakan, untuk mengecek modul terakhir digunakan logic dibawah
                        if (countModulLeft == 0)
                        {
                            var getLastestModule = data.OrderByDescending(Q => Q.EndTime).FirstOrDefault();

                            if (getLastestModule != null)
                            {
                                var getIndex = data.FindIndex(Q => Q.ModuleId == getLastestModule.ModuleId);
                                data[getIndex].IsLast = true;
                            }
                        }
                    }
                }
                else
                {

                    var query = await DB.GetCourseOverviewPassedOrFailedQuery(user, trainingId.GetValueOrDefault()).AsNoTracking().ToListAsync();
                    query = query.OrderBy(Q => Q.IsForRemedial).ThenBy(Q => Q.SetupModuleId).ToList();

                    var data = query.GroupBy(Q => Q.ModuleId).Select(async Q => new ModuleContentViewModel
                    {
                        Orders = Q.First().Orders,
                        SetupModuleId = Q.First().SetupModuleId,
                        ModuleId = Q.First().ModuleId,
                        ImageUrl = Q.First().BlobId == null ? "" : (await FileService.GetFileAsync(Q.First().BlobId.ToString(), Q.First().Mime)),
                        FileSize = Q.First().MaterialBlobId == null ? 0 : (await FileService.GetFileSize(Q.First().MaterialBlobId.ToString(), Q.First().MaterialMime)),
                        ContentName = Q.First().ContentName,
                        ModuleType = Q.First().ModuleType,
                        ModuleTimeLength = Q.First().ModuleTimeLength,
                        ModuleTotalPoints = Q.First().ModuleTotalPoints,
                        StartTime = Q.First().StartTime,
                        EndTime = Q.First().EndTime,
                        IsPublished = Q.First().IsPublished,
                        ModuleStartTime = Q.First().ModuleStartTime,
                        ModuleEndTime = Q.First().ModuleEndTime,
                        CourseId = Q.First().CourseId,
                        IsAvailable = true,
                        IsDownloaded = Q.First().ModuleId == null ? false : (await GetStatusDownload(Q.First().ModuleId.ToString(), user) || await GetStatusDownload(Q.First().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user))
                    }).Select(Q => Q.Result)
                     .ToList();


                    var coursesId = DB.Trainings.Where(Q => Q.TrainingId == trainingId.GetValueOrDefault() && Q.IsDeleted == false).Select(Q => Q.CourseId).ToList(); 
                    var listExisting = DB.SetupModules.Where(x => coursesId.Contains(x.CourseId.GetValueOrDefault())).Where(x => x.AssesmentId != null).Select(x => x.SetupModuleId).ToList();

                    if (listExisting == null)
                    {
                        listExisting = new List<int>();
                    }
                    var assesmentData = DB.SetupModules.Where(x => listExisting.Contains(x.SetupModuleId)).Where(x => x.AssesmentId != null).Where(x => !x.IsDeleted).Select(x => new ModuleContentViewModel
                    {
                        Orders = (int)x.Order,
                        SetupModuleId = x.SetupModuleId,
                        ContentName = x.Assesment.Name,
                        AssesmentId = x.AssesmentId,
                        IsAvailable = true,
                        ModuleType = x.EnumCategoryPreDuringPost,

                    }).ToList();

                    responses.AddRange(assesmentData);

                    responses.AddRange(data);

                }
                //biar bisa
                var max = responses.Count();
                if (max > 0)
                {
                    var maxIndex = max - 1;
                    responses[maxIndex].IsLast = true;
                }

                return (responses.OrderBy(x => x.Orders).ToList(), isEnrolled);
            }

            //Kalau masuk ke courseid tidak dapat di Enrolled, makanya enrolled nya false dan menggunakan queryWithoutEnrollLearnining
            else if (courseId != null && courseId != 0)
            {
                var allSetupModule = await this.DB.SetupModules.Where(Q => Q.CourseId == courseId && Q.IsDeleted == false).ToListAsync();

                var allSetupModuleList = new List<ModuleContentViewModel>();
                var isEnrolled = false;


                foreach (var setupModuleIdbyCourse in allSetupModule)
                {
                    if (setupModuleIdbyCourse.ModuleId != null)
                    {
                        var queryWithoutEnrollLearnining = await this.DB.GetSetupModuleOverviewQueryWithoutEnrollLearnings(setupModuleIdbyCourse.SetupModuleId).ToListAsync();

                        var datas = queryWithoutEnrollLearnining.GroupBy(Q => Q.ModuleId).Select(async Q => new ModuleContentViewModel
                        {
                            SetupModuleId = Q.FirstOrDefault().SetupModuleId,
                            ModuleId = Q.FirstOrDefault().ModuleId,
                            ImageUrl = Q.FirstOrDefault().BlobId == null ? "" : await FileService.GetFileAsync(Q.FirstOrDefault().BlobId.ToString(), Q.FirstOrDefault().Mime),
                            ContentName = Q.FirstOrDefault().ContentName,
                            ModuleType = Q.FirstOrDefault().ModuleType,
                            ModuleTimeLength = Q.FirstOrDefault().ModuleTimeLength,
                            ModuleTotalPoints = Q.FirstOrDefault().ModuleTotalPoints,
                            IsAvailable = true,
                            IsDownloaded = await GetStatusDownload(Q.FirstOrDefault().ModuleId.ToString(), user) || await GetStatusDownload(Q.FirstOrDefault().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user),
                            Orders = Q.First().Orders,
                        }).Select(Q => Q.Result)
                    .ToList();

                        allSetupModuleList.AddRange(datas);
                    }
                    else
                    {

                        var datas = DB.SetupModules.Where(x => x.AssesmentId != null).Where(x => x.SetupModuleId == setupModuleIdbyCourse.SetupModuleId).Include(x => x.Assesment).Select(Q => new ModuleContentViewModel
                        {
                            SetupModuleId = Q.SetupModuleId,
                            AssesmentId = Q.AssesmentId,
                            // ImageUrl = await FileService.GetFileAsync(Q.Assesment..BlobId.ToString(), Q.First().Mime),
                            ContentName = Q.Assesment.Name,
                            ModuleType = Q.EnumCategoryPreDuringPost,
                            IsAvailable = true,
                            Orders = (int)Q.Order,
                            //    IsDownloaded = await GetStatusDownload(Q.FirstOrDefault().ModuleId.ToString(), user) || await GetStatusDownload(Q.FirstOrDefault().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user)
                        }).Select(Q => Q)
                    .ToList();

                        allSetupModuleList.AddRange(datas);
                    }
                }
                return (allSetupModuleList.OrderBy(x => x.Orders).ToList(), isEnrolled);
            }

            else if (setupModuleId != null)
            {
                var isEnrolled = false;

                var enrollLearnings = await this
                    .DB
                    .EnrollLearnings
                    .Where(Q => Q.SetupModuleId == setupModuleId && Q.EmployeeId.ToLower() == user.ToLower())
                    .Select(Q => new
                    {
                        Q.IsEnrolled,
                        Q.RemedialLevel,
                    })
                    .FirstOrDefaultAsync();

                if (enrollLearnings != null)
                {
                    isEnrolled = enrollLearnings.IsEnrolled;
                }
                else
                {
                    var queryWithoutEnrollLearnining = await this.DB.GetSetupModuleOverviewQueryWithoutEnrollLearnings(setupModuleId.GetValueOrDefault()).ToListAsync();
                    var datas = queryWithoutEnrollLearnining.GroupBy(Q => Q.ModuleId).Select(async Q => new ModuleContentViewModel
                    {
                        Orders = Q.First().Orders,
                        SetupModuleId = Q.First().SetupModuleId,
                        ModuleId = Q.First().ModuleId,
                        ImageUrl = await FileService.GetFileAsync(Q.First().BlobId.ToString(), Q.First().Mime),
                        ContentName = Q.First().ContentName,
                        ModuleType = Q.First().ModuleType,
                        ModuleTimeLength = Q.First().ModuleTimeLength,
                        ModuleTotalPoints = Q.First().ModuleTotalPoints,
                        IsAvailable = true,
                        IsDownloaded = await GetStatusDownload(Q.First().ModuleId.ToString(), user) || await GetStatusDownload(Q.First().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user)

                    }).Select(Q => Q.Result).ToList();
                    return (datas, false);
                }

                var query = await DB.GetSetupModuleOverviewQuery(user, setupModuleId.GetValueOrDefault()).ToListAsync();

                var data = query.GroupBy(Q => Q.ModuleId).Select(async Q => new ModuleContentViewModel
                {
                    SetupModuleId = Q.First().SetupModuleId,
                    ModuleId = Q.First().ModuleId,
                    ImageUrl = await FileService.GetFileAsync(Q.First().BlobId.ToString(), Q.First().Mime),
                    ContentName = Q.First().ContentName,
                    ModuleType = Q.First().ModuleType,
                    ModuleTimeLength = Q.First().ModuleTimeLength,
                    ModuleTotalPoints = Q.First().ModuleTotalPoints,
                    StartTime = Q.First().StartTime,
                    EndTime = Q.First().EndTime,
                    IsAvailable = true,
                    Orders = Q.First().Orders,
                    IsDownloaded = await GetStatusDownload(Q.First().ModuleId.ToString(), user) || await GetStatusDownload(Q.First().SetupModuleId.ToString(), user) || await GetStatusDownload(trainingId.ToString(), user)
                }).Select(Q => Q.Result)
                .ToList();
                return (data.OrderBy(x => x.Orders).ToList(), isEnrolled);
            }

            //return null, false kalo gak ada training id dan setupmoduleid
            return (new List<ModuleContentViewModel>(), false);

        }

        public async Task<ActionResult<BaseResponse>> GetModuleIsRemed(int trainingId, int setupModuleId, string employeeId, string SkillCheckGUID)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var IsConfigRemed = false;
            var IsAllScored = false;

            var setupConfig = await this
                    .DB
                    .SetupModules
                    .Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false)
                    .Select(Q => Q.EnumRemedialOption).FirstOrDefaultAsync();

            var getEnrollLearningId = await this
                    .DB
                    .EnrollLearnings
                    .Where(Q => Q.EmployeeId == employeeId && Q.TrainingId == trainingId)
                    .Select(Q => Q.EnrollLearningId).FirstOrDefaultAsync();

            var getTrainingEndDate = await this
                    .DB
                    .Trainings
                    .Where(Q => Q.TrainingId == trainingId && Q.IsDeleted == false)
                    .Select(Q => Q.EndDate).FirstOrDefaultAsync();

            //var getTaskAnswers = await this
            //    .DB
            //    .TaskAnswers
            //    .Where(Q => Q.TrainingId == trainingId && Q.SetupModuleId == setupModuleId && Q.EmployeeId == employeeId)
            //    .FirstOrDefaultAsync();

            var listAssesments = this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Where(Q => Q.AssesmentId != null).Select(Q => Q.AssesmentId).ToList();

            if (setupConfig.Trim() != "No Need")
            {
                switch (setupConfig.Trim())
                {
                    case "During Training Schedule":
                        if (thisDate.Date <= getTrainingEndDate.Value.Date)
                            IsConfigRemed = true;
                        break;
                    case "Limit":
                        var checkLimit = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Select(Q => Q.RemedialLimit).FirstOrDefaultAsync();
                        if (this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Any(x => x.AssesmentId != null))
                        {
                            var jmlAttempt = new List<float>();
                            if (SkillCheckGUID.Trim() != "")
                            {
                                jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => listAssesments.Contains(Q.AssesmentGUID) && Q.EmployeeGUID == employeeId && Q.SkillCheckGUID == SkillCheckGUID && Q.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                            }
                            else
                            {
                                jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => listAssesments.Contains(Q.AssesmentGUID) && Q.EmployeeGUID == employeeId && Q.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                            }
                            if (jmlAttempt.Count > 0)
                            {
                                if (jmlAttempt.Max() < checkLimit)
                                    IsConfigRemed = true;
                                else
                                    IsConfigRemed = false;
                            }
                            else
                            {
                                IsConfigRemed = true;
                            }
                            //assesment
                        }
                        else
                        {
                            var jmlAttempt = this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.SetupModuleId == setupModuleId && Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                            if (jmlAttempt.Count > 0)
                            {
                                if (jmlAttempt.Max() < checkLimit)
                                    IsConfigRemed = true;
                                else
                                    IsConfigRemed = false;
                                //modul
                            }
                            else
                            {
                                IsConfigRemed = true;
                            }
                        }
                        break;
                    default:
                        IsConfigRemed = false;
                        break;
                }
                if (IsConfigRemed == true)
                {
                    if (this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Any(x => x.AssesmentId != null))
                    {
                        //assesment
                        //var listAssesments = this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Where(Q => Q.AssesmentId != null).Select(Q => Q.AssesmentId).ToList();
                        var jmlAttempt = new List<float>();
                        if (SkillCheckGUID.Trim() != "")
                        {
                            jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => listAssesments.Contains(Q.AssesmentGUID) && Q.EmployeeGUID == employeeId && Q.SkillCheckGUID == SkillCheckGUID && Q.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                        }
                        else
                        {
                            jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => listAssesments.Contains(Q.AssesmentGUID) && Q.EmployeeGUID == employeeId && Q.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                        }
                        if (jmlAttempt.Count > 0)
                        {
                            var checkIsCompleteData = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == employeeId && listAssesments.Contains(x.AssesmentGUID) && x.SkillCheckGUID == SkillCheckGUID && x.TrainingId == trainingId).OrderBy(x => x.SkillCheckGUID).ThenBy(n => n.Attempts).AsQueryable().ToListAsync();
                            var checkIsnotCompleteData = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == employeeId && listAssesments.Contains(x.AssesmentGUID) && x.SkillCheckGUID == SkillCheckGUID && x.TrainingId == trainingId).Where(x => x.IsScored == true && x.IsFinished == true && x.IsDraft == false).OrderBy(x => x.SkillCheckGUID).ThenBy(n => n.Attempts).AsQueryable().ToListAsync();
                            if (checkIsCompleteData.Count == checkIsnotCompleteData.Count)
                            {
                                IsAllScored = true;
                            }
                            else
                            {
                                IsAllScored = false;
                            }
                        }
                        else
                        {
                            IsAllScored = true;
                        }
                    }
                    else
                    {
                        var getTaskId = await (from itm in DB.Tasks
                                               join mdl in DB.Modules on itm.ModuleId equals mdl.ModuleId
                                               join smd in DB.SetupModules on mdl.ModuleId equals smd.ModuleId
                                               where smd.ModuleId != null && smd.SetupModuleId == setupModuleId && (itm.QuestionTypeId == 6 || itm.QuestionTypeId == 7 || itm.QuestionTypeId == 11)
                                               select itm.TaskId).ToListAsync();

                        var jmlAttempt = this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.SetupModuleId == setupModuleId && Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                        if (jmlAttempt.Count > 0)
                        {
                            var checkIsCompleteData = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(x => x.TaskAnswer.SetupModuleId == setupModuleId && x.TaskAnswer.EmployeeId == employeeId && x.TaskAnswer.TrainingId == trainingId).ToListAsync();
                            var checkIsnotCompleteData = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(x => x.TaskAnswer.SetupModuleId == setupModuleId && x.TaskAnswer.EmployeeId == employeeId && x.TaskAnswer.TrainingId == trainingId).Where(x => x.IsChecked == true).ToListAsync();
                            //if (!await this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.SetupModuleId == setupModuleId && Q.TaskAnswer.EmployeeId == employeeId).Where(Q => getTaskId.Contains(Q.TaskId)).Select(Q => !Q.IsChecked).FirstOrDefaultAsync())
                            if (checkIsCompleteData.Count == checkIsnotCompleteData.Count)
                            {
                                IsAllScored = true;
                            }
                            else
                            {
                                IsAllScored = false;
                            }
                        }
                        else
                        {
                            IsAllScored = true;
                        }
                    }
                }
            }
            else
            {
                if (this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Any(x => x.AssesmentId != null))
                {
                    //var listAssesments = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId && Q.IsDeleted == false).Where(Q => Q.AssesmentId != null).Select(Q => Q.AssesmentId).ToListAsync();
                    var jmlAttempt = new List<float>();
                    if (SkillCheckGUID.Trim() != "")
                    {
                        jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => listAssesments.Contains(Q.AssesmentGUID) && Q.EmployeeGUID == employeeId && Q.SkillCheckGUID == SkillCheckGUID && Q.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                    }
                    else
                    {
                        jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => listAssesments.Contains(Q.AssesmentGUID) && Q.EmployeeGUID == employeeId && Q.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                    }
                    if (jmlAttempt.Count == 0)
                    {
                        IsConfigRemed = true;
                        IsAllScored = true;
                    }
                    //assesment
                }
                else
                {
                    var jmlAttempt = this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.SetupModuleId == setupModuleId && Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TrainingId == trainingId).Select(Q => Q.Attempts).ToList();
                    if (jmlAttempt.Count == 0)
                    {
                        IsConfigRemed = true;
                        IsAllScored = true;
                    }
                    //modul
                }
            }
            if (IsConfigRemed == true && IsAllScored == true)
            {
                //isremed true
                //ganti base response
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            else
            {
                //isremed false
                //ganti base response
                var msg = new Message
                {
                    En = "Your module is not able to do Remedial",
                    Id = "Modul yang dipilih tidak dapat Remedial"
                };
                return StatusCode(400, BaseResponse.BadRequest(msg));
            }
        }

        public async Task<CourseReviewSummaryViewModel> GetCourseReview(int trainingId, int limit, int page)
        {
            var query = await (from c in DB.Courses

                               join t in DB.Trainings on c.CourseId equals t.CourseId

                               join cr in DB.TrainingRatings on c.CourseId equals cr.CourseId

                               join e in DB.Employees on cr.EmployeeId equals e.EmployeeId

                               join b in DB.Blobs on e.BlobId equals b.BlobId
                               into eb
                               from b in eb.DefaultIfEmpty()

                                   //where cr.Review != null
                               where cr.TrainingId == trainingId
                               where t.TrainingId == trainingId
                               select new
                               {
                                   e.EmployeeName,
                                   cr.CreatedAt,
                                   cr.RatingScore,
                                   cr.Review,
                                   e.BlobId,
                                   b.Mime
                               }).AsNoTracking()
                        .ToListAsync();

            if (query.Count() == 0)
            {
                return null;
            }

            var reviews = query.Select(async Q => new CourseReviewViewModel
            {
                EmployeeName = Q.EmployeeName,
                SubmitTime = Q.CreatedAt,
                Score = Q.RatingScore,
                Review = Q.Review,
                ImageUrl = await FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime)
            }).Select(Q => Q.Result).OrderByDescending(Q => Q.Score)
            .ToList();

            int five = 0, four = 0, three = 0, two = 0, one = 0;

            foreach (var item in reviews)
            {
                if (item.Score == 1 || item.Score == 2)
                {
                    one++;
                }
                if (item.Score == 3 || item.Score == 4)
                {
                    two++;
                }
                if (item.Score == 5 || item.Score == 6)
                {
                    three++;
                }
                if (item.Score == 7 || item.Score == 8)
                {
                    four++;
                }
                if (item.Score == 9 || item.Score == 10)
                {
                    five++;
                }
            }

            var data = new CourseReviewSummaryViewModel
            {
                Average = reviews.Select(x => x.Score).Average() / 2,
                CountOverall = reviews.Count(),
                Count5Stars = five,
                Count4Stars = four,
                Count3Stars = three,
                Count2Stars = two,
                Count1Stars = one,
                Reviews = reviews.Skip(page).Take(limit).ToList(),
            };
            data.Average = Math.Round(data.Average, 1, MidpointRounding.AwayFromZero);


            return data;
        }

        /// <summary>
        /// untuk mengassign member ke training yang dipilih
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AssignTrainingMember(string user, TeamMemberAssignModel model)
        {
            //await this.QueueCheckUsers(model.EmployeeId, model.TrainingId, model.SetupModuleId);

            var isTeamLeader = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == user.ToLower())
                .Select(Q => Q.IsTeamLeader)
                .FirstOrDefaultAsync();

            var query = await (from m in DB.Modules
                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join c in DB.Courses on sm.CourseId equals c.CourseId into smc
                               from c in smc.DefaultIfEmpty()
                               join t in DB.Trainings on c.CourseId equals t.CourseId into ct
                               from t in ct.DefaultIfEmpty()
                               where t.TrainingId == model.TrainingId
                               select new
                               {
                                   setupmoduleId = sm.SetupModuleId
                               }).AsNoTracking().FirstOrDefaultAsync();

            if (isTeamLeader == true)
            {
                List<AssignedLearnings> modelList = new List<AssignedLearnings>();
                List<EnrollLearnings> enrollLearningsModelList = new List<EnrollLearnings>();
                List<EnrollLearningTimes> enrollLearningTimesModelList = new List<EnrollLearningTimes>();
                var learningName = "";

                if (model.SetupModuleId.HasValue)
                {
                    learningName = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == model.SetupModuleId).Select(Q => Q.Module.ModuleName).FirstOrDefaultAsync();
                }
                else
                {
                    learningName = await this.DB.Trainings.Where(Q => Q.TrainingId == model.TrainingId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();
                }

                var assignName = await this.DB.Employees.Where(Q => Q.EmployeeId.ToLower() == user.ToLower()).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();

                foreach (var id in model.EmployeeId)
                {
                    var assignLearning = new AssignedLearnings();

                    if (query != null)
                    {
                        assignLearning = new AssignedLearnings
                        {
                            AssignedTo = id,
                            AssignedBy = user,
                            AssignedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                            TrainingId = model.TrainingId,
                            SetupModuleId = query.setupmoduleId
                        };
                    }
                    else
                    {
                        assignLearning = new AssignedLearnings
                        {
                            AssignedTo = id,
                            AssignedBy = user,
                            AssignedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                            TrainingId = model.TrainingId,
                        };
                    }

                    modelList.Add(assignLearning);
                    DB.AssignedLearnings.AddRange(modelList);

                    var enroll = new EnrollLearnings
                    {
                        EmployeeId = id,
                        TrainingId = model.TrainingId,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        IsEnrolled = false,
                        IsQueued = true
                    };
                    enrollLearningsModelList.Add(enroll);
                    DB.EnrollLearnings.AddRange(enrollLearningsModelList);

                    var enrollMapping = new EnrollLearningTimes();

                    if (query != null)
                    {
                        enrollMapping = new EnrollLearningTimes
                        {
                            EnrollLearningId = enroll.EnrollLearningId,
                            SetupModuleId = query.setupmoduleId,
                            StartTime = null
                        };
                    }
                    else
                    {
                        enrollMapping = new EnrollLearningTimes
                        {
                            EnrollLearningId = enroll.EnrollLearningId,
                            StartTime = null
                        };
                    }

                    enrollLearningTimesModelList.Add(enrollMapping);
                    DB.EnrollLearningTimes.AddRange(enrollLearningTimesModelList);

                    var messageContent = "Anda mendapatkan new assignment " + learningName + " dari " + assignName + ". Silahkan akses Learning Queue di My Learning";

                    var newMobileInbox = new MobileInboxes
                    {
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        CreatedBy = user,
                        EmployeeId = id,
                        IsRead = false,
                        Message = messageContent,
                        MobileInboxTypeId = (int)MobileInboxType.AssignLearning,
                        Title = "Assigned New Learning"
                    };

                    this.DB.MobileInboxes.Add(newMobileInbox);

                }


            }


            await DB.SaveChangesAsync();

            return;
        }

        /// <summary>
        /// untuk mengassign member ke module yang dipilih
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AssignModuleMember(string user, TeamMemberAssignModel model)
        {
            //await this.QueueCheckUsers(model.EmployeeId, model.TrainingId, model.SetupModuleId);

            var isTeamLeader = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == user.ToLower())
                .Select(Q => Q.IsTeamLeader)
                .FirstOrDefaultAsync();

            if (isTeamLeader == true)
            {
                List<AssignedLearnings> modelList = new List<AssignedLearnings>();
                List<EnrollLearnings> enrollLearningsModelList = new List<EnrollLearnings>();
                List<EnrollLearningTimes> enrollLearningTimesModelList = new List<EnrollLearningTimes>();

                foreach (var id in model.EmployeeId)
                {
                    var assignLearning = new AssignedLearnings();

                    assignLearning = new AssignedLearnings
                    {
                        AssignedTo = id,
                        AssignedBy = user,
                        AssignedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        TrainingId = null,
                        SetupModuleId = model.SetupModuleId
                    };

                    modelList.Add(assignLearning);
                    DB.AssignedLearnings.AddRange(modelList);

                    var enroll = new EnrollLearnings
                    {
                        EmployeeId = id,
                        TrainingId = null,
                        SetupModuleId = model.SetupModuleId,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        IsEnrolled = false,
                        IsQueued = true
                    };
                    enrollLearningsModelList.Add(enroll);
                    DB.EnrollLearnings.AddRange(enrollLearningsModelList);

                    var enrollMapping = new EnrollLearningTimes();

                    enrollMapping = new EnrollLearningTimes
                    {
                        EnrollLearningId = enroll.EnrollLearningId,
                        SetupModuleId = model.SetupModuleId.GetValueOrDefault(),
                        StartTime = null
                    };

                    enrollLearningTimesModelList.Add(enrollMapping);
                    DB.EnrollLearningTimes.AddRange(enrollLearningTimesModelList);
                }


            }


            await DB.SaveChangesAsync();

            return;
        }

        public async Task<TeamMemberGridModel> SearchMember(MemberGridFilter filter, string employeeId)
        {
            var tamEmployee = await this.DB.TamemployeeStructure.Where(Q => Q.NoReg == employeeId && Q.Staffing == 100).FirstOrDefaultAsync();
            var outletId = await this.DB.Employees.Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower()).Select(Q => Q.OutletId).FirstOrDefaultAsync();
            var queryTeam = await (from e in this.DB.Employees
                                   join t in this.DB.TeamDetails
                                   on e.EmployeeId equals t.EmployeeId into at
                                   from atn in at.DefaultIfEmpty()
                                   where e.EmployeeId.ToLower() == employeeId.ToLower()
                                   select new
                                   {
                                       atn.Team.TeamLeaderId,
                                       TeamId = atn.Team.TeamId > 0 ? atn.TeamId : 0
                                   }).FirstOrDefaultAsync();
            var isCurrentEmployeeTeamLeader = employeeId.ToLower() == (queryTeam.TeamLeaderId == null ? "" : queryTeam.TeamLeaderId.ToLower());
            var isTamEmployee = tamEmployee != null && string.IsNullOrEmpty(outletId) == true;
            if (queryTeam.TeamId < 0)
            {
                return null;
            }
            if (string.IsNullOrEmpty(filter.EmployeeName))
            {
                filter.EmployeeName = "";
            }
            else
            {
                filter.EmployeeName = filter.EmployeeName.ToLower();
            }

            var query = await (from t in DB.TeamDetails

                               join epm in DB.EmployeePositionMappings
                               on t.EmployeeId equals epm.EmployeeId

                               join b in DB.Blobs
                               on epm.Employee.BlobId equals b.BlobId
                               into blob

                               from blobs in blob.DefaultIfEmpty()
                               where
                               epm.Employee.EmployeeName.ToLower().Contains(filter.EmployeeName)
                               &&
                               t.TeamId == queryTeam.TeamId
                               &&
                               epm.EmployeeId != employeeId
                               select new
                               {
                                   epm.EmployeeId,
                                   epm.Employee.EmployeeName,
                                   BlobId = epm.Employee.BlobId != null ? epm.Employee.BlobId.ToString() : "",
                                   Position = epm.Position.PositionName
                               })
                               .AsNoTracking()
                               .ToListAsync();

            var listEmployeeIds = query.Select(Q => Q.EmployeeId.ToLower()).ToList();

            var listEmployeeEnrolledAlready = await this.DB.EnrollLearnings.Where(Q => Q.TrainingId == filter.TrainingId && listEmployeeIds.Contains(Q.EmployeeId.ToLower()) && (Q.IsQueued == true || Q.IsEnrolled == true)).Select(Q => Q.EmployeeId).ToListAsync();
            var listEmployeeAldreadyAssigned = await this.DB.AssignedLearnings.Where(Q => Q.TrainingId == filter.TrainingId && listEmployeeIds.Contains(Q.AssignedTo.ToLower())).Select(Q => Q.AssignedTo).ToListAsync();

            // Orang Dealer
            var data = query.GroupBy(Q => Q.EmployeeId)
                       .Select(Q => new TeamMemberDetail
                       {
                           EmployeeId = Q.Key,
                           Name = Q.Select(X => X.EmployeeName).FirstOrDefault(),
                           Position = Q.Select(X => X.Position).FirstOrDefault(),
                           ImageUrl = FileService.GetFileDetailAsync(Q.Select(X => X.BlobId).First()).Result.FileUrl,
                           IscChecked = false,
                           IsValidAsignee = isCurrentEmployeeTeamLeader && (listEmployeeEnrolledAlready.Where(T => T.ToLower() == Q.Key.ToLower()).Any() == false && listEmployeeAldreadyAssigned.Where(T => T.ToLower() == Q.Key.ToLower()).Any() == false)
                       })
                       .ToList();

            // Orang TAM
            if (isTamEmployee && isCurrentEmployeeTeamLeader)
            {
                var validAsignees = await GetValidTamAsigneeEmployeeIds(data, tamEmployee.TalentLevel);
                foreach (var element in data)
                {
                    if (validAsignees.Contains(element.EmployeeId) && (listEmployeeEnrolledAlready.Where(T => T.ToLower() == element.EmployeeId.ToLower()).Any() == false && listEmployeeAldreadyAssigned.Where(T => T.ToLower() == element.EmployeeId.ToLower()).Any()) == false)
                    {
                        element.IsValidAsignee = true;
                    }
                    else
                    {
                        element.IsValidAsignee = false;
                    }
                }
            }

            var grid = new TeamMemberGridModel
            {
                TeamMember = data
            };

            return grid;
        }
        private async Task<List<string>> GetValidTamAsigneeEmployeeIds(List<TeamMemberDetail> memberList, int tamEmployeeTalentLevel)
        {
            var maximumLevelBelow = 2;
            var employeeIds = memberList.Select(Q => Q.EmployeeId).ToList();
            return await this.DB.TamemployeeStructure
                                .Where(Q => Q.Staffing == 100
                                && employeeIds.Contains(Q.NoReg)
                                && Q.TalentLevel >= tamEmployeeTalentLevel - maximumLevelBelow
                                )
                                .Select(Q => Q.NoReg)
                                .ToListAsync();
        }
        /// <summary>
        /// untuk mendapatkan isi content dari module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="setupModuleId"></param>
        /// <returns></returns>
        public async Task<GetModule> GetModuleContent(string user, int moduleId, int setupModuleId, int trainingId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var query = new List<GetModuleTemp>();
            if (DB.EnrollLearnings.Where(Q => Q.EmployeeId == user && Q.TrainingId == trainingId).Any())
            {
                query = await (from el in DB.EnrollLearnings

                               join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId into elt2
                               from elt in elt2.DefaultIfEmpty()

                               join sm in DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId

                               join m in DB.Modules on sm.ModuleId equals m.ModuleId

                               join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId

                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join st in DB.SetupTasks on sm.SetupModuleId equals st.SetupModuleId into stsm2
                               from st in stsm2.DefaultIfEmpty()

                               join b in DB.Blobs on m.MaterialBlobId equals b.BlobId into b2
                               from b in b2.DefaultIfEmpty()

                               where sm.SetupModuleId == setupModuleId && sm.ModuleId == moduleId && el.EmployeeId == user && (el.TrainingId == trainingId || el.SetupModuleId == setupModuleId)
                               select new GetModuleTemp
                               {
                                   SetupModuleId = sm.SetupModuleId,
                                   ModuleName = m.ModuleName,
                                   Duration = tp.Time,
                                   point = tp.Points,
                                   ModuleDescription = m.ModuleDescription,
                                   MinimumScore = sm.MinimumScore == null ? 0 : sm.MinimumScore,
                                   MaterialType = mt.MaterialTypeName,
                                   FileUrl = m.MaterialTypeId != MaterailTypeEnum.GameId ? m.MaterialBlobId.ToString() : m.MaterialLink,
                                   FileName = b.Name,
                                   Mime = b.Mime,
                                   isForRemedial = sm.IsForRemedial == null ? false : sm.IsForRemedial,
                                   EndTime = elt.EndTime.HasValue ? elt.EndTime : null,
                                   StartTime = elt.StartTime.HasValue ? elt.StartTime : null,
                                   TestTime = st == null ? 0 : st.TestTime,
                                   IsDownloadble = m.MaterialDownloadable
                               }).AsNoTracking()
                               .Distinct()
                         .ToListAsync();
            }
            else
            {
                query = await (from el in DB.EnrollLearnings

                               join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId into elt2
                               from elt in elt2.DefaultIfEmpty()

                               join sm in DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId

                               join m in DB.Modules on sm.ModuleId equals m.ModuleId

                               join tp in DB.TimePoints on sm.TimePointId equals tp.TimePointId

                               join mt in DB.MaterialTypes on m.MaterialTypeId equals mt.MaterialTypeId

                               join st in DB.SetupTasks on sm.SetupModuleId equals st.SetupModuleId into stsm2
                               from st in stsm2.DefaultIfEmpty()

                               join b in DB.Blobs on m.MaterialBlobId equals b.BlobId into b2
                               from b in b2.DefaultIfEmpty()

                               where sm.SetupModuleId == setupModuleId && sm.ModuleId == moduleId && el.EmployeeId == user

                               select new GetModuleTemp
                               {
                                   SetupModuleId = sm.SetupModuleId,
                                   ModuleName = m.ModuleName,
                                   Duration = tp.Time,
                                   point = tp.Points,
                                   ModuleDescription = m.ModuleDescription,
                                   MinimumScore = sm.MinimumScore == null ? 0 : sm.MinimumScore,
                                   MaterialType = mt.MaterialTypeName,
                                   FileUrl = m.MaterialTypeId != MaterailTypeEnum.GameId ? m.MaterialBlobId.ToString() : m.MaterialLink,
                                   FileName = b.Name,
                                   Mime = b.Mime,
                                   isForRemedial = sm.IsForRemedial == null ? false : sm.IsForRemedial,
                                   EndTime = elt.EndTime.HasValue ? elt.EndTime : null,
                                   StartTime = elt.StartTime.HasValue ? elt.StartTime : null,
                                   TestTime = st == null ? 0 : st.TestTime,
                                   IsDownloadble = m.MaterialDownloadable
                               }).AsNoTracking()
                               .Distinct()
                         .ToListAsync();
            }

            var dataRating = await (from c in DB.Courses
                                    join tr in DB.Trainings on c.CourseId equals tr.CourseId
                                    join sm in DB.SetupModules on c.CourseId equals sm.CourseId
                                    join mo in DB.Modules on sm.ModuleId equals mo.ModuleId
                                    join cr in DB.TrainingRatings on c.CourseId equals cr.CourseId into leftjoin
                                    from cr in leftjoin.DefaultIfEmpty()
                                    where mo.ModuleId == moduleId && sm.SetupModuleId == setupModuleId
                                    select new
                                    {
                                        RatingScore = cr == null ? 0 : cr.RatingScore,
                                    }).ToListAsync();

            var detail = query
                .Select(Q => new GetModule
                {
                    SetupModuleId = Q.SetupModuleId,
                    ModuleName = Q.ModuleName,
                    Duration = Q.Duration,
                    point = Q.point,
                    ModuleDescription = Q.ModuleDescription,
                    MinimumScore = Q.MinimumScore,
                    MaterialType = Q.MaterialType,
                    IsForRemedial = Q.isForRemedial,
                    FileUrl = Q.FileUrl == null ? null : (Q.MaterialType != MaterailTypeEnum.Game ? FileService.GetFileAsync(Q.FileUrl.ToString(), Q.Mime).Result : Q.FileUrl),
                    FileDownloadUrl = Q.FileUrl == null ? null : (Q.MaterialType != MaterailTypeEnum.Game ? FileService.GetDownloadFileAsync(Q.FileUrl.ToString(), Q.Mime).Result : Q.FileUrl),
                    FileSize = Q.FileUrl == null ? 0 : FileService.GetFileSize(Q.FileUrl.ToString(), Q.Mime).Result,
                    FileName = Q.FileName,
                    ContentType = Q.Mime,
                    StartTime = Q.StartTime,
                    EndTime = Q.EndTime,
                    TestTime = Q.TestTime,
                    IsDownloadable = Q.IsDownloadble,
                    Rating = 0,
                })
                .FirstOrDefault();

            try
            {
                var value = Convert.ToDouble(dataRating.Sum(Q => Q.RatingScore)) / Convert.ToDouble(dataRating.Count());
                var canEnroll = false;
                var getTrainingEndDate = await this
                    .DB
                    .Trainings
                    .Where(Q => Q.TrainingId == trainingId && Q.IsDeleted == false)
                    .Select(Q => Q.EndDate).FirstOrDefaultAsync();
                if (Double.IsNaN(value))
                {
                    value = 0.0;
                }
                if(thisDate.Date <= getTrainingEndDate)
                {
                    canEnroll = true;
                    detail.CanEnroll = canEnroll;
                }

                detail.Rating = value;
            }
            catch
            {
                detail.Rating = 0.0;
            }

            return detail;
        }

        public async Task<bool> PassedPrequisite(string user, int trainingId)
        {
            //Check if the user already passed the Prequisite
            var getPrerequisite = await (from t in DB.Trainings
                                         join c in DB.Courses on t.CourseId equals c.CourseId
                                         join pc in DB.CoursePrerequisiteMappings on c.CourseId equals pc.PrevCourseId
                                         where t.TrainingId == trainingId

                                         select new
                                         {
                                             CourseId = pc.NextCourseId,
                                             SetupModuleId = pc.NextSetupModuleId
                                         }).ToListAsync();

            if (getPrerequisite == null || getPrerequisite.Count() == 0)
            {
                return true;
            }

            var setupModuleIdsPrerequisite = getPrerequisite.Where(Q => Q.SetupModuleId != null).Select(Q => Q.SetupModuleId.Value).ToList();

            var courseIdsPrerequisite = getPrerequisite.Where(Q => Q.CourseId != null).Select(Q => Q.CourseId.Value).ToList();

            //Take all the completed Trainings and Setup Module
            // var completedLearning = await this.DB.LearningHistories.Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && (Q.TrainingId != null || Q.SetupModuleId != null)).Select(Q => new
            // {
            //     Q.SetupModuleId,
            //     Q.TrainingId
            // }).Distinct().ToListAsync();

            //Training atau Module yang sudah lulus
            var passedLearning = await this.DB.EnrollLearnings.Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && Q.IsPassed == true).Select(Q => new
            {
                Q.TrainingId,
                Q.SetupModuleId
            }).ToListAsync();

            if (setupModuleIdsPrerequisite.Count() > 0)
            {
                var completedSetupModuleIds = passedLearning.Where(Q => Q.SetupModuleId != null).Select(Q => Q.SetupModuleId).Distinct().ToList();

                var isSetupModulePassed = setupModuleIdsPrerequisite.Where(Q => !completedSetupModuleIds.Contains(Q)).Count() == 0;

                if (isSetupModulePassed == false)
                {
                    return false;
                }

            }

            if (courseIdsPrerequisite.Count() > 0)
            {
                var completedTrainingIds = passedLearning.Where(Q => Q.TrainingId != null).Select(Q => Q.TrainingId).Distinct().ToList();

                //Get completed Training => CourseId
                var completedCourseIds = await this.DB.Trainings.Where(Q => Q.IsDeleted == false && completedTrainingIds.Contains(Q.TrainingId)).Select(Q => Q.CourseId).Distinct().ToListAsync();

                var isCoursePassed = courseIdsPrerequisite.Where(Q => !completedCourseIds.Contains(Q)).Count() == 0;

                if (isCoursePassed == false)
                {
                    return false;
                }

                var isHaveCertificate = await this.DB.Trainings.Where(Q => courseIdsPrerequisite.Contains(Q.CourseId) && Q.EnumCertificate != null && Q.IsDeleted == false).Select(Q => Q.TrainingId).ToListAsync();

                var lstFlagCert = new List<bool>();

                foreach(var item in isHaveCertificate)
                {
                    var flaggingUserCertificate = false;
                    var isCertified = await this.DB.EmployeeCertificates.Where(Q => Q.TrainingId == item && Q.EmployeeId == user).ToListAsync();
                    if(isCertified.Count() > 0)
                    {
                        flaggingUserCertificate = true;
                    }
                    else
                    {
                        flaggingUserCertificate = false;
                    }
                    lstFlagCert.Add(flaggingUserCertificate);
                }

                if(isHaveCertificate.Count() > 0)
                {
                    if (lstFlagCert.Contains(false))
                    {
                        return false;
                    }
                }

            }

            //var isHaveCertificate = await this.DB.Trainings.Where(Q => Q.TrainingId == trainingId).Select(Q => Q.EnumCertificate).FirstOrDefaultAsync();

            //var isCertified = await this.DB.EmployeeCertificates.Where(Q => Q.TrainingId == trainingId && Q.EmployeeId == user).ToListAsync();

            //if (isHaveCertificate != null)
            //{
            //    if (isCertified.Count() == 0)
            //    {
            //        return false;
            //    }
            //}

            return true;
        }

        public async Task<bool> IsEligible(string user, int trainingId)
        {
            var checkUserEligible = await (from epm in DB.EmployeePositionMappings
                                           join tpm in DB.TrainingPositionMappings on epm.PositionId equals tpm.PositionId
                                           where epm.EmployeeId == user && tpm.TrainingId == trainingId
                                           select epm).FirstOrDefaultAsync();

            if (checkUserEligible == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> IsEligibleBySetupModule(string user, int setupModuleId)
        {
            var trainingId = await (from sm in DB.SetupModules
                                    join tr in DB.Trainings on sm.CourseId equals tr.CourseId
                                    where sm.SetupModuleId == setupModuleId && tr.IsDeleted == false
                                    select tr.TrainingId).FirstOrDefaultAsync();

            var checkUserEligible = await (from epm in DB.EmployeePositionMappings
                                           join tpm in DB.TrainingPositionMappings on epm.PositionId equals tpm.PositionId
                                           where epm.EmployeeId == user && tpm.TrainingId == trainingId
                                           select epm).FirstOrDefaultAsync();

            if (checkUserEligible == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> EnrollTraining(string user, EnrollQueueModel model)
        {

            //var trainingData = await DB.Trainings.FirstOrDefaultAsync(x => x.TrainingId == model.TrainingId);
            //if (DateTime.Now.Date > trainingData.EndDate.Value.Date || DateTime.Now.Date < trainingData.StartDate.Value.Date)
            //{
            //    return false;
            //}
            var isEnroll = await this.EnrollCheck(user, model.TrainingId, null);

            if (isEnroll == true)
            {
                return false;
            }

            // var isQuotaMax = await this.IsTrainingFull(model.TrainingId);

            // if (isQuotaMax == true)
            // {
            //     return false;
            // }

            var isPassedPrerequisite = await this.PassedPrequisite(user, model.TrainingId);

            if (isPassedPrerequisite == false)
            {
                return false;
            }

            var query = await (from m in DB.Modules
                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join c in DB.Courses on sm.CourseId equals c.CourseId into smc
                               from c in smc.DefaultIfEmpty()
                               join t in DB.Trainings on c.CourseId equals t.CourseId into ct
                               from t in ct.DefaultIfEmpty()
                               where t.TrainingId == model.TrainingId && sm.IsDeleted == false && sm.ModuleId != null
                               select new
                               {
                                   setupmoduleId = sm.SetupModuleId
                               }).AsNoTracking().ToListAsync();


            var assesmentQuery = await (from a in DB.Assesments
                                        join sm in DB.SetupModules on a.GUID equals sm.AssesmentId
                                        join c in DB.Courses on sm.CourseId equals c.CourseId into smc
                                        from c in smc.DefaultIfEmpty()
                                        join t in DB.Trainings on c.CourseId equals t.CourseId into ct
                                        from t in ct.DefaultIfEmpty()
                                        where t.TrainingId == model.TrainingId && sm.IsDeleted == false && sm.AssesmentId != null
                                        select new
                                        {
                                            setupmoduleId = sm.SetupModuleId
                                        }).AsNoTracking().ToListAsync();

            query.AddRange(assesmentQuery);

            var enroll = await DB.EnrollLearnings.Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();

            var courseData = await (from t in this.DB.Trainings
                                    join c in this.DB.Courses on t.CourseId equals c.CourseId
                                    where t.TrainingId == model.TrainingId
                                    select new
                                    {
                                        c.LearningTypeId,
                                        c.CoursePrice
                                    }).FirstOrDefaultAsync();

            var isOfflineTraining = courseData.LearningTypeId == LearningType.OfflineId || courseData.LearningTypeId == LearningType.BothId;
            var isPaidCourse = courseData.CoursePrice == null || courseData.CoursePrice > 0;

            var needApprovalTraining = isOfflineTraining || isPaidCourse;

            if (enroll == null)
            {
                enroll = new EnrollLearnings
                {
                    EmployeeId = user,
                    TrainingId = model.TrainingId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    IsEnrolled = true,
                    IsQueued = false,
                    IsJoined = needApprovalTraining ? false : true
                };
                DB.EnrollLearnings.Add(enroll);

            }
            else
            {
                enroll.IsEnrolled = true;
                enroll.IsQueued = false;
                enroll.IsJoined = needApprovalTraining ? false : true;
            }

            List<EnrollLearningTimes> newData = new List<EnrollLearningTimes>();

            foreach (var setupModule in query)
            {
                newData.Add(new EnrollLearningTimes
                {
                    EnrollLearningId = enroll.EnrollLearningId,
                    SetupModuleId = setupModule.setupmoduleId,
                    StartTime = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    EndTime = null,

                });
            }

            DB.EnrollLearningTimes.AddRange(newData);

            await DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelEnrollTraining(string user, EnrollQueueModel model)
        {
            var isEnroll = await this.EnrollCheck(user, model.TrainingId, null);

            if (isEnroll == false)
            {
                return false;
            }

            var enroll = await DB.EnrollLearnings.Include(Q => Q.EnrollLearningTimes).Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();

            if (enroll != null)
            {
                DB.EnrollLearnings.Remove(enroll);
            }

            await DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// untuk enroll user ke module yang dipilih
        /// </summary>
        /// <param name="user"></param>
        /// <param name="setupModuleId"></param>
        /// <returns></returns>
        public async Task<bool> EnrollModule(string user, int setupModuleId)
        {
            var isEnroll = await this.EnrollCheck(user, null, setupModuleId);



            if (isEnroll == true)
            {
                return false;
            }

            var enroll = await DB.EnrollLearnings.Where(Q => Q.EmployeeId.ToLower() == user.ToLower()
            && Q.TrainingId == null && Q.SetupModuleId == setupModuleId).FirstOrDefaultAsync();

            if (enroll == null)
            {
                enroll = new EnrollLearnings
                {
                    EmployeeId = user,
                    TrainingId = null,
                    SetupModuleId = setupModuleId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    IsEnrolled = true,
                    IsQueued = false,
                    IsJoined = true
                };
                DB.EnrollLearnings.Add(enroll);

            }
            else
            {
                enroll.IsEnrolled = true;
                enroll.IsQueued = false;
                enroll.IsJoined = true;
            }

            var newData = new EnrollLearningTimes
            {
                EnrollLearningId = enroll.EnrollLearningId,
                SetupModuleId = setupModuleId,
                StartTime = DateTime.UtcNow.ToIndonesiaTimeZone(),
                EndTime = null
            };

            DB.EnrollLearningTimes.Add(newData);

            await DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelEnrollModule(string user, int setupModuleId)
        {
            var isEnroll = await this.EnrollCheck(user, null, setupModuleId);

            if (isEnroll == false)
            {
                return false;
            }

            var enroll = await DB.EnrollLearnings.Include(Q => Q.EnrollLearningTimes).Where(Q => Q.EmployeeId.ToLower() == user.ToLower()
              && Q.TrainingId == null && Q.SetupModuleId == setupModuleId).FirstOrDefaultAsync();

            if (enroll != null)
            {
                DB.EnrollLearnings.Remove(enroll);
            }

            await DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EnrollCheck(string user, int? trainingId, int? setupModuleId)
        {
            //if (trainingId != null)
            //{
            //    var trainingData = await DB.Trainings.OrderByDescending(x => x.TrainingId).FirstOrDefaultAsync(x => x.TrainingId == trainingId);
            //    if (DateTime.Now.Date > trainingData.EndDate.Value.Date || DateTime.Now.Date < trainingData.StartDate.Value.Date)
            //    {
            //        return false;
            //    }
            //}

            var queryIsEnrolled = (from el in DB.EnrollLearnings
                                   join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                                   where el.EmployeeId == user && el.TrainingId == trainingId && el.IsEnrolled == true
                                   select new
                                   {
                                       el.IsEnrolled,
                                       el.TrainingId,
                                       el.SetupModuleId
                                   }).AsNoTracking();

            if (trainingId != null)
            {
                queryIsEnrolled = queryIsEnrolled.Where(Q => Q.TrainingId == trainingId);
            }
            else if (setupModuleId != null)
            {
                queryIsEnrolled = queryIsEnrolled.Where(Q => Q.SetupModuleId == setupModuleId);
            }

            var alreadyEnrolled = await queryIsEnrolled.FirstOrDefaultAsync();

            if (alreadyEnrolled == null)
            {
                return false;
            }



            return true;
        }

        /// <summary>
        /// untuk cek apakah user sudah mengantri module atau training tersebut
        /// </summary>
        /// <param name="AssignEmployeeIds"></param>
        /// <param name="trainingId"></param>
        /// <param name="setupModuleId"></param>
        /// <returns></returns>
        public async Task<List<string>> QueueCheckUsers(List<string> AssignEmployeeIds, int? trainingId, int? setupModuleId)
        {
            var ifTrainingAlreadyQueue = await (from el in DB.EnrollLearnings
                                                join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                                                where AssignEmployeeIds.Contains(el.EmployeeId) && el.IsQueued == true && el.TrainingId == trainingId
                                                select el.EmployeeId).AsNoTracking().ToListAsync();

            var ifModuleAlreadyQueue = await (from el in DB.EnrollLearnings
                                              join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                                              where AssignEmployeeIds.Contains(el.EmployeeId) && el.IsQueued == true && el.SetupModuleId == setupModuleId
                                              select el.EmployeeId).AsNoTracking().ToListAsync();

            if (trainingId != null && setupModuleId == null)
            {
                var dataTraining = AssignEmployeeIds.Except(ifTrainingAlreadyQueue).ToList();

                return dataTraining;
            }

            var dataModule = AssignEmployeeIds.Except(ifModuleAlreadyQueue).ToList();

            return dataModule;
        }

        public async Task AddToQeueu(string user, EnrollQueueModel model)
        {
            var query = await (from m in DB.Modules
                               join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                               join c in DB.Courses on sm.CourseId equals c.CourseId into smc
                               from c in smc.DefaultIfEmpty()
                               join t in DB.Trainings on c.CourseId equals t.CourseId into ct
                               from t in ct.DefaultIfEmpty()
                               where t.TrainingId == model.TrainingId
                               select new
                               {
                                   setupmoduleId = sm.SetupModuleId
                               }).AsNoTracking().FirstOrDefaultAsync();

            var enroll = await DB.EnrollLearnings.Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();
            if (enroll == null)
            {
                enroll = new EnrollLearnings
                {
                    EmployeeId = user,
                    TrainingId = model.TrainingId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    IsEnrolled = false,
                    IsQueued = true
                };
                DB.EnrollLearnings.Add(enroll);
            }
            else
            {
                if (enroll.IsEnrolled == true)
                {
                    return;
                }

                enroll.IsEnrolled = false;
                enroll.IsQueued = true;
            }

            await DB.SaveChangesAsync();
        }

        public async Task UnqueueTraining(string user, EnrollQueueModel model)
        {
            var data = await this
                .DB
                .EnrollLearnings
                .Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && Q.TrainingId == model.TrainingId)
                .Select(Q => Q)
                .FirstOrDefaultAsync();

            data.IsQueued = false;
            DB.EnrollLearnings.Update(data);
            await DB.SaveChangesAsync();
        }

        public async Task UpdateQueueToEnroll(string user, EnrollQueueModel model)
        {
            var ifAlreadyQueue = await (from el in DB.EnrollLearnings
                                        join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                                        where el.EmployeeId.ToLower() == user.ToLower() && el.IsQueued == true
                                        select new
                                        {
                                            el
                                        }).FirstOrDefaultAsync();

            if (ifAlreadyQueue != null)
            {
                ifAlreadyQueue.el.IsQueued = false;
                ifAlreadyQueue.el.IsEnrolled = true;
                this.DB.EnrollLearnings.Update(ifAlreadyQueue.el);
            }
            else
            {
                await AddToQeueu(user, model);
            }

            await this.DB.SaveChangesAsync();
        }

        private async Task<EnrollLearningTimes> GetEnrollLearningTimes(string employeeId, CheckModuleStartModel model)
        {
            var query = DB.EnrollLearnings
            .Where(Q => Q.EmployeeId == employeeId)
            .AsQueryable();

            if (model.TrainingId == null)
            {
                query = query.Where(Q => Q.SetupModuleId == model.SetupModuleId);
            }

            else
            {
                query = query.Where(Q => Q.TrainingId == model.TrainingId);
            }

            var getDataEnrollLearning = await query.FirstOrDefaultAsync();

            if (getDataEnrollLearning == null)
            {
                return null;
            }

            var getEnrollLearningId = getDataEnrollLearning.EnrollLearningId;

            var getDataEnrollLearningTime = await DB.EnrollLearningTimes
                .Where(Q => Q.EnrollLearningId == getEnrollLearningId && Q.SetupModuleId == model.SetupModuleId)
                .FirstOrDefaultAsync();

            return getDataEnrollLearningTime;
        }

        public async Task<string> StartModule(string employeeId, CheckModuleStartModel model)
        {
            var data = await GetEnrollLearningTimes(employeeId, model);

            if (data == null || data.StartTime != null)
            {
                return data.StartTime?.ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
            }

            data.StartTime = DateTime.UtcNow.ToIndonesiaTimeZone();

            await DB.SaveChangesAsync();

            return data.StartTime?.ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
        }

        public async Task<ScorePointsModel> GetScorePoints(int taskTypeId, int taskId)
        {
            var model = new ScorePointsModel();
            //1   True / False
            if (taskTypeId == 1)
            {
                var timePointId = await DB.TaskTrueFalseTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //2   Matching
            if (taskTypeId == 2)
            {
                var timePointId = await DB.TaskMatchingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //3   Sequence
            if (taskTypeId == 3)
            {
                var timePointIds = await DB.TaskSequenceChoices.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //4   Tebak Gambar
            if (taskTypeId == 4)
            {
                var timePointId = await DB.TaskTebakGambarTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //5   Hot Spot
            if (taskTypeId == 5)
            {
                var timePointIds = await DB.TaskHotSpotAnswers.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //6   Short Answer
            //7   Open Question/ Essay
            //11  File Upload
            if (taskTypeId == 6 || taskTypeId == 7 || taskTypeId == 11)
            {
                model.Score = 100;
                model.Points = 0;
            }
            //8   Checklist
            if (taskTypeId == 8)
            {
                var timePointIds = await DB.TaskChecklistChoices.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //9   Rating
            if (taskTypeId == 9)
            {
                var timePointIds = new List<int>
                    {
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score0To20).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score21To40).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score41To60).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score61To80).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score81To100).FirstOrDefaultAsync()
                    };
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Max() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.Score == model.Score).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //10  Multiple Choice
            if (taskTypeId == 10)
            {
                var timePointId = await DB.TaskMultipleChoiceTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //12  Matrix
            if (taskTypeId == 12)
            {
                var timePointIds = new List<int>
                    {
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn1).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn2).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn3).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn4).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn5).FirstOrDefaultAsync()
                    };
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Max() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.Score == model.Score).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            return model;
        }

        //new function
        /// <summary>
        /// Get is pass by training id.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public async Task<UserSideIsLearningPassModel> GetUserIsPassByTrainingIdAsync(string user, int trainingId)
        {
            var enrollQuery = await DB.EnrollLearnings
                .Where(Q => Q.EmployeeId == user && Q.TrainingId == trainingId && Q.SetupModuleId == null)
                .FirstOrDefaultAsync();
            if (enrollQuery == null) return null;
            var isPass = enrollQuery.IsPassed.HasValue ? enrollQuery.IsPassed.Value : false;
            string certificateImageUrl = null;
            if (isPass)
            {
                var courseId = await DB.Trainings
                                .Where(Q => Q.TrainingId == trainingId)
                                .Select(Q => Q.CourseId)
                                .FirstOrDefaultAsync();
                var certificateId = await DB.EmployeeCertificates
                                    .Where(Q => Q.CourseId == courseId && Q.EmployeeId == user)
                                    .Select(Q => Q.EmployeeCertificateId)
                                    .FirstOrDefaultAsync();
                certificateImageUrl = await this.CertificateMan.GetSingleCertificate(user, certificateId);
            }

            return new UserSideIsLearningPassModel
            {
                CertificateImageUrl = certificateImageUrl,
                IsPass = isPass,
            };
        }

        /// <summary>
        /// Get is pass by setup modul id.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public async Task<UserSideIsLearningPassModel> GetUserIsPassBySetupModulIdAsync(string user, int setupModulId)
        {
            var enrollQuery = await DB.EnrollLearnings
                .Where(Q => Q.EmployeeId == user && Q.SetupModuleId == setupModulId && Q.TrainingId == null)
                .FirstOrDefaultAsync();
            if (enrollQuery == null) return null;
            var isPass = enrollQuery.IsPassed.HasValue ? enrollQuery.IsPassed.Value : false;
            return new UserSideIsLearningPassModel
            {
                CertificateImageUrl = null,
                IsPass = isPass,
            };
        }

        public async Task<List<ModuleScoreModel>> GetListScoreModuleByTrainingIdAsync(string user, int trainingId)
        {

            var taskAnswerList = await DB
                        .GetModuleScoresWithParam(user, trainingId)
                        .AsNoTracking()
                        .Select(Q => new ModuleScoreModel
                        {
                            ModuleName = Q.ModuleName,
                            Score = Q.Score,
                            SetupModulId = Q.SetupModuleId
                        })
                        .ToListAsync();

            if (taskAnswerList.Count <= 0)
            {
                return null;
            }

            return taskAnswerList;

        }

        public async Task<bool> IsTrainingFull(int trainingId)
        {
            var totalEnroll = await this.DB.EnrollLearnings.Where(Q => Q.TrainingId == trainingId && Q.IsEnrolled == true).CountAsync();
            var quota = await this.DB.Trainings.Where(Q => Q.TrainingId == trainingId).Select(Q => Q.Quota).FirstOrDefaultAsync();

            if (quota == null)
            {
                return false;
            }

            if (totalEnroll >= quota)
            {
                return true;
            }
            return false;
        }

        public async Task<BaseResponse> GetCountTraining(string user)
        {

            //var getAllDataCompleted = DB.GetCompletedLearningModel(user).AsQueryable();
            //var totalDataCompleted = await getAllDataCompleted.CountAsync();

            //var getAllDataContinued = DB.GetContinuedLearningModel(user).AsQueryable();
            //var totalDataContinued = await getAllDataContinued.CountAsync();

            var getAllDataCompleted = DB.GetCountTrainingCompleted(user).AsQueryable();
            var totalDataCompleted = await getAllDataCompleted.CountAsync();

            var getAllDataContinued = DB.GetCountTrainingContinued(user).AsQueryable();
            var totalDataContinued = await getAllDataContinued.CountAsync();

            return BaseResponse.ResponseOk(new
            {
                totalDataCompleted = totalDataCompleted,
                totalDataContinued = totalDataContinued
            });
        }

        public async Task<bool> GetStatusDownload(string relatedId, string employeeId)
        {
            try
            {

                var query = await DB.EmployeeDownload.Where(Q => Q.Category.ToLower() != "news").Where(Q => Q.EmployeeId == employeeId).ToListAsync();

                foreach (var q in query)
                {
                    if (q.Category.ToLower() != "news")
                    {
                        var jsonDescDeserialize = JsonConvert.DeserializeObject<Root>(q.Description);
                        foreach (var innerq in jsonDescDeserialize.downloadModel)
                        {
                            if (innerq.id == relatedId)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception x)
            {
                return false;
            }
        }

    }
}

