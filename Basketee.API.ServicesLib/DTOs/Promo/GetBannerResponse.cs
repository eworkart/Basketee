using Basketee.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class GetBannerResponse : ResponseDto
    {
        public int banner_id { get; set; }
        public string caption { get; set; }
        public string image_name { get; set; }
        public int category { get; set; }
        public int status_id { get; set; }
    }
}