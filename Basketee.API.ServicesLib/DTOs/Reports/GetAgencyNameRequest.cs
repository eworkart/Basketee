using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
   public class GetAgencyNameRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
    }
}
