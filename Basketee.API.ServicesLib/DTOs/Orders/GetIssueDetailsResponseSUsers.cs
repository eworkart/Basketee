using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetIssueDetailsResponseSUsers : ResponseDto
    {
        public SUserOrderDetailsDto order_details { get; set; }
        public SUserProductDetailsDto[] product_details { get; set; }
        public SUserAgencyDetailsDto agency_details { get; set; }
        public int has_exchange { get; set; }
        public ExchangeDto[] exchange { get; set; }
    }
}
