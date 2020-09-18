using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class ResendOtpResponseUser : ResponseDto
    {
        public OtpDetailsDto otp_details { get; set; }
    }
}