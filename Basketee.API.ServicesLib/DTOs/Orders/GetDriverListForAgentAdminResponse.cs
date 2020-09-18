using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class GetDriverListForAgentAdminResponse : ResponseDto
    {
        public List<AllDriversForAdmin> driver_details { get; set; }
    }
}
