using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class DriverDetailListDto
    {
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public string driver_availability { get; set; }
        public decimal driver_rating { get; set; }
        public string driver_profile_image { get; set; }        
    }
}
