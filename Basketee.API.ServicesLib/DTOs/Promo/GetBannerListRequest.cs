using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class GetBannerListRequest
    {
        public string auth_token { get; set; }
        public int page_number { get; set; }
        public int records_per_page { get; set; }
        public int user_id { get; set; }
        public int user_type { get; set; }
    }
}