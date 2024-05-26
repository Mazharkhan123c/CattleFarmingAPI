using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CattleFarmingAPI.Models
{
    public class AverageMilkOfCattle
    {
        public String Tag { get; set; }
        public String CattleType { get; set; }
        public Nullable<decimal> AvgMilk { get; set; }
     
    }
}