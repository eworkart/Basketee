using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetDriverOrderResponse : ResponseDto
    {
        public DriverOrderDetailDto order_details { get; set; }
        public DriverProductsDto[] products { get; set; }
        public int has_exchange { get; set; }
        public List<ExchangeDto> exchange { get; set; }
    }
}
