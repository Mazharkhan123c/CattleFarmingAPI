using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CattleFarmingAPI.Models
{
    public class CattleMilkInfo
    {
     
            public string Tag { get; set; }
            public decimal TotalMilk { get; set; }
        public bool IsAvailableToGiveMilk { get; set; }

    }
}