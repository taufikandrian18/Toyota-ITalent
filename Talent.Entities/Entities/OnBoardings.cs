using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class OnBoardings
    {
        public OnBoardings()
        {
        }

        [Key]
        public string OnBoardingID { get; set; }
        public int SectionNumber { get; set; }
        public string OnBoardingFileURL { get; set; }

        public string OnBoardingFileType { get; set; }
        public string  Description { get; set; }
        public string Title { get; set; }

        public bool? StatusView { get; set; }

    }
}
