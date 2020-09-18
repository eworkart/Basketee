using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Gen
{
    public class ForgotPasswordResponse : ResponseDto
    {
        public ForgotPasswordDto reset_password { get; set; }
    }
}
