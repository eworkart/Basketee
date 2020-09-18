using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.DTOs.TeleOrder
{
    public class AddTeleOrderRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public ProductsDto[] products { get; set; }
        public CustomerDetailsDto customer_details { get; set; }
        public bool has_exchange { get; set; }
        public ExchangeDto[] exchange { get; set; }
        public int driver_id { get; set; }
        public int time_slot_id { get; set; }
        public DateTime delivery_date { get; set; }
    }
}