﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetDriverOrderListRequest
    {
        public int user_id { get; set; }
        public int current_list { get; set; }
        public string auth_token { get; set; }
    }
}
