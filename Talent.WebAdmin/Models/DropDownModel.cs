using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Talent.WebAdmin.Models
{
    public class DropDownModel
    {
        //filter item
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class DropDownStringModel
    {
        //filter item
        public string Id { get; set; }
        public string Name { get; set; }

    }
}
