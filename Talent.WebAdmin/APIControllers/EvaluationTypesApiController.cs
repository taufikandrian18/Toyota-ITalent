using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/evaluation-types")]
    public class EvaluationTypesApiController : Controller
    {
        private readonly EvaluationTypesService _EvaluationTypeMan;

        public EvaluationTypesApiController(EvaluationTypesService evaluationTypesService)
        {
            this._EvaluationTypeMan = evaluationTypesService;
        }
        // GET: api/<controller>
        [HttpGet("get-all-evaluation-types-async")]
        public async Task<List<EvaluationTypesViewModel>> GetAllEvaluationTypesAsync()
        {
            return await this._EvaluationTypeMan.GetAllEvaluationTypesAsync();
        }
    }
}
