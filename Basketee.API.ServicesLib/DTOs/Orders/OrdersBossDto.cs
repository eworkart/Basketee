using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class OrdersBossDto
    {
        public int active_order_count { get; set; }
        public int history_order_count { get; set; }
    }
}
