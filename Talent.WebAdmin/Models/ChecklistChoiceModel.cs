using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ChecklistChoiceModel
    {
        public int TaskId { get; set; }

        public int Number { get; set; }

        public string Text { get; set; }

        public bool IsAnswer { get; set; }

        public int Score { get; set; }

    }
}
