using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.SuperUser
{
    public class LoginResponse : ResponseDto
    {
        public SuperUserLoginDto user_login { get; set; }
        public SuperUserLoginDetailsDto super_user_details { get; set; }
    }
}
