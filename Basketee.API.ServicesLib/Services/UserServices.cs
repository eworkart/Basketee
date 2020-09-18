
using Basketee.API.DAOs;
using Basketee.API.DTOs;
using Basketee.API.DTOs.Users;
using Basketee.API.Services.Helpers;
using Basketee.API.Models;
using Basketee.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Services
{
    /// <summary>
    /// Business logic behind user functions. Also acts as the facade for all consumer functions.
    /// </summary>
    public class UserServices
    {
        /// <summary>
        /// Register a new consumer.
        /// </summary>
        /// <param name="request">The DTO with requst form data parameters.</param>
        /// <returns>The response DTO with result of the operation.</returns>
        public RegisterResponse Register(RegisterRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            RegisterResponse responseDto = new RegisterResponse();
            Consumer consumer = new Consumer();
            //Get posted parameter values into the entity from the DTO
            UsersHelper.CopyToEntity(consumer, request);
            consumer.Password = TokenGenerator.GetHashedPassword(request.user_password, 49);//To fit into the password field of database table

            //Generate unique auth /access token for the user.
            consumer.AccToken = TokenGenerator.GenerateToken(request.user_name, request.user_password, request.mobile_number);
            consumer.CreatedDate = DateTime.Now;
            consumer.UpdatedDate = consumer.CreatedDate;
            consumer.StatusID = 1;
            consumer.ConsActivated = true;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    // Check if the mobile number is registered 
                    if (dao.CheckPhoneExists(request.mobile_number))
                    {
                        responseDto.code = 1;
                        responseDto.has_resource = 0;
                        responseDto.message = MessagesSource.GetMessage("cons.reg.dupl");
                    }
                    else
                    {
                        consumer = dao.Insert(consumer); // save the entity.
                        UsersHelper.CopyFromEntity(responseDto, consumer); //Copy to the response DTO
                        string otp = OTPServices.GenerateAndSendOTP(request.mobile_number);
                        if (OTPServices.SaveOTP(otp, consumer.ConsID, "C"))
                        {
                            responseDto.new_user.send_otp = 1; // state that OTP has been sent.
                        }
                        responseDto.code = 0; // Result OK.
                        responseDto.has_resource = 1;
                        responseDto.message = MessagesSource.GetMessage("cons.reg.ok");
                    }
                }
            }
            catch (Exception ex)
            {
                responseDto.MakeExceptionResponse(ex);
            }
            return responseDto;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDao"></param>
        /// <param name="userId"></param>
        /// <param name="authCode"></param>
        /// <param name="withDetails"></param>
        /// <returns></returns>
        public Consumer GetAuthUser(UserDao userDao, int userId, string authCode, bool withDetails = false)
        {
            Consumer consumer = null;
            consumer = userDao.FindById(userId, withDetails);

            if (consumer != null && consumer.AccToken == authCode)
            {
                return consumer;
            }
            return null;
        }

        public bool CheckAuthUser(int userId, string authCode, bool withDetails = false)
        {
            Consumer consumer = null;
            using (UserDao dao = new UserDao())
            {
                consumer = GetAuthUser(dao, userId, authCode, withDetails);
                if (consumer == null)
                    return false;
            }
            if (consumer.ConsBlocked || (!consumer.ConsActivated))
            {
                return false;
            }
            return consumer != null; ;
        }

        public void MakeNouserResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.user");
        }

        public void MakeNoAddressResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.address.found");
        }

        public ResponseDto ActivateUser(ActivateUserRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            try
            {
                using (UserDao userDao = new UserDao())
                {
                    consumer = GetAuthUser(userDao, request.user_id, request.auth_token);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (consumer.ConsActivated)
                    {
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("user.active");
                        return response;
                    }
                    bool validOtp = OTPServices.ValidateOTP(request.user_id, request.otp_code);
                    if (validOtp)
                    {
                        consumer.ConsActivated = true;
                        userDao.Update(consumer);
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("user.activated");
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }

        }

        //public ResendOtpResponse ResendOtp(ResendOtpRequest request)
        //{
        //    ResendOtpResponse response = new ResendOtpResponse();
        //    response.otp_details = new OtpDetailsDto();
        //    Consumer consumer = null;
        //    try
        //    {
        //        using (UserDao userDao = new UserDao())
        //        {
        //            consumer = GetAuthUser(userDao, request.user_id, request.auth_token);
        //        }
        //        if (consumer == null)
        //        {
        //            MakeNouserResponse(response);
        //            return response;
        //        }
        //        OTPServices.ResendOTP(request.user_id, consumer.PhoneNumber, "C");
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

        public UserExistsResponse CheckUserExists(UserExistsRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            UserExistsResponse response = new UserExistsResponse();
            Consumer consumer = null;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = dao.FindByMobileNumber(request.mobile_number);
                }
                if (consumer == null)
                {
                    MakeNouserResponse(response);
                    return response;
                }
                UsersHelper.CopyFromEntity(response, consumer);
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("user.found");
                return response;
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            //string newPassword = TokenGenerator.GenerateResetPassword();
            Consumer consumer = null;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = dao.FindByMobileNumber(request.mobile_number);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    OTPServices.SendOTPForForgotPassword(response, request.mobile_number, consumer.ConsID, "C");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public LoginResponse Login(LoginRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            LoginResponse response = new LoginResponse();
            Consumer consumer = null;
            string hashPassword = TokenGenerator.GetHashedPassword(request.user_password, 49);
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = dao.FindByMobileNumber(request.mobile_number);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (hashPassword == consumer.Password)
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        if (consumer.ConsBlocked)
                        {
                            response.message = MessagesSource.GetMessage("user.blocked");
                            return response;
                        }
                        if (!consumer.ConsActivated)
                        {
                            response.message = MessagesSource.GetMessage("user.not.active");
                            return response;
                        }

                        consumer.AppID = request.app_id;
                        consumer.AppToken = request.push_token;
                        consumer.LastLogin = DateTime.Now;
                        consumer.AccToken = TokenGenerator.GenerateToken(consumer.Name, consumer.Password, consumer.PhoneNumber);
                        dao.Update(consumer);
                        response.code = 0;
                        UsersHelper.CopyFromEntity(response, consumer);
                        response.has_resource = 1;
                        if (response.user_login.allow_login == 1)
                        {
                            response.code = 0;
                            response.message = MessagesSource.GetMessage("login.ok");
                            return response;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public GetUserDetailsResponse GetUserDetails(GetUserDetailsRequest request)
        {
            GetUserDetailsResponse response = new GetUserDetailsResponse();
            Consumer consumer = null;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    UsersHelper.CopyFromEntity(response, consumer);
                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("user.details");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public ResponseDto ChangePassword(ChangePasswordRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            string oldPasswordHash = TokenGenerator.GetHashedPassword(request.old_password, 49);
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (consumer.Password == oldPasswordHash)
                    {
                        consumer.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(consumer);
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("password.changed");
                        return response;
                    }
                }
                response.code = 1;
                response.has_resource = 0;
                response.message = MessagesSource.GetMessage("pass.not.chg");
                return response;
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public GetAddressesResponse GetAddresses(GetAddressesRequest request)
        {
            GetAddressesResponse response = new GetAddressesResponse();
            Consumer consumer = null;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (consumer.ConsumerAddresses.Count <= 0 && consumer.ConsumerAddresses.Where(x => x.StatusID == 1).Count() <= 0)
                    {
                        MakeNoAddressResponse(response);
                        return response;
                    }
                    UsersHelper.CopyFromEntity(response, consumer);
                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("address.details.found");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto AddAddress(AddAddressRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            response.has_resource = 0;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    ConsumerAddress address = new ConsumerAddress();
                    address.ConsID = request.user_id;
                    UsersHelper.CopyToEntity(address, request);
                    address.CreatedDate = DateTime.Now;
                    address.UpdatedDate = address.CreatedDate;
                    address.StatusID = 1;
                    if (consumer.ConsumerAddresses.Count == 0)
                    {
                        address.IsDefault = true;
                    }
                    else
                    {
                        address.IsDefault = Convert.ToBoolean(request.is_default);
                        if (Convert.ToBoolean(request.is_default))
                        {
                            foreach (var item in consumer.ConsumerAddresses)
                            {
                                item.IsDefault = false;
                            }
                            dao.Update(consumer);
                        }
                    }
                    dao.AddAddress(address);
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("addr.added");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto UpdateAddress(UpdateAddressRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            response.has_resource = 0;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);

                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    ConsumerAddress address = dao.FindAddressById(request.address_id);
                    if (address == null)
                    {
                        response.code = 1;
                        response.message = MessagesSource.GetMessage("no.addr");
                        return response;
                    }

                    if (Convert.ToBoolean(request.is_default))
                    {
                        foreach (var item in consumer.ConsumerAddresses.Where(x => x.IsDefault).ToList())
                        {
                            item.IsDefault = false;
                        }
                        UsersHelper.CopyToEntity(consumer.ConsumerAddresses.Where(x => x.AddrID == request.address_id).FirstOrDefault(), request);
                        dao.Update(consumer);
                    }
                    else
                    {
                        if (consumer.ConsumerAddresses.Where(x => x.IsDefault && x.AddrID != request.address_id).Count() == 0)
                        {
                            UsersHelper.CopyToEntity(consumer.ConsumerAddresses.Where(x => x.AddrID == request.address_id).FirstOrDefault(), request);
                            address.IsDefault = true;
                            dao.Update(consumer);
                        }
                        else
                        {
                            UsersHelper.CopyToEntity(consumer.ConsumerAddresses.Where(x => x.AddrID == request.address_id).FirstOrDefault(), request);
                            dao.Update(consumer);
                        }
                    }

                    //UsersHelper.CopyToEntity(address, request);
                    // dao.UpdateAddress(address);
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("addr.updated");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto DeleteAddress(DeleteAddressRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            response.has_resource = 0;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    //dao.FindById
                    bool isDefault;
                    bool isDeleted = dao.DeleteAddress(request.address_id, out isDefault);
                    if (isDefault)
                    {
                        response.code = 1;
                        response.message = MessagesSource.GetMessage("addr.defualt");
                        response.has_resource = 0;
                    }
                    response.code = isDeleted ? 0 : 1;
                    response.message = isDeleted ? MessagesSource.GetMessage("addr.deleted") : MessagesSource.GetMessage("no.address.found");
                    response.has_resource = 0;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto CheckOTP(CheckOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            response.has_resource = 0;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = dao.FindByMobileNumber(request.mobile_number);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    bool otpValid = OTPServices.ValidateOTP(consumer.ConsID, request.otp_code);
                    OTPServices.RemoveOTP(consumer.ConsID, request.otp_code);// Either way remove this otp if it exists.
                    if (otpValid)
                    {
                        consumer.ConsActivated = true;
                        dao.Update(consumer);
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
            }
            return response;
        }

        public GetAddressResponse GetAddress(GetAddressRequest request)
        {
            GetAddressResponse response = new GetAddressResponse();
            Consumer consumer = null;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    ConsumerAddress consAddr = consumer.ConsumerAddresses.Where(a => a.AddrID == request.address_id).First();
                    UserAddressesDto addrDto = new UserAddressesDto();
                    UsersHelper.CopyFromEntity(addrDto, consAddr, consumer.Name);
                    response.user_address = addrDto;
                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("addr.details");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto UpdateProfile(UpdateProfileRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            response.has_resource = 0;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }

                    //consumer.ProfileImage = string.IsNullOrWhiteSpace(request.profile_image) ? consumer.ProfileImage : request.profile_image; //Commented bcz image is uploading as multipart
                    if (dao.CheckPhoneExists(request.user_mobile_number))
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("cons.reg.dupl");
                    }
                    else
                    {
                        consumer.PhoneNumber = string.IsNullOrWhiteSpace(request.user_mobile_number) ? consumer.PhoneNumber : request.user_mobile_number;
                        consumer.Name = string.IsNullOrWhiteSpace(request.user_name) ? consumer.Name : request.user_name;
                        dao.Update(consumer);
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("profile.updated");
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto UpdateFirebaseDeviceToken(UpdateDeviceTokenRequest request) {

            ResponseDto response = new ResponseDto();
            response.httpCode = System.Net.HttpStatusCode.OK;
            Consumer consumer = null;

            try
            {
                using (UserDao dao = new UserDao())
                {

                    consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        response.code = 1;
                        response.httpCode = System.Net.HttpStatusCode.Unauthorized;
                        return response;
                    }

                    //if (request.push_token == null || request.push_token == "") {
                    //    response.code = 1;
                    //    response.has_resource = 0; //Wrong request
                    //    response.message = "Device token cannot be empty";
                    //    response.httpCode = System.Net.HttpStatusCode.BadRequest;
                    //    return response;
                    //}

                    if (consumer.AppToken != null && consumer.AppToken == request.push_token)
                    {
                        response.code = 0;
                        response.has_resource = 1;
                        response.httpCode = System.Net.HttpStatusCode.OK;
                        response.message = "Device token is up to date";
                    }
                    else
                    {
                        consumer.AppToken = request.push_token;
                        dao.Update(consumer);

                        response.code = 0;
                        response.has_resource = 1;
                        response.httpCode = System.Net.HttpStatusCode.OK;
                        response.message = "Device token updated to " + request.push_token;
                    }
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                response.code = 1;
                response.httpCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public ConsumerAddress GetDefaultUserAddress(int userId)
        {
            using (UserDao dao = new UserDao())
            {
                return dao.FindDefaultAddressFor(userId);
            }
        }

        public ConsumerAddress GetDefaultUserAddressForUser(int addressId)
        {
            using (UserDao dao = new UserDao())
            {
                return dao.FindDefaultAddressForUser(addressId);
            }
        }

        public bool CheckAuthSuperUser(int userId, string authCode, bool withDetails = false)
        {
            SuperAdmin superuser = null;
            using (SuperUserDao dao = new SuperUserDao())
            {
                superuser = GetAuthSuperUser(dao, userId, authCode, withDetails);
                if (superuser == null)
                    return false;
            }

            return superuser != null; ;
        }

        public SuperAdmin GetAuthSuperUser(SuperUserDao superuserDao, int userId, string authCode, bool withDetails = false)
        {
            SuperAdmin superuser = null;
            superuser = superuserDao.FindById(userId);

            if (superuser != null && superuser.AccToken == authCode)
            {
                return superuser;
            }
            return null;
        }


        public ResponseDto ChangeProfilePhoto(ChangeProfilePhotoRequest request)
        {
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            response.has_resource = 0;
            try
            {
                using (UserDao dao = new UserDao())
                {
                    //consumer = GetAuthUser(dao, request.user_id, request.auth_token, true);
                    //if (consumer == null)
                    //{
                    //    MakeNouserResponse(response);
                    //    return response;
                    //}

                    consumer = dao.FindById(request.user_id);
                    if (consumer != null)
                    {
                        consumer.ProfileImage = request.profile_image;
                        dao.Update(consumer);
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

        public ResendOtpResponse ResendOtp(ResendOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResendOtpResponse response = new ResendOtpResponse();
            response.otp_details = new OTPDetailsDto();
            Consumer consumer = null;
            try
            {
                using (UserDao userDao = new UserDao())
                {
                    consumer = userDao.FindByMobileNumber(request.mobile_number);
                }
                if (consumer == null)
                {
                    MakeNouserResponse(response);
                    return response;
                }

                OTPServices.ResendOTP(response, consumer.PhoneNumber, consumer.ConsID, "C");
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

        public ResponseDto ResetPassword(ResetPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            Consumer consumer = null;
            string newPasswordHash = TokenGenerator.GetHashedPassword(request.new_password, 49);
            string confirmPasswordHash = TokenGenerator.GetHashedPassword(request.confirm_password, 49);
            try
            {
                using (UserDao dao = new UserDao())
                {
                    consumer = dao.FindByMobileNumber(request.mobile_number);
                    if (consumer == null)
                    {
                        MakeNouserResponse(response);
                        return response;
                    }
                    if (newPasswordHash == confirmPasswordHash)
                    {
                        consumer.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(consumer);
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
