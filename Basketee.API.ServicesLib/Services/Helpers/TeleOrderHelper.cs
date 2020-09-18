using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.TeleOrder;
using Basketee.API.Models;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.Services.Helpers
{
    public class TeleOrderHelper
    {
        public static void CopyDriverFromEntity(DriverDetailsDto dto, Driver driver)
        {
            dto.driver_id = driver.DrvrID;
            dto.driver_image = ImagePathService.driverImagePath + driver.ProfileImage;
            dto.driver_mobile = driver.MobileNumber;
            dto.driver_name = driver.DriverName;
            // dto.driver_rating = driver.Rating;
        }
        public static void CopyFromEntity(AddTeleOrderResponse dto, TeleOrder order)
        {
            if (dto.order_details == null)
            {
                dto.order_details = new TeleOrderDto();
            }
            dto.order_details.order_id = order.TeleOrdID; // order_status
            dto.order_details.invoice_number = order.InvoiceNumber;
            dto.order_details.order_status = order.MOrderStatu.OrderStatus;
        }

        public static void CopyFromEntity(ConfirmTeleOrderResponse response, TeleOrder order)
        {
            TeleOrderFullDetailsDto dto = new TeleOrderFullDetailsDto();
            response.orders = dto;
            dto.order_id = order.TeleOrdID;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            dto.order_time = order.OrderTime;
            dto.delivery_date = Common.ToDateFormat(order.DeliveryDate ?? DateTime.MinValue);
            dto.delivery_time_slot = order.MDeliverySlot.SlotName;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_status = order.StatusId;
            if (order.TeleCustomers.Count > 0)
            {
                TeleCustomer cust = order.TeleCustomers.First();
                dto.consumer_name = cust.CustomerName;
                dto.consumer_mobile = cust.MobileNumber;
                dto.consumer_address = cust.Address;
                dto.consumer_location = null;
            }
            dto.driver = new DriverDetailsDto();
            if (order.Driver != null)
            {
                dto.driver = new DriverDetailsDto();
                CopyDriverFromEntity(dto.driver, order.Driver);
            }

            List<ProductsDto> pdtos = new List<ProductsDto>();
            foreach (TeleOrderDetail det in order.TeleOrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct ?? 0.0M;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge ?? 0.0M;
                pdt.shipping_promo = det.PromoShipping ?? 0.0M;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.grand_total = det.TotalAmount;
                pdtos.Add(pdt);
            }
            dto.grand_total = order.GrantTotal;
            dto.products = pdtos.ToArray();
            dto.has_exchange = (order.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (dto.has_exchange == 1)
            {
                if (dto.exchange == null)
                    dto.exchange = new List<ExchangeDto>();
                foreach (var item in order.TeleOrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    CopyFromEntity(exDto, item);
                    dto.exchange.Add(exDto);
                }
            }
        }

        public static void CopyFromEntity(OrderInvoiceDto dto, TeleOrder teleOrder)
        {
            dto.order_id = teleOrder.TeleOrdID;
            dto.invoice_number = teleOrder.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(teleOrder.OrderDate);
            dto.order_time = teleOrder.OrderTime;
            if (teleOrder.TeleCustomers.Count > 0)
            {
                TeleCustomer cust = teleOrder.TeleCustomers.First();
                dto.consumer_name = cust.CustomerName;
                dto.consumer_mobile = cust.MobileNumber;
                dto.consumer_address = cust.Address;
            }
            decimal sumPromoProduct = 0;
            decimal sumPromoShipping = 0;
            decimal sumPromoRefill = 0;
            decimal sumPromoExchange = 0;
            List<ProductsDto> pdtos = new List<ProductsDto>();
            foreach (TeleOrderDetail det in teleOrder.TeleOrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct ?? 0.0M;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge ?? 0M;
                pdt.shipping_promo = det.PromoShipping ?? 0M;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdtos.Add(pdt);

                if (teleOrder.DeliveryType)
                {
                    if (det.Quantity > 0)
                    {
                        sumPromoShipping += (det.PromoShipping * det.Quantity) ?? 0.0M;
                    }
                    if (det.RefillQuantity > 0)
                    {
                        sumPromoShipping += (det.PromoShipping * det.RefillQuantity) ?? 0.0M;
                    }
                }
                sumPromoProduct += det.PromoProduct ?? 0.0M;
                sumPromoRefill += det.PromoRefill;

                dto.has_exchange = (teleOrder.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
                if (dto.has_exchange == 1)
                {
                    if (dto.exchange == null)
                        dto.exchange = new List<ExchangeDto>();
                    foreach (var item in teleOrder.TeleOrderPrdocuctExchanges)
                    {
                        if (item.ProdID == det.ProdID)
                        {
                            ExchangeDto exDto = new ExchangeDto();
                            CopyFromEntity(exDto, item);
                            dto.exchange.Add(exDto);
                            sumPromoExchange += item.ExchangePromoPrice;
                            if (teleOrder.DeliveryType)
                                sumPromoShipping += (det.PromoShipping * item.ExchangeQuantity) ?? 0.0M;
                        }
                    }
                }
            }
            dto.agency_id = teleOrder.AgentAdmin.Agency.AgenID;
            dto.agency_name = teleOrder.AgentAdmin.Agency.AgencyName;
            dto.agency_address = teleOrder.AgentAdmin.Agency.MRegion.RegionName;
            dto.agency_location = teleOrder.AgentAdmin.Agency.MRegion.RegionName;
            //dto.agency_address = teleOrder.AgentAdmin.Agency.Region;
            //dto.agency_location = teleOrder.AgentAdmin.Agency.Region;
            dto.grand_total = teleOrder.GrantTotal;

            dto.grand_discount = (Math.Abs(sumPromoProduct) + (teleOrder.DeliveryType ? Math.Abs(sumPromoShipping) : 0) + Math.Abs(sumPromoRefill) + Math.Abs(sumPromoExchange)) * -1;
            dto.grand_total_with_discount = dto.grand_total + Math.Abs(dto.grand_discount);
            dto.products = pdtos.ToArray();
        }

        public static void CopyFromEntity(ExchangeDto response, TeleOrderPrdocuctExchange exchange)
        {
            response.exchange_id = exchange.TelOrdPrdExID;
            response.exchange_price = exchange.ExchangePrice;
            response.exchange_promo_price = exchange.ExchangePromoPrice / exchange.ExchangeQuantity;
            response.exchange_quantity = exchange.ExchangeQuantity;
            response.exchange_with = exchange.ExchangeWith;
        }
    }
}