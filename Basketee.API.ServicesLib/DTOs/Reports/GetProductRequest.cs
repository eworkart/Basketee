using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class GetProductRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public bool is_boss { get; set; }
    }
}
