using Basketee.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class GetProductDetailsResponse : ResponseDto
    {
        public ProductDetailsDto product_details { get; set; }
    }
}