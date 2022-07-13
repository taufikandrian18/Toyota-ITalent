using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeInterests
    {
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public int TopicId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeeInterests")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("TopicId")]
        [InverseProperty("EmployeeInterests")]
        public virtual Topics Topic { get; set; }
    }
}
