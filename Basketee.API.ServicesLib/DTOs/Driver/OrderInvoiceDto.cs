using Basketee.API.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class OrderInvoiceDtoDriver
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public string order_date { get; set; }
        public string consumer_name { get; set; }
        public string consumer_mobile { get; set; }
        public string consumer_address { get; set; }
        public string consumer_location { get; set; }
        public int agency_id { get; set; }
        public string agency_name { get; set; }
        public string agency_address { get; set; }
        public string agency_location { get; set; }
        public ProductsDto[] products { get; set; }
        public decimal grand_total { get; set; }
        public decimal grand_discount { get; set; }
        public decimal grand_total_with_discount { get; set; }
        public int has_exchange { get; set; }
        public List<ExchangeDto> exchange { get; set; }
    }
}
