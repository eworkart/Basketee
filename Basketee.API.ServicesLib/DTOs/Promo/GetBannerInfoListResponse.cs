using Basketee.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class GetBannerInfoListResponse : ResponseDto
    {
        public int info_id { get; set; }
        public string caption { get; set; }
        public string image_name { get; set; }
        public int position { get; set; }
        public int status_id { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
    }
}