using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideReviewService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;

        public UserSideReviewService(TalentContext context, IFileStorageService fileService)
        {
            this.DB = context;
            this.FileMan = fileService;
        }

        public async Task<List<UserSideCoachReviewModel>> GetUserSideCoachReviewAsync(int trainingId)
        {
            var query = from t in DB.Trainings
                        join tmm in DB.TrainingModuleMappings on t.TrainingId equals tmm.TrainingId
                        join c in DB.Coaches on tmm.CoachId equals c.CoachId
                        join e in DB.Employees on c.EmployeeId equals e.EmployeeId
                        join b in DB.Blobs on e.BlobId equals b.BlobId into bNull
                        from b in bNull.DefaultIfEmpty()
                        where t.TrainingId == trainingId
                        select new
                        {
                            c.CoachId,
                            e.EmployeeName,
                            b.BlobId,
                            b.Mime
                        };

            var data1 = await query.Select(Q => new UserSideCoachReviewModel
            {
                CoachId = Q.CoachId,
                CoachName = Q.EmployeeName,
                CoachImageUrl = Q.BlobId == null ? "" : this.FileMan.GetFileAsync(Q.BlobId.ToString(), Q.Mime).Result
            }).ToListAsync();

            var data = data1.GroupBy(Q => Q.CoachId).Select(Q => Q.First()).ToList();

            return data;
        }

        public async Task InsertNewTrainingRatingAsync(UserSideTrainingReviewSubmitModel model, string employeeId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var dataTraining = await this.DB.Trainings.Where(Q => Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();

            var isExist = await this.DB.TrainingRatings.AsNoTracking().AnyAsync(Q => Q.TrainingId == model.TrainingId && Q.EmployeeId == employeeId);
            if (isExist)
            {
                var trainingRating = await this.DB.TrainingRatings.Where(Q => Q.TrainingId == model.TrainingId && Q.EmployeeId == employeeId).FirstOrDefaultAsync();

                trainingRating.RatingScore = model.RatingScore;
                trainingRating.Review = model.Review;
                trainingRating.UpdatedAt = thisDate;
            }

            else
            {
                var newTrainingReview = new TrainingRatings
                {
                    EmployeeId = employeeId,
                    TrainingId = model.TrainingId,
                    CourseId = dataTraining.CourseId,
                    RatingScore = model.RatingScore,
                    Review = model.Review,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    CreatedBy = employeeId,
                    IsDeleted = false
                };

                this.DB.TrainingRatings.Add(newTrainingReview);
            }

            await this.DB.SaveChangesAsync();
        }

        public async Task InsertNewCoachRatingAsync(UserSideCoachReviewSubmitModel model, string employeeId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var dataTraining = await this.DB.Trainings.Where(Q => Q.TrainingId == model.TrainingId).FirstOrDefaultAsync();

            var newCoachRating = new CoachRatings
            {
                EmployeeId = employeeId,
                TrainingId = model.TrainingId,
                CoachId = model.CoachId,
                RatingScore = model.RatingScore,
                Review = model.Review,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = employeeId,
                IsDeleted = false
            };
            this.DB.CoachRatings.Add(newCoachRating);

            var isTimePointExist = await this.DB.TimePoints.Where(Q => Q.PointTypeId == 4 && Q.Score == model.RatingScore).FirstOrDefaultAsync();

            var getCoachInformation = await (from c in DB.Coaches
                                             join e in DB.Employees on c.EmployeeId equals e.EmployeeId
                                             where c.CoachId == model.CoachId
                                             select new
                                             {
                                                 employee = e
                                             }).FirstOrDefaultAsync();
            if (isTimePointExist != null)
            {
                getCoachInformation.employee.TeachingPoint += isTimePointExist.Points;

                var newPointHistory = new EmployeePointHistories
                {
                    EmployeeId = getCoachInformation.employee.EmployeeId,
                    RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                    PointTransactionTypeId = (int)PointTransactionTypeEnum.Income,
                    Point = isTimePointExist.Points,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                };
                this.DB.EmployeePointHistories.Add(newPointHistory);
            }
            else
            {
                var newPointHistory = new EmployeePointHistories
                {
                    EmployeeId = getCoachInformation.employee.EmployeeId,
                    RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                    PointTransactionTypeId = (int)PointTransactionTypeEnum.Income,
                    Point = 0,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                };
                this.DB.EmployeePointHistories.Add(newPointHistory);
            }

            await this.DB.SaveChangesAsync();
        }
    }
}
