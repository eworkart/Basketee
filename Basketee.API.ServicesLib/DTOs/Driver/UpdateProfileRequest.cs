using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class UpdateProfileRequest : AuthBase
    {
        public string profile_image { get; set; }
        public string driver_name { get; set; }
        public string mobile_number { get; set; }
    }
}
