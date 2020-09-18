using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class DriverDto
    {
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public string driver_image { get; set; }
        public decimal driver_rating { get; set; }
    }
}
