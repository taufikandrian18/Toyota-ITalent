using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class BannerModel
    {

        public int BannerId { get; set; }
        public int BannerTypeId { get; set; }
        public Guid BlobId { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MobilePageId { get; set; }
        public int? ContentId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ActionAt { get; set; }

        public int? ApprovalStatusId { get; set; }
        public string BannerTypeName { get; set; }
        public string ApprovalStatus { get; set; }
        public string ActionBy { get; set; }

        public string PageName { get; set; }
        public string BlobName { get; set; }
        public string Mime { get; set; }


    }

    public class BannerTypesModel
    {
        public int BannerTypeId { get; set; }
        public string Name { get; set; }
    }

    public class BannerFormModel
    {

        [Key]
        public int? BannerId { get; set; }
        public int BannerTypeId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(1024)]
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MobilePageId { get; set; }
        public int? ContentId { get; set; }
        [Required]
        [StringLength(1024)]
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        [StringLength(32)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public string BannerTypeName { get; set; }
        public string ApprovalStatus { get; set; }
        public string ActionBy { get; set; }
        public int InsertType { get; set; }

        public FileContent ImageUpload { get; set; }

    }

    public class BannerViewModel
    {
        public List<BannerModel> ListBanner { get; set; }
        public int TotalData { get; set; }

    }

    public class BannerFilterModel : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DateFilterStart { get; set; }
        public DateTimeOffset? DateFilterEnd { get; set; }
        public int? BannerTypeId { get; set; }
        public string Name { get; set; }
        public string ApprovalStatus { get; set; }
        public string ActionBy { get; set; }

    }

    public class ContentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MobileMappingPage
    {
        public int MobilePageId { get; set; }
        public string Name { get; set; }
    }

    public class ContentFilterModel
    {
        public int TypeContent { get; set; }
        public string Name { get; set; }
    }

    public class ContentWithIdFilterModel
    {
        public int TypeContent { get; set; }
        public int ContentId { get; set; }
    }

    public class ApprovalStatusForBannerModel
    {
        public int ApprovalStatusId { get; set; }
        public string ApprovalName { get; set; }
    }
}
