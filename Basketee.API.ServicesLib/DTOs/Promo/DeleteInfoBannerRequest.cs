using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class DeleteInfoBannerRequest
    {
        public int suser_id { get; set; }
        public string auth_token { get; set; }
        public int info_id { get; set; }
    }
}