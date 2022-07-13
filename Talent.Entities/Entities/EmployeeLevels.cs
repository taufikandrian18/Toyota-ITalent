using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class EmployeeLevels
    {
        [Key]
        public int EmployeeLevelId { get; set; }
        public int? MinValue { get; set; }
    }
}
