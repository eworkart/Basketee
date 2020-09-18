using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class DriverDetailsDto
    {
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public string driver_image { get; set; }
        public string driver_mobile { get; set; }
        public string driver_profile_image { get; set; }
        public decimal driver_rating { get; set; }
    }
}