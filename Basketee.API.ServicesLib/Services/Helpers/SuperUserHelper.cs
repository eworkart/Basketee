using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs;
using Basketee.API.Models;
using Basketee.API.DTOs.SuperUser;

namespace Basketee.API.Services.Helpers
{
   public class SuperUserHelper
    {
        public static void CopyToEntity(SuperAdmin superadmin, LoginRequest request)
        {
            superadmin.Password = request.password; // app_id
            superadmin.MobileNum = request.mobile_number;
            superadmin.AppToken = request.push_token;
        }
        public static void CopyFromEntity(SuperUserLoginDto dto, SuperAdmin superadmin)
        {
            dto.auth_token = superadmin.AccToken;
            dto.user_id = superadmin.SAdminID;


        }
        public static void CopyFromEntity(SuperUserLoginDetailsDto dto, SuperAdmin superadmin)
        {
            dto.profile_image = ImagePathService.superUserImagePath + superadmin.ProfileImage;
            dto.super_user_name = superadmin.FullName;


        }
    }
}
