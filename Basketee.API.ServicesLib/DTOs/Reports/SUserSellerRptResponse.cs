﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class SUserSellerRptResponse :ResponseDto
    {
        public List<SUserSellerRptDto> sales_details { get; set; }
    }
}
