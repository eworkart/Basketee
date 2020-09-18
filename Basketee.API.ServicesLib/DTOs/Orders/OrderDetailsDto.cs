using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class OrderDetailsDto : OrderDto
    {
        public string delivery_date { get; set; }
        public string time_slot_name { get; set; }
        public string delivery_status { get; set; }
        public string order_status { get; set; }
        public string agentadmin_mobile { get; set; }
        public DriverDetailsDto driver_details { get; set; }
        public ProductsDto[] product_details { get; set; }
        public int has_exchange { get; set; }
        public List<ExchangeDto> exchange { get; set; }
    }
}