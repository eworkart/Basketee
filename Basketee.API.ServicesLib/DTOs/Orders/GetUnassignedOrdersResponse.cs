﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetUnassignedOrdersResponse : ResponseDto
    {
        public ActiveOrdersDto active_orders { get; set; }
    }
}