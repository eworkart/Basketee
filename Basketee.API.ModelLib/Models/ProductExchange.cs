//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Basketee.API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductExchange
    {
        public int PrExID { get; set; }
        public int ProdID { get; set; }
        public string ExchangeWith { get; set; }
        public int ExchangeQuantity { get; set; }
        public Nullable<decimal> ExchangePrice { get; set; }
        public decimal ExchangePromoPrice { get; set; }
        public bool StatusId { get; set; }
        public short CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public short UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual SuperAdmin SuperAdmin { get; set; }
        public virtual SuperAdmin SuperAdmin1 { get; set; }
    }
}
