using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Accommodations
    {
        public Accommodations()
        {
            TrainingProcesses = new HashSet<TrainingProcesses>();
        }

        [Key]
        public int AccommodationId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int Price { get; set; }

        [InverseProperty("Accomodation")]
        public virtual ICollection<TrainingProcesses> TrainingProcesses { get; set; }
    }
}
