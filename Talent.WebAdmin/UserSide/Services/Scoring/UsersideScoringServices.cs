using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.UserSide.Models.LiveAssesment;
using Talent.WebAdmin.UserSide.Models;
using System.IO;
using ClosedXML.Excel;
using Talent.WebAdmin.Enums;
using Talent.Entities.DbQueryModels;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideScoringService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public UserSideScoringService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }

        public async Task<ActionResult<BaseResponse>> GetCourseCategorySummaryMyTeam(ScoringSummaryRequest request)
        {
            try
            {
                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;


                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }

                double totalCertificate = 0;
                double totalEnrolled = 0;


                //var employees = await this.DB.Employees.Where(x=> x.IsDeleted == false).Where(x=> employeeIds.Contains(x.EmployeeId)).OrderBy(x=> x.EmployeeName).Select(x=> new {
                //    x.EmployeeName, 
                //    x.EmployeeId
                //}).ToListAsync();

                foreach (var item in employeeIds)
                {
                    var enrollLearning = this.DB.EnrollLearnings.Where(x => x.EmployeeId == item).Where(x => x.TrainingId == request.TrainingId).FirstOrDefault();

                    if (enrollLearning != null)
                    {
                        totalEnrolled++;
                    }

                    var certificateEmployee = this.DB.EmployeeCertificates.Where(x => x.EmployeeId == item).Where(x => x.TrainingId == request.TrainingId).FirstOrDefault();

                    if (certificateEmployee != null)
                    {
                        totalCertificate++;
                    }
                }

                //foreach (var item in employeeIds)
                //{
                //    var certificateEmployee = this.DB.EmployeeCertificates.Where(x => x.EmployeeId == item).Where(x => x.TrainingId == request.TrainingId).FirstOrDefault();

                //    if (certificateEmployee != null)
                //    {
                //        totalCertificate++;
                //    }
                //}

                double enrollmentPercentage = (totalEnrolled / (double)employeeIds.Count()) * 100;
                double certificationPercentage = (totalCertificate / (double)employeeIds.Count()) * 100;

                var data = new ScoringSummaryResponse
                {
                    CertificationPercentage = Math.Round(certificationPercentage, 2),
                    EnrollmentPercentage = Math.Round(enrollmentPercentage, 2),
                    YEnroll = employeeIds.Count(),
                    YCertification = employeeIds.Count(),
                    XEnroll = totalEnrolled,
                    XCertification = totalCertificate,
                };

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetAttempts(RequestLiveAssesmentSkillCheckModel request)
        {
            try
            {
                int att = 0;
                var attempts = await DB.LiveAssesmentSkillChecks.Where(y => y.EmployeeGUID == request.EmployeeGUID).Where(y => y.SkillCheckGUID == request.SkillcheckGUID).Where(y => y.AssesmentGUID == request.AssesmentGUID).Where(y => y.TrainingId == request.TrainingID).OrderByDescending(x => x.Attempts).FirstOrDefaultAsync();

                if (attempts != null)
                {
                    att = (int)attempts.Attempts;
                }
                return StatusCode(200, BaseResponse.ResponseOk(new
                {
                    Attempts = att + 1
                }));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetAttemptTask(TaskAnswerGetAttemptModel taskAnswers)
        {
            try
            {
                var attempts = await DB.TaskAnswerDetails.Include(y => y.TaskAnswer).Where(y => y.TaskAnswer.SetupModuleId == taskAnswers.SetupModuleId && y.TaskAnswer.TrainingId == taskAnswers.TrainingId && y.TaskAnswer.EmployeeId == taskAnswers.EmployeeId).Where(y => y.TaskId == taskAnswers.Answer.FirstOrDefault().TaskId).OrderByDescending(y => y.Attempts).Select(y => y.Attempts).ToListAsync();

                var returnAtm = new AttemptTask();
                if (attempts.Count() > 0)
                {
                    returnAtm.attempts = (int)attempts.Max() + 1;
                }
                else
                {
                    returnAtm.attempts = 1;
                }
                return StatusCode(200, BaseResponse.ResponseOk(returnAtm));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        // ini original GetScoringCourseSummaryMyTeam

        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyTeam(ScoringSummaryRequest request)
        {
            try
            {
                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;

                double totalPreEnrolled = 0;
                double totalDuringEnrolled = 0;
                double totalPostEnrolled = 0;

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }


                //var employees = await this.DB.Employees.Where(x=> x.IsDeleted == false).Where(x=> employeeIds.Contains(x.EmployeeId)).OrderBy(x=> x.EmployeeName).Select(x=> new {
                //    x.EmployeeId, 
                //    x.EmployeeName
                //}).ToListAsync();

                var preNA = false;
                var duringNA = false;
                var postNA = false;

                var dataSetupLearning = (from t in DB.Trainings
                                         join c in DB.Courses on t.CourseId equals c.CourseId
                                         join sm in DB.SetupModules on c.CourseId equals sm.CourseId
                                         where t.TrainingId == request.TrainingId && t.IsDeleted == false && sm.IsDeleted == false
                                         select new
                                         { sm.AssesmentId, sm.ModuleId, sm.SetupModuleId, sm.TrainingTypeId, sm.Order }).ToList();

                var preData = dataSetupLearning.Where(x => x.TrainingTypeId == 1).Select(x => x.SetupModuleId).ToList();
                var duringData = dataSetupLearning.Where(x => x.TrainingTypeId == 2).Select(x => x.SetupModuleId).ToList();
                var postData = dataSetupLearning.Where(x => x.TrainingTypeId == 3).Select(x => x.SetupModuleId).ToList();

                foreach (var item in employeeIds)
                {
                    var preDataList = new List<ScoringCourseSummaryAttemptResponse>();
                    var duringDataList = new List<ScoringCourseSummaryAttemptResponse>();
                    var postDataList = new List<ScoringCourseSummaryAttemptResponse>();

                    //var memberData = new ScoringCourseSummaryMemberResponse
                    //{
                    //    //EmployeeName =item.EmployeeName,
                    //    EmployeeName = await this.DB.Employees.Where(q => q.EmployeeId == item).Select(q => q.EmployeeName).FirstOrDefaultAsync(),
                    //};



                    //var xqData = this.DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Where(x => x.EnrollLearning.EmployeeId == item).Where(x => x.EnrollLearning.TrainingId == request.TrainingId).Include(x => x.SetupModule);

                    //var preData = await qData.Where(x => x.SetupModule.TrainingTypeId == 1).Select(x => x.SetupModuleId).Distinct().ToListAsync();
                    //var duringData = await qData.Where(x => x.SetupModule.TrainingTypeId == 2).Select(x => x.SetupModuleId).Distinct().ToListAsync();
                    //var postData = await qData.Where(x => x.SetupModule.TrainingTypeId == 3).Select(x => x.SetupModuleId).Distinct().ToListAsync();




                    foreach (var datumPredata in preData)
                    {
                        var assesments = await this.DB.SetupModules.Include(x => x.Assesment).Where(x => x.SetupModuleId == datumPredata).Where(x => x.AssesmentId != null).ToListAsync();
                        foreach (var assesment in assesments)
                        {
                            var skillChecks = await this.DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).Where(x => x.AssesmentGUID == assesment.AssesmentId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = skillChecks.AssesmentsNavigator.Name,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = GetTrackingProgressStatus(item, assesment.AssesmentId, datumPredata, request.TrainingId).Status,
                                //Score = GetTrackingProgressStatus(item, assesment.AssesmentId, datumPredata, request.TrainingId).Score,
                            };
                            preDataList.Add(sub);
                        }
                        var modules = await this.DB.SetupModules.Include(x => x.Module).Where(x => x.SetupModuleId == datumPredata).Where(x => x.ModuleId != null)
                            .Select(x => new
                            {
                                x.ModuleId,
                                x.Module.ModuleName
                            }).ToListAsync();
                        foreach (var module in modules)
                        {
                            //var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = module.ModuleName,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = GetTrackingProgressStatusModule(item, module.ModuleId, datumPredata, request.TrainingId).Status,
                                //Score = GetTrackingProgressStatusModule(item, modulenya.ModuleId, datumPredata, request.TrainingId).Score

                            };
                            preDataList.Add(sub);
                        }
                    }

                    foreach (var datumDuringData in duringData)
                    {
                        var assesments = await this.DB.SetupModules.Include(x => x.Assesment).Where(x => x.SetupModuleId == datumDuringData).Where(x => x.AssesmentId != null).ToListAsync();
                        foreach (var assesment in assesments)
                        {
                            var skillChecks = await this.DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).Where(x => x.AssesmentGUID == assesment.AssesmentId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = skillChecks.AssesmentsNavigator.Name,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = GetTrackingProgressStatus(item, assesment.AssesmentId, datumDuringData, request.TrainingId).Status,
                                //Score = GetTrackingProgressStatus(item, assesment.AssesmentId, datumDuringData, request.TrainingId).Score,

                            };
                            duringDataList.Add(sub);
                        }
                        var modules = await this.DB.SetupModules.Include(x => x.Module).Where(x => x.SetupModuleId == datumDuringData).Where(x => x.ModuleId != null)
                            .Select(x => new
                            {
                                x.ModuleId,
                                x.Module.ModuleName
                            }).ToListAsync();
                        foreach (var module in modules)
                        {
                            //var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = module.ModuleName,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = GetTrackingProgressStatusModule(item, module.ModuleId, datumDuringData, request.TrainingId).Status,
                                //Score = GetTrackingProgressStatusModule(item, modulenya.ModuleId, datumDuringData, request.TrainingId).Score

                            };
                            duringDataList.Add(sub);
                        }
                    }

                    foreach (var datumPostData in postData)
                    {
                        var assesments = await this.DB.SetupModules.Include(x => x.Assesment).Where(x => x.SetupModuleId == datumPostData).Where(x => x.AssesmentId != null).ToListAsync();
                        foreach (var assesment in assesments)
                        {
                            var skillChecks = await this.DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).Where(x => x.AssesmentGUID == assesment.AssesmentId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = skillChecks.AssesmentsNavigator.Name,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = GetTrackingProgressStatus(item, assesment.AssesmentId, datumPostData, request.TrainingId).Status,
                                //Score = GetTrackingProgressStatus(item, assesment.AssesmentId, datumPostData, request.TrainingId).Score,

                            };
                            postDataList.Add(sub);
                        }
                        var modules = await this.DB.SetupModules.Include(x => x.Module).Where(x => x.SetupModuleId == datumPostData).Where(x => x.ModuleId != null)
                            .Select(x => new
                            {
                                x.ModuleId,
                                x.Module.ModuleName
                            }).ToListAsync();
                        foreach (var module in modules)
                        {
                            //var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = module.ModuleName,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = GetTrackingProgressStatusModule(item, module.ModuleId, datumPostData, request.TrainingId).Status,
                                //Score = GetTrackingProgressStatusModule(item, modulenya.ModuleId, datumPostData, request.TrainingId).Score,

                            };
                            postDataList.Add(sub);
                        }
                    }

                    if (preDataList.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in preDataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == preDataList.Count)
                        {
                            totalPreEnrolled++;
                        }
                        //if (preDataList.Where(x => x.EnumStatus == "2" || x.EnumStatus == "3").Any())
                        //{
                        //    totalPreEnrolled++;
                        //};
                    }
                    else
                    {
                        preNA = true;
                    }

                    if (duringDataList.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in duringDataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == duringDataList.Count)
                        {
                            totalDuringEnrolled++;
                        }

                        //if (duringDataList.Where(x => x.EnumStatus == "2" || x.EnumStatus == "3").Any())
                        //{
                        //    totalDuringEnrolled++;
                        //};
                    }
                    else
                    {
                        duringNA = true;
                    }

                    if (postDataList.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in postDataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == postDataList.Count)
                        {
                            totalPostEnrolled++;
                        }
                        //if (postDataList.Where(x => x.EnumStatus == "2" || x.EnumStatus == "3").Any())
                        //{
                        //    totalPostEnrolled++;
                        //};
                    }
                    else
                    {
                        postNA = true;
                    }
                }

                double prePercentage = (totalPreEnrolled / (double)employeeIds.Count()) * 100;
                double duringPercentage = (totalDuringEnrolled / (double)employeeIds.Count()) * 100;
                double postPercentage = (totalPostEnrolled / (double)employeeIds.Count()) * 100;
                var courses = await DB.Trainings.Where(x => x.TrainingId == request.TrainingId).FirstOrDefaultAsync();
                //var setupModules = DB.SetupModules.Where(x=> x.CourseId == courses.CourseId);

                preNA = DB.SetupModules.Where(x => x.CourseId == courses.CourseId).Where(x => x.TrainingTypeId == 1).Any();
                duringNA = DB.SetupModules.Where(x => x.CourseId == courses.CourseId).Where(x => x.TrainingTypeId == 2).Any();
                postNA = DB.SetupModules.Where(x => x.CourseId == courses.CourseId).Where(x => x.TrainingTypeId == 3).Any();
                var data = new ScoringCourseSummaryResponse
                {
                    PreX = totalPreEnrolled,
                    DuringX = totalDuringEnrolled,
                    PostX = totalPostEnrolled,
                    PreY = employeeIds.Count(),
                    DuringY = employeeIds.Count(),
                    PostY = employeeIds.Count(),
                    PrePercentage = Math.Round(prePercentage, 2),
                    DuringPercentage = Math.Round(duringPercentage, 2),
                    PostPercentage = Math.Round(postPercentage, 2),
                    PreNA = !preNA,
                    DuringNA = !duringNA,
                    PostNA = !postNA,
                };




                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        // ini replicate GetScoringCourseSummaryMyTeam

        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyTeamReplicate(ScoringSummaryRequest request)
        {
            try
            {
                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;

                double totalPreEnrolled = 0;
                double totalDuringEnrolled = 0;
                double totalPostEnrolled = 0;

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }

                var dataGetMyCoachDetail = await this.DB.GetDataMyCoachDetailSummary(request.TrainingId).ToListAsync();

                var preNA = false;
                var duringNA = false;
                var postNA = false;

                var dataSetupLearning = (from t in DB.Trainings
                                         join c in DB.Courses on t.CourseId equals c.CourseId
                                         join sm in DB.SetupModules on c.CourseId equals sm.CourseId
                                         where t.TrainingId == request.TrainingId && t.IsDeleted == false && sm.IsDeleted == false
                                         select new
                                         { sm.AssesmentId, sm.ModuleId, sm.SetupModuleId, sm.TrainingTypeId, sm.Order }).ToList();

                var preData = dataSetupLearning.Where(x => x.TrainingTypeId == 1).Select(x => x.SetupModuleId).ToList();
                var duringData = dataSetupLearning.Where(x => x.TrainingTypeId == 2).Select(x => x.SetupModuleId).ToList();
                var postData = dataSetupLearning.Where(x => x.TrainingTypeId == 3).Select(x => x.SetupModuleId).ToList();

                foreach (var item in employeeIds)
                {
                    var preDataList = new List<ScoringCourseSummaryAttemptResponse>();
                    var duringDataList = new List<ScoringCourseSummaryAttemptResponse>();
                    var postDataList = new List<ScoringCourseSummaryAttemptResponse>();

                    foreach (var datumPredata in preData)
                    {
                        var assesments = dataGetMyCoachDetail.Where(x => x.setupmoduleid == datumPredata && x.employeeid == item).Where(x => x.assesmentid != null)
                            .Select(x => new
                            {
                                x.assesmentid,
                                x.Name,
                                x.returnValue
                            }).ToList();
                        foreach (var assesment in assesments)
                        {
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = assesment.Name,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = assesment.returnValue,
                                //Score = GetTrackingProgressStatus(item, assesment.AssesmentId, datumPredata, request.TrainingId).Score,
                            };
                            preDataList.Add(sub);
                        }
                        var modules = dataGetMyCoachDetail.Where(x => x.setupmoduleid == datumPredata && x.employeeid == item).Where(x => x.moduleid != null)
                            .Select(x => new
                            {
                                x.moduleid,
                                x.modulename,
                                x.returnValue
                            }).ToList();
                        foreach (var module in modules)
                        {
                            //var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = module.modulename,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = module.returnValue
                                //Score = GetTrackingProgressStatusModule(item, modulenya.ModuleId, datumPredata, request.TrainingId).Score

                            };
                            preDataList.Add(sub);
                        }
                    }

                    foreach (var datumDuringData in duringData)
                    {
                        var assesments = dataGetMyCoachDetail.Where(x => x.setupmoduleid == datumDuringData && x.employeeid == item).Where(x => x.assesmentid != null)
                            .Select(x => new
                            {
                                x.assesmentid,
                                x.Name,
                                x.returnValue
                            }).ToList();
                        foreach (var assesment in assesments)
                        {
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = assesment.Name,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = assesment.returnValue,
                                //Score = GetTrackingProgressStatus(item, assesment.AssesmentId, datumDuringData, request.TrainingId).Score,

                            };
                            duringDataList.Add(sub);
                        }
                        var modules = dataGetMyCoachDetail.Where(x => x.setupmoduleid == datumDuringData && x.employeeid == item).Where(x => x.moduleid != null)
                            .Select(x => new
                            {
                                x.moduleid,
                                x.modulename,
                                x.returnValue
                            }).ToList();
                        foreach (var module in modules)
                        {
                            //var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = module.modulename,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = module.returnValue,
                                //Score = GetTrackingProgressStatusModule(item, modulenya.ModuleId, datumDuringData, request.TrainingId).Score

                            };
                            duringDataList.Add(sub);
                        }
                    }

                    foreach (var datumPostData in postData)
                    {
                        var assesments = dataGetMyCoachDetail.Where(x => x.setupmoduleid == datumPostData && x.employeeid == item).Where(x => x.assesmentid != null)
                            .Select(x => new
                            {
                                x.assesmentid,
                                x.Name,
                                x.returnValue
                            }).ToList();
                        foreach (var assesment in assesments)
                        {
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = assesment.Name,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = assesment.returnValue,
                                //Score = GetTrackingProgressStatus(item, assesment.AssesmentId, datumPostData, request.TrainingId).Score,

                            };
                            postDataList.Add(sub);
                        }
                        var modules = dataGetMyCoachDetail.Where(x => x.setupmoduleid == datumPostData && x.employeeid == item).Where(x => x.moduleid != null)
                            .Select(x => new
                            {
                                x.moduleid,
                                x.modulename,
                                x.returnValue
                            }).ToList();
                        foreach (var module in modules)
                        {
                            //var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                            var sub = new ScoringCourseSummaryAttemptResponse
                            {
                                Name = module.modulename,
                                //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                EnumStatus = module.returnValue,
                                //Score = GetTrackingProgressStatusModule(item, modulenya.ModuleId, datumPostData, request.TrainingId).Score,

                            };
                            postDataList.Add(sub);
                        }
                    }

                    if (preDataList.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in preDataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == preDataList.Count)
                        {
                            totalPreEnrolled++;
                        }
                        //if (preDataList.Where(x => x.EnumStatus == "2" || x.EnumStatus == "3").Any())
                        //{
                        //    totalPreEnrolled++;
                        //};
                    }
                    else
                    {
                        preNA = true;
                    }

                    if (duringDataList.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in duringDataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == duringDataList.Count)
                        {
                            totalDuringEnrolled++;
                        }

                        //if (duringDataList.Where(x => x.EnumStatus == "2" || x.EnumStatus == "3").Any())
                        //{
                        //    totalDuringEnrolled++;
                        //};
                    }
                    else
                    {
                        duringNA = true;
                    }

                    if (postDataList.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in postDataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == postDataList.Count)
                        {
                            totalPostEnrolled++;
                        }
                        //if (postDataList.Where(x => x.EnumStatus == "2" || x.EnumStatus == "3").Any())
                        //{
                        //    totalPostEnrolled++;
                        //};
                    }
                    else
                    {
                        postNA = true;
                    }
                }

                double prePercentage = (totalPreEnrolled / (double)employeeIds.Count()) * 100;
                double duringPercentage = (totalDuringEnrolled / (double)employeeIds.Count()) * 100;
                double postPercentage = (totalPostEnrolled / (double)employeeIds.Count()) * 100;
                var courses = await DB.Trainings.Where(x => x.TrainingId == request.TrainingId).FirstOrDefaultAsync();

                preNA = DB.SetupModules.Where(x => x.CourseId == courses.CourseId).Where(x => x.TrainingTypeId == 1).Any();
                duringNA = DB.SetupModules.Where(x => x.CourseId == courses.CourseId).Where(x => x.TrainingTypeId == 2).Any();
                postNA = DB.SetupModules.Where(x => x.CourseId == courses.CourseId).Where(x => x.TrainingTypeId == 3).Any();
                var data = new ScoringCourseSummaryResponse
                {
                    PreX = totalPreEnrolled,
                    DuringX = totalDuringEnrolled,
                    PostX = totalPostEnrolled,
                    PreY = employeeIds.Count(),
                    DuringY = employeeIds.Count(),
                    PostY = employeeIds.Count(),
                    PrePercentage = Math.Round(prePercentage, 2),
                    DuringPercentage = Math.Round(duringPercentage, 2),
                    PostPercentage = Math.Round(postPercentage, 2),
                    PreNA = !preNA,
                    DuringNA = !duringNA,
                    PostNA = !postNA,
                };

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyTeamDetail(ScoringSummaryRequest request)
        {
            try
            {

                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;

                double counter = 0;
                double employeeCounter = 0;


                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }

                var intTrainingTypeId = 0;

                switch (request.TrainingTypeId.ToLower())
                {
                    case "pre":
                        intTrainingTypeId = 1;
                        break;
                    case "during":
                        intTrainingTypeId = 2;
                        break;
                    case "post":
                        intTrainingTypeId = 3;
                        break;
                    default:
                        break;
                }


                //var employees = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => employeeIds.Contains(x.EmployeeId)).OrderBy(x => x.EmployeeName).Skip(request.Page).Take(request.Limit).Select(x => new
                //{
                //    x.EmployeeName,
                //    x.EmployeeId
                //}).ToListAsync();

                var employees = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => employeeIds.Contains(x.EmployeeId)).OrderBy(x => x.EmployeeName).Select(x => new
                {
                    x.EmployeeName,
                    x.EmployeeId
                }).ToListAsync();



                var members = new List<ScoringCourseSummaryMemberResponse>();

                foreach (var item in employees)
                {

                    var memberData = new ScoringCourseSummaryMemberResponse
                    {
                        //EmployeeName = await this.DB.Employees.Where(q => q.EmployeeId == item.EmployeeId).Select(q => q.EmployeeName).FirstOrDefaultAsync(),
                        EmployeeName = item.EmployeeName
                    };

                    var dataList = new List<ScoringCourseSummaryAttemptResponse>();

                    var masterData = await this.DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Where(x => x.EnrollLearning.EmployeeId == item.EmployeeId).Where(x => x.EnrollLearning.TrainingId == request.TrainingId).Include(x => x.SetupModule).Where(x => x.SetupModule.TrainingTypeId == intTrainingTypeId).Select(x => x.SetupModuleId).Distinct().ToListAsync();

                    if (masterData != null)
                    {
                        counter++;
                        foreach (var datum in masterData)
                        {
                            var assesments = await this.DB.SetupModules.Include(x => x.Assesment).Where(x => x.SetupModuleId == datum).Where(x => x.AssesmentId != null).ToListAsync();
                            foreach (var assesment in assesments)
                            {
                                var skillChecks = await this.DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).Where(x => x.AssesmentGUID == assesment.AssesmentId).FirstOrDefaultAsync();
                                var sub = new ScoringCourseSummaryAttemptResponse
                                {
                                    Name = skillChecks.AssesmentsNavigator.Name,
                                    //Status = this.DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.AssesmentId == skillChecks.AssesmentGUID).Where(x=> x.EndTime != null).Any(),
                                    EnumStatus = GetTrackingProgressStatus(item.EmployeeId, assesment.AssesmentId, datum, request.TrainingId).Status,
                                    Score = GetTrackingProgressStatus(item.EmployeeId, assesment.AssesmentId, datum, request.TrainingId).Score,

                                };
                                dataList.Add(sub);
                            }

                            var modules = await this.DB.SetupModules.Include(x => x.Module).Where(x => x.SetupModuleId == datum).Where(x => x.ModuleId != null).ToListAsync();
                            foreach (var module in modules)
                            {
                                var modulenya = await this.DB.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefaultAsync();
                                var sub = new ScoringCourseSummaryAttemptResponse
                                {
                                    Name = modulenya.ModuleName,
                                    //Status = this.DB.EnrollLearningTimes.Include(x=> x.SetupModule).Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).Where(x=> x.SetupModuleId == datum).Where(x=> x.SetupModule.ModuleId == modulenya.ModuleId).Where(x=> x.EndTime != null).Any(),
                                    EnumStatus = GetTrackingProgressStatusModule(item.EmployeeId, modulenya.ModuleId, datum, request.TrainingId).Status,
                                    Score = GetTrackingProgressStatusModule(item.EmployeeId, modulenya.ModuleId, datum, request.TrainingId).Score

                                };
                                dataList.Add(sub);
                            }
                        }
                    }

                    memberData.Scores = dataList;
                    memberData.Count = dataList.Count();

                    if (memberData.Scores != null && memberData.Scores.Count > 0)
                    {
                        int tempCounter = 0;
                        foreach (var item2 in dataList)
                        {
                            if (item2.EnumStatus == "2")
                            {
                                tempCounter++;
                            }
                            if (item2.EnumStatus == "3")
                            {
                                tempCounter++;
                            }
                        }

                        if (tempCounter == dataList.Count)
                        {
                            employeeCounter++;
                        }

                        //if (memberData.Scores.Where(x => x.EnumStatus != "1").Any())
                        //{
                        //    employeeCounter++;
                        //};
                    }
                    members.Add(memberData);

                }


                double percentage = ((double)employeeCounter) / employeeIds.Count * 100;

                var data = new ScoringCourseSummaryResponseDetail
                {
                    Percentage = Math.Round(percentage, 2),
                    //Data = members.Skip(request.Page).Take(request.Limit).OrderByDescending(x => x.Count).ThenBy(x => x.EmployeeName).ToList(),
                    Data = members.OrderBy(x => x.EmployeeName).Skip(request.Page).Take(request.Limit).ToList(),

                };

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetScoringCourseSummaryMyCoachDetail(ScoringSummaryRequest request, string filetype)
        {
            // try
            // {
            if (request.Limit == 0)
            {
                request.Limit = 10;
            }

            if (request.Page == 0)
            {
                request.Page = 1;
            }

            request.Page = (request.Page - 1) * request.Limit;

            double counter = 0;

            var headerList = new List<ScoringCourseSummaryAttemptResponse>();
            var tempHeaderList = new List<ScoringCourseSummaryAttemptResponse>();

            var employeeList = new List<ScoringCourseSummaryMemberResponse>();

            var employeeIds = new List<EmployeeByCoach>();
            if (!request.IsCoach)
            {
                //var query = this.DB.Teams.AsQueryable();

                //if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                //{
                //    query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                //}

                //var team = query.FirstOrDefault();

                //employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
            }
            else
            {
                var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                if (coachData != null)
                {
                    if (coachData.Category != null)
                    {
                        if (coachData.Category.ToLower() == "dealer")
                        {
                            var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                            if (queryDealer != null)
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId.ToString()).ToListAsync();
                            }
                        }
                        else if (coachData.Category.ToLower() == "outlet")
                        {
                            var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                            if (queryDealer != null)
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId.ToString(), queryDealer.OutletId.ToString()).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).ToListAsync();
                        }
                    }
                    else
                    {
                        employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).ToListAsync();
                    }
                }

            }

            var intTrainingTypeId = 0;
            var strCategory = "";
            switch (request.TrainingTypeId.ToLower())
            {
                case "pre":
                    intTrainingTypeId = 1;
                    strCategory = "Pre";
                    break;
                case "during":
                    intTrainingTypeId = 2;
                    strCategory = "During";
                    break;
                case "post":
                    intTrainingTypeId = 3;
                    strCategory = "Post";
                    break;
                default:
                    break;
            }

            //var employees = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => new
            //{
            //    x.EmployeeId,
            //    x.EmployeeName,
            //    x.Outlet
            //}).ToListAsync();

            //var employees = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => new ScoringCourseSummaryMemberResponse
            //{
            //    EmployeeName = x.EmployeeName,
            //    EmployeeId = x.EmployeeId,
            //    OutletName = x.Outlet == null ? "" : x.Outlet.Name,
            //}).ToListAsync();

            //employeeList = employees.Select(x => new ScoringCourseSummaryMemberResponse
            //    {
            //        EmployeeName = x.EmployeeName,
            //        EmployeeId = x.EmployeeId,
            //        OutletName = x.Outlet == null ? "" : x.Outlet.Name,
            //    }).ToList();

            //var elt = DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).Where(q => employeeIds.Contains(q.EnrollLearning.EmployeeId)).Where(x => x.EnrollLearning.TrainingId == request.TrainingId).Select(x => x.SetupModule).ToList();
            //if (intTrainingTypeId > 0)
            //{
            //    elt = elt.Where(x => x.TrainingTypeId == intTrainingTypeId).ToList();
            //}

            //elt = elt.OrderBy(x => x.Order).ToList();

            var dataSetupLearning = (from t in DB.Trainings
                                     join c in DB.Courses on t.CourseId equals c.CourseId
                                     join sm in DB.SetupModules on c.CourseId equals sm.CourseId
                                     where t.TrainingId == request.TrainingId && t.IsDeleted == false && sm.IsDeleted == false
                                     select new
                                     { sm.AssesmentId, sm.ModuleId, sm.SetupModuleId, sm.TrainingTypeId, sm.Order }).ToList();

            if (intTrainingTypeId > 0)
            {
                dataSetupLearning = dataSetupLearning.Where(x => x.TrainingTypeId == intTrainingTypeId).ToList();
            }


            var assesmentId = dataSetupLearning.Where(x => x.AssesmentId != null).Select(x => x.AssesmentId).ToList();

            //var skillCheckId = DB.AssesmentSkillChecks.Include(x => x.SkillChecksNavigator).Where(x => assesmentId.Contains(x.AssesmentGUID)).Select(x => new ScoringCourseSummaryAttemptResponse
            //{
            //    GUID = x.SkillCheckGUID,
            //    ParentId = x.AssesmentGUID,
            //    Name = x.SkillChecksNavigator == null ? "" : x.SkillChecksNavigator.Name,
            //    Category = "Assesments"
            //}).Distinct().ToList();

            var skillCheckId = (from c in DB.AssesmentSkillChecks
                                join a in DB.Assesments on c.AssesmentGUID equals a.GUID
                                join sc in DB.SkillChecks on c.SkillCheckGUID equals sc.GUID
                                join sm in DB.SetupModules on a.GUID equals sm.AssesmentId
                                where assesmentId.Contains(c.AssesmentGUID)
                                select new ScoringCourseSummaryAttemptResponse
                                {
                                    GUID = c.SkillCheckGUID,
                                    ParentId = sm.AssesmentId,
                                    Name = sc.Name == null ? "" : sc.Name,
                                    Category = "Assesments",
                                    Orders = sm.Order,
                                    ChildOrders = c.Order
                                }).Distinct().ToList();


            var moduleId = dataSetupLearning.Where(x => x.ModuleId != null).Select(x => new ScoringCourseSummaryAttemptResponse
            {
                GUID = x.ModuleId.ToString(),
                ParentId = x.SetupModuleId.ToString(),
                Name = DB.Modules.Where(xx => xx.ModuleId == x.ModuleId).FirstOrDefault() == null ? "" : DB.Modules.Where(xx => xx.ModuleId == x.ModuleId).FirstOrDefault().ModuleName,
                Category = "Module",
                Orders = x.Order,
                ChildOrders = 0
            }).ToList();

            tempHeaderList.AddRange(skillCheckId);
            tempHeaderList.AddRange(moduleId);

            tempHeaderList = tempHeaderList.Distinct().ToList();

            foreach (var temp in tempHeaderList.OrderBy(q => q.ParentId))
            {
                if (!headerList.Select(x => x.GUID).ToList().Contains(temp.GUID))
                {
                    //if (temp.Category == "Module")
                    //{
                    //    temp.Orders =
                    //    elt.Where(op => op.SetupModuleId.ToString() == temp.ParentId).Where(op => op.ModuleId.ToString() == temp.GUID).FirstOrDefault() == null ? 0 : elt.Where(op => op.SetupModuleId.ToString() == temp.ParentId).Where(op => op.ModuleId.ToString() == temp.GUID).FirstOrDefault().Order;
                    //}
                    //else
                    //{
                    //    temp.Orders = elt.Where(op => op.SetupModuleId.ToString() == temp.ParentId).Where(op => op.ModuleId.ToString() == temp.GUID).FirstOrDefault() == null ? 0 : elt.Where(op => op.AssesmentId == temp.ParentId).FirstOrDefault().Order;
                    //}
                    headerList.Add(temp);
                }
            }



            //var tempListEmployee = employeeList.OrderBy(x => x.OutletName).ThenBy(x => x.EmployeeName).ToList();
            var tempListEmployee = employeeIds.OrderBy(x => x.OutletName).ThenBy(x => x.EmployeeName).ToList();

            var listEmployee = new List<ScoringCourseSummaryMemberResponse>();

            //foreach (var datum in tempListEmployee)
            //{
            //    if (!listEmployee.Select(x => x.EmployeeId).ToList().Contains(datum.EmployeeId))
            //    {
            //        listEmployee.Add(datum);
            //    }
            //}


            var generateExcel = await GenerateExcel(tempListEmployee, headerList.OrderBy(x => x.Orders).ThenBy(x => x.ChildOrders).ToList(), request.TrainingId.ToString());

            var reportName = "Scoring Detail Report (" + strCategory + ")-" + DateTime.UtcNow.AddHours(7).ToShortDateString(); ;
            return File(generateExcel, FormatDocumentEnum.ExcelFormat, reportName + "." + filetype);
            // }
            // catch (Exception x)
            // {
            //     return StatusCode(500, BaseResponse.Error(null, x));
            // }
        }


        public async Task<ActionResult<BaseResponse>> GetCourseCategorySummaryMyTeamDetail(ScoringSummaryRequest request)
        {
            try
            {
                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;

                var listEnrollmentMembers = new List<ScoringSummaryDetailMemberResponse>();
                var listCertificateMembers = new List<ScoringSummaryDetailMemberResponse>();

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                    //employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Skip(request.Page).Take(request.Limit).Select(x => x.EmployeeId).ToList();

                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                    //employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).Skip(request.Page).Take(request.Limit).ToListAsync();

                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                    //employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Skip(request.Page).Take(request.Limit).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                                //employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Skip(request.Page).Take(request.Limit).Select(x => x.EmployeeId).ToListAsync();

                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            //employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Skip(request.Page).Take(request.Limit).Select(x => x.EmployeeId).ToListAsync();

                        }
                    }

                }

                double totalCertificate = 0;
                double totalEnrolled = 0;



                //var employees = await this.DB.Employees.Where(x=> x.IsDeleted == false).Where(x=> employeeIds.Contains(x.EmployeeId)).OrderBy(x=> x.EmployeeName).Skip(request.Page).Take(request.Limit).Select(x=> new {
                //    x.EmployeeId,
                //    x.EmployeeName
                //}).ToListAsync();

                foreach (var item in employeeIds)
                {
                    var enrollLearning = this.DB.EnrollLearnings.Where(x => x.EmployeeId == item).Where(x => x.TrainingId == request.TrainingId).FirstOrDefault();

                    if (enrollLearning != null)
                    {
                        totalEnrolled++;
                    }

                    if (enrollLearning != null)
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = await this.DB.Employees.Where(q => q.EmployeeId == item).Select(q => q.EmployeeName).FirstOrDefaultAsync(),
                            EnrollmentDate = enrollLearning.CreatedAt.ToString(),
                        };

                        listEnrollmentMembers.Add(datum);
                    }
                    else
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = await this.DB.Employees.Where(q => q.EmployeeId == item).Select(q => q.EmployeeName).FirstOrDefaultAsync(),
                            EnrollmentDate = null,
                        };

                        listEnrollmentMembers.Add(datum);
                    }

                    var certificateEmployee = this.DB.EmployeeCertificates.Where(x => x.TrainingId == request.TrainingId).Where(x => x.EmployeeId == item).FirstOrDefault();

                    if (certificateEmployee != null)
                    {
                        totalCertificate++;
                    }

                    if (certificateEmployee != null)
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = await this.DB.Employees.Where(q => q.EmployeeId == item).Select(q => q.EmployeeName).FirstOrDefaultAsync(),
                            EnrollmentDate = certificateEmployee.EventDate.ToString(),
                        };
                        listCertificateMembers.Add(datum);
                    }
                    else
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = await this.DB.Employees.Where(q => q.EmployeeId == item).Select(q => q.EmployeeName).FirstOrDefaultAsync(),
                            EnrollmentDate = null,
                        };

                        listCertificateMembers.Add(datum);
                    }

                }

                double enrollmentPercentage = (totalEnrolled / (double)employeeIds.Count()) * 100;
                double certificationPercentage = (totalCertificate / (double)employeeIds.Count()) * 100;

                var data = new ScoringSummaryDetailResponse
                {
                    CertificatePercentage = Math.Round(certificationPercentage, 2),
                    EnrollmentPercentage = Math.Round(enrollmentPercentage, 2),
                    EnrollmentMembers = listEnrollmentMembers.OrderBy(q => q.EmployeeName).Skip(request.Page).Take(request.Limit).ToList(),
                    CertificateMembers = listCertificateMembers.OrderBy(q => q.EmployeeName).Skip(request.Page).Take(request.Limit).ToList(),
                };

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetCourseCategorySummaryMyCoachDetail(ScoringSummaryRequest request, string filetype)
        {
            try
            {


                request.Page = (request.Page - 1) * request.Limit;

                var listEnrollmentMembers = new List<ScoringSummaryDetailMemberResponse>();
                var listCertificateMembers = new List<ScoringSummaryDetailMemberResponse>();

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }


                double totalCertificate = 0;
                double totalEnrolled = 0;

                //var employees = await this.DB.Employees.Where(x=> x.IsDeleted == false).Include(x=> x.Outlet).Where(x=> employeeIds.Contains(x.EmployeeId)).Select(x=> new {
                //    x.EmployeeName,
                //    x.EmployeeId,
                //    x.Outlet
                //}).ToListAsync();

                foreach (var item in employeeIds)
                {
                    var enrollLearning = this.DB.EnrollLearnings.Where(x => x.EmployeeId == item).Where(x => x.TrainingId == request.TrainingId).FirstOrDefault();

                    if (enrollLearning != null)
                    {
                        totalEnrolled++;
                    }

                    var employeeData = await this.DB.Employees.Where(q => q.EmployeeId == item).FirstOrDefaultAsync();

                    if (enrollLearning != null)
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = employeeData.EmployeeName,
                            EnrollmentDate = enrollLearning.CreatedAt.ToString("MM/dd/yyyy hh:mm:ss"),
                            OutletName = await this.DB.Outlets.Where(q => q.OutletId == employeeData.OutletId).Select(q => q.Name).FirstOrDefaultAsync(),
                        };

                        listEnrollmentMembers.Add(datum);
                    }
                    else
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = employeeData.EmployeeName,
                            EnrollmentDate = null,
                            OutletName = await this.DB.Outlets.Where(q => q.OutletId == employeeData.OutletId).Select(q => q.Name).FirstOrDefaultAsync(),

                        };

                        listEnrollmentMembers.Add(datum);
                    }

                    var certificateEmployee = this.DB.EmployeeCertificates.Where(x => x.TrainingId == request.TrainingId).Where(x => x.EmployeeId == item).FirstOrDefault();

                    if (certificateEmployee != null)
                    {
                        totalCertificate++;
                    }


                    if (certificateEmployee != null)
                    {
                        var datum = new ScoringSummaryDetailMemberResponse
                        {
                            EmployeeName = employeeData.EmployeeName,
                            OutletName = await this.DB.Outlets.Where(q => q.OutletId == employeeData.OutletId).Select(q => q.Name).FirstOrDefaultAsync(),
                            EnrollmentDate = certificateEmployee.EventDate.ToString("MM/dd/yyyy hh:mm:ss"),
                        };
                        listCertificateMembers.Add(datum);
                    }
                }

                //foreach (var item in employeeIds)
                //{
                //    var certificateEmployee = this.DB.EmployeeCertificates.Where(x => x.TrainingId == request.TrainingId).Where(x => x.EmployeeId == item).FirstOrDefault();

                //    if (certificateEmployee != null)
                //    {
                //        totalCertificate++;
                //    }

                //    var employeeData = await this.DB.Employees.Where(q => q.EmployeeId == item).FirstOrDefaultAsync();

                //    if (certificateEmployee != null)
                //    {
                //        var datum = new ScoringSummaryDetailMemberResponse
                //        {
                //            EmployeeName = employeeData.EmployeeName,
                //            OutletName = await this.DB.Outlets.Where(q => q.OutletId == employeeData.OutletId).Select(q => q.Name).FirstOrDefaultAsync(),
                //            EnrollmentDate = certificateEmployee.EventDate.ToString("MM/dd/yyyy hh:mm:ss"),
                //        };
                //        listCertificateMembers.Add(datum);
                //    }
                //}

                double enrollmentPercentage = (totalEnrolled / (double)employeeIds.Count()) * 100;
                double certificationPercentage = (totalCertificate / (double)employeeIds.Count()) * 100;

                var data = new ScoringSummaryDetailResponse
                {
                    CertificatePercentage = Math.Round(certificationPercentage, 2),
                    EnrollmentPercentage = Math.Round(enrollmentPercentage, 2),
                    EnrollmentMembers = listEnrollmentMembers,
                    CertificateMembers = listCertificateMembers,
                };

                var paramData = new List<ScoringCoachExcel>();

                if (request.EnumCategory.ToLower() == "enrollment")
                {
                    paramData = data.EnrollmentMembers.Select(o => new ScoringCoachExcel
                    {
                        Name = o.EmployeeName,
                        OutletName = o.OutletName,
                        EnrollmentDate = o.EnrollmentDate,
                    }).ToList();
                }
                else
                {
                    paramData = data.CertificateMembers.Select(o => new ScoringCoachExcel
                    {
                        Name = o.EmployeeName,
                        OutletName = o.OutletName,
                        EnrollmentDate = o.EnrollmentDate,
                    }).ToList();
                }

                paramData = paramData.OrderBy(x => x.OutletName).ThenBy(x => x.Name).ToList();

                var generateExcel = await GenerateExcelSummary(paramData, request.TrainingId.ToString());

                var reportName = "Scoring Report (" + request.EnumCategory + ")-" + DateTime.UtcNow.AddHours(7).ToShortDateString(); ;
                return File(generateExcel, FormatDocumentEnum.ExcelFormat, reportName + "." + filetype);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> RemedialReportMyTeam(ScoringReportRequest request)
        {
            try
            {

                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;

                var moduleDetail = new List<ScoringReportDataDetail>();
                var scoringReportDataDetailsAssesment = new List<ScoringReportDataDetail>();
                var scoringReportDataAssesment = new List<ScoringReportData>();
                var scoringReportDataModule = new List<ScoringReportData>();
                var employeeIds = new List<String>();

                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = await this.DB.TeamDetails.Include(x => x.Employee).Where(x => x.TeamId == team.TeamId).Select(x => x.EmployeeId).ToListAsync();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    var outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                }
                                else
                                {
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer.OutletId).Select(x => x.OutletId).ToListAsync();
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                }
                                else
                                {
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }
                var employees = DB.Employees.Where(x => x.IsDeleted == false).Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => new
                {
                    x.EmployeeName,
                    x.EmployeeId
                }).ToList();

                if (request.EmployeeId != "" && request.EmployeeId != null)
                {
                    employees = employees.Where(x => x.EmployeeId == request.EmployeeId).ToList();
                }

                var response = employees.Select(x => new RemedialReportDatas
                {
                    EmployeeGUID = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                }).ToList();

                if (request.EmployeeId != "" && request.EmployeeId != null)
                {
                    response = response.Where(x => x.EmployeeGUID == request.EmployeeId).ToList();
                }

                foreach (var e in response)
                {

                    var q = DB.SkillChecks.Include(x => x.AssesmentsSkillCheckNavigation).Include(x => x.LiveAssesmentSkillCheckNavigation);


                    var SkillCheckData = q.Where(x => x.LiveAssesmentSkillCheckNavigation.Where(sx => sx.TrainingId == request.TrainingID).Where(sx => sx.AssesmentGUID == request.AssesmentGuid).Where(sx => sx.EmployeeGUID == e.EmployeeGUID).Any()).Select(x => new RemedialReportDataDetailSkillCheck
                    {
                        SkillCheckName = x.Name,
                        SkillCheckGUID = x.GUID,
                        Order = DB.AssesmentSkillChecks.Where(zxx => zxx.SkillCheckGUID == x.GUID).Where(zxx => zxx.AssesmentGUID == request.AssesmentGuid).FirstOrDefault().Order,
                    }).ToList();

                    SkillCheckData = SkillCheckData.OrderBy(x => x.Order).ToList();


                    foreach (var f in SkillCheckData)
                    {
                        var assesmentScore = DB.LiveAssesmentSkillChecks.Include(x => x.ScorerNavigator).Where(x => x.AssesmentGUID == request.AssesmentGuid).Where(x => x.EmployeeGUID == e.EmployeeGUID).Where(x => x.SkillCheckGUID == f.SkillCheckGUID).Where(x => x.TrainingId == request.TrainingID).Select(x => new RemedialReportDataDetailScore
                        {
                            Attempts = x.Attempts,
                            CreatedAt = x.CreatedAt.ToString(),
                            Feedback = x.Text,
                            IsFinished = x.IsFinished,
                            IsDraft = x.IsDraft,
                            IsScored = x.IsScored,
                            Score = x.Score,
                            ScoringBy = x.ScorerNavigator == null ? "" : x.ScorerNavigator.EmployeeName,
                        }).OrderByDescending(x => x.CreatedAt).ToList();
                        f.Scores = assesmentScore;
                    }

                    e.Detail = SkillCheckData;

                }

                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> SkillCheckPerUser(ScoringReportRequest request)
        {
            try
            {

                var moduleDetail = new List<ScoringReportDataDetail>();
                var scoringReportDataDetailsAssesment = new List<ScoringReportDataDetail>();
                var scoringReportDataAssesment = new List<ScoringReportData>();
                var scoringReportDataModule = new List<ScoringReportData>();


                var employees = await this.DB.Employees.Where(x => x.IsDeleted == false)
                   .Where(q => q.EmployeeId == request.EmployeeId)

                   .Select(x => new Employees { EmployeeId = x.EmployeeId, EmployeeName = x.EmployeeName })
                   .ToListAsync();


                var response = employees.Select(x => new RemedialReportData
                {
                    EmployeeGUID = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                }).ToList();


                foreach (var e in response)
                {
                    var queries = this.DB.LiveAssesmentSkillChecks.Include(x => x.SkillCheckNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).Include(x => x.LiveAssesmentSkillCheckScoreNavigation).Include(x => x.ScorerNavigator).Where(x => x.EmployeeGUID == e.EmployeeGUID);
                    if (!String.IsNullOrWhiteSpace(request.AssesmentGuid))
                    {
                        queries = queries.Where(x => x.AssesmentGUID == request.AssesmentGuid);
                    }

                    if (!String.IsNullOrWhiteSpace(request.SkillCheckGUID))
                    {
                        queries = queries.Where(x => x.SkillCheckGUID == request.SkillCheckGUID);
                    }

                    if (request.Attempts != null)
                    {
                        queries = queries.Where(x => x.Attempts == request.Attempts);
                    }


                    var liveAssesmentSkillCheckList = await queries.Where(x => x.TrainingId == request.TrainingID).OrderByDescending(x => x.CreatedAt).ToListAsync();

                    var detailData = new List<RemedialReportDataDetail>();
                    foreach (var liveAssesmentSkillChecks in liveAssesmentSkillCheckList)
                    {
                        if (!detailData.Select(x => x.SkillCheckGuid).ToList().Contains(liveAssesmentSkillChecks.SkillCheckGUID))
                        {
                            var subData = new RemedialReportDataDetail
                            {
                                SkillCheckGuid = liveAssesmentSkillChecks.SkillCheckGUID,
                                Guid = liveAssesmentSkillChecks.GUID,
                                TrainingID = liveAssesmentSkillChecks.TrainingId,
                                Attempts = liveAssesmentSkillChecks.Attempts,
                                Score = liveAssesmentSkillChecks.Score,
                                Feedback = liveAssesmentSkillChecks.Text,
                                SkillCheckName = liveAssesmentSkillChecks.SkillCheckNavigator.Name,
                                ScoringBy = liveAssesmentSkillChecks.ScorerNavigator == null ? "" : liveAssesmentSkillChecks.ScorerNavigator.EmployeeName,
                                CreatedAt = liveAssesmentSkillChecks.CreatedAt.ToString(),
                                IsDraft = liveAssesmentSkillChecks.IsDraft,
                                IsFinished = liveAssesmentSkillChecks.IsFinished,
                                IsScored = liveAssesmentSkillChecks.IsScored,
                                Description = liveAssesmentSkillChecks.SkillCheckNavigator.Description,
                                ScoreDescription = liveAssesmentSkillChecks.SkillCheckNavigator.SkillCheckScoreConfigsNavigation.Where(x => x.SkillCheckGUID == liveAssesmentSkillChecks.SkillCheckGUID).Where(x => x.Score == liveAssesmentSkillChecks.Score).FirstOrDefault() == null ? "" : liveAssesmentSkillChecks.SkillCheckNavigator.SkillCheckScoreConfigsNavigation.Where(x => x.SkillCheckGUID == liveAssesmentSkillChecks.SkillCheckGUID).Where(x => x.Score == liveAssesmentSkillChecks.Score).FirstOrDefault().Description,
                                ScoreTitle = liveAssesmentSkillChecks.SkillCheckNavigator.SkillCheckScoreConfigsNavigation.Where(x => x.SkillCheckGUID == liveAssesmentSkillChecks.SkillCheckGUID).Where(x => x.Score == liveAssesmentSkillChecks.Score).FirstOrDefault() == null ? "" : liveAssesmentSkillChecks.SkillCheckNavigator.SkillCheckScoreConfigsNavigation.Where(x => x.SkillCheckGUID == liveAssesmentSkillChecks.SkillCheckGUID).Where(x => x.Score == liveAssesmentSkillChecks.Score).FirstOrDefault().Text,
                                Orders = DB.AssesmentSkillChecks.Where(ox => ox.AssesmentGUID == liveAssesmentSkillChecks.AssesmentGUID).Where(x => x.SkillCheckGUID == liveAssesmentSkillChecks.SkillCheckGUID).First().Order,
                            };
                            detailData.Add(subData);
                        }
                    }
                    e.Detail = detailData.OrderBy(x => x.Orders).ToList();
                }



                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }



        public async Task<ActionResult<BaseResponse>> GetCourseByUser(CourseReportRequest request)
        {
            try
            {

                var responseData = new List<ResponseListUser>();
                var employeeIds = new List<string>();

                if (!request.IsCoach)
                {
                    var query = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorEmployeeGuid))
                    {
                        query = query.Where(x => x.TeamLeaderId == request.SuperiorEmployeeGuid);
                    }

                    var team = query.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    var outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                }
                                else
                                {
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer.OutletId).Select(x => x.OutletId).ToListAsync();
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                }
                                else
                                {
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }

                }
                var counter = 0;

                var employees = DB.Employees.Where(x => x.IsDeleted == false).Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => new
                {
                    x.EmployeeId,
                    x.EmployeeName
                }).ToList();

                foreach (var employee in employees)
                {
                    var responseDataDetail = new List<ResponseCourse>();

                    var employeeData = new ResponseListUser
                    {
                        EmployeeGUID = employee.EmployeeId,
                        EmployeeName = employee.EmployeeName
                    };
                    responseData.Add(employeeData);

                    var elData = this.DB.EnrollLearnings.Where(z => z.EmployeeId == employee.EmployeeId).Where(y => y.SetupModuleId != null).Include(zx => zx.Training).ThenInclude(xy => xy.Course).OrderByDescending(q => q.CreatedAt).ToList();
                    if (elData.Count != 0)
                    {
                        foreach (var item in elData)
                        {
                            if (item.Training != null)
                            {
                                var course = await DB.Courses.Include(x => x.TrainingRatings).Include(x => x.Blob).FirstOrDefaultAsync(x => x.CourseId == item.Training.CourseId);
                                var trainingRatings = this.DB.TrainingRatings.Where(xx => xx.CourseId == course.CourseId).FirstOrDefault();
                                var blob = this.DB.Blobs.Where(xyz => xyz.BlobId == course.BlobId).FirstOrDefault();
                                var timePoint = DB.SetupModules.Where(xzx => xzx.CourseId == course.CourseId).Include(xzzx => xzzx.TimePoint).FirstOrDefault();

                                var imageURL = blob == null ? "" : await this.FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
                                var coursePercentage = this.DB.CourseTrainingTypeMappings.Where(cx => cx.CourseId == course.CourseId).FirstOrDefault();

                                responseDataDetail.Add(new ResponseCourse
                                {
                                    CourseName = course == null ? "" : course.CourseName,
                                    CourseRating = trainingRatings == null ? "0" : trainingRatings.RatingScore.ToString(),
                                    CourseImageUrl = imageURL,
                                    CoursePrice = course == null ? 0 : course.CoursePrice,
                                    CoursePercentage = coursePercentage == null ? 0 : coursePercentage.Percentage,
                                    CourseTimePoint = timePoint == null ? 0 : timePoint.TimePoint.Time,
                                });
                            }
                        }
                        responseData[counter].Courses = responseDataDetail;
                    }
                    counter++;
                }

                return StatusCode(200, BaseResponse.ResponseOk(responseData));
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetCoursesListUser(CourseReportRequest request)
        {
            //request.IsGetAllCourse = true;
            //try
            {

                if (request.Limit == 0)
                {
                    request.Limit = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limit;

                var responseData = new List<ResponseCourse>();
                var listTraining = new List<int>();
                var employeeIds = new List<string>();
                var outletIds = new List<string>();

                if (!request.IsCoach)
                {
                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                    outletIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorEmployeeGuid).Where(x => x.Employee.IsDeleted == false).Select(x => x.Employee.OutletId).ToList();

                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {

                                    //var outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                    outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();


                                }
                                else
                                {
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                    outletIds = await this.DB.Outlets.Select(x => x.OutletId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    //var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer.OutletId).Select(x => x.OutletId).ToListAsync();
                                    outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer.OutletId).Select(x => x.OutletId).ToListAsync();
                                    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();


                                }
                                else
                                {
                                    //employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                    outletIds = await this.DB.Outlets.Select(x => x.OutletId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                outletIds = await this.DB.Outlets.Select(x => x.OutletId).ToListAsync();
                            }
                        }
                        else
                        {
                            employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                            outletIds = await this.DB.Outlets.Select(x => x.OutletId).ToListAsync();
                        }
                    }

                }
                if (!request.IsGetAllCourse)
                {
                    var positionIds = await DB.EmployeePositionMappings.Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => x.PositionId).ToListAsync();

                    //var outletFilterIds = await DB.Outlets.Where(x => x.OutletId.Contains(x.OutletId)).Select(x => x.OutletId).ToListAsync();

                    var trainingPositionMapping = await DB.TrainingPositionMappings.Where(x => positionIds.Contains(x.PositionId)).Select(x => x.TrainingId).ToListAsync();
                    //var query = DB.Trainings.Where(x => trainingPositionMapping.Contains(x.TrainingId)).Include(x => x.Course).Where(x => x.IsDeleted == false);


                    var trainingOutletMapping = await DB.TrainingOutletMappings.Where(x => outletIds.Contains(x.OutletId) && trainingPositionMapping.Contains(x.TrainingId)).Select(x => x.TrainingId).ToListAsync();

                    var query = DB.Trainings.Where(x => trainingOutletMapping.Contains(x.TrainingId)).Include(x => x.Course).Where(x => x.IsDeleted == false);

                    var checkCount = query.ToList().Count();

                    if (!string.IsNullOrEmpty(request.Search))
                    {
                        query = query.Where(x => x.Course.CourseName.Contains(request.Search));
                    }

                    if (request.ProgramTypeId != null)
                    {
                        query = query.Where(x => x.Course.ProgramTypeId == request.ProgramTypeId.Value);
                    }

                    var checkCountII = query.ToList().Count();

                    var trainingDatas = query.Skip(request.Page).Take(request.Limit).ToList();
                    foreach (var item in trainingDatas)
                    {
                        var tempData = new ResponseCourse();
                        if (item != null)
                        {
                            var course = await DB.Courses.Include(x => x.TrainingRatings).Include(x => x.Blob).Include(x => x.CourseTrainingTypeMappings).Include(x => x.SetupModules).ThenInclude(x => x.TimePoint).FirstOrDefaultAsync(x => x.CourseId == item.CourseId);
                            var trainingRatings = course.TrainingRatings.FirstOrDefault();
                            var blob = course.Blob;
                            var timePoint = course.SetupModules.FirstOrDefault();
                            var imageURL = blob == null ? "" : await this.FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
                            var coursePercentage = course.CourseTrainingTypeMappings.FirstOrDefault();
                            //var course = item.Training.Course;
                            //var trainingRatings = this.DB.TrainingRatings.Where(xx => xx.CourseId == course.CourseId).FirstOrDefault();
                            // var listAssesment = this.DB.SetupModules.Where(zxxcv => zxxcv.CourseId == course.CourseId).Where(x => x.AssesmentId != null).Any();
                            //var blob = this.DB.Blobs.Where(xyz => xyz.BlobId == course.BlobId).FirstOrDefault();
                            //var timePoint = DB.SetupModules.Where(xzx => xzx.CourseId == course.CourseId).Include(xzzx => xzzx.TimePoint).FirstOrDefault();
                            var trainingData = item.EndDate < DateTime.UtcNow.AddHours(7);
                            // var imageURL = blob == null ? "" : await this.FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
                            // var coursePercentage = this.DB.CourseTrainingTypeMappings.Where(cx => cx.CourseId == course.CourseId).FirstOrDefault();
                            //var listAssesment = course.SetupModules.Where(x => x.AssesmentId != null).Any();
                            var data = (from sm in DB.SetupModules
                                        join c in DB.Courses on sm.CourseId equals c.CourseId
                                        join t in DB.Trainings on c.CourseId equals t.CourseId
                                        where t.TrainingId == item.TrainingId && sm.IsDeleted == false && sm.AssesmentId != null
                                        select sm).ToList();

                            var listAssesment = false;
                            if (data.Count() > 0)
                            {
                                listAssesment = true;
                            }

                            tempData = (new ResponseCourse
                            {
                                CourseID = item.CourseId,
                                TrainingID = item.TrainingId,
                                CourseName = course == null ? "" : course.CourseName,
                                CourseRating = trainingRatings == null ? "0" : trainingRatings.RatingScore.ToString(),
                                CourseImageUrl = imageURL,
                                CoursePrice = course == null ? 0 : course.CoursePrice,
                                CoursePercentage = coursePercentage == null ? 0 : coursePercentage.Percentage,
                                CourseTimePoint = timePoint == null ? 0 : timePoint.TimePoint.Time,
                                IsAssesment = listAssesment,
                                IsFinished = trainingData,
                                CreatedAt = item.CreatedAt,
                            });
                            var trainingCertifications = await DB.TrainingCertifications.Include(x => x.Training).Where(x => x.RelatedTrainingId == item.TrainingId).FirstOrDefaultAsync();
                            if (trainingCertifications != null)
                            {
                                tempData.RelatedCourseId = trainingCertifications.Training.CourseId;
                                tempData.RelatedTrainingId = trainingCertifications.TrainingId;
                            }
                            else
                            {
                                tempData.RelatedTrainingId = null;
                                tempData.RelatedCourseId = null;
                            }
                        }
                        if (item.TrainingId != null)
                        {
                            if (!listTraining.Contains(item.TrainingId))
                            {
                                responseData.Add(tempData);
                            }
                            listTraining.Add(item.TrainingId);
                        }

                    }
                }
                else
                {
                    var positionIds = await DB.EmployeePositionMappings.Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => x.PositionId).ToListAsync();

                    //var outletFilterIds = await DB.Outlets.Where(x => x.OutletId.Contains(x.OutletId)).Select(x => x.OutletId).ToListAsync();

                    var trainingPositionMapping = await DB.TrainingPositionMappings.Where(x => positionIds.Contains(x.PositionId)).Select(x => x.TrainingId).ToListAsync();
                    //var query = DB.Trainings.Where(x => trainingPositionMapping.Contains(x.TrainingId)).Include(x => x.Course).Where(x => x.IsDeleted == false);

                    //var trainingOutletMapping = await DB.TrainingOutletMappings.Where(x => outletIds.Contains(x.OutletId)).Select(x => x.TrainingId).ToListAsync();
                    var trainingOutletMapping = await DB.TrainingOutletMappings.Where(x => outletIds.Contains(x.OutletId) && trainingPositionMapping.Contains(x.TrainingId)).Select(x => x.TrainingId).ToListAsync();


                    var query = DB.Trainings.Where(x => trainingOutletMapping.Contains(x.TrainingId)).Include(x => x.Course).Where(x => x.IsDeleted == false);

                    var checkCount = query.ToList().Count();

                    //var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    //if (coachData != null)
                    //{
                    //    if (coachData.Category != null)
                    //    {
                    //        if (coachData.Category.ToLower() == "dealer")
                    //        {
                    //            var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                    //            if (queryDealer != null)
                    //            {
                    //                //var outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();

                    //                var trainingOutletMp = await this.DB.TrainingOutletMappings.Include(Q => Q.Training).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => outletIds.Contains(Q.Outlet.OutletId) && Q.Training.IsDeleted == false).Select(Q => Q.TrainingId).Distinct().ToListAsync();
                    //                query = query.Where(x => trainingOutletMp.Contains(x.TrainingId));
                    //            }
                    //            else
                    //            {
                    //                var trainingOutletMp = await this.DB.TrainingOutletMappings.Include(Q => Q.Training).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => Q.Training.IsDeleted == false).Select(Q => Q.TrainingId).Distinct().ToListAsync();
                    //                query = query.Where(x => trainingOutletMp.Contains(x.TrainingId));
                    //            }
                    //        }
                    //        else if (coachData.Category.ToLower() == "outlet")
                    //        {
                    //            var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorEmployeeGuid).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                    //            if (queryDealer != null)
                    //            {
                    //                //var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer.OutletId).Select(x => x.OutletId).ToListAsync();

                    //                var trainingOutletMp = await this.DB.TrainingOutletMappings.Include(Q => Q.Training).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => outletIds.Contains(Q.Outlet.OutletId) && Q.Training.IsDeleted == false).Select(Q => Q.TrainingId).Distinct().ToListAsync();
                    //                query = query.Where(x => trainingOutletMp.Contains(x.TrainingId));
                    //            }
                    //            else
                    //            {
                    //                var trainingOutletMp = await this.DB.TrainingOutletMappings.Include(Q => Q.Training).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => Q.Training.IsDeleted == false).Select(Q => Q.TrainingId).Distinct().ToListAsync();
                    //                query = query.Where(x => trainingOutletMp.Contains(x.TrainingId));
                    //            }
                    //        }
                    //        else
                    //        {
                    //            var trainingOutletMp = await this.DB.TrainingOutletMappings.Include(Q => Q.Training).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => Q.Training.IsDeleted == false).Select(Q => Q.TrainingId).Distinct().ToListAsync();
                    //            query = query.Where(x => trainingOutletMp.Contains(x.TrainingId));
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var trainingOutletMp = await this.DB.TrainingOutletMappings.Include(Q => Q.Training).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => Q.Training.IsDeleted == false).Select(Q => Q.TrainingId).Distinct().ToListAsync();
                    //        query = query.Where(x => trainingOutletMp.Contains(x.TrainingId));
                    //    }
                    //}

                    if (!string.IsNullOrEmpty(request.Search))
                    {
                        query = query.Where(x => x.Course.CourseName.Contains(request.Search));
                    }

                    if (request.ProgramTypeId != null)
                    {
                        query = query.Where(x => x.Course.ProgramTypeId == request.ProgramTypeId.Value);
                    }

                    var checkCountII = query.ToList().Count();

                    var trainingDatas = query.Skip(request.Page).Take(request.Limit).ToList();
                    foreach (var item in trainingDatas)
                    {
                        var tempData = new ResponseCourse();
                        if (item != null)
                        {
                            var course = await DB.Courses.Include(x => x.TrainingRatings).Include(x => x.Blob).Include(x => x.CourseTrainingTypeMappings).Include(x => x.SetupModules).ThenInclude(x => x.TimePoint).FirstOrDefaultAsync(x => x.CourseId == item.CourseId);
                            var trainingRatings = course.TrainingRatings.FirstOrDefault();
                            var blob = course.Blob;
                            var timePoint = course.SetupModules.FirstOrDefault();
                            var imageURL = blob == null ? "" : await this.FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
                            var coursePercentage = course.CourseTrainingTypeMappings.FirstOrDefault();
                            //var course = item.Training.Course;
                            //var trainingRatings = this.DB.TrainingRatings.Where(xx => xx.CourseId == course.CourseId).FirstOrDefault();
                            // var listAssesment = this.DB.SetupModules.Where(zxxcv => zxxcv.CourseId == course.CourseId).Where(x => x.AssesmentId != null).Any();
                            //var blob = this.DB.Blobs.Where(xyz => xyz.BlobId == course.BlobId).FirstOrDefault();
                            //var timePoint = DB.SetupModules.Where(xzx => xzx.CourseId == course.CourseId).Include(xzzx => xzzx.TimePoint).FirstOrDefault();
                            var trainingData = item.EndDate < DateTime.UtcNow.AddHours(7);
                            // var imageURL = blob == null ? "" : await this.FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
                            // var coursePercentage = this.DB.CourseTrainingTypeMappings.Where(cx => cx.CourseId == course.CourseId).FirstOrDefault();
                            //var listAssesment = course.SetupModules.Where(x => x.AssesmentId != null).Any();
                            var data = (from sm in DB.SetupModules
                                        join c in DB.Courses on sm.CourseId equals c.CourseId
                                        join t in DB.Trainings on c.CourseId equals t.CourseId
                                        where t.TrainingId == item.TrainingId && sm.IsDeleted == false && sm.AssesmentId != null
                                        select sm).ToList();

                            var listAssesment = false;
                            if (data.Count() > 0)
                            {
                                listAssesment = true;
                            }

                            tempData = (new ResponseCourse
                            {
                                CourseID = item.CourseId,
                                TrainingID = item.TrainingId,
                                CourseName = course == null ? "" : course.CourseName,
                                CourseRating = trainingRatings == null ? "0" : trainingRatings.RatingScore.ToString(),
                                CourseImageUrl = imageURL,
                                CoursePrice = course == null ? 0 : course.CoursePrice,
                                CoursePercentage = coursePercentage == null ? 0 : coursePercentage.Percentage,
                                CourseTimePoint = timePoint == null ? 0 : timePoint.TimePoint.Time,
                                IsAssesment = listAssesment,
                                IsFinished = trainingData,
                                CreatedAt = item.CreatedAt,
                            });
                            var trainingCertifications = await DB.TrainingCertifications.Include(x => x.Training).Where(x => x.RelatedTrainingId == item.TrainingId).FirstOrDefaultAsync();
                            if (trainingCertifications != null)
                            {
                                tempData.RelatedCourseId = trainingCertifications.Training.CourseId;
                                tempData.RelatedTrainingId = trainingCertifications.TrainingId;
                            }
                            else
                            {
                                tempData.RelatedTrainingId = null;
                                tempData.RelatedCourseId = null;
                            }
                        }
                        if (item.TrainingId != null)
                        {
                            if (!listTraining.Contains(item.TrainingId))
                            {
                                responseData.Add(tempData);
                            }
                            listTraining.Add(item.TrainingId);
                        }

                    }
                }

                return StatusCode(200, BaseResponse.ResponseOk(responseData.OrderByDescending(q => q.CreatedAt)));
            }
            // catch (Exception x)
            // {
            //     Console.WriteLine(x);
            //     return StatusCode(500, BaseResponse.Error(null, x));
            // }
        }

        public async Task<ActionResult<BaseResponse>> GetStatusAssesmentUser(int trainingId, string employeeId, bool isCoach)
        {
            if (!isCoach)
            {
                var teamMembers = await DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == employeeId).Where(x => x.Employee.IsDeleted != true).Select(x => x.EmployeeId).ToListAsync();
                //var response = await DB.LiveAssesmentSkillChecks.Where(x => teamMembers.Contains(x.EmployeeGUID)).Where(x => x.TrainingId == trainingId).Where(x => x.IsFinished).Where(x => x.IsScored).AnyAsync();
                var data = await DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainingId && teamMembers.Contains(x.EmployeeGUID)).Where(x => x.IsFinished == false || x.IsScored == false).ToListAsync();
                var response = true;
                if (data.Count() > 0)
                {
                    response = false;
                }
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            else
            {
                var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == employeeId).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                var teamMebers = new List<string>();
                if (coachData != null)
                {
                    if (coachData.Category != null)
                    {
                        if (coachData.Category.ToLower() == "dealer")
                        {
                            var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == employeeId).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                            if (queryDealer != null)
                            {
                                teamMebers = await this.DB.GetEmployeeListByCoach(trainingId.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else if (coachData.Category.ToLower() == "outlet")
                        {
                            var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == employeeId).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                            if (queryDealer != null)
                            {
                                teamMebers = await this.DB.GetEmployeeListByCoach(trainingId.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            teamMebers = await this.DB.GetEmployeeListByCoach(trainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                        }
                    }
                    else
                    {
                        teamMebers = await this.DB.GetEmployeeListByCoach(trainingId.ToString()).Select(x => x.EmployeeId).ToListAsync();
                    }
                }
                //var response = await DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainingId).Where(x => x.IsFinished).Where(x => x.IsScored).AnyAsync();
                //var data = await DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainingId && teamMebers.Contains(x.EmployeeGUID)).Where(x => x.IsFinished == false || x.IsScored == false).ToListAsync();
                var data = await DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainingId && teamMebers.Contains(x.EmployeeGUID)).Where(x => x.IsScored == false).ToListAsync();
                var response = true;
                if (data.Count() > 0)
                {
                    response = false;
                }
                return StatusCode(200, BaseResponse.ResponseOk(response));

            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }
        public async Task<MemoryStream> GenerateExcel(List<EmployeeByCoach> data, List<ScoringCourseSummaryAttemptResponse> headers, string trainingId)
        {
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            var headerIndex = 0;
            var columnAscii = 2;

            ws.Cell("A1").Value = "EmployeeName";
            ws.Cell("B1").Value = "Outlet";
            foreach (var item in headers)
            {
                if (item != null)
                {
                    columnAscii = columnAscii + 1;
                    var columnName = GetExcelColumnName(columnAscii);
                    ws.Cell(columnName + "1").Value = item.Name != null ? item.Name : "";

                }
                headerIndex++;
            }


            columnAscii = 2;
            var index = 2;
            foreach (var content in data)
            {
                ws.Cell("A" + index.ToString()).Value = content.EmployeeName;
                ws.Cell("B" + index.ToString()).Value = content.OutletName;
                columnAscii = 2;
                foreach (var datum in headers)
                {
                    var score = "";
                    if (datum.Category.ToLower() == "assesments" || datum.Category.ToLower() == "assessments")
                    {
                        var enumData = await DB.LiveAssesmentSkillChecks.Include(x => x.Assessments).Where(x => x.TrainingId.Value.ToString() == trainingId).Where(x => x.AssesmentGUID == datum.ParentId).FirstOrDefaultAsync();
                        if (enumData != null)
                        {
                            if (enumData.Assessments.EnumScoringMethod.ToLower() == "latest")
                            {
                                var reference = DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == content.EmployeeId).Where(x => x.AssesmentGUID == datum.ParentId).Where(x => x.SkillCheckGUID == datum.GUID).Where(x => x.TrainingId.Value.ToString() == trainingId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (reference != null && reference.IsScored == true)
                                {
                                    score = reference.Score.ToString();
                                }
                                else
                                {
                                    score = "";
                                }
                            }
                            else if (enumData.Assessments.EnumScoringMethod.ToLower() == "average")
                            {
                                var pembilang = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId.Value.ToString() == trainingId).Where(x => x.EmployeeGUID == content.EmployeeId).Where(x => x.AssesmentGUID == datum.ParentId).Where(x => x.SkillCheckGUID == datum.GUID).OrderByDescending(x => x.CreatedAt).Sum(x => x.Score);
                                var penyebut = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId.Value.ToString() == trainingId).Where(x => x.EmployeeGUID == content.EmployeeId).Where(x => x.AssesmentGUID == datum.ParentId).Where(x => x.SkillCheckGUID == datum.GUID).OrderByDescending(x => x.CreatedAt).Count();
                                var doublescore = pembilang / penyebut;
                                score = doublescore.ToString();
                                /*var reference = DB.LiveAssesmentSkillChecks.Where(x=> x.EmployeeGUID == content.EmployeeId).Where(x=> x.AssesmentGUID == datum.ParentId).Where(x=> x.SkillCheckGUID == datum.GUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                                if(reference != null){
                                   score = reference.Score;
                                } else {
                                   score = 0;
                                }*/

                            }
                            else if (enumData.Assessments.EnumScoringMethod.ToLower() == "highest")
                            {
                                var reference = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId.Value.ToString() == trainingId).Where(x => x.EmployeeGUID == content.EmployeeId).Where(x => x.AssesmentGUID == datum.ParentId).Where(x => x.SkillCheckGUID == datum.GUID).OrderByDescending(x => x.Score).FirstOrDefault();
                                if (reference != null && reference.IsScored == true)
                                {
                                    score = reference.Score.ToString();
                                }
                                else
                                {
                                    score = "";
                                }

                            }
                        }
                    }
                    else
                    {
                        //var moduleData = DB.TaskAnswerDetails.Include(x => x.TaskAnswer).Where(x => x.TaskAnswer.SetupModuleId.Value.ToString() == datum.ParentId).ToList();
                        //if (moduleData.Count() > 0)
                        //{
                        //    var scorenya = DB.FinalScores.Where(x => x.EmployeeId == content.EmployeeId).Where(x => x.ModuleId.ToString() == datum.GUID).Where(x => x.TrainingId.Value.ToString() == trainingId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        //    if (scorenya != null)
                        //    {
                        //        score = scorenya.FinalScore.ToString();
                        //    }
                        //}
                        //else
                        //{
                        //    var enrollLearningTimes = DB.EnrollLearningTimes.Include(zx => zx.EnrollLearning).Where(xz => xz.SetupModuleId.ToString() == datum.ParentId).Where(xz => xz.EnrollLearning.EmployeeId == content.EmployeeId).FirstOrDefault();
                        //    if (enrollLearningTimes == null)
                        //    {
                        //        score = "Belum Enroll";
                        //    }
                        //    else
                        //    {
                        //        if (enrollLearningTimes.EndTime != null)
                        //        {
                        //            score = "Selesai";
                        //        }
                        //        else
                        //        {
                        //            score = "Belum Menyelesaikan";
                        //        }
                        //    }
                        //}

                        var scorenya = DB.FinalScores.Where(x => x.EmployeeId == content.EmployeeId).Where(x => x.ModuleId.ToString() == datum.GUID).Where(x => x.TrainingId.Value.ToString() == trainingId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        if (scorenya != null)
                        {
                            score = scorenya.FinalScore.ToString();
                        }
                        else
                        {
                            var enrollLearningTimes = DB.EnrollLearningTimes.Include(zx => zx.EnrollLearning).Where(xz => xz.SetupModuleId.ToString() == datum.ParentId).Where(xz => xz.EnrollLearning.EmployeeId == content.EmployeeId).FirstOrDefault();
                            if (enrollLearningTimes == null)
                            {
                                score = "Belum Enroll";
                            }
                            else
                            {
                                if (enrollLearningTimes.EndTime != null)
                                {
                                    score = "Selesai";
                                }
                                else
                                {
                                    score = "Belum Menyelesaikan";
                                }
                            }
                        }

                    }
                    columnAscii = columnAscii + 1;
                    var columnName = GetExcelColumnName(columnAscii);

                    ws.Cell(columnName + index.ToString()).Value = score;
                }
                index++;


            }

            //table data

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            //var rangeHeader = ws.Range("A1:R1");
            //set table header border style
            //rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            //var dataRange = ws.Range("A2" + ":R" + (i - 1).ToString());
            //set data table border style
            //dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            //dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //set data table alignment
            //dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            //dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            //convert workbook into memorystream
            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }

        public async Task<MemoryStream> GenerateExcelSummary(List<ScoringCoachExcel> data, string trainingId)
        {
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header

            ws.Cell("A1").Value = "Employee Name";
            ws.Cell("B1").Value = "Outlet Name";
            ws.Cell("C1").Value = "Date";

            var index = 2;
            foreach (var content in data)
            {
                ws.Cell("A" + index.ToString()).Value = content.Name;
                ws.Cell("B" + index.ToString()).Value = content.OutletName;
                ws.Cell("C" + index.ToString()).Value = content.EnrollmentDate;
                index++;
            }

            //table data

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            //var rangeHeader = ws.Range("A1:R1");
            //set table header border style
            //rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            //var dataRange = ws.Range("A2" + ":R" + (i - 1).ToString());
            //set data table border style
            //dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            //dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //set data table alignment
            //dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            //dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            //convert workbook into memorystream
            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }

        public ScoreAndStatus GetTrackingProgressStatus(string EmployeeId, string AssesmentId, int? SetupModuleId, int trainigId)
        {
            ScoreAndStatus resp = new ScoreAndStatus();
            string returnValue = "1";
            string score = "N/A";
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
                                    returnValue = "3";
                                    break;
                                }
                                else
                                {
                                    returnValue = "2";
                                }
                                score = param.FinalScore.ToString();

                            }
                            else
                            {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (liveAssesment != null)
                                {
                                    returnValue = "1";
                                }
                                else
                                {
                                    returnValue = "1";
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
                                    returnValue = "3";
                                    break;

                                }
                                else
                                {
                                    returnValue = "2";
                                }
                                score = param.FinalScore.ToString();
                            }
                            else
                            {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (liveAssesment != null)
                                {
                                    returnValue = "1";
                                }
                                else
                                {
                                    returnValue = "1";
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
                                    returnValue = "3";
                                    break;
                                }
                                else
                                {
                                    returnValue = "2";
                                }
                                score = param.FinalScore.ToString();

                            }
                            else
                            {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                                if (liveAssesment != null)
                                {
                                    returnValue = "1";
                                }
                                else
                                {
                                    returnValue = "1";
                                }
                            }
                        }
                        else
                        {
                            var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                            if (liveAssesment != null)
                            {
                                returnValue = "1";
                            }
                            else
                            {
                                returnValue = "1";
                            }
                        }
                    }
                    else
                    {
                        var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).Where(x => x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        if (liveAssesment != null)
                        {
                            returnValue = "1";
                        }
                        else
                        {
                            returnValue = "1";
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
                        returnValue = "2";
                    }
                    else
                    {
                        returnValue = "3";
                    }
                    score = param.FinalScore.ToString();

                }
                else
                {
                    var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == trainigId).Where(x => x.AssesmentGUID == AssesmentId).Where(x => x.EmployeeGUID == EmployeeId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    if (liveAssesment != null)
                    {
                        returnValue = "1";
                    }
                    else
                    {
                        returnValue = "1";
                    }
                }
            }
            resp.Score = score;
            resp.Status = returnValue;
            return resp;
        }

        public ScoreAndStatus GetTrackingProgressStatusModule(string EmployeeId, int? ModuleId, int? SetupModuleId, int trainingId)
        {
            ScoreAndStatus resp = new ScoreAndStatus();
            string returnValue = "1";
            string score = "N/A";
            var param = DB.FinalScores.OrderByDescending(x => x.CreatedAt).Where(x => x.TrainingId == trainingId).Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.ModuleId == ModuleId).Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            if (param != null)
            {
                if (param.PassedStatus)
                {
                    returnValue = "2";
                }
                else
                {
                    returnValue = "3";
                }
                score = param.FinalScore.ToString();
            }
            else
            {
                //var containTask = DB.TaskAnswers.Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.TrainingId == trainingId).Count();
                var containTask = DB.TaskAnswers.Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.TrainingId == trainingId).FirstOrDefault();
                //var containTask = DB.TaskAnswers.Where(x => x.SetupModuleId == SetupModuleId && x.TrainingId == trainingId && x.EmployeeId == EmployeeId).Count();
                if (containTask != null)
                {
                    //var answerData = DB.TaskAnswerDetails.Include(x => x.TaskAnswer).ThenInclude(x => x.SetupModule).ThenInclude(x => x.EnrollLearningTimes).Where(x => x.TaskAnswer.SetupModule.ModuleId == ModuleId).Where(x => x.TaskAnswer.EmployeeId == EmployeeId).Where(x => x.ScorerGUID != null).FirstOrDefault();
                    //if (answerData != null)
                    //{
                    //    returnValue = "1";
                    //}
                    //else
                    //{
                    //    returnValue = "1";
                    //}

                    returnValue = "1";

                }
                else
                {
                    var alreadyTaken = DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Where(x => x.SetupModuleId == SetupModuleId).Where(x => x.EnrollLearning.TrainingId == trainingId).Where(x => x.EnrollLearning.EmployeeId == EmployeeId).FirstOrDefault();
                    if (alreadyTaken != null)
                    {
                        if (alreadyTaken.EndTime != null)
                        {
                            returnValue = "2";
                        }
                    }
                    else
                    {
                        returnValue = "1";
                    }
                }
            }
            resp.Score = score;
            resp.Status = returnValue;
            return resp;
        }



    }

}


