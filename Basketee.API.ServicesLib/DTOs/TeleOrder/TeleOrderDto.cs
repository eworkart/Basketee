using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.TeleOrder
{
    public class TeleOrderDto
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
        public string order_status { get; set; }
    }
}
