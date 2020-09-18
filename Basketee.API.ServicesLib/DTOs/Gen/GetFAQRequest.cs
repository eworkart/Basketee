using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Gen
{
    public class GetFAQRequest
    {
        public int row_per_page { get; set; }
        public int page_number { get; set; }
    }
}
