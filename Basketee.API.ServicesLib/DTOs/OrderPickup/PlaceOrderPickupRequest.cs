using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.DTOs.OrderPickup
{
    public class PlaceOrderPickupRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public ProductsDto[] products { get; set; }
        public bool has_exchange { get; set; }
        //public List<PickupOrderExchangeDto> exchange { get; set; }
        public List<ExchangeDto> exchange { get; set; }
    }
}