using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class DeleteBannerRequest
    {
        public int suser_id { get; set; }
        public string auth_token { get; set; }
        public int banner_id { get; set; }
    }
}