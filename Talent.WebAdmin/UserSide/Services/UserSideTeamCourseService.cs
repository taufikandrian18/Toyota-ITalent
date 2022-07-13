using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTeamCourseService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideTeamCourseService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        /// <summary>
        /// Get data course with pagination and search;
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<UserSideMyProfileCourseResponseModel> GetCoursesPaginationAsync(UserSideMyProfileCourseFilterModel filter, string employeeId)
        {
            var userInformation = await (from e in this.DB.Employees
                                         join epm in this.DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                         where e.EmployeeId.ToLower() == employeeId.ToLower()
                                         select new
                                         {
                                             e.OutletId,
                                             epm.PositionId
                                         }).ToListAsync();
            var outletId = userInformation.Select(Q => Q.OutletId).FirstOrDefault();
            var positionIds = userInformation.Select(Q => Q.PositionId).ToList();

            var initCourses = from tr in this.DB.Trainings.AsNoTracking()

                              join tom in this.DB.TrainingOutletMappings
                              on tr.TrainingId equals tom.TrainingId

                              join tpm in this.DB.TrainingPositionMappings
                              on tr.TrainingId equals tpm.TrainingId

                              join c in this.DB.Courses.AsNoTracking()
                              on tr.CourseId equals c.CourseId

                              join cr in this.DB.TrainingRatings.AsNoTracking()
                              on c.CourseId equals cr.CourseId into crc
                              from cr1 in crc.DefaultIfEmpty()

                              join b in this.DB.Blobs.AsNoTracking()
                              on c.BlobId equals b.BlobId into bl
                              from blb in bl.DefaultIfEmpty()

                              where
                              c.IsDeleted == false &&
                              cr1.IsDeleted == false &&
                              tr.IsDeleted == false

                              select new
                              {
                                  tr.TrainingId,
                                  c.CourseId,
                                  RatingScore = ((int?)cr1.RatingScore).HasValue ? cr1.RatingScore : 0,
                                  c.CourseName,
                                  BlobId = c.BlobId == null ? Guid.Empty : c.BlobId,
                                  Mime = blb.Mime == null ? null : blb.Mime,
                                  tom.OutletId,
                                  tpm.PositionId
                              };

            if (!string.IsNullOrEmpty(outletId))
            {
                initCourses = initCourses.Where(Q => Q.OutletId == outletId);
            }

            if (positionIds.Count > 0)
            {
                initCourses = initCourses.Where(Q => positionIds[0] == Q.PositionId);
            }

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                initCourses = initCourses.Where(Q => Q.CourseName.ToLower().Contains(filter.Keyword.ToLower()) == true);
            }

            var count = initCourses.Count();
            var dataCourses = initCourses.ToList();

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var assignedTraining = await this.DB.AssignedLearnings.Where(Q => Q.AssignedTo.ToLower() == employeeId.ToLower()).Select(Q => Q.TrainingId).ToListAsync();
            var enrolledTraining = await this.DB.EnrollLearnings.Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower() && (Q.IsEnrolled == true || Q.IsQueued == true)).Select(Q => Q.TrainingId).ToListAsync();

            var courses = dataCourses.GroupBy(Q => Q.TrainingId)
                .Select(async X => new UserSideMyProfileCourseModel
                {
                    TrainingId = X.First().TrainingId,
                    CourseId = X.First().CourseId,
                    CourseName = X.First().CourseName,
                    ImageUrl = X.First().BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.First().BlobId.ToString(), X.First().Mime),
                    Rating = Math.Ceiling((Convert.ToDouble(dataCourses.Where(Q => Q.TrainingId == X.First().TrainingId).Select(Q => Q.RatingScore).Sum() / dataCourses.Where(Q => Q.TrainingId == X.First().TrainingId).Count())) / 2),
                    IsAssign = assignedTraining.Where(Q => Q == X.First().TrainingId).Any() || enrolledTraining.Where(Q => Q == X.First().TrainingId).Any()
                })
                .Select(Q => Q.Result)
                .ToList();

            var totalData = courses.Count();

            if (totalData <= filter.PageSize)
            {
                filter.PageIndex = 1;
            };

            courses = courses
                .Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToList();

            var responseCourses = new UserSideMyProfileCourseResponseModel
            {
                Courses = courses,
                TotalData = totalData
            };

            return responseCourses;
        }

        public async Task AsignCourse(UserSideMyProfileAsignLearningModel form, string assignedBy)
        {
            var isAsigned = await this.EmployeeIsAsigneTraining(form.AssignedTo, form.TrainingId);

            var isTeamLeader = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == assignedBy.ToLower())
                .Select(Q => Q.IsTeamLeader)
                .FirstOrDefaultAsync();

            var querySetupModule = await (from m in DB.Modules.AsNoTracking()
                                          join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                                          join c in DB.Courses on sm.CourseId equals c.CourseId into smc
                                          from c in smc.DefaultIfEmpty()
                                          join t in DB.Trainings on c.CourseId equals t.CourseId into ct
                                          from t in ct.DefaultIfEmpty()
                                          where t.TrainingId == form.TrainingId
                                          select new
                                          {
                                              setupmoduleId = sm.SetupModuleId
                                          }).AsNoTracking().FirstOrDefaultAsync();

            if (isTeamLeader == true && isAsigned == false)
            {
                var newData = new AssignedLearnings();

                if (querySetupModule != null)
                {
                    newData = new AssignedLearnings
                    {
                        TrainingId = form.TrainingId,
                        SetupModuleId = querySetupModule.setupmoduleId,
                        AssignedTo = form.AssignedTo,
                        AssignedBy = assignedBy,
                        AssignedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    };
                }
                else
                {
                    newData = new AssignedLearnings
                    {
                        TrainingId = form.TrainingId,
                        AssignedTo = form.AssignedTo,
                        AssignedBy = assignedBy,
                        AssignedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    };
                }
                // Insert new AssignedLearnings.
                this.DB.AssignedLearnings.Add(newData);

                var enroll = new EnrollLearnings
                {
                    EmployeeId = form.AssignedTo,
                    TrainingId = form.TrainingId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    IsEnrolled = false,
                    IsQueued = true
                };

                this.DB.EnrollLearnings.Add(enroll);

                var enrollMapping = new EnrollLearningTimes();

                if (querySetupModule != null)
                {
                    enrollMapping = new EnrollLearningTimes
                    {
                        EnrollLearningId = enroll.EnrollLearningId,
                        SetupModuleId = querySetupModule.setupmoduleId,
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

                this.DB.EnrollLearningTimes.Add(enrollMapping);

                var courseName = await this.DB.Trainings.Where(Q => Q.TrainingId == form.TrainingId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();
                var assignName = await this.DB.Employees.Where(Q => Q.EmployeeId.ToLower() == assignedBy.ToLower()).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();

                var messageContent = "Anda mendapatkan new assignment " + courseName + " dari " + assignName + ". Silahkan akses Learning Queue di My Learning";

                var newMobileInbox = new MobileInboxes
                {
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = assignedBy,
                    EmployeeId  = form.AssignedTo,
                    IsRead = false,
                    Message = messageContent,
                    MobileInboxTypeId = (int)MobileInboxType.AssignLearning,
                    Title = "Assigned New Learning"
                };

                this.DB.MobileInboxes.Add(newMobileInbox);

                // Save changes to DB.
                await this.DB.SaveChangesAsync();
            }

        }

        public async Task AsignCourseUpdate(UserSideMyProfileAsignLearningModel form, int assignedLearningId, string assignedBy)
        {
            var isAsigned = await this.EmployeeIsAsigneTraining(form.AssignedTo, form.TrainingId);

            var querySetupModule = await (from m in DB.Modules.AsNoTracking()
                                          join sm in DB.SetupModules on m.ModuleId equals sm.ModuleId
                                          join c in DB.Courses on sm.CourseId equals c.CourseId into smc
                                          from c in smc.DefaultIfEmpty()
                                          join t in DB.Trainings on c.CourseId equals t.CourseId into ct
                                          from t in ct.DefaultIfEmpty()
                                          where t.TrainingId == form.TrainingId
                                          select new
                                          {
                                              setupmoduleId = sm.SetupModuleId
                                          }).AsNoTracking().FirstOrDefaultAsync();

            var isTeamLeader = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == assignedBy.ToLower())
                .Select(Q => Q.IsTeamLeader)
                .FirstOrDefaultAsync();

            if (isTeamLeader == true && isAsigned == false)
            {
                var oldAsign = await this
                .DB
                .AssignedLearnings
                .AsNoTracking()
                .Where(Q => Q.AssignedLearningId == assignedLearningId)
                .FirstOrDefaultAsync();

                // Update AssignedLearnings.
                oldAsign.TrainingId = form.TrainingId;
                oldAsign.AssignedTo = form.AssignedTo;
                oldAsign.AssignedBy = assignedBy;
                oldAsign.AssignedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                if (querySetupModule != null)
                {
                    oldAsign.SetupModuleId = querySetupModule.setupmoduleId;
                }

                this.DB.Update(oldAsign);

                // Save changes to DB.
                await this.DB.SaveChangesAsync();
            }

        }

        public async Task<bool> EmployeeIsAsigneTraining(string employeeId, int trainingId)
        {
            var isAsigned = await (from el in DB.EnrollLearnings
                                   join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId

                                   where el.EmployeeId == employeeId && el.IsQueued == true && el.TrainingId == trainingId
                                   select el.EmployeeId).AsNoTracking().AnyAsync();

            return isAsigned;
        }
    }

}