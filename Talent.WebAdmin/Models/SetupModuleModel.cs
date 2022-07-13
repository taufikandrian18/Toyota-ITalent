using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Models
{
  public class SetupModuleModuleViewModel
  {
    public int ModuleId { get; set; }
    public string ModuleName { get; set; }
    public string ModuleDescription { get; set; }

  }

  public class ModuleForSetupModel
  {
    public int ModuleId { get; set; }
    public string ModuleName { get; set; }
  }

  public class TaskForSetupModuleModel
  {
    public string TaskCode { get; set; }
    public int TaskId { get; set; }
    public int? TaskTypeId { get; set; }
    public int? Score { get; set; }
    public int? Points { get; set; }

  }

  public class TaskCodeFilteredFormModel
  {
    public string TaskCode { get; set; }
    public int? CompetencyId { get; set; }
    public int? ModuleId { get; set; }

  }
  public class TaskScorePointFilteredModel
  {
    public int? TaskTypeId { get; set; }
    public int? TaskId { get; set; }

  }


  public class SetupModuleFormModel
  {
    public int? SetupModuleId { get; set; }
    public int? CourseId { get; set; }
    public string AssesmentId { get; set; }
    public string AssesmentName { get; set; }
    public int? ModuleId { get; set; }
    public string ModuleName { get; set; }
    public string ModuleDescription { get; set; }
    public int? TrainingTypeId { get; set; }
    public int TimePointId { get; set; }
    public int? Score { get; set; }
    public int? Points { get; set; }
    public bool? IsRecommendedForYou { get; set; }
    public bool? IsPopular { get; set; }
    public int? MinimumScore { get; set; }
    public bool? IsForRemedial { get; set; }
    public SetupTaskFormModel SetupTaskForm { get; set; }
    public int InputType { get; set; }
    public int Order { get; set; }
    public string EnumRemedialOption { get; set; }
    public float RemedialLimit { get; set; }
    public string EnumScoringMethod { get; set; }
    public bool IsByOption { get; set; }
    public string EnumCategoryPreDuringPost { get; set; }
    public List<ResponseAssesmentModel> AssesmentList { get; set; }
  }

  public class SetupTaskFormModel
  {
    public int? SetupTaskId { get; set; }
    public int? SetupModuleId { get; set; }
    public int? PopQuizId { get; set; }
    public int? CompetencyId { get; set; }
    public string CompetencyNameMapping { get; set; }
    public int? EvaluationTypeId { get; set; }
    public int? CompetencyTypeId { get; set; }
    public string PrefixCode { get; set; }
    public string CompetencyTypeName { get; set; }
    public string EvaluationTypeName { get; set; }
    public int? ModuleId { get; set; }
    public string ModuleName { get; set; }
    public string ModuleDescription { get; set; }
    public int? TestTime { get; set; }
    public bool? IsGrouping { get; set; }
    public int? TotalParticipant { get; set; }
    public int? TotalQuestion { get; set; }
    public int? QuestionPerParticipant { get; set; }
    public List<SetupTaskCodesFormModel> TaskList { get; set; }
  }

  public class SetupTaskCodesFormModel
  {
    public int? SetupTaskId { get; set; }
    public int? TaskId { get; set; }
    public int? QuestionNumber { get; set; }
    public string TaskName { get; set; }
    public string TaskCode { get; set; }
    public int? TaskTypeId { get; set; }
    public int? Score { get; set; }
    public int? Points { get; set; }
  }
}
