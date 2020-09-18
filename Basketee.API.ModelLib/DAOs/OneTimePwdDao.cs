using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DAOs
{
    public class OneTimePwdDao : DAO
    {
        public OneTimePwd Insert(OneTimePwd otp)
        {
            _context.OneTimePwds.Add(otp);
            _context.SaveChanges();
            return otp;
        }

        public OneTimePwd FindByUserId(int userId)
        {
            var otps = _context.OneTimePwds.Where(o => o.UserID == userId).OrderByDescending(x=>x.CreatedDate);
            if (otps.Count() > 0)
            {
                return otps.FirstOrDefault();
            }
            return null;
        }

        public void DeleteOlderOTP(DateTime timeLimit)
        {
            _context.OneTimePwds.Where(otp => otp.CreatedDate < timeLimit).ToList().ForEach(otp => _context.OneTimePwds.Remove(otp));
            _context.SaveChanges();
        }

        public void Update(OneTimePwd otp)
        {
            _context.Entry(otp).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteOTP(OneTimePwd otp)
        {
            _context.OneTimePwds.Remove(otp);
            _context.SaveChanges();
        }
    }
}