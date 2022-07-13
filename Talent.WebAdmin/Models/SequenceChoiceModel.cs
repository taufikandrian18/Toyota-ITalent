using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class SequenceChoiceModel
    {
        public int TaskSequenceChoiceId { get; set; }

        public int ChoiceNumber { get; set; }

        public string ChoiceText { get; set; }

        public int Score { get; set; }
    }
}
