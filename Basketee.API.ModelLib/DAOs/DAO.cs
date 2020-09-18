using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DAOs
{
    public class DAO : IDisposable
    {
        protected PertaminaLpgDbEntities _context = new PertaminaLpgDbEntities();

        public void Dispose()
        {
            _context.Dispose();
        }

        //public static string GetStandardMobileNumber(string mobileNumber)
        //{
        //    if (mobileNumber.StartsWith("+62"))
        //        return mobileNumber;
        //    else
        //        return mobileNumber.Insert(0, "+62");
        //}
    }
}