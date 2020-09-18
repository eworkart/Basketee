using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class CheckOtpRequest
    {
        //public int user_id { get; set; }
        public string mobile_number { get; set; }
        //public string imei { get; set; }
        public string otp_code { get; set; }
    }
}