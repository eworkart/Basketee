﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class OrdersDto
    {
        public OrgOrderListDto[] orders { get; set; }
    }
}