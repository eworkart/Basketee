using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
   public class GetAllOrderDetailsRequest : AuthBase
    {
        public int current_list { get; set; }
        public int page_number { get; set; }
        public int records_per_page { get; set; }
    }
}
