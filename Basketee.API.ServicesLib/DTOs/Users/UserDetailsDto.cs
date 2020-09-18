using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UserDetailsDto
    {
        public int user_id { get; set; }
        public int user_activated { get; set; }
        public int user_blocked { get; set; }
        public string user_name { get; set; }
        public List<UserAddressesDto> user_addresses { get; set; }
        public string user_mobile_num { get; set; }
        public DateTime user_created_date { get; set; }
        public DateTime user_last_login { get; set; }
        public string user_latitude { get; set; }
        public string user_longitude { get; set; }
        public string profile_image { get; set; }
    }
}