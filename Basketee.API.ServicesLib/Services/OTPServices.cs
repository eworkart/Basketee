using Basketee.API.DAOs;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Gen;

namespace Basketee.API.Services
{
    public class OTPServices
    {
        //Template for the OTP message
        const string OTP_MESSAGEING_TEMPLATE = "The OTP for activation your account with Pertamina is {0}";

        //Template for the password reset message
        const string PASSWORD_MESSAGEING_TEMPLATE = "Your password has been reset; the new password is '{0}'";

        const string TEXT_MESSAGING_SERVICE_URL = "";
        const int OTP_LENGTH = 5;
        const int OTP_LIFESPAN_SECS = 1800;//TODO change to 180

        public static string GenerateAndSendOTP(string phoneNumber)
        {
            string otp = GenerateOTP();
            SendOTP(phoneNumber, otp);
            return otp;
        }

        public static string GenerateOTP()
        {
            string otp = string.Empty;
            Random rnd = new Random();
            for (int i = 0; i < OTP_LENGTH; i++)
            {
                otp += rnd.Next(10);
            }
            return otp;
        }

        public static void SendOTP(string phoneNumber, string otp)
        {
            //string message = string.Format(OTP_MESSAGEING_TEMPLATE, otp);
            //SMSService.SendSMS(phoneNumber, message);
            SMSService.SendOTP(phoneNumber);
        }
        public static bool SaveOTP(string otp, int userId, string userType)
        {
            OneTimePwd otpEntity = new OneTimePwd { Otg = otp, UserID = userId, UserType = userType, CreatedDate = DateTime.Now };
            using (OneTimePwdDao dao = new OneTimePwdDao())
            {
                dao.Insert(otpEntity);
            }
            return true;
        }
        /// <summary>
        /// Remove all OTP records past the OTP lifespan.
        /// </summary>
        /// <param name="dao">The db context to use</param>
        private static void ReapOldOTPs(OneTimePwdDao dao)
        {
            DateTime timeLimit = DateTime.Now.AddSeconds(-OTP_LIFESPAN_SECS);
            dao.DeleteOlderOTP(timeLimit);
        }
        /// <summary>
        /// Resends an OTP message. 
        /// If there is a current OTP, then the same OTP is resent. 
        /// Otherwise a new OTP is generated and sent.
        /// </summary>
        /// <param name="userId">Id of the recipient user.</param>
        /// <param name="phoneNumber">Phone number to send OTP to.</param>
        /// <param name="userType">The user type.</param>
        public static void ResendOTP(int userId, string phoneNumber, string userType)
        {
            OneTimePwd otp = null;
            string otpString = string.Empty;
            using (OneTimePwdDao dao = new OneTimePwdDao())
            {
                ReapOldOTPs(dao);
                otp = dao.FindByUserId(userId);// Check if there is a current OTP for the user.
                if (otp != null) // If there is, just reset the timestamp on the OTP
                {
                    otpString = otp.Otg;
                    otp.CreatedDate = DateTime.Now;
                    dao.Update(otp);
                }
                else
                {
                    otpString = GenerateOTP();
                    otp = new OneTimePwd { Otg = otpString, UserID = userId, UserType = userType, CreatedDate = DateTime.Now };
                    dao.Insert(otp);
                }
            }
            SendOTP(phoneNumber, otpString);
        }

        public static void SendPasswordMessage(string phoneNumber, string newPassword)
        {
            string message = string.Format(PASSWORD_MESSAGEING_TEMPLATE, newPassword);
            SMSService.SendSMS(phoneNumber, message);
        }

        public static bool ValidateOTP(int user_id, string otp_code)
        {
            //if (otp_code == "1111")//TODO : comment out this block; only for testing
            //{
            //    return true;
            //}
            using (OneTimePwdDao dao = new OneTimePwdDao())
            {
                ReapOldOTPs(dao);
                OneTimePwd otp = dao.FindByUserId(user_id);
                if (otp != null && otp.Otg == otp_code)
                {
                    return true;
                }
            }
            return false;
        }

        public static void RemoveOTP(int user_id, string otp_code)
        {
            using (OneTimePwdDao dao = new OneTimePwdDao())
            {
                ReapOldOTPs(dao);
                OneTimePwd otp = dao.FindByUserId(user_id);
                if (otp != null && otp.Otg == otp_code)
                {
                    dao.DeleteOTP(otp);
                }
            }
        }

        public static void SendOTPForForgotPassword(ForgotPasswordResponse response, string mobileNumber, int userId, string userType)
        {
            if (response == null)
                response = new ForgotPasswordResponse();
            if (response.reset_password == null)
                response.reset_password = new ForgotPasswordDto();
            string otp = SMSService.SendOTP(mobileNumber);
            if(string.IsNullOrWhiteSpace(otp))
            {
                response.code = 1;
                response.has_resource = 0;
                response.message = MessagesSource.GetMessage("otp.not.sent");
                return;
            }
            if (SaveOTP(otp, userId, userType))
            {
                response.reset_password.new_password_otp_sent = 1; // state that OTP has been sent.
            }
            response.code = 0;
            response.has_resource = 1;
            response.message = MessagesSource.GetMessage("otp.sent");
        }

        public static void ResendOTP(ResendOtpResponse response, string mobileNumber, int userId, string userType)
        {
            mobileNumber = Common.GetStandardMobileNumber(mobileNumber);
            if (response == null)
                response = new ResendOtpResponse();
            if (response.otp_details == null)
                response.otp_details = new OTPDetailsDto();
            string otp = SMSService.SendOTP(mobileNumber);
            if (SaveOTP(otp, userId, userType))
            {
                response.otp_details.send_otp = 1; // state that OTP has been sent.
            }
            response.code = 0;
            response.has_resource = 1;
            response.message = MessagesSource.GetMessage("otp.sent");
        }
    }
}