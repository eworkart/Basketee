using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Agent
{
    public class LoginResponse : ResponseDto
    {
        public UserLoginDto user_login { get; set; }
    }
}