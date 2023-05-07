using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crowfunding.Models
{
    public class customCompaignDetail
    {
      public  int id { get; set; }
      public  string title { get; set; }
      public Nullable<int> status { get; set; }
      public  int? totalDonation { get; set; }
      public int? withdrawnAmount { get; set; }
      public int? countPeopleDonated { get; set; }
      public int? currentBalance { get; set; }
    }
}