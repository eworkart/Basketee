using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DAOs
{
    public class DeliverySlotDao : DAO
    {
        public List<MDeliverySlot> GetAllSlots()
        {
            return _context.MDeliverySlots.ToList();
        }

        //public List<MDeliverySlot> GetAllSlots()
        //{
        //    TimeSpan elapseTime = TimeSpan.FromMinutes(30);
        //    TimeSpan currentTime = DateTime.Now.TimeOfDay.Add(elapseTime);
        //    return _context.MDeliverySlots.Where(x => x.EndTine >= currentTime).ToList();
        //}

        public int CheckAvailabilityForDriver(DateTime dt, int timeslotId, int driverId)
        {
            int orderCount = _context.Orders.Where(x => x.DrvrID == driverId && x.DeliveryDate == dt && x.DeliverySlotID == timeslotId && (x.StatusID == 2 || x.StatusID == 3)).Count();
            int teleOrderCount = _context.TeleOrders.Where(x => x.DeliveryDate == dt && (x.StatusId == 2 || x.StatusId == 3) && x.DeliverySlotID == timeslotId && x.DrvrID == driverId).Count();
            return (orderCount + teleOrderCount);
        }

        public int CheckAvailability(DateTime dt, int timeslotId, int maxDeliveryPerDriver)
        {
            var drvrDeliveries = _context.Drivers.SelectMany(d => d.OrderDeliveries.Where(od => od.DeliveryDate == dt && od.Order.DeliverySlotID == timeslotId));
            var dcs = drvrDeliveries.GroupBy(dd => new { dd.DrvrID }).Select(gr => new { count = gr.Count() }).ToList();
            if (dcs.Count > 0 && dcs.Any(c => c.count > maxDeliveryPerDriver))
            {
                return 0;
            }
            return 1;
        }
    }
}
