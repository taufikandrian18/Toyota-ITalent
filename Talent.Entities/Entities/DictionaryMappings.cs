using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class DictionaryMappings
    {
        public Guid DictionaryId { get; set; }
        public string EmployeeId { get; set; }

        [ForeignKey("DictionaryId")]
        [InverseProperty("DictionaryMappings")]
        public virtual Dictionaries Dictionary { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("DictionaryMappings")]
        public virtual Employees Employee { get; set; }
    }
}
