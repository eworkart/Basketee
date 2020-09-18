using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.DTOs.TeleOrder
{
    public class ConfirmTeleOrderResponse : ResponseDto
    {
        public TeleOrderFullDetailsDto orders { get; set; }
    }
}