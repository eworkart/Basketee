using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
   public class SUserAgencyDetailsDto 
    {
        public int agency_id { get; set; }
        public string agency_name { get; set; }
        public string agency_address { get; set; }
        public string agency_mobile { get; set; }

    }
}
