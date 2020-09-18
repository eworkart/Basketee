using Basketee.API.DTOs;
using Basketee.API.DTOs.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class GetProductListResponse : ResponseDto
    {
        public ProductPagenationDetailsDto product_details { get; set; }
        public ProductDto[] products { get; set; }
        public int has_reminder { get; set; }
        public int reminder_id { get; set; }
        public string reminder_image { get; set; }
        public string reminder_description { get; set; }
        //public int has_exchange { get; set; }
        //public List<ExchangeDto> exchange { get; set; }
    }
}