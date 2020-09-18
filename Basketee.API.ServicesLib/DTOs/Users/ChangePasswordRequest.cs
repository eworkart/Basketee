using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class ChangePasswordRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
}