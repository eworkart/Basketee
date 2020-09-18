using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Gen
{
    public class ResendOtpResponse : ResponseDto
    {
        public OTPDetailsDto otp_details { get; set; }
    }
}
