using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using Microsoft.AspNetCore.Mvc;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideDealerService : Controller
    {
        private readonly TalentContext DB;
        private readonly UserSideAuthService AuthService;

        public UserSideDealerService(TalentContext db, UserSideAuthService authService)
        {
            this.DB = db;
            this.AuthService = authService;

        }

        public async Task<ActionResult<BaseResponse>> SearchDealer(string dealerName,bool status)
        {
            var query = DB.Dealers.AsQueryable().Where(Q=> Q.OtherCompany == status);

            if (!String.IsNullOrWhiteSpace(dealerName))
            {
                query = query.Where(x => x.DealerName.Contains(dealerName));
            }

            var dealerData = await query.Select(x => new DealerModel
            {
                DealerId = x.DealerId,
                DealerName = x.DealerName,
            }).ToListAsync();

            return StatusCode(200, BaseResponse.ResponseOk(dealerData));
        }

        public async Task<ActionResult<BaseResponse>> SearchOutlet(string outlateName, string dealerId)
        {
            var query = DB.Outlets.AsQueryable();

            if (!String.IsNullOrWhiteSpace(outlateName))
            {
                query = query.Where(x => x.Name.Contains(outlateName));
            }

            if (!String.IsNullOrWhiteSpace(dealerId))
            {
                query = query.Where(x => x.DealerId == dealerId);
            }

            var outlateData = await query.Select(x => new OutletModel
            {
                OutletId = x.OutletId,
                OutletName = x.Name,

            }).ToListAsync();

            return StatusCode(200, BaseResponse.ResponseOk(outlateData));
        }

        public async Task<ActionResult<BaseResponse>> SearchPosition( string positionName, bool? isTamPeople, bool? isOtherPeople)
        {
            try
            {
                var query = DB.Positions.AsQueryable();
                if (isTamPeople != null)
                {
                   query = query.Where(x => x.IsTamPeople == isTamPeople.Value);
                }

                if (isOtherPeople != null)
                {
                    query = query.Where(x => x.IsOtherDealer == isOtherPeople.Value);
                }

                if (!String.IsNullOrEmpty(positionName))
                {
                    query = query.Where(Q => Q.PositionName.Contains(positionName) || Q.PositionDescription.Contains(positionName));
                }

                var positionData = await query.Where(Q=> Q.PositionName !="*").Select(x => new PositionModel
                {
                    PositionId = x.PositionId,
                    PositionName = x.PositionName,
                }).ToListAsync();

                return StatusCode(200, BaseResponse.ResponseOk(positionData));
            }

            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

       
    }
}
