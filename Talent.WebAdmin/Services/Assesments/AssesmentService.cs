using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class AssesmentService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public AssesmentService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }


        public async Task<ActionResult<BaseResponse>> GetDetail(ParamAssesmentModel request)
        {
            try
            {
                var query = DB.Assesments.AsQueryable();
                if (!string.IsNullOrEmpty(request.GUID))
                {
                    query = query.Where(x => x.GUID == request.GUID);
                }

                if (!string.IsNullOrEmpty(request.Query))
                {
                    query = query.Where(x => request.Query.Contains(x.Name));
                }

                var response = await query.Include(x => x.AssesmentSkillChecksNavigation).ThenInclude(x => x.SkillChecksNavigator).OrderBy(x => x.CreatedAt).Skip(request.Page).Take(request.Limit).Select(x => new ResponseAssesmentModel
                {
                    GUID = x.GUID,
                    Name = x.Name,
                    EnumRemedialOption = x.EnumRemedialOption,
                    EnumScoringMethod = x.EnumScoringMethod,
                    LearningTime = x.LearningTime,
                    EnumTrainingType = x.TrainingTypeID,
                    IsByOption = x.IsByOption,
                    RemedialLimit = x.RemedialLimit,
                    SkillCheck = x.AssesmentSkillChecksNavigation.ToList()
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetList(ParamAssesmentModel request)
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

                var query = DB.Assesments.AsQueryable();
                if (!string.IsNullOrEmpty(request.GUID))
                {
                    query = query.Where(x => x.GUID == request.GUID);
                }

                if (!string.IsNullOrEmpty(request.Query))
                {
                    query = query.Where(x => request.Query.Contains(x.Name));
                }

                var response = await query.Include(x => x.AssesmentSkillChecksNavigation).ThenInclude(x => x.SkillChecksNavigator).OrderBy(x => x.CreatedAt).Skip(request.Page).Take(request.Limit).Select(x => new ResponseAssesmentModel
                {
                    GUID = x.GUID,
                    Name = x.Name,
                    EnumRemedialOption = x.EnumRemedialOption,
                    EnumScoringMethod = x.EnumScoringMethod,
                    LearningTime = x.LearningTime,
                    EnumTrainingType = x.TrainingTypeID,
                    IsByOption = x.IsByOption,
                    RemedialLimit = x.RemedialLimit,
                    SkillCheck = x.AssesmentSkillChecksNavigation.ToList()
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Create(RequestAssesmentModel request)
        {
            try
            {
                var data = new Assesments();
                var assesmentSkillCheckDatas = new List<AssesmentSkillChecks>();
                data.GUID = Guid.NewGuid().ToString();
                data.CreatedAt = DateTime.UtcNow.AddHours(7);
                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.LearningTime = request.LearningTime;
                data.Name = request.Name;
                data.TrainingTypeID = request.EnumTrainingType;
                data.EnumScoringMethod = request.EnumScoringMethod;
                data.EnumRemedialOption = request.EnumRemedialOption;
                data.IsByOption = request.IsByOption;
                data.RemedialLimit = request.RemedialLimit;

                DB.Assesments.Add(data);

                foreach (var item in request.AssesmentSkillChecks)
                {
                    var assesmentSkillCheckData = new AssesmentSkillChecks
                    {
                        SkillCheckGUID = item.SkillCheckGUID,
                        AssesmentGUID = data.GUID,
                        GUID = Guid.NewGuid().ToString(),
                        Order = item.Order,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                    };
                    assesmentSkillCheckDatas.Add(assesmentSkillCheckData);
                }

                await DB.AssesmentSkillChecks.AddRangeAsync(assesmentSkillCheckDatas);

                await DB.SaveChangesAsync();

                request.GUID = data.GUID;

                return StatusCode(200, BaseResponse.ResponseOk(request));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(RequestAssesmentModel request)
        {
            try
            {
                var data = await DB.Assesments.Include(x => x.AssesmentSkillChecksNavigation).FirstOrDefaultAsync(x => x.GUID == request.GUID);
                if (data == null)
                {
                    return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
                }

                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.LearningTime = request.LearningTime;
                data.Name = request.Name;
                data.TrainingTypeID = request.EnumTrainingType;
                data.EnumScoringMethod = request.EnumScoringMethod;
                data.EnumRemedialOption = request.EnumRemedialOption;
                data.IsByOption = request.IsByOption;
                data.RemedialLimit = request.RemedialLimit;

                // Delete children
                foreach (var existingChild in data.AssesmentSkillChecksNavigation.ToList())
                {
                    if (data.AssesmentSkillChecksNavigation.Any(c => c.SkillCheckGUID == existingChild.SkillCheckGUID))
                        DB.AssesmentSkillChecks.Remove(existingChild);
                }

                // Update and Insert children
                foreach (var childModel in request.AssesmentSkillChecks)
                {
                    var existingChild = data.AssesmentSkillChecksNavigation
                        .Where(c => c.GUID == childModel.SkillCheckGUID)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        DB.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new AssesmentSkillChecks
                        {
                            SkillCheckGUID = childModel.SkillCheckGUID,
                            AssesmentGUID = data.GUID,
                            GUID = Guid.NewGuid().ToString(),
                            Order = childModel.Order,
                            CreatedAt = DateTime.UtcNow.AddHours(7),
                        };
                        data.AssesmentSkillChecksNavigation.Add(newChild);
                    }
                }

                await DB.SaveChangesAsync();
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
                var data = await DB.Assesments.FirstOrDefaultAsync(x => x.GUID == request);
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

        public async Task<ActionResult<BaseResponse>> GetTrackingProgress(RequestTrackingProgressModel request) {
            try
            {
                var responseData = new List<ResponseTrackingProgressModel>();

                var query = this.DB.Teams.AsQueryable();

                if (!string.IsNullOrEmpty(request.SuperiorGUID))
                {
                    query = query.Where(x => x.TeamLeaderId == request.SuperiorGUID);
                }


                var team = query.FirstOrDefault();

                var employeeIds = await this.DB.TeamDetails.Where(x => x.TeamId == team.TeamId).Select(x=> x.EmployeeId).ToListAsync();

                var positionMappings = await this.DB.EmployeePositionMappings.Include(x => x.Employee).ThenInclude(x => x.Outlet).ThenInclude(x => x.Dealer).Include(x => x.Position).Where(x => employeeIds.Contains(x.EmployeeId)).ToListAsync();

                var data = positionMappings.Select(x => new ResponseTrackingProgressModel
                {
                    EmployeeName = x.Employee.EmployeeName,
                    OutletName= x.Employee.Outlet.Name,
                    DealerName = x.Employee.Outlet.Dealer.DealerName,
                    EmployeeGUID = x.Employee.EmployeeId,
                    PositionName = x.Position.PositionName
                }).ToList();



                responseData = data;

                foreach (var item in responseData)
                {
                    var assesmentDetail = DB.LiveAssesmentSkillChecks.Include(x=> x.Assessments).Where(x => x.EmployeeGUID == item.EmployeeGUID).Select(x => new ResponseTrackingProgressModelDetail
                    {
                        ModuleId = x.AssesmentGUID,
                        ModuleName = x.Assessments.Name,
                        Score = x.Score,
                    }).ToList();
                    if (assesmentDetail.Count() != 0)
                    {
                        item.Assesments = assesmentDetail;
                    }


                    var moduleIds = DB.Tasks.Where(x => x.QuestionTypeId != 11).Select(x => x.ModuleId).ToList();

                    var moduleDetail = DB.TaskAnswers.Where(x => x.SetupModuleId != null).Where(x => x.EmployeeId == item.EmployeeGUID).Include(x=> x.SetupModule).ThenInclude(x=> x.Module).Where(x=> moduleIds.Contains(x.SetupModule.ModuleId.Value)).Include(x => x.TaskAnswerDetails).Where(x=> x.SetupModule.CourseId != null).Where(x=> x.SetupModule.CourseId == request.CourseId).Select(x => new ResponseTrackingProgressModelDetail
                    {
                        ModuleId= x.SetupModule.Module.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module.ModuleName,
                        CourseId = x.SetupModule.CourseId.Value,
                        Score = (float)x.TaskAnswerDetails.FirstOrDefault().Score,
                    }).ToList();

                    

                    if (moduleDetail.Count() != 0)
                    {
                        item.Module = moduleDetail;
                    }

                    var moduleIdUploads = DB.Tasks.Where(x => x.QuestionTypeId == 11).Select(x => x.ModuleId).ToList();

                    var uploadDataDetail = DB.TaskAnswers.Where(x => x.SetupModuleId != null).Where(x => x.EmployeeId == item.EmployeeGUID).Include(x => x.SetupModule).ThenInclude(x => x.Module).Where(x=> moduleIdUploads.Contains(x.SetupModule.Module.ModuleId)).Include(x => x.TaskAnswerDetails).Where(x => x.SetupModule.CourseId != null).Where(x => x.SetupModule.CourseId == request.CourseId).Select(x => new ResponseTrackingProgressModelDetail
                    {
                        ModuleId = x.SetupModule.Module.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module.ModuleName,
                        CourseId = x.SetupModule.CourseId.Value,
                        Done = x.SetupModule.Module.Tasks.Where(z=> z.QuestionTypeId == 11).Any(),
                    }).ToList();

                    if (uploadDataDetail.Count() != 0)
                    {
                        item.FileUpload = uploadDataDetail;
                    }



                }

                return StatusCode(200, BaseResponse.ResponseOk(responseData));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> GetTrackingProgressUser(RequestTableOfContent request)
        {
            try
            {
                double preDataPercentage =0;
                double duringDataPercentage=0;
                double postDataPercentage=0;

                bool preNA = false;
                bool duringNA = false;
                bool postNA =false;


                var responseData = new List<ResposeTableOfContent>();

                var response = new ResponseTableOfContent(); 
                var elData =  new List<EnrollLearningTimes>();
                var tempElData = await this.DB.EnrollLearningTimes.Include(x=>x.EnrollLearning).Include(x=> x.SetupModule).ThenInclude(x=> x.Assesment).Include(x=> x.SetupModule).ThenInclude(x=> x.Module).Where(x => x.EnrollLearning.EmployeeId == request.EmployeeID).Where(x=> x.EnrollLearning.TrainingId == request.TrainingId).OrderBy(x=> x.SetupModule.SetupModuleId).ToListAsync();
                foreach (var datum in tempElData){
                    if(!elData.Select(x=> x.SetupModuleId).ToList().Contains(datum.SetupModuleId)){
                        elData.Add(datum);
                    }
                }
                    var preData = elData.Where(x => x.SetupModule.TrainingTypeId == 1).OrderBy(x=> x.SetupModule.SetupModuleId).Select(x=> new SubResponseTableOfContent {
                        ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                        ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                    Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(request.EmployeeID, x.SetupModule.ModuleId, x.SetupModuleId, request.TrainingId.Value) : GetTrackingProgressStatus(request.EmployeeID, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, request.TrainingId.Value),
                    }).ToList();

                var duringData = elData.Where(x => x.SetupModule.TrainingTypeId == 2).OrderBy(x=> x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                {
                    ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                    ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                    Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(request.EmployeeID, x.SetupModule.ModuleId, x.SetupModuleId, request.TrainingId.Value) : GetTrackingProgressStatus(request.EmployeeID, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, request.TrainingId.Value),
                }).ToList();

                var postData = elData.Where(x => x.SetupModule.TrainingTypeId == 3).OrderBy(x=> x.SetupModule.SetupModuleId).Select(x => new SubResponseTableOfContent
                {
                    ModuleId = x.SetupModule.ModuleId == null ? x.SetupModule.AssesmentId : x.SetupModule.ModuleId.ToString(),
                    ModuleName = x.SetupModule.Module == null ? x.SetupModule.Assesment.Name : x.SetupModule.Module.ModuleName,
                    Status = x.SetupModule.AssesmentId == null ? GetTrackingProgressStatusModule(request.EmployeeID, x.SetupModule.ModuleId, x.SetupModuleId, request.TrainingId.Value) : GetTrackingProgressStatus(request.EmployeeID, x.SetupModule.AssesmentId, x.SetupModule.SetupModuleId, request.TrainingId.Value),
                }).ToList();
                
                if (preData.Count()>0){
                    preDataPercentage = ((double)preData.Where(x=> x.Status == 3).Count() / (double)preData.Count()) * 100;
                } else {
                    preNA = true;
                }
                if (duringData.Count()>0){
                     duringDataPercentage = ((double)duringData.Where(x=> x.Status == 3).Count() / (double)duringData.Count()) * 100;
                }else {
                    duringNA= true;
                }
                if (postData.Count()>0){
                    postDataPercentage = ((double)postData.Where(x=> x.Status == 3).Count() / (double)postData.Count()) * 100;
                }else {
                    postNA = true;
                }

                double totalDataPercentage = (((double)postData.Where(x=> x.Status == 3).Count() + (double)duringData.Where(x=> x.Status == 3).Count()+ (double)preData.Where(x=> x.Status == 3).Count() )  / (preData.Count()+duringData.Count()+postData.Count())) * 100;

                var enrollTraining = await DB.EnrollLearnings.Where(x => x.EnrollLearningId.Equals(tempElData.FirstOrDefault().EnrollLearningId)).FirstOrDefaultAsync();

                if (totalDataPercentage == 100)
                {
                    //update passed status enroll learining training yang sudah 100%
                   
                    if(enrollTraining != null)
                    {
                        enrollTraining.IsPassed = true;
                        DB.EnrollLearnings.Update(enrollTraining);
                        await DB.SaveChangesAsync();
                    }
                }
                else
                {
                    //update not passed status enroll learining training yang < 100%
                    if (enrollTraining != null)
                    {
                        enrollTraining.IsPassed = null;
                        DB.EnrollLearnings.Update(enrollTraining);
                        await DB.SaveChangesAsync();
                    }

                }

                var courses = await DB.Trainings.Where(x=> x.TrainingId == request.TrainingId).FirstOrDefaultAsync();
                //var setupModules = DB.SetupModules.Where(x=> x.CourseId == courses.CourseId);

                preNA = DB.SetupModules.Where(x=> x.CourseId == courses.CourseId).Where(x=> x.TrainingTypeId == 1).Any();
                duringNA = DB.SetupModules.Where(x=> x.CourseId == courses.CourseId).Where(x=> x.TrainingTypeId == 2).Any();
                postNA =DB.SetupModules.Where(x=> x.CourseId == courses.CourseId).Where(x=> x.TrainingTypeId == 3).Any();

                responseData.Add(new ResposeTableOfContent {
                    CategoryName = "Pre",
                    Percentage = Math.Round(preDataPercentage, 2),
                    Detail = preData,
                    IsNA = !preNA,
                });

                responseData.Add(new ResposeTableOfContent
                {
                    CategoryName = "During",
                    Percentage = Math.Round(duringDataPercentage, 2),
                    Detail = duringData,
                    IsNA = !duringNA,
                });

                responseData.Add(new ResposeTableOfContent
                {
                    CategoryName = "Post",
                    Percentage = Math.Round(postDataPercentage, 2),
                    Detail = postData,
                    IsNA = !postNA,
                });

                response.Data = responseData;
                response.Percentage = Math.Round(totalDataPercentage, 2);
                
                var trainingCertifications = await DB.TrainingCertifications.Where(x=> x.TrainingId == request.TrainingId).FirstOrDefaultAsync();
                if(trainingCertifications != null){
                    response.RelatedTrainingId = trainingCertifications.RelatedTrainingId;
                } else {
                    response.RelatedTrainingId = null;
                }
                
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        
        public async Task<ActionResult<BaseResponse>> ExportTrackingProgressUser(RequestTableOfContent request)
        {
            try
            {
                var response= new ExportLearningProgreess();
                var setupModules = await DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.EmployeeId == request.EmployeeID).Select(x=> x.SetupModule).ToListAsync();
                
                foreach (var setupModule in setupModules)
                {
                    var modules = new List<ExportLearningProgreessDetails>();
                    var assesments = new List<ExportLearningProgreessDetails>();

                    if(setupModule.AssesmentId != null){
                        var assesment = DB.Assesments.Where(x=> x.GUID == setupModule.AssesmentId).Select(uy=> new ExportLearningProgreessDetails{
                            ModuleName = uy.Name,
                        });

                        assesments.AddRange(assesment);
                    }

                    if(setupModule.ModuleId != null){
                        var module = DB.Modules.Where(x=> x.ModuleId == setupModule.ModuleId).Select(uy=> new ExportLearningProgreessDetails{
                            ModuleName = uy.ModuleName,
                        });

                        modules.AddRange(module);
                    }
                    
                }
                
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public int  GetTrackingProgressStatus(string EmployeeId, string AssesmentId, int? SetupModuleId, int trainigId){
            int  returnValue = 1;
            var enumByOption = DB.Assesments.Where(x=> x.GUID == AssesmentId).FirstOrDefault().IsByOption;
            var skillCheckList = DB.LiveAssesmentSkillChecks.Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.TrainingId == trainigId).Select(x=>x.SkillCheckGUID).ToList();
            if (enumByOption){
                foreach(var skillCheckGUID in skillCheckList){
                    var flag =  DB.FinalScores.Include(x=> x.Assesment).Where(x=> x.SkillCheckGuid == skillCheckGUID).Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentId ==  AssesmentId).Where(x=> x.SetupModuleId == SetupModuleId).Where(x=> x.EmployeeId == EmployeeId).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                    if (flag != null){
                        if(flag.Assesment.EnumScoringMethod.ToLower() == "latest"){
                            var param = DB.FinalScores.Include(x=> x.Assesment).Where(x=> x.SkillCheckGuid == skillCheckGUID).Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentId ==  AssesmentId).Where(x=> x.EmployeeId == EmployeeId).Where(x=> x.SetupModuleId == SetupModuleId).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                            if(param !=null){
                                if (!param.PassedStatus){
                                    returnValue = 4;
                                    break;
                                }
                                else {
                                    returnValue = 3;
                                }
                            } else {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.EmployeeGUID == EmployeeId).Where(x=> x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                                if(liveAssesment != null){
                                    returnValue = 2;
                                } else {
                                    returnValue = 1;
                                }
                            } 
                        } else if (flag.Assesment.EnumScoringMethod.ToLower() == "highest"){
                            var param = DB.FinalScores.Include(x=> x.Assesment).Where(x=> x.SkillCheckGuid == skillCheckGUID).Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentId ==  AssesmentId).Where(x=> x.EmployeeId == EmployeeId).Where(x=> x.SetupModuleId == SetupModuleId).OrderByDescending(x=> x.FinalScore).FirstOrDefault();
                            if(param !=null){
                                if (!param.PassedStatus){
                                    returnValue = 4;
                                    break;

                                }
                                else {
                                    returnValue = 3;
                                }
                            }else {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.EmployeeGUID == EmployeeId).Where(x=> x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                                if(liveAssesment != null){
                                    returnValue = 2;
                                } else {
                                    returnValue = 1;
                                }
                            } 
                        } else if (flag.Assesment.EnumScoringMethod.ToLower() == "average"){
                            var param = DB.FinalScores.Include(x=> x.Assesment).Where(x=> x.SkillCheckGuid == skillCheckGUID).Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentId ==  AssesmentId).Where(x=> x.EmployeeId == EmployeeId).Where(x=> x.SetupModuleId == SetupModuleId).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                            if(param !=null){
                                if (!param.PassedStatus){
                                    returnValue = 4;
                                    break;
                                }
                                else {
                                    returnValue = 3;
                                }
                            } else {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.EmployeeGUID == EmployeeId).Where(x=> x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                                if(liveAssesment != null){
                                    returnValue = 2;
                                } else {
                                    returnValue = 1;
                                }
                            } 
                        } else {
                               var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.EmployeeGUID == EmployeeId).Where(x=> x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                                if(liveAssesment != null){
                                    returnValue = 2;
                                } else {
                                    returnValue = 1;
                                }
                        }
                    } else {
                                var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.EmployeeGUID == EmployeeId).Where(x=> x.SkillCheckGUID == skillCheckGUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                                if(liveAssesment != null){
                                    returnValue = 2;
                                } else {
                                    returnValue = 1;
                                }
                            } 
                }
            } else {
                    var param =  DB.FinalScores.OrderByDescending(x=> x.CreatedAt).Where(x=> x.TrainingId == trainigId).Where(x=> x.SetupModuleId == SetupModuleId).Where(x=> x.AssesmentId ==  AssesmentId).Where(x=> x.EmployeeId == EmployeeId).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                    if (param != null){
                        if (param.PassedStatus){
                            returnValue = 3;
                        }else{
                            returnValue = 4;
                        }
                    } else {
                        var liveAssesment = DB.LiveAssesmentSkillChecks.Where(x=> x.TrainingId == trainigId).Where(x=> x.AssesmentGUID == AssesmentId).Where(x=> x.EmployeeGUID == EmployeeId).OrderByDescending(x=> x.CreatedAt).FirstOrDefault();
                        if(liveAssesment != null){
                            returnValue = 2;
                        } else {
                            returnValue = 1;
                        }
                } 
            }
            return returnValue;
        }

        public int GetTrackingProgressStatusModule(string EmployeeId, int? ModuleId, int? SetupModuleId, int trainingId){
            int returnValue = 1;
            var param = DB.FinalScores.OrderByDescending(x=> x.CreatedAt).Where(x=> x.TrainingId == trainingId).Where(x=> x.SetupModuleId==SetupModuleId).Where(x=> x.ModuleId ==  ModuleId).Where(x=> x.EmployeeId == EmployeeId).FirstOrDefault();
            if (param != null){
                if(param.PassedStatus){
                     returnValue = 3;
                } else {
                    returnValue = 4;
                }
            } else {
                var containTask =  DB.TaskAnswers.Where(x=> x.SetupModuleId == SetupModuleId).Where(x=> x.TrainingId == trainingId).Count();
                if (containTask > 0){
                    var answerData  = DB.TaskAnswerDetails.Include(x=> x.TaskAnswer).ThenInclude(x=> x.SetupModule).ThenInclude(x=> x.EnrollLearningTimes).Where(x=> x.TaskAnswer.SetupModule.ModuleId == ModuleId).Where(x=> x.TaskAnswer.EmployeeId == EmployeeId).Where(x=> x.TaskAnswer.TrainingId == trainingId).FirstOrDefault();
                    if (answerData != null){
                        returnValue = 2;
                    } else {
                        returnValue = 1;
                    }
                } else {
                    var alreadyTaken = DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Where(x=> x.SetupModuleId == SetupModuleId).Where(x=> x.EnrollLearning.TrainingId == trainingId).Where(x=> x.EnrollLearning.EmployeeId==EmployeeId).FirstOrDefault();
                    if (alreadyTaken.EndTime != null){
                        returnValue = 3;
                    } else {
                        returnValue = 1;
                    }
                }
            } 
            return returnValue;
        }
    }
}

