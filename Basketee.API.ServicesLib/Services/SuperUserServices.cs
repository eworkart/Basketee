using Basketee.API.DTOs;
using Basketee.API.DTOs.SuperUser;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;
using System;
using Basketee.API.DAOs;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Services
{

    public class SuperUserServices
    {
        public static LoginResponse Login(LoginRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            LoginResponse response = new LoginResponse();
            SuperAdmin superuser = null;
            string hashPassword = TokenGenerator.GetHashedPassword(request.password, 49);
            try
            {
                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = dao.FindByMobileNumber(request.mobile_number);
                    if (superuser == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (hashPassword == superuser.Password)
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        //admin. = request.app_id;
                        superuser.AppToken = request.push_token;
                        superuser.AppID = request.app_id;
                        superuser.LastLogin = DateTime.Now;
                        string authToken = TokenGenerator.GenerateToken(superuser.FullName, superuser.Password, request.mobile_number);
                        superuser.AccToken = authToken;
                        dao.Update(superuser);
                        response.code = 0;
                        SuperUserLoginDto dto = new SuperUserLoginDto();
                        SuperUserHelper.CopyFromEntity(dto, superuser);

                        SuperUserLoginDetailsDto dtoDetails = new SuperUserLoginDetailsDto();
                        SuperUserHelper.CopyFromEntity(dtoDetails, superuser);


                        response.user_login = dto;
                        response.super_user_details = dtoDetails;
                        response.has_resource = 1;
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("login.ok");
                        return response;
                    }
                    else
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("login.fail");
                    }
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public static void MakeNouserResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.super.user");
        }

        //public static ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        //{
        //    ForgotPasswordResponse response = new ForgotPasswordResponse();
        //    string newPassword = "1111"; //TODO change to generation
        //                                 //TokenGenerator.GenerateResetPassword();
        //    SuperAdmin superuser = null;
        //    try
        //    {
        //        using (SuperUserDao dao = new SuperUserDao())
        //        {
        //            superuser = dao.FindByMobileNumber(request.mobile_number);
        //            if (superuser == null)
        //            {
        //                MakeNouserResponse(response);
        //                return response;
        //            }
        //            superuser.Password = TokenGenerator.GetHashedPassword(newPassword, 49);
        //            dao.Update(superuser);
        //            OTPServices.SendPasswordMessage(superuser.MobileNum, newPassword);
        //            response.code = 0;
        //            response.has_resource = 1;
        //            response.reset_password = new ResetPasswordDto();
        //            response.reset_password.password_otp_sent = 1;
        //            response.reset_password.password_reset = 1;
        //            response.message = MessagesSource.GetMessage("passwd.reset");
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.MakeExceptionResponse(ex);
        //        return response;
        //    }
        //}

        public static ResponseDto ChangePassword(ChangePasswordSuperUserRequest request)
        {
            ResponseDto response = new ResponseDto();
            SuperAdmin superuser = null;
            string oldPasswordHash = TokenGenerator.GetHashedPassword(request.old_password, 49);
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("no.super.user");
                    return response;
                }
                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = dao.FindById(request.user_id);
                    if (superuser.Password == oldPasswordHash)
                    {
                        superuser.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(superuser);
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("password.changed");
                        return response;
                    }
                }
                response.code = 1;
                response.has_resource = 0;
                response.message = MessagesSource.GetMessage("exception");
                return response;
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static bool CheckSuperUser(int userId, string authToken, ResponseDto response)
        {
            SuperAdmin superuser = SuperUserServices.GetAuthUser(userId, authToken, response);
            if (superuser == null || superuser.AccToken != authToken)
            {
                return false;
            }
            return true;
        }
        public static SuperAdmin GetAuthUser(int userId, string authToken, ResponseDto response = null)
        {
            SuperAdmin superuser = null;
            using (SuperUserDao dao = new SuperUserDao())
            {
                superuser = dao.FindById(userId);
                if (superuser != null && superuser.AccToken == authToken)
                {
                    return superuser;
                }
                if (response != null)
                {
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("invalid.superuser");
                }
                return null;
            }
        }



        public static ResponseDto ChangeProfile(ChangeProfileSuperUserRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            SuperAdmin superuser = null;
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }
                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = dao.FindById(request.user_id);
                    superuser.FullName = request.super_user_name;
                    //superuser.MobileNum = request.mobile_number;
                    //superuser.ProfileImage = request.profile_image;//Commented bcz image is uploading as multipart
                    superuser.Email = request.super_user_email;
                    dao.Update(superuser);
                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("profile.changed");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static GetSuperUserDetailsResponse GetDetails(GetSuperUserDetailsRequest request)
        {
            GetSuperUserDetailsResponse response = new GetSuperUserDetailsResponse();
            SuperAdmin superuser = null;
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }
                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = dao.FindById(request.user_id);
                    response.super_user_details = new SuperUserDetailsDto();
                    response.super_user_details.super_user_id = superuser.SAdminID;
                    response.super_user_details.profile_image = ImagePathService.superUserImagePath + superuser.ProfileImage;
                    response.super_user_details.super_user_name = superuser.FullName;
                    response.super_user_details.mobile_number = superuser.MobileNum;
                    response.super_user_details.super_user_email = superuser.Email;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("superuser.details");
                    return response;


                }

            }

            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static ResponseDto CheckOtp(CheckOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            SuperAdmin superuser = null;
            response.has_resource = 0;

            try
            {

                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = GetAuthUserbyMobileNumber(request.mobile_number);
                    if (superuser == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    bool otpValid = OTPServices.ValidateOTP(superuser.SAdminID, request.otp_code);
                    OTPServices.RemoveOTP(superuser.SAdminID, request.otp_code);// Either way remove this otp if it exists.
                    if (otpValid)
                    {

                        dao.Update(superuser);
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("otp.valid");
                        return response;
                    }
                    response.code = 1;
                    response.message = MessagesSource.GetMessage("otp.not.valid");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        //public static ResendOtpResponse ResendOtp(ResendOtpRequest request)
        //{
        //    ResendOtpResponse response = new ResendOtpResponse();
        //    response.otp_details = new OtpDetailsDto();
        //    SuperAdmin superuser = null;
        //    try
        //    {
        //        if (!SuperUserServices.CheckSuperUser(request.mobile_number))
        //        {
        //            response.message = MessagesSource.GetMessage("no.super.user");
        //            return response;
        //        }
        //        using (SuperUserDao userDao = new SuperUserDao())
        //        {

        //            superuser = userDao.FindByMobileNumber(request.mobile_number);
        //        }
        //        OTPServices.ResendOTP(superuser.SAdminID, request.mobile_number, "A");
        //        response.code = 0;
        //        response.has_resource = 1;
        //        response.message = MessagesSource.GetMessage("otp.resent");
        //        response.otp_details.send_otp = 1;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.MakeExceptionResponse(ex);
        //        return response;
        //    }
        //}
        public static ResponseDto ResetPassword(ResetPasswordRequest request)
        {
            ResponseDto response = new ResponseDto();
            SuperAdmin superuser = null;
            string newPasswordHash = TokenGenerator.GetHashedPassword(request.new_password, 49);
            string confirmPasswordHash = TokenGenerator.GetHashedPassword(request.confirm_password, 49);
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.mobile_number))
                {
                    response.message = MessagesSource.GetMessage("no.super.user");
                    return response;
                }
                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = dao.FindByMobileNumber(request.mobile_number);
                    //agentBoss = dao.FindById(request.user_id);
                    if (newPasswordHash == confirmPasswordHash)
                    {
                        superuser.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(superuser);
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("passwd.reset");
                        return response;
                    }
                }
                response.code = 1;
                response.has_resource = 0;
                response.message = MessagesSource.GetMessage("exception");
                return response;
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }
        public static bool CheckSuperUser(string mobilenumber)
        {
            SuperAdmin superuser = SuperUserServices.GetAuthUserbyMobileNumber(mobilenumber);
            if (superuser == null)
            {
                return false;
            }
            return true;
        }

        public static SuperAdmin GetAuthUserbyMobileNumber(string mobilenumber)
        {
            ResponseDto response = new ResponseDto();
            SuperAdmin superuser = null;
            using (SuperUserDao dao = new SuperUserDao())
            {
                superuser = dao.FindByMobileNumber(mobilenumber);
                if (superuser != null)
                {
                    return superuser;
                }
                if (response != null)
                {
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("invalid.superuser");
                }
                return null;
            }
        }

        public static ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            //string newPassword = TokenGenerator.GenerateResetPassword();            
            try
            {
                using (SuperUserDao dao = new SuperUserDao())
                {
                    SuperAdmin superAdmin = dao.FindByMobileNumber(request.mobile_number);
                    if (superAdmin == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    OTPServices.SendOTPForForgotPassword(response, request.mobile_number, superAdmin.SAdminID, "S");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static ResponseDto ChangeProfilePhoto(ChangeProfilePhotoRequest request)
        {
            ResponseDto response = new ResponseDto();
            SuperAdmin superuser = null;
            try
            {
                //if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                //{
                //    response.message = MessagesSource.GetMessage("invalid.super.user");
                //    return response;
                //}
                using (SuperUserDao dao = new SuperUserDao())
                {
                    superuser = dao.FindById(request.user_id);
                    if (superuser != null)
                    {
                        superuser.ProfileImage = request.profile_image;
                        dao.Update(superuser);
                    }
                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("profile.changed");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static ResendOtpResponse ResendOtp(ResendOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResendOtpResponse response = new ResendOtpResponse();
            response.otp_details = new OTPDetailsDto();
            SuperAdmin suerUser = null;
            try
            {
                using (SuperUserDao sUserDao = new SuperUserDao())
                {
                    suerUser = sUserDao.FindByMobileNumber(request.mobile_number);
                }
                if (suerUser == null)
                {
                    MakeNouserResponse(response);
                    return response;
                }

                OTPServices.ResendOTP(response, suerUser.MobileNum, suerUser.SAdminID, "S");
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("otp.resent");
                response.otp_details.send_otp = 1;
                return response;
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }
    }
}
