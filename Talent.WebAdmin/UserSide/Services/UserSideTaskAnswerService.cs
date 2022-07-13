using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTaskAnswerService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly UserSideGenerateCertificatePDFService CertificateGeneratorMan;
        private readonly PushNotificationService PNService;
        private readonly ClaimHelper HelperMan;

        public UserSideTaskAnswerService(TalentContext talentContext, IFileStorageService fileService, UserSideGenerateCertificatePDFService userSideGenerateCertificatePDFService,PushNotificationService pushNotificationService, ClaimHelper helper)
        {
            this.DB = talentContext;
            this.FileMan = fileService;
            this.CertificateGeneratorMan = userSideGenerateCertificatePDFService;
            this.PNService = pushNotificationService;
            this.HelperMan = helper;
        }

        public async Task<bool> InsertTaskAnswer(TaskAnswerInsertModel taskAnswers, string employeeId, bool withTest)
        {
            try
            {
                employeeId = employeeId.ToLower();
                var totalPoint = 0;
                var totalScore = 0;
                var isDataExist = false;

                //Untuk cek apakah sudah ada TaskAnswer yang sama
                var isExist = await this.DB.TaskAnswers
                    .Where(Q => Q.EmployeeId == employeeId && Q.SetupModuleId == taskAnswers.SetupModuleId && Q.TrainingId == taskAnswers.TrainingId)
                    .AnyAsync();

                if (isExist)
                {
                    //foreach (var ans in taskAnswers.Answer)
                    //{
                    //    var isDtlExist = await this.DB.TaskAnswerDetails
                    //    .Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.SetupModuleId == taskAnswers.SetupModuleId && Q.TaskAnswer.TrainingId == taskAnswers.TrainingId && Q.TaskId == ans.TaskId && Q.Attempts == ans.Attempts)
                    //    .AnyAsync();
                    //    if (isDtlExist)
                    //    {
                    //        isDataExist = true;
                    //    }
                    //    else
                    //    {
                    //        isDataExist = false;
                    //    }
                    //}
                    if (!await this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.SetupModuleId == taskAnswers.SetupModuleId && Q.TaskAnswer.TrainingId == taskAnswers.TrainingId && Q.Attempts == taskAnswers.Answer.FirstOrDefault().Attempts).AnyAsync())
                    {
                        isDataExist = false;
                    }
                }

                if (isExist == true && isDataExist == true)
                {
                    return true;
                }

                if (withTest == true)
                {
                    //SIMPAN KE JAWABAN DB
                    (totalPoint, totalScore) = await this.InsertTaskAnswer(taskAnswers, employeeId, totalPoint, totalScore);
                }
                //COURSE
                if (taskAnswers.TrainingId != null)
                {
                    var enrollLearnings = await this.EnrollLearning(taskAnswers, employeeId, totalPoint);

                    var enrollment = await this.DB.EnrollLearnings.Where(Q => Q.TrainingId == taskAnswers.TrainingId && Q.EmployeeId == employeeId).Select(Q => new { Q.RemedialLevel, Q.IsPassed }).FirstOrDefaultAsync();
                    //CHECK KALO ISPASSED BUKAN NULL ARTINYA UDAH LULUS ATAU FAIL JADI RETURN AJA
                    if (enrollment.IsPassed != null)
                    {
                        return true;
                    }
                    //CHECK TASK REMED ATAU TASK BIASA
                    if (enrollment.RemedialLevel == 0)
                    {
                        //var setupModule = await (from sm in this.DB.SetupModules
                        //                         join m in this.DB.Modules on sm.ModuleId equals m.ModuleId
                        //                         where sm.SetupModuleId == taskAnswers.SetupModuleId
                        //                         select new
                        //                         {
                        //                             m.ModuleName,
                        //                             sm.MinimumScore
                        //                         }).FirstOrDefaultAsync();
                        //if (totalScore >= setupModule.MinimumScore)
                        //{
                        //    var employeeBadges = await this.InsertEmployeeBadges(taskAnswers.SetupModuleId.GetValueOrDefault(), employeeId, totalPoint);
                        //    if (employeeBadges != null)
                        //    {
                        //        DB.AddRange(employeeBadges);
                        //    }
                        //}

                        /// Cek Apakah sedang mengerjakan modul terakhir training 
                        /// diluar modul remed. Jika ya, maka lanjut untuk penilaian training.

                        var (isDoingLastNormalModule, allSetupModule) = await IsDone(taskAnswers, employeeId, false);

                        if (isDoingLastNormalModule)
                        {
                            var Queue = new CalculateLearningQueue
                            {
                                //CalculateLearningQueueId = new Guid(),
                                CalculateLearningQueueId = Guid.NewGuid(),
                                EnrollLearningId = enrollLearnings.EnrollLearningId,
                                EnrollType = EnrollTypeEnum.Normal,
                                FinishedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                                SetupModuleId = null
                            };

                            DB.Add(Queue);

                            //Selesai Training
                            enrollLearnings.IsPassed = true;
                        }
                        await DB.SaveChangesAsync();
                        return true;

                        // memulai penilaian training.
                        //var minimumPassingGrade = 0;

                        //foreach (var module in allSetupModule)
                        //{
                        //    var minimumScore = module.MinimumScore;
                        //    if (minimumScore.HasValue) minimumPassingGrade = minimumPassingGrade + minimumScore.Value;
                        //}
                        //var setupModuleIds = allSetupModule.Select(Q => Q.SetupModuleId).ToList();
                        //var employeeScores = await (
                        //        from ta in this.DB.TaskAnswers
                        //        join tad in this.DB.TaskAnswerDetails on ta.TaskAnswerId equals tad.TaskAnswerId
                        //        where ta.EmployeeId == employeeId && setupModuleIds.Contains(ta.SetupModuleId.GetValueOrDefault())
                        //        select tad.Score.GetValueOrDefault()
                        //    ).ToListAsync();


                        //var employeeTotalScore = totalScore;

                        //foreach (var score in employeeScores)
                        //{
                        //    employeeTotalScore = employeeTotalScore + score;
                        //}

                        //if (employeeTotalScore < minimumPassingGrade)
                        //{
                        //    /// Cek apakah jika tidak lulus dan tersedia remed.
                        //    var (isDoneNoRemedial, _) = await IsDone(taskAnswers, employeeId, true);
                        //    if (isDoneNoRemedial)
                        //    {
                        //        /// Tidak tersedia
                        //        enrollLearnings.IsPassed = false;
                        //        await this.InsertLearningHistory(employeeId, taskAnswers);

                        //    }
                        //    else
                        //    {
                        //        enrollLearnings.RemedialLevel = 1;
                        //        InboxRemedial(employeeId);
                        //    }
                        //}
                        //else
                        //{
                        //    enrollLearnings.IsPassed = true;
                        //    await this.CertificateGeneratorMan.GenerateCertificateAfterTrainingAsync(enrollLearnings.TrainingId.Value, employeeId);
                        //    await this.InsertLearningHistory(employeeId, taskAnswers);
                        //}
                    }
                    else
                    {
                        var Queue = new CalculateLearningQueue
                        {
                            //CalculateLearningQueueId = new Guid(),
                            CalculateLearningQueueId = Guid.NewGuid(),
                            EnrollLearningId = enrollLearnings.EnrollLearningId,
                            EnrollType = EnrollTypeEnum.Remedial,
                            FinishedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                            SetupModuleId = taskAnswers.SetupModuleId
                        };

                        DB.Add(Queue);

                        //Selesai Training
                        enrollLearnings.IsPassed = true;

                        await DB.SaveChangesAsync();
                        return true;

                        //var minimumPassingGrade = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == taskAnswers.SetupModuleId).Select(Q => Q.MinimumScore.GetValueOrDefault()).FirstOrDefaultAsync();

                        //if (totalScore < minimumPassingGrade)
                        //{
                        //    //REMEDIAL MAX CUMA 3 KALO LEBIH DARI 3 DAH DI PECAT TUH ORANG
                        //    if (enrollLearnings.RemedialLevel < 3)
                        //    {
                        //        enrollLearnings.RemedialLevel++;
                        //        InboxRemedial(employeeId);
                        //    }
                        //    else
                        //    {
                        //        enrollLearnings.IsPassed = false;
                        //    }
                        //}
                        //else
                        //{
                        //    enrollLearnings.IsPassed = true;
                        //    var setupModule = await (from sm in this.DB.SetupModules
                        //                             join m in this.DB.Modules on sm.ModuleId equals m.ModuleId
                        //                             where sm.SetupModuleId == taskAnswers.SetupModuleId
                        //                             select new
                        //                             {
                        //                                 m.ModuleName,
                        //                                 sm.MinimumScore
                        //                             }).FirstOrDefaultAsync();

                        //    await this.CertificateGeneratorMan.GenerateCertificateAfterTrainingAsync(enrollLearnings.TrainingId.Value, employeeId);
                        //    await this.InsertLearningHistory(employeeId, taskAnswers);

                        //    //CHECK LULUS ATO ENGGA.. KALO LULUS DPT BADGES
                        //    if (totalScore >= setupModule.MinimumScore)
                        //    {
                        //        var employeeBadges = await this.InsertEmployeeBadges(taskAnswers.SetupModuleId.GetValueOrDefault(), employeeId, totalPoint);
                        //        if (employeeBadges != null)
                        //        {
                        //            DB.AddRange(employeeBadges);
                        //        }
                        //    }
                        //}
                    }
                }
                //SETUP MODULE
                else if (taskAnswers.SetupModuleId != null)
                {
                    var enrollLearnings = await this.EnrollLearning(taskAnswers, employeeId, totalPoint);
                    var MinimumScore = await (from sm in this.DB.SetupModules
                                              join m in this.DB.Modules on sm.ModuleId equals m.ModuleId
                                              where sm.SetupModuleId == taskAnswers.SetupModuleId
                                              select sm.MinimumScore).FirstOrDefaultAsync();
                    //CHECK LULUS ATO ENGGA.. KALO LULUS DPT BADGES
                    if (totalScore < MinimumScore)
                    {
                        enrollLearnings.IsPassed = false;
                    }
                    else
                    {
                        //enrollLearnings.IsPassed = true; //ini harus di kirim training id dari banner
                        var employeeBadges = await this.InsertEmployeeBadges(taskAnswers.SetupModuleId.GetValueOrDefault(), employeeId, totalPoint);
                        if (employeeBadges != null)
                        {
                            DB.AddRange(employeeBadges);
                        }
                    }
                    await this.InsertLearningHistory(employeeId, taskAnswers);
                    await DB.SaveChangesAsync();
                }
                //POPQUIZ
                else
                {
                    //MASUKIN POINT KE DB

                    var popQuizName = await this.DB.PopQuizzes.Where(Q => Q.PopQuizId == taskAnswers.PopQuizId).Select(Q => Q.PopQuizName).FirstOrDefaultAsync();
                    //var data = new EmployeePointHistories
                    //{
                    //    EmployeeId = employeeId,
                    //    RewardPointTypeId = 1,
                    //    PointTransactionTypeId = 1,
                    //    Point = totalPoint
                    //};
                    //this.DB.Add(data);
                    await this.InsertLearningHistory(employeeId, taskAnswers);
                    await InsertEmployeePoint(totalPoint, employeeId);
                    await DB.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception x)
            {
                if (x.InnerException.Message.Contains("UNIQUE"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> FinishModule(int setupModuleId, string employeeId, int trainingId){
            var data = await DB.EnrollLearningTimes.Include(x=> x.EnrollLearning).Where(x=> x.EnrollLearning.TrainingId == trainingId).Where(x=> x.SetupModuleId == setupModuleId).Where(x=> x.EnrollLearning.EmployeeId == employeeId).FirstOrDefaultAsync();
            if (data != null){
                return false;
            }

            data.EndTime = DateTime.UtcNow.AddHours(7);
            return true;
        }

        public async Task<(int, int)> InsertTaskAnswerDetail(TaskAnswerInsertModel TaskAnswers, TaskAnswerDetailModel Insert, string employeeId, int totalPoint, int totalScore, int getTaskAnswerId)
        {
            try
            {
                var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();

                var insertAnswerDetail = new List<TaskAnswerDetails>();

                totalPoint = totalPoint + Insert.Point;
                totalScore = totalScore + Insert.Score;

                //if (Insert.Attempts == null || Insert.Attempts == 0)
                //{
                //    if (DB.TaskAnswerDetails.Where(Q => Q.TaskAnswerId == getTaskAnswerId).ToList().Count() > 0)
                //    {
                //        var attempts = await DB.TaskAnswerDetails.Where(Q => Q.TaskAnswerId == getTaskAnswerId).Where(y => y.TaskId == TaskAnswers.Answer.FirstOrDefault().TaskId).OrderByDescending(y => y.Attempts).Select(y => y.Attempts).ToListAsync();

                //        if (attempts.Count() > 0)
                //        {
                //            Insert.Attempts = (int)attempts.Max() + 1;
                //        }
                //        else
                //        {
                //            Insert.Attempts = 1;
                //        }
                //    }
                //    else
                //    {
                //        Insert.Attempts = 1;
                //    }
                //}
                //else
                //{
                //    if (DB.TaskAnswerDetails.Where(Q => Q.TaskAnswerId == getTaskAnswerId).ToList().Count() > 0)
                //    {
                //        var attempts = await DB.TaskAnswerDetails.Where(Q => Q.TaskAnswerId == getTaskAnswerId).Where(y => y.TaskId == TaskAnswers.Answer.FirstOrDefault().TaskId).OrderByDescending(y => y.Attempts).Select(y => y.Attempts).ToListAsync();

                //        if (Insert.Attempts == 1 && (int)attempts.Max() + 1 > 1)
                //        {
                //            Insert.Attempts = (int)attempts.Max() + 1;
                //        }
                //    }
                //}

                switch (Insert.QuestionTypeId)
                {
                    case 11:
                        var insertData = new TaskAnswerDetails
                        {
                            TaskAnswerId = getTaskAnswerId,
                            TaskId = Insert.TaskId,
                            Answer = Insert.Answer,
                            Attempts = Insert.Attempts,
                            Score = Insert.Score,
                            Point = Insert.Point,
                            IsTrue = Insert.IsTrue.GetValueOrDefault(),
                            IsChecked = Insert.IsCheck.GetValueOrDefault(),
                            CreatedAt = dateNow,
                        };

                        FileContent file;
                        if (Insert.File.Base64String != null)
                        {
                            file = new FileContent
                            {
                                Base64 = Insert.File.Base64String,
                                ContentType = Insert.File.ContextType,
                                FileName = Insert.File.FileName,
                            };

                            var guid = await FileMan.UploadFileFromBase64(file);

                            insertData.AnswerBlobId = guid;

                        }

                        this.DB.Add(insertData);
                        break;

                    case 12:
                    case 9:
                    case 8:
                    case 2:
                    case 5:
                        //Detail
                        var detail = new TaskAnswerDetails
                        {
                            TaskAnswerId = getTaskAnswerId,
                            TaskId = Insert.TaskId,
                            Answer = Insert.Answer,
                            Attempts = Insert.Attempts,
                            Score = Insert.Score,
                            Point = Insert.Point,
                            IsTrue = Insert.IsTrue.GetValueOrDefault(),
                            IsChecked = Insert.IsCheck.GetValueOrDefault(),
                            CreatedAt = dateNow,
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
                            TaskAnswerId = getTaskAnswerId,
                            TaskId = Insert.TaskId,
                            Answer = Insert.Answer,
                            Score = Insert.Score,
                            Attempts = Insert.Attempts,
                            Point = Insert.Point,
                            IsTrue = Insert.IsTrue.GetValueOrDefault(),
                            IsChecked = Insert.IsCheck.GetValueOrDefault(),
                            CreatedAt = dateNow,
                        }
                        );
                        break;
                }
                await DB.SaveChangesAsync();
                return (totalPoint, totalScore);
            }
            catch(Exception x)
            {
                if (x.InnerException.Message.Contains("UNIQUE"))
                {
                    return (totalPoint, totalScore);
                }
                else
                {
                    return (totalPoint, totalScore);
                }
            }
        }

        //API UNTUK MENAMBAHKAN BAGDES SETIAP KALI MENYELESAIKAN MODULE DAN LULUS
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

            //var topicid = check.Select(Q => Q.TopicId).ToList();
            //var ebadgeid = check.Select(Q => Q.EbadgeId).ToList();

            var badges = await (from t in this.DB.Topics
                                join mtm in this.DB.ModuleTopicMappings on t.TopicId equals mtm.TopicId
                                join sm in this.DB.SetupModules on mtm.ModuleId equals sm.ModuleId
                                where sm.SetupModuleId == setupModuleId
                                //&& topicid.Contains(t.TopicId) == false && ebadgeid.Contains(t.EbadgeId.GetValueOrDefault()) == false
                                select new EmployeeBadgesViewModel
                                {
                                    EmployeeId = employeeId,
                                    TopicId = t.TopicId,
                                    EbadgeId = t.EbadgeId.GetValueOrDefault()
                                }).ToListAsync();

            foreach (var badge in check)
            {
                var existed = badges.Where(Q => Q.TopicId == badge.TopicId && Q.EbadgeId == badge.EbadgeId).FirstOrDefault();
                if(existed != null)
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

        //SIMPEN JAWABAN KE DB
        public async Task<(int, int)> InsertTaskAnswer(TaskAnswerInsertModel TaskAnswers, string employeeId, int totalPoint, int totalScore)
        {
            var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();

            var getEnrollLearning = await (from el in DB.EnrollLearnings
                                           where el.TrainingId == TaskAnswers.TrainingId && el.EmployeeId == employeeId
                                           select el).FirstOrDefaultAsync();

            //ini untuk handle banner training id
            if (TaskAnswers.TrainingId == null)
            {
                var TrainingTmp = (from s in DB.SetupModules
                                   join c in DB.Courses on s.CourseId equals c.CourseId
                                   join t in DB.Trainings on c.CourseId equals t.CourseId
                                   where s.SetupModuleId == TaskAnswers.SetupModuleId && s.IsDeleted == false && t.IsDeleted == false
                                   select t).OrderBy(Q => Q.Batch).AsQueryable();

                TaskAnswers.TrainingId = TrainingTmp.Select(Q => Q.TrainingId).FirstOrDefault();
            }

            var isExist = await this.DB.TaskAnswers
                .Where(Q => Q.EmployeeId == employeeId && Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.TrainingId == TaskAnswers.TrainingId)
                .AnyAsync();

            if(!isExist)
            {
                var insertTaskAnswer = new TaskAnswers
                {
                    EmployeeId = employeeId,
                    SetupModuleId = TaskAnswers.SetupModuleId,
                    PopQuizId = TaskAnswers.PopQuizId,
                    TrainingId = TaskAnswers.TrainingId,
                    CreatedAt = dateNow,
                };
                this.DB.TaskAnswers.Add(insertTaskAnswer);
                await DB.SaveChangesAsync();
            }

            var getTaskAnswerId = await this.DB.TaskAnswers
                .Where(Q => Q.EmployeeId == employeeId && Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.TrainingId == TaskAnswers.TrainingId)
                .Select(Q => Q.TaskAnswerId).FirstOrDefaultAsync();

            //manggil method insert task answer details

            foreach (var Insert in TaskAnswers.Answer)
            {
                (totalPoint, totalScore) = await this.InsertTaskAnswerDetail(TaskAnswers, Insert, employeeId, totalPoint, totalScore, getTaskAnswerId);
            }

            var checkIsCompleteData = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TaskAnswerId == getTaskAnswerId).AsQueryable().ToListAsync();
            var checkIsnotCompleteData = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TaskAnswerId == getTaskAnswerId).Where(x => x.IsChecked == true).AsQueryable().ToListAsync();
            if (checkIsCompleteData.Count == checkIsnotCompleteData.Count)
            {
                var setupConfig = await this
                        .DB
                        .SetupModules
                        .Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.IsDeleted == false).FirstOrDefaultAsync();

                var nilaiTaskList = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TaskAnswerId == getTaskAnswerId).OrderBy(x => x.Attempts).AsQueryable().ToListAsync();
                var jumlahAttempt = nilaiTaskList.OrderByDescending(x => x.Attempts).Select(x => x.Attempts).First();
                var jumlahTasks = DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TaskAnswerId == getTaskAnswerId).Select(x => x.TaskId).Distinct().Count();

                var getCourseId = await (from cr in DB.Trainings
                                         where cr.TrainingId == TaskAnswers.TrainingId && cr.IsDeleted == false
                                         select cr.CourseId).FirstOrDefaultAsync();

                //var getdata = await (from sm in DB.SetupModules
                //                          join t in DB.Trainings on sm.CourseId equals t.CourseId
                //                          where sm.SetupModuleId == TaskAnswers.SetupModuleId && t.IsDeleted == false
                //                          select new
                //                          {
                //                              t.CourseId,
                //                              t.TrainingId
                //                          }).FirstOrDefaultAsync();

                var getModuleId = await (from md in DB.SetupModules
                                         where md.SetupModuleId == TaskAnswers.SetupModuleId && md.IsDeleted == false
                                         select md.ModuleId).FirstOrDefaultAsync();

                var listNilaiTempObj = new List<TaskScoreTempEmpVM>();

                if (setupConfig.EnumRemedialOption.Trim() != "No Need")
                {
                    var finalScore = 0;
                    if (setupConfig.EnumScoringMethod.Trim() == "Average")
                    {
                        for (int i = 1; i <= jumlahAttempt; i++)
                        {
                            var nilaiTmp = new TaskScoreTempEmpVM();
                            nilaiTmp.EmployeeGUID = employeeId;
                            if (nilaiTaskList.Where(x => x.Attempts == i).Sum(x => x.Score) != null)
                            {
                                nilaiTmp.Score = nilaiTaskList.Where(x => x.Attempts == i).Sum(x => x.Score).Value;
                            }
                            else
                            {
                                nilaiTmp.Score = 0;
                            }

                            listNilaiTempObj.Add(nilaiTmp);
                        }
                        finalScore = (finalScore + listNilaiTempObj.Select(Q => Q.Score).Sum()) / listNilaiTempObj.Count;
                        //add to tb final score
                        var insertData = new FinalScores
                        {
                            EmployeeId = employeeId,
                            SetupModuleId = TaskAnswers.SetupModuleId,
                            TrainingId = TaskAnswers.TrainingId,
                            //TrainingId = getdata.TrainingId,
                            ModuleId = getModuleId,
                            CourseId = getCourseId,
                            //CourseId = getdata.CourseId,
                            FinalScore = (float)finalScore,
                            CreatedAt = dateNow,
                        };

                        if (finalScore < setupConfig.MinimumScore)
                        {
                            insertData.PassedStatus = false;
                            //tidak lulus
                            //push notif
                            //passing status kelulusan
                        }
                        else
                        {
                            insertData.PassedStatus = true;
                            //lulus
                            //passing status kelulusan
                        }
                        DB.FinalScores.Add(insertData);
                    }
                    else if (setupConfig.EnumScoringMethod.Trim() == "Highest")
                    {
                        for (int i = 1; i <= jumlahAttempt; i++)
                        {
                            var nilaiTmp = new TaskScoreTempEmpVM();
                            nilaiTmp.EmployeeGUID = employeeId;
                            if (nilaiTaskList.Where(x => x.Attempts == i).Sum(x => x.Score) != null)
                            {
                                nilaiTmp.Score = nilaiTaskList.Where(x => x.Attempts == i).Sum(x => x.Score).Value;
                            }
                            else
                            {
                                nilaiTmp.Score = 0;
                            }

                            listNilaiTempObj.Add(nilaiTmp);
                        }
                        finalScore = finalScore + listNilaiTempObj.Select(Q => Q.Score).Max();
                        //add to tb final score
                        var insertData = new FinalScores
                        {
                            EmployeeId = employeeId,
                            SetupModuleId = TaskAnswers.SetupModuleId,
                            TrainingId = TaskAnswers.TrainingId,
                            //TrainingId = getdata.TrainingId,
                            ModuleId = getModuleId,
                            CourseId = getCourseId,
                            //CourseId = getdata.CourseId,
                            FinalScore = (float)finalScore,
                            CreatedAt = dateNow,
                        };

                        if (finalScore < setupConfig.MinimumScore)
                        {
                            insertData.PassedStatus = false;
                            //tidak lulus
                            //push notif
                            //passing status kelulusan
                        }
                        else
                        {
                            insertData.PassedStatus = true;
                            //lulus
                            //passing status kelulusan
                        }
                        DB.FinalScores.Add(insertData);
                    }
                    else
                    {
                        for (int i = 1; i <= jumlahAttempt; i++)
                        {
                            var nilaiTmp = new TaskScoreTempEmpVM();
                            nilaiTmp.EmployeeGUID = employeeId;
                            if (nilaiTaskList.Where(x => x.Attempts == i).Sum(x => x.Score) != null)
                            {
                                nilaiTmp.Score = nilaiTaskList.Where(x => x.Attempts == i).Sum(x => x.Score).Value;
                            }
                            else
                            {
                                nilaiTmp.Score = 0;
                            }

                            listNilaiTempObj.Add(nilaiTmp);
                        }
                        finalScore = finalScore + listNilaiTempObj.Select(Q => Q.Score).LastOrDefault();
                        //add to tb final score
                        var insertData = new FinalScores
                        {
                            EmployeeId = employeeId,
                            SetupModuleId = TaskAnswers.SetupModuleId,
                            TrainingId = TaskAnswers.TrainingId,
                            //TrainingId = getdata.TrainingId,
                            ModuleId = getModuleId,
                            CourseId = getCourseId,
                            //CourseId = getdata.CourseId,

                            FinalScore = (float)finalScore,
                            CreatedAt = dateNow,
                        };

                        if (finalScore < setupConfig.MinimumScore)
                        {
                            insertData.PassedStatus = false;
                            //tidak lulus
                            //push notif
                            //passing status kelulusan
                        }
                        else
                        {
                            insertData.PassedStatus = true;
                            //lulus
                            //passing status kelulusan
                        }
                        DB.FinalScores.Add(insertData);
                    }
                }
                else
                {
                    var finalScore = 0;
                    if (setupConfig.EnumScoringMethod.Trim() == "Average")
                    {
                        var nilaiTmp = new TaskScoreTempEmpVM();
                        nilaiTmp.EmployeeGUID = employeeId;
                        if (nilaiTaskList.Where(x => x.Attempts == 1).Sum(x => x.Score) != null)
                        {
                            nilaiTmp.Score = nilaiTaskList.Where(x => x.Attempts == 1).Sum(x => x.Score).Value;
                        }
                        else
                        {
                            nilaiTmp.Score = 0;
                        }

                        listNilaiTempObj.Add(nilaiTmp);
                        finalScore = (finalScore + listNilaiTempObj.Select(Q => Q.Score).Sum()) / listNilaiTempObj.Count;
                        //add to tb final score
                        var insertData = new FinalScores
                        {
                            EmployeeId = employeeId,
                            SetupModuleId = TaskAnswers.SetupModuleId,
                            TrainingId = TaskAnswers.TrainingId,
                            //TrainingId = getdata.TrainingId,

                            ModuleId = getModuleId,
                            CourseId = getCourseId,
                            //CourseId = getdata.CourseId,

                            FinalScore = (float)finalScore,
                            CreatedAt = dateNow,
                        };

                        if (finalScore < setupConfig.MinimumScore)
                        {
                            insertData.PassedStatus = false;
                            //tidak lulus
                            //push notif
                            //passing status kelulusan
                        }
                        else
                        {
                            insertData.PassedStatus = true;
                            //lulus
                            //passing status kelulusan
                        }
                        DB.FinalScores.Add(insertData);
                    }
                    else if (setupConfig.EnumScoringMethod.Trim() == "Highest")
                    {
                        var nilaiTmp = new TaskScoreTempEmpVM();
                        nilaiTmp.EmployeeGUID = employeeId;
                        if (nilaiTaskList.Where(x => x.Attempts == 1).Sum(x => x.Score) != null)
                        {
                            nilaiTmp.Score = nilaiTaskList.Where(x => x.Attempts == 1).Sum(x => x.Score).Value;
                        }
                        else
                        {
                            nilaiTmp.Score = 0;
                        }

                        listNilaiTempObj.Add(nilaiTmp);
                        finalScore = finalScore + listNilaiTempObj.Select(Q => Q.Score).Max();
                        //add to tb final score
                        var insertData = new FinalScores
                        {
                            EmployeeId = employeeId,
                            SetupModuleId = TaskAnswers.SetupModuleId,
                            TrainingId = TaskAnswers.TrainingId,
                            //TrainingId = getdata.TrainingId,
                            ModuleId = getModuleId,
                            CourseId = getCourseId,
                            //CourseId = getdata.CourseId,
                            FinalScore = (float)finalScore,
                            CreatedAt = dateNow,
                        };

                        if (finalScore < setupConfig.MinimumScore)
                        {
                            insertData.PassedStatus = false;
                            //tidak lulus
                            //push notif
                            //passing status kelulusan
                        }
                        else
                        {
                            insertData.PassedStatus = true;
                            //lulus
                            //passing status kelulusan
                        }
                        DB.FinalScores.Add(insertData);
                    }
                    else
                    {
                        var nilaiTmp = new TaskScoreTempEmpVM();
                        nilaiTmp.EmployeeGUID = employeeId;
                        if (nilaiTaskList.Where(x => x.Attempts == 1).Sum(x => x.Score) != null)
                        {
                            nilaiTmp.Score = nilaiTaskList.Where(x => x.Attempts == 1).Sum(x => x.Score).Value;
                        }
                        else
                        {
                            nilaiTmp.Score = 0;
                        }

                        listNilaiTempObj.Add(nilaiTmp);
                        finalScore = finalScore + listNilaiTempObj.Select(Q => Q.Score).LastOrDefault();
                        //add to tb final score
                        var insertData = new FinalScores
                        {
                            EmployeeId = employeeId,
                            SetupModuleId = TaskAnswers.SetupModuleId,
                            TrainingId = TaskAnswers.TrainingId,
                            //TrainingId = getdata.TrainingId,
                            ModuleId = getModuleId,
                            CourseId = getCourseId,
                            //CourseId = getdata.CourseId,
                            FinalScore = (float)finalScore,
                            CreatedAt = dateNow,
                        };

                        if (finalScore < setupConfig.MinimumScore)
                        {
                            insertData.PassedStatus = false;
                            //tidak lulus
                            //push notif
                            //passing status kelulusan
                        }
                        else
                        {
                            insertData.PassedStatus = true;
                            //lulus
                            //passing status kelulusan
                        }
                        DB.FinalScores.Add(insertData);
                    }

                }
                try
                {
                    await DB.SaveChangesAsync();
                    var GetTaskPassedStatus = await DB.FinalScores.Where(x => x.ModuleId == getModuleId && x.SetupModuleId == TaskAnswers.SetupModuleId && x.TrainingId == TaskAnswers.TrainingId && x.EmployeeId == employeeId).OrderByDescending(x => x.CreatedAt).Select(x => x.PassedStatus).FirstOrDefaultAsync();
                    if (GetTaskPassedStatus == false)
                    {
                        //push notif
                        var getModuleName = await this.DB.Modules.Where(Q => Q.ModuleId == getModuleId).Select(Q => Q.ModuleName).FirstOrDefaultAsync();
                        var getCourseName = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.IsDeleted == false).Include(Q => Q.Course).Where(Q => Q.CourseId == getCourseId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();
                        //var getCourseName = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.IsDeleted == false).Include(Q => Q.Course).Where(Q => Q.CourseId == getdata.CourseId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();

                        var groupPositionList = new List<string>();
                        var manPowerPositionList = new List<string>();
                        var PushNotificationMyLearnings = new VMPushNotificationAdd();
                        var PushNotificationDataMyTools = new VMPushNotificationDataAdd();
                        PushNotificationMyLearnings.Title = "Remedial";
                        PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getModuleName + " , Silahkan melakukan Remedial di Modul " + getModuleName + " tersebut. Terima Kasih";
                        if (setupConfig.EnumRemedialOption.Trim() != "No Need")
                        {
                            if (setupConfig.EnumRemedialOption.Trim() == "Limit")
                            {
                                var jmlAttempt = this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.SetupModuleId == TaskAnswers.SetupModuleId && Q.TaskAnswer.EmployeeId == employeeId && Q.TaskAnswer.TrainingId == TaskAnswers.TrainingId).Select(Q => Q.Attempts).ToList();
                                if (jmlAttempt.Count > 0)
                                {
                                    if (jmlAttempt.Max() == setupConfig.RemedialLimit)
                                    {
                                        PushNotificationMyLearnings.Title = "Tidak Lulus";
                                        PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getModuleName + ". Terima Kasih";
                                    }
                                }
                            }
                        }
                        else
                        {
                            PushNotificationMyLearnings.Title = "Tidak Lulus";
                            PushNotificationMyLearnings.Body = "Maaf, anda tidak lulus pada course " + getCourseName + "-" + getModuleName + ". Terima Kasih";
                        }
                        PushNotificationMyLearnings.SenderId = employeeId.ToLower();
                        PushNotificationMyLearnings.IsPublished = true;
                        PushNotificationMyLearnings.GroupPositions = groupPositionList;
                        PushNotificationMyLearnings.ManPowerPosition = manPowerPositionList;

                        PushNotificationMyLearnings.SpecifiedEmployeeId = new List<string>{
                        employeeId.ToLower()
                    };

                        PushNotificationDataMyTools.Category = "Remedial";
                        PushNotificationDataMyTools.DataID = TaskAnswers.TrainingId;
                        PushNotificationDataMyTools.DataSecondId = getCourseId;
                        //PushNotificationDataMyTools.DataSecondId = getdata.CourseId;

                        await this.PNService.CreatePushNotificationRemedialScores(PushNotificationMyLearnings, PushNotificationDataMyTools);

                        getEnrollLearning.IsPassed = null;
                        await DB.SaveChangesAsync();
                    }
                    return (totalPoint, totalScore);
                }
                catch(Exception x)
                {
                    if (x.InnerException.Message.Contains("UNIQUE"))
                    {
                        return (totalPoint, totalScore);
                    }
                    else
                    {
                        return (totalPoint, totalScore);
                    }
                }
            }

            return (totalPoint, totalScore);
        }

        //
        public async Task<EnrollLearnings> EnrollLearning(TaskAnswerInsertModel TaskAnswers, string employeeId, int totalPoint)
        {
            var enrollLearnings = new EnrollLearnings();
            if (TaskAnswers.TrainingId != null)
            {
                enrollLearnings = await this.DB.EnrollLearnings.Where(Q => Q.EmployeeId == employeeId && Q.TrainingId == TaskAnswers.TrainingId).FirstOrDefaultAsync();
            }
            else
            {
                //enrollLearnings = await this.DB.EnrollLearnings.Where(Q => Q.EmployeeId == employeeId && Q.SetupModuleId == TaskAnswers.SetupModuleId).FirstOrDefaultAsync();
                enrollLearnings = await this.DB.EnrollLearningTimes.Include(Q => Q.EnrollLearning).Where(Q => Q.EnrollLearning.EmployeeId == employeeId && Q.SetupModuleId == TaskAnswers.SetupModuleId).Select(Q => Q.EnrollLearning).FirstOrDefaultAsync();
            }
            var eltimes = await this.DB.EnrollLearningTimes.Where(Q => Q.SetupModuleId == TaskAnswers.SetupModuleId && Q.EnrollLearningId == enrollLearnings.EnrollLearningId).FirstOrDefaultAsync();

            eltimes.EndTime = DateTime.UtcNow.ToIndonesiaTimeZone();
            var employeeTime = eltimes.EndTime?.Subtract(eltimes.StartTime.Value).TotalMinutes;

            var maximalTime = await (from sm in this.DB.SetupModules
                                     join tp in this.DB.TimePoints on sm.TimePointId equals tp.TimePointId
                                     where sm.SetupModuleId == TaskAnswers.SetupModuleId
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
            //var data = new EmployeePointHistories
            //{
            //    EmployeeId = employeeId,
            //    RewardPointTypeId = 1,
            //    PointTransactionTypeId = 1,
            //    Point = totalPoint
            //};
            //this.DB.Add(data);

            ////Nambahin exp dan point
            //var employee = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).FirstOrDefaultAsync();
            //employee.EmployeeExperience = employee.EmployeeExperience + totalPoint;
            //employee.LearningPoint = employee.LearningPoint + totalPoint;

            await InsertEmployeePoint(totalPoint, employeeId);

            return enrollLearnings;
        }

        public async Task InsertLearningHistory(string employeeId, TaskAnswerInsertModel taskAnswers)
        {
            var name = "";

            //if user enroll training done
            if (taskAnswers.TrainingId != null)
            {
                name = await (from c in this.DB.Courses
                              join t in this.DB.Trainings on c.CourseId equals t.CourseId
                              where t.TrainingId == taskAnswers.TrainingId
                              select c.CourseName).FirstOrDefaultAsync();

                var isExisted = await this.DB.LearningHistories
                    .Where(Q => Q.EmployeeId == employeeId && Q.TrainingId == taskAnswers.TrainingId).AnyAsync();

                if (!isExisted)
                {
                    DB.Add(new LearningHistories
                    {
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        EmployeeId = employeeId,
                        TrainingId = taskAnswers.TrainingId,
                        SetupModuleId = null,
                        PopQuizId = null,
                        Name = name
                    });
                }
            }

            //if user enroll module done
            else if (taskAnswers.SetupModuleId != null)
            {
                name = await (from sm in this.DB.SetupModules
                              join m in this.DB.Modules on sm.ModuleId equals m.ModuleId
                              where sm.SetupModuleId == taskAnswers.SetupModuleId
                              select m.ModuleName).FirstOrDefaultAsync();

                DB.Add(new LearningHistories
                {
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    EmployeeId = employeeId,
                    TrainingId = null,
                    SetupModuleId = taskAnswers.SetupModuleId,
                    PopQuizId = null,
                    Name = name
                });
            }

            //if user enroll pop quiz done
            else
            {
                name = await this.DB.PopQuizzes
                    .Where(Q => Q.PopQuizId == taskAnswers.PopQuizId)
                    .Select(Q => Q.PopQuizName)
                    .FirstOrDefaultAsync();

                DB.Add(new LearningHistories
                {
                    EmployeeId = employeeId,
                    TrainingId = null,
                    SetupModuleId = null,
                    PopQuizId = taskAnswers.PopQuizId,
                    Name = name
                });
            }
        }

        //INBOX
        public void InboxRemedial(string employeeId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var remedial = new MobileInboxes
            {
                EmployeeId = employeeId,
                MobileInboxTypeId = 6,
                Title = "Remedial",
                Message = "Maaf, Anda tidak lulus. Silahkan melakukan Remedial pada Module yang ada. Terima kasih.",
                CreatedAt = thisDate,
                CreatedBy = "SYSTEM",
                IsRead = false
            };
            this.DB.MobileInboxes.Add(remedial);
        }

        //CHECK JUMLAH MODUL YANG SUDAH DI SELESAIKAN DAN YANG MASIH ADA
        public async Task<(bool, List<SetupModules>)> IsDone(TaskAnswerInsertModel taskAnswers, string employeeId, bool withRemedial)
        {
            //var allSetupModuleQuery = from t in this.DB.Trainings
            //                          join sm in this.DB.SetupModules on t.CourseId equals sm.CourseId
            //                          join el in this.DB.EnrollLearnings on t.TrainingId equals el.TrainingId
            //                          where t.TrainingId == taskAnswers.TrainingId && sm.IsDeleted == false
            //                          select sm;


            //var numberofModuleFinishQuery = from t in this.DB.Trainings
            //                                join el in this.DB.EnrollLearnings on t.TrainingId equals el.TrainingId
            //                                join elt in this.DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId
            //                                join sm in this.DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId
            //                                where t.TrainingId == taskAnswers.TrainingId && el.EmployeeId == employeeId && elt.EndTime != null
            //                                select sm;

            var allSetupModuleQuery = from t in this.DB.Trainings
                                      join el in this.DB.EnrollLearnings on t.TrainingId equals el.TrainingId
                                      join elt in this.DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId
                                      join sm in this.DB.SetupModules on elt.SetupModuleId equals sm.SetupModuleId
                                      where t.TrainingId == taskAnswers.TrainingId && el.EmployeeId == employeeId
                                      select new
                                      {
                                          sm,
                                          elt
                                      };

            var numberofModuleFinishQuery = allSetupModuleQuery.Where(Q => Q.elt.EndTime != null);

            if (!withRemedial)
            {
                //allSetupModuleQuery = allSetupModuleQuery.Where(Q => Q.sm.IsForRemedial == false);
                //numberofModuleFinishQuery = numberofModuleFinishQuery.Where(Q => Q.sm.IsForRemedial == false);
            }

            var allSetupModule = await allSetupModuleQuery.Select(Q => Q.sm).ToListAsync();
            var numberofModuleFinish = await numberofModuleFinishQuery.Select(Q => Q.sm.MinimumScore).ToListAsync();

            // INI minus 1 artinya sedang submit taskanswer yang terakhir
            // E.g : total 7 module, sudah selesai 6, mau submit ke 7
            //if (numberofModuleFinish.Count() != allSetupModule.Count())
            //{
            //    return (false, null);
            //}
            //return (true, allSetupModule);

            if (numberofModuleFinish.Count() == allSetupModule.Count())
            {
                return (true, allSetupModule);
            }
            return (false, null);

        }

        public async Task<bool> InsertEmployeePoint(int totalPoint, string employeeId)
        {
            var data = new EmployeePointHistories
            {
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                EmployeeId = employeeId,
                RewardPointTypeId = 1,
                PointTransactionTypeId = 1,
                Point = totalPoint
            };
            this.DB.Add(data);

            //Nambahin exp dan point
            var employee = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).FirstOrDefaultAsync();
            employee.EmployeeExperience = employee.EmployeeExperience + totalPoint;
            employee.LearningPoint = employee.LearningPoint + totalPoint;
            return true;
        }


    }
}
