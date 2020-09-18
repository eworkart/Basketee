using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.OrderPickup;
using Basketee.API.DTOs.Promo;
using Basketee.API.Models;
using Basketee.API.DAOs;
using Basketee.API.Services.Helpers;
using Basketee.API.ServicesLib.Services.Helpers;
using Basketee.API.DTOs.Orders;
using Basketee.API.DTOs;

namespace Basketee.API.Services
{
    public class OrderPickupServices
    {
        private UserServices _userServices = new UserServices();
        private OrdersServices _ordersServices = new OrdersServices();

        public PlaceOrderPickupResponse PlacePickupOrder(PlaceOrderPickupRequest request)
        {
            PlaceOrderPickupResponse response = new PlaceOrderPickupResponse();
            try
            {
                AgentAdmin admin = AgentAdminServices.GetAuthAdmin(request.user_id, request.auth_token, response);
                if (admin == null)
                {
                    return response;
                }

                UpdatePickupOrderRequest(request); // To update request from mobile, Reason : not passing expected values from mobile

                TeleOrder ord = new TeleOrder();
                PopulateOrder(ord);
                ord.AgadmID = admin.AgadmID;
                ord.CreatedBy = admin.AgadmID;
                ord.UpdatedBy = admin.AgadmID;
                AddProductsToOrder(ord, request.products);
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    if (request.has_exchange)
                    {
                        if (request.exchange == null)
                        {
                            MakeInvalidExchangeInputResponse(response);
                            return response;
                        }
                        foreach (ExchangeDto item in request.exchange)
                        {
                            ProductExchange pxg = dao.FindProductExchangeById(item.exchange_id);
                            TeleOrderPrdocuctExchange opex = new TeleOrderPrdocuctExchange();
                            if (pxg == null)
                            {
                                MakeInvalidExchangeInputResponse(response);
                                return response;
                            }
                            opex.ProdID = pxg.ProdID;
                            opex.ExchangePrice = item.exchange_price;
                            opex.CreatedDate = DateTime.Now;
                            opex.ExchangePromoPrice = item.exchange_promo_price * item.exchange_quantity;
                            opex.ExchangeQuantity = item.exchange_quantity;
                            opex.ExchangeWith = item.exchange_with;
                            opex.StatusId = true;//TODO
                            //ord.GrantTotal -= (opex.ExchangePrice - opex.ExchangePromoPrice) * opex.ExchangeQuantity;
                            //opex.TeleOrder = ord;
                            opex.SubTotal = item.exchange_price * item.exchange_quantity;
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
                            opex.TotalAmount = opex.SubTotal + opex.ExchangePromoPrice;
                            ord.TeleOrderPrdocuctExchanges.Add(opex);
                            //ord.ShippingCharge += (product.shipping_cost * item.exchange_quantity);
                            //ord.PromoShipping += (product.shipping_promo * item.exchange_quantity);
                            ord.NumberOfProducts += item.exchange_quantity;
                            ord.ExchangeSubTotal += opex.SubTotal;
                            ord.PromoExchange += opex.ExchangePromoPrice;
                            ord.GrantTotal += opex.TotalAmount;
                        }
                    }
                    int agId = admin.AgenID;
                    lock (InvoiceService.monitor)
                    {
                        ord.InvoiceNumber = InvoiceService.GenerateInvoiceNumber(agId);
                        dao.Insert(ord);
                    }

                    OrderPickupHelper.CopyFromEntity(response, ord);
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("place.pickup.order");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public static void UpdatePickupOrderRequest(PlaceOrderPickupRequest request)
        {
            // To update request from mobile, Reason : not passing expected values from mobile
            foreach (var item in request.products)
            {
                using (ProductDao prodDao = new ProductDao())
                {
                    Product product = prodDao.FindProductById(item.product_id);
                    OrdersServices.UpdateProductForReq(item, product,true);
                    if (request.has_exchange)
                    {
                        OrdersServices.UpdateProductExchangeForReq(item, product, request.exchange, true);
                    }
                }
            }
        }

        public ConfirmPickupOrderResponse ConfirmPickupOrder(ConfirmPickupOrderRequest request)
        {
            ConfirmPickupOrderResponse response = new ConfirmPickupOrderResponse();
            try
            {
                if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                using (TeleOrderDao dao = new TeleOrderDao())
                {
                    TeleOrder order = dao.FindById(request.order_id, true);
                    order.StatusId = OrdersServices.ID_ORDER_CLOSED;
                    string invNo = InvoiceService.GenerateInvoiceNumber(order.AgentAdmin.AgenID);
                    order.InvoiceNumber = invNo;
                    dao.Update(order);
                    dao.Update(order);
                    OrderPickupHelper.CopyFromEntity(response, order);

                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("cnfrm.pickup.order");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        private static void PopulateOrder(TeleOrder ord)
        {
            ord.StatusId = OrdersServices.ID_ORDER_ACCEPTED;//2;
            ord.OrderTime = DateTime.Now.TimeOfDay;
            ord.OrderDate = DateTime.Now.Date;
            ord.InvoiceNumber = "";
            ord.CreatedDate = DateTime.Now;
            ord.UpdatedDate = ord.CreatedDate;
            ord.DeliveryDate = null;
            ord.DeliveryType = false; //0 for pickup 1 for telp
        }

        private static void AddProductsToOrder(TeleOrder ord, ProductsDto[] dtos)
        {           
            if (ord.PromoProduct == null)
                ord.PromoProduct = 0;

            foreach (var prd in dtos)
            {
                TeleOrderDetail od = new TeleOrderDetail();

                od.TeleOrder = ord;
                od.ProdID = prd.product_id;
                od.CreatedDate = ord.CreatedDate;

                od.Quantity = prd.quantity;
                od.UnitPrice = prd.unit_price;
                od.SubTotal = prd.unit_price * prd.quantity; //prd.sub_total;
                od.PromoProduct = prd.product_promo;                

                od.RefillQuantity = prd.refill_quantity;
                od.RefillPrice = prd.refill_price;
                od.PromoRefill = prd.refill_promo;
                od.RefillSubTotal = prd.refill_price * prd.refill_quantity;

                if (prd.quantity > 0)
                {
                    //ord.ShippingCharge += prd.shipping_cost;
                    //ord.PromoShipping += prd.shipping_promo;
                    od.TotalAmount = od.SubTotal + prd.product_promo ; //prd.sub_total;
                }
                if (prd.refill_quantity > 0)
                {
                    //ord.ShippingCharge += prd.shipping_cost;
                   // ord.PromoShipping += prd.shipping_promo;
                    od.RefillTotalAmount = od.RefillSubTotal + prd.refill_promo; //prd.sub_total;
                }
                ord.SubTotal += od.SubTotal;
                ord.RefillSubTotal += od.RefillSubTotal;
                ord.PromoProduct += od.PromoProduct.Value;
                ord.PromoRefill += od.PromoRefill;
                ord.GrantTotal += (od.TotalAmount + od.RefillTotalAmount); //prd.sub_total;
                ord.NumberOfProducts = prd.quantity + prd.refill_quantity;
                ord.TeleOrderDetails.Add(od);


                //od.TeleOrder = ord;
                //od.ProdID = prd.product_id;
                //od.CreatedDate = ord.CreatedDate;
                //od.PromoProduct = prd.product_promo;
                //od.PromoShipping = prd.shipping_promo;
                //od.ShippingCharge = prd.shipping_cost;
                //od.SubTotal = prd.sub_total;
                //od.Quantity = prd.quantity;
                //od.UnitPrice = prd.unit_price;
                //od.RefillPrice = prd.refill_price;
                //od.PromoRefill = prd.refill_promo;
                //od.RefillQuantity = prd.refill_quantity;

                //ord.SubTotal += prd.sub_total;
                //ord.ShippingCharge += prd.shipping_cost;
                //ord.PromoProduct += prd.product_promo;
                //ord.PromoShipping += prd.shipping_promo;
                //ord.TeleOrderDetails.Add(od);
                //ord.GrantTotal += prd.sub_total;

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

        public static void MakeInvalidExchangeInputResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("invalid.exchange.input");
        }
    }
}
