using Basketee.API.DTOs;
using Basketee.API.DTOs.Users;
using Basketee.API.Services;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Basketee.API.DTOs.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net.Http.Formatting;
using Basketee.API.UserAuthentication;
using System.Security.Claims;
using Basketee.API.DAOs;
using Basketee.API.DTOs.Orders;

namespace Basketee.API.Controllers
{
    public class CommonController : ApiController
    {
        private OrdersServices _ordersServices = new OrdersServices();

        [BasicAuthentication]
        [MyAuthorize(Roles = "Consumer, Driver")]
        [ActionName("test_api")]
        public NegotiatedContentResult<GetOrderListResponse> TestApi([FromBody] GetOrderListRequest request)
        {
            GetOrderListResponse resp = _ordersServices.GetOrderList(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [BasicAuthentication]
        [MyAuthorize(Roles = "Consumer")]
        [ActionName("get_order_list_consumer")]
        public NegotiatedContentResult<GetOrderListResponse> GetConsumerOrderList(GetOrderListRequest request)
        {
            GetOrderListResponse resp = _ordersServices.GetOrderList(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [BasicAuthentication]
        [MyAuthorize(Roles = "Driver")]
        [ActionName("get_order_list_driver")]
        public NegotiatedContentResult<GetDriverOrderListResponse> GetDriverOrderList(GetDriverOrderListRequest request)
        {
            GetDriverOrderListResponse resp = _ordersServices.GetDriverOrderList(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [BasicAuthentication]
        [MyAuthorize(Roles = "Consumer")]
        [ActionName("get_order_list_consumer_test")]
        //[Route("api/test")]
        public NegotiatedContentResult<GetOrderListResponse> GetConsumerOrderListTest()
        {
            //OrderDao dao = new OrderDao();

            var identity = (ClaimsIdentity)User.Identity;

            var userId = identity.Claims
                       .FirstOrDefault(c => c.Type == "ID").Value;

            var accTocken = identity.Claims
                       .FirstOrDefault(c => c.Type == "AccToken").Value;

            var Roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value).ToArray();

            //var custOrderList = dao.GetOrderList(int.Parse(ID), 1);
            //return Request.CreateResponse(HttpStatusCode.OK, custOrderList);

            GetOrderListRequest ordReq = new GetOrderListRequest();
            ordReq.user_id = int.Parse(userId);
            ordReq.auth_token = accTocken;    
            ordReq.current_list = 1;

            GetOrderListResponse resp = _ordersServices.GetOrderList(ordReq);
            return Content(HttpStatusCode.OK, resp);
        }

        [BasicAuthentication]
        [MyAuthorize(Roles = "Driver")]
        [ActionName("get_order_list_driver_test")]
        //[Route("api/test")]
        public NegotiatedContentResult<GetDriverOrderListResponse> GetDriverOrderListTest()
        {
            //OrderDao dao = new OrderDao();

            var identity = (ClaimsIdentity)User.Identity;

            var userId = identity.Claims
                       .FirstOrDefault(c => c.Type == "ID").Value;

            var accTocken = identity.Claims
                       .FirstOrDefault(c => c.Type == "AccToken").Value;

            var Roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value).ToArray();

            //var custOrderList = dao.GetOrderList(int.Parse(ID), 1);
            //return Request.CreateResponse(HttpStatusCode.OK, custOrderList);

            GetDriverOrderListRequest ordReq = new GetDriverOrderListRequest();
            ordReq.user_id = int.Parse(userId);
            ordReq.auth_token = accTocken;
            ordReq.current_list = 1;

            GetDriverOrderListResponse resp = _ordersServices.GetDriverOrderList(ordReq);
            return Content(HttpStatusCode.OK, resp);
        }

        //[BasicAuthentication]
        //[MyAuthorize(Roles = "Driver")]
        //[ActionName("get_order_list_driver")]
        ////[Route("api/test")]
        //public HttpResponseMessage GetDriverOrderList()
        //{
        //    OrderDao dao = new OrderDao();

        //    var identity = (ClaimsIdentity)User.Identity;

        //    var ID = identity.Claims
        //               .FirstOrDefault(c => c.Type == "ID").Value;

        //    var dvrOrderList = dao.GetDriverOrderList(int.Parse(ID), 1);
        //    return Request.CreateResponse(HttpStatusCode.OK, dvrOrderList);
        //}
    }
}
