using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class GetDriverNameDto
    {
        public int driver_id { get; set; }
        public string driver_name { get; set; }
    }
}
