using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class DeleteProductRequest
    {
        public int suser_id { get; set; }
        public string auth_token { get; set; }
        public int product_id { get; set; }
    }
}