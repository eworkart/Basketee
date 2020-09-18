using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.AgentBoss
{
   public class GetAgentBossDetailsResponse : ResponseDto
    {
        public AgentBossMoreDetailsDto agent_boss_details { get; set; }
    }
}
