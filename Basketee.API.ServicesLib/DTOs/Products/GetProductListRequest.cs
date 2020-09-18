using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class GetProductListRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public bool is_admin { get; set; }
        public int row_per_page { get; set; }
        public int page_number { get; set; }
    }
}