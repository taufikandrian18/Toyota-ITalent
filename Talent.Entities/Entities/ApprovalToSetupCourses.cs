using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToSetupCourses
    {
        [Key]
        public int ApprovalId { get; set; }
        public int CourseId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToSetupCourses")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("CourseId")]
        [InverseProperty("ApprovalToSetupCourses")]
        public virtual Courses Course { get; set; }
    }
}
