using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetOrderListRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public int current_list { get; set; }
    }
}