using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ProgramTypeModel
    {
        public int ProgramTypeId { get; set; }
        public string ProgramTypeName { get; set; }
    }

    public class ProgramTypeViewModel
    {
        public List<ProgramTypeModel> ListProgramTypeModel { get; set; }
        public int TotalItem { get; set; }
    }
}
