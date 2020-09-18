using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class GetAgencyNameDto
    {
        public int agency_id { get; set; }
        public string agency_name { get; set; }
    }
}
