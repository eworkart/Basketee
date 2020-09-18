using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Agent
{
    public class AgentAdminDetailsDto
    {
        public int agent_admin_id { get; set; }
        public string profile_image { get; set; }
        public string agent_admin_name { get; set; }
        public string mobile_number { get; set; }
        public string agent_admin_email { get; set; }
        public string agency_name { get; set; }
    }
}