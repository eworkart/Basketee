using Basketee.API.DTOs;
using Basketee.API.DTOs.Orders;
using System.Web.Http;
using Basketee.API.Services;
using System.Web.Http.Results;
using System.Net;
using Basketee.API.DTOs.OrderPickup;
using Basketee.API.DTOs.TeleOrder;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Basketee.API.Controllers
{
    //TODO: Move declaration to a correct place
    public class TelemetryEvents {
        public static string GetActiveOrderCount = "GetActiveOrderCount";
        public static string SubmitReview = "SubmitReview";

        public static string PlaceOrder = "PlaceOrder";
        public static string GetTimeslot = "GetTimeslot";
    }

    public class OrdersController : ApiController
    {
        #region Telemetry
        private TelemetryClient tm = new TelemetryClient();
        

        #endregion

        private OrdersServices _ordersServices = new OrdersServices();
        private OrderPickupServices _orderpickupServices = new OrderPickupServices();
        private TeleOrderServices _teleorderServices = new TeleOrderServices();
        private DriverServices _driverServices = new DriverServices();

        [HttpPost]
        [ActionName("get_active_order_count")]
        public NegotiatedContentResult<GetActiveOrderCountResponse> PostOrderCount([FromBody]GetActiveOrderCountRequest request)
        {
            var properties = new Dictionary<string, string> { { "request", JsonConvert.SerializeObject(request) } };
            tm.TrackEvent(TelemetryEvents.GetActiveOrderCount, properties);

            GetActiveOrderCountResponse resp = _ordersServices.GetActiveOrderCount(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("submit_review")]
        public NegotiatedContentResult<ResponseDto> PostSubmitReview([FromBody]SubmitReviewRequest request)
        {
            tm.TrackEvent(TelemetryEvents.SubmitReview);

            ResponseDto resp = _ordersServices.SubmitReview(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_timeslot")]
        public NegotiatedContentResult<GetTimeSlotResponse> PostGetTimeslot([FromBody]GetTimeslotRequest request)
        {
            var properties = new Dictionary<string, string> { { "request", JsonConvert.SerializeObject(request) } };
            tm.TrackEvent(TelemetryEvents.GetTimeslot, properties);

            GetTimeSlotResponse resp = _ordersServices.GetTimeSlot(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("place_order")]
        [ActionInputValidationFilter()]
        //[AuthenticationValidationFilter(role: AuthRoles.Consumer)]
        public NegotiatedContentResult<PlaceOrderResponse> PostPlaceOrder([FromBody]PlaceOrderRequest request)
        {

            var properties = new Dictionary<string, string> {{"request", JsonConvert.SerializeObject(request) }};
            tm.TrackEvent(TelemetryEvents.PlaceOrder, properties);

            PlaceOrderResponse resp = _ordersServices.PlaceOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_current_order_list")]
        public NegotiatedContentResult<GetOrderListResponse> PostGetCurrentOrderList([FromBody]GetOrderListRequest request)
        {
            GetOrderListResponse resp = _ordersServices.GetOrderList(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_order_details")]
        public NegotiatedContentResult<GetOrderDetailsResponse> PostGetOrderDetails([FromBody]GetOrderDetailsRequest request)
        {
            GetOrderDetailsResponse resp = _ordersServices.GetOrderDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("cancel_order")]
        public NegotiatedContentResult<ResponseDto> PostCancelOrder([FromBody]CancelOrderRequest request)
        {
            ResponseDto resp = _ordersServices.CancelOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_unassigned_orders")]
        public NegotiatedContentResult<GetUnassignedOrdersResponse> PostGetUnassignedOrders([FromBody]GetUnassignedOrdersRequest request)
        {
            GetUnassignedOrdersResponse resp = _ordersServices.GetUnassignedOrders(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_all_unassigned_orders")]
        public NegotiatedContentResult<GetAllUnassignedOrdersResponse> PostGetAllUnassignedOrders([FromBody]GetAllUnAssignedOrdersRequest request)
        {
            GetAllUnassignedOrdersResponse resp = _ordersServices.GetAllUnassignedOrders(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_driver_details")]
        public NegotiatedContentResult<GetDriverDetailsResponse> PostGetDriverDetails([FromBody]GetDriverDetailsRequest request)
        {
            GetDriverDetailsResponse resp = _ordersServices.GetDriverDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("confirm_order")]
        public NegotiatedContentResult<ConfirmOrderResponse> PostConfirmOrder([FromBody]ConfirmOrderRequest request)
        {
            ConfirmOrderResponse resp = _ordersServices.ConfirmOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }


        [HttpPost]
        [ActionName("get_org_order_list")]
        public NegotiatedContentResult<GetOrgOrderListResponse> PostGetOrgOrderList([FromBody]GetOrgOrderListRequest request)
        {
            GetOrgOrderListResponse resp = _ordersServices.GetOrgOrderList(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_org_orders_details")]
        public NegotiatedContentResult<GetOrgOrderDetailsResponse> PostGetOrgOrdersDetails([FromBody]GetOrgOrderDetailsRequest request)
        {
            GetOrgOrderDetailsResponse resp = _ordersServices.GetOrgOrderDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_invoice_details")]
        public NegotiatedContentResult<GetInvoiceDetailsResponse> PostGetOrgOrdersDetails([FromBody]GetInvoiceDetailsRequest request)
        {
            GetInvoiceDetailsResponse resp = _ordersServices.GetInvoiceDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("place_pickup_order")]
        public NegotiatedContentResult<PlaceOrderPickupResponse> PostPlacePickupOrder([FromBody]PlaceOrderPickupRequest request)
        {
            PlaceOrderPickupResponse resp = _orderpickupServices.PlacePickupOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("confirm_pickup_order")]
        public NegotiatedContentResult<ConfirmPickupOrderResponse> PostConfirmPickupOrder([FromBody]ConfirmPickupOrderRequest request)
        {
            ConfirmPickupOrderResponse resp = _orderpickupServices.ConfirmPickupOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("add_tele_order")]
        public NegotiatedContentResult<AddTeleOrderResponse> PostAddTeleOrder([FromBody]AddTeleOrderRequest request)
        {
            AddTeleOrderResponse resp = _teleorderServices.AddTeleOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("confirm_tele_order")]
        public NegotiatedContentResult<ConfirmTeleOrderResponse> PostConfirmTeleOrder([FromBody]ConfirmTeleOrderRequest request)
        {
            ConfirmTeleOrderResponse resp = _teleorderServices.ConfirmTeleOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_assigned_order_count")]
        public NegotiatedContentResult<GetAssignedOrderCountResponse> PostGetAssignedOrderCount([FromBody]GetAssignedOrderCountRequest request)
        {
            GetAssignedOrderCountResponse resp = _ordersServices.GetAssignedOrderCount(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_driver_order_list")]
        public NegotiatedContentResult<GetDriverOrderListResponse> PostGetDriverOrderList([FromBody]GetDriverOrderListRequest request)
        {
            GetDriverOrderListResponse resp = _ordersServices.GetDriverOrderList(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_driver_order")]
        public NegotiatedContentResult<GetDriverOrderResponse> PostGetDriverOrder([FromBody]GetDriverOrderRequest request)
        {
            GetDriverOrderResponse resp = _ordersServices.GetDriverOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("close_order")]
        public NegotiatedContentResult<CloseOrderResponse> PostCloseOrder([FromBody]CloseOrderRequest request)
        {
            CloseOrderResponse resp = _ordersServices.CloseOrder(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("PostSendEmail")]
        public NegotiatedContentResult<ResponseDto> PostSendEmail([FromBody]SendEmailRequest request)
        {
            ResponseDto resp = _ordersServices.SendEmail(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("out_for_delivery")]
        public NegotiatedContentResult<ResponseDto> OutForDelivery([FromBody]OutForDeliveryRequest request)
        {
            ResponseDto resp = _ordersServices.OutForDelivery(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [ActionName("get_ereceipt_consumer")]
        public NegotiatedContentResult<GetEReceiptResponse> PostGetEReceipt([FromBody]GetEReceiptRequest request)
        {
            GetEReceiptResponse resp = _ordersServices.GetERecieptDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_driver_list")]
        public NegotiatedContentResult<GetDriverListResponse> PostGetDriverList([FromBody]GetDriverListRequest request)
        {
            GetDriverListResponse resp = _ordersServices.GetDriverList(request);
            return Content(HttpStatusCode.OK, resp); 
        }

        /// <summary>
        /// To get active orders count for boss
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetActiveOrderCountBossResponse</returns>
        [HttpPost]
        [ActionName("get_active_order_count_boss")]
        public NegotiatedContentResult<GetActiveOrderCountBossResponse> PostOrderCountBoss([FromBody]GetActiveOrderCountBossRequest request)
        {
            GetActiveOrderCountBossResponse resp = _ordersServices.GetActiveOrderCountBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        /// <summary>
        /// To get order lists for boss
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetOrderListBossResponse</returns>
        [HttpPost]
        [ActionName("get_org_order_list_boss")]
        public NegotiatedContentResult<GetOrderListBossResponse> PostOrderListBoss([FromBody]GetOrderListBossRequest request)
        {
            GetOrderListBossResponse resp = _ordersServices.GetOrderListBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        /// <summary>
        /// To get order lists for boss
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetOrderListBossResponse</returns>
        [HttpPost]
        [ActionName("get_org_order_details_boss")]
        public NegotiatedContentResult<GetOrderDetailsBossResponse> PostOrderDetailsBoss([FromBody]GetOrderDetailsBossRequest request)
        {
            GetOrderDetailsBossResponse resp = _ordersServices.GetOrderDetailsBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        /// <summary>
        /// Send e-receipt to agentadmin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("send_email")]
        public NegotiatedContentResult<ResponseDto> SendInvoiceEmail([FromBody]AgentAdminInvoiceMailSendRequest request)
        {
            //EmailServices.SendMail("ttyy.ns@eraminfotech.in", "Mail Title", "Mail Message Content");
            ResponseDto resp = _ordersServices.SendInvoiceMail(request);
            return Content(HttpStatusCode.OK, resp);
        }

        /// <summary>
        /// Get driver list for tele order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("driver_for_agent_admin")]
        public NegotiatedContentResult<GetDriverListForAgentAdminResponse> PostGetDriverListForAgentAdmin([FromBody]GetDriverListForAgentAdminRequest request)
        {
            GetDriverListForAgentAdminResponse resp = _ordersServices.GetDriversForAgentAdmin(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("forced_cancel_order_suser")]
        public NegotiatedContentResult<ResponseDto> SUser([FromBody]ForcedCancelOrderSuserRequest request)
        {
            ResponseDto resp = _ordersServices.ForcedCancelOrderSuser(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_issues_count")]
        public NegotiatedContentResult<GetIssuesCountResponse> PostGetIssuesCount([FromBody]GetIssuesCountRequest request)
        {
            GetIssuesCountResponse resp = _ordersServices.GetIssuesCount(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_issues_list")]
        public NegotiatedContentResult<GetIssuesListResponse> PostGetIssuesList([FromBody]GetIssuesListRequest request)
        {
            GetIssuesListResponse resp = _ordersServices.GetIssuesList(request);
            return Content(HttpStatusCode.OK, resp);
        }


        /// <summary>
        /// Get driver e-receipt
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_ereceipt_driver")]
        public NegotiatedContentResult<Basketee.API.DTOs.Driver.GetEReceiptResponse> PostGetEreceiptDriver([FromBody] Basketee.API.DTOs.Driver.GetEReceiptRequest request)
        {
            Basketee.API.DTOs.Driver.GetEReceiptResponse resp = _driverServices.GetERecieptDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }
        /// <summary>
        /// Get driver e-receipt
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_issue_details")]
        public NegotiatedContentResult<GetIssueDetailsResponseSUsers> PostIssueDetailsSuser([FromBody] GetIssueDetailsRequestSUsers request)
        {
            GetIssueDetailsResponseSUsers resp = _ordersServices.GetIssueDetailsResponseSUsers(request);
            return Content(HttpStatusCode.OK, resp);
        }

        /// <summary>
        /// Get all order details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_all_order_details")]
        public NegotiatedContentResult<GetAllOrderDetailsResponse> PostGetAllOrderDetails([FromBody]GetAllOrderDetailsRequest request)
        {
            GetAllOrderDetailsResponse resp = _ordersServices.GetAllOrderDetails(request);
            return Content(HttpStatusCode.OK, resp);
        }
    }
}