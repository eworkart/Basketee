using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Users
{
    public class UpdateAddressRequest : AddAddressRequest
    {
        public int address_id { get; set; }
    }
}