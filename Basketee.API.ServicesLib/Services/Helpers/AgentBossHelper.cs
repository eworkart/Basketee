using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.AgentBoss;
using Basketee.API.Models;

namespace Basketee.API.Services.Helpers
{
    public class AgentBossHelper
    {
        //public static void CopyToEntity(AgentAdmin agent, LoginRequest request)
        //{
        //    agent.Password = request.password; // app_id
        //    agent.MobileNumber = request.mobile_number;
        //    agent.AppToken = request.push_token;
        //}
        public static void CopyFromEntity(AgentBossLoginDto dto, AgentBoss agentBoss)
        {
            dto.auth_token = agentBoss.AccToken;
            dto.user_id = agentBoss.AbosID;
            if (dto.user_details == null)
            {
                dto.user_details = new AgentBossDetailsDto();
                CopyFromEntity(dto.user_details, agentBoss);
            }
        }
        //public static void CopyToEntity(AgentAdmin agent, ForgotPasswordRequest request)
        //{
        //    agent.MobileNumber = request.mobile_number;
        //}
        //public static void CopyFromEntity(ResetPasswordDto dto, AgentAdmin agent)
        //{
        //    // password_reset, password_otp_sent
        //}
        //public static void CopyToEntity(AgentAdmin agent, ChangePasswordAgentAdminRequest request)
        //{
        //    // old_password, new_password, user_id, auth_token
        //}
        //public static void CopyToEntity(AgentAdmin agent, ChangeProfileAgentAdminRequest request)
        //{
        //    agent.ProfileImage = request.profile_image; // user_id, auth_token
        //    agent.MobileNumber = request.mobile_number;
        //    agent.AgentAdminName = request.agent_admin_name;
        //}
        //public static void CopyToEntity(AgentAdmin agent, GetAgentAdminDetailsRequest request)
        //{
        //    //  user_id, auth_token
        //}
        public static void CopyFromEntity(AgentBossDetailsDto dto, AgentBoss agentBoss)
        {
            dto.profile_image = ImagePathService.agentBossImagePath + agentBoss.ProfileImage;
            dto.agent_boss_name = agentBoss.OwnerName;
            dto.agency_name = agentBoss.Agency.AgencyName;
            dto.agentboss_email = agentBoss.Email != null ? agentBoss.Email : string.Empty;
            dto.agentboss_phone = agentBoss.MobileNumber;
        }
    }
}
