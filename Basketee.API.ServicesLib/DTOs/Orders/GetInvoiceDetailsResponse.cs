using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetInvoiceDetailsResponse : ResponseDto
    {
        public OrderInvoiceDto orders { get; set; }
    }
}