using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CattleFarmingAPI.Models
{
    public class CattleInfo
    {
      //  public int ID { get; set; }
        public string Tag { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Age { get; set; }
        public string VaccineType { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
      
    }
}