using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.DTOs.OrderPickup
{
    public class ConfirmPickupOrderResponse : ResponseDto
    {
        public PickupOrderDto orders { get; set; }
    }
}