using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.OrderPickup
{
    public class ConfirmPickupOrderRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public int order_id { get; set; }
    }
}