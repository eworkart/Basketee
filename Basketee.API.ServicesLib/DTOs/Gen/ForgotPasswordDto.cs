using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Gen
{
    public class ForgotPasswordDto
    {
        public int password_reset { get; set; }
        public int new_password_otp_sent { get; set; }
    }
}
