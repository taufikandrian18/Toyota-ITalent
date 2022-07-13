using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class CompetencyTypes
    {
        public CompetencyTypes()
        {
            Competencies = new HashSet<Competencies>();
        }

        [Key]
        public int CompetencyTypeId { get; set; }
        [Required]
        [StringLength(32)]
        public string CompetencyTypeName { get; set; }

        [InverseProperty("CompetencyType")]
        public virtual ICollection<Competencies> Competencies { get; set; }
    }
}
