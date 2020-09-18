using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class AddProductRequest
    {
        public int suser_id { get; set; }
        public string auth_token { get; set; }
        public string product_name { get; set; }
        public int position { get; set; }
        public string product_image { get; set; }
        public string product_image_details { get; set; }
        public decimal tube_price { get; set; }
        public decimal tube_promo_price { get; set; }
        public decimal refill_price { get; set; }
        public decimal refill_promo_price { get; set; }
        public decimal shipping_price { get; set; }
        public decimal shipping_promo_price { get; set; }
        public int published { get; set; }
        public ExchangeDto exchange { get; set; }
    }
}