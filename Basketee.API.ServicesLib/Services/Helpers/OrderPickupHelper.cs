using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.OrderPickup;
using Basketee.API.Models;
using Basketee.API.DTOs.Orders;
using Basketee.API.Services.Helpers;
using Basketee.API.Services;

namespace Basketee.API.ServicesLib.Services.Helpers
{
    class OrderPickupHelper
    {
        //public static void CopyToEntity(TeleOrderDetail order, PlaceOrderPickupRequest request)
        //{
        //    order.ProdID = request.products.product_id; // user_id, auth_token, grand_total
        //    order.Quantity = request.products.quantity;
        //    order.UnitPrice = request.products.unit_price;
        //    order.SubTotal = request.products.sub_total;
        //    order.PromoProduct = request.products.product_promo;
        //}
        public static void CopyFromEntity(PlaceOrderPickupResponse response, TeleOrder order)
        {
            if (response.order_details == null)
            {
                response.order_details = new PickupOrderResponseDto();
            }
            response.order_details.order_id = order.TeleOrdID;
            response.order_details.invoice_number = order.InvoiceNumber;
        }

        public static void CopyToEntity(Order order, ConfirmPickupOrderRequest request)
        {
            order.OrdrID = request.order_id; // user_id, auth_token
        }
        public static void CopyFromEntity(ConfirmPickupOrderResponse response, TeleOrder order)
        {
            PickupOrderDto odt = new PickupOrderDto();
            response.orders = odt;
            odt.order_id = order.TeleOrdID;
            odt.order_date = Common.ToDateFormat(order.OrderDate);
            odt.invoice_number = order.InvoiceNumber;
            odt.order_status = order.StatusId;
            odt.grand_total = order.GrantTotal;

            //dto.oder = odtos.ToArray();
            List<ProductsDto> pdtos = new List<ProductsDto>();
            foreach (TeleOrderDetail det in order.TeleOrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct ?? 0M;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge ?? 0M;
                pdt.shipping_promo = det.PromoShipping ?? 0M;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdtos.Add(pdt);
            }
            odt.products = pdtos.ToArray();
            odt.has_exchange = (order.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
            if (odt.has_exchange == 1)
            {
                if (odt.exchange == null)
                    odt.exchange = new List<ExchangeDto>();
                foreach (var item in order.TeleOrderPrdocuctExchanges)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    TeleOrderHelper.CopyFromEntity(exDto, item);
                    odt.exchange.Add(exDto);
                }
            }
        }
    }
}
