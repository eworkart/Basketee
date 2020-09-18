using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class GetProductDetailsRequest
    {
        public string auth_token { get; set; }
        public int product_id { get; set; }
        public int user_id { get; set; }
        public bool is_admin { get; set; }
    }
}