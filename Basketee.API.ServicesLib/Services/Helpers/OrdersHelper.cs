using System.Collections.Generic;
using System.Linq;
using Basketee.API.DTOs.Orders;
using Basketee.API.Models;
using System.Globalization;
using System;

namespace Basketee.API.Services.Helpers

{
    public class OrdersHelper
    {

        public static void CopyFromEntity(DriverDetailsDto dto, Driver driver)
        {
            dto.driver_id = driver.DrvrID;
            dto.driver_image = ImagePathService.driverImagePath + driver.ProfileImage;
            dto.driver_mobile = driver.MobileNumber;
            dto.driver_name = driver.DriverName;
            // dto.driver_rating = driver.Rating;
        }

        public static void CopyFromEntity(OrderInvoiceDto dto, Order order)
        {
            dto.order_id = order.OrdrID;
            dto.invoice_number = order.InvoiceNumber;
            dto.consumer_name = order.Consumer.Name;
            dto.consumer_mobile = order.Consumer.PhoneNumber;
            dto.consumer_address = order.ConsumerAddress.Address;
            dto.consumer_location = order.ConsumerAddress.RegionName;
            dto.postal_code = order.ConsumerAddress.PostalCode;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime;
            List<ProductsDto> pdtos = new List<ProductsDto>();
            decimal sumPromoProduct = 0;
            decimal sumPromoShipping = 0;
            decimal sumPromoRefill = 0;
            decimal sumPromoExchange = 0;
            foreach (OrderDetail det in order.OrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge;
                pdt.shipping_promo = det.PromoShipping;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdtos.Add(pdt);

                if (det.Quantity > 0)
                {
                    sumPromoShipping += (det.PromoShipping * det.Quantity);
                }
                if (det.RefillQuantity > 0)
                {
                    sumPromoShipping += (det.PromoShipping * det.RefillQuantity);
                }
                sumPromoProduct += det.PromoProduct;
                sumPromoRefill += det.PromoRefill;
                dto.has_exchange = (order.OrderPrdocuctExchanges.Count > 0 ? 1 : 0);
                if (dto.has_exchange == 1)
                {
                    if (dto.exchange == null)
                        dto.exchange = new List<ExchangeDto>();
                    foreach (var item in order.OrderPrdocuctExchanges)
                    {
                        if (item.ProdID == det.ProdID)
                        {
                            ExchangeDto exDto = new ExchangeDto();
                            CopyFromEntity(exDto, item);
                            dto.exchange.Add(exDto);
                            sumPromoExchange += item.ExchangePromoPrice;
                            sumPromoShipping += (det.PromoShipping * item.ExchangeQuantity);
                        }
                    }
                }
            }
            if (order.AgentAdmin != null)
            {
                dto.agency_id = order.AgentAdmin.Agency.AgenID;
                dto.agency_name = order.AgentAdmin.Agency.AgencyName;
                dto.agency_address = order.AgentAdmin.Agency.MRegion.RegionName;
                dto.agency_location = order.AgentAdmin.Agency.MRegion.RegionName;
                //dto.agency_address = order.AgentAdmin.Agency.Region;
                //dto.agency_location = order.AgentAdmin.Agency.Region;
            }

            dto.grand_total = order.GrandTotal;
            dto.grand_discount = (Math.Abs(sumPromoProduct) + Math.Abs(sumPromoShipping) + Math.Abs(sumPromoRefill) + Math.Abs(sumPromoExchange)) * -1;
            dto.grand_total_with_discount = dto.grand_total + Math.Abs(dto.grand_discount);
            dto.products = pdtos.ToArray();
        }

        public static void CopyFromEntity(OrderFullDetailsDto dto, Order order, Driver drv = null)
        {
            dto.order_id = order.OrdrID;
            dto.delivery_date = Common.ToDateFormat(order.DeliveryDate);
            dto.delivery_time_slot = order.MDeliverySlot.SlotName;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_status = order.StatusID;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.consumer_name = order.Consumer.Name;
            dto.consumer_mobile = order.Consumer.PhoneNumber;
            dto.consumer_address = order.ConsumerAddress.Address;
            dto.consumer_location = order.ConsumerAddress.RegionName;

            dto.driver = new DriverDetailsDto();
            if (order.OrderDeliveries.Count > 0)
            {
                OrderDelivery od = order.OrderDeliveries.First();
                dto.driver = new DriverDetailsDto();
                if (drv == null)
                {
                    drv = od.Driver;
                }
                CopyFromEntity(dto.driver, drv);
                //dto.driver.driver_id = od.Driver.DrvrID;
                //dto.driver.driver_image = od.Driver.ProfileImage;
                //dto.driver.driver_mobile = od.Driver.MobileNumber;
                //dto.driver.driver_name = od.Driver.DriverName;
                ////dto.driver.driver_rating = od.Driver.ra

            }

            List<ProductsDto> pdtos = new List<ProductsDto>();
            foreach (OrderDetail det in order.OrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge;
                pdt.shipping_promo = det.PromoShipping;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdtos.Add(pdt);
            }
            dto.grand_total = order.GrandTotal;
            dto.products = pdtos.ToArray();

            dto.has_exchange = (order.OrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (dto.has_exchange == 1)
            {
                if (dto.exchange == null)
                    dto.exchange = new List<ExchangeDto>();
                foreach (var item in order.OrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    OrdersHelper.CopyFromEntity(exDto, item);
                    dto.exchange.Add(exDto);
                }
            }
        }

        public static void CopyFromEntity(OrderFullDetailsDto dto, TeleOrder teleOrder, Driver drv = null)
        {
            dto.order_id = teleOrder.TeleOrdID;
            dto.delivery_date = Common.ToDateFormat(teleOrder.DeliveryDate.HasValue ? teleOrder.DeliveryDate.Value : DateTime.MinValue);
            dto.delivery_time_slot = teleOrder.MDeliverySlot.SlotName;
            dto.invoice_number = teleOrder.InvoiceNumber;
            dto.order_status = teleOrder.StatusId;
            if (teleOrder.TeleCustomers.Count > 0)
            {
                var teleConsumer = teleOrder.TeleCustomers.Where(x => x.TeleOrdID == teleOrder.TeleOrdID).FirstOrDefault();
                if (teleConsumer != null)
                {
                    dto.consumer_name = teleConsumer.CustomerName;
                    dto.consumer_mobile = teleConsumer.MobileNumber;
                    dto.consumer_address = teleConsumer.Address;
                    dto.consumer_location = teleConsumer.Address;
                    //dto.latitude = teleConsumer.Latitude;
                    //dto.longitude = teleConsumer.Longitude;
                }
            }

            dto.driver = new DriverDetailsDto();
            if (teleOrder.TeleOrderDetails.Count > 0)
            {
                TeleOrderDetail od = teleOrder.TeleOrderDetails.First();
                dto.driver = new DriverDetailsDto();
                if (drv == null)
                {
                    drv = teleOrder.Driver;
                }
                CopyFromEntity(dto.driver, drv);
                //dto.driver.driver_id = od.Driver.DrvrID;
                //dto.driver.driver_image = od.Driver.ProfileImage;
                //dto.driver.driver_mobile = od.Driver.MobileNumber;
                //dto.driver.driver_name = od.Driver.DriverName;
                ////dto.driver.driver_rating = od.Driver.ra

            }

            List<ProductsDto> pdtos = new List<ProductsDto>();
            foreach (TeleOrderDetail det in teleOrder.TeleOrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct.HasValue ? det.PromoProduct.Value : 0;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge.HasValue ? det.ShippingCharge.Value : 0;
                pdt.shipping_promo = det.PromoShipping.HasValue ? det.PromoShipping.Value : 0;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdtos.Add(pdt);
            }
            dto.grand_total = teleOrder.GrantTotal;
            dto.products = pdtos.ToArray();
            dto.has_exchange = (teleOrder.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (dto.has_exchange == 1)
            {
                if (dto.exchange == null)
                    dto.exchange = new List<ExchangeDto>();
                foreach (var item in teleOrder.TeleOrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    TeleOrderHelper.CopyFromEntity(exDto, item);
                    dto.exchange.Add(exDto);
                }
            }
        }

        //public static void CopyFromEntity(OrgOrderListDto dto, Order order)
        //{
        //    dto.order_id = order.OrdrID;
        //    dto.invoice_number = order.InvoiceNumber;
        //    dto.consumer_name = order.Consumer.Name;
        //    dto.consumer_mobile = order.Consumer.PhoneNumber;
        //    dto.consumer_address = order.ConsumerAddress.Address;
        //    dto.order_date = order.OrderDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    dto.order_time = order.OrderTime.ToString("t", CultureInfo.InvariantCulture);
        //    //dto.order_type = order.
        //    dto.grand_total = order.GrandTotal;
        //}

        public static void CopyToEntity(ConsumerReview review, SubmitReviewRequest request)
        {
            review.ConsID = request.user_id; // auth_token
            review.Rating = request.rating;
            review.ReasonID = request.reason_id;
            review.Comments = request.comments;
        }
        //public static void CopyToEntity(Order order, GetTimeslotRequest request)
        //{
        //    order.OrdrID = request.user_id; // auth_token
        //}
        //public static void CopyFromEntity(GetTimeSlotResponse response, MDeliverySlot order)
        //{
        //    if (response.time_slot == null)
        //    {
        //        response.time_slot = new TimeSlotDto();
        //    }
        //    response.time_slot.time_slot_id = order.SlotID; // date, availability
        //    response.time_slot.time_slot_name = order.SlotName;
        //}
        //public static void CopyToEntity(Order order, OrderDetail ord_det, PlaceOrderRequest request)
        //{
        //    order.OrdrID = request.user_id; // auth_token
        //    order.AddrID = request.address_id;
        //    order.DeliveryDate = request.delivery_date;
        //    order.DeliverySlotID = request.time_slot_id;
        //    ord_det.OrdtID = request.products.product_id;
        //    ord_det.Quantity = request.products.quantity;
        //    ord_det.UnitPrice = request.products.unit_price;
        //    ord_det.SubTotal = request.products.sub_total;
        //    ord_det.PromoProduct = request.products.product_promo;
        //    ord_det.ShippingCharge = request.products.shipping_cost;
        //    ord_det.PromoShipping = request.products.shipping_promo;
        //}
        //public static void CopyFromEntity(PlaceOrderResponse response, MOrderStatu order_status, Order order)
        //{
        //    if (response.order_details == null)
        //    {
        //        response.order_details = new OrderDto();
        //    }
        //    response.order_details.order_id = order.OrdrID;
        //    response.order_details.invoice_number = order.InvoiceNumber;
        //    response.order_details.order_status = order_status.OrderStatus;
        //}

        public static void CopyFromEntity(OrderDto dto, Order order)
        {
            dto.order_id = order.OrdrID;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime;
            dto.grand_total = order.GrandTotal;
        }
        public static void CopyFromEntity(OrderDetailsDto dto, Order order, string agentAdminMob, bool withDetails)
        {
            dto.order_id = order.OrdrID;
            dto.invoice_number = order.InvoiceNumber; // driver_details : driver_name, driver_image
            dto.order_date = Common.ToDateFormat(order.OrderDate); // product_details : product_name, quantity, unit_price, sub_total, product_promo, shipping_cost, shipping_promo
            dto.order_time = order.OrderTime;
            dto.delivery_date = Common.ToDateFormat(order.DeliveryDate);
            dto.time_slot_name = order.MDeliverySlot.SlotName;
            dto.grand_total = order.GrandTotal;
            dto.order_status = order.MOrderStatu.OrderStatus;
            dto.agentadmin_mobile = agentAdminMob;

            OrderDelivery odel = order.OrderDeliveries.Count > 0 ? order.OrderDeliveries.First() : null;
            if (odel != null)
            {
                dto.delivery_status = odel.MDeliveryStatu.DeliveryStatus;
                if (odel.Driver != null)
                {
                    DriverDetailsDto drvDto = new DriverDetailsDto();
                    dto.driver_details = drvDto;
                    CopyFromEntity(drvDto, odel.Driver);
                    //drvDto.driver_name = odel.Driver.DriverName;
                    //drvDto.driver_image = odel.Driver.ProfileImage;
                }
            }
            if (order.OrderDetails.Count > 0)
            {
                List<OrderDetail> odetLst = order.OrderDetails.ToList();
                dto.product_details = new ProductsDto[odetLst.Count];
                for (int i = 0; i < odetLst.Count; i++)
                {
                    ProductsDto prodDto = new ProductsDto();
                    CopyFromEntity(prodDto, odetLst[i]);
                    dto.product_details[i] = prodDto;
                }
            }
            dto.has_exchange = (order.OrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (dto.has_exchange == 1)
            {
                if (dto.exchange == null)
                    dto.exchange = new List<ExchangeDto>();
                foreach (var item in order.OrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    CopyFromEntity(exDto, item);
                    dto.exchange.Add(exDto);
                }
            }
        }

        public static void CopyFromEntity(ProductsDto dto, OrderDetail odet)
        {
            dto.product_id = odet.ProdID;
            dto.product_name = odet.Product != null ? odet.Product.ProductName : string.Empty;
            dto.product_promo = odet.PromoProduct;
            dto.quantity = odet.Quantity;
            dto.shipping_cost = odet.ShippingCharge;
            dto.shipping_promo = odet.PromoShipping;
            dto.sub_total = odet.SubTotal;
            dto.unit_price = odet.UnitPrice;
            dto.refill_price = odet.RefillPrice;
            dto.refill_promo = odet.PromoRefill;
            dto.refill_quantity = odet.RefillQuantity;
        }

        //public static void CopyToEntity(Order order, CancelOrderRequest request)
        //{
        //    order.ConsID = request.user_id; // auth_token
        //    order.OrdrID = request.order_id;
        //}

        public static void CopyToEntity(Order order, CloseOrderRequest request)
        {
            order.ConsID = request.user_id; // auth_token
            order.OrdrID = request.order_id;
        }
        public static void CopyToEntity(Order order, GetAssignedOrderCountRequest request)
        {
            order.ConsID = request.user_id; // auth_token
        }
        public static void CopyFromEntity(GetAssignedOrderCountResponse dto, Order odet)
        {
            //dto.assigned_order_count = odet.;
        }
        public static void CopyToEntity(Order order, GetDriverOrderListRequest request)
        {
            order.ConsID = request.user_id; // auth_token, current_list
        }
        public static void CopyToEntity(Order order, GetDriverOrderRequest request)
        {
            order.ConsID = request.user_id; // auth_token
            order.OrdrID = request.order_id;
        }
        public static void CopyFromEntity(DriverOrderDetailsDto odt, Order order)
        {
            odt.order_id = order.OrdrID;
            odt.order_type = OrdersServices.OrderType.OrderApp.ToString(); //"normal";
            odt.invoice_number = order.InvoiceNumber;
            odt.order_date = Common.ToDateFormat(order.OrderDate);
            odt.order_time = order.OrderTime.ToString();
            odt.consumer_address = order.ConsumerAddress.Address;
            odt.consumer_mobile = order.Consumer.PhoneNumber;
            odt.consumer_name = order.Consumer.Name;
            odt.grand_total = order.GrandTotal;
            odt.delivery_timeslot_id = order.DeliverySlotID;
        }
        public static void CopyFromEntity(DriverOrderDetailsDto odt, TeleOrder order)
        {
            odt.order_id = order.TeleOrdID;
            odt.order_type = OrdersServices.OrderType.OrderTelp.ToString();
            odt.invoice_number = order.InvoiceNumber;
            odt.order_date = Common.ToDateFormat(order.OrderDate);
            odt.order_time = order.OrderTime.ToString();
            if (order.TeleCustomers.Count > 0)
            {
                TeleCustomer cust = order.TeleCustomers.First();
                odt.consumer_address = cust.Address;
                odt.consumer_mobile = cust.MobileNumber;
                odt.consumer_name = cust.CustomerName;
            }
            odt.grand_total = order.GrantTotal;
            odt.delivery_timeslot_id = order.DeliverySlotID ?? 0;
        }
        public static void CopyFromEntity(DriverOrderDetailDto dto, Order order)
        {
            dto.order_id = order.OrdrID; // order_type, latitude, longitude
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime.ToString(@"hh\:mm");
            dto.consumer_name = order.Consumer.Name;
            dto.consumer_mobile = order.Consumer.PhoneNumber;
            dto.consumer_address = order.ConsumerAddress.Address;
            dto.latitude = order.ConsumerAddress.Latitude;
            dto.longitude = order.ConsumerAddress.Longitude;
            dto.delivery_timeslot_id = order.DeliverySlotID;
            dto.grand_total = order.GrandTotal;
            dto.delivery_status_id = order.OrderDeliveries.Count > 0 ? order.OrderDeliveries.Where(x => x.OrdrID == order.OrdrID).FirstOrDefault().StatusId : 0;
            dto.order_status_id = order.StatusID;
            dto.agent_admin_phone_no = order.AgentAdmin != null ? order.AgentAdmin.MobileNumber : string.Empty;
        }
        public static DriverProductsDto[] CopyFromEntity(Order order)
        {
            List<DriverProductsDto> pdtos = new List<DriverProductsDto>();
            foreach (OrderDetail det in order.OrderDetails)
            {
                DriverProductsDto pdt = new DriverProductsDto();
                pdt.product_id = det.Product.ProdID;  // grand_total, delivery_status_id, delivery_timeslot_id
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge;
                pdt.shipping_promo = det.PromoShipping;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdtos.Add(pdt);
            }
            return pdtos.ToArray();
        }

        public static OrderDetailsBossDto CopyFromEntity(OrderDetailsBossDto dto, GetAllOrdersByAgentBoss_Result order)
        {
            if (dto == null)
                dto = new OrderDetailsBossDto();

            dto.order_id = order.OrdrID;
            dto.order_type = order.OrderType;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime.ToString(@"hh\:mm");
            dto.consumer_name = order.ConsumerName;
            dto.consumer_mobile = order.ConsumerMobile;
            dto.consumer_address = order.ConsumerAddress;
            dto.grand_total = order.GrandTotal;

            return dto;
        }

        public static void CopyFromEntity(OrderFullDetailsBossDto dto, Order order, Driver drv = null)
        {
            dto.order_id = order.OrdrID;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.delivery_date = Common.ToDateFormat(order.DeliveryDate);
            if (!(order.MDeliverySlot == null))
            {
                dto.time_slot_id = order.MDeliverySlot.SlotID;
                dto.time_slot_name = order.MDeliverySlot.SlotName;
            }
            dto.invoice_number = order.InvoiceNumber;
            dto.order_status = order.StatusID;
            dto.consumer_name = order.Consumer?.Name;
            dto.consumer_mobile = order.Consumer?.PhoneNumber;
            dto.consumer_address = order.ConsumerAddress?.Address;
            dto.grand_total = order.GrandTotal;
            if (order.OrderDeliveries != null && order.OrderDeliveries.Count > 0)
            {
                OrderDelivery od = order.OrderDeliveries.First();
                dto.driver_details = new DriverDetailsBossDto();
                drv = od.Driver;
                if (drv != null)
                {
                    CopyFromEntity(dto.driver_details, drv);
                    dto.driver_details.delivery_status = od.StatusId;
                }
            }

            List<ProductsBossDto> pdtos = new List<ProductsBossDto>();
            foreach (OrderDetail det in order.OrderDetails)
            {
                ProductsBossDto pdt = new ProductsBossDto();
                pdt.product_name = det.Product?.ProductName;
                pdt.product_promo = det.PromoProduct;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge;
                pdt.shipping_promo = det.PromoShipping;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdt.grand_total = det.TotamAmount;
                pdtos.Add(pdt);
            }
            dto.product_details = pdtos;
            dto.has_exchange = (order.OrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (dto.has_exchange == 1)
            {
                if (dto.exchange == null)
                    dto.exchange = new List<ExchangeDto>();
                foreach (var item in order.OrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    CopyFromEntity(exDto, item);
                    dto.exchange.Add(exDto);
                }
            }
        }

        public static void CopyFromEntity(OrderFullDetailsBossDto dto, TeleOrder order, Driver drv = null)
        {
            dto.order_id = order.TeleOrdID;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.delivery_date = Common.ToDateFormat(order.DeliveryDate.Value);

            //dto.delivery_date = order.DeliveryDate.ToDateTime();
            if (!(order.MDeliverySlot == null))
            {
                dto.time_slot_id = order.MDeliverySlot.SlotID;
                dto.time_slot_name = order.MDeliverySlot.SlotName;
            }
            dto.invoice_number = order.InvoiceNumber;
            dto.order_status = order.StatusId;
            dto.consumer_name = order.TeleCustomers?.FirstOrDefault()?.CustomerName;
            dto.consumer_mobile = order.TeleCustomers?.FirstOrDefault()?.MobileNumber;
            dto.consumer_address = order.TeleCustomers?.FirstOrDefault()?.Address;
            dto.grand_total = order.GrantTotal;
            if (drv != null)
            {
                dto.driver_details = new DriverDetailsBossDto();
                CopyFromEntity(dto.driver_details, drv);
                if (!(order.MOrderStatu == null))
                    dto.driver_details.delivery_status = order.MOrderStatu.OrstID.ToInt();
            }

            List<ProductsBossDto> pdtos = new List<ProductsBossDto>();
            foreach (TeleOrderDetail det in order.TeleOrderDetails)
            {
                ProductsBossDto pdt = new ProductsBossDto();
                pdt.product_name = det.Product?.ProductName;
                pdt.product_promo = det.PromoProduct.ToDecimal();
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge.ToDecimal();
                pdt.shipping_promo = det.PromoShipping.ToDecimal();
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.grand_total = det.TotalAmount;
                pdtos.Add(pdt);
            }
            dto.product_details = pdtos;
            dto.has_exchange = (order.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (dto.has_exchange == 1)
            {
                if (dto.exchange == null)
                    dto.exchange = new List<ExchangeDto>();
                foreach (var item in order.TeleOrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    TeleOrderHelper.CopyFromEntity(exDto, item);
                    dto.exchange.Add(exDto);
                }
            }
        }

        public static void CopyFromEntity(DriverDetailsBossDto dto, Driver driver)
        {
            dto.driver_id = driver.DrvrID;
            dto.driver_profile_image = ImagePathService.driverImagePath + driver.ProfileImage;
            dto.driver_mobile = driver.MobileNumber;
            dto.driver_name = driver.DriverName;

            if (!(driver.ConsumerReviews == null))

                //if (driver.ConsumerReviews.Count > 0)
                if (!(driver.ConsumerReviews == null) && driver.ConsumerReviews.Count > 0)

                    dto.driver_rating = driver.ConsumerReviews.Average(r => r.Rating).ToDecimal();
        }

        //public static void CopyFromEntity(IssueDetailsDto dto, Order order)
        //{
        //    dto.order_id = order.OrdrID;
        //    dto.invoice_number = order.InvoiceNumber;
        //    dto.order_date = order.OrderDate.ToString("dd/MM/yyyy");
        //    dto.order_time = order.OrderTime.ToString();
        //    dto.grand_total = order.GrandTotal;

        //}

        public static OrderDetailsBossDto CopyFromEntity(OrderDetailsBossDto dto, GetAllOrdersByAgentAdmin_Result order)
        {
            if (dto == null)
                dto = new OrderDetailsBossDto();

            dto.order_id = order.OrdrID;
            dto.order_type = order.OrderType;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime.ToString(@"hh\:mm");
            dto.consumer_name = order.ConsumerName;
            dto.consumer_mobile = order.ConsumerMobile;
            dto.consumer_address = order.ConsumerAddress;
            dto.grand_total = order.GrandTotal;

            return dto;
        }

        public static SUserOrderDetailsDto CopyFromEntity(SUserOrderDetailsDto dto, GetOrderDeatilsbyOrderIdForSUser_Result order)
        {
            if (dto == null)
                dto = new SUserOrderDetailsDto();

            dto.order_id = order.OrdrID;
            dto.order_type = OrdersServices.OrderType.OrderApp.ToString();
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime;//.ToString(@"hh\:mm");
            dto.consumer_name = order.Name;
            dto.consumer_mobile = order.PhoneNumber;
            dto.consumer_address = order.Address;
            dto.grand_total = order.GrandTotal;
            return dto;
        }
        public static SUserProductDetailsDto CopyFromEntity(SUserProductDetailsDto dto, GetProductDeatilsbyOrderIdForSUser_Result products)
        {
            if (dto == null)
                dto = new SUserProductDetailsDto();

            dto.product_name = products.ProductName;
            dto.quantity = products.Quantity;
            dto.unit_price = products.UnitPrice;
            dto.sub_total = products.SubTotal;
            dto.product_promo = products.PromoProduct;
            dto.refill_quantity = products.RefillQuantity;
            dto.refill_price = products.RefillPrice;
            dto.refill_promo = products.PromoRefill;
            dto.shipping_cost = products.ShippingPrice;
            dto.shipping_promo = products.ShippingPromoPrice;
            dto.grand_total = products.TotamAmount;
            return dto;
        }
        public static SUserAgencyDetailsDto CopyFromEntity(SUserAgencyDetailsDto dto, GetAgencyDetailsbyOrderIdForSUser_Result agency)
        {
            if (dto == null)
            {
                dto = new SUserAgencyDetailsDto();
            }
            dto.agency_id = agency.AgenID;
            dto.agency_name = string.IsNullOrEmpty(agency.AgencyName) ? string.Empty : agency.AgencyName;
            dto.agency_mobile = string.IsNullOrEmpty(agency.MobileNumber) ? string.Empty : agency.MobileNumber;
            dto.agency_address = string.IsNullOrEmpty(agency.Address) ? string.Empty : agency.Address;
            return dto;
        }
        public static ExchangeDto CopyFromEntity(ExchangeDto dto, ProductExchange product_Exchange)
        {
            if (dto == null)
            {
                dto = new ExchangeDto();
                return dto;
            }
            if (dto.exchange_quantity > 0)
            {
                dto.exchange_id = product_Exchange.PrExID;
                dto.exchange_quantity = product_Exchange.ExchangeQuantity;
                dto.exchange_price = product_Exchange.ExchangePromoPrice;
                dto.exchange_promo_price = product_Exchange.ExchangePromoPrice;
                dto.exchange_with = product_Exchange.ExchangeWith;
            }
            else
            {
                dto.exchange_id = 0;
                dto.exchange_quantity = 0;
                dto.exchange_price = 0;
                dto.exchange_promo_price = 0;
                dto.exchange_with = "";
            }

            return dto;
        }










        public static IssueDetailsDto CopyFromEntity(IssueDetailsDto dto, GetIssuesListForSUser_Result issue_list)
        {
            if (dto == null)
                dto = new IssueDetailsDto();

            dto.order_id = issue_list.OrdrID;
            dto.order_type = OrdersServices.OrderType.OrderApp.ToString();
            dto.invoice_number = issue_list.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(issue_list.OrderDate);
            dto.order_time = issue_list.OrderTime.ToString(@"hh\:mm");
            dto.consumer_name = issue_list.Name;
            dto.consumer_mobile = issue_list.PhoneNumber;
            dto.consumer_address = issue_list.Address;
            dto.grand_total = issue_list.GrandTotal;


            return dto;
        }

        public static void CopyFromEntity(AllOrderDetails response, Order order)
        {
            response.agentadmin_mobile = order.AgentAdmin != null ? order.AgentAdmin.MobileNumber : string.Empty;
            response.agentadmin_image = order.AgentAdmin != null ? ImagePathService.agentAdminImagePath + order.AgentAdmin.ProfileImage : string.Empty;
            response.agentadmin_name = order.AgentAdmin != null ? order.AgentAdmin.AgentAdminName : string.Empty;
            response.delivery_date = Common.ToDateFormat(order.DeliveryDate);
            response.delivery_status = order.OrderDeliveries.Count > 0 ? order.OrderDeliveries.Where(x => x.OrdrID == order.OrdrID).FirstOrDefault().StatusId : 0;
            response.grand_total = order.GrandTotal;
            response.invoice_number = order.InvoiceNumber;
            response.order_date = Common.ToDateFormat(order.OrderDate);
            response.order_id = order.OrdrID;
            response.order_status = order.StatusID;
            response.order_time = order.OrderTime.ToString();
            response.time_slot_name = order.MDeliverySlot != null ? order.MDeliverySlot.SlotName : string.Empty;
            if (response.driver_details == null)
                response.driver_details = new DriverDetailsDto();
            if (order.Driver != null)
            {
                response.driver_details.driver_id = order.Driver.DrvrID;
                response.driver_details.driver_name = order.Driver.DriverName;
                response.driver_details.driver_image = ImagePathService.driverImagePath + order.Driver.ProfileImage;
                response.driver_details.driver_mobile = order.Driver.MobileNumber;
            }
            else
            {
                response.driver_details.driver_name = string.Empty;
                response.driver_details.driver_image = string.Empty;
                response.driver_details.driver_mobile = string.Empty;
            }
            if (response.product_details == null)
                response.product_details = new List<ProductsDto>();
            foreach (var item in order.OrderDetails)
            {
                ProductsDto prodDto = new ProductsDto();
                OrdersHelper.CopyFromEntity(prodDto, item);
                response.product_details.Add(prodDto);
            }

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

        public static void CopyFromEntity(ExchangeDto response, OrderPrdocuctExchange exchange)
        {
            response.exchange_id = exchange.OrdPrdExID;
            response.exchange_price = exchange.ExchangePrice;
            response.exchange_promo_price = exchange.ExchangePromoPrice / exchange.ExchangeQuantity;
            response.exchange_quantity = exchange.ExchangeQuantity;
            response.exchange_with = exchange.ExchangeWith;
        }

        public static void CopyFromEntity(DriverOrderDetailDto dto, TeleOrder order, Driver drv = null)
        {
            dto.order_id = order.TeleOrdID;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime.ToString(@"hh\:mm");
            dto.delivery_status_id = order.TeleOrderDeliveries.Count > 0 ? order.TeleOrderDeliveries.Where(x => x.TeleOrdID == order.TeleOrdID).FirstOrDefault().StatusId : 0;
            dto.order_status_id = order.StatusId;
            dto.agent_admin_phone_no = order.AgentAdmin != null ? order.AgentAdmin.MobileNumber : string.Empty;
            if (order.TeleCustomers.Count > 0)
            {
                var teleConsumer = order.TeleCustomers.Where(x => x.TeleOrdID == order.TeleOrdID).FirstOrDefault();
                if (teleConsumer != null)
                {
                    dto.consumer_name = teleConsumer.CustomerName;
                    dto.consumer_mobile = teleConsumer.MobileNumber;
                    dto.consumer_address = teleConsumer.Address;
                    dto.latitude = teleConsumer.Latitude;
                    dto.longitude = teleConsumer.Longitude;
                }
            }

            dto.delivery_timeslot_id = Convert.ToInt32(order.DeliverySlotID);
            dto.grand_total = order.GrantTotal;
        }

        public static DriverProductsDto[] CopyFromEntity(TeleOrder teleOrder)
        {
            List<DriverProductsDto> pdtos = new List<DriverProductsDto>();
            foreach (TeleOrderDetail det in teleOrder.TeleOrderDetails)
            {
                DriverProductsDto pdt = new DriverProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct.HasValue ? det.PromoProduct.Value : 0;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge.HasValue ? det.ShippingCharge.Value : 0;
                pdt.shipping_promo = det.PromoShipping.HasValue ? det.PromoShipping.Value : 0;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdtos.Add(pdt);
            }
            return pdtos.ToArray();
        }










    }

}

