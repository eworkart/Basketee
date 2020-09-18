using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Agent
{
    public class ResetPasswordDto
    {
        public int password_reset { get; set; }
        public int password_otp_sent { get; set; }
    }
}