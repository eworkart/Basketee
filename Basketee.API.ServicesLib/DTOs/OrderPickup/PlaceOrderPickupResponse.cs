using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.DTOs.OrderPickup
{
    public class PlaceOrderPickupResponse : ResponseDto
    {
        public PickupOrderResponseDto order_details { get; set; }
    }
}