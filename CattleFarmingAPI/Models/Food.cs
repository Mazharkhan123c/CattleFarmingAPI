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
    
    public partial class Food
    {
        public int ID { get; set; }
        public string Quantity { get; set; }
        public string Date { get; set; }
        public string Item { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string CattleTag { get; set; }
        public int FarmId { get; set; }
    }
}
