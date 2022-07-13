using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
  public class AreaSpecialists
  {
    public AreaSpecialists()
    {
      Outlets = new HashSet<Outlets>();
    }
    [Key]
    [StringLength(32)]
    public string AreaSpecialistId { get; set; }
    [Required]
    [StringLength(128)]
    public string AreaSpecialistName { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required]
    [StringLength(64)]
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    [Required]
    [StringLength(64)]
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }

    [InverseProperty("AreaSpecialist")]
    public virtual ICollection<Outlets> Outlets { get; set; }
  }
}
