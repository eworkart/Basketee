using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetOrgOrderDetailsResponse : ResponseDto
    {
        public OrderFullDetailsDto order_details { get; set; }
    }
}