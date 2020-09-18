using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Orders
{
    public class TimeslotDisplayDto
    {
        public string day_name { get; set; }
        public string day_date { get; set; }
        //public int time_slot_id { get; set; }
        //public string time_slot_name { get; set; }
        //public int availability { get; set; }
        //public int agency_id { get; set; }
        public List<TimeslotDaysDto> time_slot { get; set; }
    }
}
