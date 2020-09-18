using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Agent
{
    public class UserLoginDto
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public AgentAdminDetailsDto agent_admin_details { get; set; }
    }
}