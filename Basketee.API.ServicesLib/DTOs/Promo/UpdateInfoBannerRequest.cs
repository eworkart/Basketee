using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class UpdateInfoBannerRequest
    {
        public int suser_id { get; set; }
        public string auth_token { get; set; }
        public int info_id { get; set; }
        public string caption { get; set; }
        public string image_name { get; set; }
        public int position { get; set; }
        public int status_id { get; set; }
    }
}