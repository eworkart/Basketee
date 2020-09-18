using Basketee.API.DTOs;
using Basketee.API.DTOs.Agent;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.Orders;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Services
{
    public class AgentAdminServices
    {
        public static LoginResponse Login(LoginRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            LoginResponse response = new LoginResponse();
            AgentAdmin admin = null;
            string hashPassword = TokenGenerator.GetHashedPassword(request.password, 49);
            try
            {
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    admin = dao.FindByMobileNumber(request.mobile_number);
                    if (admin == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (hashPassword == admin.Password)
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        //admin. = request.app_id;
                        admin.AppToken = request.push_token;
                        admin.AppID = request.app_id;
                        admin.LastLogin = DateTime.Now;
                        string authToken = TokenGenerator.GenerateToken(admin.AgentAdminName, admin.Password, request.mobile_number);
                        admin.AccToken = authToken;
                        dao.Update(admin);
                        response.code = 0;
                        UserLoginDto dto = new UserLoginDto();
                        AgentHelper.CopyFromEntity(dto, admin);
                        response.user_login = dto;
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

        //public static ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        //{
        //    ForgotPasswordResponse response = new ForgotPasswordResponse();
        //    string newPassword = "1111"; //TODO change to generation
        //        //TokenGenerator.GenerateResetPassword();
        //    AgentAdmin admin = null;
        //    try
        //    {
        //        using (AgentAdminDao dao = new AgentAdminDao())
        //        {
        //            admin = dao.FindByMobileNumber(request.mobile_number);
        //            if (admin == null)
        //            {
        //                MakeNouserResponse(response);
        //                return response;
        //            }
        //            admin.Password = TokenGenerator.GetHashedPassword(newPassword, 49);
        //            dao.Update(admin);
        //            OTPServices.SendPasswordMessage(admin.MobileNumber, newPassword);
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

        public static void MakeNouserResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.admin");
        }

        public static bool CheckAdmin(int userId, string authToken, ResponseDto response)
        {
            AgentAdmin admin = AgentAdminServices.GetAuthAdmin(userId, authToken, response);
            if (admin == null || admin.AccToken != authToken)
            {
                return false;
            }
            return true;
        }


        public static AgentAdmin GetAuthAdmin(int userId, string authToken, ResponseDto response = null)
        {
            AgentAdmin admin = null;
            using (AgentAdminDao dao = new AgentAdminDao())
            {
                admin = dao.FindById(userId);
                if (admin != null && admin.AccToken == authToken)
                {
                    return admin;
                }
                if(response != null)
                {
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("invalid.admin");
                }
                return null;
            }
        }

        public static ResponseDto ChangePassword(ChangePasswordAgentAdminRequest request)
        {
            ResponseDto response = new ResponseDto();
            AgentAdmin admin = null;
            string oldPasswordHash = TokenGenerator.GetHashedPassword(request.old_password, 49);
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    admin = dao.FindById(request.user_id);
                    if (admin.Password == oldPasswordHash)
                    {
                        admin.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(admin);
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

        public static ResponseDto ChangeProfile(ChangeProfileAgentAdminRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            AgentAdmin admin = null;
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    admin = dao.FindById(request.user_id);
                    admin.AgentAdminName = request.agent_admin_name;
                    //admin.MobileNumber = request.mobile_number;                    
                    //admin.ProfileImage = string.IsNullOrWhiteSpace(request.profile_image) ? admin.ProfileImage : request.profile_image; //Commented bcz image is uploading as multipart
                    admin.email = request.agent_admin_email;
                    dao.Update(admin);
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

        public static GetAgentAdminDetailsResponse GetDetails(GetAgentAdminDetailsRequest request)
        {
            GetAgentAdminDetailsResponse response = new GetAgentAdminDetailsResponse();
            AgentAdmin admin = null;
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    admin = dao.FindById(request.user_id);
                    response.agent_admin_details = new AgentAdminDetailsDto();
                    response.agent_admin_details.agent_admin_id = admin.AgadmID;
                    response.agent_admin_details.agent_admin_name = admin.AgentAdminName;
                    response.agent_admin_details.mobile_number = admin.MobileNumber;
                    response.agent_admin_details.profile_image = ImagePathService.agentAdminImagePath + admin.ProfileImage;
                    response.agent_admin_details.agent_admin_email = admin.email;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("admin.details");
                    return response;

                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            //string newPassword = TokenGenerator.GenerateResetPassword();            
            try
            {
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    AgentAdmin agentAdmin = dao.FindByMobileNumber(request.mobile_number);
                    if (agentAdmin == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    OTPServices.SendOTPForForgotPassword(response, request.mobile_number, agentAdmin.AgadmID, "A");
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
            AgentAdmin admin = null;
            try
            {
                //if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                //{
                //    return response;
                //}
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    admin = dao.FindById(request.user_id);
                    if (admin != null)
                    {
                        admin.ProfileImage = request.profile_image;
                        dao.Update(admin);
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

        public static ResponseDto CheckOTP(CheckOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            AgentAdmin agentAdmin = null;
            response.has_resource = 0;
            try
            {
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    agentAdmin = dao.FindByMobileNumber(request.mobile_number);
                    //agentBoss = GetAuthAgentBoss(request.user_id, request.auth_token, response);
                    //agentBoss = GetAuthAgentBossNotAuthToken(request.user_id, response);
                    if (agentAdmin == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }

                    bool otpValid = OTPServices.ValidateOTP(agentAdmin.AgadmID, request.otp_code);
                    OTPServices.RemoveOTP(agentAdmin.AgadmID, request.otp_code);// Either way remove this otp if it exists.
                    if (otpValid)
                    {
                        agentAdmin.StatusId = true;
                        dao.Update(agentAdmin);
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("otp.valid");
                        response.has_resource = 1;
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
            }
            return response;
        }

        public static ResendOtpResponse ResendOtp(DTOs.Gen.ResendOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResendOtpResponse response = new ResendOtpResponse();
            response.otp_details = new OTPDetailsDto();
            AgentAdmin agentAdmin = null;
            try
            {
                using (AgentAdminDao adminDao = new AgentAdminDao())
                {
                    // agentBoss = GetAuthAgentBoss(request.user_id, request.auth_token, response);
                    agentAdmin = adminDao.FindByMobileNumber(request.mobile_number);
                }
                if (agentAdmin == null)
                {
                    MakeNouserResponse(response);
                    return response;
                }

                OTPServices.ResendOTP(response, agentAdmin.MobileNumber, agentAdmin.AgadmID, "A");
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

        public static ResponseDto ResetPassword(ResetPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            AgentAdmin agentAdmin = null;
            string newPasswordHash = TokenGenerator.GetHashedPassword(request.new_password, 49);
            string confirmPasswordHash = TokenGenerator.GetHashedPassword(request.confirm_password, 49);
            try
            {
                using (AgentAdminDao dao = new AgentAdminDao())
                {
                    agentAdmin = dao.FindByMobileNumber(request.mobile_number);
                    if (agentAdmin == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (newPasswordHash == confirmPasswordHash)
                    {
                        agentAdmin.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(agentAdmin);
                        response.code = 0;
                        response.has_resource = 1;
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
    }
}
