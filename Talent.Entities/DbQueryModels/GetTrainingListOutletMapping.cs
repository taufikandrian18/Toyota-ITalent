using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetTrainingListOutletMapping
    {
        public int TrainingId { get; set; }
        public int OutletId { get; set; }
        public int DealerID { get; set; }
    }
}
