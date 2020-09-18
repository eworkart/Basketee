using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class PlaceOrderRespOrderDto
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public string order_status { get; set; }
    }
}