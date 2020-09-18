using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class OutForDeliveryRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public int order_id { get; set; }
        public string order_type { get; set; }
    }
}
