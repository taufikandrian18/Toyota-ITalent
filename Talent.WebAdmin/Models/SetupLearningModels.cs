using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SetupLearningModels
    {
        public int SetupLearningId { get; set; }
        public int? SetupModuleId { get; set; }
        public int? PopQuizId { get; set; }
        public int? CourseId { get; set; }
        public string LearningName { get; set; }
        public string ProgramTypeName { get; set; }
        public string LearningCategoryName { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class SetupLearningPaginate
    {
        public List<SetupLearningModels> Data { get; set; }
        public int TotalData { get; set; }
    }


    public class SetupLearningGridFilter : GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string ProgramType { get; set; }
        public string LearningCategory { get; set; }
        public string LearningName { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class ProgramTypesModel
    {
        public string ProgramTypeName { get; set; }
    }

    public class SetupLearningCourseLockUnlock
    {
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public List<SetupLearningModuleLockUnlock> Module { get; set; }
        public List<SetupLearningModuleLockUnlock> Assesment {get;set;}
    }
    public class SetupLearningModuleLockUnlock
    {
        public int? ModuleId { get; set; }
        public string AssesmentId {get;set;}
        public string ModuleName { get; set; }
        public bool IsPublishedModule { get; set; }
    }
    public class SetupLearningCourseLockUnlockGet
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string AssesmentName {get;set;}
        public string AssesmentGUID {get;set;}
        public bool IsPublishedModule { get; set; }
    }
}
