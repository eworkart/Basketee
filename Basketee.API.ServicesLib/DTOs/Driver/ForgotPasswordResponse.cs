using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class ForgotPasswordResponseDriver : ResponseDto
    {
        public ResetPasswordDto reset_password { get; set; }
    }
}
