using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    /// <summary>
    /// OrderList Response
    /// </summary>
    public class GetOrderListBossResponse : ResponseDto
    {
        public OrdersBossDto orders { get; set; }
        public List<OrderDetailsBossDto> active_orders { get; set; }
        public List<OrderDetailsBossDto> history_orders { get; set; }
    }
}

