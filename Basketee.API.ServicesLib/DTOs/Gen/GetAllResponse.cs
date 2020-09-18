using Basketee.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Gen
{
    public class GetAllResponse : ResponseDto
    {
        public FAQDto[] faq_list { get; set; }
    }
}