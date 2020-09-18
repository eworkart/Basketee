using Basketee.API.DTOs.Promo;
using Basketee.API.Models;

namespace Basketee.API.Services.Helpers
{
    public class PromoHelper
    {
        public static void CopyToEntity(PromoBanner promo, GetBannerRequest request)
        {
            promo.Category = request.category; // auth_token
        }
        public static void CopyFromEntity(GetBannerResponse response, PromoBanner promo)
        {
            response.banner_id = promo.BannerID;
            response.caption = promo.Caption;
            response.image_name = ImagePathService.bannersImagePath + promo.BannerImage;
            response.category = promo.Category;
            response.status_id = promo.StatusId ? 1 : 0;
        }
        public static void CopyToEntity(PromoBanner promo, GetBannerListRequest request)
        {
            // auth_token, page_number, records_per_page
        }
        public static void CopyFromEntity(BannerDto response, PromoBanner promo)
        {
            response.banner_id = promo.BannerID;
            response.caption = promo.Caption;
            response.category = promo.Category;
            response.status_id = promo.StatusId ? 1 : 0;
            response.image_name = ImagePathService.bannersImagePath + promo.BannerImage;
            response.created_by = promo.CreatedBy;
            response.created_date = promo.CreatedDate;
            response.updated_by = promo.UpdatedBy;
            response.updated_date = promo.UpdatedDate;
        }
        public static void CopyToEntity(PromoInfo promo, GetInfoBannerRequest request)
        {
            // auth_token
        }
        public static void CopyFromEntity(InfoBannerDto response, PromoInfo promo)
        {
            response.info_id = promo.InfoID;
            response.caption = promo.Caption;
            response.status_id = promo.StatusID;
            response.image_name = ImagePathService.promoInfoImagePath + promo.InfoImage;
            response.position = promo.Position;
        }
        public static void CopyToEntity(PromoInfo promo, GetInfoBannerListRequest request)
        {
            // auth_token, page_number, records_per_page
        }
        public static void CopyFromEntity(InfoBannerListDto response, PromoInfo promo)
        {
            response.info_id = promo.InfoID;
            response.caption = promo.Caption;
            response.status_id = promo.StatusID;
            response.image_name = ImagePathService.promoInfoImagePath + promo.InfoImage;
            response.position = promo.Position;
            response.created_by = promo.CreatedBy;
            response.created_date = promo.CreatedDate;
            response.updated_by = promo.UpdatedBy;
            response.updated_date = promo.UpdatedDate;
        }
    }
}