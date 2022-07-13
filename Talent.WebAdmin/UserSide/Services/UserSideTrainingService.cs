using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Models.LiveAssesment;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTrainingService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly PushNotificationService PNService;

        public UserSideTrainingService(TalentContext talentContext, IFileStorageService fileService,PushNotificationService pushNotificationService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
            this.PNService = pushNotificationService;
        }

        /// <summary>
        /// Get all training data by training id
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        public async Task<UserSideTrainingModel> GetTrainingById(int trainingId, string employeeId)
        {
            var queryTraining = await (from t in DB.Trainings
                                       join c in DB.Courses
                                       on t.CourseId equals c.CourseId
                                       where t.TrainingId == trainingId && t.IsDeleted == false
                                       select new
                                       {
                                           TrainingId = t.TrainingId,
                                           Batch = t.Batch,
                                           StartDate = t.StartDate,
                                           EndDate = t.EndDate,
                                           CourseId = c.CourseId,
                                           CourseName = c.CourseName,
                                           t.ApprovedAt
                                       })
                                       .FirstOrDefaultAsync();

            var queryEmployeeTrainings = await (from el in DB.EnrollLearnings
                                                join e in DB.Employees
                                                on el.EmployeeId equals e.EmployeeId

                                                join b in DB.Blobs
                                                on e.BlobId equals b.BlobId into ab
                                                from b in ab.DefaultIfEmpty()

                                                join epm in DB.EmployeePositionMappings
                                                on e.EmployeeId equals epm.EmployeeId into aepm
                                                from epm in aepm.DefaultIfEmpty()

                                                join p in DB.Positions
                                                on epm.PositionId equals p.PositionId into ap
                                                from p in ap.DefaultIfEmpty()

                                                where el.TrainingId == queryTraining.TrainingId && e.EmployeeId.ToLower() != employeeId.ToLower() && el.IsJoined == true
                                                select new
                                                {
                                                    EmployeeId = e.EmployeeId,
                                                    EmployeeName = e.EmployeeName,
                                                    Blob = b,
                                                    PositionName = p.PositionName
                                                })
                                                .ToListAsync();

            var dataEmployeeTrainings = new List<UserSideEmployeeTrainingModel>();
            foreach (var dataEmployeeTraining in queryEmployeeTrainings)
            {
                string imageUrlProfile = null;
                if (dataEmployeeTraining.Blob != null)
                {
                    imageUrlProfile = await this.FileService.GetFileAsync(dataEmployeeTraining.Blob.BlobId.ToString(), dataEmployeeTraining.Blob.Mime);
                }

                var newEmployeeTraining = new UserSideEmployeeTrainingModel
                {
                    EmployeeId = dataEmployeeTraining.EmployeeId,
                    EmployeeName = dataEmployeeTraining.EmployeeName,
                    ImageUrl = imageUrlProfile,
                    PositionName = dataEmployeeTraining.PositionName,
                    IsEvaluate = await NeedToBeEvaluated(trainingId, dataEmployeeTraining.EmployeeId)
                };
                dataEmployeeTrainings.Add(newEmployeeTraining);
            }

            var dataTraining = new UserSideTrainingModel
            {
                TrainingId = queryTraining.TrainingId,
                CourseId = queryTraining.CourseId,
                Batch = queryTraining.Batch,
                StartDate = queryTraining.StartDate,
                EndDate = queryTraining.EndDate,
                CourseName = queryTraining.CourseName,
                ApprovedAt = queryTraining.ApprovedAt,
                ListEmployeeTraining = dataEmployeeTrainings
            };

            return dataTraining;
        }

        public async Task<UserSideTrainingModel> GetTrainingByIdTeamCoachAsync(int trainingId, string employeeId, bool IsCoach)
        {
            var queryTraining = await (from t in DB.Trainings
                                       join c in DB.Courses
                                       on t.CourseId equals c.CourseId
                                       where t.TrainingId == trainingId && t.IsDeleted == false
                                       select new
                                       {
                                           TrainingId = t.TrainingId,
                                           Batch = t.Batch,
                                           StartDate = t.StartDate,
                                           EndDate = t.EndDate,
                                           CourseId = c.CourseId,
                                           CourseName = c.CourseName,
                                           t.ApprovedAt
                                       })
                                       .FirstOrDefaultAsync();
            
            var employeeIds =  new List<string>();

            var queryEmployeeTrainings = await (from el in DB.EnrollLearnings
                                                join e in DB.Employees
                                                on el.EmployeeId equals e.EmployeeId

                                                join o in DB.Outlets
                                                on e.OutletId equals o.OutletId into eo
                                                from o in eo.DefaultIfEmpty()

                                                join b in DB.Blobs
                                                on e.BlobId equals b.BlobId into ab
                                                from b in ab.DefaultIfEmpty()

                                                join epm in DB.EmployeePositionMappings
                                                on e.EmployeeId equals epm.EmployeeId into aepm
                                                from epm in aepm.DefaultIfEmpty()

                                                join p in DB.Positions
                                                on epm.PositionId equals p.PositionId into ap
                                                from p in ap.DefaultIfEmpty()

                                                where el.TrainingId == queryTraining.TrainingId && e.EmployeeId.ToLower() != employeeId.ToLower() && el.IsJoined == true && e.IsDeleted == false
                                                select new
                                                {
                                                    EmployeeId = e.EmployeeId,
                                                    EmployeeName = e.EmployeeName,
                                                    Blob = b,
                                                    PositionName = p.PositionName,
                                                    OutletId = o.OutletId
                                                })
                                                .ToListAsync();

            if (!IsCoach)
            {
                var teamDetails = await DB.TeamDetails.Include(x => x.Team).Where(x => x.Team.TeamLeaderId == employeeId).ToListAsync();
                employeeIds = teamDetails.Select(x => x.EmployeeId).ToList();
            }
            else
            {
                var coachCategory = await DB.Coaches.Where(x => x.EmployeeId == employeeId).Select(x => x.Category).FirstOrDefaultAsync();
                if (coachCategory.ToLower() == "dealer")
                {
                    var coachDealer = await DB.Employees.Where(x => x.EmployeeId == employeeId).Include(x => x.Outlet).Select(x => x.Outlet.DealerId).FirstOrDefaultAsync();
                    var coachoutletIds = await DB.Outlets.Where(x => x.DealerId == coachDealer).Select(x => x.OutletId).ToListAsync();
                    queryEmployeeTrainings = queryEmployeeTrainings.Where(x => coachoutletIds.Contains(x.OutletId)).ToList();
                }
                else if (coachCategory.ToLower() == "outlet")
                {
                    var outletCoachData = await DB.Employees.Where(x => x.EmployeeId == employeeId).Include(x => x.Outlet).Select(x => x.Outlet.OutletId).FirstOrDefaultAsync();
                    queryEmployeeTrainings = queryEmployeeTrainings.Where(x => outletCoachData.Contains(x.OutletId)).ToList();
                }
            }

            var dataEmployeeTrainings = new List<UserSideEmployeeTrainingModel>();
            foreach (var dataEmployeeTraining in queryEmployeeTrainings)
            {
                string imageUrlProfile = null;
                if (dataEmployeeTraining.Blob != null)
                {
                    imageUrlProfile = await this.FileService.GetFileAsync(dataEmployeeTraining.Blob.BlobId.ToString(), dataEmployeeTraining.Blob.Mime);
                }
                
                var newEmployeeTraining = new UserSideEmployeeTrainingModel
                {
                    EmployeeId = dataEmployeeTraining.EmployeeId,
                    EmployeeName = dataEmployeeTraining.EmployeeName,
                    ImageUrl = imageUrlProfile,
                    PositionName = dataEmployeeTraining.PositionName,
                    IsEvaluate = await NeedToBeEvaluated(trainingId, dataEmployeeTraining.EmployeeId)
                };
                dataEmployeeTrainings.Add(newEmployeeTraining);
            }

            var dataTraining = new UserSideTrainingModel
            {
                TrainingId = queryTraining.TrainingId,
                CourseId = queryTraining.CourseId,
                Batch = queryTraining.Batch,
                StartDate = queryTraining.StartDate,
                EndDate = queryTraining.EndDate,
                CourseName = queryTraining.CourseName,
                ApprovedAt = queryTraining.ApprovedAt,
                ListEmployeeTraining = dataEmployeeTrainings.OrderBy(q => q.EmployeeName).ToList()
            };

            return dataTraining;
        }

        public async Task<bool> NeedToBeEvaluated(int trainingId, string employeeId)
        {
            var isNeedToBeEvaluated = await (from ta in DB.TaskAnswers
                                             join tad in DB.TaskAnswerDetails on ta.TaskAnswerId equals tad.TaskAnswerId

                                             where
                                             ta.TrainingId == trainingId &&
                                             ta.EmployeeId.ToLower() == employeeId.ToLower() &&
                                             tad.IsChecked == false

                                             select new
                                             {
                                                 tad.TaskAnswerDetailId
                                             }).AnyAsync();
            return isNeedToBeEvaluated;
        }

        /// <summary>
        /// Get all training data.
        /// </summary>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        public async Task<UserSideTrainingViewModel> GetTraining()
        {
            var query = await (from t in DB.Trainings.AsNoTracking()
                               join c in DB.Courses.AsNoTracking()
                               on t.CourseId equals c.CourseId
                               join b in DB.Blobs
                               on c.BlobId equals b.BlobId
                               where t.IsDeleted == false
                               select new
                               {
                                   Course = c,
                                   Training = t,
                                   Blob = b
                               })
                               .ToListAsync();

            var dataCourseTrainings = new List<UserSideAllTrainingModel>();
            foreach (var dataCourseTraining in query)
            {
                string imageUrl = null;
                if (dataCourseTraining.Blob != null)
                {
                    imageUrl = await this.FileService.GetFileAsync(dataCourseTraining.Blob.BlobId.ToString(), dataCourseTraining.Blob.Mime);
                }
                var newCourseTraining = new UserSideAllTrainingModel
                {
                    TrainingId = dataCourseTraining.Training.TrainingId,
                    CourseName = dataCourseTraining.Course.CourseName,
                    Batch = dataCourseTraining.Training.Batch,
                    StartDate = dataCourseTraining.Training.StartDate,
                    EndDate = dataCourseTraining.Training.EndDate,
                    ImageUrl = imageUrl,
                    ApprovedAt = dataCourseTraining.Training.ApprovedAt
                };
                dataCourseTrainings.Add(newCourseTraining);
            }
            var dataTrainingList = new UserSideTrainingViewModel
            {
                ListCourseTraining = dataCourseTrainings
            };

            return dataTrainingList;
        }

        /// <summary>
        /// Get data question evaluate trainee data by training id & employee id.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<UserSideEmployeeTrainingModel> GetEvaluateTrainee(int trainingId, string employeeId)
        {
            // Get data employee
            var queryEmployee = await (from e in DB.Employees
                                       join b in DB.Blobs
                                       on e.BlobId equals b.BlobId into ab
                                       from b in ab.DefaultIfEmpty()

                                       where e.EmployeeId == employeeId
                                       select new
                                       {
                                           e.EmployeeId,
                                           e.EmployeeName,
                                           Blob = b,
                                       }).OrderBy(q => q.EmployeeName)
                                       .FirstOrDefaultAsync();

            var imageUrl = "";
            if (queryEmployee.Blob != null)
            {
                imageUrl = await this.FileService.GetFileAsync(queryEmployee.Blob.BlobId.ToString(), queryEmployee.Blob.Mime);
            }

            // Get List Question
            var queryQuestion = await (from ta in DB.TaskAnswers

                                       join tad in DB.TaskAnswerDetails
                                       on ta.TaskAnswerId equals tad.TaskAnswerId

                                       join b in DB.Blobs
                                       on tad.AnswerBlobId equals b.BlobId into ab
                                       from b in ab.DefaultIfEmpty()

                                       join t1 in DB.Tasks
                                       on tad.TaskId equals t1.TaskId

                                       join qt in DB.QuestionTypes
                                       on t1.QuestionTypeId equals qt.QuestionTypeId

                                       join tsat in DB.TaskShortAnswerTypes
                                       on t1.TaskId equals tsat.TaskId into atsat
                                       from tsat in atsat.DefaultIfEmpty()

                                       join tet in DB.TaskEssayTypes
                                       on t1.TaskId equals tet.TaskId into atet
                                       from tet in atet.DefaultIfEmpty()

                                       join tfut in DB.TaskFileUploadTypes
                                       on t1.TaskId equals tfut.TaskId into atfut
                                       from tfut in atfut.DefaultIfEmpty()

                                       where
                                       (t1.QuestionTypeId == Constant.QuestionTypes.QuestionShortAnswer ||
                                       t1.QuestionTypeId == Constant.QuestionTypes.QuestionEssay ||
                                       t1.QuestionTypeId == Constant.QuestionTypes.QuestionFileUpload) &&
                                       ta.TrainingId == trainingId && ta.EmployeeId == employeeId && tad.IsChecked == false

                                       orderby t1.TaskNumber ascending

                                       select new
                                       {
                                           t1.TaskNumber,
                                           qt.QuestionTypeId,
                                           qt.QuestionTypeName,
                                           tad.TaskAnswerDetailId,

                                           Question =
                                           !string.IsNullOrEmpty(tsat.Question) ? tsat.Question :
                                           !string.IsNullOrEmpty(tet.Question) ? tet.Question :
                                           !string.IsNullOrEmpty(tfut.Question) ? tfut.Question : "",

                                           TaskId =
                                           !string.IsNullOrEmpty(tsat.Question) ? tsat.TaskId :
                                           !string.IsNullOrEmpty(tet.Question) ? tet.TaskId :
                                           !string.IsNullOrEmpty(tfut.Question) ? tfut.TaskId : 0,

                                           Awnser = !string.IsNullOrEmpty(tad.Answer) ? tad.Answer : "",

                                           Blob = b
                                       })
                                       .ToListAsync();

            var dataEvaluateTrainee = new List<UserSideEvaluateTraineeModel>();
            foreach (var dataQueryEvaluateTrainee in queryQuestion)
            {
                var imageUrlQuestion = "";
                if (dataQueryEvaluateTrainee.Blob != null)
                {
                    imageUrlQuestion = await this.FileService.GetFileAsync(dataQueryEvaluateTrainee.Blob.BlobId.ToString(), dataQueryEvaluateTrainee.Blob.Mime);
                    var newEvaluateTraineee = new UserSideEvaluateTraineeModel
                    {
                        TaskAnswerDetailId = dataQueryEvaluateTrainee.TaskAnswerDetailId,
                        QuestionNumber = dataQueryEvaluateTrainee.TaskNumber,
                        QuestionTypeId = dataQueryEvaluateTrainee.QuestionTypeId,
                        QuestionTypeName = dataQueryEvaluateTrainee.QuestionTypeName,
                        Question = dataQueryEvaluateTrainee.Question,
                        TaskId = dataQueryEvaluateTrainee.TaskId,
                        Answer = !string.IsNullOrEmpty(dataQueryEvaluateTrainee.Awnser) ? dataQueryEvaluateTrainee.Awnser : imageUrlQuestion,
                        TaskAnswerType = dataQueryEvaluateTrainee.Blob.Mime,
                    };
                    dataEvaluateTrainee.Add(newEvaluateTraineee);
                }
                else
                {
                    var newEvaluateTraineee = new UserSideEvaluateTraineeModel
                    {
                        TaskAnswerDetailId = dataQueryEvaluateTrainee.TaskAnswerDetailId,
                        QuestionNumber = dataQueryEvaluateTrainee.TaskNumber,
                        QuestionTypeId = dataQueryEvaluateTrainee.QuestionTypeId,
                        QuestionTypeName = dataQueryEvaluateTrainee.QuestionTypeName,
                        Question = dataQueryEvaluateTrainee.Question,
                        TaskId = dataQueryEvaluateTrainee.TaskId,
                        Answer = !string.IsNullOrEmpty(dataQueryEvaluateTrainee.Awnser) ? dataQueryEvaluateTrainee.Awnser : imageUrlQuestion,
                        TaskAnswerType = dataQueryEvaluateTrainee.QuestionTypeName,
                    };
                    dataEvaluateTrainee.Add(newEvaluateTraineee);
                }
            }

            var dataQuestion = new UserSideEmployeeTrainingModel
            {
                EmployeeId = queryEmployee.EmployeeId,
                EmployeeName = queryEmployee.EmployeeName,
                ImageUrl = imageUrl,
                ListQuestion = dataEvaluateTrainee
            };

            return dataQuestion;
        }

        /// <summary>
        /// Update score by TaskAnswerDetailId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        public async Task<String> InsertScore(List<UserSideEvaluationModel> userSideEvaluations, string employeeId)
        {
            var trainingAnswerIds = userSideEvaluations.Select(Q => Q.TaskAnswerDetailId).ToList();
            var oldDataAnswer = await (
                from tad in DB.TaskAnswerDetails
                join ta in DB.TaskAnswers on tad.TaskAnswerId equals ta.TaskAnswerId
                where trainingAnswerIds.Contains(tad.TaskAnswerDetailId)
                select new TaskAnswerModelScoreTemp
                {
                    TaskAnswerDetails = tad,
                    TaskAnswerId = ta.TaskAnswerId,
                    SetupModuleId = ta.SetupModuleId,
                    TrainingId =  ta.TrainingId,
                    EmployeeId = ta.EmployeeId
                }
            ).ToListAsync();

            foreach (var question in oldDataAnswer)
            {
                var item = userSideEvaluations.Where(Q => Q.TaskAnswerDetailId == question.TaskAnswerDetails.TaskAnswerDetailId).First();
                question.TaskAnswerDetails.Score = item.Score;
                question.TaskAnswerDetails.IsChecked = true;
            }
            var setupModuleIds = oldDataAnswer.Select(Q => Q.SetupModuleId.Value).ToList();
            var coachId = await DB.Coaches.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.CoachId).FirstOrDefaultAsync();
            var trainingId = oldDataAnswer.Select(Q => Q.TrainingId.Value).FirstOrDefault();
            if (trainingId == 0) return "TrainingId is not found";
            await AlreadyEvaluated(setupModuleIds, coachId, trainingId);


            foreach(var taskAnswer in oldDataAnswer)
            {
                var checkIsCompleteData = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == taskAnswer.EmployeeId && Q.TaskAnswer.TaskAnswerId == taskAnswer.TaskAnswerId && Q.TaskAnswer.SetupModuleId == taskAnswer.SetupModuleId).AsQueryable().ToListAsync();
                var checkIsnotCompleteData = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == taskAnswer.EmployeeId && Q.TaskAnswer.TaskAnswerId == taskAnswer.TaskAnswerId && Q.TaskAnswer.SetupModuleId == taskAnswer.SetupModuleId).Where(x => x.IsChecked == true).AsQueryable().ToListAsync();
                if (checkIsCompleteData.Count == checkIsnotCompleteData.Count)
                {
                    await CalculateToFinalScores(taskAnswer, employeeId);
                }
            }
            return null;
        }

        public async Task CalculateToFinalScores(TaskAnswerModelScoreTemp oldDataAnswer, string employeeId)
        {
            var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();

            var getEnrollLearning = await (from el in DB.EnrollLearnings
                                           where el.TrainingId == oldDataAnswer.TrainingId && el.EmployeeId == oldDataAnswer.EmployeeId
                                           select el).FirstOrDefaultAsync();

            var setupConfig = await this
                    .DB
                    .SetupModules
                    .Where(Q => Q.SetupModuleId == oldDataAnswer.SetupModuleId && Q.IsDeleted == false).FirstOrDefaultAsync();

            var nilaiTaskList = await DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == oldDataAnswer.EmployeeId && Q.TaskAnswer.TaskAnswerId == oldDataAnswer.TaskAnswerId).OrderBy(x => x.Attempts).AsQueryable().ToListAsync();
            var jumlahAttempt = nilaiTaskList.OrderByDescending(x => x.Attempts).Select(x => x.Attempts).First();
            var jumlahTasks = DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.EmployeeId == oldDataAnswer.EmployeeId && Q.TaskAnswer.TaskAnswerId == oldDataAnswer.TaskAnswerId).Select(x => x.TaskId).Distinct().Count();

            var getCourseId = await(from cr in DB.Trainings
                                    where cr.TrainingId == oldDataAnswer.TrainingId && cr.IsDeleted == false
                                    select cr.CourseId).FirstOrDefaultAsync();

            var getModuleId = await(from md in DB.SetupModules
                                    where md.SetupModuleId == oldDataAnswer.SetupModuleId && md.IsDeleted == false
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
                        nilaiTmp.EmployeeGUID = oldDataAnswer.EmployeeId;
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
                        EmployeeId = oldDataAnswer.EmployeeId,
                        SetupModuleId = oldDataAnswer.SetupModuleId,
                        TrainingId = oldDataAnswer.TrainingId,
                        ModuleId = getModuleId,
                        CourseId = getCourseId,
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
                        nilaiTmp.EmployeeGUID = oldDataAnswer.EmployeeId;
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
                        EmployeeId = oldDataAnswer.EmployeeId,
                        SetupModuleId = oldDataAnswer.SetupModuleId,
                        TrainingId = oldDataAnswer.TrainingId,
                        ModuleId = getModuleId,
                        CourseId = getCourseId,
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
                        nilaiTmp.EmployeeGUID = oldDataAnswer.EmployeeId;
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
                        EmployeeId = oldDataAnswer.EmployeeId,
                        SetupModuleId = oldDataAnswer.SetupModuleId,
                        TrainingId = oldDataAnswer.TrainingId,
                        ModuleId = getModuleId,
                        CourseId = getCourseId,
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
                    nilaiTmp.EmployeeGUID = oldDataAnswer.EmployeeId;
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
                        EmployeeId = oldDataAnswer.EmployeeId,
                        SetupModuleId = oldDataAnswer.SetupModuleId,
                        TrainingId = oldDataAnswer.TrainingId,
                        ModuleId = getModuleId,
                        CourseId = getCourseId,
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
                    nilaiTmp.EmployeeGUID = oldDataAnswer.EmployeeId;
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
                        EmployeeId = oldDataAnswer.EmployeeId,
                        SetupModuleId = oldDataAnswer.SetupModuleId,
                        TrainingId = oldDataAnswer.TrainingId,
                        ModuleId = getModuleId,
                        CourseId = getCourseId,
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
                    nilaiTmp.EmployeeGUID = oldDataAnswer.EmployeeId;
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
                        EmployeeId = oldDataAnswer.EmployeeId,
                        SetupModuleId = oldDataAnswer.SetupModuleId,
                        TrainingId = oldDataAnswer.TrainingId,
                        ModuleId = getModuleId,
                        CourseId = getCourseId,
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
            await DB.SaveChangesAsync();
            var GetTaskPassedStatus = await DB.FinalScores.Where(x => x.ModuleId == getModuleId && x.SetupModuleId == oldDataAnswer.SetupModuleId && x.TrainingId == oldDataAnswer.TrainingId && x.EmployeeId == oldDataAnswer.EmployeeId).OrderByDescending(x => x.CreatedAt).Select(x => x.PassedStatus).FirstOrDefaultAsync();
            if (GetTaskPassedStatus == false)
            {
                //push notif
                var getModuleName = await this.DB.Modules.Where(Q => Q.ModuleId == getModuleId).Select(Q => Q.ModuleName).FirstOrDefaultAsync();
                var getCourseName = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == oldDataAnswer.SetupModuleId && Q.IsDeleted == false).Include(Q => Q.Course).Where(Q => Q.CourseId == getCourseId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();
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
                        var jmlAttempt = this.DB.TaskAnswerDetails.Include(Q => Q.TaskAnswer).Where(Q => Q.TaskAnswer.SetupModuleId == oldDataAnswer.SetupModuleId && Q.TaskAnswer.EmployeeId == oldDataAnswer.EmployeeId && Q.TaskAnswer.TrainingId == oldDataAnswer.TrainingId).Select(Q => Q.Attempts).ToList();
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
                PushNotificationMyLearnings.SenderId = employeeId;
                PushNotificationMyLearnings.IsPublished = true;
                PushNotificationMyLearnings.GroupPositions = groupPositionList;
                PushNotificationMyLearnings.ManPowerPosition = manPowerPositionList;

                PushNotificationMyLearnings.SpecifiedEmployeeId = new List<string>{
                        oldDataAnswer.EmployeeId.ToLower()
                    };

                PushNotificationDataMyTools.Category = "Remedial";
                PushNotificationDataMyTools.DataID = oldDataAnswer.TrainingId;
                PushNotificationDataMyTools.DataSecondId = getCourseId;

                await this.PNService.CreatePushNotificationRemedialScores(PushNotificationMyLearnings, PushNotificationDataMyTools);

                getEnrollLearning.IsPassed = null;
                await DB.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get all training data by employee ID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>List of training data in <seealso cref="List{UserSideTrainingModel}"/> format.</returns>
        public async Task<UserSideTrainingViewModel> GetTrainingByEmployeeId(string employeeId)
        {

            var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone().Date;

            var query = from tr in DB.Trainings
                        join cou in DB.Courses on tr.CourseId equals cou.CourseId
                        join b in DB.Blobs on cou.BlobId equals b.BlobId
                        join tmm in DB.TrainingModuleMappings on tr.TrainingId equals tmm.TrainingId
                        join coa in DB.Coaches on tmm.CoachId equals coa.CoachId
                        where tr.IsDeleted == false && coa.EmployeeId == employeeId && tr.ApprovedAt != null && (tr.StartDate == null || (dateNow >= tr.StartDate))
                        select new
                        {
                            tr.TrainingId,
                            cou.CourseName,
                            tr.Batch,
                            tr.StartDate,
                            tr.EndDate,
                            b.BlobId,
                            b.Mime,
                            tr.ApprovedAt
                        };

            var data = await query.Select(Q => new UserSideAllTrainingModel
            {
                TrainingId = Q.TrainingId,
                CourseName = Q.CourseName,
                Batch = Q.Batch,
                StartDate = Q.StartDate,
                EndDate = Q.EndDate,
                ImageUrl = GenerateImageUrl(Q.BlobId, Q.Mime).Result,
                ApprovedAt = Q.ApprovedAt
            }).ToListAsync();

            data = data.GroupBy(Q => Q.TrainingId).Select(Q => Q.First()).OrderByDescending(Q => Q.ApprovedAt).ToList();

            var dataTrainingList = new UserSideTrainingViewModel
            {
                ListCourseTraining = data
            };

            return dataTrainingList;
        }

        /// <summary>
        /// generate image url
        /// </summary>
        /// <param name="BlobId"></param>
        /// <param name="BlobMime"></param>
        /// <returns></returns>
        private async Task<string> GenerateImageUrl(Guid? BlobId, string BlobMime)
        {
            if (BlobId == null)
            {
                return null;
            }

            return await this.FileService.GetFileAsync(BlobId.ToString(), BlobMime);
        }

        public async Task AlreadyEvaluated(List<int> setupModuleIds, int coachId, int trainingId)
        {
            var questionTypeIds = new List<int> { 6, 7, 11 };
            var listEvaluatePoint = new List<UserSideEvaluateCoachPoints>();

            foreach (var setupModuleId in setupModuleIds)
            {
                var alreadyEvaluated = await (
                    from ta in this.DB.TaskAnswers
                    join tad in this.DB.TaskAnswerDetails on ta.TaskAnswerId equals tad.TaskAnswerId
                    join t in this.DB.Tasks on tad.TaskId equals t.TaskId
                    where questionTypeIds.Contains(t.QuestionTypeId) && tad.IsChecked == true && ta.SetupModuleId == setupModuleId
                    select new
                    {
                        ta.SetupModule,
                        tad.TaskId,
                        tad.IsChecked
                    }
                ).AnyAsync();

                if (alreadyEvaluated == false)
                {
                    var dataPoint = await (
                        from tmm in this.DB.TrainingModuleMappings
                        join tp in this.DB.TimePoints on tmm.TimePointId equals tp.TimePointId
                        join c in this.DB.Coaches on tmm.CoachId equals c.CoachId
                        where tmm.TrainingId == trainingId && tmm.CoachId == coachId && tmm.SetupModuleId == setupModuleId
                        select new
                        {
                            tp.TimePointId,
                            tp.Points,
                            c.EmployeeId,
                        }
                    ).FirstOrDefaultAsync();

                    if (dataPoint != null)
                    {
                        var evaluatePoint = new UserSideEvaluateCoachPoints
                        {
                            Points = dataPoint.Points,
                            EmployeeId = dataPoint.EmployeeId
                        };

                        listEvaluatePoint.Add(evaluatePoint);
                    }
                }
            }

            if (listEvaluatePoint.Count > 0)
            {
                var listEmployeePointHistories = new List<EmployeePointHistories>();
                foreach (var evalPoint in listEvaluatePoint)
                {
                    var timePointHistories = new EmployeePointHistories
                    {
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        EmployeeId = evalPoint.EmployeeId,
                        Point = evalPoint.Points,
                        RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                        PointTransactionTypeId = (int)PointTransactionTypeEnum.Income
                    };

                    listEmployeePointHistories.Add(timePointHistories);
                }

                this.DB.EmployeePointHistories.AddRange(listEmployeePointHistories);

                var employee = await this.DB.Employees.Where(Q => Q.EmployeeId == listEvaluatePoint.First().EmployeeId).FirstOrDefaultAsync();

                var totalPoint = listEvaluatePoint.Sum(Q => Q.Points);

                employee.TeachingPoint += totalPoint;
            }

            await this.DB.SaveChangesAsync();
        }

    }
}