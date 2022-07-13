using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Approvals
    {
        public Approvals()
        {
            ApprovalHistories = new HashSet<ApprovalHistories>();
            Inboxes = new HashSet<Inboxes>();
        }

        [Key]
        public int ApprovalId { get; set; }
        [Required]
        [StringLength(1024)]
        public string ContentName { get; set; }
        [Required]
        [StringLength(255)]
        public string ContentCategory { get; set; }
        public int? ApprovalMappingId { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApprovalStatusId { get; set; }
        [StringLength(64)]
        public string ActionBy { get; set; }
        public DateTime? ActionAt { get; set; }
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(64)]
        public string ApprovalTo { get; set; }

        [ForeignKey("ActionBy")]
        [InverseProperty("ApprovalsActionByNavigation")]
        public virtual Employees ActionByNavigation { get; set; }
        [ForeignKey("ApprovalMappingId")]
        [InverseProperty("Approvals")]
        public virtual ApprovalMappings ApprovalMapping { get; set; }
        [ForeignKey("ApprovalStatusId")]
        [InverseProperty("Approvals")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
        [ForeignKey("ApprovalTo")]
        [InverseProperty("ApprovalsApprovalToNavigation")]
        public virtual Employees ApprovalToNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ApprovalsCreatedByNavigation")]
        public virtual Employees CreatedByNavigation { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToBanners ApprovalToBanners { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToCourses ApprovalToCourses { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToEvents ApprovalToEvents { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToGuides ApprovalToGuides { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToModules ApprovalToModules { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToNews ApprovalToNews { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToPopQuizzes ApprovalToPopQuizzes { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToSetupCourses ApprovalToSetupCourses { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToSetupModules ApprovalToSetupModules { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToSurveys ApprovalToSurveys { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToTasks ApprovalToTasks { get; set; }
        [InverseProperty("Approval")]
        public virtual ApprovalToTrainings ApprovalToTrainings { get; set; }
        [InverseProperty("Approval")]
        public virtual ICollection<ApprovalHistories> ApprovalHistories { get; set; }
        [InverseProperty("Approval")]
        public virtual ICollection<Inboxes> Inboxes { get; set; }
    }
}
