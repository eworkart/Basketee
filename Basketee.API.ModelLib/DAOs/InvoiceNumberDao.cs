using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class InvoiceNumberDao : DAO
    {
        public InvoiceNumber FindByAgencyId(int agencyId)
        {
            var invs = _context.InvoiceNumbers.Where(nr => nr.AgenID == agencyId);
            if(invs.Count() > 0)
            {
                return invs.First();
            }
            return null;
        }

        public Agency FindAgencyById(int agencyId)
        {
            return _context.Agencies.Find(agencyId);
        }

        public void Insert(InvoiceNumber invNr)
        {
            _context.InvoiceNumbers.Add(invNr);
            _context.SaveChanges();
        }

        public void Update(InvoiceNumber invNr)
        {
            _context.Entry(invNr);
            _context.SaveChanges();
        }
    }
}
