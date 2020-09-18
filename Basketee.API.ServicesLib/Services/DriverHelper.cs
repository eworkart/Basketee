using Pertamina.LPG.API.DTOs.Driver;
using Pertamina.LPG.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pertamina.LPG.API.Services.Helpers
{
    class DriverHelper
    {
        public static void CopyToEntity(Driver driver, LoginRequest request)
        {
            driver.MobileNumber = request.mobile_number; // password
            driver.AppID = request.app_id;
            driver.AppToken = request.push_token;
        }
        public static void CopyFromEntity(LoginResponse dto, Driver driver, Reminder reminder)
        {
            if (dto.user_login == null)
                dto.user_login = new UserLoginDto();
            dto.user_login.user_id = driver.DrvrID;
            dto.user_login.auth_token = driver.AccToken;
            if (dto.driver_details == null)
                dto.driver_details = new DriverDetails();
            dto.driver_details.agency_name = (driver.Agency != null ? driver.Agency.AgencyName : "");
            dto.driver_details.driver_name = driver.DriverName;
            dto.driver_details.mobile_number = driver.MobileNumber;
            dto.driver_details.profile_image = driver.ProfileImage;
            //if (dto.has_reminder)
            //{
            if (dto.reminder_details == null)
                dto.reminder_details = new ReminderDetailsDto();
            dto.reminder_details.reminder_description = reminder == null ? "" : reminder.Description;
            dto.reminder_details.reminder_id = reminder == null ? 0 : reminder.RmdrID;
            dto.reminder_details.reminder_image = reminder == null ? "" : reminder.ReminderImage;
            //}
        }
        public static void CopyToEntity(Driver driver, ForgotPasswordRequest request)
        {
            driver.MobileNumber = request.mobile_number;
        }
        public static void CopyFromEntity(ResetPasswordDto response, Driver driver)
        {
            //response.password_reset = driver.;
            //response.password_otp_sent = driver.;

        }
        public static void CopyToEntity(Driver driver, ChangePasswordDriverRequest request)
        {
            driver.DrvrID = request.user_id; // auth_token, old_password, new_password
        }
        public static void CopyToEntity(Driver driver, GetAgentDriverRequest request)
        {
            driver.DrvrID = request.user_id; // auth_token
        }
        public static void CopyFromEntity(DriverDetailsDto response, Driver driver)
        {
            response.driver_id = driver.DrvrID;
            response.profile_image = driver.ProfileImage;
            response.driver_name = driver.DriverName;
            response.mobile_number = driver.MobileNumber;
        }
        public static void CopyFromEntity(DTOs.Orders.DriverDetailListDto response, Driver driver)
        {
            response.driver_id = driver.DrvrID;
            response.driver_name = driver.DriverName;
            response.driver_availability = "";
        }
    }
}
