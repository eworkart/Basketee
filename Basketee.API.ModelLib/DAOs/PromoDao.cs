using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class PromoDao : DAO
    {
        public PromoBanner FindByCategoty(int category)
        {
            var banners = _context.PromoBanners.Where(b => b.Category == category && b.StatusId);
            return banners.Count() > 0 ? banners.First() : null;
        }

        public List<PromoInfo> GetInfoBanners()
        {
            return _context.PromoInfoes.Where(x => x.StatusID == 1).ToList();
        }

        public List<PromoBanner> GetBannerList(int pageNumber, int rowsPerPage)
        {
            return _context.PromoBanners.Where(x=>x.StatusId).OrderBy(b => b.BannerID).Skip(pageNumber * rowsPerPage).Take(rowsPerPage).ToList();
        }

        public List<PromoInfo> GetInfoBannerList(int pageNumber, int recordsPerPage)
        {
            return _context.PromoInfoes.Where(x => x.StatusID == 1).OrderBy(b => b.Position).Skip(pageNumber * recordsPerPage).Take(recordsPerPage).ToList();
        }
    }
}