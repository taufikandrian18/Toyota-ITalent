using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SetupCourseFormModel
    {
        public int CourseId { get; set; }
        public string AssesmentId { get; set; }
        public string AssesemntName { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public bool? IsRecommendedForYou { get; set; }
        public bool? IsPopular { get; set; }
        public int ApprovalId { get; set; }
        public int Order { get; set; }
        public List<CourseTrainingTypeMappingModel> ListCourseTrainingTypeMappings { get; set; }
        public List<CoursePrerequisiteViewModel> ListCoursePrerequisiteMappings { get; set; }
        public List<CourseLearningObjectiveModel> ListCourseLearningObjectives { get; set; }
        public List<SetupModuleFormModel> ListSetupModules { get; set; }
        public List<RequestAssesmentModel> ListAssesment { get; set; }
    }

    public class CourseTrainingTypeMappingModel
    {
        public int CourseId { get; set; }
        public int TrainingTypeId { get; set; }
        public int? MinimumScore { get; set; }
        [StringLength(256)]
        public string WorkloadRequirement { get; set; }
        public int? Percentage { get; set; }
    }

    public class CoursePrerequisiteMappingModel
    {
        public int CoursePrerequisiteMappingId { get; set; }
        public int PrevCourseId { get; set; }
        public int? NextCourseId { get; set; }
        public int? NextSetupModuleId { get; set; }
    }

    public class CoursePrerequisiteViewModel
    {
        public int? NextCourseId { get; set; }
        public int? NextSetupModuleId { get; set; }
        public string Name { get; set; }
    }

    public class CourseLearningObjectiveModel
    {
        public int LearningObjectiveId { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string LearningObjectiveName { get; set; }
    }

    public class ScorePointsModel
    {
        public int Score { get; set; }
        public int Points { get; set; }
    }
}
