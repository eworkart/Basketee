using Basketee.API.DTOs;
using Basketee.API.DTOs.Agent;
using Basketee.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Controllers
{
    public class AgentAdminController : ApiController
    {
        [HttpPost]
        [ActionName("login")]
        public NegotiatedContentResult<LoginResponse> PostLogin([FromBody]LoginRequest request)
        {
            LoginResponse resp = AgentAdminServices.Login(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("forgot_password")]
        public NegotiatedContentResult<ForgotPasswordResponse> PostForgotPassword([FromBody]ForgotPasswordRequest request)
        {
            ForgotPasswordResponse resp = AgentAdminServices.ForgotPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_password_agent_admin")]
        public NegotiatedContentResult<ResponseDto> PostChangePassword([FromBody]ChangePasswordAgentAdminRequest request)
        {
            ResponseDto resp = AgentAdminServices.ChangePassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_profile_agent_admin")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfile([FromBody]ChangeProfileAgentAdminRequest request)
        {
            ResponseDto resp = AgentAdminServices.ChangeProfile(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_agent_admin_details")]
        public NegotiatedContentResult<GetAgentAdminDetailsResponse> PostGetDetails([FromBody]GetAgentAdminDetailsRequest request)
        {
            GetAgentAdminDetailsResponse resp = AgentAdminServices.GetDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("check_otp")]
        public NegotiatedContentResult<ResponseDto> PostCheckOTP([FromBody]CheckOtpRequest request)
        {
            ResponseDto resp = AgentAdminServices.CheckOTP(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("resend_otp")]
        public NegotiatedContentResult<ResendOtpResponse> PostResendOtp([FromBody] DTOs.Gen.ResendOtpRequest request)
        {
            ResendOtpResponse resp = AgentAdminServices.ResendOtp(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("reset_password")]
        public NegotiatedContentResult<ResponseDto> PostResetPassword([FromBody]ResetPasswordRequest request)
        {
            ResponseDto resp = AgentAdminServices.ResetPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("test_send_OTP")]
        public NegotiatedContentResult<string> TestEmailSend([FromBody]GetAgentAdminDetailsRequest request)
        {
            //EmailServices.SendMail("veena.cn@eraminfotech.in", "Mail Title", "Mail Message Content");
            //return Content(HttpStatusCode.OK, "Mail Sent");

            SMSService.SendOTP("","");
            return Content(HttpStatusCode.OK, "OTP Sent");
        }

        [HttpPost]
        [ActionName("test_push_notification")]
        public NegotiatedContentResult<string> TestPushNotification([FromBody]TestPushNotificationRequest request)
        {
            OrdersServices _ordersServices = new OrdersServices();
            _ordersServices.TestPushNotification(request.push_token);
            return Content(HttpStatusCode.OK, "Notification sent");
        }
        [HttpPost]
        [ActionName("test_email")]
        public NegotiatedContentResult<string> TestEmail([FromBody]TestEmailRequest request)
        {
            EmailServices.SendMail(request.email_id,"Test mail :  Please ignore","This is a test mail, Please ignore");
            return Content(HttpStatusCode.OK, "Notification sent");
        }
        
        [HttpPost]
        [ActionName("change_profilephoto_agent_admin")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfilePhoto([FromBody]ChangeProfilePhotoRequest request)
        {
            ResponseDto resp = AgentAdminServices.ChangeProfilePhoto(request);
            return Content(HttpStatusCode.OK, resp);
        }
    }
}
