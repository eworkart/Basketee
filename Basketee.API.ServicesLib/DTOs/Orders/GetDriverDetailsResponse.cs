using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetDriverDetailsResponse : ResponseDto
    {
        public DriverDetailsDto driver_details { get; set; }
    }
}