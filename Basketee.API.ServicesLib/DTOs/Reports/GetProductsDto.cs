using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class GetProductsDto
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
    }
}
