using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class GetAddressResponse : ResponseDto
    {
        public UserAddressesDto user_address { get; set; }
    }
}