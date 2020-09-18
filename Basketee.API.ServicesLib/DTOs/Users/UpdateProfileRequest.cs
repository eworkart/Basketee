using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UpdateProfileRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public string profile_image { get; set; }
        public string user_name { get; set; }
        public string user_mobile_number { get; set; }
    }
}