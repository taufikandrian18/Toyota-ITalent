using System;
using System.ComponentModel;

namespace Talent.WebAdmin.Enums
{
    public class ProductGalleryApprovalStatusesEnum
    {
        [Flags]
        public enum ProductGalleryApprovals
        {
            [Description("Draft")]
            Draft = 1,

            [Description("Rejected")]
            Rejected = 2,

            [Description("Published")]
            Published = 3,
        }
    }
}
