﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.AgentBoss
{
    public class LoginRequest
    {
        public string mobile_number { get; set; }
        public string password { get; set; }
        public string app_id { get; set; }
        public string push_token { get; set; }       
    }
}
