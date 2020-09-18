using Basketee.API.DTOs.Users;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.Services.Helpers
{
    public class UsersHelper
    {
        /// <summary>
        /// Copies available field values from the DTO into the entity object.
        /// </summary>
        /// <param name="consumer">Consumer instance to copy to.</param>
        /// <param name="request">RegisterRequest to copy from.</param>
        public static void CopyToEntity(Consumer consumer, RegisterRequest request)
        {
            consumer.Name = request.user_name;
            consumer.PhoneNumber = request.mobile_number;
            consumer.Password = request.user_password;
            consumer.IMEI = request.imei;
        }
        /// <summary>
        /// Copies available field values from the entity into the DTO object.
        /// </summary>
        /// <param name="consumer">RegisterRequest to copy from.</param>
        /// <param name="request">Consumer instance to copy to.</param>
        public static void CopyFromEntity(RegisterResponse response, Consumer consumer)
        {
            if (response.new_user == null)
            {
                response.new_user = new NewUserDto();
            }
            response.new_user.user_id = consumer.ConsID;
            response.new_user.auth_token = consumer.AccToken;
        }
        public static void CopyToEntity(Consumer consumer, LoginRequest request)
        {
            consumer.PhoneNumber = request.mobile_number;
            consumer.AppToken = request.push_token;
            consumer.Password = request.user_password;
            consumer.AppID = request.app_id;
        }
        public static void CopyFromEntity(LoginResponse response, Consumer consumer)
        {
            if (response.user_login == null)
            {
                response.user_login = new UserLoginDto();
            }
            response.user_login.user_activated = consumer.ConsActivated ? 1 : 0;
            response.user_login.user_exists = 1;
            response.user_login.user_blocked = consumer.ConsBlocked ? 1 : 0;
            response.user_login.user_id = consumer.ConsID;
            response.user_login.auth_token = consumer.AccToken;
            response.user_login.allow_login = (consumer.ConsActivated && (consumer.ConsBlocked != true)) ? 1 : 0;
            if (response.user_details == null)
            {
                response.user_details = new UserLoginDetailsDto();
            }
            response.user_details.user_name = consumer.Name;
            response.user_details.profile_image = consumer.ProfileImage != null ? ImagePathService.consumerImagePath + consumer.ProfileImage : string.Empty;
            response.user_details.mobile_number = consumer.PhoneNumber;
            response.user_details.postal_code = (consumer.ConsumerAddresses.Count > 0 ? consumer.ConsumerAddresses.Where(x => x.IsDefault == true).FirstOrDefault().PostalCode : string.Empty);
            response.user_details.region_name = (consumer.ConsumerAddresses.Count > 0 ? consumer.ConsumerAddresses.Where(x => x.IsDefault == true).FirstOrDefault().RegionName : string.Empty);
            response.user_details.user_address = (consumer.ConsumerAddresses.Count > 0 ? consumer.ConsumerAddresses.Where(x => x.IsDefault == true).FirstOrDefault().Address : string.Empty);
        }
        public static void CopyToEntity(Consumer consumer, GetUserDetailsRequest request)
        {
            consumer.ConsID = request.user_id;
            consumer.AccToken = request.auth_token;// auth_token, source
        }
        public static void CopyFromEntity(GetUserDetailsResponse response, Consumer consumer)
        {
            if (response.user_details == null)
            {
                response.user_details = new UserDetailsDto();
            }
            response.user_details.user_id = consumer.ConsID;
            //response.user_details.user_activated = consumer.ConsActivated ? 1 : 0;
            //response.user_details.user_blocked = consumer.ConsBlocked ? 1 : 0;
            response.user_details.user_name = consumer.Name;
            response.user_details.user_mobile_num = consumer.PhoneNumber;
            //response.user_details.user_created_date = consumer.CreatedDate;
            //response.user_details.user_last_login = consumer.LastLogin ?? DateTime.MinValue;
            response.user_details.profile_image = consumer.ProfileImage != null ? ImagePathService.consumerImagePath + consumer.ProfileImage : string.Empty;
            //if (consumer.ConsumerAddresses.Count() > 0)
            //{
            //    response.user_details.user_addresses = new List<UserAddressesDto>();
            //    foreach (ConsumerAddress consAddr in consumer.ConsumerAddresses)
            //    {
            //        UserAddressesDto usrAddr = new UserAddressesDto();
            //        CopyFromEntity(usrAddr, consAddr, consumer.Name);
            //        response.user_details.user_addresses.Add(usrAddr);
            //        if (consAddr.IsDefault)
            //        {
            //            response.user_details.user_latitude = consAddr.Latitude;
            //            response.user_details.user_longitude = consAddr.Longitude;
            //        }
            //    }
            //}
            response.user_details.user_mobile_num = consumer.PhoneNumber;
        }

        public static void CopyFromEntity(UserAddressesDto usrAddr, ConsumerAddress consAddr, string userName)
        {
            usrAddr.address_id = consAddr.AddrID; // more_info, is_default
            usrAddr.user_name = userName;
            usrAddr.is_default = consAddr.IsDefault ? 1 : 0;
            usrAddr.user_address = consAddr.Address;
            usrAddr.region_name = consAddr.RegionName;
            usrAddr.more_info = consAddr.AdditionalInfo;
            usrAddr.postal_code = consAddr.PostalCode;
            usrAddr.user_latitude = consAddr.Latitude;
            usrAddr.user_longitude = consAddr.Longitude;
        }
        //public static void CopyToEntity(Consumer consumer, ResendOtpRequest request)
        //{
        //    consumer.ConsID = request.user_id;
        //    consumer.AccToken = request.auth_token;
        //    consumer.IMEI = request.imei;
        //}
        /*
        public static void CopyFromEntity(ResendOtpResponse response, Consumer consumer)
        {
            response.otp_details.send_otp = consumer.;
        }
        */
        public static void CopyToEntity(Consumer consumer, ActivateUserRequest request)
        {
            consumer.ConsID = request.user_id;
            consumer.AccToken = request.auth_token;
            consumer.IMEI = request.imei;
        }
        public static void CopyToEntity(Consumer consumer, UserExistsRequest request)
        {
            consumer.PhoneNumber = request.mobile_number;
            consumer.IMEI = request.imei;
        }
        public static void CopyFromEntity(UserExistsResponse response, Consumer consumer)
        {
            if (response.user_status == null)
            {
                response.user_status = new UserStatusDto();
            }
            response.user_status.user_id = consumer.ConsID;
            response.user_status.auth_token = consumer.AccToken;
            response.user_status.user_exists = 1;
        }

        public static void CopyFromEntity(GetAddressesResponse response, Consumer consumer)
        {
            if (response.user_addresses == null)
            {
                response.user_addresses = new List<UserAddressesDto>();
            }
            if (consumer.ConsumerAddresses.Count() > 0)
            {
                response.user_addresses = new List<UserAddressesDto>();
                foreach (ConsumerAddress consAddr in consumer.ConsumerAddresses.Where(x => x.StatusID == 1))
                {
                    UserAddressesDto usrAddr = new UserAddressesDto();
                    CopyFromEntity(usrAddr, consAddr, consumer.Name);
                    response.user_addresses.Add(usrAddr);
                }
            }
        }

        public static void CopyToEntity(ConsumerAddress address, AddAddressRequest request)
        {
            address.ConsID = request.user_id;
            address.Address = request.user_address;
            address.RegionName = request.region_name;
            address.PostalCode = request.postal_code;
            address.AdditionalInfo = request.more_info;
            address.IsDefault = Convert.ToBoolean(request.is_default);
            address.Latitude = request.user_latitude;
            address.Longitude = request.user_longitude;
        }
    }
}