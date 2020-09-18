using Basketee.API.DTOs.Promo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class InfoBannerResponse : ResponseDto
    {
        public InfoBannerDto[] info_banners { get; set; }
    }
}