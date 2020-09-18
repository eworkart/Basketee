using Basketee.API.Models;
using System.Linq;

namespace Basketee.API.DAOs
{
    public class UserDao : DAO
    {
        public Consumer Insert(Consumer consumer)
        {
            _context.Consumers.Add(consumer);
            _context.SaveChanges();
            return consumer;
        }

        /// <summary>
        /// Retrieves a consumer record and returns the entity instance, if available.
        /// </summary>
        /// <param name="id">Promary key id of the consumer.</param>
        /// <param name="withDetails">Whether to include all child data like addresses etc.</param>
        /// <returns>Instance of Consumer with data or null if record not found.</returns>
        public Consumer FindById(int id, bool withDetails = false)
        {
            if (withDetails)
            {
                var consumers = _context.Consumers.Include("ConsumerAddresses").Where(c => c.ConsID == id && c.StatusID == 1);
                if (consumers.Count() > 0)
                {
                    return consumers.Single();
                }
                return null;
            }
            return _context.Consumers.Find(id);
        }

        public bool CheckPhoneExists(string phoneNumber)
        {
            bool exists = _context.Consumers.Any(c => c.PhoneNumber.Replace("+968", "") == phoneNumber.Replace("+968", "") && c.StatusID == 1);
            return exists;
        }

        public void Update(Consumer consumer)
        {
            _context.Entry(consumer).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public Consumer FindByMobileNumber(string mobileNumber)
        {
            var consumers = _context.Consumers.Where(c => c.PhoneNumber.Replace("+968", "") == mobileNumber.Replace("+968", "") && c.StatusID == 1);
            if (consumers.Count() > 0)
            {
                return consumers.Single();
            }
            
            return null;
        }

        public void UpdateAddress(ConsumerAddress address)
        {
           // _context.
            _context.Entry(address).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public void AddAddress(ConsumerAddress address)
        {
            _context.ConsumerAddresses.Add(address);
            _context.SaveChanges();
        }

        public bool DeleteAddress(int addressId,out bool isDefault)
        {
            ConsumerAddress addr = _context.ConsumerAddresses.Find(addressId);            
            if (addr != null)
            {
                isDefault = addr.IsDefault;
                if (!isDefault)
                {
                    addr.StatusID = 0;
                    _context.Entry(addr).State = System.Data.EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
            }
            isDefault = false;
            return false;
        }

        public ConsumerAddress FindAddressById(int addressId)
        {
            return _context.ConsumerAddresses.Where(x=>x.AddrID == addressId && x.StatusID == 1).FirstOrDefault();
        }

        public ConsumerAddress FindDefaultAddressFor(int userId)
        {
            return _context.ConsumerAddresses.Where(a => a.ConsID == userId && a.IsDefault && a.StatusID == 1).First();
        }

        public ConsumerAddress FindDefaultAddressForUser(int addressID)
        {
            return _context.ConsumerAddresses.Where(a => a.AddrID  == addressID && a.IsDefault && a.StatusID == 1).FirstOrDefault();
        }
    }
}