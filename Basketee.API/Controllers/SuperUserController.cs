using Basketee.API.DTOs;
using Basketee.API.DTOs.SuperUser;
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
    public class SuperUserController : ApiController
    {
        [HttpPost]
        [ActionName("login")]
        public NegotiatedContentResult<LoginResponse> PostLogin([FromBody]LoginRequest request)
        {
            LoginResponse resp = SuperUserServices.Login(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("forgot_password")]
        public NegotiatedContentResult<ForgotPasswordResponse> PostForgotPassword([FromBody]ForgotPasswordRequest request)
        {
            ForgotPasswordResponse resp = SuperUserServices.ForgotPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("change_password_super_user")]
        public NegotiatedContentResult<ResponseDto> PostChangePassword([FromBody]ChangePasswordSuperUserRequest request)
        {
            ResponseDto resp = SuperUserServices.ChangePassword(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("change_profile_super_user")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfile([FromBody]ChangeProfileSuperUserRequest request)
        {
            ResponseDto resp = SuperUserServices.ChangeProfile(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_super_user_details")]
        public NegotiatedContentResult<ResponseDto> PostGetDetails([FromBody]GetSuperUserDetailsRequest request)
        {
            ResponseDto resp = SuperUserServices.GetDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("check_otp")]
        public NegotiatedContentResult<ResponseDto> PostCheckOtp([FromBody]CheckOtpRequest request)
        {
            ResponseDto resp = SuperUserServices.CheckOtp(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("resend_otp")]
        public NegotiatedContentResult<ResendOtpResponse> PostResendOtp([FromBody]ResendOtpRequest request)
        {
            ResendOtpResponse resp = SuperUserServices.ResendOtp(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("reset_password")]
        public NegotiatedContentResult<ResponseDto> PostResetPassword([FromBody]ResetPasswordRequest request)
        {
            ResponseDto resp = SuperUserServices.ResetPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_profilephoto_super_user")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfilePhoto([FromBody]ChangeProfilePhotoRequest request)
        {
            ResponseDto resp = SuperUserServices.ChangeProfilePhoto(request);
            return Content(HttpStatusCode.OK, resp);
        }

    }
}
