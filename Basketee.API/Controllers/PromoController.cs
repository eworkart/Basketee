using System.Web.Http;
using Basketee.API.DTOs.Promo;
using Basketee.API.Services;
using System.Web.Http.Results;
using System.Net;

namespace Basketee.API.Controllers
{
    public class PromoController : ApiController
    {
        private PromoServices _promoServices = new PromoServices();
        [HttpPost]
        [ActionName("get_banner")]
        public NegotiatedContentResult<GetBannerResponse> GetBanner([FromBody]GetBannerRequest request)
        {
            GetBannerResponse resp = _promoServices.GetBanner(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_banner_list")]
        public NegotiatedContentResult<GetBannerListResponse> GetBannerList([FromBody]GetBannerListRequest request)
        {
            GetBannerListResponse resp = _promoServices.GetBannerList(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_info_banner")]
        public NegotiatedContentResult<InfoBannerResponse> GetInfoBanner([FromBody]GetInfoBannerRequest request)
        {
            InfoBannerResponse resp = _promoServices.GetInfoBanner(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_info_banner_list")]
        public NegotiatedContentResult<GetInfoBannerListResponse> GetInfoBannerList([FromBody]GetInfoBannerListRequest request)
        {
            GetInfoBannerListResponse resp = _promoServices.GetInfoBannerList(request);
            return Content(HttpStatusCode.OK, resp);
        }
    }
}