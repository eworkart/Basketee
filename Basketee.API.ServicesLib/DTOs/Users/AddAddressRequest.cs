using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class AddAddressRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public string user_address { get; set; }
        public string region_name { get; set; }
        public string postal_code { get; set; }
        public string more_info { get; set; }
        public int is_default { get; set; }
        public string user_latitude { get; set; }
        public string user_longitude { get; set; }
    }
}