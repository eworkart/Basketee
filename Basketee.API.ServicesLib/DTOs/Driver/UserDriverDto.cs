using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class UserDriverDto : DriverDetailsDto
    {
        public string agency_name { get; set; }
    }
}
