using Basketee.API.DTOs.TeleOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.TeleOrder
{
    public class AddTeleOrderResponse : ResponseDto
    {
        public TeleOrderDto order_details { get; set; }
    }
}