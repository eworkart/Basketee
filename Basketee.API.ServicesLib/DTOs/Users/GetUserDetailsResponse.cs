using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class GetUserDetailsResponse : ResponseDto
    {
        public UserDetailsDto user_details { get; set; }
    }
}