using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.AgentBoss
{
    public class ChangeProfileAgentBossRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public string profile_image { get; set; }
        public string agent_boss_name { get; set; }
        public string mobile_number { get; set; }
        public string agent_boss_email { get; set; }
    }
}
