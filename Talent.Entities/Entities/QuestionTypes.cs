using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class QuestionTypes
    {
        public QuestionTypes()
        {
            Tasks = new HashSet<Tasks>();
        }

        [Key]
        public int QuestionTypeId { get; set; }
        [Required]
        [StringLength(64)]
        public string QuestionTypeName { get; set; }

        [InverseProperty("QuestionType")]
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
