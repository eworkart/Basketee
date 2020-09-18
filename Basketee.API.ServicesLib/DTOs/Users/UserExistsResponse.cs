using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UserExistsResponse : ResponseDto
    {
        public UserStatusDto user_status { get; set; }
    }
}