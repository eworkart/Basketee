using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class ProductDao : DAO
    {
        public List<Product> GetProducts(int pageNumber, int rowsPerPage)
        {
            //page number starts with 0, as requested by mobile UI team
            return _context.Products.Where(x=>x.StatusId && x.Published).OrderBy(p => p.Position).Skip(pageNumber * rowsPerPage).Take(rowsPerPage).ToList();
        }

        public int GetTotalCount()
        {
            return _context.Products.Where(x => x.StatusId && x.Published).Count();
        }

        public Product FindProductById(int productId)
        {
            return _context.Products.Include("ProductExchanges").Where(x => x.StatusId && x.Published).FirstOrDefault(p => p.ProdID == productId);
        }

        public Reminder GetRemindersForProducts()
        {
            var reminders = _context.Reminders.Where(x => !x.UserType && x.StatusId).OrderByDescending(x=>x.UpdatedDate).ToList();
            if (reminders != null)
            {
                return reminders.FirstOrDefault();
            }
            return null;
        }
    }
}