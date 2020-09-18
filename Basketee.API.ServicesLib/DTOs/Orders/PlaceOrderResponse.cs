using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class PlaceOrderResponse : ResponseDto
    {
        public PlaceOrderRespOrderDto order_details { get; set; }
    }
}