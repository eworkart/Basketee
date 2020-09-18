using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class ConfirmOrderResponse : ResponseDto
    {
        public DriverDetailsDto driver_details { get; set; }
        public OrderFullDetailsDto order_details { get; set; }
        public ProductsDto products { get; set; }
    }
}