using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class OrderDto1
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public decimal grand_total { get; set; }
        public string delivery_date { get; set; }
        public string order_date { get; set; }
        public string consumer_name { get; set; }
        public string consumer_mobile { get; set; }
        public string consumer_address { get; set; }
        public string consumer_location { get; set; }
        public string delivery_time_slot { get; set; }
        public int agency_id { get; set; }
        public string agency_name { get; set; }
        public string agency_address { get; set; }
        public string agency_location { get; set; }
    }
}