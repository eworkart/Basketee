using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetTimeslotRequest
    {
        public int user_id { get; set; }
        public int user_address_id { get; set; }
        public string auth_token { get; set; }
    }
}