using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Modules
    {
        public Modules()
        {
            ApprovalToModules = new HashSet<ApprovalToModules>();
            ModuleTopicMappings = new HashSet<ModuleTopicMappings>();
            Rewards = new HashSet<Rewards>();
            SetupModules = new HashSet<SetupModules>();
            SetupTasks = new HashSet<SetupTasks>();
            Tasks = new HashSet<Tasks>();
            FinalScores = new HashSet<FinalScores>();
        }

        [Key]
        public int ModuleId { get; set; }
        public Guid BlobId { get; set; }
        public int MaterialTypeId { get; set; }
        public Guid? MaterialBlobId { get; set; }
        [Required]
        [StringLength(255)]
        public string ModuleName { get; set; }
        [Required]
        public string ModuleDescription { get; set; }
        [StringLength(1024)]
        public string MaterialLink { get; set; }
        public bool MaterialDownloadable { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("ModulesBlob")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("MaterialBlobId")]
        [InverseProperty("ModulesMaterialBlob")]
        public virtual Blobs MaterialBlob { get; set; }
        [ForeignKey("MaterialTypeId")]
        [InverseProperty("Modules")]
        public virtual MaterialTypes MaterialType { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<ApprovalToModules> ApprovalToModules { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<ModuleTopicMappings> ModuleTopicMappings { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<Rewards> Rewards { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<SetupModules> SetupModules { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<SetupTasks> SetupTasks { get; set; }
        [InverseProperty("Module")]
        public virtual ICollection<Tasks> Tasks { get; set; }

        [InverseProperty("Module")]
        public virtual ICollection<TaskScores> TaskScoreNavigation { get; set; }

        [InverseProperty("Module")]
        public virtual ICollection<FinalScores> FinalScores { get; set; }
    }
}
