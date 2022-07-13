using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class MasterNewsFormModel
    {
        //filter item
        public int? NewsId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string NewsTitle { get; set; }
        public string NewsLink { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int NewsCategoryId { get; set; }
        public bool IsDownloadable { get; set; }
        public Guid? ThumbnailBlobId { get; set; }
        public Guid? FileBlobId { get; set; }

        public FileContent ThumbnailUpload { get; set; }

        public FileContent FileUpload { get; set; }

        // //file url
        // public string ThumbnailUrl { get; set; }
        // public string fileUrl { get; set; }
        // public string ThumbnailName { get; set; }
        // public string FileName { get; set; }

        // //file
        // public IFormFile Thumbnail { get; set; }
        // public IFormFile File { get; set; }

    }
}
