using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class UserReminderDto
    {
        public int reminder_id { get; set; }
        public string reminder_image { get; set; }
        public string reminder_description { get; set; }
}
}
