using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Talent.Entities.Entities
{
    public class SkillChecksScoreConfigs
    {

        public SkillChecksScoreConfigs()
        {
        }

        [Key]
        public String GUID {get;set;}
        public String SkillCheckGUID{get;set;}
        public float Point {get;set;}
        public float Score {get;set;}
        public String Text {get;set;}
        public String Description { get; set; }
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        [ForeignKey("SkillCheckGUID")]
        [InverseProperty("SkillCheckScoreConfigsNavigation")]
        public virtual SkillChecks SkillChecksNavigator { get; set; }
       
    }
}