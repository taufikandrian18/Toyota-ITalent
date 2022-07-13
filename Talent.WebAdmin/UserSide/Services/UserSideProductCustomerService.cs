using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProductCustomerService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;

        public UserSideProductCustomerService(TalentContext context, ClaimHelper hm)
        {
            this.DB = context;
            this.HelperMan = hm;
        }

        public async Task<List<UserSideProductCustomerTypeListView>> GetAllUserSideProductCustomerTypeName(Guid productId)
        {
            var query2 = await (from c in DB.ProductCustomers
                                join d in DB.ProductCustomerTypes on c.ProductCustomerTypeId equals d.ProductCustomerTypeId
                                where c.IsDeleted == false && c.ProductId == productId && c.ApprovedAt != null
                                select new UserSideProductCustomerTypeListView
                                {
                                    ProductCustomerTypeName = d.ProductCustomerTypeName
                                }).AsNoTracking().ToListAsync();

            return query2;
        }

        public async Task<UserSideProductCustomerPaginate> GetAllUserSideProductCustomerFiltered(int pageSize, int pageIndex, string productCustomerTypeName,Guid ProductId)
        {
            var skipCount = Math.Ceiling((decimal)(pageIndex - 1) * pageSize);

            if(ProductId != Guid.Empty)
            {
                var query2 = await (from c in DB.ProductCustomers
                                    where c.IsDeleted == false && c.ProductId == ProductId && c.ApprovedAt != null
                                    select new UserSideProductCustomerModel
                                    {
                                        ProductCustomerId = c.ProductCustomerId,
                                        ProductId = c.ProductId,
                                        ProductCustomerTypeId = c.ProductCustomerTypeId,
                                        ProductCustomerUmur = c.ProductCustomerUmur,
                                        ProductCustomerStatus = c.ProductCustomerStatus,
                                        ProductCustomerPekerjaan = c.ProductCustomerPekerjaan,
                                        ProductCustomerPenghasilan = c.ProductCustomerPenghasilan,
                                        ProductCustomerPrevVehicle = c.ProductCustomerPrevVehicle,
                                        ProductCustomerKebutuhan = c.ProductCustomerKebutuhan,
                                        ProductCustomerProspectSource = c.ProductCustomerProspectSource,
                                        CreatedAt = c.CreatedAt,
                                        UpdatedAt = c.UpdatedAt
                                    }).AsNoTracking().ToListAsync();

                var dataProductCustomerFiltered = query2.Skip((int)skipCount).Take(pageSize).ToList();

                var dataProductCustomerCount = dataProductCustomerFiltered.Count();

                var dataProductCustomerName = this.DB.ProductCustomerTypes.Select(Q => new UserSideProductCustomerTypeListView
                {
                    ProductCustomerTypeName = Q.ProductCustomerTypeName
                });

                var data = await dataProductCustomerName.ToListAsync();

                return new UserSideProductCustomerPaginate
                {
                    ProductCustomers = dataProductCustomerFiltered,
                    TotalProductCustomer = dataProductCustomerCount,
                    ProductCustomerTypeNames = data
                };
            }
            else
            {
                var query2 = await (from c in DB.ProductCustomers
                                    where c.IsDeleted == false && c.ApprovedAt != null
                                    select new UserSideProductCustomerModel
                                    {
                                        ProductCustomerId = c.ProductCustomerId,
                                        ProductId = c.ProductId,
                                        ProductCustomerTypeId = c.ProductCustomerTypeId,
                                        ProductCustomerUmur = c.ProductCustomerUmur,
                                        ProductCustomerStatus = c.ProductCustomerStatus,
                                        ProductCustomerPekerjaan = c.ProductCustomerPekerjaan,
                                        ProductCustomerPenghasilan = c.ProductCustomerPenghasilan,
                                        ProductCustomerPrevVehicle = c.ProductCustomerPrevVehicle,
                                        ProductCustomerKebutuhan = c.ProductCustomerKebutuhan,
                                        ProductCustomerProspectSource = c.ProductCustomerProspectSource,
                                        CreatedAt = c.CreatedAt,
                                        UpdatedAt = c.UpdatedAt
                                    }).AsNoTracking().ToListAsync();

                var dataProductCustomerFiltered = query2.Skip((int)skipCount).Take(pageSize).ToList();

                var dataProductCustomerCount = dataProductCustomerFiltered.Count();

                var dataProductCustomerName = this.DB.ProductCustomerTypes.Select(Q => new UserSideProductCustomerTypeListView
                {
                    ProductCustomerTypeName = Q.ProductCustomerTypeName
                });

                var data = await dataProductCustomerName.ToListAsync();

                return new UserSideProductCustomerPaginate
                {
                    ProductCustomers = dataProductCustomerFiltered,
                    TotalProductCustomer = dataProductCustomerCount,
                    ProductCustomerTypeNames = data
                };
            }
            
        }
    }
}
