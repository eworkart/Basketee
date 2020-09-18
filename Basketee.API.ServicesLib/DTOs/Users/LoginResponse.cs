using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class LoginResponse : ResponseDto
    {
        public UserLoginDto user_login { get; set; }
        public UserLoginDetailsDto user_details { get; set; }
    }
}