using Basketee.API.DTOs.Driver;
using Basketee.API.DTOs.Orders;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Basketee.API.Services.Helpers
{
    class DriverHelper
    {
        public const string APPSETTING_MAX_DRIVER_ORDER = "MaxDriverOrder";
        public static void CopyToEntity(Driver driver, LoginRequest request)
        {
            driver.MobileNumber = request.mobile_number; // password
            driver.AppID = request.app_id;
            driver.AppToken = request.push_token;
        }

        public static void CopyFromEntity(LoginResponse dto, Driver driver, Reminder reminder)
        {
            if (dto.user_login == null)
                dto.user_login = new UserLoginDto();
            dto.user_login.user_id = driver.DrvrID;
            dto.user_login.auth_token = driver.AccToken;
            if (dto.driver_details == null)
                dto.driver_details = new DriverDetails();
            dto.driver_details.agency_name = (driver.Agency != null ? driver.Agency.AgencyName : "");
            dto.driver_details.driver_name = driver.DriverName;
            dto.driver_details.mobile_number = driver.MobileNumber;
            dto.driver_details.profile_image = ImagePathService.driverImagePath + driver.ProfileImage;
            //if (dto.has_reminder)
            //{
            if (dto.reminder_details == null)
                dto.reminder_details = new ReminderDetailsDto();
            dto.reminder_details.reminder_description = reminder == null ? string.Empty : reminder.Description;
            dto.reminder_details.reminder_id = reminder == null ? 0 : reminder.RmdrID;
            dto.reminder_details.reminder_image = reminder == null ? string.Empty : ImagePathService.reminderImagePath + reminder.ReminderImage;
            //}
        }

        //public static void CopyToEntity(Driver driver, ForgotPasswordRequest request)
        //{
        //    driver.MobileNumber = request.mobile_number;
        //}
        public static void CopyFromEntity(ResetPasswordDto response, Driver driver)
        {
            //response.password_reset = driver.;
            //response.password_otp_sent = driver.;

        }
        public static void CopyToEntity(Driver driver, ChangePasswordDriverRequest request)
        {
            driver.DrvrID = request.user_id; // auth_token, old_password, new_password
        }
        public static void CopyToEntity(Driver driver, GetAgentDriverRequest request)
        {
            driver.DrvrID = request.user_id; // auth_token
        }
        public static void CopyFromEntity(DTOs.Driver.DriverDetailsDto response, Driver driver)
        {
            response.driver_id = driver.DrvrID;
            response.profile_image = ImagePathService.driverImagePath + driver.ProfileImage;
            response.driver_name = driver.DriverName;
            response.mobile_number = driver.MobileNumber;
        }

        public static void CopyFromEntity(DTOs.Orders.DriverDetailListDto response, DAOs.OrderDao.DriverOrder driver)
        {
            response.driver_id = driver.drvr_id;
            response.driver_availability = driver.tot_assignment.ToString() + "/" + Common.GetAppSetting<string>(APPSETTING_MAX_DRIVER_ORDER, string.Empty);
            response.driver_name = driver.drvr_name;
            //response.driver_profile_image=driver.dr
        }

        public static void CopyFromEntity(OrderInvoiceDto dto, Order order)
        {
            dto.order_id = order.OrdrID;
            dto.invoice_number = order.InvoiceNumber;
            dto.consumer_name = order.Consumer.Name;
            dto.consumer_mobile = order.Consumer.PhoneNumber;
            dto.consumer_address = order.ConsumerAddress.Address;
            dto.consumer_location = order.ConsumerAddress.RegionName;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
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

                if (det.PromoProduct != Convert.ToDecimal(0))
                {
                    sumPromoShipping += det.PromoShipping;
                }
                if (det.RefillQuantity > 0)
                {
                    sumPromoShipping += det.PromoShipping;
                }
                sumPromoProduct += det.PromoProduct;                
                sumPromoRefill += det.PromoRefill;
                dto.has_exchange = (order.OrderPrdocuctExchanges.Count > 0 ? 1 : 0);
                if (dto.has_exchange == 1)
                {
                    if (dto.exchange == null)
                        dto.exchange = new List<Basketee.API.DTOs.Orders.ExchangeDto>();
                    foreach (var item in order.OrderPrdocuctExchanges)
                    {
                        Basketee.API.DTOs.Orders.ExchangeDto exDto = new Basketee.API.DTOs.Orders.ExchangeDto();
                        OrdersHelper.CopyFromEntity(exDto, item);
                        dto.exchange.Add(exDto);
                        sumPromoExchange += exDto.exchange_promo_price;
                        sumPromoShipping += det.PromoShipping;
                    }
                }
            }
            dto.agency_id = order.AgentAdmin.Agency.AgenID;
            dto.agency_name = order.AgentAdmin.Agency.AgencyName;
            dto.agency_address = order.AgentAdmin.Agency.MRegion.RegionName;
            dto.agency_location = order.AgentAdmin.Agency.MRegion.RegionName;
            //dto.agency_address = order.AgentAdmin.Agency.Region;
            //dto.agency_location = order.AgentAdmin.Agency.Region;
            dto.grand_total = order.GrandTotal;
            
            dto.grand_discount = (Math.Abs(sumPromoProduct) + Math.Abs(sumPromoShipping) + Math.Abs(sumPromoRefill) + Math.Abs(sumPromoExchange)) * -1;
            dto.grand_total_with_discount = dto.grand_total + Math.Abs(dto.grand_discount);
            dto.products = pdtos.ToArray();
        }

        public static void CopyFromEntity(OrderInvoiceDto dto, TeleOrder order)
        {
            dto.order_id = order.TeleOrdID;
            dto.invoice_number = order.InvoiceNumber;
            dto.order_date = Common.ToDateFormat(order.OrderDate);
            if (order.TeleCustomers.Count > 0)
            {
                TeleCustomer cust = order.TeleCustomers.First();
                dto.consumer_name = cust.CustomerName;
                dto.consumer_mobile = cust.MobileNumber;
                dto.consumer_address = cust.Address;
            }
            List<ProductsDto> pdtos = new List<ProductsDto>();
            decimal sumPromoProduct = 0;
            decimal sumPromoShipping = 0;
            decimal sumPromoRefill = 0;
            decimal sumPromoExchange = 0;
            foreach (TeleOrderDetail det in order.TeleOrderDetails)
            {
                ProductsDto pdt = new ProductsDto();
                pdt.product_id = det.Product.ProdID;
                pdt.product_name = det.Product.ProductName;
                pdt.product_promo = det.PromoProduct ?? 0.0M;
                pdt.quantity = det.Quantity;
                pdt.shipping_cost = det.ShippingCharge ?? 0M;
                pdt.shipping_promo = det.PromoShipping ?? 0M;
                pdt.sub_total = det.SubTotal;
                pdt.unit_price = det.UnitPrice;
                pdt.refill_price = det.RefillPrice;
                pdt.refill_promo = det.PromoRefill;
                pdt.refill_quantity = det.RefillQuantity;
                pdtos.Add(pdt);
                if (order.DeliveryType)
                {
                    if (det.PromoProduct != Convert.ToDecimal(0))
                    {
                        sumPromoShipping += det.PromoShipping ?? 0M;
                    }
                    if (det.RefillQuantity > 0)
                    {
                        sumPromoShipping += det.PromoShipping ?? 0M;
                    }
                }
                sumPromoProduct += det.PromoProduct ?? 0M;                
                sumPromoRefill += det.PromoRefill;
                dto.has_exchange = (order.TeleOrderPrdocuctExchanges.Count > 0 ? 1 : 0);
                if (dto.has_exchange == 1)
                {
                    if (dto.exchange == null)
                        dto.exchange = new List<Basketee.API.DTOs.Orders.ExchangeDto>();
                    foreach (var item in order.TeleOrderPrdocuctExchanges)
                    {
                        Basketee.API.DTOs.Orders.ExchangeDto exDto = new Basketee.API.DTOs.Orders.ExchangeDto();
                        TeleOrderHelper.CopyFromEntity(exDto, item);
                        dto.exchange.Add(exDto);
                        sumPromoExchange += exDto.exchange_promo_price;
                        if (order.DeliveryType)
                            sumPromoShipping += det.PromoShipping ?? 0M;
                    }
                }
            }
            dto.agency_id = order.AgentAdmin.Agency.AgenID;
            dto.agency_name = order.AgentAdmin.Agency.AgencyName;
            dto.agency_address = order.AgentAdmin.Agency.MRegion.RegionName;
            dto.agency_location = order.AgentAdmin.Agency.MRegion.RegionName;
            //dto.agency_address = order.AgentAdmin.Agency.Region;
            //dto.agency_location = order.AgentAdmin.Agency.Region;
            dto.grand_total = order.GrantTotal;           
            dto.grand_discount = (Math.Abs(sumPromoProduct) + Math.Abs(sumPromoShipping) + Math.Abs(sumPromoRefill) + Math.Abs(sumPromoExchange)) * -1;
            dto.grand_total_with_discount = dto.grand_total + Math.Abs(dto.grand_discount);
            dto.products = pdtos.ToArray();
        }
    }
}
