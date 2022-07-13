using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class CourseService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly FileService FileServiceMan;
        private readonly ApprovalService ApprovalMan;
        public CourseService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.FileServiceMan = fs;
            this.ApprovalMan = approvalService;
        }

        public string GetUserLogin()
        {
            try
            {
                return ContextMan.CookieClaims.EmployeeId;
            }
            catch
            {
                return "SYSTEM";
            }
        }

        public async Task<CourseViewModel> GetAllCourses()
        {
            var allCourses = await this.DB.Courses.Select(Q => new CourseModel
            {
                CourseId = Q.CourseId,
                ProgramTypeId = Q.ProgramTypeId,
                LevelId = Q.LevelId,
                CourseCategoryId = Q.CourseCategoryId,
                LearningTypeId = Q.LearningTypeId,
                BlobId = Q.BlobId,
                CourseName = Q.CourseName,
                CoursePrice = Q.CoursePrice.GetValueOrDefault(),
                CourseDescription = Q.CourseDescription,
                Others = Q.Others,
                IsRecommendedForYou = Q.IsRecommendedForYou.GetValueOrDefault(),
                IsPopular = Q.IsPopular.GetValueOrDefault(),
                //IsPublished = Q.IsPublished.GetValueOrDefault(),
                CreatedBy = Q.CreatedBy,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
                IsDeleted = Q.IsDeleted,
            }).Where(Q => Q.IsDeleted == false).ToListAsync();

            var totalItem = await this.DB.Courses.CountAsync();

            var result = new CourseViewModel
            {
                ListCourseModel = allCourses,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<CourseModel> GetCourseById(int id)
        {
            var result = await DB.Courses.AsNoTracking().Select(Q => new CourseModel
            {
                CourseId = Q.CourseId,
                ProgramTypeId = Q.ProgramTypeId,
                LevelId = Q.LevelId,
                CourseCategoryId = Q.CourseCategoryId,
                LearningTypeId = Q.LearningTypeId,
                BlobId = Q.BlobId,
                CourseName = Q.CourseName,
                CoursePrice = Q.CoursePrice.GetValueOrDefault(),
                CourseDescription = Q.CourseDescription,
                Others = Q.Others,
                IsRecommendedForYou = Q.IsRecommendedForYou.GetValueOrDefault(),
                IsPopular = Q.IsPopular.GetValueOrDefault(),
                //IsPublished = Q.IsPublished.GetValueOrDefault(),
                CreatedBy = Q.CreatedBy,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
                IsDeleted = Q.IsDeleted
            }).Where(Q => Q.IsDeleted == false && Q.CourseId == id).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new CourseModel();
            }

            return result;
        }

        public async Task<int> CreateCourse(CourseFormModel model)
        {
            var a = GetUserLogin();
            Courses createModel = new Courses
            {
                ProgramTypeId = model.ProgramTypeId,
                LevelId = model.LevelId,
                CourseCategoryId = model.CourseCategoryId,
                LearningTypeId = model.LearningTypeId,
                //BlobId = model.BlobId,

                CourseName = model.CourseName,
                CoursePrice = model.CoursePrice,
                CourseDescription = model.CourseDescription,
                Others = model.Others,
                IsRecommendedForYou = false,
                IsPopular = false,
                //IsPublished = false,

                CreatedBy = GetUserLogin(),
                UpdatedBy = GetUserLogin(),
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                IsDeleted = false,
            };

            if (model.ImageUpload != null)
            {
                if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);
                    createModel.BlobId = blobId;
                }
            }

            this.DB.Courses.Add(createModel);

            var newApproval = new ApprovalCreateFormModel
            {
                ContentName = model.CourseName,
                ContentCategory = ContentCategoryEnums.Course,
                ApprovalStatusId = model.ApprovalId,
                PageEnum = PageEnums.Course,
                ContentId = createModel.CourseId
            };

            var approval = await ApprovalMan.CreateNewApproval(newApproval);

            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                createModel.CourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }

            await this.DB.SaveChangesAsync();
            return createModel.CourseId;
        }

        public async Task<bool> UpdateCourse(CourseFormModel model)
        {
            var updateModel = await this.DB.Courses.FindAsync(model.CourseId);

            var updateSetupLearning = await this.DB
              .SetupLearning
              .Where(Q => Q.CourseId == updateModel.CourseId && Q.LearningName == updateModel.CourseName)
              .FirstOrDefaultAsync();

            if (updateModel == null)
            {
                return false;
            }

            Blobs blobs = null;

            if (updateModel.BlobId != null)
            {
                blobs = await this.DB.Blobs.Where(Q => Q.BlobId == updateModel.BlobId).FirstOrDefaultAsync();
            }

            if (model.ImageUpload != null)
            {
                if (string.IsNullOrEmpty(model.ImageUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.ImageUpload);

                    if (blobs != null)
                    {
                        await FileServiceMan.DeleteFileAsync(blobs.BlobId, blobs.Mime);
                    }

                    updateModel.BlobId = blobId;
                }
            }
            //else if (model.BlobId != null)
            //{
            //    if (model.BlobId == updateModel.BlobId)
            //    {
            //        updateModel.BlobId = model.BlobId;
            //    }
            //}

            if(updateSetupLearning != null)
            {
                updateSetupLearning.LearningName = model.CourseName;
            }

            updateModel.ProgramTypeId = model.ProgramTypeId;
            updateModel.LevelId = model.LevelId;
            updateModel.CourseCategoryId = model.CourseCategoryId;
            updateModel.LearningTypeId = model.LearningTypeId;
            //updateModel.BlobId = model.BlobId;

            updateModel.CourseName = model.CourseName;
            updateModel.CoursePrice = model.CoursePrice;
            updateModel.CourseDescription = model.CourseDescription;
            updateModel.Others = model.Others;
            //updateModel.IsRecommendedForYou = model.IsRecommendedForYou;
            //updateModel.IsPopular = model.IsPopular;
            //updateModel.IsPublished = model.IsPublished;

            var currentApproval = await DB.ApprovalToCourses.Where(Q => Q.CourseId == model.CourseId).FirstOrDefaultAsync();
            var newApproval = new ApprovalUpdateFormModel
            {
                ApprovalId = currentApproval.ApprovalId,
                PageEnum = PageEnums.Course,
                ApprovalStatusId = model.ApprovalId,
                ContentName = model.CourseName,
                ContentId = updateModel.CourseId,
                ContentCategory = ContentCategoryEnums.Course
            };
            var approval = await ApprovalMan.UpdateNewApproval(newApproval);
            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId) // submit
            {
                updateModel.CourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else
            {
                updateModel.CourseApprovedAt = null;
            }
            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateModel.UpdatedBy = GetUserLogin();
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            try
            {
                var deleteModel = await this.DB.Courses.FindAsync(id);

                if (deleteModel == null) // deleteModel.IsDeleted == true
                {
                    return false;
                }

                deleteModel.IsDeleted = true;
                deleteModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                deleteModel.UpdatedBy = GetUserLogin();

                var approvalsToCourse = await this.DB.ApprovalToCourses.Where(Q => Q.CourseId == id).FirstOrDefaultAsync();

                if (approvalsToCourse == null)
                {
                    return false;
                }

                this.DB.ApprovalToCourses.Remove(approvalsToCourse);

                var isDeleted = await this.ApprovalMan.DeleteApproval(approvalsToCourse.ApprovalId);

                if (isDeleted == false)
                {
                    return false;
                }

                // delete setup course

                var approvalId2 = await this.DB.ApprovalToSetupCourses.Where(Q => Q.CourseId == id).Select(Q => Q.ApprovalId).FirstOrDefaultAsync();

                if (approvalId2 != 0)
                {
                    var deleteInbox = await DB.Inboxes.Where(Q => Q.ApprovalId == approvalId2).ToListAsync();
                    if (deleteInbox.Count > 0)
                    {
                        this.DB.RemoveRange(deleteInbox);
                    }
                    var deleteModel2 = await this.DB.ApprovalToSetupCourses.FindAsync(approvalId2);
                    this.DB.ApprovalToSetupCourses.Remove(deleteModel2);

                    var deleteModel3 = await this.DB.Approvals.FindAsync(approvalsToCourse.ApprovalId);
                    this.DB.Approvals.Remove(deleteModel3);
                }

                var deleteSetupLearning = await DB.SetupLearning.Where(Q => Q.CourseId == id).ToAsyncEnumerable().ToList();
                if (deleteSetupLearning.Count > 0 && deleteSetupLearning != null)
                {
                    DB.SetupLearning.RemoveRange(deleteSetupLearning);
                }

                // delete release training

                var trainingToDelete = await this.DB.Trainings.AsNoTracking().Where(Q => Q.CourseId == id).ToListAsync();

                for (int i = 0; i < trainingToDelete.Count(); i++)
                {
                    var approvalId3 = await this.DB.ApprovalToTrainings.Where(Q => Q.TrainingId == trainingToDelete[i].TrainingId).Select(Q => Q.ApprovalId).FirstOrDefaultAsync();

                    if (approvalId3 != 0)
                    {
                        var deleteInbox = await DB.Inboxes.Where(Q => Q.ApprovalId == approvalId3).ToListAsync();
                        if (deleteInbox.Count > 0)
                        {
                            this.DB.RemoveRange(deleteInbox);
                        }
                        var deleteModel2 = await this.DB.ApprovalToTrainings.FindAsync(approvalId3);
                        this.DB.ApprovalToTrainings.Remove(deleteModel2);

                        var deleteModel3 = await this.DB.Approvals.FindAsync(approvalId3);
                        this.DB.Approvals.Remove(deleteModel3);
                    }

                    var update = await this.DB.Trainings.FindAsync(trainingToDelete[i].TrainingId);

                    if (update.Top5Course != 0)
                    {
                        update.Top5Course = 0;
                        var findTop5 = await this.DB.Trainings.Where(Q => Q.IsDeleted == false && Q.Top5Course != 0).Take(5).OrderBy(Q => Q.Top5Course).ToListAsync();
                        findTop5.Remove(update);
                        var currentPost = 5;
                        for (int a = 0; a < findTop5.Count; a++)
                        {
                            findTop5[a].Top5Course = currentPost;
                            currentPost--;
                        }
                    }

                    update.IsDeleted = true;
                    update.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    update.UpdatedBy = GetUserLogin();
                }

                await DB.SaveChangesAsync();
                return true;
            }

            catch
            {
                return false;
            }
        }
    }

    public class CourseJoinService
    {
        private readonly TalentContext DB;
        private readonly CourseService CourseMan;
        private readonly ProgramTypeService ProgramTypeMan;
        private readonly LevelService LevelMan;
        private readonly CourseCategoryService CourseCategoryMan;
        private readonly LearningTypeService LearningTypeMan;
        private readonly BlobService BlobMan;
        private readonly ApprovalStatusService ApprovalStatusMan;

        public CourseJoinService(TalentContext db, CourseService courseService, ProgramTypeService programTypeService, LevelService levelService, CourseCategoryService courseCategoryService, LearningTypeService learningTypeService, BlobService blobService, ApprovalStatusService approvalService)
        {
            this.DB = db;
            CourseMan = courseService;
            ProgramTypeMan = programTypeService;
            LevelMan = levelService;
            CourseCategoryMan = courseCategoryService;
            LearningTypeMan = learningTypeService;
            BlobMan = blobService;
            ApprovalStatusMan = approvalService;
        }

        public async Task<CourseJoinViewModel> GetAllCourses(CourseFilter filter)
        {
            var query = (
                from c in DB.Courses.AsNoTracking()
                join p in DB.ProgramTypes.AsNoTracking() on c.ProgramTypeId equals p.ProgramTypeId
                join lv in DB.Levels.AsNoTracking() on c.LevelId equals lv.LevelId
                join cc in DB.CourseCategories.AsNoTracking() on c.CourseCategoryId equals cc.CourseCategoryId
                join l in DB.LearningTypes.AsNoTracking() on c.LearningTypeId equals l.LearningTypeId
                join b in DB.Blobs.AsNoTracking() on c.BlobId equals b.BlobId
                join atc in DB.ApprovalToCourses.AsNoTracking() on c.CourseId equals atc.CourseId into latc
                from atc in latc.DefaultIfEmpty()
                join a in DB.Approvals on atc.ApprovalId equals a.ApprovalId into la
                from a in la.DefaultIfEmpty()
                join s in DB.ApprovalStatus on a.ApprovalStatusId equals s.ApprovalStatusId into ls
                from s in ls.DefaultIfEmpty()
                where c.IsDeleted == false
                select new CourseJoinModel
                {
                    CourseId = c.CourseId,
                    ProgramTypeId = c.ProgramTypeId,
                    LevelId = c.LevelId,
                    CourseCategoryId = c.CourseCategoryId,
                    LearningTypeId = c.LearningTypeId,
                    BlobId = c.BlobId,
                    ApprovalId = a.ApprovalStatusId,

                    ProgramTypeName = p.ProgramTypeName,
                    LevelName = lv.LevelName,
                    CourseCategoryName = cc.CourseCategoryName,
                    LearningName = l.LearningName,
                    Mime = b.Mime,
                    FileName = b.Name,
                    ApprovalName = c.CourseApprovedAt != null ? ApprovalStatusesEnum.Approve : s.ApprovalName ?? ApprovalStatusesEnum.Approve,

                    CourseName = c.CourseName,
                    CoursePrice = c.CoursePrice,
                    CourseDescription = c.CourseDescription,
                    Others = c.Others,
                    IsRecommendedForYou = c.IsRecommendedForYou,
                    IsPopular = c.IsPopular,
                    //IsPublished = c.IsPublished,

                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IsDeleted = c.IsDeleted
                });

            if (filter.StartDate != null && filter.EndDate != null)
            {

                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (string.IsNullOrEmpty(filter.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.ToLower().Contains(filter.CourseName.ToLower()));
            }

            if (string.IsNullOrEmpty(filter.ProgramTypeName) == false)
            {
                query = query.Where(Q => Q.ProgramTypeName.Equals(filter.ProgramTypeName));
            }

            if (string.IsNullOrEmpty(filter.LearningName) == false)
            {
                query = query.Where(Q => Q.LearningName.Equals(filter.LearningName));
            }

            if (string.IsNullOrEmpty(filter.CourseCategoryName) == false)
            {
                query = query.Where(Q => Q.CourseCategoryName.Equals(filter.CourseCategoryName));
            }

            if (string.IsNullOrEmpty(filter.Pricing) == false && filter.Pricing.Equals("Pay"))
            {
                query = query.Where(Q => Q.CoursePrice > 0);
            }

            if (string.IsNullOrEmpty(filter.Pricing) == false && filter.Pricing.Equals("Free"))
            {
                query = query.Where(Q => Q.CoursePrice == 0);
            }

            if (string.IsNullOrEmpty(filter.ApprovalName) == false)
            {
                query = query.Where(Q => Q.ApprovalName.Equals(filter.ApprovalName));
            }

            query = query.OrderByDescending(Q => Q.UpdatedAt);

            switch (filter.SortBy)
            {
                case "courseName":
                    query = query.OrderBy(Q => Q.CourseName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "courseNameDesc":
                    query = query.OrderByDescending(Q => Q.CourseName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "programType":
                    query = query.OrderBy(Q => Q.ProgramTypeName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "programTypeDesc":
                    query = query.OrderByDescending(Q => Q.ProgramTypeName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "learningType":
                    query = query.OrderBy(Q => Q.LearningName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "learningTypeDesc":
                    query = query.OrderByDescending(Q => Q.LearningName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "category":
                    query = query.OrderBy(Q => Q.CourseCategoryName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "categoryDesc":
                    query = query.OrderByDescending(Q => Q.CourseCategoryName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "pricing":
                    query = query.OrderBy(Q => Q.CoursePrice).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "pricingDesc":
                    query = query.OrderByDescending(Q => Q.CoursePrice).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "approvalStatus":
                    query = query.OrderBy(Q => Q.ApprovalName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "approvalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var resultQuery = await query.Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();
            var totalData = await query.CountAsync();

            var result = new CourseJoinViewModel
            {
                ListCourseJoinModel = resultQuery,
                TotalItem = totalData
            };

            return result;
        }

        public async Task<CourseJoinModel> GetCourseJoinById(int id)
        {
            var course = await CourseMan.GetCourseById(id);

            if (course.CourseName == null)
            {
                return new CourseJoinModel();
            }

            var programType = await ProgramTypeMan.GetProgramTypeById(course.ProgramTypeId);
            var level = await LevelMan.GetLevelById(course.LevelId);
            var courseCategory = await CourseCategoryMan.GetCourseCategoryById(course.CourseCategoryId);
            var learningType = await LearningTypeMan.GetLearningTypeById(course.LearningTypeId);
            var blob = await BlobMan.GetBlobById(course.BlobId);
            var approvalStatus = await ApprovalStatusMan.GetApprovalStatusById(course.ApprovalId);

            var result = new CourseJoinModel
            {
                CourseId = course.CourseId,

                ProgramTypeId = course.ProgramTypeId,
                LevelId = course.LevelId,
                CourseCategoryId = course.CourseCategoryId,
                LearningTypeId = course.LearningTypeId,
                BlobId = course.BlobId,
                ApprovalId = course.ApprovalId,

                ProgramTypeName = programType.ProgramTypeName,
                LevelName = level.LevelName,
                CourseCategoryName = courseCategory.CourseCategoryName,
                LearningName = learningType.LearningName,
                Mime = blob.Mime,
                FileName = blob.Name,
                ApprovalName = approvalStatus.ApprovalName,

                CourseName = course.CourseName,
                CoursePrice = course.CoursePrice,
                CourseDescription = course.CourseDescription,
                Others = course.Others,
                IsRecommendedForYou = course.IsRecommendedForYou,
                IsPopular = course.IsPopular,
                IsPublished = course.IsPublished,

                CreatedBy = course.CreatedBy,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt,
                IsDeleted = course.IsDeleted,
            };

            return result;
        }
    }
}