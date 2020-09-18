using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class AllDriversForAdmin
    {
        public DriverDto drivers { get; set; }
        public List<TimeslotDisplayDto> driver_availability { get; set; }
    }
}
