﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetActiveOrderCountBossResponse : ResponseDto
    {
        public ActiveOrdersBossDto active_orders { get; set; }
    }
}