using Basketee.API.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.OrderPickup
{
    public class PickupOrderDto
    {
        public decimal grand_total { get; set; }
        public string invoice_number { get; set; }
        public string order_date { get; set; }
        public int order_id { get; set; }
        public short order_status { get; set; }
        public ProductsDto[] products { get; set; }
        public int has_exchange { get; set; }
        public List<ExchangeDto> exchange { get; set; }
    }
}
