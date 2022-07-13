using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class OnBoardingModel
    {
        public string ID { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
    }

    public class OnBoardingModelList
    {
        public List<OnBoardingModel> onBoardings { get; set; }
    }


}
