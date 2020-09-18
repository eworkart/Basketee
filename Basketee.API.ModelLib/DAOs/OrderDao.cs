using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.Models;

namespace Basketee.API.DAOs
{
    public class OrderDao : DAO
    {
        public int GetOrderCountFor(int user_id)
        {
            //return _context.Orders.Where(o => o.ConsID == user_id && o.StatusID == statusCode).Count();
            return _context.Orders.Where(o => o.ConsID == user_id && (o.StatusID == 1 || o.StatusID == 2 || o.StatusID == 3)).Count();
        }

        public List<Order> GetOrderList(int userId, int currentList)
        {
            if (currentList == 1)
                return _context.Orders.Where(o => o.ConsID == userId && (o.StatusID == 1 || o.StatusID == 2 || o.StatusID == 3)).ToList();
            if (currentList == 0)
                return _context.Orders.Where(o => o.ConsID == userId && o.StatusID == 4).ToList();
            return null;
        }

        public Order FindById(int orderId, bool withDetails = false)
        {
            if (withDetails)
            {
                return _context.Orders.Include("OrderDetails").Include("MOrderStatu").FirstOrDefault(o => o.OrdrID == orderId);
            }
            return _context.Orders.Where(o => o.OrdrID == orderId).FirstOrDefault();
        }

        public TeleOrder FindTeleOrderById(int teleOrderId)
        {
            return _context.TeleOrders.Where(o => o.TeleOrdID == teleOrderId).FirstOrDefault();
        }

        public TeleOrder FindTeleOrderForDriver(int teleOrderId, int driverId)
        {
            return _context.TeleOrders.Where(o => o.TeleOrdID == teleOrderId && o.DrvrID == driverId).FirstOrDefault();
        }

        public Order FindOrderForDriver(int orderId, int driverId)
        {
            return _context.Orders.Where(o => o.OrdrID == orderId && o.DrvrID == driverId).FirstOrDefault();
        }

        public void Update(Order ord)
        {
            _context.Entry(ord).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Insert(Order ord)
        {
            _context.Orders.Add(ord);
            _context.SaveChanges();
        }

        public void Insert(Order ord, List<OrderAllocationLog> ordAlloLog)
        {
            foreach (var item in ordAlloLog)
                ord.OrderAllocationLogs.Add(item);
            _context.Orders.Add(ord);
            _context.SaveChanges();
        }

        public int GetAgentOrderCount(int agentAdminId, int status)
        {
            //return _context.Orders.Where(o => o.AgadmID == agentAdminId && o.StatusID == status).Count();
            var cnt = (from ord in _context.Orders
                       join ordAllo in _context.OrderAllocationLogs on ord.OrdrID equals ordAllo.OrdrID
                       where ord.StatusID == status && ordAllo.AgadmID == agentAdminId && ordAllo.AssignmentType == 1
                       select ord).ToList().Count();
            return cnt;
        }

        public List<Order> GetAgentOrders(int agentAdminId, int status)
        {
            //return _context.Orders.Where(o => o.AgadmID == agentAdminId && o.StatusID == status).ToList();
            var orders = (from ord in _context.Orders
                          join ordAllo in _context.OrderAllocationLogs on ord.OrdrID equals ordAllo.OrdrID
                          where ord.StatusID == status && ordAllo.AgadmID == agentAdminId && ordAllo.AssignmentType == 1
                          select ord).ToList();
            return orders;
        }

        public Order GetAgentAdminOrder(int agentAdminId, int orderId)
        {
            var ordrs = _context.Orders.Where(o => o.AgadmID == agentAdminId && o.OrdrID == orderId);
            if (ordrs.Count() > 0)
            {
                return ordrs.First();
            }
            return null;
        }

        public Order GetAgentBossOrder(int agentBossId, int orderId)
        {
            var ordrs = (from ord in _context.Orders
                         join admin in _context.AgentAdmins on ord.AgadmID equals admin.AgadmID
                         join boss in _context.AgentBosses on admin.AgenID equals boss.AgenID
                         where ord.OrdrID == orderId && boss.AbosID == agentBossId select ord).ToList();
            if (ordrs.Count() > 0)
            {
                return ordrs.FirstOrDefault();
            }
            return null;
        }

        public TeleOrder GetAgentTeleOrder(int agentAdminId, int orderId)
        {
            var ordrs = _context.TeleOrders.Where(o => o.AgadmID == agentAdminId && o.TeleOrdID == orderId);
            if (ordrs.Count() > 0)
            {
                return ordrs.First();
            }
            return null;
        }

        public Order GetConsumerOrder(int consumer_id, int orderId)
        {
            var ordrs = _context.Orders.Where(o => o.ConsID == consumer_id && o.OrdrID == orderId);
            if (ordrs.Count() > 0)
            {
                return ordrs.First();
            }
            return null;
        }

        public Order GetDriverOrder(int driver_id, int orderId)
        {
            var ordrs = _context.Orders.Where(o => o.DrvrID == driver_id && o.OrdrID == orderId);
            if (ordrs.Count() > 0)
            {
                return ordrs.First();
            }
            return null;
        }

        public List<Order> GetOrgOrderList(int agentAdminId, int current_list)
        {
            var ordrs = _context.Orders.Where(o => o.AgadmID == agentAdminId);
            if (current_list == 1)
            {
                return ordrs.Where(o => o.StatusID == 2 || o.StatusID == 3).ToList();
            }
            return ordrs.Where(o => o.StatusID == 4).ToList();
        }

        public ProductExchange FindProductExchangeById(int exchangeId)
        {
            return _context.ProductExchanges.Find(exchangeId);
        }

        public int GetAssignedOrderCount(int userId, int status)
        {
            List<int> statusArray = new List<int>();
            statusArray.AddRange(new List<int>() { 2, 3 });

            var data = _context.Orders.SelectMany(o => o.OrderDeliveries.Where(od => od.DrvrID == userId && statusArray.Contains(od.Order.StatusID))).Select(od => od.Order).Distinct().ToList();
            var count = data.Where(x => DateTime.Compare(x.DeliveryDate.Date, DateTime.Today) == 0).Count();
            //Driver drv = _context.Drivers.Find(userId);
            //int count = drv.OrderDeliveries.Where(od => od.Order.StatusID == status).Count();
            //int count = drv.OrderDeliveries.Where(od => od.Order.StatusID == status && od.Order.DrvrID == userId ).Count();
            return count;
        }
        public List<Order> GetDriverOrderList(int userId, int currentList)
        {
            List<int> statusArray = new List<int>();
            if (currentList == 1)
                statusArray.AddRange(new List<int>() { 2, 3 });
            else
                statusArray.Add(4);

            var ords = _context.Orders.SelectMany(o => o.OrderDeliveries.Where(od => od.DrvrID == userId && statusArray.Contains(od.Order.StatusID))).Select(od => od.Order).Distinct().ToList();
            ords = ords.Where(x => DateTime.Compare(x.DeliveryDate.Date, DateTime.Today) == 0).ToList();
            //int stat = currentList == 1 ? 2 : 4;
            //var ords = _context.Orders.SelectMany(o => o.OrderDeliveries.Where(od => od.DrvrID == userId && od.Order.StatusID == stat)).Select(od => od.Order).Distinct();
            return ords.ToList();
        }


        public List<SellerReport> ABossSellerRpt(int userId, int currentList)
        {
            var sellerRpt = new List<SellerReport>();
            //var rpt = _context.Orders
            //           .Join(_context.OrderDetails, o => o.OrdrID, od => od.OrdrID, (o, od) => new { o, od })
            //           .Join(_context.Products, prod => prod.od.ProdID, c => c.ProdID, (prod, c) => new { prod, c })

            //            .Select(m => new {
            //                        //ProdId = m.ppc.p.Id, // or m.ppc.pc.ProdId
            //                        //CatId = m.c.CatId
            //                             });

            var rptOrderData = from order in _context.Orders
                               join order_details in _context.OrderDetails
                               on order.OrdrID equals order_details.OrdrID
                               join product in _context.Products
                               on order_details.ProdID equals product.ProdID
                               where order.StatusID == 4
                               select new SellerReportList
                               {
                                   product_id = order_details.ProdID,
                                   delivery_date = order.DeliveryDate,
                                   total_amount = order_details.TotamAmount
                               };

            sellerRpt = rptOrderData
                 .GroupBy(i => i.delivery_date.Month)
                 .Select(g => new SellerReport
                 {
                     key = "Month",
                     value = g.Sum(i => i.total_amount)
                 }).ToList();



            //var rptTeleOrderData = from teleorder in _context.TeleOrders
            //                   join tele_order_details in _context.TeleOrderDetails
            //                   on teleorder.TeleOrdID equals tele_order_details.TeleOrdID
            //                   join product in _context.Products
            //                   on tele_order_details.ProdID equals product.ProdID
            //                   where teleorder.StatusId == 4
            //                   select new SellerReport
            //                   {
            //                       key = "Month",
            //                       value = ""
            //                   };
            return sellerRpt;
        }
        public void InsertDelivery(OrderDelivery odel)
        {
            _context.OrderDeliveries.Add(odel);
            _context.SaveChanges();
        }
        public void UpdateDelivery(OrderDelivery odel)
        {
            _context.Entry(odel).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
        }
        public List<DriverOrder> GetDriverFromOrder(int agent_admin_id, int order_id)
        {
            List<DriverOrder> driver = _context.GetAvailabeDriversForTheOrder(agent_admin_id, order_id).Select(x => new DriverOrder() { agen_id = x.AgenID, dbpt_id = x.DbptID, dp_distance = x.DPDistance.Value, drvr_id = x.DrvrID, drvr_name = x.DriverName, tot_assignment = (x.tot_assignment.HasValue) ? x.tot_assignment.Value : 0 }).ToList();
            return driver.ToList();
        }

        /// <summary>
        ///  To get order lists for boss
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="status"></param>
        /// <returns>List<GetAllOrdersByAgentBoss_Result></returns>
        public List<GetAllOrdersByAgentBoss_Result> GetAgentBossOrders(int agentBossId, List<int> arrStatus = null)
        {
            List<GetAllOrdersByAgentBoss_Result> orders = _context.GetAllOrdersByAgentBoss(agentBossId).ToList();
            if (arrStatus != null && arrStatus.Count > 0)
                orders = orders.Where(o => arrStatus.Contains(o.StatusID)).ToList();
            return orders;
        }

        /// <summary>
        /// To get active orders count for boss
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="arrStatus"></param>
        /// <returns>int</returns>
        public int GetAgentBossOrdersCount(int agentBossId, List<int> arrStatus)
        {
            int ordersCount = GetAgentBossOrders(agentBossId, arrStatus).Count;
            return ordersCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="driverId"></param>
        /// <param name="periodType"></param>
        /// <param name="periodRange"></param>
        /// <returns>List<GetReviewReportByAgentBoss_Result> </returns>
        public List<GetReviewReportByAgentBoss_Result> GetReviewReportByAgentBoss(int agentBossId, int driverId, int periodType, int periodRange)
        {
            List<GetReviewReportByAgentBoss_Result> reportDetails = _context.GetReviewReportByAgentBoss(agentBossId, driverId, periodType, periodRange).ToList();
            return reportDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="driverId"></param>
        /// <param name="periodType"></param>
        /// <param name="periodRange"></param>
        /// <returns>List<GetReviewReasonByAgentBoss_Result></returns>
        public List<GetReviewReasonByAgentBoss_Result> GetReviewReasonByAgentBoss(int agentBossId, int driverId, int periodType, int periodRange)
        {
            List<GetReviewReasonByAgentBoss_Result> reportDetails = _context.GetReviewReasonByAgentBoss(agentBossId, driverId, periodType, periodRange).ToList();
            return reportDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="totalType"></param>
        /// <param name="periodType"></param>
        /// <param name="periodRange"></param>
        /// <param name="productIds"></param>
        /// <returns>List<GetSellerReportByAgentBoss_Result></returns>
        public List<GetSellerReportByAgentBoss_Result> GetSellerReportByAgentBoss(int agentBossId, int totalType, int periodType, int periodRange, string productIds)
        {
            List<GetSellerReportByAgentBoss_Result> reportDetails = _context.GetSellerReportByAgentBoss(agentBossId, totalType, periodType, periodRange, productIds).ToList();
            return reportDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <returns>List<GetDriversByAgentBoss_Result></returns>
        public List<GetDriversByAgentBoss_Result> GetDriversByAgentBoss(int agentBossId)
        {
            List<GetDriversByAgentBoss_Result> reportDetails = _context.GetDriversByAgentBoss(agentBossId).ToList();
            return reportDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <returns>List<GetProductsByAgentBoss_Result></returns>
        public List<GetProductsByAgentBoss_Result> GetProductsByAgentBoss(int agentBossId)
        {
            List<GetProductsByAgentBoss_Result> reportDetails = _context.GetProductsByAgentBoss(agentBossId).ToList();
            return reportDetails;
        }

        /// <summary>
        ///  To get order lists for boss
        /// </summary>
        /// <param name="agentAdminId"></param>
        /// <param name="status"></param>
        /// <returns>List<GetAllOrdersByAgentBoss_Result></returns>
        public List<GetAllOrdersByAgentAdmin_Result> GetAllOrdersByAgentAdmin(int agentAdminId, List<int> arrStatus = null)
        {
            List<GetAllOrdersByAgentAdmin_Result> orders = _context.GetAllOrdersByAgentAdmin(agentAdminId).ToList();
            if (arrStatus != null && arrStatus.Count > 0)
                orders = orders.Where(o => arrStatus.Contains(o.StatusID)).ToList();
            return orders;
        }

        public class GetDriverAgent
        {
            public float cust_lat { get; set; }
            public float cust_lng { get; set; }
            public DateTime delivery_date { get; set; }
            public int delivery_slot { get; set; }
        }
        public class DriverOrder
        {
            public int agen_id { get; set; }
            public int dbpt_id { get; set; }
            public double dp_distance { get; set; }
            public int drvr_id { get; set; }
            public string drvr_name { get; set; }
            public int tot_assignment { get; set; }
        }

        public class SellerReport
        {
            public string key { get; set; }
            public Decimal value { get; set; }
        }
        public class SellerReportList
        {
            public int product_id { get; set; }
            public DateTime delivery_date { get; set; }
            public Decimal total_amount { get; set; }
        }


        public int GetIssueCount(short statusCode)
        {
            var data = _context.GetIssuesListCountForSUser().FirstOrDefault();
            int count = data.HasValue ? data.Value : 0;
            return count;
            //return _context.Orders.Where(o => o.StatusID == statusCode).Count();
        }
        public List<Order> GetIssuesList(short statusCode)
        {
            // return _context.Orders.Where(o =>  o.StatusID == statusCode).ToList();
            var IssuesList = (from ord in _context.Orders
                              join cons in _context.Consumers on ord.ConsID equals cons.ConsID
                              join addrs in _context.ConsumerAddresses on ord.AddrID equals addrs.AddrID
                              where ord.StatusID == statusCode
                              select ord).ToList();
            return IssuesList;
        }

        public Order GetOrderDetailsbyOrderId(int orderid)
        {
            // return _context.Orders.Where(o =>  o.StatusID == statusCode).ToList();
            var IssuesList = (from ord in _context.Orders
                              join cons in _context.Consumers on ord.ConsID equals cons.ConsID
                              join addrs in _context.ConsumerAddresses on ord.AddrID equals addrs.AddrID
                              where ord.OrdrID == orderid
                              select ord);
            return IssuesList.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="driverId"></param>
        /// <param name="periodType"></param>
        /// <param name="periodRange"></param>
        /// <returns>List<GetReviewReportByAgentBoss_Result> </returns>
        public List<GetReviewReportBySUser_Result> GetReviewReportBySUser(int userId, int agencyId, int periodType, int periodRange)
        {
            List<GetReviewReportBySUser_Result> reportDetails = _context.GetReviewReportBySUser(userId, agencyId, periodType, periodRange).ToList();
            return reportDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="agencyId"></param>
        /// <param name="periodType"></param>
        /// <param name="periodRange"></param>
        /// <returns>List<GetReviewReasonByAgentBoss_Result></returns>
        public List<GetReviewReasonBySUser_Result> GetReviewReasonBySUser(int userId, int agencyId, int periodType, int periodRange)
        {
            List<GetReviewReasonBySUser_Result> reviewreasonreportDetails = _context.GetReviewReasonBySUser(userId, agencyId, periodType, periodRange).ToList();
            return reviewreasonreportDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentBossId"></param>
        /// <param name="totalType"></param>
        /// <param name="periodType"></param>
        /// <param name="periodRange"></param>
        /// <param name="productIds"></param>
        /// <returns>List<GetSellerReportByAgentBoss_Result></returns>
        public List<GetSellerReportBySUser_Result> GetSellerReportBySUser(int userId, int totalType, int periodType, int periodRange, int numOfproducts, string productIds, int agencyId)
        {
            List<GetSellerReportBySUser_Result> sellerreportDetails = _context.GetSellerReportBySUser(userId, totalType, periodType, periodRange, productIds, numOfproducts, agencyId).ToList();
            return sellerreportDetails;
        }




        public Order GetOrderDetailsSuser(int orderid)
        {
            // return _context.Orders.Where(o =>  o.StatusID == statusCode).ToList();
            var OrderDetails = (from ord in _context.Orders
                                join cons in _context.Consumers on ord.ConsID equals cons.ConsID
                                join addrs in _context.ConsumerAddresses on ord.AddrID equals addrs.AddrID
                                where ord.OrdrID == orderid
                                select ord);
            return OrderDetails.First();
        }

        public List<OrderDetail> GetProductDetailsSuser(int orderid)
        {
            // return _context.Orders.Where(o =>  o.StatusID == statusCode).ToList();
            var ProductList = (from ord in _context.OrderDetails
                               join Prd in _context.Products on ord.ProdID equals Prd.ProdID
                               where ord.OrdrID == orderid
                               select ord).ToList();
            return ProductList;
        }

        public Order GetAgencyDetailsSuser(int orderid)
        {
            // return _context.Orders.Where(o =>  o.StatusID == statusCode).ToList();
            var AgencyDetails = (from ord in _context.Orders
                                 join agntAdmn in _context.AgentAdmins on ord.AgadmID equals agntAdmn.AgadmID
                                 join agncy in _context.Agencies on agntAdmn.AgenID equals agncy.AgenID
                                 where ord.OrdrID == orderid
                                 select ord);
            return AgencyDetails.First();
        }


        public GetOrderDeatilsbyOrderIdForSUser_Result GetOrderDeatilsbyOrderIdForSUser(int orderId)
        {
            GetOrderDeatilsbyOrderIdForSUser_Result OrderDetails = _context.GetOrderDeatilsbyOrderIdForSUser(orderId).FirstOrDefault();
            return OrderDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>

        /// <returns>List<GetProductDeatilsbyOrderIdForSUser_Result> </returns>
        public List<GetProductDeatilsbyOrderIdForSUser_Result> GetProductDeatilsbyOrderIdForSUser(int orderId)
        {
            List<GetProductDeatilsbyOrderIdForSUser_Result> ProductDetails = _context.GetProductDeatilsbyOrderIdForSUser(orderId).ToList();
            return ProductDetails;
        }

        public GetAgencyDetailsbyOrderIdForSUser_Result GetAgencyDetailsbyOrderIdForSUser(int orderId)
        {
            GetAgencyDetailsbyOrderIdForSUser_Result AgencyDetails = _context.GetAgencyDetailsbyOrderIdForSUser(orderId).FirstOrDefault();
            return AgencyDetails;
        }

        public List<GetIssuesListForSUser_Result> GetIssuesListForSUser()
        {
            List<GetIssuesListForSUser_Result> IssueDetails = _context.GetIssuesListForSUser().ToList();
            return IssueDetails;
        }
        public Order GetCheckStatusById(int order_Id)
        {
            return _context.Orders.Find(order_Id);
        }

        //public List<Order> GetAllOrderForConsumer(int userId, int currentList, int pageNumber, int recordsPerPage)

        public List<Order> GetAllOrderForConsumer(int userId, int currentList, int pageNumber, int recordsPerPage)
        {
            if (currentList == 1)
                return _context.Orders.Where(o => o.ConsID == userId && (o.StatusID == 1 || o.StatusID == 2 || o.StatusID == 3)).OrderBy(x => x.OrderDate).Skip((pageNumber - 1) * recordsPerPage).Take(recordsPerPage).ToList();
            if (currentList == 0)
                return _context.Orders.Where(o => o.ConsID == userId && o.StatusID == 4).ToList().OrderBy(x => x.OrderDate).Skip((pageNumber - 1) * recordsPerPage).Take(recordsPerPage).ToList(); ;
            return null;

        }

        public List<OrderPrdocuctExchange> GetProductExchangeSuser(int orderid)
        {

            return _context.OrderPrdocuctExchanges.Where(o => o.OrdrID == orderid).ToList();
        }

        public List<Order> GetOutForDeliveryOrders(int driverId)
        {
            return _context.Orders.Where(o => o.DrvrID == driverId && o.StatusID == 3).ToList();
        }
    }

    public class IssuesCountForSuperUser
    {
        public int IssuesCount { get; set; }
    }
}