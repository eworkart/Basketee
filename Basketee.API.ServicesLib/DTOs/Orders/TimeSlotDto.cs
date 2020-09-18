using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class TimeSlotDto
    {        
        public string date { get; set; }
        public int time_slot_id { get; set; }
        public string time_slot_name { get; set; }
        public int availability { get; set; }
        public int agency_id { get; set; }



        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            TimeSlotDto other = (TimeSlotDto)obj;
            if (this.time_slot_id != other.time_slot_id)
            {
                return false;
            }
            if (this.date != other.date)
            {
                return false;
            }
            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            int hash = 29;
            hash = hash * this.time_slot_id * 7;
            if (this.date != null)
            {
                hash = hash * this.date.GetHashCode() * 11;
            }
            return hash;
        }
    }
}