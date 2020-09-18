using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DAOs
{
    public class AgencyDao : DAO
    {
        public List<DistributionPoint> GetDistributionPointsBetween(string lowerLatitude, string upperLatitude, string lowerLongitude, string upperLongitude)
        {
            var dps = _context.DistributionPoints.Where(dp =>
                (dp.Latitude.CompareTo(lowerLatitude) > 0) &&
                (dp.Latitude.CompareTo(upperLatitude) < 0) &&
                (dp.Longitude.CompareTo(lowerLongitude) > 0) &&
                (dp.Longitude.CompareTo(upperLongitude) < 0)
                );


            return dps.ToList();
        }

        public List<Agency> GetAgenciesBetween(string lowerLatitude, string upperLatitude, string lowerLongitude, string upperLongitude)
        {
            var dps = GetDistributionPointsBetween(lowerLatitude, upperLatitude, lowerLongitude, upperLongitude)
                .Select(dp => dp.Agency).Distinct();

            return dps.ToList();
        }

        public List<DistributionAgency> GetDistributionAgencies(double cust_lat, double cust_lon)
        {
            List<DistributionAgency> agencies = _context.GetDistribution(cust_lat, cust_lon).Select(x => new DistributionAgency() { AgadmID = x.AgadmID, AgenID = x.AgenID, DbptID = x.DbptID, DPDistance = x.DPDistance.Value }).ToList();
            return agencies.ToList();
        }
        public List<Agency> GetAgencies()
        {
            return _context.Agencies.Where(p => p.StatusId == true).ToList();
             
        }

       
    }

    public class DistributionAgency
    {
        public int DbptID { get; set; }
        public int AgenID { get; set; }
        public int AgadmID { get; set; }
        public double DPDistance { get; set; }
    }
   
}