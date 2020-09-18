using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UserExistsRequest
    {
        public string mobile_number { get; set; }
        public string imei { get; set; }
    }
}