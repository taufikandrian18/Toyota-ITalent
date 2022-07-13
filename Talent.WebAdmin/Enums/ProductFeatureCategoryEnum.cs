using System;
using System.ComponentModel;

namespace Talent.WebAdmin.Enums
{
    public class ProductFeatureCategoryEnum
    {
        [Flags]
        public enum ProductFeatureCategories
        {
            [Description("Interior")]
            Interior = 1,

            [Description("Exterior")]
            Exterior = 2,

            [Description("Safety")]
            Safety = 3,

            [Description("Engine")]
            Engine = 4,

            [Description("Performance")]
            Performance = 5,
        }
    }
}
