using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Basketee.API.DTOs;
using Basketee.API.DTOs.AgentBoss;
using Basketee.API.Services;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Controllers
{
    public class AgentBossController : ApiController
    {
        [HttpPost]
        [ActionName("login")]
        public NegotiatedContentResult<LoginResponse> PostLogin([FromBody]LoginRequest request)
        {
            LoginResponse resp = AgentBossServices.Login(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("forgot_password")]
        public NegotiatedContentResult<ForgotPasswordResponse> PostForgotPassword([FromBody]ForgotPasswordRequest request)
        {
            ForgotPasswordResponse resp = AgentBossServices.ForgotPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("reset_password")]
        public NegotiatedContentResult<ResponseDto> PostResetPassword([FromBody]ResetPasswordRequest request)
        {
            ResponseDto resp = AgentBossServices.ResetPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_password_agent_boss")]
        public NegotiatedContentResult<ResponseDto> PostChangePassword([FromBody]ChangePasswordAgentBossRequest request)
        {
            ResponseDto resp = AgentBossServices.ChangePassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_profile_agent_boss")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfile([FromBody]ChangeProfileAgentBossRequest request)
        {
            ResponseDto resp = AgentBossServices.ChangeProfile(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_agent_boss_details")]
        public NegotiatedContentResult<GetAgentBossDetailsResponse> PostGetDetails([FromBody]GetAgentBossDetailsRequest request)
        {
            GetAgentBossDetailsResponse resp = AgentBossServices.GetDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("check_otp")]
        public NegotiatedContentResult<ResponseDto> PostCheckOTP([FromBody]CheckOtpRequest request)
        {
            ResponseDto resp = AgentBossServices.CheckOTP(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("resend_otp")]
        public NegotiatedContentResult<ResendOtpResponse> PostResendOtp([FromBody]ResendOtpRequest request)
        {
            ResendOtpResponse resp = AgentBossServices.ResendOtp(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_profilephoto_agent_boss")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfilePhoto([FromBody]ChangeProfilePhotoRequest request)
        {
            ResponseDto resp = AgentBossServices.ChangeProfilePhoto(request);
            return Content(HttpStatusCode.OK, resp);
        }
    }
}
