using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CattleFarmingAPI.Models
{
    public class ViewCattle
    {
     //   public int ID { get; set; }
        public String Tag { get; set; }
        public String CattleType { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public String isVaccinated { get; set; }
    }
}