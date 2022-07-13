using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToCourses
    {
        [Key]
        public int ApprovalId { get; set; }
        public int CourseId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToCourses")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("CourseId")]
        [InverseProperty("ApprovalToCourses")]
        public virtual Courses Course { get; set; }
    }
}
