using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.TeleOrder
{
    public class CustomerDetailsDto
    {
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string customer_mobile { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}