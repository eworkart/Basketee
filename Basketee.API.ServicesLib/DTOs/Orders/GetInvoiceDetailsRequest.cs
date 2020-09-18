using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetInvoiceDetailsRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public int order_id { get; set; }
        public string order_type { get; set; }
        public int is_agent_admin { get; set; }
    }
}