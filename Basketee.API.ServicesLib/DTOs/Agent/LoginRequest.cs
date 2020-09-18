using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Agent
{
    public class LoginRequest
    {
        public string mobile_number { get; set; }
        public string password { get; set; }
        public string app_id { get; set; }
        public string push_token { get; set; }
        public int user_id { get; set; }
        public string auth_token { get; set; }
    }
}