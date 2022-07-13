using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideAcomodationModel
    {
    }

    public class UserSideAcomodationListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        //harga price untuk berapa orang
        public int Each { get; set; }
        //buat di front end show
        //public string EachString { get; set; }
    }

    public class UserSideAcomodationConfirmModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
