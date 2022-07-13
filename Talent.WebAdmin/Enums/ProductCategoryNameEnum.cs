using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Talent.WebAdmin.Enums
{
    public class ProductCategoryNameEnum
    {
        public static string AllCategories = "All";
        public static string SuvCategories = "SUV";
        public static string SedanCategories = "Sedan";
        public static string MpvCategories = "MPV";
        public static string HatchbackCategories = "Hatchback";
        public static string CommercialCategories = "Commercial";
    }

    [Flags]
    public enum ProductCategoriesEnum
    {
        [Description("All")]
        All = 1,

        [Description("SUV")]
        SUV = 2,

        [Description("Sedan")]
        Sedan = 3,

        [Description("MPV")]
        MPV = 4,

        [Description("Hatchback")]
        Hatchback = 5,

        [Description("Commercial")]
        Commercial = 6,
    }
}
