using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
  [Produces("application/json")]
  [Route("api/v1/setup-course")]
  public class SetupCourseApiController : Controller
  {
    private readonly SetupCourseService SetupCourseMan;

    public SetupCourseApiController(SetupCourseService setupCourseService)
    {
      this.SetupCourseMan = setupCourseService;
    }

    [HttpGet("getAllCourseNoSetup", Name = "get-all-courses-no-setup")]
    public async Task<ActionResult<CourseViewModel>> GetAllCourseNoSetup(string search, bool? descending)
    {
      if (descending == null)
      {
        descending = true;
      }
      var result = await this.SetupCourseMan.GetAllCourseNoSetup(search, descending);
      return Ok(result);
    }

    [HttpGet("getAllModules", Name = "get-all-modules")]
    public async Task<ActionResult<List<ModuleForSetupModel>>> GetAllModules(string search)
    {
      var result = await this.SetupCourseMan.GetAllModules(search);
      return Ok(result);
    }

    [HttpGet("getAllPrerequisites", Name = "get-all-prerequisites")]
    public async Task<ActionResult<List<CoursePrerequisiteViewModel>>> GetPrerequisites(string search)
    {
      var result = await this.SetupCourseMan.GetAllPrerequisites(search);
      return Ok(result);
    }

    [HttpGet("getAllTaskCompetencies", Name = "get-all-task-competencies")]
    public async Task<ActionResult<List<CompetencyMappingJoinModel>>> GetAllTaskCompetencies(string search)
    {
      var result = await this.SetupCourseMan.GetAllTaskCompetencies(search);
      return Ok(result);
    }

    [HttpGet("getAllTaskModules", Name = "get-all-task-modules")]
    public async Task<ActionResult<List<ModuleForSetupModel>>> GetAllTaskModules(string search)
    {
      var result = await this.SetupCourseMan.GetAllTaskModules(search);
      return Ok(result);
    }

    [HttpGet("getAllTaskFiltered", Name = "get-all-task-filtered")]
    public async Task<ActionResult<List<TaskForSetupModuleModel>>> GetAllTaskFiltered([FromQuery] TaskCodeFilteredFormModel model)
    {
      var result = await this.SetupCourseMan.GetAllTaskFiltered(model);
      return Ok(result);
    }

    [HttpGet("getScorePoints", Name = "get-task-score-point")]
    public async Task<ActionResult<ScorePointsModel>> GetApiScorePoints([FromQuery] TaskScorePointFilteredModel model)
    {
      var result = await this.SetupCourseMan.GetApiScorePoints(model);
      return Ok(result);
    }

    [HttpGet("get/{id}", Name = "get-setup-course-by-id")]
    public async Task<ActionResult<CourseJoinModel>> GetSetupCourseById(int id)
    {
      var result = await SetupCourseMan.GetSetupCourseById(id);
      return Ok(result);
    }

    [HttpPost("create", Name = "create-setup-course")]
    public async Task<ActionResult<int>> CreateSetupCourse([FromBody] SetupCourseFormModel model)
    {
      string messages = string.Join("; ", ModelState.Values
                                  .SelectMany(x => x.Errors)
                                  .Select(x => x.ErrorMessage));


      if (ModelState.IsValid == false)
      {
        return BadRequest(messages);
      }

      var result = await SetupCourseMan.CreateSetupCourse(model);
      return Ok(result);
    }
  }
}
