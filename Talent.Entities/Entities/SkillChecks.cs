using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Talent.Entities.Entities
{
  public class SkillChecks
  {

    public SkillChecks()
    {
      AssesmentsSkillCheckNavigation = new HashSet<AssesmentSkillChecks>();
      SkillChecksQuestionsNavigation = new HashSet<SkillChecksQuestions>();
      SkillCheckScoreConfigsNavigation = new HashSet<SkillChecksScoreConfigs>();
      LiveAssesmentSkillCheckScoreNavigation = new HashSet<LiveAssesmentSkillCheckScores>();
      FinalScores = new HashSet<FinalScores>();
    }

    [Key]
    public String GUID { get; set; }
    public String Title { get; set; }
    public String Name { get; set; }
    public Boolean IsQuestion { get; set; }
    public float MinimumScore { get; set; }
    public String EnumFeedbackScore { get; set; }
    public String Description { get; set; }
    public String MediaBlobId { get; set; }
    public String CreatedBy {get;set;}
    public String UpdatedBy {get;set;}
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("SkillChecksNavigator")]
    public virtual ICollection<AssesmentSkillChecks> AssesmentsSkillCheckNavigation { get; set; }

    [InverseProperty("SkillCheckNavigator")]
    public virtual ICollection<SkillChecksQuestions> SkillChecksQuestionsNavigation { get; set; }

    [JsonIgnore]
    [InverseProperty("SkillChecksNavigator")]
    public virtual ICollection<SkillChecksScoreConfigs> SkillCheckScoreConfigsNavigation { get; set; }


    [InverseProperty("SkillCheckNavigator")]
    public virtual ICollection<LiveAssesmentSkillChecks> LiveAssesmentSkillCheckNavigation { get; set; }

    [InverseProperty("SkillCheckNavigator")]
    public virtual ICollection<LiveAssesmentSkillCheckScores> LiveAssesmentSkillCheckScoreNavigation { get; set; }

    [InverseProperty("SkillCheck")]
    public virtual ICollection<FinalScores> FinalScores { get; set; }



  }
}