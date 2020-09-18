using Basketee.API.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class GetEReceiptResponse : ResponseDto
    {
        public OrderInvoiceDto orders { get; set; }
    }
}
