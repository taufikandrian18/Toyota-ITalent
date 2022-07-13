using System;
using System.ComponentModel.DataAnnotations;

namespace Talent.Entities.Entities
{
    public class GetAllMyCoachReport
    {
        public GetAllMyCoachReport()
        {
        }
        [Key]
        public int trainingid { get; set; }
        [StringLength(450)]
        public string assesmentid { get; set; }
        public int? moduleid { get; set; }
        [StringLength(255)]
        public string modulename { get; set; }
        public int setupmoduleid { get; set; }
        public int? trainingtypeid { get; set; }
        public float moduleorder { get; set; }
        [StringLength(int.MaxValue)]
        public string enumscoringmethod { get; set; }
        [StringLength(int.MaxValue)]
        public string Name { get; set; }
        [StringLength(450)]
        public string guid { get; set; }
        public float? score { get; set; }
        public float? skillcheckorder { get; set; }
        public bool? isbyoption { get; set; }
        [StringLength(64)]
        public string employeeid { get; set; }
        public bool? passedstatus { get; set; }
        public DateTime? createdAt { get; set; }
        public float? finalscore { get; set; }
        [StringLength(1)]
        public string returnValue { get; set; }
        public DateTime? endtime { get; set; }
    }
}
