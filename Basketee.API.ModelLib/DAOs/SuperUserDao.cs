using System;
using System.Linq;
using Basketee.API.DAOs;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
   public class SuperUserDao : DAO
    {
        public SuperAdmin FindByMobileNumber(string mobileNumber)
        {
            var boss = _context.SuperAdmins.Where(a =>a.StatusID && a.MobileNum.Replace("+968", "")  == mobileNumber.Replace("+968", ""));
            if (boss.Count() > 0)
            {
                return boss.Single();
            }
            return null;
        }

        public void Update(SuperAdmin boss)
        {
            _context.Entry(boss).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public SuperAdmin FindById(int userId)
        {
            return _context.SuperAdmins.Find(userId);
        }
    }
}
