//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CattleFarmingAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cattle
    {
        public string Tag { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Gender { get; set; }
        public string CattleType { get; set; }
        public string DOB { get; set; }
        public string DateOfEntryFarm { get; set; }
        public string CattleBreed { get; set; }
        public string CattleObtained { get; set; }
        public int FarmID { get; set; }
        public string Status { get; set; }
        public Nullable<int> Cost { get; set; }
        public Nullable<int> CostOfDead { get; set; }
    }
}
