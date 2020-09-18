using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class AllOrderDetails
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public string order_date { get; set; }
        public string order_time { get; set; }
        public string delivery_date { get; set; }
        public string time_slot_name { get; set; }
        public decimal grand_total { get; set; }
        public int order_status { get; set; }
        public int delivery_status { get; set; }
        public DriverDetailsDto driver_details { get; set; }
        public string agentadmin_mobile { get; set; }
        public string agentadmin_name { get; set; }
        public string agentadmin_image { get; set; }
        public List<ProductsDto> product_details { get; set; }
        public int has_exchange { get; set; }
        public List<ExchangeDto> exchange { get; set; }
    }
}
