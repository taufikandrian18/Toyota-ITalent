using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class KeyActions
    {
        public KeyActions()
        {
            CompetencyKeyActionMappings = new HashSet<CompetencyKeyActionMappings>();
        }

        [Key]
        public int KeyActionId { get; set; }
        [Required]
        [StringLength(16)]
        public string KeyActionCode { get; set; }
        [Required]
        [StringLength(255)]
        public string KeyActionName { get; set; }
        public string KeyActionDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }

        [InverseProperty("KeyAction")]
        public virtual ICollection<CompetencyKeyActionMappings> CompetencyKeyActionMappings { get; set; }
    }
}
