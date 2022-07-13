using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Enums
{
    public class ProductSegmentEnum
    {
        public static string Low = "Low";
        public static string Medium = "Medium";
        public static string High = "High";
        public static string Luxury = "Luxury";
        public const int LowId = 1;
        public const int MediumId = 2;
        public const int HighId = 3;
        public const int LuxuryId = 4;

    }

    [Flags]
    public enum ProductSegments
    {
        [Description("Low")]
        Low = 1,

        [Description("Medium")]
        Medium = 2,

        [Description("High")]
        High = 3,

        [Description("Luxury")]
        Luxury = 4,
    }
}
