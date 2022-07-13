using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SetupTop5CourseJoinedModel
    {
        public int CourseId { get; set; }
        public int TrainingId { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Date { get; set; }
        public int Top5Course { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? ApprovedAt{ get; set; }
        public int Batch { get; set; }
    }

    public class SetupTop5CourseViewModel
    {
        public List<SetupTop5CourseJoinedModel> ListSetupTop5Course { get; set; }
        public int TotalItem { get; set; }
    }

    public class SetupTop5CourseSearch
    {
        public string CourseName { get; set; }
    }

    public class SetupTop5CourseDropdownData
    {
        public int CourseId { get; set; }
        public int TrainingId { get; set; }
        public string CourseName { get; set; }
        public bool IsDeleted { get; set; }
        public int Top5Course { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int Batch { get; set; }

    }

    public class SetupTop5CourseDropdownList
    {
        public List<SetupTop5CourseDropdownData> CourseNameDropdown { get; set; }
    }

    public class SetupTop5TrainingId
    {
        public int TrainingId { get; set; }
    }

    public class RearrangeTop5Course
    {
        public List<SetupTop5TrainingId> RearrangeTop5CourseList { get; set; }
    }
}
