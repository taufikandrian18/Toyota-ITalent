using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
  public class AssesmentSkillChecks
  {

    [Key]
    public String GUID { get; set; }
    public String SkillCheckGUID { get; set; }
    public String AssesmentGUID { get; set; }
    public float Order { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("SkillCheckGUID")]
    [InverseProperty("AssesmentsSkillCheckNavigation")]
    public virtual SkillChecks SkillChecksNavigator { get; set; }

    [ForeignKey("AssesmentGUID")]
    [InverseProperty("AssesmentSkillChecksNavigation")]
    public virtual Assesments AssesmentsNavigator { get; set; }
  }
}