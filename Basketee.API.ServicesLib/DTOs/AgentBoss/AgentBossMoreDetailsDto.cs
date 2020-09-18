using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.AgentBoss
{
    public class AgentBossMoreDetailsDto : AgentBossDetailsDto
    {
        public int agent_boss_id { get; set; }
        public string mobile_number { get; set; }
        public string agent_boss_email { get; set; }
    }
}
