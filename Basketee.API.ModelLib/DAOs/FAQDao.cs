using Basketee.API.Models;
using System.Linq;

namespace Basketee.API.DAOs
{
    public class FAQDao : DAO
    {
        public MFaq[] GetFaqs(int pageNumber = 0, int rowsPerPage = 9)
        {
            //return _context.MFaqs.OrderBy(f => f.Position).ToArray();
            return _context.MFaqs.Where(x=>x.StatusID).OrderBy(p => p.Position).Skip(pageNumber * rowsPerPage).Take(rowsPerPage).ToArray();
        }
    }
}