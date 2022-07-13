using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideBlobModel
    {
        public Guid BlobId { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
    }

    public class UserSideBlobFormModel
    {
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        [Required]
        public string Mime { get; set; }
    }

    public class UserSideBlobViewModel
    {
        public List<UserSideBlobModel> ListBlobModel { get; set; }
        public int TotalItem { get; set; }
    }
}
