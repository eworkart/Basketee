using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class DriverDao : DAO
    {
        public Driver FindById(int driverId)
        {
            return _context.Drivers.Find(driverId);
        }
        public Driver FindByMobileNumber(string mobileNumber)
        {
            var drvs = _context.Drivers.Where(c => c.MobileNumber.Replace("+968", "")  == mobileNumber.Replace("+968", ""));
            if (drvs.Count() > 0)
            {
                return drvs.Single();
            }

            return null;
        }

        public Driver FindByMobileNumberAndPassword(string mobileNumber, string passWord)
        {
            var drvs = _context.Drivers.Where(c =>c.StatusId && c.MobileNumber.Replace("+968", "") == mobileNumber.Replace("+968", "") && c.Password == passWord);
            if (drvs.Count() > 0)
            {
                return drvs.Single();
            }

            return null;
        }
        public void Update(Driver driver)
        {
            _context.Entry(driver).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public Reminder GetRemindersForDriver()
        {
            var reminders = _context.Reminders.Where(x => x.UserType && x.StatusId).OrderByDescending(x => x.UpdatedDate).ToList();
            if(reminders != null)
            {
                return reminders.FirstOrDefault();
            }
            return null;
        }

        public List<Driver> GetAllDriverNames()
        {
            return _context.Drivers.Where(p => p.StatusId == true).ToList();
        }

        public List<DriversForAdmin> GetAllDriversByAgentId(int agentAdminId)
        {
            List<DriversForAdmin> drivers = _context.GetAllDriversForAgentAdmin(agentAdminId).Select(x => new DriversForAdmin() { DriverName = x.DriverName,DrvrID = x.DrvrID }).ToList();
            return drivers;            
        }

        public List<Driver> GetDrivers()
        {
            return _context.Drivers.ToList();
        }
    }

    public class DriversForAdmin
    {
        public int DrvrID { get; set; }
        public string DriverName { get; set; }
    }
}
