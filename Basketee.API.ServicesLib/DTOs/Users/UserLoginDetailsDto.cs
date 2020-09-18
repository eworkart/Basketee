using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Users
{
    public class UserLoginDetailsDto
    {
        public string user_name { get; set; }
        public string profile_image { get; set; }
        public string mobile_number { get; set; }
        public string user_address { get; set; }
        public string region_name { get; set; }
        public string postal_code { get; set; }
    }
}
