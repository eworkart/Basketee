using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.AgentBoss
{
    public class ForgotPasswordRequestAgentBoss
    {
        public string mobile_number { get; set; }
        public int user_id { get; set; }
        public string auth_token { get; set; }
    }
}
