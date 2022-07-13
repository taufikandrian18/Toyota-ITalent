using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?Linkrequest=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/remedial/")]
    public class UserSideRemedialController : Controller
    {
        private readonly UserSideRemedialService RemedialService;

        public UserSideRemedialController(UserSideRemedialService service)
        {
            this.RemedialService = service;
        }

    }
}
