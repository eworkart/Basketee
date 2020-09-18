using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Driver
{
    public class LoginResponse : ResponseDto
    {
        public UserLoginDto user_login { get; set; }
        public int has_reminder { get; set; }
        public ReminderDetailsDto reminder_details { get; set; }
        public DriverDetails driver_details { get; set; }
    }
}
