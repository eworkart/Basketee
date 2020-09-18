using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class ProductsBossDto
    {
        public string product_name { get; set; }
        public decimal unit_price { get; set; }
        public int quantity { get; set; }
        public decimal sub_total { get; set; }
        public decimal product_promo { get; set; }
        public decimal shipping_cost { get; set; }
        public decimal shipping_promo { get; set; }
        public int refill_quantity { get; set; }
        public decimal refill_price { get; set; }
        public decimal refill_promo { get; set; }
        public decimal grand_total { get; set; }
    }
}

