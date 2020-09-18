using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class TeleOrderDao : DAO
    {
        public void Insert(TeleOrder ord)
        {
            _context.TeleOrders.Add(ord);
            _context.SaveChanges();
        }

        public ProductExchange FindProductExchangeById(int exchangeId)
        {
            return _context.ProductExchanges.Find(exchangeId);
        }

        public TeleOrder FindById(int teleOrdID, bool withDetails = false)
        {
            //TeleOrder to;
            //to.TeleOrderDetails
            if (withDetails)
            {
                return _context.TeleOrders.Include("MOrderStatu").Include("TeleOrderDetails").Include("Driver").Single(to => to.TeleOrdID == teleOrdID);
            }
            return _context.TeleOrders.Single(o => o.TeleOrdID == teleOrdID);
        }

        public void Update(TeleOrder order)
        {
            _context.Entry(order).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }
        public void UpdateDelivery(TeleOrderDelivery odel)
        {
            _context.Entry(odel).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public List<TeleOrder> GetDriverOrderList(int userId, int currentList)
        {
            List<int> statusArray = new List<int>();
            if (currentList == 1)
                statusArray.AddRange(new List<int>() { 2, 3 });
            else
                statusArray.Add(4);

            var ords = _context.TeleOrders.SelectMany(o => o.TeleOrderDeliveries.Where(od => od.DrvrID == userId && statusArray.Contains(od.TeleOrder.StatusId))).Select(od => od.TeleOrder).Distinct().ToList();
            ords = ords.Where(x => DateTime.Compare(x.DeliveryDate.Value.Date, DateTime.Today) == 0).ToList();
            //Driver drv = _context.Drivers.Find(userId);
            //int stat = currentList == 1 ? 2 : 4;
            //var ords = _context.TeleOrders.Include("TeleCustomers").Where(to => to.DrvrID == userId && to.StatusId == stat);
            return ords.ToList();
        }

        public int GetAssignedOrderCount(int userId, int status)
        {
            List<int> statusArray = new List<int>();
                statusArray.AddRange(new List<int>() { 2, 3 });
            var ords = _context.TeleOrders.Where(x=>x.DeliveryType).SelectMany(o => o.TeleOrderDeliveries.Where(od => od.DrvrID == userId && statusArray.Contains(od.TeleOrder.StatusId))).Select(od => od.TeleOrder).Distinct().ToList();
            var count = ords.Where(x => DateTime.Compare(x.DeliveryDate.Value.Date, DateTime.Today) == 0).Count();
            //Driver drv = _context.Drivers.Find(userId);
            //int count = drv.TeleOrders.Where(od => od.StatusId == status).Count();
            //int count = drv.TeleOrders.Where(od => od.StatusId == status && od.DrvrID == userId).Count();
            return count;
        }

        public TeleOrder GetAgentAdminOrder(int agentAdminId, int orderId)
        {
            var ordrs = _context.TeleOrders.Where(o => o.AgadmID == agentAdminId && o.TeleOrdID == orderId);
            if (ordrs.Count() > 0)
            {
                return ordrs.First();
            }
            return null;
        }

        public TeleOrder GetAgentBossOrder(int agentBossId, int orderId)
        {
            var ordrs = (from ord in _context.TeleOrders
                         join admin in _context.AgentAdmins on ord.AgadmID equals admin.AgadmID
                         join boss in _context.AgentBosses on admin.AgenID equals boss.AgenID
                         where ord.TeleOrdID == orderId && boss.AbosID == agentBossId
                         select ord).ToList();
            if (ordrs.Count() > 0)
            {
                return ordrs.FirstOrDefault();
            }
            return null;
        }

        public TeleOrder GetDriverOrder(int driverId, int orderId)
        {
            var ordrs = _context.TeleOrders.Where(o => o.DrvrID == driverId && o.TeleOrdID == orderId);
            if (ordrs.Count() > 0)
            {
                return ordrs.First();
            }
            return null;
        }

        public List<TeleOrder> GetOutForDeliveryTeleOrders(int driverId)
        {
            return _context.TeleOrders.Where(o => o.DrvrID == driverId && o.StatusId == 3).ToList();
        }
    }
}
