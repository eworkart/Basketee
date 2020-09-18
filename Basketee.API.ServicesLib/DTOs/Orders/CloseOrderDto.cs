using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class CloseOrderDto
    {
        public int order_id { get; set; }
        public string order_type { get; set; }
        public int order_status_id { get; set; }
    }
}
