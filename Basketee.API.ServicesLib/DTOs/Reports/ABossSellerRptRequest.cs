using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class ABossSellerRptRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public int total_type { get; set; }
        public int periodical_data { get; set; }
        public List<ProductInputDto> products { get; set; }
    }
}
