using System.Linq;
using System;
using System.Collections.Generic;
using Basketee.API.DTOs.Orders;
using Basketee.API.DTOs;
using Basketee.API.DAOs;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;
using System.Threading;
using System.Web.Hosting;
using System.Globalization;
using System.Text;
using System.IO;
using Microsoft.ApplicationInsights;

namespace Basketee.API.Services
{
    public class OrdersServices
    {
        #region Telemetry
        private TelemetryClient tm = new TelemetryClient();



        #endregion
        #region Const
        //TODO: place correct ids here
        public const short ID_ORDER_ACTIVE = 1,
             ID_ORDER_ACCEPTED = 2,
             ID_ORDER_OUT_FOR_DELIVERY = 3,
             ID_ORDER_CLOSED = 4,
             ID_ORDER_CANCELLED = 5,
             ID_ORDER_SYS_CANCEL = 6,
             ID_ORDER_CANCELLED_BY_SUSER = 7;
        public const short DELIVERY_STATUS_ASSIGNED = 1,
            DELIVERY_STATUS_OUTFORDELIVERY = 2,
            DELIVERY_STATUS_CANCELLED = 3,
            DELIVERY_STATUS_ONTIMEDELIVERY = 4,
            DELIVERY_STATUS_LATEDELIVERY = 5,
            DELIVERY_STATUS_EARLYDELIVERY = 6;
        public const string APPSETTING_MSG_TO_ASSIGNED_DRIVER = "MsgToAssignedDriver";
        public const string APPSETTING_TITLE_FOR_ASSIGNED_DRIVER = "TitleForAssignedDriver";
        public const string APPSETTING_MSG_TO_AGENT_ADMINS_IN_RANGE = "MsgToAgentAdminsInRange";
        public const string APPSETTING_TITLE_FOR_AGENT_ADMINS_IN_RANGE = "TitleForAgentAdminsInRange";
        public const string APPSETTING_MSG_TO_SUPER_ADMIN = "MsgToSuperAdmin";
        public const string APPSETTING_TITLE_FOR_SUPER_ADMIN = "TitleForSuperAdmin";
        public const string APPSETTING_MSG_TO_CONSUMER_FOR_RATING = "MsgToConsumerForRating";
        public const string APPSETTING_TITLE_FOR_CONSUMER_TO_RATE = "TitleForConsumerToRate";
        public const string APPSETTING_MSG_FOR_CONSUMER_AT_SADMIN_CANCEL = "MsgToConsumerAtSAdminCancel";
        public const string APPSETTING_TITLE_FOR_CONSUMER_AT_SADMIN_CANCEL = "TitleForConsumerAtSAdminCancel";
        public const string ORDER_NOTIFICATION_TEMPLATE = "An order has been posted in your teritory; order Id = {0}";
        public const string ORDER_ALLOCATION_TEMPLATE = "Kindly deliver the following order; order Id = {0}";
        public const int FOLLOWUP_DELAY = 1 * 60 * 1000;
        const double DAYS_TO_LIST = 3;
        public const int MAX_DELIVERY_PER_SLOT = 10;
        #endregion

        #region Enum
        public enum OrderType
        {
            OrderApp, OrderTelp
        }
        #endregion

        #region Private Variables
        private UserServices _userServices = new UserServices();
        //private OrdersServices _ordersservices = new OrdersServices();
        #endregion

        #region Public Methods       

        public GetActiveOrderCountResponse GetActiveOrderCount(GetActiveOrderCountRequest request)
        {

            GetActiveOrderCountResponse response = new GetActiveOrderCountResponse();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    int orderCount = dao.GetOrderCountFor(request.user_id);
                    response.active_orders = new ActiveOrdersDto { user_id = request.user_id, active_order_count = orderCount };
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = orderCount > 0 ?
                        MessagesSource.GetMessage("active.orders") :
                        MessagesSource.GetMessage("no.active.orders");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }

            return response;
        }


        public ResponseDto SubmitReview(SubmitReviewRequest request)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                ConsumerReview review = new ConsumerReview();
                review.Comments = request.comments;
                review.Rating = request.rating;
                review.ReasonID = review.Rating <= 3 ? request.reason_id : (int?)null;
                review.ConsID = request.user_id;
                review.DrvrID = request.driver_id;
                review.OrdrID = request.order_id;
                using (ConsumerReviewDao dao = new ConsumerReviewDao())
                {
                    dao.Insert(review);
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("review.posted");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public PlaceOrderResponse PlaceOrder(PlaceOrderRequest request)
        {
            PlaceOrderResponse response = new PlaceOrderResponse();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }


                if (request.delivery_date <= DateTime.MinValue)
                {
                    MakeInvalidDeliveryDateFormat(response);
                    return response;
                }

                using (OrderDao dao = new OrderDao())
                {
                    Order ord = new Order();

                    UpdatePlaceOrderRequest(request); // To update request from mobile, Reason : not passing expected values from mobile

                    PopulateOrder(ord, request.user_id, request.address_id, request.time_slot_id);

                    ord.DeliveryDate = request.delivery_date;

                    AddProductsToOrder(ord, request.products);

                    if (request.has_exchange)
                    {
                        if (request.exchange != null)
                        {
                            foreach (ExchangeDto exdto in request.exchange)
                            {
                                OrderPrdocuctExchange opex = new OrderPrdocuctExchange();
                                ProductExchange pxg = dao.FindProductExchangeById(exdto.exchange_id);
                                if (pxg == null)
                                {
                                    //MakeInvalidExchangeInputResponse(response);
                                    response.code = 1;
                                    response.has_resource = 0;
                                    response.message = "Cannot find exchange with id " + exdto.exchange_id;

                                    return response;
                                }
                                opex.ProdID = pxg.ProdID;
                                opex.ExchangePrice = exdto.exchange_price;
                                opex.CreatedDate = DateTime.Now;
                                opex.ExchangePromoPrice = exdto.exchange_promo_price * exdto.exchange_quantity;
                                opex.ExchangeQuantity = exdto.exchange_quantity;
                                opex.ExchangeWith = exdto.exchange_with;
                                opex.StatusId = true;//TODO
                                opex.SubTotal = exdto.exchange_price * exdto.exchange_quantity;
                                //ord.GrandTotal -= (opex.ExchangePrice - opex.ExchangePromoPrice) * opex.ExchangeQuantity;                              

                                var product = request.products.Where(p => p.product_id == pxg.ProdID).FirstOrDefault();
                                if (product == null)
                                {
                                    //MakeInvalidExchangeInputResponse(response);
                                    response.code = 1;
                                    response.has_resource = 0;
                                    response.message = "Cannot find product with id " + pxg.ProdID + " in the products array";
                                    return response;
                                }
                                opex.TotalAmount = opex.SubTotal + opex.ExchangePromoPrice + (product.shipping_cost * exdto.exchange_quantity) + (product.shipping_promo * exdto.exchange_quantity);
                                ord.OrderPrdocuctExchanges.Add(opex);
                                ord.ShippingCharge += (product.shipping_cost * exdto.exchange_quantity);
                                ord.PromoShipping += (product.shipping_promo * exdto.exchange_quantity);
                                ord.NumberOfProducts += exdto.exchange_quantity;
                                ord.ExchangeSubTotal += opex.SubTotal;
                                ord.PromoExchange += opex.ExchangePromoPrice;
                                ord.GrandTotal += opex.TotalAmount;
                            }
                        }
                        else
                        {
                            MakeInvalidExchangeInputResponse(response);
                            return response;
                        }
                    }
                    ord.StatusID = ID_ORDER_ACTIVE;// 1;
                    //dao.Insert(ord);
                    //AgencyOrderAlertService.NotifyAgencies(ord.OrdrID);

                    List<OrderAllocationLog> ordAllo = new List<OrderAllocationLog>();
                    using (AgencyDao agentDao = new AgencyDao())
                    {
                        //var dps1 = agentDao.GetDistributionPointsBetween(Convert.ToString(lowLat),
                        //     Convert.ToString(upLat),
                        //     Convert.ToString(loLon),
                        //     Convert.ToString(upLon));
                        using (UserDao usrdao = new UserDao())
                        {
                            var consumer = _userServices.GetAuthUser(usrdao, request.user_id, request.auth_token, true);
                            if (consumer == null)
                            {
                                _userServices.MakeNouserResponse(response);
                                return response;
                            }

                            foreach (var item in consumer.ConsumerAddresses)
                            {
                                if (item.AddrID == request.address_id)
                                {
                                    item.IsDefault = true;
                                }
                                else
                                    item.IsDefault = false;
                            }
                            usrdao.Update(consumer);
                            ConsumerAddress consAddr = consumer.ConsumerAddresses.Where(a => a.AddrID == request.address_id).First();
                            //consAddr.IsDefault = true;
                            var dps = agentDao.GetDistributionAgencies(Convert.ToDouble(consAddr.Latitude), Convert.ToDouble(consAddr.Longitude));
                            ordAllo = dps.Select(x => new OrderAllocationLog()
                            {
                                AgadmID = x.AgadmID,
                                DbptID = x.DbptID,
                                Distance = Convert.ToDecimal(x.DPDistance),
                                UpdatedBy = request.user_id,
                                CreatedBy = Convert.ToInt16(request.user_id),
                                TimeSlotAvailable = (TimeslotService.CheckTimeslotFree(ord.DeliveryDate, consAddr.Latitude, consAddr.Longitude, ord.DeliverySlotID, x.AgenID)), //true,
                                AllocationStatus = 0,
                                AssignmentType = 1,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                            }).ToList();
                            dao.Insert(ord, ordAllo);
                            ord = dao.FindById(ord.OrdrID, true);
                            //AgencyOrderAlertService.NotifyAgencies(ord.OrdrID);
                            using (AgentAdminDao agentAdminDao = new AgentAdminDao())
                            {
                                foreach (var item in ordAllo)
                                {
                                    AgentAdmin agentAdmin = agentAdminDao.FindById(item.AgadmID);
                                    int orderCount = dao.GetAgentOrderCount(agentAdmin.AgadmID, 1);
                                    ReadAndSendPushNotification(APPSETTING_MSG_TO_AGENT_ADMINS_IN_RANGE, APPSETTING_TITLE_FOR_AGENT_ADMINS_IN_RANGE, agentAdmin.AppToken, ord.OrdrID, 0, orderCount, PushMessagingService.APPSETTING_APPLICATION_ID_AADMIN, PushMessagingService.APPSETTING_SENDER_ID_AADMIN, (int)PushMessagingService.PushType.TypeOne);
                                }
                            }
                        }
                    }

                    response.code = 0;
                    response.has_resource = 1;
                    response.order_details = new PlaceOrderRespOrderDto();
                    response.order_details.invoice_number = ord.InvoiceNumber;
                    response.order_details.order_id = ord.OrdrID;
                    response.order_details.order_status = ord.MOrderStatu.OrderStatus;
                    response.message = MessagesSource.GetMessage("order.placed");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }

            return response;
        }

        public GetTimeSlotResponse GetTimeSlot(GetTimeslotRequest request)
        {
            GetTimeSlotResponse response = new GetTimeSlotResponse();
            response.has_resource = 1;
            response.message = MessagesSource.GetMessage("with.timeslots");
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                TimeSpan tTomm = new TimeSpan(1, 0, 0, 0);
                TimeSpan tDat = new TimeSpan(2, 0, 0, 0);
                //ConsumerAddress consumerAddr = _userServices.GetDefaultUserAddress(request.user_id);
                ConsumerAddress consumerAddr = _userServices.GetDefaultUserAddressForUser(request.user_address_id);
                if (consumerAddr == null)
                {
                    _userServices.MakeNoAddressResponse(response);
                    return response;
                }
                TimeslotDisplayDto[] timeSlots = TimeslotService.GetTimeslotsDisplay(DateTime.Now, consumerAddr.Latitude, consumerAddr.Longitude, -1, response);
                var timeslotList = timeSlots.ToList();
                response.days = timeSlots.ToList();
                response.code = 0;

            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }

            return response;
        }

        public GetOrderListResponse GetOrderList(GetOrderListRequest request)
        {
            GetOrderListResponse response = new GetOrderListResponse();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    List<Order> ordList = dao.GetOrderList(request.user_id, request.current_list);
                    if (ordList == null)
                    {
                        MakeNoOrderFoundResponse(response);
                        return response;
                    }
                    if (ordList.Count <= 0)
                    {
                        MakeNoOrderFoundResponse(response);
                        return response;
                    }
                    response.order_details = new OrderDto[ordList.Count];
                    for (int i = 0; i < ordList.Count; i++)
                    {
                        OrderDto odDto = new OrderDto();
                        OrdersHelper.CopyFromEntity(odDto, ordList[i]);
                        response.order_details[i] = odDto;
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("ordr.listed");
                }

            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }
            return response;
        }

        public GetOrderDetailsResponse GetOrderDetails(GetOrderDetailsRequest request)
        {
            GetOrderDetailsResponse response = new GetOrderDetailsResponse();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    Order ord = dao.FindById(request.order_id, true);
                    OrderDetailsDto oDto = new OrderDetailsDto();
                    string agentAdminMob = ord.AgentAdmin != null ? ord.AgentAdmin.MobileNumber : string.Empty;
                    OrdersHelper.CopyFromEntity(oDto, ord, agentAdminMob, true);
                    using (ConsumerReviewDao conReviewDao = new ConsumerReviewDao())
                    {
                        if (ord.DrvrID.HasValue)
                        {
                            List<ConsumerReview> conReview = new List<ConsumerReview>();
                            conReview = conReviewDao.GetReviewByDriver(ord.DrvrID.Value);
                            oDto.driver_details.driver_rating = conReview.Count > 0 ? Convert.ToDecimal(conReview.Average(x => x.Rating)) : 0;
                        }
                    }
                    response.order_details = oDto;
                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("ordr.details");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }

            return response;
        }

        public ResponseDto CancelOrder(CancelOrderRequest request)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {

                    Order ord = dao.FindById(request.order_id, false);
                    if (ord.StatusID >= 3)
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("ordr.cant.cancel");
                        return response;
                    }

                    ord.StatusID = ID_ORDER_CANCELLED;
                    dao.Update(ord);

                    var orderDeliveries = ord.OrderDeliveries.Where(x => x.OrdrID == ord.OrdrID);
                    if (orderDeliveries.Count() > 0)
                    {
                        OrderDelivery orderDelivery = orderDeliveries.FirstOrDefault();
                        orderDelivery.StatusId = DELIVERY_STATUS_CANCELLED;
                        dao.UpdateDelivery(orderDelivery);
                    }
                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("ordr.cancelled");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }

            return response;
        }


        ///////Sprint 2


        public GetUnassignedOrdersResponse GetUnassignedOrders(GetUnassignedOrdersRequest request)
        {
            GetUnassignedOrdersResponse response = new GetUnassignedOrdersResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    response.active_orders = new ActiveOrdersDto();
                    response.active_orders.active_order_count = dao.GetAgentOrderCount(request.user_id, 1);
                    response.active_orders.user_id = request.user_id;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("order.count");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }

            return response;
        }

        public GetAllUnassignedOrdersResponse GetAllUnassignedOrders(GetAllUnAssignedOrdersRequest request)
        {
            GetAllUnassignedOrdersResponse response = new GetAllUnassignedOrdersResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    List<Order> ordrs = dao.GetAgentOrders(request.user_id, 1);
                    response.active_orders = new ActiveOrdersDto();
                    response.active_orders.active_order_count = dao.GetAgentOrderCount(request.user_id, 1);
                    // response.active_orders.user_id = request.user_id;
                    List<OrderFullDetailsDto> ordrDtos = new List<OrderFullDetailsDto>();
                    foreach (Order order in ordrs)
                    {
                        OrderFullDetailsDto dto = new OrderFullDetailsDto();
                        OrdersHelper.CopyFromEntity(dto, order);
                        dto.driver = null;
                        ordrDtos.Add(dto);
                    }
                    response.orders = ordrDtos.ToArray();
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("ordr.listed");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }


            return response;
        }

        public GetDriverDetailsResponse GetDriverDetails(GetDriverDetailsRequest request)
        {
            GetDriverDetailsResponse response = new GetDriverDetailsResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    Order ordr = dao.FindById(request.order_id, true);
                    if (ordr != null)
                    {
                        if (ordr.OrderDeliveries.Count > 0)
                        {
                            OrderDelivery odel = ordr.OrderDeliveries.First();
                            if (odel != null)
                            {
                                Driver drv = odel.Driver;
                                DriverDetailsDto dto = new DriverDetailsDto();
                                OrdersHelper.CopyFromEntity(dto, drv);
                                response.driver_details = dto;
                                response.code = 0;
                                response.has_resource = 1;
                                response.message = MessagesSource.GetMessage("driver.details");
                                return response;
                            }
                        }
                    }
                }
                response.code = 1;
                response.has_resource = 0;
                response.message = MessagesSource.GetMessage("no.details");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                tm.TrackException(ex);
            }
            return response;
        }

        public ConfirmOrderResponse ConfirmOrder(ConfirmOrderRequest request)
        {
            ConfirmOrderResponse response = new ConfirmOrderResponse();
            try
            {
                AgentAdmin admin = AgentAdminServices.GetAuthAdmin(request.user_id, request.auth_token, response);
                if (admin == null)
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    Order ordr = dao.FindById(request.order_id);
                    if (ordr != null && ordr.StatusID == ID_ORDER_ACTIVE)
                    {
                        ordr.AgadmID = admin.AgadmID;
                        Driver drv = null;
                        using (DriverDao ddao = new DriverDao())
                        {
                            drv = ddao.FindById(request.driver_id);
                            OrderDelivery odel = new OrderDelivery();
                            odel.DrvrID = drv.DrvrID;
                            odel.AgadmID = request.user_id;
                            odel.CreatedDate = DateTime.Now;
                            odel.DeliveryDate = ordr.DeliveryDate;
                            odel.AcceptedDate = DateTime.Now;
                            odel.StatusId = DELIVERY_STATUS_ASSIGNED;//1;
                            odel.Order = ordr;
                            ordr.OrderDeliveries.Add(odel);
                        }
                        ordr.StatusID = ID_ORDER_ACCEPTED;//2;
                        int agId = drv.AgenID;
                        lock (InvoiceService.monitor)
                        {
                            string invNo = InvoiceService.GenerateInvoiceNumber(agId);
                            ordr.InvoiceNumber = invNo;
                            ordr.DrvrID = request.driver_id;
                            dao.Update(ordr);
                        }
                        ordr = dao.FindById(ordr.OrdrID, true);
                        if (ordr.DeliveryDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                        {
                            int orderCount = dao.GetAssignedOrderCount(request.driver_id, OrdersServices.ID_ORDER_ACCEPTED);
                            using (TeleOrderDao teledao = new TeleOrderDao())
                            {
                                orderCount += teledao.GetAssignedOrderCount(request.driver_id, OrdersServices.ID_ORDER_ACCEPTED);
                            }
                            ReadAndSendPushNotification(APPSETTING_MSG_TO_ASSIGNED_DRIVER, APPSETTING_TITLE_FOR_ASSIGNED_DRIVER, drv.AppToken, 0, 0, orderCount, PushMessagingService.APPSETTING_APPLICATION_ID_DRIVER, PushMessagingService.APPSETTING_SENDER_ID_DRIVER, (int)PushMessagingService.PushType.TypeOne);
                        }
                        OrderFullDetailsDto dto = new OrderFullDetailsDto();
                        OrdersHelper.CopyFromEntity(dto, ordr, drv);
                        using (ConsumerReviewDao conReviewDao = new ConsumerReviewDao())
                        {
                            List<ConsumerReview> conReview = new List<ConsumerReview>();
                            conReview = conReviewDao.GetReviewByDriver(request.driver_id);
                            dto.driver.driver_rating = conReview.Count > 0 ? Convert.ToDecimal(conReview.Average(x => x.Rating)) : 0;
                        }

                        response.order_details = dto;
                        response.code = 0;
                        response.has_resource = 1;
                        response.message = MessagesSource.GetMessage("ordr.details");
                        return response;
                    }
                    response.code = 1;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("no.details");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetOrgOrderListResponse GetOrgOrderList(GetOrgOrderListRequest request)
        {
            GetOrgOrderListResponse response = new GetOrgOrderListResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    var orders = dao.GetAllOrdersByAgentAdmin(request.user_id);
                    if (orders != null && orders.Count > 0)
                    {
                        #region ActiveOrders
                        List<int> activeStatus = new List<int>() { ID_ORDER_ACCEPTED, ID_ORDER_OUT_FOR_DELIVERY };

                        var activeOrders = orders.Where(a => activeStatus.Contains(a.StatusID)).Select
                            (
                                a => OrdersHelper.CopyFromEntity(new OrderDetailsBossDto(), a)
                            ).ToList();

                        #endregion ActiveOrders

                        #region HistoryOrders

                        List<int> historyStatus = new List<int>() { ID_ORDER_CLOSED };
                        var historyOrders = orders.Where(a => historyStatus.Contains(a.StatusID)).Select
                            (
                                a => OrdersHelper.CopyFromEntity(new OrderDetailsBossDto(), a)
                            ).ToList();

                        #endregion ActiveOrders

                        response.orders = new OrdersBossDto() { active_order_count = activeOrders.Count, history_order_count = historyOrders.Count };
                        response.active_orders = activeOrders;
                        response.history_orders = historyOrders;
                    }

                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("ordr.listed");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public GetOrgOrderDetailsResponse GetOrgOrderDetails(GetOrgOrderDetailsRequest request)
        {
            GetOrgOrderDetailsResponse response = new GetOrgOrderDetailsResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                OrderType orderType;
                try
                {
                    orderType = request.order_type.ToEnumValue<OrderType>();
                }
                catch
                {
                    MakeInvalidOrderTypeResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    //Order ord = new Order();
                    //TeleOrder teleOrder = new TeleOrder();
                    OrderFullDetailsDto dto = new OrderFullDetailsDto();
                    if (orderType == OrderType.OrderApp)
                    {
                        Order ord = dao.GetAgentAdminOrder(request.user_id, request.order_id);
                        if (ord == null)
                        {
                            MakeInvalidOrderResponse(response);
                            return response;
                        }
                        dto = new OrderFullDetailsDto();
                        OrdersHelper.CopyFromEntity(dto, ord);
                    }
                    else if (orderType == OrderType.OrderTelp)
                    {
                        TeleOrder teleOrder = dao.GetAgentTeleOrder(request.user_id, request.order_id);
                        if (teleOrder == null)
                        {
                            MakeInvalidTeleOrderResponse(response);
                            return response;
                        }
                        dto = new OrderFullDetailsDto();
                        OrdersHelper.CopyFromEntity(dto, teleOrder);
                    }

                    response.order_details = dto;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("ordr.details");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetInvoiceDetailsResponse GetInvoiceDetails(GetInvoiceDetailsRequest request)
        {
            GetInvoiceDetailsResponse response = new GetInvoiceDetailsResponse();
            try
            {
                int userType = 0;
                if (request.is_agent_admin == 1)
                {
                    userType = (int)UserType.AgentAdmin;
                    if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }
                else if (request.is_agent_admin == 0)
                {
                    userType = (int)UserType.AgentBoss;
                    if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }

                OrderType orderType;
                try
                {
                    orderType = request.order_type.ToEnumValue<OrderType>();
                }
                catch
                {
                    MakeInvalidOrderTypeResponse(response);
                    return response;
                }
                response.orders = GetOrderInvoiceOrReciept(request.user_id, orderType, request.order_id, userType);
                if (response.orders == null)
                {
                    MakeNoOrderFoundResponse(response);
                    return response;
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("invoice.details");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetEReceiptResponse GetERecieptDetails(GetEReceiptRequest request)
        {
            GetEReceiptResponse response = new GetEReceiptResponse();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                response.orders = GetOrderInvoiceOrReciept(request.user_id, OrderType.OrderApp, request.order_id, (int)UserType.Consumer);
                if (response.orders == null)
                {
                    MakeNoOrderFoundResponse(response);
                    return response;
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("ereceipt.details");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public OrderInvoiceDto GetOrderInvoiceOrReciept(int user_id, OrderType order_type, int order_id, int userType)
        {
            OrderInvoiceDto response = new OrderInvoiceDto();
            if (order_type == OrderType.OrderApp)
            {
                using (OrderDao dao = new OrderDao())
                {
                    Order ord = null;
                    switch (userType)
                    {
                        case (int)UserType.AgentBoss:
                            ord = dao.GetAgentBossOrder(user_id, order_id);
                            break;
                        case (int)UserType.AgentAdmin:
                            ord = dao.GetAgentAdminOrder(user_id, order_id);
                            break;
                        case (int)UserType.Driver:
                            ord = dao.GetDriverOrder(user_id, order_id);
                            break;
                        case (int)UserType.Consumer:
                            ord = dao.GetConsumerOrder(user_id, order_id);
                            break;
                        default:
                            ord = null;
                            break;
                    }
                    //Order ord = (isConReceipt ? dao.GetConsumerOrder(user_id, order_id) : dao.GetAgentAdminOrder(user_id, order_id));
                    if (ord == null)
                        return null;
                    OrderInvoiceDto dto = new OrderInvoiceDto();
                    OrdersHelper.CopyFromEntity(dto, ord);
                    response = dto;
                }
            }
            else if (order_type == OrderType.OrderTelp)
            {
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    TeleOrder ord = null; //dao.GetAgentOrder(user_id, order_id);
                    switch (userType)
                    {
                        case (int)UserType.AgentBoss:
                            ord = dao.GetAgentBossOrder(user_id, order_id);
                            break;
                        case (int)UserType.AgentAdmin:
                            ord = dao.GetAgentAdminOrder(user_id, order_id);
                            break;
                        case (int)UserType.Driver:
                            ord = dao.GetDriverOrder(user_id, order_id);
                            break;
                        default:
                            ord = null;
                            break;
                    }
                    if (ord == null)
                        return null;
                    OrderInvoiceDto dto = new OrderInvoiceDto();
                    TeleOrderHelper.CopyFromEntity(dto, ord);
                    response = dto;
                }
            }
            return response;
        }

        public static void PopulateOrder(Order ord, int consumerId, int addressId, short slotId = 1)
        {
            ord.AddrID = addressId;
            ord.StatusID = ID_ORDER_ACTIVE;//1;
            ord.OrderTime = DateTime.Now.TimeOfDay;
            ord.OrderDate = DateTime.Now.Date;
            ord.InvoiceNumber = "";
            ord.DeliverySlotID = slotId;
            ord.CreatedDate = DateTime.Now;
            ord.CreatedBy = consumerId;
            ord.UpdatedBy = consumerId;
            ord.UpdatedDate = ord.CreatedDate;
            ord.ConsID = consumerId;
            ord.GrandTotal = 0.0M;
        }

        public static void UpdatePlaceOrderRequest(PlaceOrderRequest request)
        {
            // To update request from mobile, Reason : not passing expected values from mobile
            foreach (var item in request.products)
            {
                using (ProductDao prodDao = new ProductDao())
                {
                    Product product = prodDao.FindProductById(item.product_id);
                    UpdateProductForReq(item, product, false);
                    if (request.has_exchange)
                    {
                        UpdateProductExchangeForReq(item, product, request.exchange.ToList(), false);
                    }
                }
            }
        }

        public static void UpdateProductForReq(ProductsDto item, Product product, bool isPickup)
        {
            item.product_name = product.ProductName;
            if (item.quantity > 0)
            {
                item.unit_price = product.TubePrice;
                item.product_promo = (product.TubePromoPrice - product.TubePrice) * item.quantity;
            }
            else
            {
                item.unit_price = 0;
                item.product_promo = 0;
            }

            if (item.refill_quantity > 0)
            {
                item.refill_price = product.RefillPrice;
                item.refill_promo = (product.RefillPromoPrice - product.RefillPrice) * item.refill_quantity;
            }
            else
            {
                item.refill_price = 0;
                item.refill_promo = 0;
            }

            item.shipping_cost = product.ShippingPrice;
            item.shipping_promo = product.ShippingPromoPrice - product.ShippingPrice;
            item.sub_total = (item.unit_price * item.quantity) + item.product_promo
                            + (item.refill_price * item.refill_quantity) + item.refill_promo;
            if (!isPickup)
            {
                item.sub_total += (item.shipping_cost * item.quantity) + (item.shipping_promo * item.quantity)
                            + (item.shipping_cost * item.refill_quantity) + (item.shipping_promo * item.refill_quantity);
            }
            //return item;
        }

        public static void UpdateProductExchangeForReq(ProductsDto item, Product product, List<ExchangeDto> exchangeRequest, bool isPickup)
        {
            using (ProductDao prodDao = new ProductDao())
            {
                foreach (var exchangeItem in exchangeRequest)
                {
                    var exchangeProduct = product.ProductExchanges.Where(p => p.PrExID == exchangeItem.exchange_id && p.ProdID == item.product_id).FirstOrDefault();
                    if (exchangeProduct != null)
                    {
                        exchangeItem.exchange_with = exchangeProduct.ExchangeWith;
                        exchangeItem.exchange_price = exchangeProduct.ExchangePrice.HasValue ? exchangeProduct.ExchangePrice.Value : 0;
                        exchangeItem.exchange_promo_price = exchangeProduct.ExchangePrice.HasValue ? (exchangeProduct.ExchangePromoPrice - exchangeProduct.ExchangePrice.Value) : 0;
                        item.sub_total += (exchangeItem.exchange_price * exchangeItem.exchange_quantity) + (exchangeItem.exchange_promo_price * exchangeItem.exchange_quantity);

                        if (!isPickup)
                        {
                            item.sub_total += (item.shipping_cost * exchangeItem.exchange_quantity) + (item.shipping_promo * exchangeItem.exchange_quantity);
                        }
                    }
                }
            }
            //return exchangeRequest;
        }

        public static void AddProductsToOrder(Order ord, ICollection<ProductsDto> dtos)
        {
            foreach (var prd in dtos)
            {
                OrderDetail od = new OrderDetail();
                od.Order = ord;
                od.ProdID = prd.product_id;
                od.CreatedDate = ord.CreatedDate;

                od.Quantity = prd.quantity;
                od.UnitPrice = prd.unit_price;
                od.SubTotal = prd.unit_price * prd.quantity; //prd.sub_total;
                od.PromoProduct = prd.product_promo;
                od.ShippingCharge = prd.shipping_cost;
                od.PromoShipping = prd.shipping_promo;

                od.RefillQuantity = prd.refill_quantity;
                od.RefillPrice = prd.refill_price;
                od.PromoRefill = prd.refill_promo;
                od.RefillSubTotal = prd.refill_price * prd.refill_quantity;

                if (prd.quantity > 0)
                {
                    ord.ShippingCharge += (prd.shipping_cost * prd.quantity);
                    ord.PromoShipping += (prd.shipping_promo * prd.quantity);
                    od.TotamAmount = od.SubTotal + prd.product_promo + (prd.shipping_promo * prd.quantity) + (prd.shipping_cost * prd.quantity); //prd.sub_total;
                }
                if (prd.refill_quantity > 0)
                {
                    ord.ShippingCharge += (prd.shipping_cost * prd.refill_quantity);
                    ord.PromoShipping += (prd.shipping_promo * prd.refill_quantity);
                    od.RefillTotalAmount = od.RefillSubTotal + prd.refill_promo + (prd.shipping_promo * prd.refill_quantity) + (prd.shipping_cost * prd.refill_quantity); //prd.sub_total;
                }
                ord.SubTotal += od.SubTotal;
                ord.RefillSubTotal += od.RefillSubTotal;
                ord.PromoProduct += od.PromoProduct;
                ord.PromoRefill += od.PromoRefill;
                ord.GrandTotal += (od.TotamAmount + od.RefillTotalAmount); //prd.sub_total;
                ord.NumberOfProducts = prd.quantity + prd.refill_quantity;
                ord.OrderDetails.Add(od);

                //OrderDetail od = new OrderDetail();
                //od.Order = ord;
                //od.ProdID = prd.product_id;
                //od.CreatedDate = ord.CreatedDate;
                //od.PromoProduct = (prd.unit_price - prd.product_promo) * prd.quantity;
                //od.PromoShipping = (prd.shipping_cost - prd.shipping_promo) * prd.quantity;
                //od.ShippingCharge = prd.shipping_cost;
                //od.SubTotal = prd.sub_total;
                //od.Quantity = prd.quantity;
                //od.UnitPrice = prd.unit_price;                
                //ord.SubTotal += prd.sub_total;
                //ord.ShippingCharge += prd.shipping_cost * prd.quantity;
                //ord.PromoProduct += (prd.product_promo - prd.unit_price) * prd.quantity;
                //ord.PromoShipping += (prd.shipping_promo - prd.shipping_cost) * prd.quantity;
                //ord.OrderDetails.Add(od);
                //ord.GrandTotal += (prd.unit_price - prd.product_promo + prd.shipping_cost - prd.shipping_promo) * prd.quantity;                
            }
        }

        //Sprint 3

        public GetAssignedOrderCountResponse GetAssignedOrderCount(GetAssignedOrderCountRequest request)
        {
            GetAssignedOrderCountResponse response = new GetAssignedOrderCountResponse();
            try
            {
                if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                response.order = new AssignedOrderCountDto();
                using (OrderDao dao = new OrderDao())
                {
                    response.order.assigned_order_count = dao.GetAssignedOrderCount(request.user_id, ID_ORDER_ACCEPTED);
                }
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    response.order.assigned_order_count += dao.GetAssignedOrderCount(request.user_id, ID_ORDER_ACCEPTED);
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("assg.order.count");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public GetDriverOrderListResponse GetDriverOrderList(GetDriverOrderListRequest request)
        {
            GetDriverOrderListResponse response = new GetDriverOrderListResponse();
            try
            {
                if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                List<DriverOrderDetailsDto> ordDtoLst = new List<DriverOrderDetailsDto>();
                using (OrderDao dao = new OrderDao())
                {
                    List<Order> ordList = dao.GetDriverOrderList(request.user_id, request.current_list);
                    response.order_count = ordList.Count;
                    for (int i = 0; i < ordList.Count; i++)
                    {
                        DriverOrderDetailsDto odDto = new DriverOrderDetailsDto();
                        OrdersHelper.CopyFromEntity(odDto, ordList[i]);
                        ordDtoLst.Add(odDto);
                    }
                }
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    List<TeleOrder> ordList = dao.GetDriverOrderList(request.user_id, request.current_list);
                    response.order_count += ordList.Count;
                    for (int i = 0; i < ordList.Count; i++)
                    {
                        DriverOrderDetailsDto odDto = new DriverOrderDetailsDto();
                        OrdersHelper.CopyFromEntity(odDto, ordList[i]);
                        ordDtoLst.Add(odDto);
                    }
                }
                response.code = 0;
                response.order_details = ordDtoLst.ToArray();
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("drv.ordr.listed");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public GetDriverOrderResponse GetDriverOrder(GetDriverOrderRequest request)
        {
            GetDriverOrderResponse response = new GetDriverOrderResponse();
            try
            {
                if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                OrderType orderType;
                try
                {
                    orderType = request.order_type.ToEnumValue<OrderType>();
                }
                catch
                {
                    MakeInvalidOrderTypeResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    DriverOrderDetailDto dto = new DriverOrderDetailDto();
                    if (orderType == OrderType.OrderApp)
                    {
                        Order order = dao.FindOrderForDriver(request.order_id, request.user_id);
                        if (order == null)
                        {
                            MakeInvalidOrderResponse(response);
                            return response;
                        }
                        OrdersHelper.CopyFromEntity(dto, order);
                        response.order_details = dto;
                        response.products = OrdersHelper.CopyFromEntity(order);
                        response.has_exchange = (order.OrderPrdocuctExchanges.Count > 0 ? 1 : 0);
                        if (response.has_exchange == 1)
                        {
                            if (response.exchange == null)
                                response.exchange = new List<ExchangeDto>();
                            foreach (var item in order.OrderPrdocuctExchanges)
                            {
                                ExchangeDto exDto = new ExchangeDto();
                                OrdersHelper.CopyFromEntity(exDto, item);
                                response.exchange.Add(exDto);
                            }
                        }
                    }
                    else if (orderType == OrderType.OrderTelp)
                    {
                        TeleOrder teleOrder = dao.FindTeleOrderForDriver(request.order_id, request.user_id);
                        if (teleOrder == null)
                        {
                            MakeInvalidTeleOrderResponse(response);
                            return response;
                        }
                        OrdersHelper.CopyFromEntity(dto, teleOrder);
                        response.order_details = dto;
                        response.products = OrdersHelper.CopyFromEntity(teleOrder);
                        response.has_exchange = (teleOrder.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
                        if (response.has_exchange == 1)
                        {
                            if (response.exchange == null)
                                response.exchange = new List<ExchangeDto>();
                            foreach (var item in teleOrder.TeleOrderPrdocuctExchanges)
                            {
                                ExchangeDto exDto = new ExchangeDto();
                                TeleOrderHelper.CopyFromEntity(exDto, item);
                                response.exchange.Add(exDto);
                            }
                        }
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("drv.ordr.got");
                }
            }
            catch (Exception ex)///I Started here
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public CloseOrderResponse CloseOrder(CloseOrderRequest request)
        {
            CloseOrderResponse response = new CloseOrderResponse();
            try
            {
                if (request.is_driver)
                {
                    if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                    {
                        DriverServices.MakeNoDriverResponse(response);
                        return response;
                    }
                }
                else
                {
                    if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }
                OrderType orderType;
                try
                {
                    orderType = request.order_type.ToEnumValue<OrdersServices.OrderType>();
                }
                catch
                {
                    MakeInvalidOrderTypeResponse(response);
                    return response;
                }
                if (orderType == OrderType.OrderApp)
                {
                    using (OrderDao dao = new OrderDao())
                    {
                        Order ord = dao.FindById(request.order_id, false);
                        ord.StatusID = ID_ORDER_CLOSED;
                        dao.Update(ord);

                        OrderDelivery orderDelivery = ord.OrderDeliveries.Where(x => x.OrdrID == ord.OrdrID).FirstOrDefault();
                        if (orderDelivery.DeliveryDate.HasValue)
                        {
                            if (orderDelivery.DeliveryDate.Value == DateTime.Now)
                            {
                                //TODO
                                orderDelivery.deviation = 0;
                                orderDelivery.StatusId = DELIVERY_STATUS_ONTIMEDELIVERY;
                            }
                            else if (orderDelivery.DeliveryDate.Value > DateTime.Now)
                            {
                                //TODO
                                //orderDelivery.deviation = Convert.ToInt32(orderDelivery.DeliveryDate.Value - DateTime.Now);
                                orderDelivery.StatusId = DELIVERY_STATUS_EARLYDELIVERY;
                            }
                            else if (orderDelivery.DeliveryDate.Value < DateTime.Now)
                            {
                                //TODO
                                //orderDelivery.deviation = Convert.ToInt32(orderDelivery.DeliveryDate.Value - DateTime.Now);
                                orderDelivery.StatusId = DELIVERY_STATUS_LATEDELIVERY;
                            }
                        }

                        dao.UpdateDelivery(orderDelivery);
                        response.order_details = new CloseOrderDto();
                        response.order_details.order_id = ord.OrdrID;
                        response.order_details.order_status_id = ord.StatusID;
                        response.order_details.order_type = request.order_type;

                        using (UserDao userDao = new UserDao())
                        {
                            int type = request.is_driver ? (int)PushMessagingService.PushType.TypeOne : (int)PushMessagingService.PushType.TypeTwo;
                            Consumer consumer = userDao.FindById(ord.ConsID);
                            ReadAndSendPushNotification(APPSETTING_MSG_TO_CONSUMER_FOR_RATING, APPSETTING_TITLE_FOR_CONSUMER_TO_RATE, consumer.AppToken, request.order_id, ord.DrvrID.HasValue ? ord.DrvrID.Value : 0, 0, PushMessagingService.APPSETTING_APPLICATION_ID_CONSUMER, PushMessagingService.APPSETTING_SENDER_ID_CONSUMER, type);
                        }
                    }
                }
                else if (orderType == OrderType.OrderTelp)
                {
                    using (TeleOrderDao dao = new TeleOrderDao())
                    {
                        TeleOrder teleOrder = dao.FindById(request.order_id, false);
                        teleOrder.StatusId = ID_ORDER_CLOSED;
                        dao.Update(teleOrder);

                        TeleOrderDelivery teleOrderDelivery = teleOrder.TeleOrderDeliveries.Where(x => x.TeleOrdID == teleOrder.TeleOrdID).FirstOrDefault();
                        if (teleOrderDelivery.DeliveryDate.HasValue)
                        {
                            if (teleOrderDelivery.DeliveryDate.Value == DateTime.Now)
                            {
                                //TODO
                                teleOrderDelivery.deviation = 0;
                                teleOrderDelivery.StatusId = DELIVERY_STATUS_ONTIMEDELIVERY;
                            }
                            else if (teleOrderDelivery.DeliveryDate.Value > DateTime.Now)
                            {
                                //TODO
                                //orderDelivery.deviation = Convert.ToInt32(orderDelivery.DeliveryDate.Value - DateTime.Now);
                                teleOrderDelivery.StatusId = DELIVERY_STATUS_EARLYDELIVERY;
                            }
                            else if (teleOrderDelivery.DeliveryDate.Value < DateTime.Now)
                            {
                                //TODO
                                //orderDelivery.deviation = Convert.ToInt32(orderDelivery.DeliveryDate.Value - DateTime.Now);
                                teleOrderDelivery.StatusId = DELIVERY_STATUS_LATEDELIVERY;
                            }
                        }

                        dao.UpdateDelivery(teleOrderDelivery);

                        response.order_details = new CloseOrderDto();
                        response.order_details.order_id = teleOrder.TeleOrdID;
                        response.order_details.order_status_id = teleOrder.StatusId;
                        response.order_details.order_type = request.order_type;
                    }
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("ordr.closed");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto SendEmail(SendEmailRequest request)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    Order ord = dao.FindById(request.order_id, false);
                    //TODO 
                    // EmailServices.SendMail(...)
                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("email.sent");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public ResponseDto OutForDelivery(OutForDeliveryRequest request)
        {
            OutForDeliveryResponse response = new OutForDeliveryResponse();
            try
            {
                if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                OrderType orderType;
                try
                {
                    orderType = request.order_type.ToEnumValue<OrdersServices.OrderType>();
                }
                catch
                {
                    MakeInvalidOrderTypeResponse(response);
                    return response;
                }
                if (orderType == OrderType.OrderApp)
                {
                    using (OrderDao dao = new OrderDao())
                    {
                        //Update other out for delivery status to accepted, updated by CVN
                        List<Order> OfdOrders = dao.GetOutForDeliveryOrders(request.user_id);
                        foreach (var item in OfdOrders)
                        {
                            item.StatusID = ID_ORDER_ACCEPTED;
                            dao.Update(item);
                        }

                        Order ord = dao.FindById(request.order_id, false);
                        ord.StatusID = ID_ORDER_OUT_FOR_DELIVERY;
                        dao.Update(ord);
                        OrderDelivery orderDelivery = ord.OrderDeliveries.Where(x => x.OrdrID == ord.OrdrID).FirstOrDefault();
                        orderDelivery.StatusId = DELIVERY_STATUS_OUTFORDELIVERY;
                        orderDelivery.StartDate = DateTime.Now;
                        dao.UpdateDelivery(orderDelivery);
                        OutForDeliveryDto ofdel = new OutForDeliveryDto();
                        ofdel.order_id = ord.OrdrID;
                        ofdel.order_status_id = ord.StatusID;
                        ofdel.order_type = request.order_type;
                        response.order_details = ofdel;
                    }
                }
                else if (orderType == OrderType.OrderTelp)
                {
                    using (TeleOrderDao dao = new TeleOrderDao())
                    {
                        //Update other out for delivery status to accepted, updated by CVN
                        List<TeleOrder> OfdOrders = dao.GetOutForDeliveryTeleOrders(request.user_id);
                        foreach (var item in OfdOrders)
                        {
                            item.StatusId = ID_ORDER_ACCEPTED;
                            dao.Update(item);
                        }

                        TeleOrder teleOrder = dao.FindById(request.order_id, false);
                        if (teleOrder == null)
                        {
                            MakeNoOrderFoundResponse(response);
                            return response;
                        }
                        teleOrder.StatusId = ID_ORDER_OUT_FOR_DELIVERY;
                        dao.Update(teleOrder);
                        TeleOrderDelivery orderDelivery = teleOrder.TeleOrderDeliveries.Where(x => x.TeleOrdID == teleOrder.TeleOrdID).FirstOrDefault();
                        orderDelivery.StatusId = DELIVERY_STATUS_OUTFORDELIVERY;
                        orderDelivery.StartDate = DateTime.Now;
                        dao.UpdateDelivery(orderDelivery);
                        OutForDeliveryDto ofdel = new OutForDeliveryDto();
                        ofdel.order_id = teleOrder.TeleOrdID;
                        ofdel.order_status_id = teleOrder.StatusId;
                        ofdel.order_type = request.order_type;
                        response.order_details = ofdel;
                    }
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("out.for.delivery");
            }

            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public void MakeNoOrderFoundResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.order");
        }

        public void MakeInvalidOrderTypeResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.order.type");
        }

        //updated
        public GetDriverListResponse GetDriverList(GetDriverListRequest request)
        {
            GetDriverListResponse response = new GetDriverListResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    var driverdetails = dao.GetDriverFromOrder(request.user_id, request.order_id);
                    if (driverdetails.Count <= 0)
                    {
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("driver_list_for_order_not_found");
                        return response;
                    }

                    response.driver_details = new List<DriverDetailListDto>();
                    using (DriverDao drvrDao = new DriverDao())
                    {
                        foreach (var item in driverdetails)
                        {
                            DriverDetailListDto drverDetail = new DriverDetailListDto();
                            //Driver drvr = drvrDao.FindById(item.drvr_id);
                            DriverHelper.CopyFromEntity(drverDetail, item);
                            var drvr = drvrDao.FindById(item.drvr_id);
                            drverDetail.driver_profile_image = drvr.ProfileImage != null ? ImagePathService.driverImagePath + drvr.ProfileImage : string.Empty;
                            using (ConsumerReviewDao conReviewDao = new ConsumerReviewDao())
                            {
                                List<ConsumerReview> conReview = new List<ConsumerReview>();
                                conReview = conReviewDao.GetReviewByDriver(item.drvr_id);
                                drverDetail.driver_rating = conReview.Count > 0 ? Convert.ToDecimal(conReview.Average(x => x.Rating)) : 0;
                            }
                            response.driver_details.Add(drverDetail);
                        }
                    }
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("driver_list_for_order_found");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        /// <summary>
        /// To get active orders count for boss
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetActiveOrderCountBossResponse</returns>
        public GetActiveOrderCountBossResponse GetActiveOrderCountBoss(GetActiveOrderCountBossRequest request)
        {
            GetActiveOrderCountBossResponse response = new GetActiveOrderCountBossResponse();
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    List<int> arrStatus = new List<int>()
                    {
                        ID_ORDER_ACCEPTED ,
                        ID_ORDER_OUT_FOR_DELIVERY
                    };

                    int orderCount = dao.GetAgentBossOrdersCount(request.user_id, arrStatus);
                    response.active_orders = new ActiveOrdersBossDto { active_order_count = orderCount };
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = orderCount > 0 ? MessagesSource.GetMessage("active.orders") : MessagesSource.GetMessage("no.active.orders");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        /// <summary>
        ///  To get order lists for boss
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetOrderListBossResponse</returns>
        public GetOrderListBossResponse GetOrderListBoss(GetOrderListBossRequest request)
        {
            GetOrderListBossResponse response = new GetOrderListBossResponse();
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    var orders = dao.GetAgentBossOrders(request.user_id);
                    if (orders != null && orders.Count > 0)
                    {
                        #region ActiveOrders
                        List<int> activeStatus = new List<int>() { ID_ORDER_ACCEPTED, ID_ORDER_OUT_FOR_DELIVERY };

                        var activeOrders = orders.Where(a => activeStatus.Contains(a.StatusID)).Select
                            (
                                a => OrdersHelper.CopyFromEntity(new OrderDetailsBossDto(), a)
                            ).ToList();

                        #endregion ActiveOrders

                        #region HistoryOrders

                        List<int> historyStatus = new List<int>() { ID_ORDER_CLOSED };
                        var historyOrders = orders.Where(a => historyStatus.Contains(a.StatusID)).Select
                            (
                                a => OrdersHelper.CopyFromEntity(new OrderDetailsBossDto(), a)
                            ).ToList();

                        #endregion ActiveOrders

                        response.orders = new OrdersBossDto() { active_order_count = activeOrders.Count, history_order_count = historyOrders.Count };
                        response.active_orders = activeOrders;
                        response.history_orders = historyOrders;
                    }

                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("boss.ordr.listed");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        /// <summary>
        /// To get order details for boss
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetOrderDetailsBossResponse</returns>
        public GetOrderDetailsBossResponse GetOrderDetailsBoss(GetOrderDetailsBossRequest request)
        {
            GetOrderDetailsBossResponse response = new GetOrderDetailsBossResponse();
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }

                OrderFullDetailsBossDto dto = new OrderFullDetailsBossDto();
                OrderType orderType = request.order_type.ToEnumValue<OrderType>();
                if (orderType == OrderType.OrderApp)
                {
                    Order ord = new OrderDao().FindById(request.order_id, true);
                    if (ord != null)
                        OrdersHelper.CopyFromEntity(dto, ord);
                }
                else if (orderType == OrderType.OrderTelp)
                {
                    TeleOrder ord = new TeleOrderDao().FindById(request.order_id, true);
                    if (ord != null)
                        OrdersHelper.CopyFromEntity(dto, ord, ord.Driver);
                }

                response.order_details = dto;
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("ordr.details");

            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        /// <summary>
        /// Send e-receipt to agentadmin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseDto SendInvoiceMail(AgentAdminInvoiceMailSendRequest request)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }

                using (AgentAdminDao adminDao = new AgentAdminDao())
                {
                    OrderType orderType;
                    try
                    {
                        orderType = request.order_type.ToEnumValue<OrderType>();
                    }
                    catch
                    {
                        MakeInvalidOrderTypeResponse(response);
                        return response;
                    }
                    OrderInvoiceDto orderInvoice = GetOrderInvoiceOrReciept(request.user_id, orderType, request.order_id, (int)UserType.AgentAdmin);
                    if (orderInvoice == null)
                    {
                        MakeNoOrderFoundResponse(response);
                        return response;
                    }
                    var admin = adminDao.FindById(request.user_id);
                    if (string.IsNullOrEmpty(admin.email))
                    {
                        response.code = 1;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("mail.id.not.found");
                    }
                    string subject = "e-Receipt_" + request.order_id.ToString() + "_" + DateTime.Now.Date.ToShortDateString();
                    //string emailBody = GetEmailBody(admin, orderInvoice);
                    string emailAttachment = CreateEmailAttachment(admin, orderInvoice);
                    string emailBody = CreateEmailBody(orderInvoice);
                    //var attachment = PdfServices.GetAttachment(emailBody, orderInvoice.order_id);
                    byte[] fileBytes = PdfServices.HtmlTOPdf(emailAttachment);
                    EmailServices.SendMailWithAttachment(admin.email, subject, emailBody, request.order_id, fileBytes);

                    //EmailServices.SendMail(admin.email, subject, emailBody.ToString());
                }
                response.code = 0;
                response.has_resource = 0;
                response.message = MessagesSource.GetMessage("invoice.mail.sent");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }


        /// <summary>
        /// Get drivers for agent admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetDriverListForAgentAdminResponse GetDriversForAgentAdmin(GetDriverListForAgentAdminRequest request)
        {
            GetDriverListForAgentAdminResponse response = new GetDriverListForAgentAdminResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (DriverDao drvrDao = new DriverDao())
                {
                    var drivers = drvrDao.GetAllDriversByAgentId(request.user_id);
                    if (drivers.Count == 0)
                    {
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("driver_list_for_order_not_found");
                        return response;
                    }

                    using (DeliverySlotDao slotDao = new DeliverySlotDao())
                    {
                        List<MDeliverySlot> timeSlots = null;
                        DateTime startDate = DateTime.Now;
                        DateTime endDate = startDate.Date.AddDays(DAYS_TO_LIST);                        

                        response.driver_details = new List<AllDriversForAdmin>();
                        using (ConsumerReviewDao conReviewDao = new ConsumerReviewDao())
                        {
                            foreach (var item in drivers)
                            {
                                List<TimeslotDisplayDto> slotDtos = new List<TimeslotDisplayDto>();
                                Driver dvr = drvrDao.FindById(item.DrvrID);
                                AllDriversForAdmin driverDetail = new AllDriversForAdmin();
                                driverDetail.drivers = new DriverDto();
                                driverDetail.drivers.driver_id = dvr.DrvrID;
                                driverDetail.drivers.driver_name = dvr.DriverName;
                                driverDetail.drivers.driver_image = dvr.ProfileImage != null ? ImagePathService.driverImagePath + dvr.ProfileImage : string.Empty;
                                timeSlots = slotDao.GetAllSlots();
                                int count = 1;
                                DateTime dt = startDate;
                                int addDayCountToday = 0;
                                int addDayCountTommorw = 1;
                                int addDayCountDayAftrTmrw = 2;
                                for (int intCnt = 0; intCnt < DAYS_TO_LIST; intCnt++)
                                {
                                    TimeSpan elapseTime = TimeSpan.FromMinutes(30);
                                    TimeSpan currentTime = DateTime.Now.TimeOfDay.Add(elapseTime);
                                    List<MDeliverySlot> slots = new List<MDeliverySlot>();
                                    //if (dt.Date == DateTime.Now.Date)
                                    //{
                                    //    slots = timeSlots.Where(x => x.EndTine >= currentTime).ToList();
                                    //    if (slots.Count <= 0)
                                    //    {
                                    //        dt = dt.AddDays(1);
                                    //        slots = timeSlots;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    slots = timeSlots;
                                    //}
                                    slots = timeSlots;
                                    if (dt.Date.DayOfWeek.ToString() == TimeslotService.DAY_NAME_SUNDAY)
                                    {
                                        dt = dt.AddDays(1);
                                        addDayCountToday++;
                                        addDayCountTommorw++;
                                        addDayCountDayAftrTmrw++;
                                    }

                                    TimeslotDisplayDto dto = new TimeslotDisplayDto();
                                    if (dt.Date == DateTime.Now.Date.AddDays(addDayCountToday))
                                    {
                                        dto.day_name = TimeslotService.DAY_NAME_TODAY;
                                    }
                                    else if (dt.Date == DateTime.Now.Date.AddDays(addDayCountTommorw))
                                    {
                                        dto.day_name = TimeslotService.DAY_NAME_TOMORROW;
                                    }
                                    else if (dt.Date == DateTime.Now.Date.AddDays(addDayCountDayAftrTmrw))
                                    {
                                        dto.day_name = TimeslotService.DAY_NAME_DAY_AFTER_TOMORROW;
                                    }

                                    dto.day_date = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    dto.time_slot = new List<TimeslotDaysDto>();
                                    foreach (MDeliverySlot slt in slots)
                                    {
                                        TimeslotDaysDto Daydto = new TimeslotDaysDto();
                                        Daydto.time_slot_id = slt.SlotID;//count;
                                        Daydto.time_slot_name = slt.SlotName;
                                        Daydto.availability = ((slt.EndTine < currentTime && dto.day_name == TimeslotService.DAY_NAME_TODAY) ? 0 : slotDao.CheckAvailabilityForDriver(dt, Daydto.time_slot_id, item.DrvrID)).ToString() + "/" + Common.GetAppSetting<string>(DriverHelper.APPSETTING_MAX_DRIVER_ORDER, string.Empty); ;
                                        dto.time_slot.Add(Daydto);
                                        count++;
                                    }
                                    slotDtos.Add(dto);
                                    dt = dt.AddDays(1);
                                }
                                driverDetail.driver_availability = slotDtos;

                                List<ConsumerReview> conReview = new List<ConsumerReview>();
                                conReview = conReviewDao.GetReviewByDriver(item.DrvrID);
                                driverDetail.drivers.driver_rating = conReview.Count > 0 ? Convert.ToDecimal(conReview.Average(x => x.Rating)) : 0;

                                response.driver_details.Add(driverDetail);
                            }
                        }
                    }
                }
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("driver_list_for_order_found");
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }





        public GetIssuesCountResponse GetIssuesCount(GetIssuesCountRequest request)
        {
            GetIssuesCountResponse response = new GetIssuesCountResponse();
            try
            {
                if (!_userServices.CheckAuthSuperUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    int orderCount = dao.GetIssueCount(ID_ORDER_ACTIVE);
                    response.issues = new IssueCountDto { issue_count = orderCount };
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = orderCount > 0 ?
                        MessagesSource.GetMessage("active.orders") :
                        MessagesSource.GetMessage("no.active.orders");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetIssuesListResponse GetIssuesList(GetIssuesListRequest request)
        {
            GetIssuesListResponse response = new GetIssuesListResponse();
            try
            {
                if (!_userServices.CheckAuthSuperUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {
                    int orderCount = dao.GetIssueCount(ID_ORDER_ACTIVE);
                    response.orders = new OrderCountDto { order_count = orderCount };

                    List<GetIssuesListForSUser_Result> IssuesList = dao.GetIssuesListForSUser();
                    response.order_details = new IssueDetailsDto[IssuesList.Count];
                    for (int i = 0; i < IssuesList.Count; i++)
                    {
                        IssueDetailsDto orddtlsdto = new IssueDetailsDto();
                        OrdersHelper.CopyFromEntity(orddtlsdto, IssuesList[i]);
                        response.order_details[i] = orddtlsdto;
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("ordr.listed");
                }

            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }
        public ResponseDto ForcedCancelOrderSuser(ForcedCancelOrderSuserRequest request)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                if (!_userServices.CheckAuthSuperUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }
                if (!CheckOrderStatus(request.order_id))
                {
                    MakeInvalidOrderResponse(response);
                    return response;
                }
                using (OrderDao dao = new OrderDao())
                {

                    Order ord = dao.FindById(request.order_id, false);
                    ord.StatusID = ID_ORDER_CANCELLED_BY_SUSER;
                    dao.Update(ord);

                    ReadAndSendPushNotification(APPSETTING_MSG_FOR_CONSUMER_AT_SADMIN_CANCEL, APPSETTING_TITLE_FOR_CONSUMER_AT_SADMIN_CANCEL, ord.Consumer.AppToken, request.order_id, 0, 0, PushMessagingService.APPSETTING_APPLICATION_ID_CONSUMER, PushMessagingService.APPSETTING_SENDER_ID_CONSUMER, (int)PushMessagingService.PushType.TypeThree);

                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("ordr.cancelled");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetIssueDetailsResponseSUsers GetIssueDetailsResponseSUsers(GetIssueDetailsRequestSUsers request)
        {
            GetIssueDetailsResponseSUsers response = new GetIssueDetailsResponseSUsers();
            try
            {
                if (!_userServices.CheckAuthSuperUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }

                using (OrderDao dao = new OrderDao())
                {

                    GetOrderDeatilsbyOrderIdForSUser_Result orddtls = dao.GetOrderDeatilsbyOrderIdForSUser(request.order_id);
                    SUserOrderDetailsDto oDto = new SUserOrderDetailsDto();
                    OrdersHelper.CopyFromEntity(oDto, orddtls);
                    response.order_details = oDto;

                    List<GetProductDeatilsbyOrderIdForSUser_Result> prdList = dao.GetProductDeatilsbyOrderIdForSUser(request.order_id);
                    response.product_details = new SUserProductDetailsDto[prdList.Count];
                    for (int i = 0; i < prdList.Count; i++)
                    {
                        SUserProductDetailsDto prddtlsdto = new SUserProductDetailsDto();
                        OrdersHelper.CopyFromEntity(prddtlsdto, prdList[i]);
                        response.product_details[i] = prddtlsdto;
                    }


                    GetAgencyDetailsbyOrderIdForSUser_Result agncyorddtls = dao.GetAgencyDetailsbyOrderIdForSUser(request.order_id);
                    SUserAgencyDetailsDto agncyDto = new SUserAgencyDetailsDto();
                    OrdersHelper.CopyFromEntity(agncyDto, agncyorddtls);
                    response.agency_details = agncyDto;


                    List<OrderPrdocuctExchange> ordprdExcng = dao.GetProductExchangeSuser(request.order_id);
                    response.has_exchange = (ordprdExcng.Count > 0 ? 1 : 0);
                    if (ordprdExcng.Count > 0)
                    {
                        response.exchange = new ExchangeDto[ordprdExcng.Count];
                        for (int i = 0; i < ordprdExcng.Count; i++)
                        {
                            ExchangeDto exchangedto = new ExchangeDto();
                            OrdersHelper.CopyFromEntity(exchangedto, ordprdExcng[i]);
                            response.exchange[i] = exchangedto;
                        }
                    }

                    response.code = 0;
                    response.has_resource = 0;
                    response.message = MessagesSource.GetMessage("issue.details");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetAllOrderDetailsResponse GetAllOrderDetails(GetAllOrderDetailsRequest request)
        {
            GetAllOrderDetailsResponse response = new GetAllOrderDetailsResponse();
            try
            {
                if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                {
                    _userServices.MakeNouserResponse(response);
                    return response;
                }

                using (OrderDao dao = new OrderDao())
                {
                    var orderDetails = dao.GetAllOrderForConsumer(request.user_id, request.current_list, request.page_number, request.records_per_page);
                    if (orderDetails.Count == 0)
                    {
                        MakeNoOrderFoundResponse(response);
                        return response;
                    }
                    response.order_details = new List<AllOrderDetails>();
                    foreach (var item in orderDetails)
                    {
                        AllOrderDetails ordDetail = new AllOrderDetails();
                        OrdersHelper.CopyFromEntity(ordDetail, item);
                        using (ConsumerReviewDao conReviewDao = new ConsumerReviewDao())
                        {
                            if (item.DrvrID.HasValue)
                            {
                                List<ConsumerReview> conReview = new List<ConsumerReview>();
                                conReview = conReviewDao.GetReviewByDriver(item.DrvrID.Value);
                                ordDetail.driver_details.driver_rating = conReview.Count > 0 ? Convert.ToDecimal(conReview.Average(x => x.Rating)) : 0;
                            }
                        }
                        response.order_details.Add(ordDetail);
                    }

                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("ordr.listed");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public bool CheckOrderStatus(int order_id)
        {
            Order order = null;
            using (OrderDao dao = new OrderDao())
            {
                order = GetCheckStatus(dao, order_id);
                if (order == null)
                    return false;
            }

            return order != null;
        }
        public Order GetCheckStatus(OrderDao orderDao, int order_id)
        {
            Order order = null;
            order = orderDao.GetCheckStatusById(order_id);

            if (order != null && order.StatusID == ID_ORDER_ACTIVE)
            {
                return order;
            }
            return null;
        }

        public void MakeInvalidOrderResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.order");
        }

        public void MakeInvalidTeleOrderResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.tele.order");
        }

        public void MakeInvalidExchangeInputResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.exchange.input");
        }

        public void MakeInvalidDeliveryDateFormat(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.delivery.date.format");
        }

        public void ReadAndSendPushNotification(string messageTypeKey, string msgTitleKey, string appToken, int orderId, int driverId, int orderCount, string applicationId, string senderId, int type)
        {
            string msgContent = Common.GetAppSetting<string>(messageTypeKey, string.Empty);
            msgContent = msgContent.Replace("{order_id}", orderId.ToString());
            msgContent = msgContent.Replace("{order_count}", orderCount > 0 ? orderCount.ToString() : string.Empty);
            msgContent = msgContent.Replace("{driver_id}", driverId.ToString());
            string msgTitle = Common.GetAppSetting<string>(msgTitleKey, string.Empty);
            PushMessagingService.SendPushNotification(appToken, msgContent, msgTitle, applicationId, senderId, orderId, driverId, type);
        }

        public void TestPushNotification(string deviceId)
        {
            ReadAndSendPushNotification(APPSETTING_MSG_TO_ASSIGNED_DRIVER, APPSETTING_TITLE_FOR_ASSIGNED_DRIVER, deviceId, 0, 0, 0, PushMessagingService.APPSETTING_APPLICATION_ID_DRIVER, PushMessagingService.APPSETTING_SENDER_ID_DRIVER, (int)PushMessagingService.PushType.TypeOne);
        }
        #endregion

        #region Private Methods
        private string GetEmailBody(AgentAdmin admin, OrderInvoiceDto orderInvoice)
        {
            string mailBody = "";
            mailBody += "<style>td{border: 1px solid black;} table{border-style: solid;}</style>";
            mailBody += "<div>Hello " + admin.AgentAdminName + ",</div></br></br>";
            mailBody += "<div>e-Reciept for order id " + orderInvoice.order_id + "</div></br>";
            mailBody += "<table><tr><td>Invoice Number</td><td>" + orderInvoice.invoice_number + "</td></tr>";
            mailBody += "<tr><td>Order Date</td><td>" + orderInvoice.order_date + "</td></tr>";
            mailBody += "<tr><td>Order Time</td><td>" + orderInvoice.order_time + "</td></tr>";
            mailBody += "<tr><td>Consumer Name</td><td>" + orderInvoice.consumer_name + "</td></tr>";
            mailBody += "<tr><td>Consumer Mobile</td><td>" + orderInvoice.consumer_mobile + "</td></tr>";
            mailBody += "<tr><td>Consumer Address</td><td>" + orderInvoice.consumer_address + "</td></tr>";
            mailBody += "<tr><td>Consumer Location</td><td>" + orderInvoice.consumer_location + "</td></tr>";
            mailBody += "<tr><td>Postal Code</td><td>" + orderInvoice.postal_code + "</td></tr>";
            mailBody += "<tr><td>Agency Name</td><td>" + orderInvoice.agency_name + "</td></tr>";
            mailBody += "<tr><td>Agency Address</td><td>" + orderInvoice.agency_address + "</td></tr>";
            mailBody += "<tr><td>Agency Location</td><td>" + orderInvoice.agency_location + "</td></tr>";
            foreach (var item in orderInvoice.products)
            {
                mailBody += "<tr><td>Product Name</td><td>" + item.product_name + "</td></tr>";
                mailBody += "<tr><td>Quantity</td><td>" + item.quantity + "</td></tr>";
                mailBody += "<tr><td>Unit Price</td><td>" + item.unit_price + "</td></tr>";
                mailBody += "<tr><td>Sub Total</td><td>" + item.sub_total + "</td></tr>";
                mailBody += "<tr><td>Product Promo</td><td>" + item.product_promo + "</td></tr>";
                mailBody += "<tr><td>Shipping Cost</td><td>" + item.shipping_cost + "</td></tr>";
                mailBody += "<tr><td>Shipping Promo</td><td>" + item.shipping_promo + "</td></tr>";
            }
            mailBody += "<tr><td>Grant Total</td><td>" + orderInvoice.grand_total + "</td></tr>";

            return mailBody;
        }

        private string CreateEmailBody(OrderInvoiceDto orderInvoice)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~\\Templates\\eRecieptToAdminMessage.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{order_id}", orderInvoice.order_id.ToString());
            return body;
        }

        private string CreateEmailAttachment(AgentAdmin admin, OrderInvoiceDto orderInvoice)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~\\Templates\\eRecieptToAdminTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentAdminName}", admin.AgentAdminName); //replacing the required things  
            body = body.Replace("{order_id}", orderInvoice.order_id.ToString());
            body = body.Replace("{invoice_number}", orderInvoice.invoice_number);
            body = body.Replace("{order_date}", orderInvoice.order_date);
            body = body.Replace("{order_time}", orderInvoice.order_time.ToString());
            body = body.Replace("{consumer_name}", orderInvoice.consumer_name);
            body = body.Replace("{consumer_mobile}", orderInvoice.consumer_mobile);
            body = body.Replace("{consumer_address}", orderInvoice.consumer_address);
            body = body.Replace("{consumer_location}", orderInvoice.consumer_location);
            body = body.Replace("{postal_code}", orderInvoice.postal_code);
            body = body.Replace("{agency_name}", orderInvoice.agency_name);
            body = body.Replace("{agency_address}", orderInvoice.agency_address);
            body = body.Replace("{agency_location}", orderInvoice.agency_location);

            StringBuilder productsList = new StringBuilder();
            int count = 1;
            foreach (var item in orderInvoice.products)
            {
                string products = "";
                if (item.quantity > 0)
                {
                    products += " <tr>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'> " + count + ". " + item.product_name + " </td>";
                    products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.quantity + " </td>";
                    products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.unit_price + " </td>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.quantity * item.unit_price + "</td>";
                    products += "</tr >";
                    products += " <tr>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Promo </td>";
                    products += " <td height='24' style='font-size: 14px;font-family:arial;text-align:center;color: #777;'> " + item.quantity + " </td >";
                    products += "<td height='24' style='font - size: 14px; font-family:arial; text-align:right; color: #777;'> " + item.product_promo / item.quantity + " </td >";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.product_promo + " </td >";
                    products += "</tr >";
                    if (item.shipping_cost > 0)
                    {
                        products += " <tr>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Ongkos Kirim </td >";
                        products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.quantity + " </td>";
                        products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.shipping_cost + " </td>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.quantity * item.shipping_cost + " </td>";
                        products += "</tr>";
                        products += " <tr>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Promo Ongkos Kirim </td >";
                        products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.quantity + " </td>";
                        products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.shipping_promo + " </td>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.quantity * item.shipping_promo + " </td >";
                        products += "</tr >";
                    }
                }
                if (item.refill_quantity > 0)
                {
                    products += "<tr ><td colspan='4' height='15' style='border-bottom: solid 1px #ccc;'></td> </tr >";
                    products += " <tr>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>" + item.product_name + "- Refill </td >";
                    products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.refill_quantity + " </td>";
                    products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.refill_price + " </td>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.refill_quantity * item.refill_price + " </td>";
                    products += "</tr>";
                    products += " <tr>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Promo </td >";
                    products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.refill_quantity + " </td>";
                    products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.refill_promo / item.refill_quantity + " </td>";
                    products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.refill_promo + " </td >";
                    products += "</tr >";
                    if (item.shipping_cost > 0)
                    {
                        products += " <tr>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Ongkos Kirim </td >";
                        products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.refill_quantity + " </td>";
                        products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.shipping_cost + " </td>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.refill_quantity * item.shipping_cost + " </td>";
                        products += "</tr>";
                        products += " <tr>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Promo Ongkos Kirim </td >";
                        products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.refill_quantity + " </td>";
                        products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.shipping_promo + " </td>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.refill_quantity * item.shipping_promo + " </td >";
                        products += "</tr >";
                    }
                }
                if (orderInvoice.has_exchange == 1)
                {
                    StringBuilder exchangeList = new StringBuilder();
                    foreach (var itemExchange in orderInvoice.exchange)
                    {
                        products += "<tr ><td colspan='4' height='15' style='border-bottom: solid 1px #ccc;'></td> </tr >";
                        products += " <tr>";
                        //products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>" + item.product_name + " dengan " + itemExchange.exchange_quantity + " x " + itemExchange.exchange_with + " </td>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>" + item.product_name + " dengan " + itemExchange.exchange_with + " </td>";
                        products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + itemExchange.exchange_quantity + " </td>";
                        products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + itemExchange.exchange_price + " </td>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + itemExchange.exchange_quantity * itemExchange.exchange_price + "</td>";
                        products += "</tr >";
                        products += " <tr>";
                        products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Promo</td>";
                        products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + itemExchange.exchange_quantity + " </td>";
                        products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + itemExchange.exchange_promo_price + " </td>"; // exchange_promo_price is updated here to match ereceipt format(not multiplied with quantity) in API doc
                        products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + itemExchange.exchange_quantity * itemExchange.exchange_promo_price + "</td>"; // exchange_promo_price is updated here to match ereceipt format(not multiplied with quantity) in API doc
                        products += "</tr >";
                        if (item.shipping_cost > 0)
                        {
                            products += " <tr>";
                            products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Ongkos Kirim </td >";
                            products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + itemExchange.exchange_quantity + " </td>";
                            products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.shipping_cost + " </td>";
                            products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + itemExchange.exchange_quantity * item.shipping_cost + " </td>";
                            products += "</tr>";
                            products += " <tr>";
                            products += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Promo Ongkos Kirim </td >";
                            products += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + itemExchange.exchange_quantity + " </td>";
                            products += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.shipping_promo + " </td>";
                            products += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + itemExchange.exchange_quantity * item.shipping_promo + " </td >";
                            products += "</tr >";
                        }
                        //string exchanges = "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar harga</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_price + "</td></tr>";
                        //exchanges += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar promo harga</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_promo_price + "</td></tr>";
                        //exchanges += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar kuantitas</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_quantity + "</td></tr>";
                        //exchanges += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar dengan</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_with + "</td></tr>";
                        exchangeList.Append(products);
                    }
                }

                //string products = "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Nama barang</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.product_name + "</td></tr>";
                //products += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Jumlah</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.quantity + "</td></tr>";
                //products += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Harga satuan</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.unit_price + "</td></tr>";
                //products += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Total</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.sub_total + "</td></tr>";
                //products += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Promo</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.product_promo + "</td></tr>";
                //products += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Ongkos kirim</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.shipping_cost + "</td></tr>";
                //products += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Promo ongkos kirim</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.shipping_promo + "</td></tr>";
                productsList.Append(products);
                count++;
            }
            body = body.Replace("{products}", productsList.ToString());
            //TODO
            //body = body.Replace("{has_exchange}", orderInvoice.has_exchange.ToString());
            //if (orderInvoice.has_exchange == 1)
            //{
            //    StringBuilder exchangeList = new StringBuilder();
            //    foreach (var item in orderInvoice.exchange)
            //    {
            //        string exchanges = " <tr>";
            //        exchanges += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>" + item.product_name + " dengan " + item.exchange_quantity + " x " + item.exchange_with + " </td>";
            //        exchanges += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.exchange_quantity + " </td>";
            //        exchanges += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.exchange_price + " </td>";
            //        exchanges += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.exchange_quantity * item.exchange_price + "</td>";
            //        exchanges += "</tr >";
            //        exchanges += " <tr>";
            //        exchanges += "<td height='24' style='font-size: 14px;font-family:arial;color: #777;'>Tukar Promo</td>";
            //        exchanges += " <td height='24' style='font-size: 14px;font-family:arial;text-align: center;color: #777;'>" + item.exchange_quantity + " </td>";
            //        exchanges += "<td height='24' style='font-size: 14px; font-family:arial; text-align: right; color: #777;'>" + item.exchange_promo_price + " </td>";
            //        exchanges += "<td height='24' style='font-size: 14px;font-family:arial;text-align: right;color: #777;'>" + item.exchange_quantity * item.exchange_promo_price + "</td>";
            //        exchanges += "</tr >";

            //        //string exchanges = "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar harga</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_price + "</td></tr>";
            //        //exchanges += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar promo harga</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_promo_price + "</td></tr>";
            //        //exchanges += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar kuantitas</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_quantity + "</td></tr>";
            //        //exchanges += "<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar dengan</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>" + item.exchange_with + "</td></tr>";
            //        exchangeList.Append(exchanges);
            //    }
            //    body = body.Replace("{exchanges}", exchangeList.ToString());
            //}
            //else
            //    body = body.Replace("{exchanges}", string.Empty);
            body = body.Replace("{grand_total_with_discount}", orderInvoice.grand_total_with_discount.ToString());
            body = body.Replace("{grand_discount}", orderInvoice.grand_discount.ToString());
            body = body.Replace("{grand_total}", orderInvoice.grand_total.ToString());

            return body;
        }
        #endregion
    }

}
