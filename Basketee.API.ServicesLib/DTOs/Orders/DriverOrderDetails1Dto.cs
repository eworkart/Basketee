using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
     public class DriverOrderDetailDto
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public string order_date { get; set; }
        public string consumer_name { get; set; }
        public string consumer_mobile { get; set; }
        public string consumer_address { get; set; }
        public string order_type { get; set; }
        public string order_time { get; set; }
        public decimal grand_total { get; set; }
        public int delivery_timeslot_id { get; set; }
        public int delivery_status_id { get; set; }
        public int order_status_id { get; set; }
        public string agent_admin_phone_no { get; set; }
    }
}
