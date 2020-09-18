using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class ResetPasswordDto
    {
        public int password_reset { get; set; }
        public int password_otp_sent { get; set; }
    }
}
