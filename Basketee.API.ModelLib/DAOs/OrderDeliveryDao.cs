using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.DAOs
{
    public class OrderDeliveryDao : DAO
    {
        public IQueryable<OrderDelivery> GetDeliveriesFor(DateTime startDate, DateTime endDate, string lowerLatitude, string upperLatitude, string lowerLongitude, string upperLongitude)
        {
            var dpIds = _context.DistributionPoints.Where(dp =>
                (dp.Latitude.CompareTo(lowerLatitude) > 0) &&
                (dp.Latitude.CompareTo(upperLatitude) < 0) &&
                (dp.Longitude.CompareTo(lowerLongitude) > 0) &&
                (dp.Longitude.CompareTo(upperLongitude) < 0)
                ).Select(dp => dp.DbptID);

            var orderDeliveries = _context.Drivers.Where(dr => dpIds.Contains(dr.DbptID)).SelectMany(d => d.OrderDeliveries.Where(od => od.DeliveryDate >= startDate && od.DeliveryDate <= endDate));
            return orderDeliveries;
        }

        
    }
}