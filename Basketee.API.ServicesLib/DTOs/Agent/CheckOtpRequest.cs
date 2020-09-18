using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Agent
{
    public class CheckOtpRequest
    {
        public string mobile_number { get; set; }
        public string otp_code { get; set; }
    }
}
