using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class InfoBannerListDto
    {
        public int info_id { get; set; }
        public string caption { get; set; }
        public string image_name { get; set; }
        public int position { get; set; }
        public int status_id { get; set; }
        public short created_by { get; set; }
        public DateTime created_date { get; set; }
        public short updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
}