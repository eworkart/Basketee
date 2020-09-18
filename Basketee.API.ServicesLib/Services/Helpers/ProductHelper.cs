using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.Models;
using Basketee.API.DTOs.Product;

namespace Basketee.API.Services.Helpers
{
    public class ProductHelper
    {
        public static void CopyToEntity(Consumer consumer, GetProductListRequest request)
        {
            consumer.ConsID = request.user_id; // row_per_page, page_number
            consumer.AppToken = request.auth_token;

        }
        public static void CopyFromEntity(ProductDto productDto, Product product)
        {
            productDto.product_id = product.ProdID; // product_description
            productDto.product_image = ImagePathService.productImagePath + product.ProductImage;
            productDto.product_title = product.ProductName;
            productDto.Price_refil = product.RefillPrice;
            productDto.price_of_the_tube = product.TubePrice;
        }

        public static void CopyFromEntity(GetProductListResponse dto, Reminder reminder)
        {
            dto.reminder_description = reminder == null ? string.Empty : reminder.Description;
            dto.reminder_id = reminder == null ? 0 : reminder.RmdrID;
            dto.reminder_image = reminder == null ? string.Empty : ImagePathService.reminderImagePath + reminder.ReminderImage;            
        }

        public static void CopyToEntity(Product product, GetProductDetailsRequest request)
        {
            product.ProdID = request.product_id; // auth_token
        }
        public static void CopyFromEntity(ProductDetailsDto dto, Product product)
        {
            dto.product_id = product.ProdID; // product_description
            dto.product_name = product.ProductName;
            dto.position = product.Position;
            dto.product_image = ImagePathService.productImagePath + product.ProductImage;
            dto.product_image_details = ImagePathService.productImagePath + product.ProductImageDetails;
            dto.tube_price = product.TubePrice;
            dto.tube_promo_price = product.TubePromoPrice;
            dto.refill_price = product.RefillPrice;
            dto.refill_promo_price = product.RefillPromoPrice;
            dto.shipping_price = product.ShippingPrice;
            dto.shipping_promo_price = product.ShippingPromoPrice;
            if(product.ProductExchanges.Count > 0)
            {
                dto.has_exchange = true;
                dto.exchange = new ExchangeDto[product.ProductExchanges.Count];
                var prdExchanges = product.ProductExchanges.ToList();
                for(int i = 0; i < product.ProductExchanges.Count; i++)
                {
                    ExchangeDto exDto = new ExchangeDto();
                    exDto.exchange_id = prdExchanges[i].PrExID;
                    exDto.exchange_with = prdExchanges[i].ExchangeWith;
                    exDto.exchange_quantity = prdExchanges[i].ExchangeQuantity;
                    exDto.exchange_price = prdExchanges[i].ExchangePrice.Value;
                    exDto.exchange_promo_price = prdExchanges[i].ExchangePromoPrice;
                    dto.exchange[i] = exDto;
                }
            }
        }

        public static void CopyFromEntity(ExchangeDto response, ProductExchange exchange)
        {
            response.exchange_id = exchange.PrExID;
            response.exchange_price = exchange.ExchangePrice ?? 0M;
            response.exchange_promo_price = exchange.ExchangePromoPrice;
            response.exchange_quantity = exchange.ExchangeQuantity;
            response.exchange_with = exchange.ExchangeWith;
        }
    }
}