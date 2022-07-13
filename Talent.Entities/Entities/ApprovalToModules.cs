using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToModules
    {
        [Key]
        public int ApprovalId { get; set; }
        public int ModuleId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToModules")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("ApprovalToModules")]
        public virtual Modules Module { get; set; }
    }
}
