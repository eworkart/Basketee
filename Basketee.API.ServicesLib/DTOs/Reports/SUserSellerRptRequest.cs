using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class SUserSellerRptRequest
    {
        public int user_id { get; set; }
        public string auth_token { get; set; }
        public int total_type { get; set; }
        public int periodical_data { get; set; }
        //public List<int> productIds { get; set; }
        public List<ProductInputDto> products { get; set; }        
        public int number_of_products { get; set; }
        public int agency_id { get; set; }
        
    }
}
