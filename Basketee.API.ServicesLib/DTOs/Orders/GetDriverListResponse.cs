using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.Driver;

namespace Basketee.API.DTOs.Orders
{
    public class GetDriverListResponse : ResponseDto
    {
        public List<DriverDetailListDto> driver_details { get; set; }
    }
}
