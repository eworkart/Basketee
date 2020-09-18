using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class ResetPasswordDto
    {
        public int password_reset { get; set; }
        public int new_password_otp_sent { get; set; }
    }
}