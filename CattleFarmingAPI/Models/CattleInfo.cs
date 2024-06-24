using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CattleFarmingAPI.Models
{
    public class CattleInfo
    {
        public string Weight { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseCost { get; set; }
        public string VaccineType { get; set; }
        public Nullable<System.DateTime> VaccineDate { get; set; }
    }
}