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
    
    public partial class MilkSale
    {
        public int ID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Date { get; set; }
        public Nullable<decimal> PerLtrPrice { get; set; }
        public int CustomerID { get; set; }
        public Nullable<decimal> Earn { get; set; }
        public string Note { get; set; }
        public string CattleType { get; set; }
    }
}
