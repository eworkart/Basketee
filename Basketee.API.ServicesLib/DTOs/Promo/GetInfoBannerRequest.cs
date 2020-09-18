using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class GetInfoBannerRequest
    {
        public string auth_token { get; set; }
        public int user_id { get; set; }
        public int user_type { get; set; }
    }
}