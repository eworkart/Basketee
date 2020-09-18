using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class ForgotPasswordResponseUser : ResponseDto
    {
        public ResetPasswordDto reset_password { get; set; }
    }
}