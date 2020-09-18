using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetOrderDetailsBossRequest : AuthBase
    {
        public int order_id { get; set; }
        public string order_type { get; set; }
    }
}
