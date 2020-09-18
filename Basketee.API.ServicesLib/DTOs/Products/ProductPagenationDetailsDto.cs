using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Product
{
    public class ProductPagenationDetailsDto
    {
        public int total_num_products { get; set; }
        public int row_per_page { get; set; }
        public int page_number { get; set; }
        public int total_pages { get; set; }
    }
}