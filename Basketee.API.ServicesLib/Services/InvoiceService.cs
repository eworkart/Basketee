using Basketee.API.DAOs;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Services
{
    public class InvoiceService
    {
        public static readonly object monitor = new object();

        public static string GenerateInvoiceNumber(int agencyId)
        {
            using (InvoiceNumberDao dao = new InvoiceNumberDao())
            {
                Agency agency = dao.FindAgencyById(agencyId);
                int nr = 1;
                InvoiceNumber invNr = dao.FindByAgencyId(agencyId);
                if (invNr == null)
                {
                    invNr = new InvoiceNumber();
                    invNr.AgenID = agencyId;
                    invNr.InvoiceNumber1 = 1;
                    dao.Insert(invNr);
                }
                else
                {
                    nr = invNr.InvoiceNumber1 + 1;
                    invNr.InvoiceNumber1 += 1;
                    dao.Update(invNr);
                }
                string invNumber = string.Format("{0}/{1}/{2:0000}/{3}", agency.MRegion.RegionCode, agency.SoldToNumber, nr, DateTime.Now.ToString("ddMMyyyy", CultureInfo.InvariantCulture));
                return invNumber;
            }
        }
    }
}
