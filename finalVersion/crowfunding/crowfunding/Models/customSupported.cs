using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crowfunding.Models
{
    public class customSupported
    {
      public int  compaignId  { get; set; }
      public  int  donationId { get; set; }
      public  string title { get; set; }
      public int? status { get; set; }
      public int? amount { get; set; }
    }
}