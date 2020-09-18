using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.SuperUser
{
   public class SuperUserLoginDto
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
    }
}
