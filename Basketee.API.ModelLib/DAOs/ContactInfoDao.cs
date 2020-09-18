using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DAOs
{
    public class ContactInfoDao : DAO
    {
        public ContactInfo GetDefaultContact()
        {
            return _context.ContactInfoes.Where(x => x.StatusId).OrderByDescending(x=>x.CreatedDate).FirstOrDefault();
            //return _context.ContactInfoes.Where(x => x.StatusId && x.IsDefault).FirstOrDefault();
        }
    }
}
