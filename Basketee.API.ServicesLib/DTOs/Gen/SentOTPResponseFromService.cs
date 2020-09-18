using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Gen
{
    public class SentOTPResponseFromService
    {
        public string OTP { get; set; }
        public string NoTelp { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string DescMsg { get; set; }
    }
}
