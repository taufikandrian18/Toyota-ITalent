using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class BlobModel
    {
        public Guid BlobId { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
    }

    public class BlobFormModel
    {
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        [Required]
        public string Mime { get; set; }
    }

    public class BlobViewModel
    {
        public List<BlobModel> ListBlobModel { get; set; }
        public int TotalItem { get; set; }
    }
}
