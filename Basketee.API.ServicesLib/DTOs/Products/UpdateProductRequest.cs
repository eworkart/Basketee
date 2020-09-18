using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class UpdateProductRequest
    {
        public int suser_id { get; set; }
        public string auth_token { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int position { get; set; }
        public string product_image { get; set; }
        public string product_image_details { get; set; }
        public float tube_price { get; set; }
        public float tube_promo_price { get; set; }
        public float refill_price { get; set; }
        public float refill_promo_price { get; set; }
        public float shipping_price { get; set; }
        public float shipping_promo_price { get; set; }
        public int published { get; set; }
        public bool has_exchange { get; set; }
        public ExchangeDto exchange { get; set; }
    }
}