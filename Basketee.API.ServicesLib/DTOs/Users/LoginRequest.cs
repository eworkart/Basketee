using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class LoginRequest
    {
        public string mobile_number { get; set; }
        public string user_password { get; set; }
        public string app_id { get; set; }
        public string push_token { get; set; }
    }
}
