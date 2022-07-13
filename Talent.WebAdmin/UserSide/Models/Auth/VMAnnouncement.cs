using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
  public class VMANnouncement
  {
    public bool? ExpTime30 { get; set; }
    public bool? ExpTime7 { get; set; }
    public bool? ChangePswd3M { get; set; }
    public bool? LoginPoints { get; set; }
    public bool? Announcement { get; set; }
    public bool? ChangePswd { get; set; }
  }


}