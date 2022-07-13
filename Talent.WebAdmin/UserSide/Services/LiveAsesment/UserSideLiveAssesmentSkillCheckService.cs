using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.UserSide.Models.LiveAssesment;
using System.Net.Http;
using Newtonsoft.Json;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideLiveAssesmentSkillCheckService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;
        private readonly PushNotificationService PNService;

        public UserSideLiveAssesmentSkillCheckService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService, PushNotificationService pushNotificationService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
            this.PNService = pushNotificationService;
        }


        public async Task<ActionResult<BaseResponse>> GetDetail(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {
                var query = DB.LiveAssesmentSkillChecks.AsQueryable();
                var response = await query.Where(x => x.GUID == request.GUID).Select(x => new ResponseLiveAssesmentSkillCheckModel
                {
                    GUID = x.GUID,
                    Answer = x.Answer,
                    Point = x.Point,
                    Score = x.Score,
                    SkillcheckGUID = x.SkillCheckGUID,
                    EmployeeGUID = x.EmployeeGUID,
                    Text = x.Text,
                    IsScored = x.IsScored,
                    Attempts = x.Attempts,
                    IsFinished = x.IsFinished,
                    IsDraft = x.IsDraft,


                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetListAssesmentSkillCheck(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {
                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var trainingQuery = (from sm in DB.SetupModules
                                     join t in DB.Trainings on sm.CourseId equals t.CourseId
                                     where t.IsDeleted == false && sm.IsDeleted == false && t.TrainingId == request.TrainingID
                                     select sm);

                //var query = DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).ThenInclude(x => x.SetupModule).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.LiveAssesmentSkillCheckNavigation).AsQueryable();
                var query = DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).ThenInclude(x => x.SetupModule).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).AsQueryable();

                if (request.SkillcheckGUID != "" && request.SkillcheckGUID != null)
                {
                    query = query.Where(x => x.SkillCheckGUID == request.SkillcheckGUID);
                }

                if (request.TrainingID != 0 && request.TrainingID != null)
                {
                    var listAssesmentId = new List<string>();
                    var listSetupModule = new List<int>();

                    //var trainingQuery = DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Where(x => x.EnrollLearning.TrainingId == request.TrainingID).Include(x => x.SetupModule).Where(x => x.SetupModuleId != 0).Select(x => x.SetupModule).ToList();
                    foreach (var item in trainingQuery)
                    {

                        if (item.AssesmentId != null)
                        {
                            if (!listAssesmentId.Contains(item.AssesmentId))
                            {
                                listAssesmentId.Add(item.AssesmentId);
                                listSetupModule.Add(item.SetupModuleId);
                            }

                        }
                    }
                    query = query.Where(x => listAssesmentId.Contains(x.AssesmentGUID)).Where(x => x.AssesmentsNavigator.SetupModule.Where(z => listSetupModule.Contains(z.SetupModuleId)).Any());
                }
                //var test = query.ToList();

                if (request.SuperiorGUID != "" && request.SuperiorGUID != null)
                {
                    var employeeIds = new List<string>();
                    if (!request.IsCoach)
                    {
                        employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorGUID).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                    }
                    else
                    {
                        var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorGUID).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                        if (coachData != null)
                        {
                            if (coachData.Category != null)
                            {
                                //if (coachData.Category.ToLower() == "dealer")
                                //{
                                //    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                                //    if (queryDealer != null)
                                //    {
                                //        var outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //    else
                                //    {
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //}
                                //else if (coachData.Category.ToLower() == "outlet")
                                //{
                                //    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.OutletId).FirstOrDefaultAsync();
                                //    if (queryDealer != null)
                                //    {
                                //        var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //    else
                                //    {
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //}
                                //else
                                //{
                                //    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                //}
                                if (coachData.Category.ToLower() == "dealer")
                                {
                                    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                    if (queryDealer != null)
                                    {
                                        employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                    }
                                }
                                else if (coachData.Category.ToLower() == "outlet")
                                {
                                    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                    if (queryDealer != null)
                                    {
                                        employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                    }
                                }
                                else
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString()).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                //employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }

                    }

                    var enrollemnt = DB.EnrollLearnings.Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => x.EnrollLearningId).ToList();
                    var listSetupModule = DB.EnrollLearningTimes.Where(x => enrollemnt.Contains(x.EnrollLearningId.Value)).Select(x => x.SetupModuleId).ToList();
                    var listAssesment = DB.SetupModules.Where(x => x.AssesmentId != null).Where(x => listSetupModule.Contains(x.SetupModuleId)).Select(x => x.AssesmentId).ToList();
                    query = query.Where(x => listAssesment.Contains(x.AssesmentGUID));

                }


                if (request.EmployeeGUID != "" && request.EmployeeGUID != null)
                {
                    query = query.Where(x => x.SkillChecksNavigator.LiveAssesmentSkillCheckNavigation.Any(z => z.EmployeeGUID == request.EmployeeGUID));
                }

                if (request.AssesmentGUID != "" && request.AssesmentGUID != null)
                {
                    query = query.Where(x => x.AssesmentGUID == request.AssesmentGUID);
                }

                if (request.SkillcheckGUID != "" && request.SkillcheckGUID != null)
                {
                    query = query.Where(x => x.SkillCheckGUID == request.SkillcheckGUID);
                }

                //query = query.OrderBy(x=> x.Order).Skip(request.Page).Take(request.Limt);
                query = query.OrderBy(x => x.Order);




                var response = await query.Select(x => new ResponseAssesmentSkillCheckList
                {
                    GUID = x.GUID,
                    Skillcheck = x.SkillChecksNavigator,
                    Orders = x.Order,
                    Assesment = new ResponseSubAssesmnets
                    {
                        GUID = x.AssesmentGUID,
                        EnumRemedialOption = x.AssesmentsNavigator.EnumRemedialOption,
                        MinimumScore = x.AssesmentsNavigator.MinimumScore,
                        EnumScoringMethod = x.AssesmentsNavigator.EnumScoringMethod,
                        IsByOption = x.AssesmentsNavigator.IsByOption,
                        LearningTime = x.AssesmentsNavigator.LearningTime,
                        Name = x.AssesmentsNavigator.Name,
                        RemedialLimit = x.AssesmentsNavigator.RemedialLimit,
                        TrainingTypeID = x.AssesmentsNavigator.TrainingTypeID,
                    },
                    Configurations = x.SkillChecksNavigator.SkillCheckScoreConfigsNavigation.ToList(),
                    AssesmentGUID = x.AssesmentGUID,
                    SkillCheckGUID = x.SkillCheckGUID,
                    IsScored = DB.LiveAssesmentSkillChecks.Where(y => y.TrainingId == request.TrainingID).Where(y => y.EmployeeGUID == request.EmployeeGUID).Where(y => y.IsScored).Where(y => y.SkillCheckGUID == x.SkillCheckGUID).Where(y => y.AssesmentGUID == x.AssesmentGUID).OrderByDescending(zx => zx.CreatedAt).FirstOrDefault() == null ? false : DB.LiveAssesmentSkillChecks.Where(y => y.TrainingId == request.TrainingID).Where(y => y.EmployeeGUID == request.EmployeeGUID).Where(y => y.IsScored).Where(y => y.SkillCheckGUID == x.SkillCheckGUID).Where(y => y.AssesmentGUID == x.AssesmentGUID).OrderByDescending(zx => zx.CreatedAt).FirstOrDefault().IsScored,
                    IsFinished = DB.LiveAssesmentSkillChecks.Where(y => y.TrainingId == request.TrainingID).Where(y => y.EmployeeGUID == request.EmployeeGUID).Where(y => y.IsScored).Where(y => y.SkillCheckGUID == x.SkillCheckGUID).Where(y => y.AssesmentGUID == x.AssesmentGUID).OrderByDescending(zx => zx.CreatedAt).FirstOrDefault() == null ? false : DB.LiveAssesmentSkillChecks.Where(y => y.TrainingId == request.TrainingID).Where(y => y.EmployeeGUID == request.EmployeeGUID).Where(y => y.IsFinished).Where(y => y.SkillCheckGUID == x.SkillCheckGUID).Where(y => y.AssesmentGUID == x.AssesmentGUID).OrderByDescending(zx => zx.CreatedAt).FirstOrDefault().IsFinished,
                }).ToListAsync();

                foreach (var item in response)
                {
                    var blobId = DB.Blobs.FirstOrDefault(x => x.BlobId.ToString().ToLower() == item.Skillcheck.MediaBlobId.ToLower());
                    if (blobId != null)
                    {
                        item.Skillcheck.MediaBlobId = await FileService.GetFileAsync(blobId.BlobId.ToString(), blobId.Mime);
                    }
                    // nahan looping json
                    item.Skillcheck.AssesmentsSkillCheckNavigation = null;
                    item.Skillcheck.LiveAssesmentSkillCheckNavigation = null;
                    item.Skillcheck.LiveAssesmentSkillCheckScoreNavigation = null;
                    item.Skillcheck.SkillCheckScoreConfigsNavigation = null;

                    foreach (var subItem in item.Configurations)
                    {
                        subItem.SkillChecksNavigator = null;
                    }
                }

                // var content = new JsonResult(response);
                //var data =  JsonConvert.SerializeObject(response);



                return StatusCode(200, BaseResponse.ResponseOk(response.OrderBy(x => x.Orders).ThenBy(x => x.Assesment.Name)));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<ActionResult<BaseResponse>> GetListAssesment(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {
                var employeeIds = new List<string>();

                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var trainingQuery = (from sm in DB.SetupModules
                                     join t in DB.Trainings on sm.CourseId equals t.CourseId
                                     where t.IsDeleted == false && sm.IsDeleted == false && t.TrainingId == request.TrainingID
                                     select sm);

                //var query = DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).ThenInclude(x => x.SetupModule).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.LiveAssesmentSkillCheckNavigation).AsQueryable();
                var query = DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).ThenInclude(x => x.SetupModule).Include(x => x.SkillChecksNavigator).AsQueryable();

                if (request.SkillcheckGUID != "" && request.SkillcheckGUID != null)
                {
                    query = query.Where(x => x.SkillCheckGUID == request.SkillcheckGUID);
                }

                //var tos = query.ToList();

                if (request.TrainingID != 0 && request.TrainingID != null)
                {
                    var listAssesmentId = new List<string>();
                    var listSetupModule = new List<int>();

                    //var trainingQuery = DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Where(x => x.EnrollLearning.TrainingId == request.TrainingID).Include(x => x.SetupModule).Where(x => x.SetupModuleId != 0).Select(x => x.SetupModule).ToList();
                    foreach (var item in trainingQuery)
                    {

                        if (item.AssesmentId != null)
                        {
                            if (!listAssesmentId.Contains(item.AssesmentId))
                            {
                                listAssesmentId.Add(item.AssesmentId);
                                listSetupModule.Add(item.SetupModuleId);
                            }

                        }
                    }

                    var listAssesmentSkillCheck = DB.AssesmentSkillChecks.Where(x => listAssesmentId.Contains(x.AssesmentGUID)).Where(x => x.AssesmentsNavigator.SetupModule.Where(z => listSetupModule.Contains(z.SetupModuleId)).Any()).ToList();

                    var listAssesementGUID = new List<string>();
                    var listAssesementSkillCheckGUID = new List<string>();
                    foreach (var item in listAssesmentSkillCheck)
                    {
                        if (!listAssesementGUID.Contains(item.AssesmentGUID))
                        {
                            listAssesementGUID.Add(item.AssesmentGUID);
                            listAssesementSkillCheckGUID.Add(item.GUID);
                        }
                    }
                    query = query.Where(x => listAssesementSkillCheckGUID.Contains(x.GUID));
                }


                //var test = query.ToList();

                if (request.SuperiorGUID != "" && request.SuperiorGUID != null)
                {
                    if (!request.IsCoach)
                    {
                        employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorGUID).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                    }
                    else
                    {
                        var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorGUID).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                        if (coachData != null)
                        {
                            if (coachData.Category != null)
                            {
                                //if (coachData.Category.ToLower() == "dealer")
                                //{
                                //    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                                //    if (queryDealer != null)
                                //    {
                                //        var outletIds = await this.DB.Outlets.Where(x => x.DealerId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //    else
                                //    {
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //}
                                //else if (coachData.Category.ToLower() == "outlet")
                                //{
                                //    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.OutletId).FirstOrDefaultAsync();
                                //    if (queryDealer != null)
                                //    {
                                //        var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer).Select(x => x.OutletId).ToListAsync();
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Where(x => outletIds.Contains(x.OutletId)).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //    else
                                //    {
                                //        employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                //    }
                                //}
                                //else
                                //{
                                //    employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                //}

                                if (coachData.Category.ToLower() == "dealer")
                                {
                                    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                    if (queryDealer != null)
                                    {
                                        employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                    }
                                }
                                else if (coachData.Category.ToLower() == "outlet")
                                {
                                    var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                    if (queryDealer != null)
                                    {
                                        employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                    }
                                }
                                else
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString()).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                //employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString()).Select(x => x.EmployeeId).ToListAsync();

                            }
                        }

                    }

                    //var enrollemnt = DB.EnrollLearnings.Where(x => employeeIds.Contains(x.EmployeeId) && x.TrainingId == request.TrainingID).Select(x => x.EnrollLearningId).ToList();
                    //var listSetupModule = DB.EnrollLearningTimes.Where(x => enrollemnt.Contains(x.EnrollLearningId.Value)).Select(x => x.SetupModuleId).ToList();
                    //var listAssesment = DB.SetupModules.Where(x => x.AssesmentId != null).Where(x => listSetupModule.Contains(x.SetupModuleId)).Select(x => x.AssesmentId).ToList();
                    //query = query.Where(x => listAssesment.Contains(x.AssesmentGUID));

                }

                //var test2 = query.ToList();

                if (request.EmployeeGUID != "" && request.EmployeeGUID != null)
                {
                    query = query.Where(x => x.SkillChecksNavigator.LiveAssesmentSkillCheckNavigation.Any(z => z.EmployeeGUID == request.EmployeeGUID));
                    employeeIds.Add(request.EmployeeGUID);

                }

                if (request.AssesmentGUID != "" && request.AssesmentGUID != null)
                {
                    query = query.Where(x => x.AssesmentGUID == request.AssesmentGUID);
                }

                query = query.OrderByDescending(x => x.CreatedAt);

                //var courseId = await DB.Trainings.Include(pz => pz.Course).Where(xx => xx.TrainingId == request.TrainingID).OrderByDescending(zz => zz.TrainingId).FirstOrDefaultAsync();


                var response = await query.Select(x => new ResponseAssesmentSkillCheckList
                {
                    Orders = DB.SetupModules.Where(zz => zz.CourseId.Value == trainingQuery.Select(c => c.CourseId).FirstOrDefault()).Where(zz => zz.AssesmentId == x.AssesmentGUID).FirstOrDefault().Order,
                    GUID = x.GUID,
                    Assesment = new ResponseSubAssesmnets
                    {
                        GUID = x.AssesmentGUID,
                        EnumRemedialOption = x.AssesmentsNavigator.EnumRemedialOption,
                        MinimumScore = x.AssesmentsNavigator.MinimumScore,
                        EnumScoringMethod = x.AssesmentsNavigator.EnumScoringMethod,
                        IsByOption = x.AssesmentsNavigator.IsByOption,
                        LearningTime = x.AssesmentsNavigator.LearningTime,
                        Name = x.AssesmentsNavigator.Name,
                        RemedialLimit = x.AssesmentsNavigator.RemedialLimit,
                        TrainingTypeID = x.AssesmentsNavigator.TrainingTypeID,
                    },
                    AssesmentGUID = x.AssesmentGUID,

                }).ToListAsync();

                foreach (var item in response)
                {
                    //  var latestAttempts= DB.LiveAssesmentSkillChecks.Where(x=> x.AssesmentGUID == item.AssesmentGUID).OrderByDescending(x=> x.Attempts).FirstOrDefault().Attempts;
                    //item.IsScored = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == request.TrainingID).Where(x => x.AssesmentGUID == item.AssesmentGUID).Where(x => employeeIds.Contains(x.EmployeeGUID)).Where(x => x.IsScored == false).Any();
                    //item.IsFinished = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == request.TrainingID).Where(x => x.AssesmentGUID == item.AssesmentGUID).Where(x => employeeIds.Contains(x.EmployeeGUID)).Where(x => x.IsFinished == false).Any();

                    var dataLiveAssessment = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == request.TrainingID).Where(x => x.AssesmentGUID == item.AssesmentGUID).Where(x => employeeIds.Contains(x.EmployeeGUID));
                    item.IsScored = dataLiveAssessment.Where(x => x.IsScored == false).Any();
                    item.IsFinished = dataLiveAssessment.Where(x => x.IsFinished == false).Any();
                    if (!item.IsFinished)
                    {
                        item.IsFinished = true;
                    }
                    else
                    {
                        item.IsFinished = false;
                    }
                    if (!item.IsScored)
                    {
                        item.IsScored = true;
                    }
                    else
                    {
                        item.IsScored = false;
                    }
                    //item.IsDoing = DB.LiveAssesmentSkillChecks.Where(x => x.TrainingId == request.TrainingID).Where(x => x.AssesmentGUID == item.AssesmentGUID).Where(x => employeeIds.Contains(x.EmployeeGUID)).Any();
                    item.IsDoing = dataLiveAssessment.Any();
                }



                //var content = new JsonResult(response);
                //var data = JsonConvert.SerializeObject(response);



                return StatusCode(200, BaseResponse.ResponseOk(response.OrderBy(x => x.Orders)));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }




        public async Task<ActionResult<BaseResponse>> GetAssesmentSkillCheckDetail(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {

                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var query = DB.AssesmentSkillChecks.Include(x => x.AssesmentsNavigator).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).FirstOrDefault(x => x.GUID == request.GUID);

                var response = new ResponseAssesmentSkillCheckList
                {
                    GUID = query.GUID,
                    // Skillcheck = query.SkillChecksNavigator,
                    // Assesment = query.AssesmentsNavigator,
                    Order = query.Order,
                    AssesmentGUID = query.AssesmentGUID,
                    SkillCheckGUID = query.SkillCheckGUID,
                    IsFinished = DB.LiveAssesmentSkillChecks.Where(y => y.EmployeeGUID == request.EmployeeGUID).Where(y => y.Attempts == request.Attempts).Where(y => y.IsFinished).Where(y => y.SkillCheckGUID == query.SkillCheckGUID).Any()
                };

                var blobId = DB.Blobs.FirstOrDefault(x => x.BlobId.ToString().ToLower() == response.Skillcheck.MediaBlobId.ToLower());
                if (blobId != null)
                {
                    response.Skillcheck.MediaBlobId = await FileService.GetFileAsync(blobId.BlobId.ToString(), blobId.Mime);
                }

                // response.Skillcheck.SkillCheckScoreConfigsNavigation = DB.SkillChecksScoreConfigs.Where(x => x.SkillCheckGUID == response.SkillCheckGUID).ToList();

                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetListTeamMemberSkillCheck(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {

                if (request.Limt == 0)
                {
                    request.Limt = 100;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    var q = this.DB.Teams.AsQueryable();

                    if (!string.IsNullOrEmpty(request.SuperiorGUID))
                    {
                        q = q.Where(x => x.TeamLeaderId == request.SuperiorGUID);
                    }

                    var team = q.FirstOrDefault();

                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorGUID).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorGUID).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString(), queryDealer.DealerId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else if (coachData.Category.ToLower() == "outlet")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => new { x.Outlet.DealerId, x.Outlet.OutletId }).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString(), queryDealer.DealerId, queryDealer.OutletId).Select(x => x.EmployeeId).ToListAsync();
                                }
                            }
                            else
                            {
                                employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString()).Select(x => x.EmployeeId).ToListAsync();
                            }
                        }
                        else
                        {
                            //employeeIds = await this.DB.Employees.Where(x => x.IsDeleted == false).Select(x => x.EmployeeId).ToListAsync();
                            employeeIds = await this.DB.GetEmployeeListByCoach(request.TrainingID.ToString()).Select(x => x.EmployeeId).ToListAsync();

                        }
                    }

                }

                var query = DB.LiveAssesmentSkillChecks.Include(x => x.EmployeeNavigator).Where(x => employeeIds.Contains(x.EmployeeGUID));



                if (request.TrainingID != null && request.TrainingID != 0)
                {
                    var enrollLearnings = DB.EnrollLearnings.Where(x => x.TrainingId == request.TrainingID).Select(x => x.EnrollLearningId).ToList();
                    var enrollLearningTimes = DB.EnrollLearningTimes.Where(x => enrollLearnings.Contains(x.EnrollLearningId.Value)).Select(x => x.SetupModuleId).ToList();
                    var setupModules = DB.SetupModules.Where(x => enrollLearningTimes.Contains(x.SetupModuleId)).Where(x => x.AssesmentId != null).Select(x => x.AssesmentId).ToList();
                    query = query.Where(x => setupModules.Contains(x.AssesmentGUID));
                }

                query = query.Where(x => x.TrainingId == request.TrainingID).OrderByDescending(x => x.CreatedAt);

                if (request.AssesmentGUID != "" && request.AssesmentGUID != null)
                {
                    query = query.Where(x => x.AssesmentGUID == request.AssesmentGUID);
                }
                if (request.SkillcheckGUID != "" && request.SkillcheckGUID != null)
                {
                    query = query.Where(x => x.SkillCheckGUID == request.SkillcheckGUID);
                }

                //var queryWaiting = query.Where(x => !x.IsScored || !x.IsFinished || x.IsDraft).Select(x => x.EmployeeNavigator).Skip(request.Page).Take(request.Limt);
                //var queryScore = query.Where(x => x.IsScored).Where(x => !x.IsDraft).Where(x => x.IsFinished).Select(x => x.EmployeeNavigator).Skip(request.Page).Take(request.Limt);

                var waiting = query.Where(x => x.IsScored == false && x.IsFinished == true && x.IsDraft == false).Select(x => x.EmployeeGUID).Distinct().ToList();
                var finish = query.Where(x => x.IsScored == true && x.IsFinished == true && x.IsDraft == false).Select(x => x.EmployeeGUID).Distinct().ToList();

                var queryWaiting = DB.Employees.Where(x => waiting.Contains(x.EmployeeId)).Skip(request.Page).Take(request.Limt);
                var queryScore = DB.Employees.Where(x => finish.Contains(x.EmployeeId)).Skip(request.Page).Take(request.Limt);

                var responseScore = await queryScore.Select(x => new ResponseAssesmentTeamMemberList
                {
                    EmployeeGUID = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                    EmployeePhotos = x.BlobId.ToString(),
                }).Distinct().ToListAsync();

                var responseWait = await queryWaiting.Select(x => new ResponseAssesmentTeamMemberList
                {
                    EmployeeGUID = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                    EmployeePhotos = x.BlobId.ToString(),
                }).Distinct().ToListAsync();

                responseScore = responseScore.Where(zo => !responseWait.Select(x => x.EmployeeGUID).ToList().Contains(zo.EmployeeGUID)).ToList();

                foreach (var item in responseScore)
                {
                    if (item.EmployeePhotos != null)
                    {
                        var blobId = DB.Blobs.FirstOrDefault(x => x.BlobId.ToString().ToLower() == item.EmployeePhotos.ToLower());
                        if (blobId != null)
                        {
                            item.EmployeePhotos = await FileService.GetFileAsync(blobId.BlobId.ToString(), blobId.Mime);

                        }
                    }
                }

                foreach (var item in responseWait)
                {
                    if (item.EmployeePhotos != null)
                    {
                        var blobId = DB.Blobs.FirstOrDefault(x => x.BlobId.ToString().ToLower() == item.EmployeePhotos.ToLower());
                        if (blobId != null)
                        {
                            item.EmployeePhotos = await FileService.GetFileAsync(blobId.BlobId.ToString(), blobId.Mime);

                        }
                    }
                }

                var jsonResponse = new ResponseAssesmentTeamMember();
                jsonResponse.WaitingForScore = responseWait;
                jsonResponse.Finish = responseScore;

                return StatusCode(200, BaseResponse.ResponseOk(jsonResponse));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> GetSkillCheckListByUser(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {

                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;


                var enrolled = DB.EnrollLearnings.Where(x => x.EmployeeId == request.EmployeeGUID).Include(x => x.SetupModule).ThenInclude(x => x.Assesment).Where(x => x.SetupModule.AssesmentId != null).Where(x => x.SetupModuleId != null);

                if (request.TrainingID != null && request.TrainingID != 0)
                {
                    enrolled = enrolled.Where(x => x.TrainingId == request.TrainingID);
                }

                var data = enrolled.Select(x => x.SetupModule.Assesment).ToList();

                var listAssesmentId = new List<string>();
                foreach (var item in data)
                {
                    listAssesmentId.Add(item.GUID);
                }

                var skillCheck = await DB.AssesmentSkillChecks.Where(x => listAssesmentId.Contains(x.AssesmentGUID)).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).Select(x => x.SkillChecksNavigator).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(skillCheck));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> GetSkillCheckListBySuperior(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {

                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorGUID).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorGUID).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
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
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.OutletId).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer).Select(x => x.OutletId).ToListAsync();
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

                var enrollemnt = DB.EnrollLearnings.Where(x => employeeIds.Contains(x.EmployeeId)).Select(x => x.EnrollLearningId).ToList();
                var listSetupModule = DB.EnrollLearningTimes.Where(x => enrollemnt.Contains(x.EnrollLearningId.Value)).Select(x => x.SetupModuleId).ToList();
                var listAssesment = DB.SetupModules.Where(x => x.AssesmentId != null).Where(x => listSetupModule.Contains(x.SetupModuleId)).Select(x => x.AssesmentId).ToList();



                var skillCheck = await DB.AssesmentSkillChecks.Where(x => listAssesment.Contains(x.AssesmentGUID)).Include(x => x.SkillChecksNavigator).ThenInclude(x => x.SkillCheckScoreConfigsNavigation).Select(x => x.SkillChecksNavigator).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(skillCheck));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }



        public async Task<ActionResult<BaseResponse>> GetListTeamMemberSkillCheckScore(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {

                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var employeeIds = new List<string>();
                if (!request.IsCoach)
                {
                    employeeIds = DB.TeamDetails.Include(x => x.Employee).Include(x => x.Team).Where(x => x.Team.TeamLeaderId == request.SuperiorGUID).Where(x => x.Employee.IsDeleted == false).Select(x => x.EmployeeId).ToList();
                }
                else
                {
                    var coachData = await this.DB.Coaches.Where(x => x.EmployeeId == request.SuperiorGUID).Where(x => x.CoachIsActive).FirstOrDefaultAsync();
                    if (coachData != null)
                    {
                        if (coachData.Category != null)
                        {
                            if (coachData.Category.ToLower() == "dealer")
                            {
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
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
                                var queryDealer = await this.DB.Employees.Where(x => x.IsDeleted == false).Include(x => x.Outlet).Where(x => x.EmployeeId == request.SuperiorGUID).Select(x => x.Outlet.OutletId).FirstOrDefaultAsync();
                                if (queryDealer != null)
                                {
                                    var outletIds = await this.DB.Outlets.Where(x => x.OutletId == queryDealer).Select(x => x.OutletId).ToListAsync();
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


                var query = DB.LiveAssesmentSkillChecks.Include(x => x.EmployeeNavigator).Include(x => x.SkillCheckNavigator).Where(x => employeeIds.Contains(x.EmployeeGUID)).Select(x => x.EmployeeNavigator);

                query = query.Skip(request.Page).Take(request.Limt);

                var response = await query.Select(x => new ResponseAssesmentTeamMemberScoreList
                {
                    EmployeeGUID = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                    EmployeePhotos = x.BlobId.ToString(),
                }).ToListAsync();



                foreach (var item in response)
                {
                    var blobId = DB.Blobs.FirstOrDefault(x => x.BlobId.ToString().ToLower() == item.EmployeePhotos.ToLower());
                    if (blobId != null)
                    {
                        item.EmployeePhotos = await FileService.GetFileAsync(blobId.BlobId.ToString(), blobId.Mime);

                    }
                    item.SkillCheck = DB.LiveAssesmentSkillChecks.Include(x => x.SkillCheckNavigator).FirstOrDefault(x => x.EmployeeGUID == item.EmployeeGUID);
                }

                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }






        public async Task<ActionResult<BaseResponse>> GetList(ParamLiveAssesmentSkillCheckModel request)
        {
            try
            {

                if (request.Limt == 0)
                {
                    request.Limt = 10;
                }

                if (request.Page == 0)
                {
                    request.Page = 1;
                }

                request.Page = (request.Page - 1) * request.Limt;

                var query = DB.LiveAssesmentSkillChecks.AsQueryable();
                if (request.SkillcheckGUID != "" && request.SkillcheckGUID != null)
                {
                    query = query.Where(x => x.SkillCheckGUID == request.SkillcheckGUID);
                }

                if (request.EmployeeGUID != "" && request.EmployeeGUID != null)
                {
                    query = query.Where(x => x.EmployeeGUID == request.EmployeeGUID);
                }

                query = query.Skip(request.Page).Take(request.Limt);

                var response = await query.Select(x => new ResponseLiveAssesmentSkillCheckModel
                {
                    GUID = x.GUID,
                    Answer = x.Answer,
                    Point = x.Point,
                    Score = x.Score,
                    SkillcheckGUID = x.SkillCheckGUID,
                    EmployeeGUID = x.EmployeeGUID,
                    Text = x.Text,
                    IsScored = x.IsScored,
                    Attempts = x.Attempts,
                    IsDraft = x.IsDraft,


                    IsFinished = x.Attempts == request.Attempts ? x.IsFinished : false,
                }).ToListAsync();


                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Create(RequestLiveAssesmentSkillCheckModel request)
        {
            try
            {
                //var existingData = await DB.LiveAssesmentSkillChecks.Include(x => x.Assessments).ThenInclude(x => x.SetupModule)
                //var existingData = await DB.LiveAssesmentSkillChecks
                //    .Where(x => x.EmployeeGUID == request.EmployeeGUID)
                //    .Where(x => x.SkillCheckGUID == request.SkillcheckGUID)
                //    .Where(x => x.AssesmentGUID == request.AssesmentGUID)
                //    .Where(x => x.TrainingId == request.TrainingID)
                //    .Where(x => x.Attempts == request.Attempts) // ini nanti di komen
                //    .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();


                //if (existingData != null)
                //{
                    //if (existingData.IsFinished == true && existingData.IsScored == true)
                    //{

                    //}
                    //else
                    //{


                    //var msg = new Message
                    //{
                    //    En = "Your latest attempt is not finished / scored",
                    //    Id = "Percobaan terakhir belum selesai / dinilai"
                    //};
                    //return StatusCode(400, BaseResponse.BadRequest(request, msg));


                    //}
                //}
                //else
                //{
                    //var data = new LiveAssesmentSkillChecks();
                    //data.GUID = Guid.NewGuid().ToString();
                    //data.CreatedAt = DateTime.UtcNow.AddHours(7);
                    //data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                    //data.EmployeeGUID = request.EmployeeGUID;
                    //data.Point = request.Point;
                    //data.Score = request.Score;
                    //data.SkillCheckGUID = request.SkillcheckGUID;
                    //data.Text = request.Text;
                    //data.IsScored = false;
                    //data.IsFinished = request.IsFinished;
                    //data.IsDraft = request.IsDraft;
                    //data.Attempts = request.Attempts;
                    //data.AssesmentGUID = request.AssesmentGUID;
                    //data.TrainingId = request.TrainingID;

                    //DB.LiveAssesmentSkillChecks.Add(data);

                    ////await DB.SaveChangesAsync();

                    //var elt = await DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).Where(x => x.EnrollLearning.EmployeeId == request.EmployeeGUID).Where(x => x.SetupModule.AssesmentId == request.AssesmentGUID).Where(x => x.EnrollLearning.TrainingId == request.TrainingID).OrderByDescending(x => x.SetupModule.SetupModuleId).FirstOrDefaultAsync();
                    //if (elt != null)
                    //{
                    //    elt.EndTime = DateTime.UtcNow.AddHours(7);
                    //}

                    //DB.EnrollLearningTimes.Update(elt);
                    //await DB.SaveChangesAsync();
                    //return StatusCode(200, BaseResponse.ResponseOk());
                //}

                var data = new LiveAssesmentSkillChecks();
                data.GUID = Guid.NewGuid().ToString();
                data.CreatedAt = DateTime.UtcNow.AddHours(7);
                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.EmployeeGUID = request.EmployeeGUID;
                data.Point = request.Point;
                data.Score = request.Score;
                data.SkillCheckGUID = request.SkillcheckGUID;
                data.Text = request.Text;
                data.IsScored = false;
                data.IsFinished = request.IsFinished;
                data.IsDraft = request.IsDraft;
                data.Attempts = request.Attempts;
                data.AssesmentGUID = request.AssesmentGUID;
                data.TrainingId = request.TrainingID;

                DB.LiveAssesmentSkillChecks.Add(data);

                await DB.SaveChangesAsync();

                var elt = await DB.EnrollLearningTimes.Include(x => x.EnrollLearning).Include(x => x.SetupModule).Where(x => x.EnrollLearning.EmployeeId == request.EmployeeGUID).Where(x => x.SetupModule.AssesmentId == request.AssesmentGUID).Where(x => x.EnrollLearning.TrainingId == request.TrainingID).OrderByDescending(x => x.SetupModule.SetupModuleId).FirstOrDefaultAsync();
                if (elt != null)
                {
                    elt.EndTime = DateTime.UtcNow.AddHours(7);
                }

                DB.EnrollLearningTimes.Update(elt);
                await DB.SaveChangesAsync();
                return StatusCode(200, BaseResponse.ResponseOk());

            }
            catch (Exception x)
            {
                if(x.InnerException.Message.Contains("UNIQUE"))
                {
                    return StatusCode(200, BaseResponse.ResponseOk());
                }
                else
                {
                    return StatusCode(500, BaseResponse.Error(null, x));
                }
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(RequestLiveAssesmentSkillCheckModel request)
        {
            try
            {
                var data = await DB.LiveAssesmentSkillChecks.FirstOrDefaultAsync(x => x.GUID == request.GUID);
                if (data == null)
                {
                    return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
                }

                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.EmployeeGUID = request.EmployeeGUID;
                data.Point = request.Point;
                data.Score = request.Score;
                data.SkillCheckGUID = request.SkillcheckGUID;
                data.Text = request.Text;
                data.IsScored = request.IsScored;
                // data.Attempts = request.Attempts;
                data.IsFinished = request.IsFinished;
                if (data.IsScored)
                {
                    data.IsDraft = false;
                }
                else
                {
                    data.IsDraft = request.IsDraft;
                }
                data.AssesmentGUID = request.AssesmentGUID;
                data.ScorerGUID = request.ScorerGUID;


                await DB.SaveChangesAsync();

                var config = await DB.SetupModules.FirstOrDefaultAsync(x => x.AssesmentId == request.AssesmentGUID && x.IsDeleted == false);
                if (config.IsByOption)
                {
                    var checkIsCompleteData = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).OrderBy(x => x.SkillCheckGUID).ThenBy(n => n.Attempts).AsQueryable().ToListAsync();
                    var checkIsnotCompleteData = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).Where(x => x.IsScored == true && x.IsFinished == true && x.IsDraft == false).OrderBy(x => x.SkillCheckGUID).ThenBy(n => n.Attempts).AsQueryable().ToListAsync();
                    if (checkIsCompleteData.Count == checkIsnotCompleteData.Count)
                    {
                        await this.CalculateScoreByOption(request, config);
                        //push notif di pisah

                        //var GetAssesmentPassedStatus = await DB.FinalScores.Where(x => x.AssesmentId == request.AssesmentGUID && x.SkillCheckGuid == request.SkillcheckGUID).ToListAsync();
                        //foreach (var item in GetAssesmentPassedStatus)
                        //{
                        //    if (item.PassedStatus == false)
                        //    {
                        //        //push notif
                        //        var getAssesmentName = await this.DB.Assesments.Where(Q => Q.GUID == request.AssesmentGUID).Select(Q => Q.Name).FirstOrDefaultAsync();
                        //        var getCourseName = await this.DB.SetupModules.Include(Q => Q.Course).Where(Q => Q.CourseId == config.CourseId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();
                        //        var getTrainingId = await (from tr in DB.TrainingModuleMappings
                        //                                   where tr.SetupModuleId == config.SetupModuleId
                        //                                   select tr.TrainingId).FirstOrDefaultAsync();
                        //        var groupPositionList = new List<string>();
                        //        var manPowerPositionList = new List<string>();
                        //        var PushNotificationMyLearnings = new VMPushNotificationAdd();
                        //        var PushNotificationDataMyTools = new VMPushNotificationDataAdd();
                        //        PushNotificationMyLearnings.Title = "Remedial";
                        //        PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course "+ getCourseName +"-"+ getAssesmentName + " , Silahkan melakukan Remedial di Assesment "+ getAssesmentName + " tersebut. Terima Kasih";
                        //        PushNotificationMyLearnings.SenderId = request.ScorerGUID;
                        //        PushNotificationMyLearnings.IsPublished = true;
                        //        PushNotificationMyLearnings.GroupPositions = groupPositionList;
                        //        PushNotificationMyLearnings.ManPowerPosition = manPowerPositionList;

                        //        PushNotificationMyLearnings.SpecifiedEmployeeId = new List<string>{
                        //            request.EmployeeGUID
                        //        };

                        //        PushNotificationDataMyTools.Category = "Remedial";
                        //        PushNotificationDataMyTools.DataID = getTrainingId;
                        //        PushNotificationDataMyTools.DataSecondId = config.CourseId;

                        //        await this.PNService.CreatePushNotificationRemedialScores(PushNotificationMyLearnings, PushNotificationDataMyTools);
                        //    }
                        //}
                    }
                    else
                    {
                        return StatusCode(200, BaseResponse.ResponseOk());
                    }
                }
                else
                {
                    var getEnrollLearning = await (from el in DB.EnrollLearnings
                                                   where el.TrainingId == request.TrainingID && el.EmployeeId == request.EmployeeGUID
                                                   select el).FirstOrDefaultAsync();

                    var jumlahSkillCheck = DB.AssesmentSkillChecks.Where(x => x.AssesmentGUID == request.AssesmentGUID).Count();

                    var checkIsCompleteData = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).OrderBy(x => x.SkillCheckGUID).ThenBy(n => n.Attempts).AsQueryable().ToListAsync();
                    var checkIsnotCompleteData = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).Where(x => x.IsScored == true && x.IsFinished == true && x.IsDraft == false).OrderBy(x => x.SkillCheckGUID).ThenBy(n => n.Attempts).AsQueryable().ToListAsync();
                    if (checkIsCompleteData.Count == checkIsnotCompleteData.Count && checkIsnotCompleteData.Select(x => x.SkillCheckGUID).Distinct().Count() == jumlahSkillCheck)
                    {
                        await this.CalculateScoreByTotalSC(request, config);
                        //push notif di pisah
                        var GetAssesmentPassedStatus = await DB.FinalScores.Where(x => x.AssesmentId == request.AssesmentGUID && x.SetupModuleId == config.SetupModuleId && x.EmployeeId == request.EmployeeGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
                        if (GetAssesmentPassedStatus != null)
                        {
                            if (GetAssesmentPassedStatus.PassedStatus == false)
                            {
                                //push notif
                                var getAssesmentName = await this.DB.Assesments.Where(Q => Q.GUID == request.AssesmentGUID).Select(Q => Q.Name).FirstOrDefaultAsync();
                                var getCourseName = await this.DB.SetupModules.Include(Q => Q.Course).Where(Q => Q.CourseId == config.CourseId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();


                                var groupPositionList = new List<string>();
                                var manPowerPositionList = new List<string>();
                                var PushNotificationMyLearnings = new VMPushNotificationAdd();
                                var PushNotificationDataMyTools = new VMPushNotificationDataAdd();
                                PushNotificationMyLearnings.Title = "Remedial";
                                PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getAssesmentName + " , Silahkan melakukan Remedial di Assesment " + getAssesmentName + " tersebut. Terima Kasih";
                                if (config.EnumRemedialOption.Trim() != "No Need")
                                {
                                    if (config.EnumRemedialOption.Trim() == "Limit")
                                    {
                                        var jmlAttempt = new List<float>();
                                        jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => Q.AssesmentGUID == request.AssesmentGUID && Q.EmployeeGUID == request.EmployeeGUID && Q.TrainingId == request.TrainingID).Select(Q => Q.Attempts).ToList();
                                        if (jmlAttempt.Count > 0)
                                        {
                                            if (jmlAttempt.Max() == config.RemedialLimit)
                                            {
                                                PushNotificationMyLearnings.Title = "Tidak Lulus";
                                                PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getAssesmentName + ". Terima Kasih";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PushNotificationMyLearnings.Title = "Tidak Lulus";
                                    PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getAssesmentName + ". Terima Kasih";
                                }
                                PushNotificationMyLearnings.SenderId = request.ScorerGUID;
                                PushNotificationMyLearnings.IsPublished = true;
                                PushNotificationMyLearnings.GroupPositions = groupPositionList;
                                PushNotificationMyLearnings.ManPowerPosition = manPowerPositionList;

                                PushNotificationMyLearnings.SpecifiedEmployeeId = new List<string>{
                                    request.EmployeeGUID.ToLower()
                                };

                                PushNotificationDataMyTools.Category = "Remedial";
                                PushNotificationDataMyTools.DataID = request.TrainingID;
                                PushNotificationDataMyTools.DataSecondId = config.CourseId;

                                await this.PNService.CreatePushNotificationRemedialScores(PushNotificationMyLearnings, PushNotificationDataMyTools);

                                getEnrollLearning.IsPassed = null;
                                await DB.SaveChangesAsync();
                            }
                            else
                            {
                                //getEnrollLearning.IsPassed = true;
                                await DB.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        return StatusCode(200, BaseResponse.ResponseOk());
                    }
                }

                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            try
            {
                var data = await DB.LiveAssesmentSkillChecks.FirstOrDefaultAsync(x => x.GUID == request);
                if (data == null)
                {
                    return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
                }

                data.DeletedAt = DateTime.UtcNow.AddHours(7);


                await DB.SaveChangesAsync();
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> CleansingData(int trainingId)
        {
            try
            {
                var query = DB.GetListDoubleDataByTraining(trainingId).AsQueryable();

                var data = await query.OrderBy(x => x.skillcheckguid).ToListAsync();

                foreach(var datum in data)
                {
                    var tempData = DB.GetAssesmentSkillCheckDouble(trainingId, datum.employeeguid, datum.skillcheckguid, datum.attempts).OrderByDescending(x => x.Attempts).ThenByDescending(x => x.CreatedAt).AsQueryable();
                    if(tempData.Where(x => x.IsScored == true).Any())
                    {
                        var dataGUID = await tempData.Where(x => x.IsScored == false).Select(x => x.GUID).ToListAsync();
                        // delete
                        foreach(var request in dataGUID)
                        {
                            var delAssObj = await DB.LiveAssesmentSkillChecks.FirstOrDefaultAsync(x => x.GUID == request);
                            DB.LiveAssesmentSkillChecks.Remove(delAssObj);
                            await DB.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var countData = await tempData.ToListAsync();
                        var getData = countData.Take(countData.Count() - 1).Select(x => x.GUID).ToList();
                        //delete
                        foreach (var request in getData)
                        {
                            var delAssObj = await DB.LiveAssesmentSkillChecks.FirstOrDefaultAsync(x => x.GUID == request);
                            DB.LiveAssesmentSkillChecks.Remove(delAssObj);
                            await DB.SaveChangesAsync();
                        }
                    }
                }
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch(Exception ex)
            {
                return StatusCode(500, BaseResponse.Error(null, ex));
            }
        }

        public async Task<ActionResult<BaseResponse>> CleansingDataModule()
        {
            try
            {
                var query = DB.GetListDoubleDataModuleByTaskAnswerId().AsQueryable();

                var data = await query.OrderBy(x => x.taskid).ToListAsync();
                foreach (var datum in data)
                {
                    var tempData = DB.GetModuleCheckDouble(datum.taskanswerid, datum.taskid, datum.attempts).OrderByDescending(x => x.Attempts).ThenByDescending(x => x.CreatedAt).AsQueryable();
                    var countData = await tempData.ToListAsync();
                    var getData = countData.Take(countData.Count() - 1).Select(x => x.TaskAnswerDetailId).ToList();
                    //delete
                    foreach (var request in getData)
                    {
                        var delAssObj = await DB.TaskAnswerDetails.FirstOrDefaultAsync(x => x.TaskAnswerDetailId == request);
                        DB.TaskAnswerDetails.Remove(delAssObj);
                        await DB.SaveChangesAsync();
                    }
                }
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse.Error(null, ex));
            }
        }


        public async Task<ActionResult<BaseResponse>> CalculateScoreByOption(RequestLiveAssesmentSkillCheckModel request, SetupModules config)
        {
            try
            {
                var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();
                var finalScore = 0.0;

                var getSetupModuleId = await (from sm in DB.SetupModules
                                              where sm.AssesmentId == request.AssesmentGUID && sm.IsDeleted == false
                                              select sm.SetupModuleId).FirstOrDefaultAsync();

                var getCourseId = await (from cr in DB.Trainings
                                         where cr.TrainingId == request.TrainingID && cr.IsDeleted == false
                                         select cr.CourseId).FirstOrDefaultAsync();

                var getModuleId = await (from md in DB.SetupModules
                                         where md.SetupModuleId == getSetupModuleId
                                         select md.ModuleId).FirstOrDefaultAsync();

                var getEnrollLearning = await (from el in DB.EnrollLearnings
                                               where el.TrainingId == request.TrainingID && el.EmployeeId == request.EmployeeGUID
                                               select el).FirstOrDefaultAsync();

                var jumlahSkillCheck = DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).Select(x => x.SkillCheckGUID).Distinct().ToList();
                var checkLulus = new List<bool>();
                for (int i = 0; i < jumlahSkillCheck.Count(); i++)
                {
                    var skillCheckMinScore = await (from sm in DB.SkillChecks
                                                    where sm.GUID == jumlahSkillCheck[i]
                                                    select sm.MinimumScore).FirstOrDefaultAsync();
                    var nilaiPSkillCheck = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.SkillCheckGUID == jumlahSkillCheck[i] && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).OrderBy(x => x.Attempts).AsQueryable().ToListAsync();
                    var jumlahAttempt = nilaiPSkillCheck.OrderByDescending(x => x.Attempts).Select(x => x.Attempts).First();
                    var nilaiTempObj = new LiveAssesmentScoreTempVM();
                    foreach (var itm in nilaiPSkillCheck)
                    {
                        if (config.EnumRemedialOption.Trim() != "No Need")
                        {
                            nilaiTempObj.SkillcheckGUID = itm.SkillCheckGUID;
                            if (itm.Attempts == jumlahAttempt)
                            {
                                nilaiTempObj.AvgScore = (nilaiTempObj.AvgScore + itm.Score) / jumlahAttempt;
                                if (nilaiTempObj.HighestScore < itm.Score)
                                {
                                    nilaiTempObj.HighestScore = itm.Score;
                                }
                                nilaiTempObj.LatestScore = itm.Score;

                            }
                            else
                            {
                                nilaiTempObj.AvgScore += itm.Score;
                                if (nilaiTempObj.HighestScore < itm.Score)
                                {
                                    nilaiTempObj.HighestScore = itm.Score;
                                }
                                nilaiTempObj.LatestScore = itm.Score;
                            }
                        }
                        else
                        {
                            nilaiTempObj.SkillcheckGUID = itm.SkillCheckGUID;
                            nilaiTempObj.AvgScore = itm.Score;
                            nilaiTempObj.HighestScore = itm.Score;
                            nilaiTempObj.LatestScore = itm.Score;
                        }
                    }
                    var passedStatus = false;
                    if (config.EnumScoringMethod == "Average")
                    {
                        if (nilaiTempObj.AvgScore >= skillCheckMinScore)
                        {
                            passedStatus = true;
                            finalScore = nilaiTempObj.AvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                SkillCheckGuid = jumlahSkillCheck[i],
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);

                        }
                        else
                        {
                            finalScore = nilaiTempObj.AvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                SkillCheckGuid = jumlahSkillCheck[i],
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);

                        }
                        checkLulus.Add(passedStatus);
                    }
                    else if (config.EnumScoringMethod == "Highest")
                    {
                        if (nilaiTempObj.HighestScore >= skillCheckMinScore)
                        {
                            passedStatus = true;
                            finalScore = nilaiTempObj.HighestScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                SkillCheckGuid = jumlahSkillCheck[i],
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                        }
                        else
                        {
                            finalScore = nilaiTempObj.HighestScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                SkillCheckGuid = jumlahSkillCheck[i],
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                        }
                        checkLulus.Add(passedStatus);
                    }
                    else
                    {
                        if (nilaiTempObj.LatestScore >= skillCheckMinScore)
                        {
                            passedStatus = true;
                            finalScore = nilaiTempObj.LatestScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                SkillCheckGuid = jumlahSkillCheck[i],
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                        }
                        else
                        {
                            finalScore = nilaiTempObj.LatestScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                SkillCheckGuid = jumlahSkillCheck[i],
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                        }
                        checkLulus.Add(passedStatus);
                    }
                    await DB.SaveChangesAsync();
                }
                if (checkLulus.Contains(false))
                {
                    var getAssesmentName = await this.DB.Assesments.Where(Q => Q.GUID == request.AssesmentGUID).Select(Q => Q.Name).FirstOrDefaultAsync();
                    var getCourseName = await this.DB.SetupModules.Include(Q => Q.Course).Where(Q => Q.CourseId == config.CourseId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();
                    var groupPositionList = new List<string>();
                    var manPowerPositionList = new List<string>();
                    var PushNotificationMyLearnings = new VMPushNotificationAdd();
                    var PushNotificationDataMyTools = new VMPushNotificationDataAdd();
                    PushNotificationMyLearnings.Title = "Remedial";
                    PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getAssesmentName + " , Silahkan melakukan Remedial di Assesment " + getAssesmentName + " tersebut. Terima Kasih";
                    if (config.EnumRemedialOption.Trim() != "No Need")
                    {
                        if (config.EnumRemedialOption.Trim() == "Limit")
                        {
                            var jmlAttempt = new List<float>();
                            jmlAttempt = this.DB.LiveAssesmentSkillChecks.Where(Q => Q.AssesmentGUID == request.AssesmentGUID && Q.EmployeeGUID == request.EmployeeGUID && Q.TrainingId == request.TrainingID).Select(Q => Q.Attempts).ToList();
                            if (jmlAttempt.Count > 0)
                            {
                                if (jmlAttempt.Max() == config.RemedialLimit)
                                {
                                    PushNotificationMyLearnings.Title = "Tidak Lulus";
                                    PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getAssesmentName + ". Terima Kasih";
                                }
                            }
                        }
                    }
                    else
                    {
                        PushNotificationMyLearnings.Title = "Tidak Lulus";
                        PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getAssesmentName + ". Terima Kasih";
                    }
                    PushNotificationMyLearnings.SenderId = request.ScorerGUID;
                    PushNotificationMyLearnings.IsPublished = true;
                    PushNotificationMyLearnings.GroupPositions = groupPositionList;
                    PushNotificationMyLearnings.ManPowerPosition = manPowerPositionList;

                    PushNotificationMyLearnings.SpecifiedEmployeeId = new List<string>{
                        request.EmployeeGUID.ToLower()
                    };

                    PushNotificationDataMyTools.Category = "Remedial";
                    PushNotificationDataMyTools.DataID = request.TrainingID;
                    PushNotificationDataMyTools.DataSecondId = config.CourseId;

                    await this.PNService.CreatePushNotificationRemedialScores(PushNotificationMyLearnings, PushNotificationDataMyTools);

                    getEnrollLearning.IsPassed = null;
                    await DB.SaveChangesAsync();
                }
                else
                {
                    //getEnrollLearning.IsPassed = true;
                    await DB.SaveChangesAsync();
                }
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<ActionResult<BaseResponse>> CalculateScoreByTotalSC(RequestLiveAssesmentSkillCheckModel request, SetupModules config)
        {
            try
            {
                var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();
                var finalScore = 0.0;
                var passedStatus = false;
                var nilaiPSkillCheck = await DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).OrderBy(x => x.Attempts).ThenBy(n => n.SkillCheckGUID).AsQueryable().ToListAsync();
                var jumlahAttempt = nilaiPSkillCheck.OrderByDescending(x => x.Attempts).Select(x => x.Attempts).First();
                //var jumlahSkillCheck = DB.LiveAssesmentSkillChecks.Where(x => x.EmployeeGUID == request.EmployeeGUID && x.AssesmentGUID == request.AssesmentGUID && x.TrainingId == request.TrainingID).Select(x => x.SkillCheckGUID).Distinct().Count();
                var jumlahSkillCheck = DB.AssesmentSkillChecks.Where(x => x.AssesmentGUID == request.AssesmentGUID).Count();

                var getSetupModuleId = await (from sm in DB.SetupModules
                                              where sm.AssesmentId == request.AssesmentGUID && sm.IsDeleted == false
                                              select sm.SetupModuleId).FirstOrDefaultAsync();

                var getCourseId = await (from cr in DB.Trainings
                                         where cr.TrainingId == request.TrainingID && cr.IsDeleted == false
                                         select cr.CourseId).FirstOrDefaultAsync();

                var getModuleId = await (from md in DB.SetupModules
                                         where md.SetupModuleId == getSetupModuleId
                                         select md.ModuleId).FirstOrDefaultAsync();

                var listNilaiTempObj = new List<LiveAssesmentScoreTempEmpVM>();
                if (config.EnumRemedialOption.Trim() != "No Need")
                {
                    for (int i = 1; i <= jumlahAttempt; i++)
                    {
                        if (nilaiPSkillCheck.Where(x => x.Attempts == i).Select(x => x.SkillCheckGUID).Distinct().Count() == jumlahSkillCheck)
                        {
                            var nilaiTempObj = new LiveAssesmentScoreTempEmpVM();
                            nilaiTempObj.EmployeeGUID = request.EmployeeGUID;
                            nilaiTempObj.Score = nilaiPSkillCheck.Where(x => x.Attempts == i).Sum(x => x.Score) / jumlahSkillCheck;

                            listNilaiTempObj.Add(nilaiTempObj);

                            //add to tb final score
                        }
                    }
                }
                else
                {
                    if (nilaiPSkillCheck.Select(x => x.SkillCheckGUID).Distinct().Count() == jumlahSkillCheck)
                    {
                        var nilaiTempObj = new LiveAssesmentScoreTempEmpVM();
                        nilaiTempObj.EmployeeGUID = request.EmployeeGUID;
                        nilaiTempObj.Score = nilaiPSkillCheck.Where(x => x.Attempts == 1).Sum(x => x.Score) / jumlahSkillCheck;

                        listNilaiTempObj.Add(nilaiTempObj);

                        //add to tb final score
                    }
                }

                if (listNilaiTempObj.Count() > 0)
                {
                    if (config.EnumScoringMethod == "Average")
                    {
                        var finalAvgScore = 0.0;
                        foreach (var itm in listNilaiTempObj)
                        {
                            finalAvgScore += itm.Score;
                        }
                        finalAvgScore = finalAvgScore / listNilaiTempObj.Count();
                        if (finalAvgScore >= config.MinimumScore)
                        {
                            //lulus
                            //passing status kelulusan
                            //push notif kelulusan

                            passedStatus = true;
                            finalScore = finalAvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                            await DB.SaveChangesAsync();

                            return StatusCode(200, BaseResponse.ResponseOk());
                        }
                        else
                        {
                            //tidak lulus
                            //passing status kelulusan
                            //push notif kelulusan

                            finalScore = finalAvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                            await DB.SaveChangesAsync();

                            return StatusCode(200, BaseResponse.ResponseOk());
                        }
                    }
                    else if (config.EnumScoringMethod == "Highest")
                    {
                        var finalAvgScore = 0.0;
                        finalAvgScore = listNilaiTempObj.Max(x => x.Score);
                        if (finalAvgScore >= config.MinimumScore)
                        {
                            //lulus
                            //passing status kelulusan
                            //push notif kelulusan

                            passedStatus = true;
                            finalScore = finalAvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                            await DB.SaveChangesAsync();

                            return StatusCode(200, BaseResponse.ResponseOk());
                        }
                        else
                        {
                            //tidak lulus
                            //passing status kelulusan
                            //push notif kelulusan

                            finalScore = finalAvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                            await DB.SaveChangesAsync();

                            return StatusCode(200, BaseResponse.ResponseOk());
                        }
                    }
                    else
                    {
                        var finalAvgScore = 0.0;
                        finalAvgScore = listNilaiTempObj.Select(x => x.Score).Last();
                        if (finalAvgScore >= config.MinimumScore)
                        {
                            //lulus
                            //passing status kelulusan
                            //push notif kelulusan

                            passedStatus = true;
                            finalScore = finalAvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                            await DB.SaveChangesAsync();

                            return StatusCode(200, BaseResponse.ResponseOk());
                        }
                        else
                        {
                            //tidak lulus
                            //passing status kelulusan
                            //push notif kelulusan

                            finalScore = finalAvgScore;
                            //push notif kelulusan

                            var insertData = new FinalScores
                            {
                                EmployeeId = request.EmployeeGUID,
                                AssesmentId = request.AssesmentGUID,
                                SetupModuleId = getSetupModuleId,
                                TrainingId = request.TrainingID,
                                ModuleId = getModuleId,
                                CourseId = getCourseId,
                                FinalScore = (float)finalScore,
                                PassedStatus = passedStatus,
                                CreatedAt = dateNow,
                            };

                            DB.FinalScores.Add(insertData);
                            await DB.SaveChangesAsync();

                            return StatusCode(200, BaseResponse.ResponseOk());
                        }
                    }
                }
                else
                {
                    return StatusCode(200, BaseResponse.ResponseOk());
                }

            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> IsAllowNPS(ParamIsAllowNPSModel request)
        {
            try
            {
                var status = false;
                var trainingData = await DB.Trainings.Where(x => x.TrainingId.ToString() == request.TrainingId).FirstOrDefaultAsync();
                var setupModuleData = await DB.SetupModules.Where(x => x.AssesmentId == request.AssesmentId).Where(x => x.CourseId == trainingData.CourseId).OrderByDescending(x => x.SetupModuleId).FirstOrDefaultAsync();
                var comparer = await DB.SetupModules.Where(x => x.CourseId == trainingData.CourseId).OrderByDescending(x => x.Order).FirstOrDefaultAsync();

                if (comparer.Order == setupModuleData.Order)
                {
                    var countAssesment = await DB.AssesmentSkillChecks.Where(x => x.AssesmentGUID == request.AssesmentId).CountAsync();
                    var countLiveAssesment = await DB.LiveAssesmentSkillChecks.Where(x => x.Attempts == 1).Where(x => x.AssesmentGUID == request.AssesmentId).Where(x => x.TrainingId.Value.ToString() == request.TrainingId).Where(x => x.EmployeeGUID == request.EmployeeId).CountAsync();
                    if (countAssesment == countLiveAssesment)
                    {
                        status = true;
                    }
                }

                var response = new
                {
                    Status = status,
                };
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }



    }


}
