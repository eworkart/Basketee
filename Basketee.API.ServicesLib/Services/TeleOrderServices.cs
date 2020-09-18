using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.TeleOrder;
using Basketee.API.DTOs.Promo;
using Basketee.API.Models;
using Basketee.API.DAOs;
using Basketee.API.Services.Helpers;
using Basketee.API.DTOs.Orders;
using System.Globalization;
using Basketee.API.DTOs;

namespace Basketee.API.Services
{
    public class TeleOrderServices
    {
        OrdersServices _orderService = new OrdersServices();
        public AddTeleOrderResponse AddTeleOrder(AddTeleOrderRequest request)
        {
            AddTeleOrderResponse response = new AddTeleOrderResponse();
            try
            {
                AgentAdmin admin = AgentAdminServices.GetAuthAdmin(request.user_id, request.auth_token, response);
                if (admin == null)
                {
                    return response;
                }

                if (request.delivery_date <= DateTime.MinValue)
                {
                    _orderService.MakeInvalidDeliveryDateFormat(response);
                    return response;
                }

                UpdateTeleOrderRequest(request); // To update request from mobile, Reason : not passing expected values from mobile

                TeleCustomer cons = new TeleCustomer();
                cons.Address = request.customer_details.customer_address;
                cons.CustomerName = request.customer_details.customer_name;
                cons.MobileNumber = request.customer_details.customer_mobile;
                cons.StatusId = true;
                //cons.Latitude = request.customer_details.latitude;
                //cons.Longitude = request.customer_details.longitude;
                cons.CreatedBy = admin.AgadmID;
                cons.CreatedDate = DateTime.Now;

                TeleOrder ord = new TeleOrder();
                PopulateOrder(ord, cons.TeleCustID, request, admin);
                AddProductsToOrder(ord, request.products);

                ord.DrvrID = request.driver_id;
                ord.DeliverySlotID = (short)request.time_slot_id;
                //ord.DeliveredDate = null;

                using (TeleOrderDao odao = new TeleOrderDao())
                {
                    if (request.has_exchange)
                    {
                        if (request.exchange == null)
                        {
                            _orderService.MakeInvalidExchangeInputResponse(response);
                            return response;
                        }

                        foreach (ExchangeDto exdto in request.exchange)
                        {
                            TeleOrderPrdocuctExchange opex = new TeleOrderPrdocuctExchange();
                            ProductExchange pxg = odao.FindProductExchangeById(exdto.exchange_id);
                            if (pxg == null)
                            {
                                _orderService.MakeInvalidExchangeInputResponse(response);
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
                            ord.TeleOrderPrdocuctExchanges.Add(opex);
                            ord.ShippingCharge += (product.shipping_cost * exdto.exchange_quantity);
                            ord.PromoShipping += (product.shipping_promo * exdto.exchange_quantity);
                            ord.NumberOfProducts += exdto.exchange_quantity;
                            ord.ExchangeSubTotal += opex.SubTotal;
                            ord.PromoExchange += opex.ExchangePromoPrice;
                            ord.GrantTotal += opex.TotalAmount;
                        }
                    }
                    //cons.TeleOrder = ord;
                    odao.Insert(ord);
                    cons.TeleOrdID = ord.TeleOrdID;
                    using (TeleOrderCustomerDao tcDao = new TeleOrderCustomerDao())
                    {
                        tcDao.Insert(cons);
                    }
                    ord = odao.FindById(ord.TeleOrdID, true);
                    TeleOrderHelper.CopyFromEntity(response, ord);
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("add.tele.order");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public static void UpdateTeleOrderRequest(AddTeleOrderRequest request)
        {
            // To update request from mobile, Reason : not passing expected values from mobile
            foreach (var item in request.products)
            {
                using (ProductDao prodDao = new ProductDao())
                {
                    Product product = prodDao.FindProductById(item.product_id);
                    OrdersServices.UpdateProductForReq(item, product,false);
                    if (request.has_exchange)
                    {
                        OrdersServices.UpdateProductExchangeForReq(item, product, request.exchange.ToList(), false);
                    }
                }
            }
        }

        public ConfirmTeleOrderResponse ConfirmTeleOrder(ConfirmTeleOrderRequest request)
        {
            ConfirmTeleOrderResponse response = new ConfirmTeleOrderResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    TeleOrder order = dao.FindById(request.order_id, true);
                    if (order == null)
                    {
                        MakeNoTeleOrderFoundResponse(response);
                        return response;
                    }
                    order.StatusId = OrdersServices.ID_ORDER_ACCEPTED;//2;
                    order.DrvrID = request.driver_id;
                    //Driver drv = order.Driver;
                    //int agId = drv.AgenID;
                    Driver drv = null;
                    using (DriverDao ddao = new DriverDao())
                    {
                        drv = ddao.FindById(request.driver_id);
                        if (drv == null)
                        {
                            DriverServices.MakeNoDriverResponse(response);
                            return response;
                        }
                        TeleOrderDelivery odel = new TeleOrderDelivery();
                        odel.DrvrID = drv.DrvrID;
                        odel.AgadmID = request.user_id;
                        odel.CreatedDate = DateTime.Now;
                        odel.DeliveryDate = order.DeliveryDate;
                        odel.AcceptedDate = DateTime.Now;
                        odel.StatusId = OrdersServices.DELIVERY_STATUS_ASSIGNED;//1;
                        odel.TeleOrder = order;
                        order.TeleOrderDeliveries.Add(odel);
                    }
                    lock (InvoiceService.monitor)
                    {
                        string invNo = InvoiceService.GenerateInvoiceNumber(drv.AgenID);
                        order.InvoiceNumber = invNo;
                        dao.Update(order);
                    }
                    TeleOrderHelper.CopyFromEntity(response, order);
                    using (ConsumerReviewDao conReviewDao = new ConsumerReviewDao())
                    {
                        List<ConsumerReview> conReview = new List<ConsumerReview>();
                        conReview = conReviewDao.GetReviewByDriver(request.driver_id);
                        response.orders.driver.driver_rating = conReview.Count > 0 ? Convert.ToDecimal(conReview.Average(x => x.Rating)) : 0;
                    }

                    if (order.DeliveryDate.HasValue && order.DeliveryDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        int orderCount = dao.GetAssignedOrderCount(request.driver_id, OrdersServices.ID_ORDER_ACCEPTED);                       
                        using (OrderDao ordDao = new OrderDao())
                        {
                            orderCount += ordDao.GetAssignedOrderCount(request.driver_id, OrdersServices.ID_ORDER_ACCEPTED);
                        }
                        _orderService.ReadAndSendPushNotification(OrdersServices.APPSETTING_MSG_TO_ASSIGNED_DRIVER, OrdersServices.APPSETTING_TITLE_FOR_ASSIGNED_DRIVER, drv.AppToken, request.order_id, request.driver_id, orderCount, PushMessagingService.APPSETTING_APPLICATION_ID_DRIVER, PushMessagingService.APPSETTING_SENDER_ID_DRIVER, (int)PushMessagingService.PushType.TypeOne);
                    }

                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("cnfrm.tele.order");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        private void PopulateOrder(TeleOrder ord, int teleCustID, AddTeleOrderRequest request, AgentAdmin admin)
        {
            ord.StatusId = OrdersServices.ID_ORDER_ACTIVE;// 1;
            ord.OrderTime = DateTime.Now.TimeOfDay;
            ord.OrderDate = DateTime.Now.Date;
            ord.InvoiceNumber = "";
            ord.CreatedDate = DateTime.Now;
            ord.UpdatedDate = ord.CreatedDate;
            ord.DeliveryDate = request.delivery_date;
            ord.DeliveryType = true; //0 for pickup 1 for telp
            ord.AgadmID = admin.AgadmID;
            ord.CreatedBy = admin.AgadmID;
            ord.UpdatedBy = admin.AgadmID;
        }

        public void MakeNoTeleOrderFoundResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("no.tele.order");
        }

        private void AddProductsToOrder(TeleOrder ord, ProductsDto[] products)
        {
            if (ord.ShippingCharge == null)
                ord.ShippingCharge = 0;
            if (ord.PromoShipping == null)
                ord.PromoShipping = 0;
            if (ord.PromoProduct == null)
                ord.PromoProduct = 0;

            foreach (var prd in products)
            {
                TeleOrderDetail od = new TeleOrderDetail();
                od.TeleOrder = ord;
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
                    od.TotalAmount = od.SubTotal + prd.product_promo + (prd.shipping_promo * prd.quantity) + (prd.shipping_cost * prd.quantity); //prd.sub_total;
                }
                if (prd.refill_quantity > 0)
                {
                    ord.ShippingCharge += (prd.shipping_cost * prd.quantity);
                    ord.PromoShipping += (prd.shipping_promo * prd.quantity);
                    od.RefillTotalAmount = od.RefillSubTotal + prd.refill_promo + (prd.shipping_promo * prd.refill_quantity) + (prd.shipping_cost * prd.refill_quantity); //prd.sub_total;
                }
                ord.SubTotal += od.SubTotal;
                ord.RefillSubTotal += od.RefillSubTotal;
                ord.PromoProduct += od.PromoProduct.Value;
                ord.PromoRefill += od.PromoRefill;
                ord.GrantTotal += (od.TotalAmount + od.RefillTotalAmount); //prd.sub_total;
                ord.NumberOfProducts = prd.quantity + prd.refill_quantity;
                ord.TeleOrderDetails.Add(od);


                //TeleOrderDetail od = new TeleOrderDetail();
                //od.TeleOrder = ord;
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
                //ord.TeleOrderDetails.Add(od);
                //ord.GrantTotal += (prd.unit_price - prd.product_promo + prd.shipping_cost - prd.shipping_promo) * prd.quantity;
            }
        }
    }
}
