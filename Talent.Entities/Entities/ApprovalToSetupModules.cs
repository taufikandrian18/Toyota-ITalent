using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class ApprovalToSetupModules
    {
        [Key]
        public int ApprovalId { get; set; }
        public int SetupModuleId { get; set; }

        [ForeignKey("ApprovalId")]
        [InverseProperty("ApprovalToSetupModules")]
        public virtual Approvals Approval { get; set; }
        [ForeignKey("SetupModuleId")]
        [InverseProperty("ApprovalToSetupModules")]
        public virtual SetupModules SetupModule { get; set; }
    }
}
