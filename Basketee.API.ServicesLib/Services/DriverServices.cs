using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs;
using Basketee.API.DTOs.Driver;
using Basketee.API.DAOs;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;
using Basketee.API.DTOs.Gen;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.Services
{
    public class DriverServices
    {
        UserServices _userServices = new UserServices();
        OrdersServices _ordersServices = new OrdersServices();
        public LoginResponse Login(LoginRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            LoginResponse response = new LoginResponse();
            Driver driver = null;
            string hashPassword = TokenGenerator.GetHashedPassword(request.password, 49);
            try
            {
                using (DriverDao dao = new DriverDao())
                {
                    //driver = dao.FindByMobileNumber(request.mobile_number);
                    driver = dao.FindByMobileNumberAndPassword(request.mobile_number, hashPassword);

                    if (driver == null)
                    {
                        MakeNoDriverResponse(response);
                        return response;
                    }

                    driver.AppID = request.app_id;
                    driver.AppToken = request.push_token;
                    driver.LastLogin = DateTime.Now;
                    driver.AccToken = TokenGenerator.GenerateToken(driver.DriverName, driver.Password, driver.MobileNumber);
                    dao.Update(driver);
                    response.code = 0;
                    response.user_login = new UserLoginDto();
                    response.driver_details = new DriverDetails();
                    response.reminder_details = new ReminderDetailsDto();

                    var reminder = dao.GetRemindersForDriver();
                    response.has_reminder = (reminder == null ? 0 : 1);
                    //if (reminder == null)
                    //{
                    //    reminder = new Reminder();
                    //}
                    DriverHelper.CopyFromEntity(response, driver, reminder);

                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("login.ok");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        //public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        //{
        //    ForgotPasswordResponse response = new ForgotPasswordResponse();
        //    string newPassword = TokenGenerator.GenerateResetPassword();
        //    Driver drv = null;
        //    try
        //    {

        //        using (DriverDao dao = new DriverDao())
        //        {
        //            drv = dao.FindByMobileNumber(request.mobile_number);
        //            drv.Password = TokenGenerator.GetHashedPassword(newPassword, 49);
        //            dao.Update(drv);
        //            OTPServices.SendPasswordMessage(drv.MobileNumber, newPassword);
        //            response.code = 0;
        //            response.has_resource = 1;
        //            response.reset_password = new ResetPasswordDto();
        //            response.reset_password.password_otp_sent = 1;
        //            response.reset_password.password_reset = 1;
        //            response.message = MessagesSource.GetMessage("passwd.reset");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.MakeExceptionResponse(ex);
        //    }

        //    return response;
        //}

        public ResponseDto CheckOTP(CheckOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResponseDto response = new ResponseDto();
            Driver driver = null;
            response.has_resource = 0;
            try
            {
                using (DriverDao dao = new DriverDao())
                {
                    driver = dao.FindByMobileNumber(request.mobile_number);
                    if (driver == null)
                    {
                        MakeNoDriverResponse(response);
                        return response;
                    }


                    bool otpValid = OTPServices.ValidateOTP(driver.DrvrID, request.otp_code);
                    OTPServices.RemoveOTP(driver.DrvrID, request.otp_code);// Either way remove this otp if it exists.
                    if (otpValid)
                    {
                        driver.StatusId = true;
                        dao.Update(driver);
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

        //public ResendOtpResponse ResendOtp(ResendOtpRequest request)
        //{
        //    ResendOtpResponse response = new ResendOtpResponse();
        //    response.otp_details = new OtpDetailsDto();
        //    Driver driver = null;
        //    try
        //    {
        //        using (DriverDao userDao = new DriverDao())
        //        {
        //            driver = userDao.FindByMobileNumber(request.mobile_number);
        //        }
        //        if (driver == null)
        //        {
        //            MakeNoDriverResponse(response);
        //            return response;
        //        }

        //        OTPServices.ResendOTP(driver.DrvrID, driver.MobileNumber, "D");
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

        public ResponseDto ResetPassword(ResetPasswordRequest request)
        {
            ResponseDto response = new ResponseDto();
            Driver driver = null;
            string newPasswordHash = TokenGenerator.GetHashedPassword(request.new_password, 49);
            string confirmPasswordHash = TokenGenerator.GetHashedPassword(request.confirm_password, 49);
            try
            {

                using (DriverDao dao = new DriverDao())
                {
                    driver = dao.FindByMobileNumber(request.mobile_number);
                    if (driver == null)
                    {
                        MakeNoDriverResponse(response);
                        return response;
                    }
                    if (newPasswordHash == confirmPasswordHash)
                    {
                        driver.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(driver);
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

        public ResponseDto ChangePasswordDriver(ChangePasswordDriverRequest request)
        {
            ResponseDto response = new ResponseDto();
            Driver driver = null;
            string oldPasswordHash = TokenGenerator.GetHashedPassword(request.old_password, 49);
            try
            {

                using (DriverDao dao = new DriverDao())
                {
                    driver = dao.FindById(request.user_id);
                    if (driver.Password == oldPasswordHash)
                    {
                        driver.Password = TokenGenerator.GetHashedPassword(request.new_password, 49);
                        dao.Update(driver);
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("password.changed");
                        return response;
                    }
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("pass.not.chg");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetAgentDriverResponse GetAgentDriver(GetAgentDriverRequest request)
        {
            GetAgentDriverResponse response = new GetAgentDriverResponse();
            Driver driver = null;
            try
            {
                if (!CheckAuthDriver(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (DriverDao dao = new DriverDao())
                {
                    driver = dao.FindById(request.user_id);
                    DTOs.Driver.DriverDetailsDto dto = new DTOs.Driver.DriverDetailsDto();
                    DriverHelper.CopyFromEntity(dto, driver);
                    response.code = 0;
                    response.has_resource = 1;
                    response.driver_details = dto;
                    response.message = MessagesSource.GetMessage("got.agent.driver");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }
        public static void MakeNoDriverResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.driver");
        }

        public static bool CheckAuthDriver(int driverId, string authCode)
        {
            Driver drv = null;
            using (DriverDao dao = new DriverDao())
            {
                drv = GetAuthDriver(dao, driverId, authCode);
            }

            return drv != null; ;
        }

        private static Driver GetAuthDriver(DriverDao dao, int driverId, string authCode)
        {
            Driver drv = dao.FindById(driverId);
            if (drv == null)
                return null;
            if (drv.AccToken == authCode)
            {
                return drv;
            }
            return null;
        }

        public void MakeInvalidOrderTypeResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.order.type");
        }
        public DTOs.Driver.GetEReceiptResponse GetERecieptDetails(DTOs.Driver.GetEReceiptRequest request)
        {
            DTOs.Driver.GetEReceiptResponse response = new DTOs.Driver.GetEReceiptResponse();
            try
            {
                if (!CheckAuthDriver(request.user_id, request.auth_token))
                {
                    MakeNoDriverResponse(response);
                    return response;
                }
                OrdersServices.OrderType orderType;
                try
                {
                    orderType = request.order_type.ToEnumValue<OrdersServices.OrderType>();
                }
                catch
                {
                    MakeInvalidOrderTypeResponse(response);
                    return response;
                }
                //orderType = request.order_type.ToEnumValue<OrdersServices.OrderType>();
                //response.orders = GetOrderInvoiceOrReciept(request.user_id, orderType, request.order_id, true);
                response.orders = _ordersServices.GetOrderInvoiceOrReciept(request.user_id, orderType, request.order_id, (int)UserType.Driver);
                if(response.orders == null)
                {
                    _ordersServices.MakeNoOrderFoundResponse(response);
                    return response;
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("ereceipt.details");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public static OrderInvoiceDto GetOrderInvoiceOrReciept(int user_id, OrdersServices.OrderType order_type, int order_id, bool isConReceipt)
        {
            OrderInvoiceDto response = new OrderInvoiceDto();
            if (order_type == OrdersServices.OrderType.OrderApp)
            {
                using (OrderDao dao = new OrderDao())
                {
                    //Order ord = (isConReceipt ? dao.GetConsumerOrder(user_id, order_id) : dao.GetAgentOrder(user_id, order_id));
                    Order ord = dao.GetDriverOrder(user_id, order_id);
                    OrderInvoiceDto dto = new OrderInvoiceDto();
                    DriverHelper.CopyFromEntity(dto, ord);
                    response = dto;
                }
            }
            else if (order_type == OrdersServices.OrderType.OrderTelp)
            {
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    TeleOrder ord = dao.GetDriverOrder(user_id, order_id);
                    OrderInvoiceDto dto = new OrderInvoiceDto();
                    DriverHelper.CopyFromEntity(dto, ord);
                    response = dto;
                }
            }
            return response;
        }

        public UpdateProfileResponse UpdateProfile(UpdateProfileRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            UpdateProfileResponse response = new UpdateProfileResponse();
            try
            {
                if (!CheckAuthDriver(request.user_id, request.auth_token))
                {
                    MakeNoDriverResponse(response);
                    return response;
                }
                using (DriverDao dao = new DriverDao())
                {
                    Driver driver = dao.FindById(request.user_id);
                    driver.DriverName = request.driver_name;
                    //driver.MobileNumber = request.mobile_number;
                    //driver.ProfileImage = request.profile_image; //Commented bcz image is uploading as multipart
                    dao.Update(driver);
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

        public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            //string newPassword = TokenGenerator.GenerateResetPassword();            
            try
            {
                using (DriverDao dao = new DriverDao())
                {
                    Driver driver = dao.FindByMobileNumber(request.mobile_number);
                    if (driver == null)
                    {
                        MakeNoDriverResponse(response);
                        return response;
                    }
                    OTPServices.SendOTPForForgotPassword(response, request.mobile_number, driver.DrvrID, "D");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public ResponseDto ChangeProfilePhoto(ChangeProfilePhotoRequest request)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                //if (!CheckAuthDriver(request.user_id, request.auth_token))
                //{
                //    MakeNoDriverResponse(response);
                //    return response;
                //}
                using (DriverDao dao = new DriverDao())
                {
                    Driver driver = dao.FindById(request.user_id);
                    if (driver != null)
                    {
                        driver.ProfileImage = request.profile_image;
                        dao.Update(driver);
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

        public ResendOtpResponse ResendOtp(ResendOtpRequest request)
        {
            request.mobile_number = Common.GetStandardMobileNumber(request.mobile_number);
            ResendOtpResponse response = new ResendOtpResponse();
            response.otp_details = new OTPDetailsDto();
            Driver driver = null;
            try
            {
                using (DriverDao driverDao = new DriverDao())
                {
                    driver = driverDao.FindByMobileNumber(request.mobile_number);
                }
                if (driver == null)
                {
                    MakeNoDriverResponse(response);
                    return response;
                }

                OTPServices.ResendOTP(response, driver.MobileNumber, driver.DrvrID, "D");
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
