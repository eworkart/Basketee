using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class ProductDto
    {
        public int product_id { get; set; }
        public string product_title { get; set; }
        public string product_image { get; set; }
        public decimal Price_refil { get; set; }
        public decimal price_of_the_tube { get; set; }
    }
}