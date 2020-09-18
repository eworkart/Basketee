using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class OrderDto
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public decimal grand_total { get; set; }
        public string order_date { get; set; }
        public TimeSpan order_time { get; set; }
    }
}