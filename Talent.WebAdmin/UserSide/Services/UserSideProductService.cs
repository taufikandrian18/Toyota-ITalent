using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Talent.Entities.Entities;
using TAM.Talent.Commons.Core.Interfaces;
using static Talent.WebAdmin.UserSide.Models.UserSideProductModel;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly BlobService BlobService;

        public UserSideProductService(TalentContext context, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = context;
            this.FileService = fileService;
            this.BlobService = blobService;
        }
        /// <summary>
        /// GEt all Product + pagination
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<List<UserSideProductInformationModel>> GetAllUserSideProductFiltered(int pageSize, int pageIndex, string productCategory)
        {

            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            //var query2 = await (from c in DB.Products
            //                    join d in DB.ProductCategories on c.ProductCategoryId equals d.ProductCategoryId
            //                    where c.IsCompetitor == false && c.IsDeleted == false && c.ApprovedAt != null
            //                    orderby c.ProductSegment
            //                    select new UserSideProductGridModel
            //                    {
            //                        ProductId = c.ProductId,
            //                        ProductName = c.ProductName,
            //                        ProductCategory = d.ProductCategoryName,
            //                        ProductSegment = c.ProductSegment,
            //                        IsCompetitor = c.IsCompetitor,
            //                        BlobId = c.BlobId,
            //                        CreatedAt = c.CreatedAt,
            //                        UpdatedAt = c.UpdatedAt
            //                    }).AsNoTracking().ToListAsync();

            var query3 = await (from c in DB.Products
                                join d in DB.ProductCategories on c.ProductCategoryId equals d.ProductCategoryId
                                join e in DB.ProductTypes on c.ProductId equals e.ProductId
                                where c.IsCompetitor == false && c.IsDeleted == false && c.ApprovedAt != null && e.ProductTypeId != null && e.ApprovedAt != null
                                orderby c.ProductSegment
                                select new UserSideProductGridModel
                                {
                                    ProductId = c.ProductId,
                                    ProductName = c.ProductName,
                                    ProductCategory = d.ProductCategoryName,
                                    ProductSegment = c.ProductSegment,
                                    IsCompetitor = c.IsCompetitor.HasValue ? c.IsCompetitor.Value : false,
                                    BlobId = c.BlobId,
                                    CreatedAt = c.CreatedAt,
                                    UpdatedAt = c.UpdatedAt
                                }).AsNoTracking().Distinct().ToListAsync();

            var dataProductFiltered = new List<UserSideProductInformationModel>();

            if (productCategory != "All")
            {
                dataProductFiltered.AddRange(query3.Where(x => x.ProductCategory == productCategory)
                .Select(x => new UserSideProductInformationModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    BlobId = x.BlobId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .Skip((int)skipCount)
                .Take(pageSize)
                .ToList());
            }
            else
            {
                dataProductFiltered.AddRange(query3
                .Select(x => new UserSideProductInformationModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    BlobId = x.BlobId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .Skip((int)skipCount)
                .Take(pageSize)
                .ToList());
            }

            var index = 0;
            foreach (var datum in dataProductFiltered)
            {

                var imageURL = "";
                var imageFileName = "";
                var blobData = await this.BlobService.GetBlobById(datum.BlobId);
                if(blobData != null)
                {
                    imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                    imageFileName = blobData.Name;
                    dataProductFiltered[index].ImageUrl = imageURL;
                    dataProductFiltered[index].ImageFileName = imageFileName;
                }
                index++;
            }

            return dataProductFiltered;
        }

        public async Task<List<UserSideProductCatListModel>> GetAllUserSideProductCategoryList()
        {
            List<UserSideProductCatListModel> data = new List<UserSideProductCatListModel>
            {
                new UserSideProductCatListModel { FilterId = Guid.NewGuid(), FilterName = "All" }
            };
            //return Task.Run(() =>
            //{
            //    //Doing stuff here

            //    var enumData = (from UserSideProductCat e in Enum.GetValues(typeof(UserSideProductCat))
            //                    select new UserSideProductCatListModel
            //                    {
            //                        FilterId = (int)e,
            //                        FilterName = e.ToString()
            //                    }).ToList();

            //    return enumData;
            //});
            data.AddRange(await this.DB.ProductCategories.Where(Q => Q.IsDeleted == false ).Select(Q => new UserSideProductCatListModel
            {
                FilterId = Q.ProductCategoryId,
                FilterName = Q.ProductCategoryName
            }).ToListAsync());

            return data;
        }
    }
}
