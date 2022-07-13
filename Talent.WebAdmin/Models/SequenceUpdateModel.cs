using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class SequenceUpdateModel
    {
        public TaskModel Task { get; set; }

        [Required]
        [StringLength(2000)]
        public string Question { get; set; }

        public string Answer { get; set; }

        public int CompetencyTypeId { get; set; }

        public List<SequenceChoiceModel> Choices { get; set; }

        public FileContent FileContent { get; set; }
    }
}
