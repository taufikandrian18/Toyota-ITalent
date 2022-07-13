using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideBulkCalculatingLearningService
    {
        private readonly TalentContext DB;
        private readonly UserSideGenerateCertificatePDFService CertificateGeneratorMan;


        public UserSideBulkCalculatingLearningService(TalentContext db, UserSideGenerateCertificatePDFService userSideGenerateCertificatePDFService)
        {
            this.DB = db;
            this.CertificateGeneratorMan = userSideGenerateCertificatePDFService;
        }

        public async Task<bool> BulkCalculating(int rangeDate)
        {
            var todayDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var Queue = await DB.CalculateLearningQueue.Where(Q => todayDate.Date >= Q.FinishedAt.AddDays(rangeDate).Date).OrderByDescending(Q => Q.FinishedAt).ToListAsync();

            foreach (var CalculateQueue in Queue)
            {
                var enrollLearning = await DB.EnrollLearnings.Where(Q => Q.EnrollLearningId == CalculateQueue.EnrollLearningId).FirstOrDefaultAsync();

                var training = await DB.Trainings.Where(Q => Q.TrainingId == enrollLearning.TrainingId).FirstOrDefaultAsync();

                bool isModulePassed = true;
                bool isWeightedPassed = true;

                //cek kalo trainingnya udh diapus, langsung aja diapus dari queue
                if (training.IsDeleted == true)
                {
                    //delete from queue
                    DB.Remove(CalculateQueue);
                    await DB.SaveChangesAsync();
                    continue;
                }

                //CHECK KALO ISPASSED BUKAN NULL ARTINYA UDAH LULUS ATAU FAIL JADI Continue AJA
                if (enrollLearning.IsPassed != null)
                {
                    DB.Remove(CalculateQueue);
                    await DB.SaveChangesAsync();
                    continue;
                }

                //Normal Training (non Remedial)
                if (enrollLearning.RemedialLevel == 0)
                {
                    // Get All Setup Module Within Training with their min score and employee score semua
                    var EnrolledSetupModulesWithRemedial = await DB.GetSetupModuleCalculateList(enrollLearning.EnrollLearningId).ToListAsync();

                    if (EnrolledSetupModulesWithRemedial.Count == 0)
                    {
                        continue;
                    }

                    // yang ini bukan remedial
                    var EnrolledSetupModules = EnrolledSetupModulesWithRemedial.Where(Q => Q.IsForRemedial == false).ToList();

                    //Genarate Badges only for normal Modules
                    foreach (var setupModule in EnrolledSetupModules)
                    {
                        if (setupModule.EmployeeScore >= setupModule.MinimumScore)
                        {
                            var employeeBadges = await this.InsertEmployeeBadges(setupModule.SetupModuleId, enrollLearning.EmployeeId, setupModule.EmployeePoint);
                            if (employeeBadges != null)
                            {
                                DB.AddRange(employeeBadges);
                            }
                        }
                    }

                    //weighted score sesuai dengan rumus yg sudah disepakati
                    var WeigthedScore = DB.GetWeightedModelCalculate(enrollLearning.EnrollLearningId).Select(Q => Q.Weighted).FirstOrDefault();
                    
                    //dilihat dari inputtan user
                    var TotalMinimumScore = DB.CourseTrainingTypeMappings.Where(Q => Q.CourseId == training.CourseId && Q.TrainingTypeId == 4).Select(Q => Q.MinimumScore).FirstOrDefault();


                    //cek bagian 1 dia lulus atau tidak
                    if (WeigthedScore < TotalMinimumScore)
                    {
                        isWeightedPassed = false;
                    }
                    else
                    {
                        isWeightedPassed = true;
                    }

                    //cek kalo bagian 1 gak lulus, bagian 2 skip
                    if (isWeightedPassed == true)
                    {
                        foreach (var setupModule in EnrolledSetupModules)
                        {
                            if (setupModule.EmployeeScore < setupModule.MinimumScore)
                            {
                                isModulePassed = false;
                                break;
                            }
                        }
                    }

                    //Check for remedial
                    if (isWeightedPassed == false || isModulePassed == false)
                    {
                        //Check if there is remedial module for this training
                        //var allSetupModuleQuery = from t in this.DB.Trainings
                        //                          join sm in this.DB.SetupModules on t.CourseId equals sm.CourseId
                        //                          where t.TrainingId == enrollLearning.TrainingId && sm.IsDeleted == false
                        //                          select sm;

                        //var totalSetupModuleOnTraining = await allSetupModuleQuery.ToListAsync();

                        //if (totalSetupModuleOnTraining.Count() != EnrolledSetupModules.Count())
                        //check apakah ada modul remedial
                        if (EnrolledSetupModulesWithRemedial.Where(Q => Q.IsForRemedial).Any() == true)
                        {
                            //update for remedial

                            enrollLearning.RemedialLevel = 1;
                            InboxRemedial(enrollLearning.EmployeeId);
                        }
                        //there's no remedial module
                        else
                        {
                            enrollLearning.IsPassed = false;
                            InsertLearningHistoryForTraining(EnrolledSetupModules.FirstOrDefault().CourseName, enrollLearning);
                        }
                    }
                    //Training Success / Passed
                    else
                    {
                        enrollLearning.IsPassed = true;
                        await this.CertificateGeneratorMan.GenerateCertificateAfterTrainingAsync(enrollLearning.TrainingId.GetValueOrDefault(), enrollLearning.EmployeeId);
                        InsertLearningHistoryForTraining(EnrolledSetupModules.FirstOrDefault().CourseName, enrollLearning);
                    }
                }
                //For Remdial
                else
                {
                    var CalculateSetupModule = await DB.GetSpecificSetupModuleCalculate(enrollLearning.EnrollLearningId, CalculateQueue.SetupModuleId.GetValueOrDefault(), enrollLearning.TrainingId).FirstOrDefaultAsync();

                    //Check Remedial is not passed, so they have to take another remedial module
                    if (CalculateSetupModule.EmployeeScore < CalculateSetupModule.MinimumScore)
                    {
                        //REMEDIAL MAX Attemp: 3x  
                        if (enrollLearning.RemedialLevel < 3)
                        {
                            enrollLearning.RemedialLevel++;
                            InboxRemedial(enrollLearning.EmployeeId);
                        }
                        // Training Failed
                        else
                        {
                            enrollLearning.IsPassed = false;
                        }
                    }
                    //Finished Training, Success Remedial
                    else
                    {
                        enrollLearning.IsPassed = true;

                        await this.CertificateGeneratorMan.GenerateCertificateAfterTrainingAsync(enrollLearning.TrainingId.Value, enrollLearning.EmployeeId);
                        InsertLearningHistoryForTraining(CalculateSetupModule.CourseName, enrollLearning);

                        //Genarate Employee Badge 
                        var employeeBadges = await this.InsertEmployeeBadges(CalculateSetupModule.SetupModuleId, enrollLearning.EmployeeId, CalculateSetupModule.EmployeePoint);
                        if (employeeBadges != null)
                        {
                            DB.AddRange(employeeBadges);
                        }

                    }
                }


                //Delete From Queue
                DB.Remove(CalculateQueue);
                await DB.SaveChangesAsync();
            }


            return true;

        }


        public class CalculateTask{
            public string EmployeeId {get;set;}
            public int? TrainingId {get;set;}
            public int? SetupModuleId { get; set; }
        }

        public class CalculateAssesIsByOptionTemp
        {
            public string EmployeeId { get; set; }
            public int? TrainingId { get; set; }
            public int? SetupModuleId { get; set; }
            public string AssesmentId { get; set; }
            public string SkillCheckGUID { get; set; }
        }

        public async Task<bool> BulkCalculatingFinalScore()
        {
            
            var finalScoreData  = await DB.FinalScores.Include(x=> x.Training).Where(x=> x.Training.IsCertificate).Where(x=> x.CertificationStatus == null ).Where(x=> x.PassedStatus==true).OrderByDescending(x=> x.CreatedAt).ToListAsync();
            if(finalScoreData == null){
                return false;
            }

            var addNonTaskTrainingIds = await (from elt in DB.EnrollLearningTimes
                                         join sm in DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId
                                         join el in DB.EnrollLearnings on elt.EnrollLearningId equals el.EnrollLearningId
                                         join tr in DB.Trainings on el.TrainingId equals tr.TrainingId
                                         join p2 in DB.TaskAnswers on
                                         new { F1 = (int?)tr.TrainingId, F2 = (int?)sm.SetupModuleId } equals
                                         new { F1 = p2.TrainingId, F2 = p2.SetupModuleId } into temp
                                         from p2 in temp.DefaultIfEmpty()
                                         where sm.IsDeleted == false && tr.IsDeleted == false && p2 == null && elt.EndTime != null
                                         select tr.TrainingId).Distinct().ToListAsync();

            var listOfTrainingIds = finalScoreData.Select(x=> x.TrainingId).Distinct();

            var listOfTrainings =  await DB.Trainings.Where(x=> DateTime.Now.Date > x.EndDate.Value.Date).Where(x=> listOfTrainingIds.Contains(x.TrainingId)).Where(x=> x.EnumCeritificationTrigger.ToLower() == "personal").ToListAsync();
            //var listOfTrainings =  await DB.Trainings.Where(x=> listOfTrainingIds.Contains(x.TrainingId)).Where(x=> x.EnumCeritificationTrigger.ToLower() == "personal").ToListAsync();

            listOfTrainings.AddRange(await DB.Trainings.Where(x => DateTime.Now.Date > x.EndDate.Value.Date).Where(x => addNonTaskTrainingIds.Contains(x.TrainingId)).Where(x => x.EnumCeritificationTrigger.ToLower() == "personal").ToListAsync());
            //listOfTrainings.AddRange(await DB.Trainings.Where(x => addNonTaskTrainingIds.Contains(x.TrainingId)).Where(x => x.EnumCeritificationTrigger.ToLower() == "personal").ToListAsync());

            //var listOfTrainings = await DB.Trainings.Where(x => x.EndDate.Value.Date > DateTime.Now).Where(x => x.TrainingId == 437).Where(x => x.EnumCeritificationTrigger.ToLower() == "personal").ToListAsync();

            var dataToUpdate = new List<FinalScores>();

            var dataToInsert = new List<CalculateTask>();

            var sortDataToInsert = new List<CalculateTask>();

           // listOfTrainings = listOfTrainings.Where(x=> x.EndDate.GetValueOrDefault().Date > DateTime.Now.Date).ToList();

            foreach (var training in listOfTrainings)
            {   
                if(training.IsCertificate){
                    if(training.EnumCeritificationTrigger.ToLower() == "personal")
                    {
                        var enrollLearningTimes = await DB.EnrollLearningTimes.Include(z=> z.EnrollLearning).Include(x=> x.SetupModule).Where(x=> x.EnrollLearning.TrainingId == training.TrainingId).ToListAsync();
                            foreach (var enrollLearningTime in enrollLearningTimes)
                                if(enrollLearningTime.SetupModule != null){
                                {
                                    var passed = true;                  
                                    if(enrollLearningTime.SetupModule.ModuleId != null){
                                        var moduleId= enrollLearningTime.SetupModule.ModuleId;
                                        var taskModule = await DB.TaskAnswers.Include(x => x.TaskAnswerDetails).Where(o=> o.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId && o.TrainingId == training.TrainingId).FirstOrDefaultAsync();
                                        if (taskModule == null){
                                            taskModule = new TaskAnswers();
                                            taskModule.TaskAnswerDetails = new List<TaskAnswerDetails>();
                                        }
                                        if (taskModule.TaskAnswerDetails.Count() > 0){
                                            var result = await DB.FinalScores.Where(x=> x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId).Where(x=> x.TrainingId == training.TrainingId && x.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).OrderByDescending(x=> x.CreatedAt).FirstOrDefaultAsync();
                                            if(result!=null){
                                                if (result.PassedStatus == false){
                                                    passed = false;
                                                    dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                } else {
                                                        var temp = new CalculateTask{
                                                            EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                            TrainingId = training.TrainingId,
                                                            SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                        };
                                                        dataToInsert.Add(temp);
                                                        dataToUpdate.Add(result);
                                                }
                                            } else {
                                                    passed = false;
                                                    dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                            }
                                        } else {
                                            if (enrollLearningTime.EndTime == null){
                                                passed = false;
                                                if(enrollLearningTime.EnrollLearning != null){
                                                    dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                   

                                                }
                                            } else {
                                                  var temp = new CalculateTask{
                                                            EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                            TrainingId = training.TrainingId,
                                                            SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                  };
                                                        dataToInsert.Add(temp);
                                            }
                                        } 
                                    }else if (enrollLearningTime.SetupModule.AssesmentId != null) {
                                        var assesmentData = await DB.Assesments.FirstOrDefaultAsync(z=> z.GUID == enrollLearningTime.SetupModule.AssesmentId);
                                        if(assesmentData.IsByOption){
                                            var dataToInsertTemp = new List<CalculateAssesIsByOptionTemp>();
                                            var sortDataToInsertTemp = new List<CalculateAssesIsByOptionTemp>();
                                            var assesmentSkillCheckData = await DB.AssesmentSkillChecks.Where(x=> x.AssesmentGUID == assesmentData.GUID).ToListAsync();
                                            foreach (var assesmentSkillCheck in assesmentSkillCheckData){
                                                var finalScore =  new FinalScores();
                                                if(assesmentData.EnumScoringMethod.ToLower()=="highest"){
                                                    finalScore = await DB.FinalScores.Where(z=> z.TrainingId == training.TrainingId && z.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).Where(z=> z.AssesmentId == assesmentData.GUID).Where(z=> z.SkillCheckGuid == assesmentSkillCheck.SkillCheckGUID).OrderByDescending(x=> x.FinalScore).FirstOrDefaultAsync();
                                                    if(finalScore !=null){
                                                        if(!finalScore.PassedStatus){
                                                            passed = false;
                                                            dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                            dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                        }
                                                    }else {
                                                            passed = false;
                                                            dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                            dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    }
                                                } else{
                                                    finalScore = await DB.FinalScores.Where(z=> z.TrainingId == training.TrainingId && z.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).Where(z=> z.AssesmentId == assesmentData.GUID).Where(z=> z.SkillCheckGuid == assesmentSkillCheck.SkillCheckGUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefaultAsync();
                                                    if(finalScore != null){
                                                        if(!finalScore.PassedStatus){
                                                            passed = false;
                                                            dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                            dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                        }
                                                    } else {
                                                            passed = false;
                                                            dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                            dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    }
                                                }
                                                if(passed){
                                                    //var temp = new CalculateTask{
                                                    //            EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                    //            TrainingId = training.TrainingId,
                                                    //            SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                    //};
                                                    //dataToInsert.Add(temp);
                                                    //dataToUpdate.Add(finalScore);

                                                    var temp = new CalculateAssesIsByOptionTemp
                                                    {
                                                        EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                        TrainingId = training.TrainingId,
                                                        SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId,
                                                        AssesmentId = assesmentData.GUID,
                                                        SkillCheckGUID = assesmentSkillCheck.SkillCheckGUID
                                                    };
                                                    dataToInsertTemp.Add(temp);
                                                    dataToUpdate.Add(finalScore);
                                                }
                                            }
                                            sortDataToInsertTemp = dataToInsertTemp.GroupBy(x => new { x.EmployeeId, x.TrainingId, x.SetupModuleId, x.AssesmentId }).Select(z => new CalculateAssesIsByOptionTemp
                                            {
                                                EmployeeId = z.Key.EmployeeId,
                                                TrainingId = z.Key.TrainingId,
                                                SetupModuleId = z.Key.SetupModuleId,
                                                AssesmentId = z.Key.AssesmentId,
                                                SkillCheckGUID = dataToInsertTemp.Where(x => x.EmployeeId == z.Key.EmployeeId && x.TrainingId == z.Key.TrainingId && x.SetupModuleId == z.Key.SetupModuleId && x.AssesmentId == z.Key.AssesmentId).Select(x => x.SkillCheckGUID).Distinct().Count().ToString()
                                            }).ToList();

                                            foreach(var item in sortDataToInsertTemp)
                                            {
                                                //var query = DB.GetSkillCheckByAssesmentData(item.TrainingId.HasValue ? item.TrainingId.Value : 0, item.EmployeeId, item.AssesmentId).AsQueryable();
                                                var query = DB.AssesmentSkillChecks.Where(Q => Q.AssesmentGUID == item.AssesmentId).ToList();
                                                var varQuery = query.ToList();
                                                var compare = Convert.ToInt32(sortDataToInsertTemp.Where(Q => Q.SetupModuleId == item.SetupModuleId && Q.TrainingId == item.TrainingId && Q.EmployeeId == item.EmployeeId && Q.SetupModuleId == item.SetupModuleId && Q.AssesmentId == item.AssesmentId).Select(Q => Q.SkillCheckGUID).FirstOrDefault().Trim());
                                                if (compare == query.Count())
                                                {
                                                    var temp = new CalculateTask
                                                    {
                                                        EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                        TrainingId = training.TrainingId,
                                                        SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                    };
                                                    dataToInsert.Add(temp);
                                                }
                                            }
                                        } else {
                                            var result = await DB.FinalScores.Where(x=> x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId).Where(x=> x.TrainingId == training.TrainingId && x.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).Where(x=> x.AssesmentId == assesmentData.GUID).OrderByDescending(x=> x.CreatedAt).FirstOrDefaultAsync();
                                            if (result != null){
                                                if (result.PassedStatus == false){
                                                    passed = false;
                                                    dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                }
                                            } else {
                                                passed = false;
                                                dataToUpdate.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                dataToInsert.RemoveAll(x=> x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                            }

                                            if(passed){
                                                    var temp = new CalculateTask{
                                                                EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                                TrainingId = training.TrainingId,
                                                                SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                    };
                                                    dataToInsert.Add(temp);
                                                    dataToUpdate.Add(result);
                                            }
                                        }
                                    }                               
                                }
                            }                 
                        } 
                    }
            }


            var tempSortDataToInsert = dataToInsert.GroupBy(x=> new {x.EmployeeId, x.TrainingId, x.SetupModuleId}).Select(z=> new CalculateTask{
                    EmployeeId = z.Key.EmployeeId,
                    TrainingId = z.Key.TrainingId,
                    SetupModuleId = z.Key.SetupModuleId
                    //SetupModuleId = dataToInsert.Where(x => x.EmployeeId == z.Key.EmployeeId && x.TrainingId == z.Key.TrainingId).Select(x => x.SetupModuleId).Count() //count jumlah setup module id membedakan yang lulus atau tidak
            }).ToList();

            sortDataToInsert = tempSortDataToInsert.GroupBy(x=> new {x.EmployeeId, x.TrainingId}).Select(z=> new CalculateTask{
                    EmployeeId = z.Key.EmployeeId,
                    TrainingId = z.Key.TrainingId,
                    SetupModuleId = tempSortDataToInsert.Where(x => x.EmployeeId == z.Key.EmployeeId && x.TrainingId == z.Key.TrainingId).Select(x => x.SetupModuleId).Count() //count jumlah setup module id membedakan yang lulus atau tidak
            }).ToList();

            foreach (var item in sortDataToInsert){

                //checking dengan database dengan data calculate task yang sudah di group by
                var query = DB.GetTrainingIdNonTaskData(item.TrainingId.HasValue ? item.TrainingId.Value : 0,item.EmployeeId).AsQueryable();
                //var countSortData = sortDataToInsert.Where(Q => Q.SetupModuleId == item.SetupModuleId && Q.TrainingId == item.TrainingId && Q.EmployeeId == item.EmployeeId).Count();
                var countSortData = sortDataToInsert.Where(Q => Q.SetupModuleId == item.SetupModuleId && Q.TrainingId == item.TrainingId && Q.EmployeeId == item.EmployeeId).Select(Q => Q.SetupModuleId).FirstOrDefault();
                var countTrainingId = query.Select(Q => Q.JumlahSetupModule).FirstOrDefault();
                if(Convert.ToInt32(countSortData) == Convert.ToInt32(countTrainingId))
                {
                    var valid = await this.CertificateGeneratorMan.GenerateCertificateAfterTrainingFromFinalScoresAsync(item.TrainingId.Value, item.EmployeeId);
                }
            }

            foreach (var item in dataToUpdate)
            { 
                var update = DB.FinalScores.Where(x=> x.FinalScoreId == item.FinalScoreId).FirstOrDefault();
                update.CertificationStatus = true;
                await DB.SaveChangesAsync();
            }
            return true;

        }

        public async Task<bool> BulkCalculatingFinalScoreHierarki()
        {
            
            var finalScoreData  = await DB.FinalScores.Include(x=> x.Training).Where(x=> x.Training.IsCertificate).Where(x=> x.CertificationStatus == null ).Where(x=> x.PassedStatus==true).OrderByDescending(x=> x.CreatedAt).ToListAsync();
            if(finalScoreData == null){
                return false;
            }

            var addNonTaskTrainingIds = await (from elt in DB.EnrollLearningTimes
                                               join sm in DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId
                                               join el in DB.EnrollLearnings on elt.EnrollLearningId equals el.EnrollLearningId
                                               join tr in DB.Trainings on el.TrainingId equals tr.TrainingId
                                               join p2 in DB.TaskAnswers on
                                               new { F1 = (int?)tr.TrainingId, F2 = (int?)sm.SetupModuleId } equals
                                               new { F1 = p2.TrainingId, F2 = p2.SetupModuleId } into temp
                                               from p2 in temp.DefaultIfEmpty()
                                               where sm.IsDeleted == false && tr.IsDeleted == false && p2 == null && elt.EndTime != null
                                               select tr.TrainingId).Distinct().ToListAsync();

            var listOfTrainingIds = finalScoreData.Select(x=> x.TrainingId).Distinct();

            var listOfTrainings =  await DB.Trainings.Where(x=> DateTime.Now.Date > x.EndDate.Value.Date).Where(x=> listOfTrainingIds.Contains(x.TrainingId)).Where(x=> x.EnumCeritificationTrigger.ToLower() == "hirarki").ToListAsync();
            //var listOfTrainings = await DB.Trainings.Where(x => listOfTrainingIds.Contains(x.TrainingId)).Where(x => x.EnumCeritificationTrigger.ToLower() == "hirarki").ToListAsync();

            listOfTrainings.AddRange(await DB.Trainings.Where(x => DateTime.Now.Date > x.EndDate.Value.Date).Where(x => addNonTaskTrainingIds.Contains(x.TrainingId)).Where(x => x.EnumCeritificationTrigger.ToLower() == "hirarki").ToListAsync());
            //listOfTrainings.AddRange(await DB.Trainings.Where(x => addNonTaskTrainingIds.Contains(x.TrainingId)).Where(x => x.EnumCeritificationTrigger.ToLower() == "hirarki").ToListAsync());

            //var listOfTrainings = await DB.Trainings.Where(x => x.EndDate.Value.Date > DateTime.Now).Where(x => x.TrainingId == 498).Where(x => x.EnumCeritificationTrigger.ToLower() == "hirarki").ToListAsync();

            var dataToUpdate = new List<FinalScores>();

            var dataToInsert = new List<CalculateTask>();

            var sortDataToInsert = new List<CalculateTask>();

            var sortDataToInsertLeader = new List<CalculateTask>();

            var sortDataToInsertLeaderTemp = new List<CalculateTask>();

            var dataToUpdateLeader = new List<FinalScores>();

            foreach (var training in listOfTrainings)
            {   
                if(training.IsCertificate){
                    if(training.EnumCeritificationTrigger.ToLower() == "hirarki")
                    {
                        var dataToInsertLeader = new List<CalculateTask>();

                        //buat nyari enrol learning leader
                        var enrollLearningTimes = await DB.EnrollLearningTimes.Include(z => z.EnrollLearning).Include(x => x.SetupModule).Where(x => x.EnrollLearning.TrainingId == training.TrainingId).ToListAsync();
                        //var enrollLearningTimes = await DB.EnrollLearningTimes.Include(z => z.EnrollLearning).Include(x => x.SetupModule).Where(x => x.EnrollLearning.TrainingId == training.TrainingId && x.EnrollLearning.EmployeeId == "111222").ToListAsync();
                        foreach (var enrollLearningTime in enrollLearningTimes)
                        {
                            // ini di dalam if untuk menghitung nilai leader terhadap course yang menempel di sertifikat hierarchy
                            if (enrollLearningTime.SetupModule != null)
                            {
                                var passed = true;
                                if (enrollLearningTime.SetupModule.ModuleId != null)
                                {
                                    var moduleId = enrollLearningTime.SetupModule.ModuleId;
                                    var taskModule = await DB.TaskAnswers.Include(x => x.TaskAnswerDetails).Where(o => o.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId && o.TrainingId == training.TrainingId).FirstOrDefaultAsync();
                                    if (taskModule == null)
                                    {
                                        taskModule = new TaskAnswers();
                                        taskModule.TaskAnswerDetails = new List<TaskAnswerDetails>();
                                    }
                                    if (taskModule.TaskAnswerDetails.Count() > 0)
                                    {
                                        var result = await DB.FinalScores.Where(x => x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId).Where(x => x.TrainingId == training.TrainingId && x.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
                                        if (result != null)
                                        {
                                            if (result.PassedStatus == false)
                                            {
                                                passed = false;
                                                dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                            }
                                            else
                                            {
                                                var temp = new CalculateTask
                                                {
                                                    EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                    TrainingId = training.TrainingId,
                                                    SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                };
                                                dataToInsertLeader.Add(temp);
                                                dataToUpdateLeader.Add(result);
                                            }
                                        }
                                        else
                                        {
                                            passed = false;
                                            dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                            dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                        }
                                    }
                                    else
                                    {
                                        if (enrollLearningTime.EndTime == null)
                                        {
                                            passed = false;
                                            if (enrollLearningTime.EnrollLearning != null)
                                            {
                                                dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);


                                            }
                                        }
                                        else
                                        {
                                            var temp = new CalculateTask
                                            {
                                                EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                TrainingId = training.TrainingId,
                                                SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                            };
                                            dataToInsertLeader.Add(temp);
                                        }
                                    }
                                }
                                else if (enrollLearningTime.SetupModule.AssesmentId != null)
                                {
                                    var assesmentData = await DB.Assesments.FirstOrDefaultAsync(z => z.GUID == enrollLearningTime.SetupModule.AssesmentId);
                                    if (assesmentData.IsByOption)
                                    {
                                        var dataToInsertTemp = new List<CalculateAssesIsByOptionTemp>();
                                        var sortDataToInsertTemp = new List<CalculateAssesIsByOptionTemp>();
                                        var assesmentSkillCheckData = await DB.AssesmentSkillChecks.Where(x => x.AssesmentGUID == assesmentData.GUID).ToListAsync();
                                        foreach (var assesmentSkillCheck in assesmentSkillCheckData)
                                        {
                                            var finalScore = new FinalScores();
                                            if (assesmentData.EnumScoringMethod.ToLower() == "highest")
                                            {
                                                finalScore = await DB.FinalScores.Where(z => z.TrainingId == training.TrainingId && z.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).Where(z => z.AssesmentId == assesmentData.GUID).Where(z => z.SkillCheckGuid == assesmentSkillCheck.SkillCheckGUID).OrderByDescending(x => x.FinalScore).FirstOrDefaultAsync();
                                                if (finalScore != null)
                                                {
                                                    if (!finalScore.PassedStatus)
                                                    {
                                                        passed = false;
                                                        dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                        dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    }
                                                }
                                                else
                                                {
                                                    passed = false;
                                                    dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                }
                                            }
                                            else
                                            {
                                                finalScore = await DB.FinalScores.Where(z => z.TrainingId == training.TrainingId && z.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).Where(z => z.AssesmentId == assesmentData.GUID).Where(z => z.SkillCheckGuid == assesmentSkillCheck.SkillCheckGUID).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
                                                if (finalScore != null)
                                                {
                                                    if (!finalScore.PassedStatus)
                                                    {
                                                        passed = false;
                                                        dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                        dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    }
                                                }
                                                else
                                                {
                                                    passed = false;
                                                    dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                    dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                }
                                            }
                                            if (passed)
                                            {
                                                //var temp = new CalculateTask{
                                                //            EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                //            TrainingId = training.TrainingId,
                                                //            SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                //};
                                                //dataToInsert.Add(temp);
                                                //dataToUpdate.Add(finalScore);

                                                var temp = new CalculateAssesIsByOptionTemp
                                                {
                                                    EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                    TrainingId = training.TrainingId,
                                                    SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId,
                                                    AssesmentId = assesmentData.GUID,
                                                    SkillCheckGUID = assesmentSkillCheck.SkillCheckGUID
                                                };
                                                dataToInsertTemp.Add(temp);
                                                dataToUpdateLeader.Add(finalScore);
                                            }
                                        }
                                        sortDataToInsertTemp = dataToInsertTemp.GroupBy(x => new { x.EmployeeId, x.TrainingId, x.SetupModuleId, x.AssesmentId }).Select(z => new CalculateAssesIsByOptionTemp
                                        {
                                            EmployeeId = z.Key.EmployeeId,
                                            TrainingId = z.Key.TrainingId,
                                            SetupModuleId = z.Key.SetupModuleId,
                                            AssesmentId = z.Key.AssesmentId,
                                            SkillCheckGUID = dataToInsertTemp.Where(x => x.EmployeeId == z.Key.EmployeeId && x.TrainingId == z.Key.TrainingId && x.SetupModuleId == z.Key.SetupModuleId && x.AssesmentId == z.Key.AssesmentId).Select(x => x.SkillCheckGUID).Distinct().Count().ToString()
                                        }).ToList();

                                        foreach (var item in sortDataToInsertTemp)
                                        {
                                            //var query = DB.GetSkillCheckByAssesmentData(item.TrainingId.HasValue ? item.TrainingId.Value : 0, item.EmployeeId, item.AssesmentId).AsQueryable();
                                            var query = DB.AssesmentSkillChecks.Where(Q => Q.AssesmentGUID == item.AssesmentId).ToList();
                                            if (Convert.ToInt32(sortDataToInsertTemp.Where(Q => Q.SetupModuleId == item.SetupModuleId && Q.TrainingId == item.TrainingId && Q.EmployeeId == item.EmployeeId && Q.SetupModuleId == item.SetupModuleId && Q.AssesmentId == item.AssesmentId).Select(Q => Q.SkillCheckGUID).FirstOrDefault().Trim()) == query.Count())
                                            {
                                                var temp = new CalculateTask
                                                {
                                                    EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                    TrainingId = training.TrainingId,
                                                    SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                                };
                                                dataToInsertLeader.Add(temp);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var result = await DB.FinalScores.Where(x => x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId).Where(x => x.TrainingId == training.TrainingId && x.SetupModuleId == enrollLearningTime.SetupModule.SetupModuleId).Where(x => x.AssesmentId == assesmentData.GUID).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
                                        if (result != null)
                                        {
                                            if (result.PassedStatus == false)
                                            {
                                                passed = false;
                                                dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                                dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                            }
                                        }
                                        else
                                        {
                                            passed = false;
                                            dataToUpdateLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                            dataToInsertLeader.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTime.EnrollLearning.EmployeeId);
                                        }

                                        if (passed)
                                        {
                                            var temp = new CalculateTask
                                            {
                                                EmployeeId = enrollLearningTime.EnrollLearning.EmployeeId,
                                                TrainingId = training.TrainingId,
                                                SetupModuleId = enrollLearningTime.SetupModule.SetupModuleId
                                            };
                                            dataToInsertLeader.Add(temp);
                                            dataToUpdateLeader.Add(result);
                                        }
                                    }
                                }
                            }

                            // buat nyari apakah semuanya member sudah mendapat sertifikat
                            var teamMembers = await DB.TeamDetails.Include(x => x.Team).Include(x => x.Employee).Where(x => x.Employee.IsDeleted == false).Where(x => x.Team.TeamLeaderId == enrollLearningTime.EnrollLearning.EmployeeId).Select(x => x.EmployeeId).ToListAsync();
                            var relatedTraining = await DB.TrainingCertifications.Where(x => x.TrainingId == training.TrainingId).Select(x => x.RelatedTrainingId).ToListAsync();

                            var enrollLearningTimesM = await DB.EnrollLearningTimes.Include(z => z.EnrollLearning).Include(x => x.SetupModule).Where(x => relatedTraining.Contains(x.EnrollLearning.TrainingId)).Where(x => teamMembers.Contains(x.EnrollLearning.EmployeeId)).GroupBy(x => new { x.EnrollLearning.EmployeeId, x.EnrollLearning.TrainingId }).Select(z => new {
                                EmployeeId = z.Key.EmployeeId,
                                TrainingId = z.Key.TrainingId
                            }).ToListAsync();

                            foreach (var enrollLearningTimeMember in enrollLearningTimesM)
                            {   
                                var certificates = await DB.EmployeeCertificates.Where(z => teamMembers.Contains(z.EmployeeId)).Where(z => z.TrainingId == enrollLearningTimeMember.TrainingId).CountAsync();
                                if (certificates == teamMembers.Count)
                                {
                                    var temp = new CalculateTask
                                    {
                                        EmployeeId = enrollLearningTimeMember.EmployeeId,
                                        TrainingId = training.TrainingId
                                    };
                                    dataToInsert.Add(temp);
                                }
                                else
                                {
                                    dataToInsert.RemoveAll(x => x.TrainingId.GetValueOrDefault(0) == training.TrainingId && x.EmployeeId == enrollLearningTimeMember.EmployeeId);
                                }
                            }
                        }

                        sortDataToInsertLeaderTemp.AddRange(dataToInsertLeader);
                    } 
                }
            }

            sortDataToInsert = dataToInsert.GroupBy(x=> new {x.EmployeeId, x.TrainingId}).Select(z=> new CalculateTask{
                    EmployeeId = z.Key.EmployeeId,
                    TrainingId = z.Key.TrainingId
            }).ToList();

            var tempSortDataToInsertLeader = sortDataToInsertLeaderTemp.GroupBy(x=> new {x.EmployeeId, x.TrainingId, x.SetupModuleId}).Select(z=> new CalculateTask{
                    EmployeeId = z.Key.EmployeeId,
                    TrainingId = z.Key.TrainingId,
                    SetupModuleId = z.Key.SetupModuleId
                    //SetupModuleId = dataToInsert.Where(x => x.EmployeeId == z.Key.EmployeeId && x.TrainingId == z.Key.TrainingId).Select(x => x.SetupModuleId).Count() //count jumlah setup module id membedakan yang lulus atau tidak
            }).ToList();

            sortDataToInsertLeader = tempSortDataToInsertLeader.GroupBy(x => new { x.EmployeeId, x.TrainingId }).Select(z => new CalculateTask
            {
                EmployeeId = z.Key.EmployeeId,
                TrainingId = z.Key.TrainingId,
                SetupModuleId = tempSortDataToInsertLeader.Where(x => x.EmployeeId == z.Key.EmployeeId && x.TrainingId == z.Key.TrainingId).Select(x => x.SetupModuleId).Count() //count jumlah setup module id membedakan yang lulus atau tidak
            }).ToList();

            foreach (var item in sortDataToInsertLeader)
            {
                var teamMembers = await DB.TeamDetails.Include(x => x.Team).Where(x => x.Team.TeamLeaderId == item.EmployeeId).Select(x => x.EmployeeId).ToListAsync();
                var query = DB.GetTrainingIdNonTaskData(item.TrainingId.HasValue ? item.TrainingId.Value : 0, item.EmployeeId).AsQueryable();
                if (Convert.ToInt32(sortDataToInsertLeader.Where(Q => Q.SetupModuleId == item.SetupModuleId && Q.TrainingId == item.TrainingId && Q.EmployeeId == item.EmployeeId).Select(Q => Q.SetupModuleId).FirstOrDefault()) == Convert.ToInt32(query.Select(Q => Q.JumlahSetupModule).FirstOrDefault()) && (sortDataToInsert.Where(x => x.TrainingId == item.TrainingId).Count() == teamMembers.Count()))
                {
                    var valid = await this.CertificateGeneratorMan.GenerateCertificateAfterTrainingFromFinalScoresAsync(item.TrainingId.Value, item.EmployeeId);
                }
            }

            // buat update certificate status di final score leader 
            foreach (var item in dataToUpdateLeader)
            {
                var update = DB.FinalScores.Where(x => x.FinalScoreId == item.FinalScoreId).FirstOrDefault();
                update.CertificationStatus = true;
                await DB.SaveChangesAsync();
            }

            foreach (var item in dataToUpdate)
            { 
                var update = DB.FinalScores.Where(x=> x.FinalScoreId == item.FinalScoreId).FirstOrDefault();
                update.CertificationStatus = true;
                await DB.SaveChangesAsync();
            }
            return true;

        }



        public void InsertLearningHistoryForTraining(string CourseName, EnrollLearnings enrollLearning)
        {
            DB.Add(new LearningHistories
            {
                EmployeeId = enrollLearning.EmployeeId,
                TrainingId = enrollLearning.TrainingId,
                SetupModuleId = null,
                PopQuizId = null,
                Name = CourseName
            });
        }


        public void InboxRemedial(string employeeId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var remedial = new MobileInboxes
            {
                EmployeeId = employeeId,
                MobileInboxTypeId = 6,
                Title = "Remedial",
                Message = "Maaf, Anda tidak lulus. Silahkan melakukan Remedial Pada module yang ada  Terima Kasih",
                CreatedAt = thisDate,
                CreatedBy = "SYSTEM",
                IsRead = false
            };
            this.DB.MobileInboxes.Add(remedial);
        }

        public async Task<List<EmployeeBadges>> InsertEmployeeBadges(int setupModuleId, string employeeId, int totalPoint)
        {
            //validasi point
            var minimumPoint = await (from t in this.DB.Topics
                                      join mtm in this.DB.ModuleTopicMappings on t.TopicId equals mtm.TopicId
                                      join sm in this.DB.SetupModules on mtm.ModuleId equals sm.ModuleId
                                      where sm.SetupModuleId == setupModuleId
                                      select t.TopicMinimumPoints).FirstOrDefaultAsync();

            if (totalPoint < minimumPoint)
            {
                return null;
            }


            //BADGE YANG SUDAH ADA 
            var check = await this.DB.EmployeeBadges.Where(Q => Q.EmployeeId == employeeId).Select(Q => new EmployeeBadgesViewModel
            {
                EmployeeId = Q.EmployeeId,
                EbadgeId = Q.EbadgeId.GetValueOrDefault(),
                TopicId = Q.TopicId.GetValueOrDefault()
            }).ToListAsync();

            var badges = await (from t in this.DB.Topics
                                join mtm in this.DB.ModuleTopicMappings on t.TopicId equals mtm.TopicId
                                join sm in this.DB.SetupModules on mtm.ModuleId equals sm.ModuleId
                                where sm.SetupModuleId == setupModuleId
                                select new EmployeeBadgesViewModel
                                {
                                    EmployeeId = employeeId,
                                    TopicId = t.TopicId,
                                    EbadgeId = t.EbadgeId.GetValueOrDefault()
                                }).ToListAsync();

            foreach (var badge in check)
            {
                var existed = badges.Where(Q => Q.TopicId == badge.TopicId && Q.EbadgeId == badge.EbadgeId).FirstOrDefault();
                if (existed != null)
                {
                    badges.Remove(existed);
                }
            }

            var employeeBadgesToAdd = new List<EmployeeBadges>();
            //KALAU GAK ADA YANG BARU... RETURN
            if (badges.Count() == 0)
            {
                return null;
            }
            //TAMBAHIN YANG BARU
            foreach (var x in badges)
            {

                employeeBadgesToAdd.Add(new EmployeeBadges
                {
                    EmployeeId = employeeId,
                    TopicId = x.TopicId,
                    EbadgeId = x.EbadgeId
                });
            }

            return employeeBadgesToAdd;
        }

    }
}