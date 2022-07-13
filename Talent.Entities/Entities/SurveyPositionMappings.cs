using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyPositionMappings
    {
        public int SurveyId { get; set; }
        public int PositionId { get; set; }

        [ForeignKey("PositionId")]
        [InverseProperty("SurveyPositionMappings")]
        public virtual Positions Position { get; set; }
        [ForeignKey("SurveyId")]
        [InverseProperty("SurveyPositionMappings")]
        public virtual Surveys Survey { get; set; }
    }
}
