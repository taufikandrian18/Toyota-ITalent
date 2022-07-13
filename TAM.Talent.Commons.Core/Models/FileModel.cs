using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.Talent.Commons.Core.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string Mime { get; set; }
        public string FileUrl { get; set; }
    }
}