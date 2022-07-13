using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class TopicService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;
        private readonly IFileStorageService FileMan;

        public TopicService(TalentContext talentContext, ClaimHelper claimHelper, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.HelperMan = claimHelper;
            this.FileMan = fileService;
        }

        /// <summary>
        /// get all topic before/after filter and sorting
        /// </summary>
        /// <param name="filter">query from the html to filter and sorting the data at the same time</param>
        /// <returns>return topic data after the filter and sorting</returns>
        public async Task<GridTopicModel> GetAllTopicsAsync(TopicGridFilter filter)
        {
            var query = this.DB.Topics.AsNoTracking();

            var totalData = await query.CountAsync();

            if (filter.Ebadge != 0)
            {
                query = query.Where(Q => Q.EbadgeId == filter.Ebadge);
            }

            if (filter.MinPoint != null)
            {
                query = query.Where(Q => Q.TopicMinimumPoints == filter.MinPoint);
            }

            if (filter.DateStart != null && filter.DateEnd != null)
            {
                var newStartDate = filter.DateStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (string.IsNullOrEmpty(filter.TopicName) == false)
            {
                query = query.Where(Q => Q.TopicName.Contains(filter.TopicName));
            }

            var totalDataFilter = await query.CountAsync();

            switch (filter.SortBy)
            {
                case "TopicName":
                    query = query.OrderBy(Q => Q.TopicName);
                    break;
                case "TopicNameDesc":
                    query = query.OrderByDescending(Q => Q.TopicName);
                    break;
                case "TopicDescription":
                    query = query.OrderBy(Q => Q.TopicDescription);
                    break;
                case "TopicDescriptionDesc":
                    query = query.OrderByDescending(Q => Q.TopicDescription);
                    break;
                case "Ebadge":
                    query = query.OrderBy(Q => Q.EbadgeId);
                    break;
                case "EbadgeDesc":
                    query = query.OrderByDescending(Q => Q.EbadgeId);
                    break;
                case "MinPoint":
                    query = query.OrderBy(Q => Q.TopicMinimumPoints);
                    break;
                case "MinPointDesc":
                    query = query.OrderByDescending(Q => Q.TopicMinimumPoints);
                    break;
                case "CreatedAt":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAt":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            query = query.Skip((int)skipCount).Take(filter.PageSize);

            var topics = await query.Select(Q => new TopicViewModel
            {
                TopicId = Q.TopicId,
                TopicName = Q.TopicName,
                EBadge = Q.EbadgeId,
                TopicDesc = Q.TopicDescription,
                MinPoint = Q.TopicMinimumPoints,
                CreatedAt = Q.CreatedAt.ToString("dd/MM/yyyy"),
                UpdatedAt = Q.UpdatedAt.ToString("dd/MM/yyyy")
            }).ToListAsync();

            var grid = new GridTopicModel
            {
                Topics = topics,
                TotalData = totalData,
                TotalDataFilter = totalDataFilter
            };

            return grid;
        }

        /// <summary>
        /// insert topic to database
        /// </summary>
        /// <param name="model">create form topic</param>
        /// <returns></returns>
        public async Task<int> InsertTopicAsync(TopicCreateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var checkIsExist = await this.ValidateTopicByNameAsync(model.TopicName);

            if (checkIsExist == true)
            {
                return 0;
            }

            Guid? getUploadBlob = null;
            if (!String.IsNullOrEmpty(model.FileContent.Base64))
            {
                getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);
            }

            var topicForm = new Topics
            {
                TopicName = model.TopicName,
                TopicDescription = model.TopicDesc,
                EbadgeId = model.EBadge,
                TopicMinimumPoints = model.MinPoint,
                BlobId = getUploadBlob,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId()
            };

            this.DB.Topics.Add(topicForm);
            await this.DB.SaveChangesAsync();

            return topicForm.TopicId;
        }

        /// <summary>
        /// to validate wether the name has exist or not
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns>return true if the is another data with the same name</returns>
        public async Task<bool> ValidateTopicByNameAsync(string topicName)
        {
            var isExists = await this
                .DB
                .Topics
                .AsNoTracking()
                .AnyAsync(Q => Q.TopicName.ToUpper() == topicName.ToUpper());

            return isExists;
        }

        /// <summary>
        /// to get the topic data from the id
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns>return specific topic data by id</returns>
        public async Task<TopicViewDetailModel> GetTopicByIdAsync(int topicId)
        {
            var data = await (from t in DB.Topics
                              join b in DB.Blobs on t.BlobId equals b.BlobId
                              where t.TopicId == topicId
                              select new TopicViewDetailModel
                              {
                                  TopicId = t.TopicId,
                                  TopicName = t.TopicName,
                                  EBadgeId = t.EbadgeId,
                                  MinPoint = t.TopicMinimumPoints,
                                  TopicDesc = t.TopicDescription,
                                  BlobId = t.BlobId,
                                  BlobName = b.Name,
                                  BlobFileType = b.Mime,
                              }).FirstOrDefaultAsync();

            return data;
        }

        /// <summary>
        /// validate wether the updated name has an existing topic name
        /// if the topic name is not change but the content is changed then it still can be updated
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="topicId"></param>
        /// <returns>return true when the name is changed and another data has the same name</returns>
        public async Task<bool> ValidateUpdateTopicNameAsync(string topicName, int topicId)
        {
            var data = await this
                .DB
                .Topics
                .AsNoTracking()
                .Where(Q => Q.TopicId == topicId)
                .FirstOrDefaultAsync();

            bool isChanged = data.TopicName != topicName;

            bool isExist = await ValidateTopicByNameAsync(topicName);

            bool isTrue = isChanged == true && isExist == true;

            return isTrue;
        }

        /// <summary>
        /// update topic of the selected topic data
        /// </summary>
        /// <param name="updateModel"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTopicAsync(TopicUpdateModel updateModel, int topicId)
        {
            var updatedDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var topic = await this
                .DB
                .Topics
                .Where(Q => Q.TopicId == topicId)
                .FirstOrDefaultAsync();

            if (topic == null)
            {
                return false;
            }

            var check = await this.ValidateUpdateTopicNameAsync(updateModel.TopicName, topicId);

            if (check == true)
            {
                return false;
            }

            if (!String.IsNullOrEmpty(updateModel.FileContent.Base64))
            {
                var getUploadBlob = await this.FileMan.UploadFileFromBase64(updateModel.FileContent);
                topic.BlobId = getUploadBlob;
            }

            topic.TopicName = updateModel.TopicName;
            topic.TopicDescription = updateModel.TopicDesc;
            topic.TopicMinimumPoints = updateModel.MinPoint;
            topic.EbadgeId = updateModel.EBadge;
            topic.UpdatedAt = updatedDate;
            topic.UpdatedBy = this.HelperMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// delete selected topic data
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns>return true if successfully delete data</returns>
        public async Task<bool> DeleteTopicByIdAsync(int topicId)
        {
            var usedInCoach = await this.DB.CoachTopicMappings.Where(Q => Q.TopicId == topicId).AnyAsync();
            var usedInModule = await this.DB.ModuleTopicMappings.Where(Q => Q.TopicId == topicId).AnyAsync();
            var usedInEmployeeBadges = await this.DB.EmployeeBadges.Where(Q => Q.TopicId == topicId).AnyAsync();
            var usedInEmployeeInterests = await this.DB.EmployeeInterests.Where(Q => Q.TopicId == topicId).AnyAsync();

            if (usedInCoach || usedInModule || usedInEmployeeBadges || usedInEmployeeInterests)
            {
                return false;
            }

            var topic = await this
                .DB
                .Topics
                .Where(Q => Q.TopicId == topicId)
                .FirstOrDefaultAsync();

            this.DB.Topics.Remove(topic);

            var blob = await this
                .DB
                .Blobs
                .Where(Q => Q.BlobId == topic.BlobId)
                .FirstOrDefaultAsync();

            this.DB.Blobs.Remove(blob);

            try
            {
                await this.DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// use to get all topic data for coach option
        /// </summary>
        /// <returns>all option topic</returns>
        public async Task<List<TopicCoachOptionModel>> GetAllTopicOption()
        {
            var query = from t in DB.Topics
                        join e in DB.Ebadges on t.EbadgeId equals e.EbadgeId
                        select new
                        {
                            t.TopicId,
                            t.TopicName,
                            e.EbadgeId,
                            e.EbadgeName
                        };

            var data = await query.Select(Q => new TopicCoachOptionModel
            {
                TopicId = Q.TopicId,
                TopicName = Q.TopicName,
                EbadgeId = Q.EbadgeId,
                EbadgeName = Q.EbadgeName
            }).ToListAsync();

            return data;
        }

        public async Task<List<TopicEbadgeOptionModel>> GetEbadgeOption()
        {
            var data = await this
                .DB
                .Ebadges
                .AsNoTracking()
                .Select(Q => new TopicEbadgeOptionModel
                {
                    EbadgeId = Q.EbadgeId,
                    EbadgeName = Q.EbadgeName
                }).ToListAsync();

            return data;
        }
    }
}
