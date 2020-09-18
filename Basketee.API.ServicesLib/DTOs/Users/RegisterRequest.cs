using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class RegisterRequest
    {
        public string user_name { get; set; }
        public string mobile_number { get; set; }
        public string user_password { get; set; }
        public string imei { get; set; }
    }
}