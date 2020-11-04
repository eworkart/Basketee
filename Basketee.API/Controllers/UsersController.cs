using Basketee.API.DTOs;
using Basketee.API.DTOs.Users;
using Basketee.API.Services;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Basketee.API.DTOs.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Basketee.API.Controllers
{



    public class UsersController : ApiController
    {

        private UserServices _userServices = new UserServices();
        /// <summary>
        /// Register a new consumer user
        /// </summary>
        /// <param name="request">JSON formated string with registration fields</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("register")]
        public NegotiatedContentResult<RegisterResponse> PostRegister([FromBody]RegisterRequest request)
        {
            RegisterResponse resp = _userServices.Register(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("resend_otp")]
        public NegotiatedContentResult<ResendOtpResponse> PostResendOtp([FromBody]ResendOtpRequest request)
        {
            ResendOtpResponse resp = _userServices.ResendOtp(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("activate_user")]
        public NegotiatedContentResult<ResponseDto> PostActivateUser([FromBody]ActivateUserRequest request)
        {
            ResponseDto resp =_userServices.ActivateUser(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("user_exists")]
        public NegotiatedContentResult<UserExistsResponse> PostUserExists([FromBody]UserExistsRequest request)
        {
            UserExistsResponse resp = _userServices.CheckUserExists(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("forgot_password")]
        public NegotiatedContentResult<ForgotPasswordResponse> PostForgotPassword([FromBody]ForgotPasswordRequest request)
        {
            ForgotPasswordResponse resp = _userServices.ForgotPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("login")]
        public NegotiatedContentResult<LoginResponse> PostLogin([FromBody]LoginRequest request)
        {
            LoginResponse resp = _userServices.Login(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_user_details")]
        public NegotiatedContentResult<GetUserDetailsResponse> PostGetUserDetails([FromBody]GetUserDetailsRequest request)
        {
            GetUserDetailsResponse resp = _userServices.GetUserDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("change_password")]
        public NegotiatedContentResult<ResponseDto> PostChangePassword([FromBody]ChangePasswordRequest request)
        {
            ResponseDto resp = _userServices.ChangePassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_all_addresses")]
        public NegotiatedContentResult<GetAddressesResponse> PostGetAddresses([FromBody]GetAddressesRequest request)
        {
            GetAddressesResponse resp = _userServices.GetAddresses(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("add_address")]
        public NegotiatedContentResult<ResponseDto> PostAddAddress([FromBody]AddAddressRequest request)
        {
            ResponseDto resp = _userServices.AddAddress(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("update_address")]
        public NegotiatedContentResult<ResponseDto> PostUpdateAddress([FromBody]UpdateAddressRequest request)
        {
            ResponseDto resp = _userServices.UpdateAddress(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("delete_address")]
        public NegotiatedContentResult<ResponseDto> PostDeleteAddress([FromBody]DeleteAddressRequest request)
        {
            ResponseDto resp = _userServices.DeleteAddress(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("check_otp")]
        public NegotiatedContentResult<ResponseDto> PostCheckOTP([FromBody]CheckOtpRequest request)
        {
            ResponseDto resp = _userServices.CheckOTP(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_address")]
        public NegotiatedContentResult<GetAddressResponse> PostGetAddress([FromBody]GetAddressRequest request)
        {
            GetAddressResponse resp = _userServices.GetAddress(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("update_profile")]
        public NegotiatedContentResult<ResponseDto> PostUpdateProfile([FromBody]UpdateProfileRequest request)
        {
            ResponseDto resp = _userServices.UpdateProfile(request);
            return Content(HttpStatusCode.OK, resp);
        }

        //Update User Profile
        
        [HttpPost]
        [ActionName("update_push_token")]
        [ActionInputValidationFilter()] //<- This performs validation of UpdateDeviceTokenRequest according to the annotations
        public NegotiatedContentResult<ResponseDto> PostUpdatePushToken([FromBody]UpdateDeviceTokenRequest request)
        {
            ResponseDto resp = _userServices.UpdateFirebaseDeviceToken(request);
            return Content(resp.httpCode, resp);
        }

        [HttpPost]
        [ActionName("change_profilephoto_user")]
        public NegotiatedContentResult<ResponseDto> PostChangeProfilePhoto()
        {
            return Content(HttpStatusCode.OK, ProcessProfileData(HttpContext.Current.Request, (int)UserType.Consumer));
        }

        [NonAction]
        private ResponseDto ProcessProfileData(HttpRequest httpRequest, int userType)
        {
            ChangeProfilePhotoRequest request = new ChangeProfilePhotoRequest();
            ResponseDto resp = null;
            UploadServices upImage = new UploadServices();
            var statusMessage = upImage.UploadProfilePicture(httpRequest, userType);
            if (statusMessage.ContainsKey("success"))
            {
                request.profile_image = statusMessage["message"].ToString();
                request.user_id = statusMessage["user_id"].ToInt();
                request.auth_token = statusMessage["auth_token"].ToString();
                resp = _userServices.ChangeProfilePhoto(request);
            }
            else
            {
                resp = new ResponseDto();
                resp.code = 0;
                resp.has_resource = 0;
                resp.message = statusMessage["message"].ToString();
            }

            return resp;
        }

        [HttpPost]
        [ActionName("reset_password")]
        public NegotiatedContentResult<ResponseDto> PostResetPassword([FromBody]ResetPasswordRequest request)
        {
            ResponseDto resp =  _userServices.ResetPassword(request);
            return Content(HttpStatusCode.OK, resp);
        }

        
    }
}
