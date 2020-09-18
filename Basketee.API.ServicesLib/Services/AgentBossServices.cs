using Basketee.API.DTOs;
using Basketee.API.DAOs;
using Basketee.API.DTOs.AgentBoss;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Services
{
    public class AgentBossServices
    {
        public static LoginResponse Login(LoginRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            LoginResponse response = new LoginResponse();
            AgentBoss agentBoss = null;
            try
            {
                using (AgentBossDao dao = new AgentBossDao())
                {
                    string hashPassword = TokenGenerator.GetHashedPassword(request?.password, 49);
                    agentBoss = dao.FindByMobileNumber(request.mobile_number);
                    if (agentBoss == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (hashPassword == agentBoss.Password)
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        //admin. = request.app_id;
                        agentBoss.AppToken = request.push_token;
                        agentBoss.AppID = request.app_id;
                        agentBoss.LastLogin = DateTime.Now;
                        string authToken = TokenGenerator.GenerateToken(agentBoss.OwnerName, agentBoss.Password, request.mobile_number);
                        agentBoss.AccToken = authToken;
                        dao.Update(agentBoss);
                        response.code = 0;
                        AgentBossLoginDto dto = new AgentBossLoginDto();
                        AgentBossHelper.CopyFromEntity(dto, agentBoss);
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("login.ok");
                        response.has_resource = 1;
                        response.user_login = dto;
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
        //                                 //TokenGenerator.GenerateResetPassword();
        //    AgentBoss agentBoss = null;
        //    try
        //    {
        //        using (AgentBossDao dao = new AgentBossDao())
        //        {
        //            agentBoss = dao.FindByMobileNumber(request.mobile_number);
        //            if (agentBoss == null)
        //            {
        //                MakeNouserResponse(response);
        //                return response;
        //            }
        //            agentBoss.Password = TokenGenerator.GetHashedPassword(newPassword, 49);
        //            dao.Update(agentBoss);
        //            OTPServices.SendPasswordMessage(agentBoss.MobileNumber, newPassword);
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
            response.message = MessagesSource.GetMessage("no.agentboss");
        }

        public static bool CheckAgentBoss(int userId, string authToken, ResponseDto response)
        {
            AgentBoss agentBoss = AgentBossServices.GetAuthAgentBoss(userId, authToken, response);
            if (agentBoss == null || agentBoss.AccToken != authToken)
            {
                return false;
            }
            return true;
        }

        public static bool CheckAgentBossNotAuthToken(int userId, ResponseDto response)
        {
            AgentBoss agentBoss = AgentBossServices.GetAuthAgentBossNotAuthToken(userId, response);
            if (agentBoss == null)
            {
                return false;
            }
            return true;
        }
        public static AgentBoss GetAuthAgentBoss(int userId, string authToken, ResponseDto response = null)
        {
            AgentBoss agentBoss = null;
            using (AgentBossDao dao = new AgentBossDao())
            {
                agentBoss = dao.FindById(userId);
                if (agentBoss != null && agentBoss.AccToken == authToken)
                {
                    return agentBoss;
                }
                if (response != null)
                {
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("invalid.agentboss");
                }
                return null;
            }
        }


        public static AgentBoss GetAuthAgentBossNotAuthToken(int userId, ResponseDto response = null)
        {
            AgentBoss agentBoss = null;
            using (AgentBossDao dao = new AgentBossDao())
            {
                agentBoss = dao.FindById(userId);
                if (agentBoss != null)
                {
                    return agentBoss;
                }
                if (response != null)
                {
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("invalid.agentboss");
                }
                return null;
            }
        }
        public static ResponseDto ChangePassword(ChangePasswordAgentBossRequest request)
        {
            ResponseDto response = new ResponseDto();
            AgentBoss agentBoss = null;
            string oldPasswordHash = TokenGenerator.GetHashedPassword(request.old_password, 49);
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    MakeNouserResponse(response);
                    return response;
                }
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindById(request.user_id);
                    if (agentBoss.Password == oldPasswordHash)
                    {
                        agentBoss.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(agentBoss);
                        response.code = 0;
                        response.has_resource = 1;
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

        public static ResponseDto ChangeProfile(ChangeProfileAgentBossRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            AgentBoss agentBoss = null;
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindById(request.user_id);
                    agentBoss.OwnerName = request.agent_boss_name;
                    //agentBoss.MobileNumber = request.mobile_number;
                    //agentBoss.ProfileImage = request.profile_image; //Commented bcz image is uploading as multipart
                    agentBoss.Email = request.agent_boss_email;
                    dao.Update(agentBoss);
                    response.code = 0;
                    response.has_resource = 1;
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

        public static GetAgentBossDetailsResponse GetDetails(GetAgentBossDetailsRequest request)
        {
            GetAgentBossDetailsResponse response = new GetAgentBossDetailsResponse();
            AgentBoss agentBoss = null;
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindById(request.user_id);
                    response.agent_boss_details = new AgentBossMoreDetailsDto();
                    response.agent_boss_details.agent_boss_id = agentBoss.AbosID;
                    response.agent_boss_details.profile_image = ImagePathService.agentBossImagePath + agentBoss.ProfileImage;
                    response.agent_boss_details.agent_boss_name = agentBoss.OwnerName;
                    response.agent_boss_details.mobile_number = agentBoss.MobileNumber;
                    response.agent_boss_details.agent_boss_email = agentBoss.Email;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("admin.boss.details");
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
            AgentBoss agentBoss = null;
            response.has_resource = 0;
            try
            {
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindByMobileNumber(request.mobile_number);
                    //agentBoss = GetAuthAgentBoss(request.user_id, request.auth_token, response);
                    //agentBoss = GetAuthAgentBossNotAuthToken(request.user_id, response);
                    if (agentBoss == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }


                    bool otpValid = OTPServices.ValidateOTP(agentBoss.AbosID, request.otp_code);
                    OTPServices.RemoveOTP(agentBoss.AbosID, request.otp_code);// Either way remove this otp if it exists.
                    if (otpValid)
                    {
                        agentBoss.StatusId = true;
                        dao.Update(agentBoss);
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

        //public static ResendOtpResponse ResendOtp(ResendOtpRequest request)
        //{
        //    ResendOtpResponse response = new ResendOtpResponse();
        //    response.otp_details = new OtpDetailsDto();
        //    AgentBoss agentBoss = null;
        //    try
        //    {
        //        using (AgentBossDao userDao = new AgentBossDao())
        //        {
        //            // agentBoss = GetAuthAgentBoss(request.user_id, request.auth_token, response);
        //            agentBoss = userDao.FindByMobileNumber(request.mobile_number);
        //        }
        //        if (agentBoss == null)
        //        {
        //            MakeNouserResponse(response);
        //            return response;
        //        }

        //        OTPServices.ResendOTP(agentBoss.AbosID, agentBoss.MobileNumber, "B");
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
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            AgentBoss agentBoss = null;
            string newPasswordHash = TokenGenerator.GetHashedPassword(request.new_password, 49);
            string confirmPasswordHash = TokenGenerator.GetHashedPassword(request.confirm_password, 49);
            try
            {
                //if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                //if (!AgentBossServices.CheckAgentBossNotAuthToken(request.user_id, response))
                //{
                //    return response;
                //}
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindByMobileNumber(request.mobile_number);
                    //agentBoss = dao.FindById(request.user_id);
                    if (agentBoss == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (newPasswordHash == confirmPasswordHash)
                    {
                        agentBoss.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(agentBoss);
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

        public static ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            //string newPassword = TokenGenerator.GenerateResetPassword();
            AgentBoss agentBoss = null;
            try
            {
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindByMobileNumber(request.mobile_number);
                    if (agentBoss == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    OTPServices.SendOTPForForgotPassword(response, request.mobile_number, agentBoss.AbosID, "B");
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
            AgentBoss agentBoss = null;
            try
            {
                //if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                //{
                //    return response;
                //}
                using (AgentBossDao dao = new AgentBossDao())
                {
                    agentBoss = dao.FindById(request.user_id);
                    if (agentBoss != null)
                    {
                        agentBoss.ProfileImage = request.profile_image;
                        dao.Update(agentBoss);
                    }
                    response.code = 0;
                    response.has_resource = 1;
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
            AgentBoss agentBoss = null;
            try
            {
                using (AgentBossDao bossDao = new AgentBossDao())
                {
                    agentBoss = bossDao.FindByMobileNumber(request.mobile_number);
                }
                if (agentBoss == null)
                {
                    MakeNouserResponse(response);
                    return response;
                }

                OTPServices.ResendOTP(response, agentBoss.MobileNumber, agentBoss.AbosID, "B");
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
