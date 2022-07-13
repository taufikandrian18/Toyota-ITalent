using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ModuleModel
    {
        public int ModuleId { get; set; }
        public Guid BlobId { get; set; }
        public int MaterialId { get; set; }
        public int ApprovalId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ModuleFormModel
    {
        public int ModuleId { get; set; }
        public Guid BlobId { get; set; }
        public int MaterialId { get; set; }
        public int ApprovalId { get; set; }
        [Required]
        [StringLength(64)]
        public string ModuleName { get; set; }
        [Required]
        [StringLength(1024)]
        public string ModuleDescription { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ModuleForTaskModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }
}
