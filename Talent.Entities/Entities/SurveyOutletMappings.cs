using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class SurveyOutletMappings
    {
        public int SurveyId { get; set; }
        [StringLength(64)]
        public string OutletId { get; set; }

        [ForeignKey("OutletId")]
        [InverseProperty("SurveyOutletMappings")]
        public virtual Outlets Outlet { get; set; }
        [ForeignKey("SurveyId")]
        [InverseProperty("SurveyOutletMappings")]
        public virtual Surveys Survey { get; set; }
    }
}
