using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetOrgOrderListResponse : ResponseDto
    {
        public OrdersBossDto orders { get; set; }
        public List<OrderDetailsBossDto> active_orders { get; set; }
        public List<OrderDetailsBossDto> history_orders { get; set; }
    }
}