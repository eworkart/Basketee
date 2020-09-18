using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class TimeslotDaysDto
    {
        public int time_slot_id { get; set; }
        public string time_slot_name { get; set; }
        public string availability { get; set; }
        //public int agency_id { get; set; }
    }
}
