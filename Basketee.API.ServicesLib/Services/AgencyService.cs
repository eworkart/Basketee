using Basketee.API.DAOs;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.Services
{
    public class AgencyService
    {
        const double KM_PER_LATITUDE = 111.0, KM_PER_LONGITUDE = 111.0;

        public static List<Agency> GetProximateAgencies(string latitude, string longitude)
        {
            double lat = Convert.ToDouble(latitude), lon = Convert.ToDouble(longitude);
            double lowLat = lat - TimeslotService.LATITUDE_FOR_5KM, upLat = lat + TimeslotService.LATITUDE_FOR_5KM,
                loLon = lon - TimeslotService.LONGITUDE_FOR_5KM, upLon = lon + TimeslotService.LONGITUDE_FOR_5KM;
            using (AgencyDao dao = new AgencyDao())
            {
                return dao.GetAgenciesBetween(
                    Convert.ToString(lowLat),
                    Convert.ToString(upLat),
                    Convert.ToString(loLon),
                    Convert.ToString(upLon));
            }
        }

        public static List<DistributionPoint> GetProximateDistributionPoints(string latitude, string longitude)
        {
            double lat = Convert.ToDouble(latitude), lon = Convert.ToDouble(longitude);
            double lowLat = lat - TimeslotService.LATITUDE_FOR_5KM, upLat = lat + TimeslotService.LATITUDE_FOR_5KM,
                loLon = lon - TimeslotService.LONGITUDE_FOR_5KM, upLon = lon + TimeslotService.LONGITUDE_FOR_5KM;
            using (AgencyDao dao = new AgencyDao())
            {
                return dao.GetDistributionPointsBetween(
                    Convert.ToString(lowLat),
                    Convert.ToString(upLat),
                    Convert.ToString(loLon),
                    Convert.ToString(upLon));
            }
        }

        public static Agency FindPreferredAgency(Order order, string latitude, string longitude)
        {
            List<DistributionPoint> allDpss = GetProximateDistributionPoints(latitude, longitude);
            double conLat = Convert.ToDouble(latitude), conLon = Convert.ToDouble(longitude);
            Agency preferred = null;
            int score = 0;
            foreach (DistributionPoint dp in allDpss)
            {
                int dpScore = 0;
                double dlat = (double.Parse(dp.Latitude) - conLat) * KM_PER_LATITUDE, dlon = (double.Parse(dp.Longitude) - conLon) * KM_PER_LONGITUDE;
                double distance = Math.Sqrt(dlat * dlat + dlon * dlon);
                dpScore += ComputeDistanceScore(distance);

                int maxDelivery = 0;
                foreach(Driver drv in dp.Drivers)
                {
                    int deliCount = drv.OrderDeliveries.Where(od => od.DeliveryDate == order.DeliveryDate && od.Order.DeliverySlotID == order.DeliverySlotID).Count();
                    if(deliCount > maxDelivery)
                    {
                        maxDelivery = deliCount;
                    }
                }
                dpScore += ComputeDeliveryScore(TimeslotService.MAX_DELIVERY_PER_SLOT - maxDelivery);

                if(dpScore > score)
                {
                    score = dpScore;
                    preferred = dp.Agency;
                }
            }

            return preferred;
        }

        private static int ComputeDeliveryScore(int deliverySlack)
        {
            if (deliverySlack >= 9)
            {
                return 5;
            }
            if (deliverySlack >= 7)
            {
                return 4;
            }
            if (deliverySlack >= 5)
            {
                return 3;
            }
            if (deliverySlack >= 3)
            {
                return 2;
            }
            return 1;
        }

        private static int ComputeDistanceScore(double distance)
        {
            if (distance < 1.0)
            {
                return 5;
            }
            if (distance < 2.0)
            {
                return 4;
            }
            if (distance < 3.0)
            {
                return 3;
            }
            if (distance < 4.0)
            {
                return 2;
            }
            return 1;
        }
    }
}