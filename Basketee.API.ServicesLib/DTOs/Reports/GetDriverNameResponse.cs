﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.Reports
{
    public class GetDriverNameResponse : ResponseDto
    {        
        public List<GetDriverNameDto> driver_names { get; set; }
    }
}
