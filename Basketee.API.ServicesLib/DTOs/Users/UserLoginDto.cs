using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UserLoginDto
    {
        public int user_exists { get; set; }
        public int user_activated { get; set; }
        public int user_blocked { get; set; }
        public int user_id { get; set; }
        public int allow_login { get; set; }
        public string auth_token { get; set; }
        //public string name { get; set; }
        //public string profile_image { get; set; }
        //public string mobile_no { get; set; }
        //public string address { get; set; }
        //public string region_name { get; set; }
        //public string postal_code { get; set; }
    }
}