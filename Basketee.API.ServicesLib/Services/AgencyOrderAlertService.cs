using Basketee.API.DAOs;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Basketee.API.Services
{
    public class AgencyOrderAlertService
    {
        public static void NotifyAgencies(int ordrID)
        {
            using (OrderDao dao = new OrderDao())
            {
                Order ord = dao.FindById(ordrID);
                int userId = ord.ConsID;
                string latitude = ord.ConsumerAddress.Latitude;
                string longitude = ord.ConsumerAddress.Longitude;
                List<Agency> agencies = AgencyService.GetProximateAgencies(latitude, longitude);
                string appId = ord.Consumer.AppID, appToken = ord.Consumer.AppToken;
                foreach (Agency ag in agencies)
                {
                    if (!TimeslotService.CheckTimeslotFree(ord.DeliveryDate, latitude, longitude, ord.DeliverySlotID, ag.AgenID))
                    {
                        continue;
                    }
                    string message = string.Format(OrdersServices. ORDER_NOTIFICATION_TEMPLATE, ord.OrdrID);
                    PushMessagingService.PushNotification(appId, appToken, message);
                }
                HostingEnvironment.QueueBackgroundWorkItem(t => PostProcessAgencyNotification(ord.OrdrID));
            }
        }

        private static void PostProcessAgencyNotification(int ordrID, bool firstCall = true)
        {
            Thread.Sleep(OrdersServices. FOLLOWUP_DELAY);

            using (OrderDao dao = new OrderDao())
            {
                Order order = dao.FindById(ordrID, true);
                if (order.StatusID != OrdersServices.ID_ORDER_ACCEPTED)
                {
                    if (firstCall)
                    {
                        AllocateOrderToPrefferedAgent(order.OrdrID);
                        return;
                    }
                    order.StatusID = OrdersServices.ID_ORDER_SYS_CANCEL;
                    dao.Update(order);
                }
            }
        }

        private static void AllocateOrderToPrefferedAgent(int orderId)
        {
            using (OrderDao dao = new OrderDao())
            {
                Order order = dao.FindById(orderId);
                string latitude = order.ConsumerAddress.Latitude;
                string longitude = order.ConsumerAddress.Longitude;
                Agency preferredAgency = AgencyService.FindPreferredAgency(order, latitude, longitude);
                string appId = order.Consumer.AppID, appToken = order.Consumer.AppToken;
                string message = string.Format(OrdersServices.ORDER_ALLOCATION_TEMPLATE, order.OrdrID);
                PushMessagingService.PushNotification(appId, appToken, message);
                HostingEnvironment.QueueBackgroundWorkItem(t => PostProcessAgencyNotification(order.OrdrID, false));
            }
        }
    }
}
