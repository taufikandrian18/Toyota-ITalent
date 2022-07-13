using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    /// <summary>
    /// Service class for providing various methods for interacting with news data.
    /// </summary>
    public class UserSideNewsService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideNewsService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        /// <summary>
        /// Get all news categories.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserSideNewsCategoryModel>> GetNewsCategories()
        {
            var newsCategories = await this.DB
                .NewsCategories
                .Select(Q => new UserSideNewsCategoryModel
                {
                    NewsCategoryId = Q.NewsCategoryId,
                    NewsCategoryName = Q.Name
                })
                .AsNoTracking()
                .ToListAsync();

            return newsCategories;
        }

        /// <summary>
        /// Get detail news based on news id, and get related news
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="itemPerPageRelatedNews"></param>
        /// <param name="pageIndexRelatedNews"></param>
        /// <returns></returns>
        public async Task<UserSideNewsDetailModel> GetDetailNews(int newsId, int itemPerPageRelatedNews, int pageIndexRelatedNews)
        {
            // Detail News
            var newsTemp = await NewsQueryable()
                .Where(Q => Q.NewsId == newsId)
                .FirstOrDefaultAsync();

            var news = new UserSideNewsModel
            {
                Author = newsTemp.Author,
                NewsCategoryId = newsTemp.NewsCategoryId,
                Description = newsTemp.Description,
                FileUrl = await FileService.GetFileAsync(newsTemp.FileBlobId, newsTemp.FileMime),
                IsDownloadable = newsTemp.IsDownloadable,
                ApprovedAt = newsTemp.ApprovedAt,
                IsDeleted = newsTemp.IsDeleted,
                Link = newsTemp.Link,
                NewsCategoryName = newsTemp.NewsCategoryName,
                NewsId = newsTemp.NewsId,
                ThumbnailUrl = await FileService.GetFileAsync(newsTemp.ThumbnailBlobId, newsTemp.ThumbnailMime),
                Title = newsTemp.Title,
                TotalView = newsTemp.TotalView,
                CreatedAt = newsTemp.CreatedAt,
                UpdatedAt = newsTemp.UpdateAt
            };

            // Related news based on category
            var relatedNewsTemp = await NewsQueryable()
                .OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt)
                .Where(Q => Q.NewsId != news.NewsId && Q.NewsCategoryId == news.NewsCategoryId)
                .Skip((pageIndexRelatedNews - 1) * itemPerPageRelatedNews)
                .Take(itemPerPageRelatedNews)
                .ToListAsync();

            var relatedNews = relatedNewsTemp
                .Select(async Q => new UserSideNewsModel
                {
                    Author = Q.Author,
                    NewsCategoryId = Q.NewsCategoryId,
                    Description = Q.Description,
                    FileUrl = await FileService.GetFileAsync(Q.FileBlobId, Q.FileMime),
                    IsDownloadable = Q.IsDownloadable,
                    ApprovedAt = Q.ApprovedAt,
                    IsDeleted = Q.IsDeleted,
                    Link = Q.Link,
                    NewsCategoryName = Q.NewsCategoryName,
                    NewsId = Q.NewsId,
                    ThumbnailUrl = await FileService.GetFileAsync(Q.ThumbnailBlobId, Q.ThumbnailMime),
                    Title = Q.Title,
                    TotalView = Q.TotalView,
                    CreatedAt = Q.CreatedAt,
                    UpdatedAt = Q.UpdateAt
                })
                .Select(Q => Q.Result)
                .ToList();

            var newsDetail = new UserSideNewsDetailModel()
            {
                News = news,
                RelatedNews = relatedNews
            };

            return newsDetail;
        }

        /// <summary>
        /// Get all popular news.
        /// </summary>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideNewsModel>> GetPopularNews(int itemPerPage, int pageIndex)
        {
            var newsTemp = await NewsQueryable()
                .OrderByDescending(Q => Q.TotalView)
                .Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();

            var news = newsTemp
                .Select(async Q => new UserSideNewsModel
                {
                    Author = Q.Author,
                    NewsCategoryId = Q.NewsCategoryId,
                    Description = Q.Description,
                    FileUrl = await FileService.GetFileAsync(Q.FileBlobId, Q.FileMime),
                    IsDownloadable = Q.IsDownloadable,
                    ApprovedAt = Q.ApprovedAt,
                    IsDeleted = Q.IsDeleted,
                    Link = Q.Link,
                    NewsCategoryName = Q.NewsCategoryName,
                    NewsId = Q.NewsId,
                    ThumbnailUrl = await FileService.GetFileAsync(Q.ThumbnailBlobId, Q.ThumbnailMime),
                    Title = Q.Title,
                    TotalView = Q.TotalView,
                    CreatedAt = Q.CreatedAt,
                    UpdatedAt = Q.UpdateAt
                })
                .Select(Q => Q.Result)
                .ToList();

            return news;
        }

        /// <summary>
        /// Query of news.
        /// </summary>
        /// <returns></returns>
        public IQueryable<UserSideNewsQueryModel> NewsQueryable()
        {
            var query = from n in this.DB.News.AsNoTracking()
                        join nc in this.DB.NewsCategories.AsNoTracking() on n.NewsCategoryId equals nc.NewsCategoryId
                        join atn in this.DB.ApprovalToNews.AsNoTracking() on n.NewsId equals atn.NewsId
                        join bf in this.DB.Blobs.AsNoTracking() on n.FileBlobId equals bf.BlobId
                        join bt in this.DB.Blobs.AsNoTracking() on n.ThumbnailBlobId equals bt.BlobId
                        where n.IsDeleted == false && atn.Approval.ApprovalStatus.ApprovalName == ApprovalStatusesEnum.Approve
                        select new UserSideNewsQueryModel
                        {
                            Author = n.Author,
                            NewsCategoryId = n.NewsCategoryId,
                            Description = n.Description,
                            FileBlobId = n.FileBlobId.ToString(),
                            FileMime = bf.Mime,
                            IsDownloadable = n.IsDownloadable,
                            ApprovedAt = n.ApprovedAt,
                            IsDeleted = n.IsDeleted,
                            Link = n.Link,
                            NewsCategoryName = nc.Name,
                            NewsId = n.NewsId,
                            ThumbnailBlobId = n.ThumbnailBlobId.ToString(),
                            ThumbnailMime = bt.Mime,
                            Title = n.Title,
                            TotalView = n.TotalView,
                            CreatedAt = n.CreatedAt,
                            UpdateAt = n.UpdatedAt
                        };

            return query;
        }

        /// <summary>
        /// Get all news based on filter.
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        public async Task<List<UserSideNewsModel>> GetNews(UserSideNewsFilterModel filterModel)
        {
            var query = NewsQueryable();

            // Filter by author.
            if (!string.IsNullOrEmpty(filterModel.Author))
            {
                query = query.Where(Q => Q.Author.ToLower().Contains(filterModel.Author.ToLower()));
            }

            // Filter by news category.
            if (!string.IsNullOrEmpty(filterModel.Category))
            {
                query = query.Where(Q => Q.NewsCategoryName.ToLower().Contains(filterModel.Category.ToLower()));
            }

            // Sorting
            switch (filterModel.SortBy)
            {
                case "DateAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "DateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "NameAsc":
                    query = query.OrderBy(Q => Q.Title);
                    break;
                case "NameDesc":
                    query = query.OrderByDescending(Q => Q.Title);
                    break;
                case "PopularAsc":
                    query = query.OrderBy(Q => Q.TotalView);
                    break;
                case "PopularDesc":
                    query = query.OrderByDescending(Q => Q.TotalView);
                    break;
                case "":
                    {
                        query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt);
                        break;
                    }
                case null:
                    {
                        query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt);
                        break;
                    }

                default:
                    {
                        query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt);
                        break;
                    }

            }

            var newsTemp = await query
                .Skip((filterModel.PageIndex - 1) * filterModel.ItemPerPage)
                .Take(filterModel.ItemPerPage)
                .ToListAsync();

            var news = newsTemp
                .Select(async Q => new UserSideNewsModel
                {
                    Author = Q.Author,
                    NewsCategoryId = Q.NewsCategoryId,
                    Description = Q.Description,
                    FileUrl = await FileService.GetFileAsync(Q.FileBlobId, Q.FileMime),
                    IsDownloadable = Q.IsDownloadable,
                    ApprovedAt = Q.ApprovedAt,
                    IsDeleted = Q.IsDeleted,
                    Link = Q.Link,
                    NewsCategoryName = Q.NewsCategoryName,
                    NewsId = Q.NewsId,
                    ThumbnailUrl = await FileService.GetFileAsync(Q.ThumbnailBlobId, Q.ThumbnailMime),
                    Title = Q.Title,
                    TotalView = Q.TotalView,
                    CreatedAt = Q.CreatedAt
                })
                .Select(Q => Q.Result)
                .ToList();

            return news;
        }

        /// <summary>
        /// Get news by category.
        /// </summary>
        /// <param name="newsCategoryId"></param>
        /// <param name="itemPerPage"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideNewsModel>> GetNewsByCategory(int newsCategoryId, int itemPerPage, int pageIndex)
        {
            var newsTemp = await NewsQueryable()
                .OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt)
                .Where(Q => Q.NewsCategoryId == newsCategoryId)
                .Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();

            var news = newsTemp
                .Select(async Q => new UserSideNewsModel
                {
                    Author = Q.Author,
                    NewsCategoryId = Q.NewsCategoryId,
                    Description = Q.Description,
                    FileUrl = await FileService.GetFileAsync(Q.FileBlobId, Q.FileMime),
                    IsDownloadable = Q.IsDownloadable,
                    ApprovedAt = Q.ApprovedAt,
                    IsDeleted = Q.IsDeleted,
                    Link = Q.Link,
                    NewsCategoryName = Q.NewsCategoryName,
                    NewsId = Q.NewsId,
                    ThumbnailUrl = await FileService.GetFileAsync(Q.ThumbnailBlobId, Q.ThumbnailMime),
                    Title = Q.Title,
                    TotalView = Q.TotalView,
                    CreatedAt = Q.CreatedAt,
                    UpdatedAt = Q.UpdateAt
                })
                .Select(Q => Q.Result)
                .ToList();

            return news;
        }

        public async Task<bool> AddTotalView(int newsId)
        {
            var newsData = await this.DB.News.Where(Q => Q.NewsId == newsId).FirstOrDefaultAsync();
            newsData.TotalView = newsData.TotalView + 1;

            await this.DB.SaveChangesAsync();
            return true;
        }
    }
}
