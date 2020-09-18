using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UserStatusDto
    {
        public int user_exists { get; set; }
        public string auth_token { get; set; }
        public int user_id { get; set; }
    }
}