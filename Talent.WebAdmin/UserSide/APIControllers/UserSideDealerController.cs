using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

using TAM.Talent.Commons.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.UserSide.APIControllers
{
    [Route("api/v1/dealer")]
    public class UserSideDealerController : Controller
    {
        private readonly UserSideDealerService DealerServices;


        public UserSideDealerController(UserSideDealerService service)
        {
            DealerServices = service;
        }

      
        [HttpGet("search-dealer")]
        public async Task<ActionResult<BaseResponse>> SearchDealer(string dealerName, bool status  )
        {
            var data = await DealerServices.SearchDealer(dealerName, status);
            return data;
        }


      


        [HttpGet("search-outlet")]
        public async Task<ActionResult<BaseResponse>> SearchOutlet(string outletName, string dealerId)
        {
            var data = await DealerServices.SearchOutlet(outletName,dealerId);
            return data;
        }

        [HttpGet("search-position")]
        public async Task<ActionResult<BaseResponse>> SearchPosition(string positionName, bool? isOtherPeople, bool? isTamPeople)
        {
            var data = await DealerServices.SearchPosition(positionName, isTamPeople, isOtherPeople);
            return data;
        }
    }
}
