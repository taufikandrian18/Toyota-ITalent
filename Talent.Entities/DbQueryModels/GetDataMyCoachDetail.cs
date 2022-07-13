using System;
namespace Talent.Entities.DbQueryModels
{
    public class GetDataMyCoachDetail
    {
        public int trainingid { get; set; }
        public string assesmentid { get; set; }
        public int? moduleid { get; set; }
        public string modulename { get; set; }
        public int setupmoduleid { get; set; }
        public int? trainingtypeid { get; set; }
        public float moduleorder { get; set; }
        public string enumscoringmethod { get; set; }
        public string Name { get; set; }
        public string guid { get; set; }
        public float? score { get; set; }
        public float? skillcheckorder { get; set; }
        public bool? isbyoption { get; set; }
        public string employeeid { get; set; }
        public bool? passedstatus { get; set; }
        public DateTime? createdAt { get; set; }
        public float? finalscore { get; set; }
        public string returnValue { get; set; }
        public DateTime? endtime { get; set; }
    }
}
