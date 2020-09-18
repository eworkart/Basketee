using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.OrderPickup
{
    public class PickupOrderResponseDto
    {
        public int order_id { get; set; }
        public string invoice_number { get; set; }
    }
}
