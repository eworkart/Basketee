using Basketee.API.DTOs;
using Basketee.API.DTOs.Promo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DTOs.Promo
{
    public class GetBannerListResponse : ResponseDto
    {
        public BannerDto[] banners { get; set; }
    }
}