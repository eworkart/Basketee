using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Agent;
using Basketee.API.Models;

namespace Basketee.API.Services.Helpers
{
    public class AgentHelper
    {
        public static void CopyToEntity(AgentAdmin agent, LoginRequest request)
        {
            agent.Password = request.password; // app_id
            agent.MobileNumber = request.mobile_number;
            agent.AppToken = request.push_token;
        }
        public static void CopyFromEntity(UserLoginDto dto, AgentAdmin agent)
        {
            dto.auth_token = agent.AccToken;
            dto.user_id = agent.AgadmID;
            if (dto.agent_admin_details == null)
            {
                dto.agent_admin_details = new AgentAdminDetailsDto();
                CopyFromEntity(dto.agent_admin_details, agent);
            }
        }
        //public static void CopyToEntity(AgentAdmin agent, ForgotPasswordRequest request)
        //{
        //    agent.MobileNumber = request.mobile_number;
        //}
        public static void CopyFromEntity(ResetPasswordDto dto, AgentAdmin agent)
        {
            // password_reset, password_otp_sent
        }
        public static void CopyToEntity(AgentAdmin agent, ChangePasswordAgentAdminRequest request)
        {
            // old_password, new_password, user_id, auth_token
        }
        public static void CopyToEntity(AgentAdmin agent, ChangeProfileAgentAdminRequest request)
        {
            agent.ProfileImage = request.profile_image; // user_id, auth_token
            agent.MobileNumber = request.mobile_number;
            agent.AgentAdminName = request.agent_admin_name;
        }
        public static void CopyToEntity(AgentAdmin agent, GetAgentAdminDetailsRequest request)
        {
            //  user_id, auth_token
        }
        public static void CopyFromEntity(AgentAdminDetailsDto dto, AgentAdmin agent)
        {
            dto.agent_admin_id = agent.AgenID;
            dto.profile_image = ImagePathService.agentAdminImagePath + agent.ProfileImage;
            dto.agent_admin_name = agent.AgentAdminName;
            dto.mobile_number = agent.MobileNumber;
            dto.agent_admin_email = agent.email;
            dto.agency_name = agent.Agency != null ? agent.Agency.AgencyName : string.Empty;
        }
    }
}