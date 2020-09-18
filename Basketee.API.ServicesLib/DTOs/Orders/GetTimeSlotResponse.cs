using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Orders
{
    public class GetTimeSlotResponse : ResponseDto
    {
        //public string day { get; set; }
        //public string date { get; set; }
        public List<TimeslotDisplayDto> days { get; set; }

        //public string day_name1 { get; set; }            
        
        //public string date1 { get; set; }      
       
        //public TimeSlotDto[] today { get; set; }
        //public string day_name2 { get; set; }
        //public string date2 { get; set; }
        //public TimeSlotDto[] tommorrow { get; set; }
        //public string day_name3 { get; set; }
        //public string date3 { get; set; }
        //public TimeSlotDto[] day_aftertommorrow { get; set; }
    }
}