using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class TeleOrderCustomerDao : DAO
    {
        public void Insert(TeleCustomer cons)
        {
            _context.TeleCustomers.Add(cons);
            _context.SaveChanges();
        }

        public TeleCustomer FindByTeleOrderId(int teleOrderId)
        {
            return _context.TeleCustomers.Where(x=>x.TeleOrdID == teleOrderId).FirstOrDefault();
        }
    }
}
