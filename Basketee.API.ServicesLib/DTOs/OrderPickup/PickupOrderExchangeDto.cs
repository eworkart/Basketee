using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.OrderPickup
{
    public class PickupOrderExchangeDto
    {
        public int exchange_id { get; set; }
        public string exchange_with { get; set; }
        public int exchange_quantity { get; set; }
        public decimal exchange_price { get; set; }
        public decimal exchange_promo_price { get; set; }
    }
}
