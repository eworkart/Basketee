using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class ProductDetailsDto
    {
        public string product_name { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal sub_total { get; set; }
        public decimal product_promo { get; set; }
        public decimal shipping_cost { get; set; }
        public decimal shipping_promo { get; set; }
    }
}