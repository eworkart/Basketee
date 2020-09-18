using Basketee.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class GetInfoBannerListResponse : ResponseDto
    {
        public InfoBannerListDto[] info_banners { get; set; }

    }
}