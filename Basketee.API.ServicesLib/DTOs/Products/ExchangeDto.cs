using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class ExchangeDto
    {
        public int exchange_id { get; set; }
        public string exchange_with { get; set; }
        public int exchange_quantity { get; set; }
        public decimal exchange_price { get; set; }
        public decimal exchange_promo_price { get; set; }
    }
}