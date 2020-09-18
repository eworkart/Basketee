using Basketee.API.DTOs.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Basketee.API.Services;
using Basketee.API.DTOs;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Controllers
{
    public class DriverController : ApiController
    {
        private DriverServices _driverServices = new DriverServices();

        [HttpPost]
        [ActionName("login")]
        public NegotiatedContentResult<LoginResponse> PostLogin([FromBody]LoginRequest request)
        {
            LoginResponse resp = _driverServices.Login(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("forgot_password")]
        public NegotiatedContentResult<ForgotPasswordResponse> PostForgotPassword([FromBody]ForgotPasswordRequest request)
        {
            ForgotPasswordResponse resp = _driverServices.ForgotPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("change_password_driver")]
        public NegotiatedContentResult<ResponseDto> PostChangePasswordDriver([FromBody]ChangePasswordDriverRequest request)
        {
            ResponseDto resp = _driverServices.ChangePasswordDriver(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_agent_driver")]
        public NegotiatedContentResult<GetAgentDriverResponse> PostGetAgentDriver([FromBody]GetAgentDriverRequest request)
        {
            GetAgentDriverResponse resp = _driverServices.GetAgentDriver(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_ereceipt_driver")]
        public NegotiatedContentResult<GetEReceiptResponse> PostGetEreceiptDriver([FromBody]GetEReceiptRequest request)
        {
            GetEReceiptResponse resp = _driverServices.GetERecieptDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("check_otp")]
        public NegotiatedContentResult<ResponseDto> PostCheckOTP([FromBody]CheckOtpRequest request)
        {
            ResponseDto resp = _driverServices.CheckOTP(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("resend_otp")]
        public NegotiatedContentResult<ResendOtpResponse> PostResendOtp([FromBody]ResendOtpRequest request)
        {
            ResendOtpResponse resp = _driverServices.ResendOtp(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("reset_password")]
        public NegotiatedContentResult<ResponseDto> PostResetPassword([FromBody]ResetPasswordRequest request)
        {
            ResponseDto resp = _driverServices.ResetPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("update_profile")]
        public NegotiatedContentResult<UpdateProfileResponse> PostUpdateProfile([FromBody]UpdateProfileRequest request)
        {
            UpdateProfileResponse resp = _driverServices.UpdateProfile(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_profilephoto_driver")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfilePhoto([FromBody]ChangeProfilePhotoRequest request)
        {
            ResponseDto resp = _driverServices.ChangeProfilePhoto(request);
            return Content(HttpStatusCode.OK, resp);
        }
    }
}